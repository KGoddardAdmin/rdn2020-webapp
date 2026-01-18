Imports System.Data.SqlClient
Imports System.Data
Imports System.Collections.Generic

Partial Class portal_AutomatedEmailEndingSim
    Inherits System.Web.UI.Page

#Region "Declare Variables"
    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Private MaxOpenRateMin As Integer = 13
    Private MaxOpenRateMax As Integer = 16
    Private MaxClickRateMin As Integer = 2
    Private MaxClickRateMax As Integer = 4
    Private Phase2PercentToUse As Integer = 100
    Private Phase3PercentToUse As Integer = 80
    Private Phase4PercentToUse As Integer = 50
    Private Phase2AddMin As Integer = 1
    Private Phase2AddMax As Integer = 4
    Private Phase3AddMin As Integer = 0
    Private Phase3AddMax As Integer = 3
    Private Phase4AddMin As Integer = 0
    Private Phase4AddMax As Integer = 2
    Private WildCardPercent As Integer = 20

    Private Phase1RandomMinDateRange As Integer = 1
    Private Phase1RandomMaxDateRange As Integer = 1
    Private Phase2RandomMinDateRange As Integer = 1
    Private Phase2RandomMaxDateRange As Integer = 10
    Private Phase3RandomMinDateRange As Integer = 3
    Private Phase3RandomMaxDateRange As Integer = 5
    Private Phase4RandomMinDateRange As Integer = 12
    Private Phase4RandomMaxDateRange As Integer = 22

    Private OpenCount As Integer
    Private OpenRate As Integer
    'Private OpenRateCount As Integer
    Private ClickCount As Integer
    Private ClickRate As Integer
    'Private ClickRateCount As Integer
    Private PhaseAddCount As Integer
    Private PhaseAddMin As Integer
    Private PhaseAddMax As Integer
    Private Phase2AddCount As Integer
    Private Phase3AddCount As Integer
    Private Phase4AddCount As Integer
    Private NumberOfPhaseCampaignsToUse As Integer
    Private NumberOfPhase2CampaignsToUse As Integer
    Private NumberOfPhase3CampaignsToUse As Integer
    Private NumberOfPhase4CampaignsToUse As Integer

    Private Phase1StartDate As Date
    Private Phase1RandomEndDayCount As Integer
    Private Phase1EndDayCount As Integer
    Private Phase1EndDate As Date
    Private Phase2StartDate As Date
    Private Phase2RandomEndDayCount As Integer
    Private Phase2EndDayCount As Integer
    Private Phase2EndDate As Date
    Private Phase3StartDate As Date
    Private Phase3RandomEndDayCount As Integer
    Private Phase3EndDayCount As Integer
    Private Phase3EndDate As Date
    Private Phase4StartDate As Date
    Private Phase4RandomEndDayCount As Integer
    Private Phase4EndDayCount As Integer
    Private Phase4EndDate As Date
    Private RandomClass As New Random()
    Private BeginLastMonth As Date
    Private CampaignList As New List(Of String)
    Private CampaignTimeFrameCount As Integer
    Private arLPhase As ArrayList
    Private arLClientIps As ArrayList

    Private ExistingOpens As Integer
    Private ExistingClicks As Integer
    'Private ClicksToAdd As Integer
    'Private OpensToAdd As Integer
    Private FinalClicks As Integer
    Private FinalOpens As Integer
    'Private DataBase As String
    Private TableName As String
    Private CountInCurrentMonth As Boolean = False

    'Private arLPhase2 As ArrayList
    'Private arLPhase3 As ArrayList
    'Private arLPhase4 As ArrayList
    Private arRDNPhaseClientInfo(,) As String
    'Campaign Id = 0
    'Client Campaign Id = 1
    'Data Base = 2
    'Emails Ordered = 3
    'Opens = 4
    'Clicks = 5
    'Opens To Add = 6
    'Clicks To Add  = 7


    'Private arRDNPhase2ClientInfo(,) As String
    'Private arRDNPhase3ClientInfo(,) As String
    'Private arRDNPhase4ClientInfo(,) As String    


#End Region

