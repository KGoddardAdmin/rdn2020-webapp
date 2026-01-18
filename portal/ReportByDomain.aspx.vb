Imports System.Data
Imports System.Data.SqlClient

Partial Class Portal_ReportByDomain
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Private strRTConn As String = ConfigurationSettings.AppSettings("trconstr")
    Private Client As String
    Private ClientName As String
    Private IOUId As String
    Private NameOfClient As String
    Private LoggedIn As Boolean
    Private LoggedOnUser As String
    Private StartDate As Date
    Private EndDate As Date

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            ddlGetTimeSpan.Items.Add("Current Week")
            ddlGetTimeSpan.Items.Add("Prior Week")
            ddlGetTimeSpan.Items.Add("2 Weeks Prior")
            ddlGetTimeSpan.Items.Add("3 Weeks Prior")
            ddlGetTimeSpan.Items.Add("4 Weeks Prior")
            ddlGetTimeSpan.Items.Add("5 Weeks Prior")
            ddlGetTimeSpan.Items.Add("6 Weeks Prior")
            GetClients()
        End If
        GetDates()
    End Sub

    Protected Sub cmdGetReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGetReport.Click
        GetDomainReport()
    End Sub

    Private Sub GetClients()
        Dim sqlstr As String
        sqlstr = "SELECT * " & _
                "FROM Client " & _
                "ORDER BY ClientName"
        '"WHERE ClientId IN (2,4,7,12) " & _


        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetClients As New SqlCommand(sqlstr, cnn)
                'cmdGetClients.CommandType = CommandType.StoredProcedure
                Using dtrGetClients As SqlDataReader = cmdGetClients.ExecuteReader
                    ddlClient.DataSource = dtrGetClients
                    ddlClient.DataBind()
                End Using
            End Using
        End Using
    End Sub

    Private Sub GetDates()
        Dim dteCurrentDate As Date
        dteCurrentDate = Date.Today()

        Select Case dteCurrentDate.DayOfWeek
            Case DayOfWeek.Monday
                StartDate = dteCurrentDate.AddDays(-1)
            Case DayOfWeek.Tuesday
                StartDate = dteCurrentDate.AddDays(-2)
            Case DayOfWeek.Wednesday
                StartDate = dteCurrentDate.AddDays(-3)
            Case DayOfWeek.Thursday
                StartDate = dteCurrentDate.AddDays(-4)
            Case DayOfWeek.Friday
                StartDate = dteCurrentDate.AddDays(-5)
            Case DayOfWeek.Saturday
                StartDate = dteCurrentDate.AddDays(-6)
            Case DayOfWeek.Sunday
                StartDate = dteCurrentDate
        End Select

        Select Case ddlGetTimeSpan.SelectedValue
            Case "Current Week"
                EndDate = StartDate.AddDays(7)
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
            Case Else
        End Select

    End Sub

    Private Sub GetDomainReport()
        Dim dsReport As DataSet = New DataSet()
        Dim taReport As SqlDataAdapter = New SqlDataAdapter()
        Dim count As Integer
        Dim intCampaignId As Integer
        Dim intTotalOpens As Double
        Dim intTotalClicks As Double
        Dim intListSize As Double
        Dim intDomainCId As Integer
        Dim strDomain As String
        Dim strMessage As String = ""

        'Try

        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using scReport As New SqlCommand("sp_DomainReport_GetInfo", cnn)
                scReport.CommandType = CommandType.StoredProcedure
                scReport.Parameters.Add(New SqlParameter("@ClientUId", SqlDbType.UniqueIdentifier)).Value = SqlTypes.SqlGuid.Parse(ddlClient.SelectedValue)
                scReport.Parameters.Add(New SqlParameter("@startDate", SqlDbType.DateTime)).Value = StartDate
                scReport.Parameters.Add(New SqlParameter("@endDate", SqlDbType.DateTime)).Value = EndDate
                taReport.SelectCommand = scReport
                taReport.Fill(dsReport)

                'dsReport.Tables(0).Columns.Add(New DataColumn("Domain Clicks"))
                'dsReport.Tables(0).Columns.Add(New DataColumn("Domain Opens"))

                For count = 0 To dsReport.Tables(0).Rows.Count - 1
                    intCampaignId = dsReport.Tables(0).Rows(count).Item(0)
                    intListSize = dsReport.Tables(0).Rows(count).Item(4)
                    intTotalOpens = dsReport.Tables(0).Rows(count).Item(5)
                    intTotalClicks = dsReport.Tables(0).Rows(count).Item(6)
                    strDomain = dsReport.Tables(0).Rows(count).Item(7)
                    intDomainCId = dsReport.Tables(0).Rows(count).Item(8)


                    dsReport.Tables(0).Rows(count).Item(9) = GetDomainCounts(strDomain, "Clicks", intDomainCId)
                    dsReport.Tables(0).Rows(count).Item(10) = GetDomainCounts(strDomain, "Opens", intDomainCId)


                Next
                dsReport.AcceptChanges()
                gridCampaignReportWeekly.DataSource = dsReport
                gridCampaignReportWeekly.DataBind()

            End Using

            cnn.Close()

        End Using

        'Catch ex As Exception

        'strMessage = "An Error has Occured Please Try Again"
        'cvError.IsValid = False
        'cvError.ErrorMessage = strMessage

        'Finally

        taReport.Dispose()
        dsReport.Clear()
        dsReport.Dispose()

        'End Try
    End Sub
    Public Function GetDomainCounts(ByVal Domain As String, ByVal Type As String, ByVal Id As Integer) As Integer
        Dim rtn As String = String.Empty
        Domain = LCase(Domain)
        Select Case Trim(Domain)
            Case "rdn2020.com"
                Return GetDomainCounts(Type, Id, "rdn2020")
            Case "advantageemailstats.com"
                Return GetDomainCounts(Type, Id, "AdvantageEmailStats")
            Case "campusmediastats.com"
                Return GetDomainCounts(Type, Id, "CampusMediaStats")
            Case "dailyheraldemailstats.com"
                Return GetDomainCounts(Type, Id, "DailyHeraldEmailStats")
            Case "e-reportsonline.com"
                Return GetDomainCounts(Type, Id, "eReportsOnline")
            Case "itrackemailstats.com"
                Return GetDomainCounts(Type, Id, "iTrackEmailStats")
            Case "itrackingonline.com"
                Return GetDomainCounts(Type, Id, "iTrackingOnline")
            Case "lamarkcampaignstats.com"
                Return GetDomainCounts(Type, Id, "LamarkCampaignStats")
            Case "measuredmarketingreporting.com"
                Return GetDomainCounts(Type, Id, "MeasuredMarketingReporting")
            Case "specialistsemailstats.com"
                Return GetDomainCounts(Type, Id, "SpecialistsEmailStats")
            Case "startribuneadvantagereporting.com"
                Return GetDomainCounts(Type, Id, "StarTribuneAdvantageReporting")
            Case "t5emailstats.com"
                Return GetDomainCounts(Type, Id, "T5EmailStats")
            Case "usdataemailstats.com"
                Return GetDomainCounts(Type, Id, "USDataEmailStats")
            Case "venturecontrolsemailstats.com"
                Return GetDomainCounts(Type, Id, "VentureControlEmailStats")
            Case "clearcampaignstats.com"
                Return GetDomainCounts(Type, Id, "ClearCampaignStats")
            Case "broadcastcampaignstats.com"
                Return GetDomainCounts(Type, Id, "broadcastcampaignstats")
            Case "e-reportsonline.com"
                Return GetDomainCounts(Type, Id, "eReportsOnline")
            Case "itrackemailstats.com"
                Return GetDomainCounts(Type, Id, "iTrackEmailStats")
            Case "eautoquestreporting.com"
                Return GetDomainCounts(Type, Id, "eautoquestreporting")
            Case "erinteractiveemailstats.com"
                Return GetDomainCounts(Type, Id, "erinteractiveemailstats")
            Case "godigitaltracking.com"
                Return GetDomainCounts(Type, Id, "godigitaltracking")
            Case "e-trackingreports.com"
                Return GetDomainCounts(Type, Id, "etrackingreport")
            Case "emarkettrack.com"
                Return GetDomainCounts(Type, Id, "emarkettrack")
            Case "trackingstats.info"
                Return GetDomainCounts(Type, Id, "trackingstats")
            Case "campaigntracking.info"
                Return GetDomainCounts(Type, Id, "campaigntracking")
            Case "dealeremailmarketing.com"
                Return GetDomainCounts(Type, Id, "dealeremailmarketing")
            Case "itrackweb.net"
                Return GetDomainCounts(Type, Id, "itrackweb")
            Case "campaign-statistics.net"
                Return GetDomainCounts(Type, Id, "campaignstatistics")
            Case "americaemailmarketing.com"
                Return GetDomainCounts(Type, Id, "americaemailmarketing")
            Case Else

                Return "0"
        End Select
    End Function

