Imports System.Collections.Generic
Imports System.Data
Imports System.Net
Imports System.IO
Imports System.Web.Script.Serialization
Imports System.Data.SqlClient

Partial Public Class MyAdCampaignsPullReview
    Inherits System.Web.UI.Page

    Private Const SESSION_KEY As String = "MyAdCampaignsPreviewData"
    Private Const API_USERNAME As String = "goddardent"
    Private Const API_PASSWORD As String = "Porsche2023!1"
    Private ReadOnly LOG_PATH As String = HttpContext.Current.Server.MapPath("~/portal/api_pull_review.log")

    Private Sub LogMessage(msg As String)
        Try
            System.IO.File.AppendAllText(LOG_PATH, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & " - " & msg & Environment.NewLine)
        Catch ex As Exception
            ' Ignore logging errors
        End Try
    End Sub

    Private Function GetApiToken(username As String, password As String) As String
        LogMessage("Authenticating with MyAdCampaigns API as " & username)
        Dim loginUrl As String = "https://login.myadcampaigns.com/advertiser/auth?login=" & username & "&password=" & password
        Dim request As HttpWebRequest = CType(WebRequest.Create(loginUrl), HttpWebRequest)
        request.Method = "GET"
        Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
            Using reader As New StreamReader(response.GetResponseStream())
                Dim json As String = reader.ReadToEnd()
                LogMessage("Auth response: " & json)
                Dim serializer As New JavaScriptSerializer()
                Try
                    ' Try to parse as JSON object
                    Dim authResponse = serializer.DeserializeObject(json)
                    If TypeOf authResponse Is Dictionary(Of String, Object) AndAlso DirectCast(authResponse, Dictionary(Of String, Object)).ContainsKey("token") Then
                        LogMessage("Token found in JSON response.")
                        Return authResponse("token").ToString()
                    End If
                Catch
                    ' If parsing fails, treat as plain string token
                    If Not String.IsNullOrEmpty(json) Then
                        LogMessage("Token found as plain string.")
                        Return json.Trim(""""c) ' Remove quotes if present
                    End If
                End Try
                LogMessage("Token not found in authentication response.")
                Throw New Exception("Token not found in authentication response.")
            End Using
        End Using
    End Function

    ' Shared function to pull campaign/offer data from the API
    Private Function GetCampaignOfferDataTable() As DataTable
        LogMessage("GetCampaignOfferDataTable started.")
        Dim token As String = GetApiToken(API_USERNAME, API_PASSWORD)
        LogMessage("Token acquired: " & token)
        Dim dt As New DataTable()
        dt.Columns.Add("cid", GetType(Integer))
        dt.Columns.Add("campaign_name", GetType(String))
        dt.Columns.Add("offer_id", GetType(Integer))
        dt.Columns.Add("offer_name", GetType(String))
        dt.Columns.Add("start_date", GetType(String))
        dt.Columns.Add("end_date", GetType(String))

        ' Get last 4 weeks (28 days) from today
        Dim today As Date = Date.Today
        Dim startDate As Date = today.AddDays(-27) ' 4 weeks ago
        Dim endDate As Date = today
        Dim weekStart As Date = startDate
        While weekStart <= endDate
            Dim weekEnd As Date = weekStart.AddDays(6)
            If weekEnd > endDate Then weekEnd = endDate
            Dim url As String = "https://login.myadcampaigns.com/advertiser/api/AdvertiserReports/campaign,offer?fields=campaign_id,campaign_name,offer_id,offer_name,start_date,end_date&filters=date:" & weekStart.ToString("yyyy-MM-dd") & "_" & weekEnd.ToString("yyyy-MM-dd") & "&version=4&token=" & token
            LogMessage("Requesting API: " & url)
            Dim request As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
            request.Method = "GET"
            Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                Using reader As New StreamReader(response.GetResponseStream())
                    Dim json As String = reader.ReadToEnd()
                    LogMessage("API response: " & json.Substring(0, Math.Min(500, json.Length)))
                    Dim serializer As New JavaScriptSerializer()
                    Dim apiResponse = serializer.DeserializeObject(json)
                    ' Parse API response
                    If TypeOf apiResponse Is Dictionary(Of String, Object) AndAlso DirectCast(apiResponse, Dictionary(Of String, Object)).ContainsKey("response") Then
                        Dim responseDict = DirectCast(apiResponse("response"), Dictionary(Of String, Object))
                        If responseDict.ContainsKey("list") Then
                            Dim listDict = DirectCast(responseDict("list"), Dictionary(Of String, Object))
                            If listDict.ContainsKey("rows") Then
                                Dim rowsDict = DirectCast(listDict("rows"), Dictionary(Of String, Object))
                                Dim weekRowCount As Integer = 0
                                For Each entry In rowsDict.Values
                                    If weekRowCount >= 200 Then Exit For
                                    Dim campaign = DirectCast(entry, Dictionary(Of String, Object))
                                    Dim cid As Integer = If(campaign.ContainsKey("campaign_id"), Convert.ToInt32(campaign("campaign_id")), 0)
                                    Dim campaignName As String = If(campaign.ContainsKey("campaign_name"), campaign("campaign_name").ToString(), "")
                                    Dim offerId As Integer = If(campaign.ContainsKey("offer_id"), Convert.ToInt32(campaign("offer_id")), 0)
                                    Dim offerName As String = If(campaign.ContainsKey("offer_name"), campaign("offer_name").ToString(), "")
                                    Dim startDateStr As String = If(campaign.ContainsKey("start_date"), campaign("start_date").ToString(), "")
                                    Dim endDateStr As String = If(campaign.ContainsKey("end_date"), campaign("end_date").ToString(), "")
                                    dt.Rows.Add(cid, campaignName, offerId, offerName, startDateStr, endDateStr)
                                    weekRowCount += 1
                                Next
                            End If
                        End If
                    End If
                End Using
            End Using
            weekStart = weekStart.AddDays(7)
        End While
        LogMessage("GetCampaignOfferDataTable finished. Rows: " & dt.Rows.Count)
        Return dt
    End Function

    Protected Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click
        LogMessage("btnView_Click started.")
        Try
            Dim dt As DataTable = GetCampaignOfferDataTable()
            Dim diagMsg As String = "[DIAG] btnView_Click: Pulled DataTable with " & dt.Rows.Count & " rows."
            lblStatus.Text = diagMsg
            LogMessage(diagMsg)
            lblStatus.Text &= "<br/>SessionID: " & Session.SessionID
            LogMessage("SessionID: " & Session.SessionID)
            gridPreview.DataSource = dt
            gridPreview.DataBind()
            btnSubmit.Enabled = (dt.Rows.Count > 0)
            lblStatus.Text &= If(dt.Rows.Count > 0, " Preview loaded. Review and click Submit to import.", " No data to preview.")
            LogMessage("Preview loaded. Rows: " & dt.Rows.Count)
        Catch ex As Exception
            gridPreview.DataSource = Nothing
            gridPreview.DataBind()
            btnSubmit.Enabled = False
            lblStatus.Text = "Error: " & Server.HtmlEncode(ex.Message)
            LogMessage("Error: " & ex.ToString())
            lblStatus.Text &= "<br/>SessionID: " & Session.SessionID
            LogMessage("SessionID: " & Session.SessionID)
        End Try
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        LogMessage("btnSubmit_Click started.")
        Try
            ' Set up date range (last 4 weeks)
            Dim today As Date = Date.Today
            Dim startDate As Date = today.AddDays(-27) ' 4 weeks ago
            Dim endDate As Date = today
            Dim totalInserted As Integer = 0
            Dim weekStart As Date = startDate
            Dim connStr As String = System.Configuration.ConfigurationManager.ConnectionStrings("lga4040ConnectionString").ConnectionString
            Using conn As New SqlConnection(connStr)
                conn.Open()
                While weekStart <= endDate
                    Dim weekEnd As Date = weekStart.AddDays(6)
                    If weekEnd > endDate Then weekEnd = endDate
                    ' Pull records for this week only, with pagination
                    Dim token As String = GetApiToken(API_USERNAME, API_PASSWORD)
                    Dim dt As New DataTable()
                    dt.Columns.Add("cid", GetType(Integer))
                    dt.Columns.Add("campaign_name", GetType(String))
                    dt.Columns.Add("offer_id", GetType(Integer))
                    dt.Columns.Add("offer_name", GetType(String))
                    dt.Columns.Add("start_date", GetType(String))
                    dt.Columns.Add("end_date", GetType(String))
                    Dim page As Integer = 1
                    Dim hasMore As Boolean = True
                    Dim lastFirstKey As String = ""
                    While hasMore
                        Dim url As String = "https://login.myadcampaigns.com/advertiser/api/AdvertiserReports/campaign,offer?fields=campaign_id,campaign_name,offer_id,offer_name,start_date,end_date&filters=date:" & weekStart.ToString("yyyy-MM-dd") & "_" & weekEnd.ToString("yyyy-MM-dd") & "&version=4&token=" & token & "&limit=200&page=" & page
                        LogMessage("Requesting API: " & url)
                        Dim request As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
                        request.Method = "GET"
                        Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                            Using reader As New StreamReader(response.GetResponseStream())
                                Dim json As String = reader.ReadToEnd()
                                LogMessage("API response: " & json.Substring(0, Math.Min(500, json.Length)))
                                Dim serializer As New JavaScriptSerializer()
                                Dim apiResponse = serializer.DeserializeObject(json)
                                If TypeOf apiResponse Is Dictionary(Of String, Object) AndAlso DirectCast(apiResponse, Dictionary(Of String, Object)).ContainsKey("response") Then
                                    Dim responseDict = DirectCast(apiResponse("response"), Dictionary(Of String, Object))
                                    If responseDict.ContainsKey("list") Then
                                        Dim listDict = DirectCast(responseDict("list"), Dictionary(Of String, Object))
                                        If listDict.ContainsKey("rows") Then
                                            Dim rowsDict = DirectCast(listDict("rows"), Dictionary(Of String, Object))
                                            Dim rowCount As Integer = 0
                                            Dim firstKey As String = ""
                                            For Each entry In rowsDict.Values
                                                Dim campaign = DirectCast(entry, Dictionary(Of String, Object))
                                                Dim cid As Integer = If(campaign.ContainsKey("campaign_id"), Convert.ToInt32(campaign("campaign_id")), 0)
                                                Dim offerId As Integer = If(campaign.ContainsKey("offer_id"), Convert.ToInt32(campaign("offer_id")), 0)
                                                If rowCount = 0 Then
                                                    firstKey = cid.ToString() & "|" & offerId.ToString()
                                                End If
                                                Dim campaignName As String = If(campaign.ContainsKey("campaign_name"), campaign("campaign_name").ToString(), "")
                                                Dim offerName As String = If(campaign.ContainsKey("offer_name"), campaign("offer_name").ToString(), "")
                                                Dim startDateStr As String = If(campaign.ContainsKey("start_date"), campaign("start_date").ToString(), "")
                                                Dim endDateStr As String = If(campaign.ContainsKey("end_date"), campaign("end_date").ToString(), "")
                                                dt.Rows.Add(cid, campaignName, offerId, offerName, startDateStr, endDateStr)
                                                rowCount += 1
                                            Next
                                            LogMessage("FirstKey for page " & page & ": " & firstKey)
                                            If firstKey = lastFirstKey AndAlso firstKey <> "" Then
                                                LogMessage("Breaking pagination loop: firstKey repeated from previous page.")
                                                hasMore = False
                                            ElseIf rowCount < 200 Then
                                                hasMore = False
                                            Else
                                                lastFirstKey = firstKey
                                                page += 1
                                            End If
                                        Else
                                            hasMore = False
                                        End If
                                    Else
                                        hasMore = False
                                    End If
                                Else
                                    hasMore = False
                                End If
                            End Using
                        End Using
                    End While
                    ' Build HashSets for this week
                    Dim existingCampaigns As New HashSet(Of String)()
                    Using cmd As New SqlCommand("SELECT cid, start_date, end_date FROM MyAdCampaigns WHERE start_date >= @startDate AND end_date <= @endDate", conn)
                        cmd.Parameters.AddWithValue("@startDate", weekStart)
                        cmd.Parameters.AddWithValue("@endDate", weekEnd)
                        Using reader As SqlDataReader = cmd.ExecuteReader()
                            While reader.Read()
                                Dim key As String = reader("cid").ToString() & "|" & Convert.ToDateTime(reader("start_date")).ToString("yyyy-MM-dd") & "|" & Convert.ToDateTime(reader("end_date")).ToString("yyyy-MM-dd")
                                existingCampaigns.Add(key)
                            End While
                        End Using
                    End Using
                    ' Only use cid and offer_id for offers
                    Dim existingOffers As New HashSet(Of String)()
                    Using cmd As New SqlCommand("SELECT cid, offer_id FROM CampaignOffers", conn)
                        Using reader As SqlDataReader = cmd.ExecuteReader()
                            While reader.Read()
                                Dim key As String = reader("cid").ToString() & "|" & reader("offer_id").ToString()
                                existingOffers.Add(key)
                            End While
                        End Using
                    End Using
                    Dim inserted As Integer = 0
                    For Each row As DataRow In dt.Rows
                        Dim cid As Integer = Convert.ToInt32(row("cid"))
                        Dim offerId As Integer = Convert.ToInt32(row("offer_id"))
                        Dim startDateObj As Object = If(dt.Columns.Contains("start_date"), row("start_date"), DBNull.Value)
                        Dim endDateObj As Object = If(dt.Columns.Contains("end_date"), row("end_date"), DBNull.Value)
                        Dim createdObj As Object = DateTime.Now
                        Dim startDateStr As String = If(IsDBNull(startDateObj) OrElse startDateObj Is Nothing, "", startDateObj.ToString())
                        Dim endDateStr As String = If(IsDBNull(endDateObj) OrElse endDateObj Is Nothing, "", endDateObj.ToString())
                        Dim campaignKey As String = cid.ToString() & "|" & startDateStr & "|" & endDateStr
                        If Not existingCampaigns.Contains(campaignKey) Then
                            Using cmdCampaign As New SqlCommand("UpsertMyAdCampaign", conn)
                                cmdCampaign.CommandType = CommandType.StoredProcedure
                                cmdCampaign.Parameters.AddWithValue("@cid", cid)
                                ' Safely parse start_date
                                Dim parsedStartDate As Object = DBNull.Value
                                Dim tempStartDate As DateTime
                                If Not String.IsNullOrWhiteSpace(startDateStr) AndAlso DateTime.TryParse(startDateStr, tempStartDate) Then
                                    parsedStartDate = tempStartDate
                                End If
                                cmdCampaign.Parameters.AddWithValue("@start_date", parsedStartDate)
                                ' Safely parse end_date
                                Dim parsedEndDate As Object = DBNull.Value
                                Dim tempEndDate As DateTime
                                If Not String.IsNullOrWhiteSpace(endDateStr) AndAlso DateTime.TryParse(endDateStr, tempEndDate) Then
                                    parsedEndDate = tempEndDate
                                End If
                                cmdCampaign.Parameters.AddWithValue("@end_date", parsedEndDate)
                                cmdCampaign.Parameters.AddWithValue("@created", createdObj)
                                cmdCampaign.ExecuteNonQuery()
                                LogMessage("Upserted campaign: cid=" & cid & ", start_date=" & startDateStr & ", end_date=" & endDateStr & ", created=" & createdObj)
                            End Using
                            existingCampaigns.Add(campaignKey)
                        End If
                        ' Only use cid and offer_id for offers
                        Dim offerKey As String = cid.ToString() & "|" & offerId.ToString()
                        If Not existingOffers.Contains(offerKey) Then
                            Using cmdOffer As New SqlCommand("UpsertCampaignOffer", conn)
                                cmdOffer.CommandType = CommandType.StoredProcedure
                                cmdOffer.Parameters.AddWithValue("@cid", cid)
                                cmdOffer.Parameters.AddWithValue("@offer_id", offerId)
                                cmdOffer.ExecuteNonQuery()
                                LogMessage("Upserted offer: cid=" & cid & ", offer_id=" & offerId)
                            End Using
                            existingOffers.Add(offerKey)
                        End If
                        inserted += 1
                    Next
                    totalInserted += inserted
                    LogMessage("Week " & weekStart.ToString("yyyy-MM-dd") & " to " & weekEnd.ToString("yyyy-MM-dd") & ": Inserted/checked " & inserted & " rows.")
                weekStart = weekStart.AddDays(7)
                End While
            End Using
            lblStatus.Text &= " Data submitted to database. Inserted/checked " & totalInserted & " rows."
            LogMessage("Submit complete. Total rows processed: " & totalInserted)
        Catch ex As Exception
            lblStatus.Text &= " Error: " & Server.HtmlEncode(ex.Message)
            LogMessage("Submit error: " & ex.ToString())
        End Try
        gridPreview.DataSource = Nothing
        gridPreview.DataBind()
        btnSubmit.Enabled = False
    End Sub

End Class 