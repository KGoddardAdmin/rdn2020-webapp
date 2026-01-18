Imports System.Data
Imports System.Data.SqlClient

Partial Class portal_CorrectCampaignLinks
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")

    Protected Sub cmdReset_Click(sender As Object, e As System.EventArgs) Handles cmdReset.Click
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdReSetCampaignLinks As New SqlCommand("Campaign_RemoveHTMLEncodingFromLink", cnn)
                cmdReSetCampaignLinks.CommandType = CommandType.StoredProcedure
                cmdReSetCampaignLinks.ExecuteNonQuery()
            End Using
        End Using
        lblmsg.Text = "HTML Formating has been removed from links"
        lblmsg.Font.Bold = True

    End Sub
End Class
