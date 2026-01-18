Imports System.Data.SqlClient
Imports System.Data

Partial Class portal_CampaignUpdateStatus
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Protected strCampaignIdList As String

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub cmdUpdate_Click(sender As Object, e As System.EventArgs) Handles cmdUpdate.Click
        If (clearApproved.Checked) Then
            UpdateStatus5()
        Else
            UpdateStatus4(campaignIdList.Text)
        End If
    End Sub

    Private Sub UpdateStatus4(campaignIds As String)
        Dim convertedStr As String = campaignIds.Replace(vbCr, ",").Replace(vbLf, "")
        convertedStr = convertedStr.Trim()

        If (convertedStr.EndsWith(",")) Then
            convertedStr = convertedStr.Remove(convertedStr.Length - 1)
        End If

        If (convertedStr.StartsWith(",")) Then
            convertedStr = convertedStr.Remove(0, 1)
        End If

        If (convertedStr <> "") Then
            Using cnn As New SqlConnection(strConn)
                cnn.Open()
                Using cmdSetStatus As New SqlCommand("sp_Campaign_Update_Status", cnn)
                    cmdSetStatus.CommandType = CommandType.StoredProcedure
                    cmdSetStatus.Parameters.Add(New SqlParameter("@CampaignIds", SqlDbType.VarChar)).Value = convertedStr
                    cmdSetStatus.ExecuteNonQuery()
                End Using
            End Using

            updateMsg.Text = "Status updated from Percentages Set"
        Else
            updateMsg.Text = "No Campaign ID's Entered..."
        End If
    End Sub

    Private Sub UpdateStatus5()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdSetStatus As New SqlCommand("sp_Campaign_Update_Approved_Status", cnn)
                cmdSetStatus.CommandType = CommandType.StoredProcedure
                cmdSetStatus.ExecuteNonQuery()
            End Using
        End Using
        updateMsg.Text = "Status updated from Approved And Ready To Go!"
    End Sub
End Class
