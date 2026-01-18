Imports System.Data
Imports System.Data.SqlClient

Partial Class ClientReport
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Private Client As String
    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Client = "A724149C-1888-43E1-8F14-999F296129B9"
        GetIOByClient()
    End Sub

    Private Sub GetIOByClient()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetIO As New SqlCommand("IO_GetForReport_ByClient", cnn)
                cmdGetIO.CommandType = CommandType.StoredProcedure
                cmdGetIO.Parameters.Add(New SqlParameter("@ClientUId", SqlDbType.UniqueIdentifier)).Value = SqlTypes.SqlGuid.Parse(Client)
                Using dtrGetIO As SqlDataReader = cmdGetIO.ExecuteReader
                    gridClientIO.DataSource = dtrGetIO
                    gridClientIO.DataBind()
                End Using
            End Using
        End Using
    End Sub
End Class
