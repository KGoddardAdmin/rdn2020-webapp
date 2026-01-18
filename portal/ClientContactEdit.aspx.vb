Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.VisualBasic
Imports System.Security.Cryptography

Partial Class ClientContactEdit
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GetContacts()
        End If
    End Sub

    Private Sub GetContacts()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetContacts As New SqlCommand("ClientContact_GetAllActive", cnn)
                cmdGetContacts.CommandType = CommandType.StoredProcedure
                Using dtrGetContacts = cmdGetContacts.ExecuteReader
                    ddContact.DataSource = dtrGetContacts
                    ddContact.DataBind()
                End Using
            End Using
        End Using
        ddContact.Items.Insert(0, "Select Contact")
    End Sub
    Protected Sub ddContact_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddContact.SelectedIndexChanged
        hyplPassword.NavigateUrl = "ClientContactEditPassword.aspx?c=" & ddContact.SelectedValue
    End Sub
End Class
