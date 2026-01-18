Imports System.Web.Security

Public Class Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not IsPostBack Then
            ' Add any initialization code here
        End If
    End Sub

    Protected Sub LogIn(sender As Object, e As EventArgs)
        Dim userName As String = UserName.Text
        Dim password As String = Password.Text

        ' TODO: Implement proper authentication logic here
        ' This is just a basic example
        If userName = "admin" AndAlso password = "password" Then
            FormsAuthentication.SetAuthCookie(userName, False)
            Response.Redirect("~/")
        Else
            ErrorMessage.Visible = True
            FailureText.Text = "Invalid user name or password."
        End If
    End Sub
End Class 