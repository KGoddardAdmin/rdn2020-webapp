Imports System.Data.SqlClient

Partial Class ManageContactErrors
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Private InValidLoginRedirectURL As String = ConfigurationSettings.AppSettings("InvalidLoginURL")


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckLogin()
       
    End Sub

    Protected Sub cmdClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        ClearTable()
    End Sub

    Private Sub CheckLogin()
        If Session("AcceptedUser") IsNot Nothing Then
            txtmsg.Text = String.Empty
            GetError()
        Else
            LoginFailure()
        End If
    End Sub

    Private Sub ClearTable()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdClearTable As New SqlCommand("Error_ClearTable", cnn)
                cmdClearTable.CommandType = Data.CommandType.StoredProcedure
                cmdClearTable.Parameters.Add(New SqlParameter("@ErrorId", Data.SqlDbType.Int)).Value = ErrorId.Value
                cmdClearTable.ExecuteNonQuery()
            End Using
        End Using
        GetError()

    End Sub

    Private Sub GetError()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetError As New SqlCommand("Error_Get", cnn)
                cmdGetError.CommandType = Data.CommandType.StoredProcedure
                Using dtrGetError As SqlDataReader = cmdGetError.ExecuteReader
                    If dtrGetError.HasRows Then
                        While dtrGetError.Read
                            If Not IsDBNull(dtrGetError("ErrorMsg")) Then
                                txtmsg.Text = dtrGetError("ErrorMsg")
                                ErrorId.Value = dtrGetError("ErrorId")
                            Else
                                txtmsg.Text = "There are no error messages."
                                txtmsg.Font.Bold = True
                                txtmsg.ForeColor = Drawing.Color.Red
                            End If
                        End While
                    Else
                        txtmsg.Text = "There are no error messages."
                        txtmsg.Font.Bold = True
                        txtmsg.ForeColor = Drawing.Color.Red
                    End If                   
                End Using
            End Using
        End Using
    End Sub

    Private Sub LoginFailure()
        Response.Redirect(InValidLoginRedirectURL)
    End Sub

End Class
