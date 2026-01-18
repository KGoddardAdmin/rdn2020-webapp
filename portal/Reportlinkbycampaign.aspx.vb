Imports System.Data
Imports System.Data.SqlClient


Partial Class Reportlinkbycampaign
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Private CampaignId As Integer    
    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblhmsg.Text = String.Empty
        lblhmsg.Font.Bold = True
        lblhmsg.Font.Size = FontUnit.Medium
        CampaignId = Request.QueryString("c")
        GetLinks()
        GetByCampaignId()
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

#Region "Load Grid"
    Private Sub GetLinks()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetLinks As New SqlCommand("Track_Click_LinkReport_ByCampaignId", cnn)
                cmdGetLinks.CommandType = CommandType.StoredProcedure
                cmdGetLinks.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = CampaignId
                Using dtrGetLinks As SqlDataReader = cmdGetLinks.ExecuteReader
                    gridreport.DataSource = dtrGetLinks
                    gridreport.DataBind()
                End Using
            End Using
        End Using
    End Sub
#End Region

#Region "Get Client"

    Private Sub GetByCampaignId()
        Dim ClientName As String = String.Empty
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetbyCampaignId As New SqlCommand("Client_GetByCampaignId", cnn)
                cmdGetbyCampaignId.CommandType = CommandType.StoredProcedure
                cmdGetbyCampaignId.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = CampaignId
                Using dtrGetByCampaignId As SqlDataReader = cmdGetbyCampaignId.ExecuteReader
                    While dtrGetByCampaignId.Read
                        ClientName = dtrGetByCampaignId("ClientName")
                    End While
                End Using
            End Using
        End Using
        lblhmsg.Text = "Link Report For """ & ClientName & """ On Campaign " & CampaignId
        lblhmsg.Font.Bold = True
        lblhmsg.Font.Size = FontUnit.XLarge
    End Sub

#End Region

End Class
