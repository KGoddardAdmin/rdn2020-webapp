Imports lga4040

Namespace portal
    Partial Class Admin
        Inherits System.Web.UI.MasterPage

        Public LoggedIn As Boolean
        Public LoggedOnUser As String
        Public UserClearance As String
        Private InValidLoginRedirectURL As String = ConfigurationSettings.AppSettings("InvalidLoginURL")

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            SessionCheck()
        End Sub

        Private Sub SessionCheck()
            If Session("AcceptedUser") IsNot Nothing Then
                LoggedIn = True
                Dim arruser() As String = New String() {}
                arruser = Session("AcceptedUser").Split("~")
                LoggedOnUser = arruser(0)
                LoggedOnUser &= " " & arruser(1)
            Else
                LoggedIn = False
                LoginFailure()
            End If
        End Sub
        Private Sub LoginFailure()
            Response.Redirect("Login.aspx")
        End Sub

    End Class
End Namespace