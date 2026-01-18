Imports System.Data
Imports System.Data.SqlClient

Partial Class portal_ClientWeeklyCampaignNames
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    'Private strConn As String = ConfigurationSettings.AppSettings("cn") ' To use live data feed     
    Private StartDate As Date
    Private EndDate As Date
    Private ResultCount As Integer
    Private PageSize As Integer = 5000

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        lblMessage.Text = String.Empty
        lblMessage.Font.Bold = False
        lblMessage.ForeColor = Drawing.Color.Black
        If Not IsPostBack Then
            GetClients()
            ddlGetTimeSpan.Items.Add("Current Week")
            ddlGetTimeSpan.Items.Add("Prior Week")
            ddlGetTimeSpan.Items.Add("2 Weeks Prior")
            ddlGetTimeSpan.Items.Add("3 Weeks Prior")
            ddlGetTimeSpan.Items.Add("4 Weeks Prior")
            ddlGetTimeSpan.Items.Add("5 Weeks Prior")
            ddlGetTimeSpan.Items.Add("6 Weeks Prior")
            ddlGetTimeSpan.Items.Add("7 Weeks Prior")
            ddlGetTimeSpan.Items.Add("8 Weeks Prior")
            ddlGetTimeSpan.Items.Add("9 Weeks Prior")
        End If


    End Sub

    Private Sub GetDates()

        Dim dteCurrentDate As Date
        dteCurrentDate = Date.Today()

        Select Case dteCurrentDate.DayOfWeek
            Case DayOfWeek.Monday
                StartDate = dteCurrentDate
            Case DayOfWeek.Tuesday
                StartDate = dteCurrentDate.AddDays(-1)
            Case DayOfWeek.Wednesday
                StartDate = dteCurrentDate.AddDays(-2)
            Case DayOfWeek.Thursday
                StartDate = dteCurrentDate.AddDays(-3)
            Case DayOfWeek.Friday
                StartDate = dteCurrentDate.AddDays(-4)
            Case DayOfWeek.Saturday
                StartDate = dteCurrentDate.AddDays(-5)
            Case DayOfWeek.Sunday
                StartDate = dteCurrentDate.AddDays(-6)
        End Select

        Select Case ddlGetTimeSpan.SelectedValue
            Case "Current Week"
                EndDate = StartDate.AddDays(6)
            Case "Prior Week"
                StartDate = StartDate.AddDays(-7)
                EndDate = StartDate.AddDays(7)
            Case "2 Weeks Prior"
                StartDate = StartDate.AddDays(-14)
                EndDate = StartDate.AddDays(7)
            Case "3 Weeks Prior"
                StartDate = StartDate.AddDays(-21)
                EndDate = StartDate.AddDays(7)
            Case "4 Weeks Prior"
                StartDate = StartDate.AddDays(-28)
                EndDate = StartDate.AddDays(7)
            Case "5 Weeks Prior"
                StartDate = StartDate.AddDays(-35)
                EndDate = StartDate.AddDays(7)
            Case "6 Weeks Prior"
                StartDate = StartDate.AddDays(-42)
                EndDate = StartDate.AddDays(7)
            Case "7 Weeks Prior"
                StartDate = StartDate.AddDays(-49)
                EndDate = StartDate.AddDays(7)
            Case "8 Weeks Prior"
                StartDate = StartDate.AddDays(-56)
                EndDate = StartDate.AddDays(7)
            Case "9 Weeks Prior"
                StartDate = StartDate.AddDays(-63)
                EndDate = StartDate.AddDays(7)
            Case Else
        End Select

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
            GetDates()

            BindSQL()
        Else
            'trgrid.Visible = False
            'dgCampaigns.DataSource = Nothing
            lblMessage.Text = "Sorry that Client's Campaign Names are not avialable yet."
            lblMessage.Font.Bold = True
            lblMessage.ForeColor = Drawing.Color.Red
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
