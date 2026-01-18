Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.VisualBasic
Imports System.Security.Cryptography

Partial Class UsersEdit
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Private usercount As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblmsg.Text = String.Empty
        lblmsg.Font.Bold = False
        lblmsg.ForeColor = Drawing.Color.Black

        If Session("AcceptedUser") IsNot Nothing Then
            Dim arruser() As String = New String() {}
            arruser = Session("AcceptedUser").Split("~")
            If arruser(2) <> "Administrator" Then
                Response.Redirect("default.aspx")
            End If
            If Not IsPostBack Then
                GetUsers()
                GetRoles()
            End If
        Else
            Response.Redirect("default.aspx")
        End If
    End Sub

    Protected Sub ddUser_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddUser.SelectedIndexChanged
        GetUser()
        hyplPassword.NavigateUrl = "UsersEditPassword.aspx?c=" & ddUser.SelectedValue
    End Sub

    Protected Sub cmdUpdateUser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUpdateUser.Click
        UpdateUser()
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

   
    Private Sub GetUsers()
        Dim dstUsers As New DataSet
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetUsers As New SqlCommand("Users_GetActive", cnn)
                cmdGetUsers.CommandType = CommandType.StoredProcedure
                Using dadGetUsers As New SqlDataAdapter(cmdGetUsers)
                    dadGetUsers.Fill(dstUsers, "Users")
                    If dstUsers.Tables("Users").Rows.Count > 0 Then
                        Dim Dyncolumn As New DataColumn
                        With Dyncolumn
                            .ColumnName = "User"
                            .DataType = System.Type.GetType("System.String")
                            .Expression = "FName+'       '+LName"
                        End With
                        dstUsers.Tables("Users").Columns.Add(Dyncolumn)
                        ddUser.DataTextField = "User"
                        ddUser.DataValueField = "UId"
                        ddUser.DataSource = dstUsers.Tables("Users").DefaultView
                        ddUser.DataBind()
                    End If
                    ddUser.Items.Insert(0, "Select User")
                End Using
            End Using
        End Using
    End Sub

    Private Sub GetUser()
        Dim IsActive As Integer

        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetUser As New SqlCommand("Users_GetByUid", cnn)
                cmdGetUser.CommandType = CommandType.StoredProcedure
                cmdGetUser.Parameters.Add(New SqlParameter("@UId", SqlDbType.UniqueIdentifier)).Value = SqlTypes.SqlGuid.Parse(ddUser.SelectedValue)
                Using dtrGetUser As SqlDataReader = cmdGetUser.ExecuteReader
                    While dtrGetUser.Read

                        If Not IsDBNull(dtrGetUser("UserId")) Then
                            txtLogin.Text = dtrGetUser("UserId")
                        Else
                            txtLogin.Text = String.Empty
                        End If

                        If Not IsDBNull(dtrGetUser("Email")) Then
                            txtEmail.Text = dtrGetUser("Email")
                        Else
                            txtEmail.Text = String.Empty
                        End If

                        If Not IsDBNull(dtrGetUser("FName")) Then
                            txtFName.Text = dtrGetUser("FName")
                        Else
                            txtFName.Text = String.Empty
                        End If

                        If Not IsDBNull(dtrGetUser("LName")) Then
                            txtLName.Text = dtrGetUser("LName")
                        Else
                            txtLName.Text = String.Empty
                        End If

                        ddRole.SelectedValue = dtrGetUser("RoleId")
                        IsActive = dtrGetUser("IsActive")

                    End While
                End Using
            End Using
        End Using

        If IsActive = 1 Then
            ckActive.Checked = True
        Else
            ckActive.Checked = False
        End If
    End Sub

    Public Sub EmailIsUnique(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim Count As Int16
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdEmailIsUnique As New SqlCommand("Users_EditEmailCheck", cnn)
                cmdEmailIsUnique.CommandType = CommandType.StoredProcedure
                cmdEmailIsUnique.Parameters.Add(New SqlParameter("@Email", SqlDbType.NVarChar, 100)).Value = e.Value
                cmdEmailIsUnique.Parameters.Add(New SqlParameter("@UId", SqlDbType.UniqueIdentifier)).Value = SqlTypes.SqlGuid.Parse(ddUser.SelectedValue)
                Count = cmdEmailIsUnique.ExecuteScalar
            End Using
        End Using
        If Count = 0 Then
            e.IsValid = True
        Else
            e.IsValid = False
        End If

    End Sub

    Private Sub UpdateUser()
        Dim Active As Integer
        If ckActive.Checked = True Then
            Active = 1
        Else
            Active = 0
        End If
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Try
                Using cmdUpdateUser As New SqlCommand("Users_Update", cnn)
                    cmdUpdateUser.CommandType = CommandType.StoredProcedure
                    cmdUpdateUser.Parameters.Add(New SqlParameter("@Userid", SqlDbType.NVarChar, 50)).Value = Trim(txtLogin.Text)
                    cmdUpdateUser.Parameters.Add(New SqlParameter("@FName", SqlDbType.NVarChar, 20)).Value = Trim(txtFName.Text)
                    cmdUpdateUser.Parameters.Add(New SqlParameter("@LName", SqlDbType.NVarChar, 20)).Value = Trim(txtLName.Text)
                    cmdUpdateUser.Parameters.Add(New SqlParameter("@RoleId", SqlDbType.TinyInt)).Value = ddRole.SelectedValue
                    cmdUpdateUser.Parameters.Add(New SqlParameter("@IsActive", SqlDbType.TinyInt)).Value = Active
                    cmdUpdateUser.Parameters.Add(New SqlParameter("@Email", SqlDbType.NVarChar, 150)).Value = Trim(txtEmail.Text)
                    cmdUpdateUser.Parameters.Add(New SqlParameter("@UId", SqlDbType.UniqueIdentifier)).Value = SqlTypes.SqlGuid.Parse(ddUser.SelectedValue)
                    cmdUpdateUser.ExecuteNonQuery()
                    lblmsg.Text = "User successfully updated."
                    lblmsg.Font.Bold = True
                    lblmsg.ForeColor = Drawing.Color.Green
                End Using
            Catch ex As Exception
                Response.Write(ex.ToString)

                lblmsg.Text = "There was a problem with the update, please try to update user again in a few minutes."
                lblmsg.Font.Bold = True
                lblmsg.ForeColor = Drawing.Color.Red
            End Try
        End Using

    End Sub
End Class
