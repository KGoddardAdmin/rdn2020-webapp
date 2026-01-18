Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.VisualBasic
Imports System.Security.Cryptography

Partial Class UsersCreateNew
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Private usercount As Integer = 0


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("AcceptedUser") IsNot Nothing Then
            Dim arruser() As String = New String() {}
            arruser = Session("AcceptedUser").Split("~")
            If arruser(2) <> "Administrator" Then
                Response.Redirect("default.aspx")
            End If
            If Not IsPostBack Then
                GetRoles()
            End If
        Else
            Response.Redirect("default.aspx")
        End If
    End Sub

    Protected Sub cmdAddUser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddUser.Click
        Page.Validate("frmUsers")
        If Page.IsValid Then
            CreateUser()
        End If
    End Sub

    Private Sub CreateUser()
        Dim MyGuid As Guid = Guid.NewGuid()
        'Encode the Password NO Salt used       
        Dim md5Hasher As New MD5CryptoServiceProvider()
        Dim hashedBytes As Byte()
        Dim encoder As New UTF8Encoding()
        hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(txtPassword.Text))

        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdCreateUser As New SqlCommand("Users_CreateNew", cnn)
                cmdCreateUser.CommandType = CommandType.StoredProcedure
                cmdCreateUser.Parameters.Add(New SqlParameter("@UId", SqlDbType.UniqueIdentifier)).Value = MyGuid
                cmdCreateUser.Parameters.Add(New SqlParameter("@UserId", SqlDbType.NVarChar, 50)).Value = Trim(txtLogin.Text)
                cmdCreateUser.Parameters.Add(New SqlParameter("@Password", SqlDbType.Binary, 16)).Value = hashedBytes
                cmdCreateUser.Parameters.Add(New SqlParameter("@FName", SqlDbType.NVarChar, 20)).Value = Trim(txtFName.Text)
                cmdCreateUser.Parameters.Add(New SqlParameter("@LName", SqlDbType.NVarChar, 20)).Value = Trim(txtLName.Text)
                cmdCreateUser.Parameters.Add(New SqlParameter("@RoleId", SqlDbType.TinyInt)).Value = ddRole.SelectedValue
                cmdCreateUser.Parameters.Add(New SqlParameter("@Email", SqlDbType.NVarChar, 150)).Value = Trim(txtEmail.Text)
                cmdCreateUser.ExecuteNonQuery()
            End Using
        End Using
        lblmsg.Text = "User Created."
        lblmsg.Font.Bold = True
        cmdAddUser.Visible = False

    End Sub

    Private Sub GetRoles()        
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetRoles As New SqlCommand("UserRole_Get", cnn)
                cmdGetRoles.CommandType = CommandType.StoredProcedure
                Using dtrGetRoles As SqlDataReader = cmdGetRoles.ExecuteReader
                    ddRole.DataSource = dtrGetRoles
                    ddRole.DataBind()
                End Using
            End Using
        End Using
    End Sub

    Public Sub EmailIsUnique(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdEmailIsUnique As New SqlCommand("Users_GetByEmail", cnn)
                cmdEmailIsUnique.CommandType = CommandType.StoredProcedure
                cmdEmailIsUnique.Parameters.Add(New SqlParameter("@Email", SqlDbType.NVarChar, 100)).Value = e.Value
                Using dtrEmailIsUnique As SqlDataReader = cmdEmailIsUnique.ExecuteReader
                    If dtrEmailIsUnique.HasRows Then
                        e.IsValid = False
                    Else
                        e.IsValid = True
                    End If
                End Using
            End Using
        End Using
    End Sub
End Class
