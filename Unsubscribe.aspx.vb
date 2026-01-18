Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Web
Imports System.Web.Caching
Imports System.Web.Configuration
Imports System.Web.Profile
Imports System.Web.Security
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports Microsoft.VisualBasic

Partial Class Unsubscribe
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Page.Validate("frmUnsubscribe")
        If Page.IsValid Then
            RemoveEmail()
        End If

    End Sub

    Private Sub RemoveEmail()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdRemoveEmail As New SqlCommand("Unsubscribe_InsertNew", cnn)
                Try
                    cmdRemoveEmail.CommandType = CommandType.StoredProcedure
                    cmdRemoveEmail.Parameters.Add(New SqlParameter("@Email", SqlDbType.NVarChar, 100)).Value = Trim(txtEmail.Text)
                    cmdRemoveEmail.ExecuteNonQuery()
                Catch ex As Exception
                    lblmsg.Text = "We are experiencing some technical difficulities. Please try again in a few minutes."
                    txtEmail.Text = String.Empty
                Finally
                    lblmsg.Text = "You have been removed from our email list."
                    txtEmail.Text = String.Empty
                End Try
            End Using
        End Using
    End Sub
End Class
