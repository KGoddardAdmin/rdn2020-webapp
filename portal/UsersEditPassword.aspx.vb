Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.VisualBasic
Imports System.Security.Cryptography

Partial Class UsersEditPassword
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Private UId As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblmsg.Text = String.Empty
        lblmsg.Font.Bold = False
        lblmsg.ForeColor = Drawing.Color.Black

        If Request.QueryString("c") <> String.Empty Then
            UId = Request.QueryString("c")
        Else
            lblmsg.Text = "We are sorry but you appear to have accessed this page by mistake. Please try again."
            lblmsg.Font.Bold = True
            lblmsg.ForeColor = Drawing.Color.Red
        End If
    End Sub

    Protected Sub cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click

        Page.Validate("frmpassword")
        If Page.IsValid Then
            Updatepassword()
        End If

    End Sub

    Private Sub Updatepassword()
        'Encode the Password NO Salt used       
        Dim md5Hasher As New MD5CryptoServiceProvider()
        Dim hashedBytes As Byte()
        Dim encoder As New UTF8Encoding()
        hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(txtPassword.Text))

        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdUpdatePassword As New SqlCommand("Users_UpdatePassword", cnn)
                Try
                    cmdUpdatePassword.CommandType = CommandType.StoredProcedure
                    cmdUpdatePassword.Parameters.Add(New SqlParameter("@Password", SqlDbType.Binary, 16)).Value = hashedBytes
                    cmdUpdatePassword.Parameters.Add(New SqlParameter("@UId", SqlDbType.UniqueIdentifier)).Value = SqlTypes.SqlGuid.Parse(UId)
                    cmdUpdatePassword.ExecuteNonQuery()
                Catch ex As Exception
                    lblmsg.Text = "There was a problem with updating the password. Please try again later."
                    lblmsg.Font.Bold = True
                    lblmsg.ForeColor = Drawing.Color.Red
                Finally
                    lblmsg.Text = "User password successfully updated."
                    lblmsg.Font.Bold = True
                    lblmsg.ForeColor = Drawing.Color.DarkGreen
                End Try

            End Using
        End Using
    End Sub

    

End Class
