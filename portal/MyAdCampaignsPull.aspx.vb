Imports System.Data.SqlClient
Imports System.Net
Imports System.IO
Imports System.Web.Script.Serialization
Imports System.Configuration
Imports System.Collections.Generic
Imports System.Data

Partial Class MyAdCampaignsPull
    Inherits System.Web.UI.Page

    Private Function GetApiToken(username As String, password As String) As String
        Dim loginUrl As String = "https://login.myadcampaigns.com/advertiser/auth?login=" & username & "&password=" & password
        Dim request As HttpWebRequest = CType(WebRequest.Create(loginUrl), HttpWebRequest)
        request.Method = "GET"
        Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
            Using reader As New StreamReader(response.GetResponseStream())
                Dim json As String = reader.ReadToEnd()
                Dim serializer As New JavaScriptSerializer()
                Dim authResponse = serializer.DeserializeObject(json)
                If TypeOf authResponse Is Dictionary(Of String, Object) AndAlso DirectCast(authResponse, Dictionary(Of String, Object)).ContainsKey("token") Then
                    Return authResponse("token").ToString()
                End If
                Throw New Exception("Token not found in authentication response.")
            End Using
        End Using
    End Function

    Protected Sub btnPull_Click(sender As Object, e As EventArgs)
        ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType) ' TLS 1.2
        Dim username As String = "goddardent"
        Dim password As String = "Porsche2023!1"
        Dim connStr As String = ConfigurationManager.ConnectionStrings("lga4040ConnectionString").ConnectionString
        Dim logPath As String = Server.MapPath("~/portal/api_pull.log")
        Dim startDate As Date = New Date(2025, 5, 1)
        Dim endDate As Date = New Date(2025, 5, 31)
        Dim token As String = GetApiToken(username, password)
        Dim totalInserted As Integer = 0
        File.AppendAllText(logPath, DateTime.Now.ToString() & " - Pull started. Date range: " & startDate.ToString("yyyy-MM-dd") & " to " & endDate.ToString("yyyy-MM-dd") & Environment.NewLine)
        Dim currentStart As Date = startDate
        While currentStart <= endDate
            Dim currentEnd As Date = currentStart.AddDays(6)
            If currentEnd > endDate Then currentEnd = endDate
            Dim url As String = "https://login.myadcampaigns.com/advertiser/api/AdvertiserReports/campaign,offer?filters=date:" & currentStart.ToString("yyyy-MM-dd") & "_" & currentEnd.ToString("yyyy-MM-dd") & "&version=4&token=" & token
            File.AppendAllText(logPath, DateTime.Now.ToString() & " - API request: " & url & Environment.NewLine)
            Try
                Dim request As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
                request.Method = "GET"
                Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                    Using reader As New StreamReader(response.GetResponseStream())
                        Dim json As String = reader.ReadToEnd()
                        File.AppendAllText(logPath, DateTime.Now.ToString() & " - API response received. Length: " & json.Length & Environment.NewLine)
                        Dim serializer As New JavaScriptSerializer()
                        Dim apiResponse = serializer.DeserializeObject(json)
                        If TypeOf apiResponse Is Dictionary(Of String, Object) AndAlso DirectCast(apiResponse, Dictionary(Of String, Object)).ContainsKey("response") Then
                            Dim responseDict = DirectCast(apiResponse("response"), Dictionary(Of String, Object))
                            If responseDict.ContainsKey("list") Then
                                Dim listDict = DirectCast(responseDict("list"), Dictionary(Of String, Object))
                                If listDict.ContainsKey("rows") Then
                                    Dim rowsDict = DirectCast(listDict("rows"), Dictionary(Of String, Object))
                                    Using conn As New SqlConnection(connStr)
                                        conn.Open()
                                        For Each entry In rowsDict.Values
                                            Dim campaign = DirectCast(entry, Dictionary(Of String, Object))
                                            Dim cid As Integer = If(campaign.ContainsKey("campaign_id"), Convert.ToInt32(campaign("campaign_id")), 0)
                                            Dim offerId As Integer = If(campaign.ContainsKey("offer_id"), Convert.ToInt32(campaign("offer_id")), 0)
                                            Dim cmd As New SqlCommand("IF NOT EXISTS (SELECT 1 FROM MyAdCampaigns WHERE cid = @cid AND offer_id = @offer_id) INSERT INTO MyAdCampaigns (cid, offer_id) VALUES (@cid, @offer_id)", conn)
                                            cmd.Parameters.AddWithValue("@cid", cid)
                                            cmd.Parameters.AddWithValue("@offer_id", offerId)
                                            cmd.ExecuteNonQuery()
                                            File.AppendAllText(logPath, DateTime.Now.ToString() & " - Upserted cid: " & cid & ", offer_id: " & offerId & Environment.NewLine)
                                            totalInserted += 1
                                        Next
                                        conn.Close()
                                    End Using
                                End If
                            End If
                        End If
                    End Using
                End Using
            Catch ex As Exception
                File.AppendAllText(logPath, DateTime.Now.ToString() & " - ERROR: " & ex.ToString() & Environment.NewLine)
                lblStatus.Text = "Error: " & ex.Message
                Exit While
            End Try
            currentStart = currentEnd.AddDays(1)
        End While
        File.AppendAllText(logPath, DateTime.Now.ToString() & " - Pull finished. Total inserted/updated: " & totalInserted & Environment.NewLine)
        lblStatus.Text = "Inserted/Updated " & totalInserted & " campaigns."
    End Sub
End Class 