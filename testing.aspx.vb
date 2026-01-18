
Partial Class testing
    Inherits System.Web.UI.Page
    Public value As String
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        value = Request.ServerVariables("HTTP_REFERER")
        Label1.Text = value
    End Sub
End Class
