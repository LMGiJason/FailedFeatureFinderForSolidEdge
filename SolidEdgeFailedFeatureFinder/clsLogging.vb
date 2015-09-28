Imports System.IO
Public Class clsLogging
    Public Enabled As Boolean = True
    Public ShowMsgBox As Boolean = False
    Public InfoLogFile As String
    Public ErrorLogFile As String
    Public HasError As Boolean = False
    Public Event OnError(ByVal msg As String)
    Public Event OnInfo(ByVal msg As String)
    Public VerboseLogging As Boolean = True
    Public Sub ClearLog()
        Try
            Dim logFiles() As String = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.Log.txt")
            If logFiles.Length > 0 Then
                'File.Delete(logFiles(0))
            End If
        Catch
        End Try
    End Sub

    Public Sub LogError(ByVal msg As String, Optional ByVal showMsgBox As Boolean = False, Optional ByVal stackTrace As String = "")
        Dim logger As StreamWriter = Nothing
        Dim fs As FileStream
        If Not Enabled Then Return
        Me.ShowMsgBox = showMsgBox
        Try
            HasError = True
            fs = New FileStream(InfoLogFile, FileMode.Create Or FileMode.Append, FileAccess.Write, FileShare.Write)
            logger = New StreamWriter(fs)
            logger.WriteLine(msg & Environment.NewLine & StackTrace)
            If showMsgBox Then MsgBox(msg, MsgBoxStyle.Critical)
            RaiseEvent OnError(msg & Environment.NewLine)
        Finally
            If Not (logger Is Nothing) Then
                logger.Close()
                logger = Nothing
                fs = Nothing
            End If
        End Try
    End Sub

    Public Sub LogInfo(ByVal msg As String)
        Dim logger As StreamWriter = Nothing
        Dim fs As FileStream
        If Not Enabled Then Return
        Try
            If VerboseLogging Then
                fs = New FileStream(InfoLogFile, FileMode.Create Or FileMode.Append, FileAccess.Write, FileShare.Write)
                logger = New StreamWriter(fs)
                logger.WriteLine(msg & Environment.NewLine)
                RaiseEvent OnInfo(msg & Environment.NewLine)
            End If
        Finally
            If Not (logger Is Nothing) Then
                logger.Close()
                logger = Nothing
                fs = Nothing
            End If
        End Try
    End Sub

    Public Sub New()
        Try
            Dim mLogsDir As String = Directory.GetCurrentDirectory() & "\Logs\"
            If Not Directory.Exists(mLogsDir) Then
                Directory.CreateDirectory(mLogsDir)
            End If
            Dim FileName As String = DateTime.Now.Month.ToString() & "_" & _
                           DateTime.Now.Day.ToString() & "_" & _
                           DateTime.Now.Year.ToString() & "_" & _
                           DateTime.Now.Hour.ToString() & "_" & _
                           DateTime.Now.Minute.ToString() & "_" & _
                           DateTime.Now.Second.ToString() & "_"
            InfoLogFile = mLogsDir & FileName & "InfoLog.txt"
            ErrorLogFile = mLogsDir & FileName & "ErrorLog.txt"
        Catch
        End Try
    End Sub
End Class