#Region "Display"

    Private Sub ShowPhaseIds(ByVal phase As Integer)
        Response.Write("<br>Phase " & phase & " Array<br>")
        arLPhase.Sort()
        Response.Write("<br>Phase " & phase & " Array Count " & arLPhase.Count)
        Response.Write("<br>Number Of Phase " & phase & " Campaigns To Use " & NumberOfPhaseCampaignsToUse)
        For x As Integer = 0 To NumberOfPhaseCampaignsToUse
            Response.Write("<br>" & arLPhase(x))
        Next
        Response.Write("<br><hr>")

    End Sub

    Private Sub ShowPhaseClientInfo(ByVal Phase As Integer)
        Response.Write("<br>In Show Phase <b>" & Phase & "</b> Client Info<hr>")
        For y As Integer = 0 To arRDNPhaseClientInfo.GetUpperBound(0)
            Response.Write("<br>Campaign Id = " & arRDNPhaseClientInfo(y, 0))
            Response.Write("<br>Client Campaign Id = " & arRDNPhaseClientInfo(y, 1))
            Response.Write("<br>Data Base = " & arRDNPhaseClientInfo(y, 2))
            Response.Write("<br>Emails Ordered = " & arRDNPhaseClientInfo(y, 3))
            Response.Write("<br>Opens = " & arRDNPhaseClientInfo(y, 4))
            Response.Write("<br>Clicks = " & arRDNPhaseClientInfo(y, 5))
            Response.Write("<br>Opens to Add = " & arRDNPhaseClientInfo(y, 6))
            Response.Write("<br>Clicks to Add = " & arRDNPhaseClientInfo(y, 7))
            Response.Write("<br><hr>")
        Next
    End Sub

    Private Sub ShowRandomValues()
        '========From Get Random Numbers =============
        Response.Write("<B>FROM GetRandomNumber</B>")

        Response.Write("<br>Open Rate is between " & MaxOpenRateMin & " and " & MaxOpenRateMax)
        Response.Write("<br>Open Rate is  " & OpenRate)

        Response.Write("<br><br>Click Rate is between " & MaxClickRateMin & " and " & MaxClickRateMax)
        Response.Write("<br>Clck Rate is  " & ClickRate)
        Response.Write("<Hr>")

        Response.Write("<br>Phase 1 Date  Range  is between " & Phase1RandomMinDateRange & " Days out and " & Phase1RandomMaxDateRange)
        Response.Write("<br>Phase 1 Random Day Count is  " & Phase1RandomEndDayCount)

        Response.Write("<br><br>Phase 2 Date  Range  is between " & Phase2RandomMinDateRange & " Days out and " & Phase2RandomMaxDateRange)
        Response.Write("<br>Phase 2 Random Day Count is  " & Phase2RandomEndDayCount)

        Response.Write("<br><br>Phase 3 Date  Range  is between " & Phase3RandomMinDateRange & " Days out and " & Phase3RandomMaxDateRange)
        Response.Write("<br>Phase 3 Random Day Count is  " & Phase3RandomEndDayCount)

        Response.Write("<br><br>Phase 4 Date  Range  is between " & Phase4RandomMinDateRange & " Days out and " & Phase4RandomMaxDateRange)
        Response.Write("<br>Phase 4 Random Day Count is  " & Phase4RandomEndDayCount)

    End Sub

    Private Sub ShowPhaseDateValues()
        '======From SetPhaseDates()
        Response.Write("<hr><B>FROM SetPhaseDates</B><hr>")

        Response.Write("<br>Phase 1 Day Count is " & Phase1EndDayCount)
        Response.Write("         Phase 1 Start Day is " & Phase1StartDate)
        Response.Write("         Phase 1 End Date is " & Phase1EndDate)

        Response.Write("<br><br>Phase 2 Day Count is " & Phase2EndDayCount)
        Response.Write("         Phase 2 Start Day is " & Phase2StartDate)
        Response.Write("         Phase 2 End Date is " & Phase2EndDate)

        Response.Write("<br><br>Phase 3 Day Count is " & Phase3EndDayCount)
        Response.Write("         Phase 3 Start Day is " & Phase3StartDate)
        Response.Write("         Phase 3 End Date is " & Phase3EndDate)

        Response.Write("<br><br>Phase 4 Day Count is " & Phase4EndDayCount)
        Response.Write("         Phase 4 Start Day is " & Phase4StartDate)
        Response.Write("         Phase 4 End Date is " & Phase4EndDate)
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetRandomNumbers()
        SetPhaseDates()
        GetCampaignInTimeFrame()
    End Sub


