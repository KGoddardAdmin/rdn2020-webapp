Imports System.Data
Imports System.Data.SqlClient

Partial Class CampaignListViewCreative
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Public Creative As String
    Private CampaignId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CampaignId = Request.QueryString("CampaignId")
        GetCreative()
    End Sub

    Private Sub GetCreative()
        Dim msg As String
        Dim name As String = String.Empty
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetCreative As New SqlCommand("CampaignAdCopy_GetByCampaignId", cnn)
                cmdGetCreative.CommandType = CommandType.StoredProcedure
                cmdGetCreative.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = CampaignId
                Using dtrGetCreative As SqlDataReader = cmdGetCreative.ExecuteReader
                    While dtrGetCreative.Read
                        Creative = dtrGetCreative("ConvertedCreative")
                        name = dtrGetCreative("CampaignName")
                    End While
                End Using
            End Using
        End Using
        msg = "Creative for Campaign " & name
        lblmsg.Text = msg
        lblmsg.Font.Bold = True

    End Sub
End Class
