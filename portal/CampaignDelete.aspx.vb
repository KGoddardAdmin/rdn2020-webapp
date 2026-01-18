Imports System.Data
Imports System.Data.SqlClient

Partial Class CampaignDelete
    Inherits Page

    Private ReadOnly _strConn As String = ConfigurationSettings.AppSettings("cnn")
    Private _campaignid As Integer
    Private _campaignName As String


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        lblmsg.Font.Bold = False
        lblmsg.ForeColor = Drawing.Color.Black
        _campaignid = Request.QueryString("c")
        _campaignName = Request.QueryString("Name")
        lbloutput.Text = "Are you sure you want to permentenly delete " & _campaignName & " ?"
    End Sub

    Protected Sub cmdDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDelete.Click
        DeleteFromCampaign()
        Delete()
    End Sub

    Private Sub DeleteFromCampaign()
        Using cnn As New SqlConnection(_strConn)
            cnn.Open()
            Using cmdDeleteOldCampaign As New SqlCommand("Campaign_DeleteCampaign", cnn)
                cmdDeleteOldCampaign.CommandType = CommandType.StoredProcedure
                cmdDeleteOldCampaign.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = _campaignid
                cmdDeleteOldCampaign.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Sub Delete()
        Using cnn As New SqlConnection(_strConn)
            cnn.Open()
            Using cmdDelete As New SqlCommand("CampaignAdCopy_Delete", cnn)
                Try
                    cmdDelete.CommandType = CommandType.StoredProcedure
                    cmdDelete.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = _campaignid
                    cmdDelete.ExecuteNonQuery()
                Catch ex As Exception
                    lblmsg.Text = "Sorry there was a problem trying to delete Campaign " & _campaignName & " Please try again. "
                    lblmsg.Font.Bold = True
                    lblmsg.ForeColor = Drawing.Color.Red
                Finally
                    lblmsg.Text = "Campaign " & _campaignName & " has been successfully deleted."
                    lblmsg.Font.Bold = True
                End Try
            End Using
        End Using
    End Sub
End Class
