Imports System.Data
Imports System.Data.SqlClient

Partial Class ClientReportByLink
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Private Table_Name As String
    Private Month As Integer
    Private CampaignId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CampaignId = Request.QueryString("c")
        Month = Request.QueryString("Month")
        GetReport()
    End Sub

    Private Sub GetReport()
        Dim spToUse As String = "Track_Click_" & Month & "_LinkReportByCampaignId"        
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetReport As New SqlCommand(spToUse, cnn)
                cmdGetReport.CommandType = CommandType.StoredProcedure
                cmdGetReport.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = CampaignId
                Using dtrGetReport As SqlDataReader = cmdGetReport.ExecuteReader
                    gridreport.DataSource = dtrGetReport
                    gridreport.DataBind()
                End Using
            End Using
        End Using
    End Sub

    Public Function GetLink(ByVal LinkId As Integer) As String
        Dim rtn As String = String.Empty
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetLink As New SqlCommand("Campaign_GetLink", cnn)
                cmdGetLink.CommandType = CommandType.StoredProcedure
                cmdGetLink.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = CampaignId
                cmdGetLink.Parameters.Add(New SqlParameter("@LinkId", SqlDbType.Int)).Value = LinkId
                rtn = cmdGetLink.ExecuteScalar
            End Using
        End Using
        Return rtn
    End Function
End Class
