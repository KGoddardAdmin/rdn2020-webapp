Imports System.Data.SqlClient
Imports Microsoft.VisualBasic
Imports System.Security.Cryptography

Partial Class Login
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")   
    Private FName As String
    Private LName As String
    Private Roll As String
    Private Email As String

    Protected Sub cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Page.Validate("Login1")
        If Page.IsValid Then
            If CheckLogin() = True Then
                Session("AcceptedUser") = FName & "~" & LName & "~" & Roll & "~" & Email
                LoginSuccess()
            End If
        End If
    End Sub

    Private Sub LoginSuccess()
        Dim arruser() As String = New String() {}
        arruser = Session("AcceptedUser").Split("~")
        If arruser(2) = "Client" Then
            Response.Redirect("ClientReport.aspx")
        Else
            Response.Redirect("Default.aspx")
        End If

    End Sub

    Private Sub LoginFailure()
        lblmsg.Text = "Cannot locate your account please try again."
        lblmsg.Font.Bold = True
    End Sub

    Private Function CheckLogin() As Boolean
        Dim ValidLogin As Boolean = False
        'Encrypt the password
        Dim md5Hasher As New MD5CryptoServiceProvider()
        Dim hashedDataBytes As Byte()
        Dim encoder As New UTF8Encoding()
        hashedDataBytes = md5Hasher.ComputeHash(encoder.GetBytes(Password.Text))
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdCheckLogin As New SqlCommand("users_login", cnn)
                cmdCheckLogin.CommandType = Data.CommandType.StoredProcedure
                cmdCheckLogin.Parameters.Add(New SqlParameter("@UserId", Data.SqlDbType.NVarChar, 50)).Value = UserName.Text
                cmdCheckLogin.Parameters.Add(New SqlParameter("@Password", Data.SqlDbType.Binary, 16)).Value = hashedDataBytes
                Using dtrCheckLogin As SqlDataReader = cmdCheckLogin.ExecuteReader
                    If dtrCheckLogin.HasRows Then
                        While dtrCheckLogin.Read
                            ValidLogin = True
                            If Not IsDBNull(dtrCheckLogin("FName")) Then
                                FName = dtrCheckLogin("FName")
                            End If
                            If Not IsDBNull(dtrCheckLogin("LName")) Then
                                LName = dtrCheckLogin("LName")
                            End If
                            If Not IsDBNull(dtrCheckLogin("RoleName")) Then
                                Roll = dtrCheckLogin("RoleName")
                            End If

                            If Not IsDBNull(dtrCheckLogin("Email")) Then
                                Email = dtrCheckLogin("Email")
                            End If

                        End While
                    Else
                        lblmsg.Text = "Cannot locate your account please try again."
                        lblmsg.Font.Bold = True
                    End If
                End Using
            End Using
        End Using
        Return ValidLogin
    End Function
End Class