#Region "Get Domian Counts"

    Private Function GetDomainCounts(ByVal Type As String, ByVal Id As Integer, ByVal Domian As String) As Integer
        Dim rtn As Integer
        Dim spToUse As String
        If Type = "Clicks" Then
            spToUse = "sp_DomainReport_GetDomainClicks"
        Else
            spToUse = "sp_DomainReport_GetDomainOpens"
        End If
        If Domian = "TrackingReport" Then
            Using cnn As New SqlConnection(strRTConn)
                cnn.Open()
                Using cmdGetLeadMeCounts As New SqlCommand(spToUse, cnn)
                    cmdGetLeadMeCounts.CommandType = CommandType.StoredProcedure
                    cmdGetLeadMeCounts.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = Id
                    rtn = cmdGetLeadMeCounts.ExecuteScalar
                End Using
            End Using
        Else
            Using cnn As New SqlConnection(strConn)
                cnn.Open()
                Using cmdGetLeadMeCounts As New SqlCommand(spToUse, cnn)
                    cmdGetLeadMeCounts.CommandType = CommandType.StoredProcedure
                    cmdGetLeadMeCounts.Parameters.Add(New SqlParameter("@Domain", SqlDbType.VarChar)).Value = Trim(Domian)
                    cmdGetLeadMeCounts.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = Id
                    rtn = cmdGetLeadMeCounts.ExecuteScalar
                End Using
            End Using
        End If
        Return rtn
    End Function

#End Region

End Class