#Region "Getting Cients IPs and Inserting Campaigns"

    Private Sub GetCampaignIpsAndLinksAndInsert()
        For x As Integer = 0 To arRDNPhaseClientInfo.GetUpperBound(0)
            Dim DomainId As Integer = arRDNPhaseClientInfo(x, 1)
            Dim Domain As String = arRDNPhaseClientInfo(x, 2)
            Dim Opens As Integer = arRDNPhaseClientInfo(x, 6)
            Dim Clicks As Integer = arRDNPhaseClientInfo(x, 7)
            If LCase(Domain) <> "unknown" Then
                If Clicks > 0 Then
                    ClickRoutine(DomainId, Domain, Clicks)
                End If
                If Opens > 0 Then
                    OpenRoutine(DomainId, Domain, Opens)
                End If
            End If
            Response.Write("<br><hr>")
        Next


    End Sub

    Private Sub ClickRoutine(ByVal Id As Integer, ByVal Domain As String, ByVal Clicks As Integer)

        Dim intMonth As Integer
        Dim intLastMonth As Integer
        Dim LastMonthsTable As String
        Dim ThisMonthsTable As String
        Dim dt As Date = DateAdd(DateInterval.Month, -1, Today())
        Dim Clause As String = "CampaignId"
        intMonth = DatePart(DateInterval.Month, Date.Now)
        intLastMonth = DatePart(DateInterval.Month, dt)
        LastMonthsTable = "Track_Click_" & intLastMonth
        ThisMonthsTable = "Track_Click_" & intMonth

        Response.Write("<br>Pull from tables " & LastMonthsTable & " and " & ThisMonthsTable)
        Response.Write("    Click number to add " & Clicks)

        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdClickRoutine As New SqlCommand("sp_AutomatedEmail_GetClientsIpAndLinks", cnn)
                cmdClickRoutine.CommandType = CommandType.StoredProcedure
                cmdClickRoutine.Parameters.Add(New SqlParameter("@Count", SqlDbType.Int)).Value = Clicks
                cmdClickRoutine.Parameters.Add(New SqlParameter("@Domain", SqlDbType.VarChar)).Value = Domain
                cmdClickRoutine.Parameters.Add(New SqlParameter("@LastMonthsTable", SqlDbType.VarChar)).Value = LastMonthsTable
                cmdClickRoutine.Parameters.Add(New SqlParameter("@ThisMonthsTable", SqlDbType.VarChar)).Value = ThisMonthsTable
                cmdClickRoutine.Parameters.Add(New SqlParameter("@Clause", SqlDbType.VarChar)).Value = Clause
                cmdClickRoutine.Parameters.Add(New SqlParameter("@Id", SqlDbType.Int)).Value = Id
                Using dtrClickRoutine As SqlDataReader = cmdClickRoutine.ExecuteReader
                    While dtrClickRoutine.Read
                        InsertOpens(Domain, Id, dtrClickRoutine("IP"))

                        InsertClicks(Domain, Id, dtrClickRoutine("LinkId"), dtrClickRoutine("Ip"))
                    End While
                End Using
            End Using
        End Using
    End Sub

    Private Sub OpenRoutine(ByVal Id As Integer, ByVal Domain As String, ByVal Opens As Integer)
        Dim intMonth As Integer
        Dim intLastMonth As Integer
        Dim LastMonthsTable As String
        Dim ThisMonthsTable As String
        Dim dt As Date = DateAdd(DateInterval.Month, -1, Today())
        Dim Clause As String = "ImpressionCampaignId"
        intMonth = DatePart(DateInterval.Month, Date.Now)
        intLastMonth = DatePart(DateInterval.Month, dt)
        LastMonthsTable = "Impression_" & intLastMonth
        ThisMonthsTable = "Impression_" & intMonth
        Response.Write("<br>Pull from tables " & LastMonthsTable & " and " & ThisMonthsTable)
        Response.Write("    Open number to add " & Opens)
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdOpenRoutine As New SqlCommand("sp_AutomatedEmail_GetClientsIpAndLinks", cnn)
                cmdOpenRoutine.CommandType = CommandType.StoredProcedure
                cmdOpenRoutine.Parameters.Add(New SqlParameter("@Count", SqlDbType.Int)).Value = Opens
                cmdOpenRoutine.Parameters.Add(New SqlParameter("@Domain", SqlDbType.VarChar)).Value = Domain
                cmdOpenRoutine.Parameters.Add(New SqlParameter("@LastMonthsTable", SqlDbType.VarChar)).Value = LastMonthsTable
                cmdOpenRoutine.Parameters.Add(New SqlParameter("@ThisMonthsTable", SqlDbType.VarChar)).Value = ThisMonthsTable
                cmdOpenRoutine.Parameters.Add(New SqlParameter("@Clause", SqlDbType.VarChar)).Value = Clause
                cmdOpenRoutine.Parameters.Add(New SqlParameter("@Id", SqlDbType.Int)).Value = Id
                Using dtrOpenRoutine As SqlDataReader = cmdOpenRoutine.ExecuteReader
                    While dtrOpenRoutine.Read
                        InsertOpens(Domain, Id, dtrOpenRoutine("IP"))
                    End While
                End Using
            End Using
        End Using

    End Sub

    Private Sub InsertClicks(ByVal Domain As String, ByVal CampaignId As Integer, ByVal LinkId As Integer, ByVal Ip As String) ' 0=Opens 1 = Clicks
        Dim intMonth As Integer
        Dim InsertTable As String
        intMonth = DatePart(DateInterval.Month, Date.Now)
        InsertTable = "Track_Click_" & intMonth

        Response.Write("<br>In INSERT CLICKS <BR>Domain " & Domain)
        Response.Write("  Insert Table " & InsertTable)
        Response.Write("  CampaignId  " & CampaignId)
        Response.Write("  LinkId  " & LinkId)
        Response.Write("  Ip   " & Ip)
        'Using cnn As New SqlConnection(strConn)
        '    cnn.Open()
        '    Using cmdInsertClicks As New SqlCommand("sp_AutomatedEmail_AdjustClicks", cnn)
        '        cmdInsertClicks.CommandType = CommandType.StoredProcedure
        '        cmdInsertClicks.Parameters.Add(New SqlParameter("@Domain", SqlDbType.VarChar)).Value = Domain
        '        cmdInsertClicks.Parameters.Add(New SqlParameter("@InsertTable", SqlDbType.VarChar)).Value = InsertTable
        '        cmdInsertClicks.Parameters.Add(New SqlParameter("@CampaignID", SqlDbType.Int)).Value = CampaignId
        '        cmdInsertClicks.Parameters.Add(New SqlParameter("@LinkId", SqlDbType.Int)).Value = LinkId
        '        cmdInsertClicks.Parameters.Add(New SqlParameter("@Ip", SqlDbType.VarChar)).Value = Ip
        '        cmdInsertClicks.ExecuteNonQuery()
        '    End Using
        'End Using

    End Sub

    Private Sub InsertOpens(ByVal Domain As String, ByVal CampaignId As Integer, ByVal Ip As String)
        Dim intMonth As Integer
        Dim InsertTable As String
        intMonth = DatePart(DateInterval.Month, Date.Now)
        InsertTable = "Impression_" & intMonth

        Response.Write("<br>In INSERT OPENS <BR>Domain " & Domain)
        Response.Write("  Insert Table " & InsertTable)
        Response.Write("  CampaignId  " & CampaignId)
        Response.Write("  Ip   " & Ip)
        'Using cnn As New SqlConnection(strConn)
        '    cnn.Open()
        '    Using cmdInsertOpens As New SqlCommand("sp_AutomatedEmail_AdjustOpens", cnn)
        '        cmdInsertOpens.CommandType = CommandType.StoredProcedure
        '        cmdInsertOpens.Parameters.Add(New SqlParameter("@Domain", SqlDbType.VarChar)).Value = Domain
        '        cmdInsertOpens.Parameters.Add(New SqlParameter("@InsertTable", SqlDbType.VarChar)).Value = InsertTable
        '        cmdInsertOpens.Parameters.Add(New SqlParameter("@CampaignID", SqlDbType.Int)).Value = CampaignId
        '        cmdInsertOpens.Parameters.Add(New SqlParameter("@Ip", SqlDbType.VarChar)).Value = Ip
        '        cmdInsertOpens.ExecuteNonQuery()
        '    End Using
        'End Using
    End Sub
