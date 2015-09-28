Public Class clsSimplify
    Public FacetCount As Integer
    Public mHitCount As Integer
    Public Name As String
    Public FileName As String
    Public Rank As Integer
    Public Property HitCount() As Integer
        Get
            Return mHitCount
        End Get
        Set(ByVal value As Integer)
            mHitCount = value
            Rank += FacetCount
        End Set
    End Property
    Public Sub New(ByVal n As String, ByVal fname As String)
        Name = n
        FileName = fname
    End Sub
End Class
