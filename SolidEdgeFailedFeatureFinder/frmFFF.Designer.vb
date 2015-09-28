<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFFF
    Inherits Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFFF))
        Dim MySettings1 As FailedFeatureFinderForSolidEdge.My.MySettings = New FailedFeatureFinderForSolidEdge.My.MySettings()
        Dim TreeNode1 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Assembly")
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.chkFindFailedRelationships = New System.Windows.Forms.CheckBox()
        Me.chkFindFailedFeatures = New System.Windows.Forms.CheckBox()
        Me.btnFind = New System.Windows.Forms.Button()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.Status = New System.Windows.Forms.ToolStripStatusLabel()
        Me.stateImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.btnViewLog = New System.Windows.Forms.Button()
        Me.chkTopMost = New System.Windows.Forms.CheckBox()
        Me.chkHideSE = New System.Windows.Forms.CheckBox()
        Me.btnFindNext = New System.Windows.Forms.Button()
        Me.chkInactivate = New System.Windows.Forms.CheckBox()
        Me.tabFeatures = New System.Windows.Forms.TabControl()
        Me.tabFeatures1 = New System.Windows.Forms.TabPage()
        Me.TV_FFF = New System.Windows.Forms.TreeView()
        Me.RMB_Tree = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CopyTreePath = New System.Windows.Forms.ToolStripMenuItem()
        Me.tabSimplify = New System.Windows.Forms.TabPage()
        Me.lvSimplify = New System.Windows.Forms.ListView()
        Me.hdrRank = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.hdrFacets = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.hdrCount = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.hdrFileName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.hdrFile = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.RMB_Complexety = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CopyComplexPath = New System.Windows.Forms.ToolStripMenuItem()
        Me.tabLog = New System.Windows.Forms.TabPage()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.chkGetFacetCount = New System.Windows.Forms.CheckBox()
        Me.btnLMGi = New System.Windows.Forms.Button()
        Me.StatusStrip1.SuspendLayout()
        Me.tabFeatures.SuspendLayout()
        Me.tabFeatures1.SuspendLayout()
        Me.RMB_Tree.SuspendLayout()
        Me.tabSimplify.SuspendLayout()
        Me.RMB_Complexety.SuspendLayout()
        Me.tabLog.SuspendLayout()
        Me.SuspendLayout()
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Pattern")
        Me.ImageList1.Images.SetKeyName(1, "Assembly")
        Me.ImageList1.Images.SetKeyName(2, "Dimensions")
        Me.ImageList1.Images.SetKeyName(3, "Part")
        Me.ImageList1.Images.SetKeyName(4, "warn")
        Me.ImageList1.Images.SetKeyName(5, "fail")
        Me.ImageList1.Images.SetKeyName(6, "under_constrained")
        Me.ImageList1.Images.SetKeyName(7, "conflict")
        '
        'chkFindFailedRelationships
        '
        Me.chkFindFailedRelationships.AutoSize = True
        MySettings1.FadeVal = 8
        MySettings1.Features = True
        MySettings1.HideSE = False
        MySettings1.Location = New System.Drawing.Point(0, 0)
        MySettings1.MaintainActiveState = False
        MySettings1.Relationships = True
        MySettings1.SettingsKey = ""
        MySettings1.TopMost = True
        Me.chkFindFailedRelationships.Checked = MySettings1.Relationships
        Me.chkFindFailedRelationships.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkFindFailedRelationships.DataBindings.Add(New System.Windows.Forms.Binding("Checked", MySettings1, "Relationships", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.chkFindFailedRelationships.Location = New System.Drawing.Point(12, 12)
        Me.chkFindFailedRelationships.Name = "chkFindFailedRelationships"
        Me.chkFindFailedRelationships.Size = New System.Drawing.Size(174, 21)
        Me.chkFindFailedRelationships.TabIndex = 1
        Me.chkFindFailedRelationships.Text = "Assembly relationships"
        Me.chkFindFailedRelationships.UseVisualStyleBackColor = True
        '
        'chkFindFailedFeatures
        '
        Me.chkFindFailedFeatures.AutoSize = True
        Me.chkFindFailedFeatures.Checked = MySettings1.Features
        Me.chkFindFailedFeatures.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkFindFailedFeatures.DataBindings.Add(New System.Windows.Forms.Binding("Checked", MySettings1, "Features", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.chkFindFailedFeatures.Location = New System.Drawing.Point(192, 12)
        Me.chkFindFailedFeatures.Name = "chkFindFailedFeatures"
        Me.chkFindFailedFeatures.Size = New System.Drawing.Size(124, 21)
        Me.chkFindFailedFeatures.TabIndex = 1
        Me.chkFindFailedFeatures.Text = "Model features"
        Me.chkFindFailedFeatures.UseVisualStyleBackColor = True
        '
        'btnFind
        '
        Me.btnFind.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFind.Location = New System.Drawing.Point(473, 7)
        Me.btnFind.Name = "btnFind"
        Me.btnFind.Size = New System.Drawing.Size(88, 30)
        Me.btnFind.TabIndex = 2
        Me.btnFind.Text = "Scan"
        Me.btnFind.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Status})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 430)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(657, 25)
        Me.StatusStrip1.TabIndex = 3
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'Status
        '
        Me.Status.Name = "Status"
        Me.Status.Size = New System.Drawing.Size(642, 20)
        Me.Status.Spring = True
        Me.Status.Text = "Ready"
        Me.Status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'stateImageList
        '
        Me.stateImageList.ImageStream = CType(resources.GetObject("stateImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.stateImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.stateImageList.Images.SetKeyName(0, "warn")
        Me.stateImageList.Images.SetKeyName(1, "fail")
        Me.stateImageList.Images.SetKeyName(2, "conflict")
        Me.stateImageList.Images.SetKeyName(3, "under_constrained")
        '
        'btnViewLog
        '
        Me.btnViewLog.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnViewLog.Enabled = False
        Me.btnViewLog.Location = New System.Drawing.Point(564, 389)
        Me.btnViewLog.Name = "btnViewLog"
        Me.btnViewLog.Size = New System.Drawing.Size(75, 27)
        Me.btnViewLog.TabIndex = 5
        Me.btnViewLog.Text = "View Log"
        Me.btnViewLog.UseVisualStyleBackColor = True
        '
        'chkTopMost
        '
        Me.chkTopMost.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkTopMost.AutoSize = True
        Me.chkTopMost.Checked = MySettings1.TopMost
        Me.chkTopMost.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkTopMost.DataBindings.Add(New System.Windows.Forms.Binding("Checked", MySettings1, "TopMost", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.chkTopMost.Location = New System.Drawing.Point(396, 394)
        Me.chkTopMost.Name = "chkTopMost"
        Me.chkTopMost.Size = New System.Drawing.Size(107, 21)
        Me.chkTopMost.TabIndex = 6
        Me.chkTopMost.Text = "Keep on top"
        Me.chkTopMost.UseVisualStyleBackColor = True
        '
        'chkHideSE
        '
        Me.chkHideSE.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkHideSE.AutoSize = True
        Me.chkHideSE.Checked = MySettings1.HideSE
        Me.chkHideSE.DataBindings.Add(New System.Windows.Forms.Binding("Checked", MySettings1, "HideSE", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.chkHideSE.Location = New System.Drawing.Point(16, 394)
        Me.chkHideSE.Name = "chkHideSE"
        Me.chkHideSE.Size = New System.Drawing.Size(131, 21)
        Me.chkHideSE.TabIndex = 7
        Me.chkHideSE.Text = "Hide Solid Edge"
        Me.chkHideSE.UseVisualStyleBackColor = True
        '
        'btnFindNext
        '
        Me.btnFindNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFindNext.Location = New System.Drawing.Point(567, 6)
        Me.btnFindNext.Name = "btnFindNext"
        Me.btnFindNext.Size = New System.Drawing.Size(75, 31)
        Me.btnFindNext.TabIndex = 8
        Me.btnFindNext.Text = "Go To"
        Me.btnFindNext.UseVisualStyleBackColor = True
        '
        'chkInactivate
        '
        Me.chkInactivate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkInactivate.AutoSize = True
        Me.chkInactivate.Checked = MySettings1.MaintainActiveState
        Me.chkInactivate.DataBindings.Add(New System.Windows.Forms.Binding("Checked", MySettings1, "MaintainActiveState", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.chkInactivate.Location = New System.Drawing.Point(192, 394)
        Me.chkInactivate.Name = "chkInactivate"
        Me.chkInactivate.Size = New System.Drawing.Size(182, 21)
        Me.chkInactivate.TabIndex = 9
        Me.chkInactivate.Text = "Maintain activation state"
        Me.chkInactivate.UseVisualStyleBackColor = True
        '
        'tabFeatures
        '
        Me.tabFeatures.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabFeatures.Controls.Add(Me.tabFeatures1)
        Me.tabFeatures.Controls.Add(Me.tabSimplify)
        Me.tabFeatures.Controls.Add(Me.tabLog)
        Me.tabFeatures.Location = New System.Drawing.Point(12, 39)
        Me.tabFeatures.Name = "tabFeatures"
        Me.tabFeatures.SelectedIndex = 0
        Me.tabFeatures.Size = New System.Drawing.Size(634, 344)
        Me.tabFeatures.TabIndex = 10
        '
        'tabFeatures1
        '
        Me.tabFeatures1.Controls.Add(Me.TV_FFF)
        Me.tabFeatures1.Location = New System.Drawing.Point(4, 25)
        Me.tabFeatures1.Name = "tabFeatures1"
        Me.tabFeatures1.Padding = New System.Windows.Forms.Padding(3)
        Me.tabFeatures1.Size = New System.Drawing.Size(626, 315)
        Me.tabFeatures1.TabIndex = 0
        Me.tabFeatures1.Text = "Features"
        Me.tabFeatures1.UseVisualStyleBackColor = True
        '
        'TV_FFF
        '
        Me.TV_FFF.ContextMenuStrip = Me.RMB_Tree
        Me.TV_FFF.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TV_FFF.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll
        Me.TV_FFF.ImageIndex = 0
        Me.TV_FFF.ImageList = Me.ImageList1
        Me.TV_FFF.Location = New System.Drawing.Point(3, 3)
        Me.TV_FFF.Name = "TV_FFF"
        TreeNode1.ImageIndex = 1
        TreeNode1.Name = "Assembly"
        TreeNode1.Text = "Assembly"
        Me.TV_FFF.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode1})
        Me.TV_FFF.SelectedImageIndex = 1
        Me.TV_FFF.ShowNodeToolTips = True
        Me.TV_FFF.ShowRootLines = False
        Me.TV_FFF.Size = New System.Drawing.Size(620, 309)
        Me.TV_FFF.TabIndex = 1
        '
        'RMB_Tree
        '
        Me.RMB_Tree.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.RMB_Tree.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CopyTreePath})
        Me.RMB_Tree.Name = "RMB_Tree"
        Me.RMB_Tree.Size = New System.Drawing.Size(152, 30)
        '
        'CopyTreePath
        '
        Me.CopyTreePath.Name = "CopyTreePath"
        Me.CopyTreePath.Size = New System.Drawing.Size(151, 26)
        Me.CopyTreePath.Text = "Copy Path"
        '
        'tabSimplify
        '
        Me.tabSimplify.Controls.Add(Me.lvSimplify)
        Me.tabSimplify.Location = New System.Drawing.Point(4, 25)
        Me.tabSimplify.Name = "tabSimplify"
        Me.tabSimplify.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSimplify.Size = New System.Drawing.Size(626, 315)
        Me.tabSimplify.TabIndex = 1
        Me.tabSimplify.Text = "Complexity"
        Me.tabSimplify.UseVisualStyleBackColor = True
        '
        'lvSimplify
        '
        Me.lvSimplify.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.hdrRank, Me.hdrFacets, Me.hdrCount, Me.hdrFileName, Me.hdrFile})
        Me.lvSimplify.ContextMenuStrip = Me.RMB_Complexety
        Me.lvSimplify.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvSimplify.Location = New System.Drawing.Point(3, 3)
        Me.lvSimplify.MultiSelect = False
        Me.lvSimplify.Name = "lvSimplify"
        Me.lvSimplify.Size = New System.Drawing.Size(620, 309)
        Me.lvSimplify.Sorting = System.Windows.Forms.SortOrder.Descending
        Me.lvSimplify.TabIndex = 0
        Me.lvSimplify.UseCompatibleStateImageBehavior = False
        Me.lvSimplify.View = System.Windows.Forms.View.Details
        '
        'hdrRank
        '
        Me.hdrRank.Text = "Rank"
        '
        'hdrFacets
        '
        Me.hdrFacets.Text = "Facets"
        Me.hdrFacets.Width = 85
        '
        'hdrCount
        '
        Me.hdrCount.Text = "Instances"
        Me.hdrCount.Width = 97
        '
        'hdrFileName
        '
        Me.hdrFileName.Text = "Name"
        '
        'hdrFile
        '
        Me.hdrFile.Text = "File"
        '
        'RMB_Complexety
        '
        Me.RMB_Complexety.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.RMB_Complexety.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CopyComplexPath})
        Me.RMB_Complexety.Name = "RMB"
        Me.RMB_Complexety.Size = New System.Drawing.Size(152, 30)
        '
        'CopyComplexPath
        '
        Me.CopyComplexPath.Name = "CopyComplexPath"
        Me.CopyComplexPath.Size = New System.Drawing.Size(151, 26)
        Me.CopyComplexPath.Text = "Copy Path"
        '
        'tabLog
        '
        Me.tabLog.Controls.Add(Me.txtLog)
        Me.tabLog.Location = New System.Drawing.Point(4, 25)
        Me.tabLog.Name = "tabLog"
        Me.tabLog.Padding = New System.Windows.Forms.Padding(3)
        Me.tabLog.Size = New System.Drawing.Size(626, 315)
        Me.tabLog.TabIndex = 2
        Me.tabLog.Text = "Log"
        Me.tabLog.UseVisualStyleBackColor = True
        '
        'txtLog
        '
        Me.txtLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtLog.Location = New System.Drawing.Point(3, 3)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.Size = New System.Drawing.Size(620, 309)
        Me.txtLog.TabIndex = 0
        '
        'chkGetFacetCount
        '
        Me.chkGetFacetCount.AutoSize = True
        Me.chkGetFacetCount.Location = New System.Drawing.Point(335, 12)
        Me.chkGetFacetCount.Name = "chkGetFacetCount"
        Me.chkGetFacetCount.Size = New System.Drawing.Size(127, 21)
        Me.chkGetFacetCount.TabIndex = 11
        Me.chkGetFacetCount.Text = "Get facet count"
        Me.chkGetFacetCount.UseVisualStyleBackColor = True
        '
        'btnLMGi
        '
        Me.btnLMGi.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLMGi.Image = Global.FailedFeatureFinderForSolidEdge.My.Resources.Resources.WebFormTemplate_11274_16x_color
        Me.btnLMGi.Location = New System.Drawing.Point(536, 392)
        Me.btnLMGi.Name = "btnLMGi"
        Me.btnLMGi.Size = New System.Drawing.Size(22, 23)
        Me.btnLMGi.TabIndex = 12
        Me.btnLMGi.UseVisualStyleBackColor = True
        '
        'frmFFF
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(657, 455)
        Me.Controls.Add(Me.btnLMGi)
        Me.Controls.Add(Me.chkGetFacetCount)
        Me.Controls.Add(Me.tabFeatures)
        Me.Controls.Add(Me.chkInactivate)
        Me.Controls.Add(Me.btnFindNext)
        Me.Controls.Add(Me.chkHideSE)
        Me.Controls.Add(Me.chkTopMost)
        Me.Controls.Add(Me.btnViewLog)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.btnFind)
        Me.Controls.Add(Me.chkFindFailedFeatures)
        Me.Controls.Add(Me.chkFindFailedRelationships)
        Me.DataBindings.Add(New System.Windows.Forms.Binding("Location", MySettings1, "Location", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.DoubleBuffered = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = MySettings1.Location
        Me.MinimumSize = New System.Drawing.Size(675, 500)
        Me.Name = "frmFFF"
        Me.Text = "Failed Feature Finder For Solid Edge V"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.tabFeatures.ResumeLayout(False)
        Me.tabFeatures1.ResumeLayout(False)
        Me.RMB_Tree.ResumeLayout(False)
        Me.tabSimplify.ResumeLayout(False)
        Me.RMB_Complexety.ResumeLayout(False)
        Me.tabLog.ResumeLayout(False)
        Me.tabLog.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents chkFindFailedRelationships As System.Windows.Forms.CheckBox
    Friend WithEvents chkFindFailedFeatures As System.Windows.Forms.CheckBox
    Friend WithEvents btnFind As System.Windows.Forms.Button
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents Status As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents stateImageList As System.Windows.Forms.ImageList
    Friend WithEvents btnViewLog As System.Windows.Forms.Button
    Friend WithEvents chkTopMost As System.Windows.Forms.CheckBox
    Friend WithEvents chkHideSE As System.Windows.Forms.CheckBox
    Friend WithEvents btnFindNext As System.Windows.Forms.Button
    Friend WithEvents chkInactivate As System.Windows.Forms.CheckBox
    Friend WithEvents tabFeatures As System.Windows.Forms.TabControl
    Friend WithEvents tabFeatures1 As System.Windows.Forms.TabPage
    Friend WithEvents TV_FFF As System.Windows.Forms.TreeView
    Friend WithEvents tabSimplify As System.Windows.Forms.TabPage
    Friend WithEvents lvSimplify As System.Windows.Forms.ListView
    Friend WithEvents hdrFileName As System.Windows.Forms.ColumnHeader
    Friend WithEvents hdrFile As System.Windows.Forms.ColumnHeader
    Friend WithEvents hdrRank As System.Windows.Forms.ColumnHeader
    Friend WithEvents hdrFacets As System.Windows.Forms.ColumnHeader
    Friend WithEvents hdrCount As System.Windows.Forms.ColumnHeader
    Friend WithEvents chkGetFacetCount As System.Windows.Forms.CheckBox
    Friend WithEvents tabLog As System.Windows.Forms.TabPage
    Friend WithEvents txtLog As System.Windows.Forms.TextBox
    Friend WithEvents RMB_Complexety As ContextMenuStrip
    Friend WithEvents CopyComplexPath As ToolStripMenuItem
    Friend WithEvents RMB_Tree As ContextMenuStrip
    Friend WithEvents CopyTreePath As ToolStripMenuItem
    Friend WithEvents btnLMGi As Button
End Class
