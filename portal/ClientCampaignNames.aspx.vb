Imports System.Data
Imports System.Data.SqlClient

Partial Class portal_ClientCampaignNames
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    'Private strConn As String = ConfigurationSettings.AppSettings("cn") ' To use live data feed     
    Private StartDate As Date
    Private EndDate As Date
    Private ResultCount As Integer
    Private PageSize As Integer = 500

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        lblMessage.Text = String.Empty
        lblMessage.Font.Bold = False
        lblMessage.ForeColor = Drawing.Color.Black
        If Not IsPostBack Then
            GetClients()
            DatePicker1.DateValue = Today
        End If

    End Sub

    Private Sub GetClients()
        'ddClient.Items.Add(New ListItem("Select Client", 0))
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetClientList As New SqlCommand("Client_Get", cnn)
                Using dtrGetClientList As SqlDataReader = cmdGetClientList.ExecuteReader
                    ddClient.DataSource = dtrGetClientList
                    ddClient.DataBind()
                End Using
            End Using
        End Using
        'ddClient.Items.Insert(0, "Select Client")
    End Sub

    Protected Sub cmdGetReport_Click(sender As Object, e As System.EventArgs) Handles cmdGetReport.Click
        If Trim(LCase(ddClient.SelectedItem.Text)) = "bmi elite" Then
            Dim myDate As Date = DatePicker1.DateValue.ToShortDateString()
            StartDate = myDate ' DateAdd(DateInterval.Day, -1, myDate)
            EndDate = myDate 'DateAdd(DateInterval.Day, 0, myDate)
            BindSQL()
        Else
            'trgrid.Visible = False
            'dgCampaigns.DataSource = Nothing
            'lblMessage.Text = "Sorry that Client's Campaign Names are not avialable yet."
            'lblMessage.Font.Bold = True
            'lblMessage.ForeColor = Drawing.Color.Red
            Dim myDate As Date = DatePicker1.DateValue.ToShortDateString()
            StartDate = myDate ' DateAdd(DateInterval.Day, -1, myDate)
            EndDate = myDate 'DateAdd(DateInterval.Day, 0, myDate)
            BindSQL()
        End If

    End Sub

#Region "Fill Grid"

    Sub BindSQL()

        Dim dadCampaigns As New SqlDataAdapter
        Dim DS As New DataSet
        Dim sp As String = String.Empty
        Dim RcdCount As Integer
        sp = "sp_ClientsDailyCampaigns"
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdCampaign As New SqlCommand(sp, cnn)
                cmdCampaign.CommandType = CommandType.StoredProcedure
                cmdCampaign.Parameters.Add(New SqlParameter("@StartDate", SqlDbType.DateTime)).Value = StartDate
                cmdCampaign.Parameters.Add(New SqlParameter("@EndDate", SqlDbType.DateTime)).Value = EndDate
                dadCampaigns.SelectCommand = cmdCampaign
                dadCampaigns.Fill(DS, "Campaign")
                RcdCount = DS.Tables("Campaign").Rows.Count.ToString()
            End Using
        End Using
        dgCampaigns.PageSize = PageSize
        ResultCount = RcdCount
        RecordCount.Text = "<b><font color=red>" & RcdCount & "</font> records found"
        dgCampaigns.DataSource = DS
        dgCampaigns.DataBind()
    End Sub
#End Region

End Class
