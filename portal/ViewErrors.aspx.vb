Imports System.Data.SqlClient

Partial Class ViewErrors
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtmsg.Text = String.Empty
        GetError()
    End Sub

    Protected Sub cmdClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        ClearTable()
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
End Class