#End Region


#Region "Campaigns In Time Frame"

    Private Sub GetCampaignInTimeFrame()
        arLPhase = New ArrayList
        arLClientIps = New ArrayList
        For x As Integer = 2 To 4
            Select Case x
                Case 2
                    Response.Write("<br><b>PHASE 2</B><BR>")
                    FillPhaseArrays(Phase2StartDate, Phase2EndDate, x)
                    GetNumberOfCampaignsToUse(x)
                    GetRDNClientInfo(x)
                    GetIndividualCampaignNumbers(x)
                    GetCampaignIpsAndLinksAndInsert()

                    'ShowPhaseIds(x)
                    'ShowPhaseClientInfo(x)
                Case 3
                    Response.Write("<br><b>PHASE 3</B><BR>")
                    FillPhaseArrays(Phase3StartDate, Phase3EndDate, x)
                    GetNumberOfCampaignsToUse(x)
                    GetRDNClientInfo(x)
                    GetIndividualCampaignNumbers(x)
                    GetCampaignIpsAndLinksAndInsert()

                    'ShowPhaseIds(x)
                    'ShowPhaseClientInfo(x)
                Case 4
                    Response.Write("<br><b>PHASE 4</B><BR>")
                    FillPhaseArrays(Phase4StartDate, Phase4EndDate, x)
                    GetNumberOfCampaignsToUse(x)
                    GetRDNClientInfo(x)
                    GetIndividualCampaignNumbers(x)
                    GetCampaignIpsAndLinksAndInsert()

                    'ShowPhaseIds(x)
                    'ShowPhaseClientInfo(x)
            End Select
        Next
    End Sub

    Private Sub FillPhaseArrays(ByVal StartDate As Date, ByVal EndDate As Date, ByVal Num As Integer)
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetCampaignInTimeFrame As New SqlCommand("CampaignAdCopy_GetByCreatedOn", cnn)
                cmdGetCampaignInTimeFrame.CommandType = CommandType.StoredProcedure
                cmdGetCampaignInTimeFrame.Parameters.Add(New SqlParameter("@StartDate", Data.SqlDbType.Date)).Value = StartDate
                cmdGetCampaignInTimeFrame.Parameters.Add(New SqlParameter("@EndDate", Data.SqlDbType.Date)).Value = EndDate
                Using dtrGetCampaignInTimeFrame As SqlDataReader = cmdGetCampaignInTimeFrame.ExecuteReader
                    Dim Count As Integer = 0
                    While dtrGetCampaignInTimeFrame.Read
                        Dim MyGuid As Guid = Guid.NewGuid()
                        Dim item As String
                        item = MyGuid.ToString & "|" & dtrGetCampaignInTimeFrame("CampaignId")
                        arLPhase.Add(item)
                    End While
                End Using
            End Using
        End Using
    End Sub

    Private Sub GetRDNClientInfo(ByVal Phase As Integer)
        arLPhase.Sort()
        arRDNPhaseClientInfo = New String(NumberOfPhaseCampaignsToUse - 1, 7) {}
        For x As Integer = 0 To NumberOfPhaseCampaignsToUse - 1
            Dim arlinks() As String = New String() {}
            Dim Id As Integer
            arlinks = arLPhase(x).Split("|")
            Id = arlinks(1)
            SetRDNClientArrayInfo(Id, x, Phase)
        Next
    End Sub

    Private Sub SetRDNClientArrayInfo(ByVal id As Integer, ByVal x As Integer, ByVal Phase As Integer)
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdSetRDNClientArrayInfo As New SqlCommand("sp_AutomatedEmail_GetClientInfo", cnn)
                cmdSetRDNClientArrayInfo.CommandType = CommandType.StoredProcedure
                cmdSetRDNClientArrayInfo.Parameters.Add(New SqlParameter("@Id", Data.SqlDbType.Int)).Value = id
                Using dtrSetRDNClientArrayInfo As SqlDataReader = cmdSetRDNClientArrayInfo.ExecuteReader
                    While dtrSetRDNClientArrayInfo.Read
                        Dim ClientId As Integer = 0
                        Dim DataBase As String = String.Empty
                        If Not IsDBNull(dtrSetRDNClientArrayInfo("Id")) Then
                            ClientId = dtrSetRDNClientArrayInfo("Id")
                        Else
                            ClientId = 0
                        End If
                        If Not IsDBNull(dtrSetRDNClientArrayInfo("DataBase")) Then
                            DataBase = dtrSetRDNClientArrayInfo("DataBase")
                        Else
                            DataBase = "UnKnown"
                        End If
                        arRDNPhaseClientInfo(x, 0) = id
                        arRDNPhaseClientInfo(x, 1) = ClientId
                        arRDNPhaseClientInfo(x, 2) = DataBase
                        GetClientsInfo(DataBase, ClientId, x)
                    End While
                End Using
            End Using
        End Using
    End Sub

    Private Sub GetClientsInfo(ByVal Domain As String, ByVal Id As Integer, ByVal num As Integer)
        If LCase(Domain) <> "unknown" Then
            Using CNN As New SqlConnection(strConn)
                CNN.Open()
                Using cmdGetClientsInfo As New SqlCommand("sp_AutomatedEmail_GetClientsEmailInfo", CNN)
                    cmdGetClientsInfo.CommandType = CommandType.StoredProcedure
                    cmdGetClientsInfo.Parameters.Add(New SqlParameter("@Domain", Data.SqlDbType.VarChar)).Value = Domain
                    cmdGetClientsInfo.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = Id

                    Using dtrGetClientInfo As SqlDataReader = cmdGetClientsInfo.ExecuteReader
                        While dtrGetClientInfo.Read
                            Dim OrderedAmount As Integer = 0
                            Dim Opens As Integer = 0
                            Dim Clicks As Integer = 0

                            If Not IsDBNull(dtrGetClientInfo("EmailsOrdered")) Then
                                OrderedAmount = dtrGetClientInfo("EmailsOrdered")
                            Else
                                OrderedAmount = 0
                            End If
                            If Not IsDBNull(dtrGetClientInfo("Opens")) Then
                                Opens = dtrGetClientInfo("Opens")
                            Else
                                Opens = 0
                            End If

                            If Not IsDBNull(dtrGetClientInfo("Clicks")) Then
                                Clicks = dtrGetClientInfo("Clicks")
                            Else
                                Clicks = 0
                            End If
                            arRDNPhaseClientInfo(num, 3) = OrderedAmount
                            arRDNPhaseClientInfo(num, 4) = Opens
                            arRDNPhaseClientInfo(num, 5) = Clicks
                        End While
                    End Using
                End Using
            End Using
        End If

    End Sub

    Private Sub GetIndividualCampaignNumbers(ByVal Phase As Integer)
        Dim Random As Integer
        Select Case Phase
            Case 2
                PhaseAddMin = Phase2AddMin
                PhaseAddMax = Phase2AddMax
            Case 3
                PhaseAddMin = Phase3AddMin
                PhaseAddMax = Phase3AddMax
            Case 4
                PhaseAddMin = Phase4AddMin
                PhaseAddMax = Phase4AddMax
        End Select

        For x As Integer = 0 To arRDNPhaseClientInfo.GetUpperBound(0)
            Dim WildCard As Boolean = False
            Dim EmailsOrdered As Integer = 0
            Dim ClickRateCount As Integer = 0
            Dim OpenRateCount As Integer = 0
            Dim Opens As Integer = 0
            Dim Clicks As Integer = 0
            Dim HighEnd As Boolean = True
            Dim WildCardCountHigh As Integer = 0
            Dim WildCardCountLow As Integer = 0

            EmailsOrdered = arRDNPhaseClientInfo(x, 3)
            Opens = arRDNPhaseClientInfo(x, 4)
            Clicks = arRDNPhaseClientInfo(x, 5)
            Random = RandomClass.Next(0, 100)

            ClickRate = RandomClass.Next(MaxClickRateMin, MaxClickRateMax)
            ClickRateCount = (EmailsOrdered * ClickRate) / 100
            OpenRate = RandomClass.Next(MaxOpenRateMin, MaxOpenRateMax)
            OpenRateCount = (EmailsOrdered * OpenRate) / 100

            If Random <= WildCardPercent Then
                WildCard = True
                WildCardCountHigh = PhaseAddMax * 2
                WildCardCountLow = Phase4AddMax * 2
            End If

            If ClickRateCount > Clicks Then
                If WildCard = True Then
                    ClickCount = RandomClass.Next(PhaseAddMin, WildCardCountHigh)
                Else
                    ClickCount = RandomClass.Next(PhaseAddMin, PhaseAddMax)
                End If
            Else
                If WildCard = True Then
                    ClickCount = RandomClass.Next(Phase4AddMin, WildCardCountLow)
                Else
                    ClickCount = RandomClass.Next(Phase4AddMin, Phase4AddMax)
                End If
            End If

            If OpenRateCount > Opens Then
                If WildCard = True Then
                    OpenCount = RandomClass.Next(PhaseAddMin, WildCardCountHigh)
                Else
                    OpenCount = RandomClass.Next(PhaseAddMin, PhaseAddMax)
                End If
            Else
                If WildCard = True Then
                    OpenCount = RandomClass.Next(Phase4AddMin, WildCardCountLow)
                Else
                    OpenCount = RandomClass.Next(Phase4AddMin, Phase4AddMax)
                End If
            End If
            arRDNPhaseClientInfo(x, 6) = OpenCount
            arRDNPhaseClientInfo(x, 7) = ClickCount
        Next

    End Sub

    Private Sub GetNumberOfCampaignsToUse(ByVal lnum As Integer)

        Select Case lnum
            Case 2
                NumberOfPhaseCampaignsToUse = (CInt(arLPhase.Count - 1) * CInt(Phase2PercentToUse) \ 100)

            Case 3
                NumberOfPhaseCampaignsToUse = (CInt(arLPhase.Count - 1) * CInt(Phase3PercentToUse) \ 100)
            Case 4
                NumberOfPhaseCampaignsToUse = (CInt(arLPhase.Count - 1) * CInt(Phase4PercentToUse) \ 100)
        End Select
    End Sub

