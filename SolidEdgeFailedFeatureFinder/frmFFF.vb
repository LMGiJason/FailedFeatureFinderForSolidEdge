Imports System.Collections
Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.IO
Imports SolidEdgeFramework
Imports System.Diagnostics
Imports System.Environment
Public Class frmFFF
    Private WithEvents Log As New clsLogging
    Private mSolidApp As SolidEdgeFramework.Application = Nothing
    Private mAsmDoc As SolidEdgeAssembly.AssemblyDocument
    Private mSheetmetalDoc As SolidEdgePart.SheetMetalDocument = Nothing
    Private mOcc As SolidEdgeAssembly.Occurrence = Nothing
    Private mAsmRelations As SolidEdgeAssembly.Relations3d
    Private mPartDoc As Object = Nothing
    Private mModel As SolidEdgePart.Model = Nothing
    Private mModels As SolidEdgePart.Models = Nothing
    Private mSketch As SolidEdgePart.Sketch = Nothing
    Private mSketches As SolidEdgePart.Sketchs = Nothing
    Private mPlanes As SolidEdgePart.RefPlanes
    Private mPlane As SolidEdgePart.RefPlane
    Private mPMI As SolidEdgeFrameworkSupport.PMI
    Private mPMIDims As SolidEdgeFrameworkSupport.Dimensions
    Private mPMIDim As SolidEdgeFrameworkSupport.Dimension
    Private mAsmFeatures As SolidEdgeAssembly.AssemblyFeatures

    Private mFeatures As SolidEdgePart.Features
    Private mFeature As Object
    Private mBody As SolidEdgeGeometry.Body

    Private mProcessing As Boolean = False
    Private mEdgeWasRunning As Boolean = False
    Private mCancel As Boolean = False
    Private mScanedParts As New Specialized.StringCollection
    Private mBadNodes As New List(Of TreeNode)
    Private mPartFacets As New Generic.Dictionary(Of String, clsSimplify)

    Private Sub frmFFF_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Text &= My.Application.Info.Version.ToString
    End Sub

    Private Class NodeStatus
        Sub New(ByVal name As String)
            FileName = name
        End Sub
        Public FileName As String
        Public Conflict As Boolean = False
        Public Warn As Boolean = False
        Public Failed As Boolean = False
        Public UnderConstrained As Boolean = False
        Public ToolTipText As String
    End Class

    Private Sub btnFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFind.Click
        txtLog.Clear()
        If GetEdge() Then
            Try
                If mAsmDoc.Dirty Then
                    If MsgBox("Save Solid Edge file first?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                        Cursor = Cursors.WaitCursor
                        mAsmDoc.Save()
                        Cursor = Cursors.Default
                    End If
                End If
                TV_FFF.BeginUpdate()
                TV_FFF.Nodes.Clear()
                mBadNodes.Clear()
                mPartFacets.Clear()

                Me.btnFind.Enabled = False
                Me.btnViewLog.Enabled = False
                Me.btnFindNext.Enabled = False
                TV_FFF.Enabled = False
                Dim nd As TreeNode = TV_FFF.Nodes.Add(mAsmDoc.Name)
                nd.Tag = New NodeStatus(mAsmDoc.FullName)
                nd.ImageKey = "Assembly"
                nd.SelectedImageKey = nd.ImageKey
                mCancel = False
                Status.Text = "Counting components..."
                Dim partcount As Integer = 0
                Cursor = Cursors.WaitCursor
                mScanedParts.Clear()
                mSolidApp.DelayCompute = True
#If Not Debug Then
                mSolidApp.Interactive = False
#End If
                mSolidApp.Visible = Not chkHideSE.Checked

                'Register the IOleMessageFilter to handle any threading errors.
                MessageFilter.Register()

                ScanParts(mAsmDoc.Occurrences, nd)

                Me.lvSimplify.Items.Clear()
                Dim lvi As ListViewItem
                lvSimplify.BeginUpdate()

                For Each p As clsSimplify In mPartFacets.Values
                    lvi = Me.lvSimplify.Items.Add(p.Rank.ToString)
                    lvi.SubItems.Add(p.FacetCount.ToString)
                    lvi.SubItems.Add(p.HitCount.ToString)
                    lvi.SubItems.Add(p.Name)
                    lvi.SubItems.Add(p.FileName)
                Next
                lvSimplify.Sort()
                lvSimplify.EndUpdate()

            Catch ex As Exception
                Status.Text = ex.Message
            Finally
                'Turn off the IOleMessageFilter.
                MessageFilter.Revoke()
                Cursor = Cursors.Default
                Me.btnViewLog.Enabled = IO.File.Exists(Log.InfoLogFile)
                'mSolidApp.DelayCompute = False
                mSolidApp.Interactive = True
                mSolidApp.Visible = True
                If mCancel Then
                    Status.Text = "Canceled " & mBadNodes.Count.ToString()
                Else
                    Status.Text = "Finished scan " & mBadNodes.Count.ToString()
                End If
                Me.btnFind.Enabled = True
                Me.btnViewLog.Enabled = IO.File.Exists(Log.InfoLogFile)
                Me.btnFindNext.Enabled = mBadNodes.Count > 0
                TV_FFF.Enabled = True
                TV_FFF.Nodes(0).Expand()
                TV_FFF.EndUpdate()
            End Try

        End If
        Cleanup()
    End Sub

    Private Sub ScanParts(ByRef Occs As SolidEdgeAssembly.Occurrences, ByVal nd As TreeNode)
        Try
            mSolidApp.Visible = Not chkHideSE.Checked
            ' Loop through the occurrences
            For i As Integer = 1 To Occs.Count
                mOcc = Occs.Item(i)
                Status.Text = "Reading " & mOcc.Name
                StatusStrip1.Refresh()
                ' Check to see if this occurrence has suboccurrences
                If Not mOcc.IsPatternItem Then
                    Dim curnode As TreeNode = nd.Nodes.Add(mOcc.Name)
                    curnode.Tag = New NodeStatus(mOcc.OccurrenceFileName)
                    My.Application.DoEvents()
                    If mCancel Then Exit Sub
                    If chkFindFailedRelationships.Checked Then
                        Dim hasBadRel As Boolean = False

                        Try
                            Dim mRelation As Object = Nothing
                            mAsmRelations = mOcc.Relations3d
                            For relIdx As Integer = 1 To mAsmRelations.Count
                                mRelation = mAsmRelations.Item(relIdx)
                                If mRelation.Status = 0 Then
                                    hasBadRel = True
                                    Exit For
                                End If
                                ReleaseRCW(mRelation)
                            Next
                            ReleaseRCW(mAsmRelations)
                        Catch ex As Exception
                            Debug.Print(ex.Message)
                        End Try
                        If hasBadRel Then
                            curnode.ToolTipText = "Failed Assembly Relationship"
                            curnode.ForeColor = Color.Red
                            curnode.EnsureVisible()
                            CType(curnode.Tag, NodeStatus).Conflict = True
                            CType(curnode.Tag, NodeStatus).ToolTipText = "Failed Assembly Relationship"
                            Log.LogInfo("COMPONENT , " & mOcc.OccurrenceFileName & " , " & mOcc.Name & " , " & curnode.ToolTipText)
                        End If
                    End If

                    If mOcc.Subassembly Then
                        curnode.ImageKey = "Assembly"
                        curnode.SelectedImageKey = curnode.ImageKey
                        CheckForFailedAssemblyPMI(CType(mOcc.OccurrenceDocument, SolidEdgeAssembly.AssemblyDocument), curnode)
                        CheckForFailedAssemblyFeatures(CType(mOcc.OccurrenceDocument, SolidEdgeAssembly.AssemblyDocument), curnode)
                        ScanParts(CType(mOcc.OccurrenceDocument, SolidEdgeAssembly.AssemblyDocument).Occurrences, curnode)
                    Else
                        curnode.ImageKey = "Part"
                        curnode.SelectedImageKey = curnode.ImageKey

                        If chkFindFailedFeatures.Checked Or chkGetFacetCount.Checked Then
                            If Not mScanedParts.Contains(mOcc.OccurrenceFileName) Then
                                CheckModel(curnode)
                            End If
                            IncrementSimplePart(mOcc.OccurrenceFileName)
                            mScanedParts.Add(mOcc.OccurrenceFileName)
                        End If
                    End If

                    Debug.Assert(curnode.ImageKey = "Part" Or curnode.ImageKey = "Assembly", "Error")

                    Dim stat As NodeStatus = CType(curnode.Tag, NodeStatus)
                    If stat Is Nothing Then Return
                    If stat.Failed Or stat.Conflict Then
                        mBadNodes.Add(curnode)
                        TV_FFF.EndUpdate()
                        nd.TreeView.Refresh()
                        TV_FFF.BeginUpdate()
                    End If
                End If
                ReleaseRCW(mOcc)
            Next
        Catch ex As Exception
            Log.LogError(ex.Message, False, ex.StackTrace)
            Status.Text = ex.Message
        Finally
            ReleaseRCW(mOcc)
        End Try
    End Sub

    Private Sub AppendErrorNode(nd As TreeNode, msg As String, stat As Integer)
        'nd.ForeColor = Color.Red
        nd.EnsureVisible()
        Dim ndStat As NodeStatus = CType(nd.Tag, NodeStatus)
        If String.IsNullOrEmpty(ndStat.ToolTipText) Then
            ndStat.ToolTipText = msg
        Else
            ndStat.ToolTipText &= vbCr & msg
        End If
        nd.ToolTipText = ndStat.ToolTipText

        Dim ernd As TreeNode = nd.Nodes.Add(Guid.NewGuid.ToString, msg, "alert")
        Select Case stat
            Case SolidEdgePart.FeatureStatusConstants.igFeatureFailed
                ernd.ImageKey = "fail"
                ndStat.Failed = True
            Case SolidEdgePart.FeatureStatusConstants.igFeatureWarned
                ernd.ImageKey = "warn"
                ndStat.Warn = True
            Case 33
                ernd.ImageKey = "under_constrained"
                ndStat.UnderConstrained = True
        End Select
        ernd.SelectedImageKey = ernd.ImageKey
    End Sub

    Private Sub CheckForFailedAssemblyPMI(ByVal asm As SolidEdgeAssembly.AssemblyDocument, ByVal nd As TreeNode)
        Dim ct As Integer
        Dim asmName As String = "UNKNOWN"
        Try
            ct = CType(asm.PMI.Dimensions, SolidEdgeFrameworkSupport.Dimensions).Count
            Dim dimen As SolidEdgeFrameworkSupport.Dimension
            For d As Integer = 1 To ct
                dimen = CType(asm.PMI.Dimensions, SolidEdgeFrameworkSupport.Dimensions).Item(d)
                Status.Text = "Checking " & dimen.Name
                StatusStrip1.Refresh()
                If dimen.StatusOfDimension = SolidEdgeFrameworkSupport.DimStatusConstants.seDimStatusError Then
                    Dim featureStatusMsg As String = "Failed PMI Dimension"
                    AppendErrorNode(nd, dimen.Name & " - " & featureStatusMsg, SolidEdgePart.FeatureStatusConstants.igFeatureFailed)
                    Log.LogInfo("PMI DIM , " & asmName & " , " & dimen.Name & " , " & featureStatusMsg)
                End If

                If dimen IsNot Nothing Then
                    Marshal.ReleaseComObject(dimen)
                End If
            Next
        Catch ex As Exception
            Log.LogError("ERROR , " & asmName & " , " & ex.Message, False, ex.StackTrace)
        End Try
    End Sub

    Private Sub CheckForFailedAssemblyFeatures(ByVal asm As SolidEdgeAssembly.AssemblyDocument, ByVal nd As TreeNode)
        Dim asmName As String = "UNKNOWN"
        Dim featureStatus As SolidEdgePart.FeatureStatusConstants = Nothing
        Dim featureStatusMsg As String = String.Empty
        Try
            mAsmFeatures = asm.AssemblyFeatures
            'this will take some work

            For featureIndex As Integer = 1 To mAsmFeatures.FilletWelds.Count
                mFeature = mAsmFeatures.FilletWelds.Item(featureIndex)
                Dim featureName As String = "????????"
                featureName = mFeature.Name
                Status.Text = "Checking " & featureName
                StatusStrip1.Refresh()

                GetFeatureStatus(mFeature, featureStatusMsg, featureStatus)

                If featureStatus = SolidEdgePart.FeatureStatusConstants.igFeatureFailed _
                Or featureStatus = SolidEdgePart.FeatureStatusConstants.igFeatureWarned Then
                    AppendErrorNode(nd, featureName & " - " & featureStatusMsg, featureStatus)
                    Log.LogInfo("FEATURE , " & asm.FullName & " , " & featureName & " , " & featureStatusMsg)
                End If
                ReleaseRCW(mFeature)
            Next
            ReleaseRCW(mFeature)

        Catch ex As Exception
            Log.LogError("ERROR , " & asmName & " , " & ex.Message, False, ex.StackTrace)
        End Try
    End Sub

    Private Sub CheckModel(ByVal nd As TreeNode)
        Dim featureStatus As SolidEdgePart.FeatureStatusConstants = Nothing
        Dim featureStatusMsg As String = String.Empty
        Dim tipText As String = ""
        Dim wasActive As Boolean = mOcc.Activate
        Dim partName As String = "UNKNOWN"
        Try
            mPartDoc = mOcc.OccurrenceDocument
            If mPartDoc.type <> 1 Then
                Return
            End If
            partName = mPartDoc.FullName
            Debug.Print(partName)
            If Me.chkGetFacetCount.Checked Then
                Try
                    mModels = mPartDoc.Models
                    For r As Integer = 1 To mModels.Count
                        mModel = mModels.Item(r)
                        If mPartDoc.SimplifiedModels.Count = 0 Then
                            If mModel IsNot Nothing Then
                                Try
                                    mBody = mModel.Body
                                    Status.Text = "Getting facet count " & mPartDoc.Name
                                    Me.Refresh()
                                    AddOrUpdateFacetInfo(mBody.FacetCount(1.0), mPartDoc.Name, mPartDoc.FullName)
                                Catch ex As Exception
                                    'Log.LogError("ERROR , " & partName & " , " & ex.Message, False, ex.StackTrace)
                                Finally
                                    ReleaseRCW(mBody)
                                End Try
                            End If
                        End If
                        ReleaseRCW(mModel)
                    Next
                    ReleaseRCW(mModels)
                Catch ex As Exception
                    Log.LogError("ERROR , " & partName & " , " & ex.Message, False, ex.StackTrace)
                End Try
            End If

            If chkFindFailedFeatures.Checked Then
                Try
                    mPMI = mPartDoc.PMI
                    mPMIDims = mPMI.Dimensions
                    For d As Integer = 1 To mPMIDims.Count
                        mPMIDim = mPMIDims.Item(d)
                        Dim dimName As String = mPMIDim.Name
                        Status.Text = "Checking " & dimName
                        StatusStrip1.Refresh()

                        If mPMIDim.Constraint Then
                            If mPMIDim.StatusOfDimension = SolidEdgeFrameworkSupport.DimStatusConstants.seDimStatusError Then
                                featureStatusMsg = "Failed PMI Dimension"
                                AppendErrorNode(nd, dimName & " - " & featureStatusMsg, SolidEdgePart.FeatureStatusConstants.igFeatureFailed)
                                Log.LogInfo("PMI DIM , " & partName & " , " & dimName & " , " & featureStatusMsg)
                            End If

                        End If
                        ReleaseRCW(mPMIDim)
                    Next
                    ReleaseRCW(mPMIDims)
                    ReleaseRCW(mPMI)
                Catch ex As Exception
                    Log.LogError("ERROR , " & partName & " , " & ex.Message, False, ex.StackTrace)
                End Try

                Try
                    mSketches = mPartDoc.Sketches
                    If mSketches IsNot Nothing Then
                        For sketchIndex As Integer = 1 To mSketches.Count
                            mSketch = mSketches.Item(sketchIndex)
                            Dim sketchName As String = mSketch.EdgebarName
                            Status.Text = "Checking " & sketchName
                            StatusStrip1.Refresh()
                            GetFeatureStatus(mSketch, featureStatusMsg, featureStatus)
                            If featureStatus = SolidEdgePart.FeatureStatusConstants.igFeatureFailed _
                            Or featureStatus = SolidEdgePart.FeatureStatusConstants.igFeatureWarned Then
                                AppendErrorNode(nd, sketchName & " - " & featureStatusMsg, featureStatus)
                                Log.LogInfo("SKETCH , " & mPartDoc.FullName & " , " & sketchName & " , " & featureStatusMsg.Trim)
                            Else
                                If mSketch.IsUnderDefined Then
                                    AppendErrorNode(nd, sketchName & " - under-constrained sketch", 33)
                                    Log.LogInfo("SKETCH , " & mPartDoc.FullName & " , " & sketchName & " , " & featureStatusMsg.Trim)
                                End If
                            End If
                            ReleaseRCW(mSketch)
                        Next
                        ReleaseRCW(mSketches)
                    End If

                Catch ex As Exception
                    Log.LogError("ERROR , " & partName & " , " & ex.Message, False, ex.StackTrace)
                End Try

                Try
                    mModels = mPartDoc.Models
                    For modelIndex As Integer = 1 To mModels.Count
                        mModel = mModels.Item(modelIndex)
                        If mModel IsNot Nothing Then
                            mFeatures = mModel.Features
                            For featureIndex As Integer = 1 To mFeatures.Count
                                mFeature = mFeatures.Item(featureIndex)
                                Dim featureName As String = "????????"
                                featureName = mFeature.EdgebarName
                                Status.Text = "Checking " & featureName
                                StatusStrip1.Refresh()

                                GetFeatureStatus(mFeature, featureStatusMsg, featureStatus)

                                If featureStatus = SolidEdgePart.FeatureStatusConstants.igFeatureFailed _
                                Or featureStatus = SolidEdgePart.FeatureStatusConstants.igFeatureWarned Then
                                    AppendErrorNode(nd, featureName & " - " & featureStatusMsg, featureStatus)
                                    Log.LogInfo("FEATURE , " & mPartDoc.FullName & " , " & featureName & " , " & featureStatusMsg)
                                Else
                                    Try
                                        'TODO fix this
                                        If mFeature.profile.parent.IsUnderDefined Then
                                            AppendErrorNode(nd, featureName & " - under-constrained feature", 33)
                                            Log.LogInfo("FEATURE , " & mPartDoc.FullName & " , " & featureName & " , " & featureStatusMsg)
                                        End If
                                    Catch
                                    End Try
                                End If
                                ReleaseRCW(mFeature)
                            Next
                            ReleaseRCW(mFeature)
                        End If
                        ReleaseRCW(mModel)
                    Next
                    ReleaseRCW(mModels)
                Catch ex As Exception
                    Log.LogError("ERROR , " & partName & " , " & ex.Message, False, ex.StackTrace)
                End Try

                Try
                    mPlanes = mPartDoc.RefPlanes
                    For plnIndex As Integer = 1 To mPlanes.Count
                        mPlane = mPlanes.Item(plnIndex)
                        Dim planeName As String = mPlane.Name
                        If planeName IsNot Nothing Then
                            planeName = mPlane.EdgebarName
                            Status.Text = "Checking " & planeName
                            StatusStrip1.Refresh()
                            GetFeatureStatus(mPlane, featureStatusMsg, featureStatus)
                            If featureStatus = SolidEdgePart.FeatureStatusConstants.igFeatureFailed _
                            Or featureStatus = SolidEdgePart.FeatureStatusConstants.igFeatureWarned Then
                                AppendErrorNode(nd, planeName & " - " & featureStatusMsg, featureStatus)
                                Log.LogInfo("PLANE , " & mPartDoc.FullName & " , " & planeName & " , " & featureStatusMsg)
                            End If
                        End If

                        ReleaseRCW(mPlane)
                    Next
                    ReleaseRCW(mPlanes)
                Catch ex As Exception
                    Log.LogError("ERROR , " & partName & " , " & ex.Message, False, ex.StackTrace)
                End Try

            End If
            ReleaseRCW(mPartDoc)

            If Not wasActive And chkInactivate.Checked Then
                mOcc.Activate = wasActive
            End If
        Catch ex As Exception
            Log.LogError(ex.Message, False, ex.StackTrace)
            Status.Text = ex.Message
        Finally
            ReleaseRCW(mPlanes)
            ReleaseRCW(mPlane)
            ReleaseRCW(mSketches)
            ReleaseRCW(mSketch)
            ReleaseRCW(mFeature)
            ReleaseRCW(mModels)
            ReleaseRCW(mModel)
            ReleaseRCW(mPartDoc)
        End Try

    End Sub

    Private Sub GetFeatureStatus(o As Object, ByRef strMessage As String, ByRef status As SolidEdgePart.FeatureStatusConstants)
        Dim PINNED_FeatureStatusMsg As New String(" "c, 255)
        Dim hGC As GCHandle = GCHandle.Alloc(PINNED_FeatureStatusMsg, GCHandleType.Pinned)
        Try
            status = o.Status(PINNED_FeatureStatusMsg)
            strMessage = PINNED_FeatureStatusMsg.Trim
        Catch
            Try
                status = o.GetStatusEx(PINNED_FeatureStatusMsg)
                strMessage = PINNED_FeatureStatusMsg.Trim
            Catch 'fail

            End Try
        Finally
            hGC.Free()
        End Try

    End Sub

    Private Sub AddOrUpdateFacetInfo(ByVal facetCount As Integer, ByVal name As String, ByVal fullName As String)
        Dim par As clsSimplify = Nothing
        If Not mPartFacets.TryGetValue(fullName, par) Then
            par = New clsSimplify(name.Substring(0, name.IndexOf("."c)), fullName)
            mPartFacets.Add(fullName, par)
            par.FacetCount = facetCount
            par.HitCount = 0
        End If
    End Sub

    Private Sub IncrementSimplePart(ByVal fullName As String)
        Dim par As clsSimplify = Nothing
        If mPartFacets.TryGetValue(fullName, par) Then
            par.HitCount += 1
        End If
    End Sub

    Private Function GetEdge() As Boolean
        Dim appType As Type = Nothing
        Try
            If EdgeIsRunning() Then
                mSolidApp = TryCast(Marshal.GetActiveObject("SolidEdge.Application"), SolidEdgeFramework.Application)
                mEdgeWasRunning = True
                Status.Text = "SolidEdge running"
            Else
                Status.Text = "Solid Edge must be running"
                Return False
                mEdgeWasRunning = False
            End If

            If mSolidApp.ActiveDocumentType = DocumentTypeConstants.igAssemblyDocument _
            Or mSolidApp.ActiveDocumentType = DocumentTypeConstants.igSyncAssemblyDocument Then
                mAsmDoc = CType(mSolidApp.ActiveDocument, SolidEdgeAssembly.AssemblyDocument)
            Else
                Status.Text = "Requires assembly document"
                Return False
            End If

            Return True
        Catch ex As Exception
            Log.LogError("Error in " & MethodBase.GetCurrentMethod.Name & " Error:" & ex.Message, , ex.StackTrace)
            Status.Text = ex.Message
            Return False
        Finally
            Cursor = Cursors.Default
        End Try
    End Function

    Private Function EdgeIsRunning() As Boolean
        Try
            For Each p As Process In Process.GetProcesses(System.Environment.MachineName)
                If p.ProcessName = "Edge" And p.Responding = True Then
                    Return True
                End If
            Next p
        Catch ex As Exception
            Log.LogError("Error in " & MethodBase.GetCurrentMethod.Name & " Error:" & ex.Message, , ex.StackTrace)
        End Try
        Return False
    End Function

    Private Sub Cleanup()
        Try
            ReleaseRCW(mPartDoc)
            ReleaseRCW(mAsmDoc)
            If Not (mSolidApp Is Nothing) Then
                mSolidApp.DelayCompute = False
                mSolidApp.Interactive = True
                mSolidApp.Visible = True
                ReleaseRCW(mSolidApp)
            End If
        Catch ex As Exception
            Log.LogError("Error in " & MethodBase.GetCurrentMethod.Name & " Error:" & ex.Message, , ex.StackTrace)
            Status.Text = "Error"
        End Try

    End Sub

    Private Sub frmFFF_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Cleanup()
    End Sub

    Private Sub TV_FFF_DrawNode(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DrawTreeNodeEventArgs) Handles TV_FFF.DrawNode
        Dim pt As New Point(0, 0)
        Dim stat As NodeStatus = Nothing
        Try
            e.DrawDefault = True
            stat = CType(e.Node.Tag, NodeStatus)
            If stat Is Nothing Then Return
            pt.X = e.Node.Bounds.Right
            pt.Y = e.Node.Bounds.Top
            If stat.Conflict Then
                e.Graphics.DrawImage(Me.stateImageList.Images("conflict"), pt.X, pt.Y)
                pt.X += Me.stateImageList.Images("conflict").Width
            End If
            If stat.Failed Then
                e.Graphics.DrawImage(Me.stateImageList.Images("fail"), pt.X, pt.Y)
                pt.X += Me.stateImageList.Images("fail").Width
            End If
            If stat.Warn Then
                e.Graphics.DrawImage(Me.stateImageList.Images("warn"), pt.X, pt.Y)
                pt.X += Me.stateImageList.Images("warn").Width
            End If
            If stat.UnderConstrained Then
                e.Graphics.DrawImage(Me.stateImageList.Images("under_constrained"), pt.X, pt.Y)
            End If
        Catch ex As Exception
            'Status.Text = ex.Message
        End Try

    End Sub

    Private Sub btnViewLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewLog.Click
        Try
            Interaction.Shell("Notepad.exe " + Log.InfoLogFile, AppWinStyle.NormalFocus, False, 500)
        Catch ex As Exception
            Status.Text = ex.Message
        End Try
    End Sub

    Private Sub chkTopMost_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkTopMost.CheckedChanged
        Me.TopMost = chkTopMost.Checked
    End Sub

    Private Sub frmFFF_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            mCancel = True
        End If
    End Sub

    Private Sub btnFindNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNext.Click
        Static idx As Integer = 0
        If mBadNodes.Count > 0 Then
            If idx > mBadNodes.Count - 1 Then
                idx = 0
            End If
            Dim theNode As TreeNode = mBadNodes(idx)
            theNode.EnsureVisible()
            idx += 1
        End If
    End Sub

    Private Sub TV_FFF_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs)
        Dim stat As NodeStatus = CType(e.Node.Tag, NodeStatus)
        My.Computer.Clipboard.SetText(stat.FileName)
    End Sub

    Private Sub lvSimplify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvSimplify.Click
        Debug.Print(lvSimplify.SelectedItems(0).SubItems(2).Text)
    End Sub


    ' The column currently used for sorting.
    Private m_SortingColumn As ColumnHeader

    Private Sub lvSimplify_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvSimplify.ColumnClick
        ' Get the new sorting column.
        Dim new_sorting_column As ColumnHeader = _
            lvSimplify.Columns(e.Column)

        ' Figure out the new sorting order.
        Dim sort_order As System.Windows.Forms.SortOrder
        If m_SortingColumn Is Nothing Then
            ' New column. Sort ascending.
            sort_order = SortOrder.Ascending
        Else
            ' See if this is the same column.
            If new_sorting_column.Equals(m_SortingColumn) Then
                ' Same column. Switch the sort order.
                If m_SortingColumn.Text.StartsWith("> ") Then
                    sort_order = SortOrder.Descending
                Else
                    sort_order = SortOrder.Ascending
                End If
            Else
                ' New column. Sort ascending.
                sort_order = SortOrder.Ascending
            End If

            ' Remove the old sort indicator.
            m_SortingColumn.Text = _
                m_SortingColumn.Text.Substring(2)
        End If

        ' Display the new sort order.
        m_SortingColumn = new_sorting_column
        If sort_order = SortOrder.Ascending Then
            m_SortingColumn.Text = "> " & m_SortingColumn.Text
        Else
            m_SortingColumn.Text = "< " & m_SortingColumn.Text
        End If

        ' Create a comparer.
        lvSimplify.ListViewItemSorter = New  _
            ListViewComparer(e.Column, sort_order)

        ' Sort.
        lvSimplify.Sort()

    End Sub

    Private Sub lvSimplify_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lvSimplify.MouseClick
        Try
            If lvSimplify.SelectedItems.Count > 0 Then
                My.Computer.Clipboard.SetText(lvSimplify.SelectedItems(0).SubItems(4).Text)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ReleaseRCW(ByRef o As Object)
        If o IsNot Nothing Then
            Dim refCount As Integer = Marshal.ReleaseComObject(o)
            o = Nothing
            Debug.Assert(refCount = 0)
        End If
    End Sub

    Private Sub Log_OnError(msg As String) Handles Log.OnError
        txtLog.AppendText(NewLine & msg)
    End Sub

    Private Sub Log_OnInfo(msg As String) Handles Log.OnInfo
        txtLog.AppendText(NewLine & msg)
    End Sub

    Private Sub TV_FFF_NodeMouseClick1(sender As Object, e As TreeNodeMouseClickEventArgs) Handles TV_FFF.NodeMouseClick
        If e.Node.Tag IsNot Nothing Then
            'mnuSelect.Enabled = CType(e.Node.Tag, NodeStatus).Failed
        End If
    End Sub

    Private Sub CopyPath_Click(sender As Object, e As EventArgs) Handles CopyTreePath.Click
        If TV_FFF.SelectedNode IsNot Nothing Then
            Dim nd As NodeStatus = TV_FFF.SelectedNode.Tag
            Clipboard.SetText(nd.FileName)
        End If
    End Sub

    Private Sub CopyComplexPath_Click(sender As Object, e As EventArgs) Handles CopyComplexPath.Click
        If lvSimplify.SelectedItems.Count = 1 Then
            Dim fullName As String = lvSimplify.SelectedItems(0).SubItems(4).Text
            Clipboard.SetText(fullName)
        End If
    End Sub

    Private Sub RMB_Complexety_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles RMB_Complexety.Opening
        e.Cancel = lvSimplify.SelectedItems.Count <> 1
    End Sub

    Private Sub RMB_Tree_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles RMB_Tree.Opening
        e.Cancel = TV_FFF.SelectedNode Is Nothing
    End Sub

    Private Sub btnLMGi_Click(sender As Object, e As EventArgs) Handles btnLMGi.Click
        Try
            Process.Start("www.tlmgi.com")
        Catch
        End Try

    End Sub
End Class