#End Region

    Private Sub SetPhaseDates()

        Phase1EndDayCount = Phase1RandomEndDayCount
        Phase1StartDate = DateAdd(DateInterval.Day, -Phase1EndDayCount, Today())
        Phase1EndDate = Today

        Phase2EndDayCount = Phase1RandomEndDayCount + Phase2RandomEndDayCount
        Phase2StartDate = DateAdd(DateInterval.Day, -Phase2EndDayCount, Today())
        Phase2EndDate = DateAdd(DateInterval.Day, -1, Phase1StartDate)

        Phase3EndDayCount = Phase1RandomEndDayCount + Phase2RandomEndDayCount + Phase3RandomEndDayCount
        Phase3StartDate = DateAdd(DateInterval.Day, -Phase3EndDayCount, Today())
        Phase3EndDate = DateAdd(DateInterval.Day, -1, Phase2StartDate)

        Phase4EndDayCount = Phase1RandomEndDayCount + Phase2RandomEndDayCount + Phase3RandomEndDayCount + Phase4RandomEndDayCount
        Phase4StartDate = DateAdd(DateInterval.Day, -Phase4EndDayCount, Today())
        Phase4EndDate = DateAdd(DateInterval.Day, -1, Phase3StartDate)

        BeginLastMonth = DateAdd("D", -1.0 * DatePart("D", Today) + 1, DateAdd("m", -1, Today))

        If Phase4StartDate < BeginLastMonth Then
            Phase4StartDate = BeginLastMonth
        End If

        'ShowPhaseDateValues()
    End Sub

    Private Sub GetRandomNumbers()
        'OpenRate = RandomClass.Next(MaxOpenRateMin, MaxOpenRateMax)
        'ClickRate = RandomClass.Next(MaxClickRateMin, MaxClickRateMax)
        Phase1RandomEndDayCount = RandomClass.Next(Phase1RandomMinDateRange, Phase1RandomMaxDateRange)
        Phase2RandomEndDayCount = RandomClass.Next(Phase2RandomMinDateRange, Phase2RandomMaxDateRange)
        Phase3RandomEndDayCount = RandomClass.Next(Phase3RandomMinDateRange, Phase3RandomMaxDateRange)
        Phase4RandomEndDayCount = RandomClass.Next(Phase4RandomMinDateRange, Phase4RandomMaxDateRange)
        'ShowRandomValues()

    End Sub





End Class

