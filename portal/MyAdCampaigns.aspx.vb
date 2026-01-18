Imports System.Collections.Generic
Imports System.Net
Imports System.IO
Imports System.Web.Script.Serialization
Imports System.Data
Imports System.Globalization

Partial Class Portal_MyAdCampaigns
    Inherits System.Web.UI.Page

    Private Function DictionariesToDataTable(dicts As List(Of Dictionary(Of String, Object))) As DataTable
        Dim dt As New DataTable()
        Dim logPath As String = Server.MapPath("~/portal/api_error.log")
        If dicts Is Nothing OrElse dicts.Count = 0 Then Return dt
        ' Collect all unique keys from all dictionaries
        Dim allKeys As New HashSet(Of String)()
        For Each dict In dicts
            If dict Is Nothing Then
                File.AppendAllText(logPath, DateTime.Now.ToString() & " - Null dictionary found in DictionariesToDataTable." & Environment.NewLine)
                Continue For
            End If
            For Each key In dict.Keys
                allKeys.Add(key)
            Next
        Next
        ' Add columns
        For Each key In allKeys
            If Not dt.Columns.Contains(key) Then
                dt.Columns.Add(key)
            End If
        Next
        ' Add rows
        For Each dict In dicts
            If dict Is Nothing Then
                File.AppendAllText(logPath, DateTime.Now.ToString() & " - Null dictionary found in DictionariesToDataTable (row population)." & Environment.NewLine)
                Continue For
            End If
            Dim row = dt.NewRow()
            For Each key In allKeys
                Try
                    If dict.ContainsKey(key) AndAlso dict(key) IsNot Nothing Then
                        row(key) = dict(key).ToString()
                    Else
                        row(key) = ""
                    End If
                Catch ex As Exception
                    File.AppendAllText(logPath, DateTime.Now.ToString() & " - Exception in row population for key '" & key & "': " & ex.ToString() & Environment.NewLine)
                    row(key) = ""
                End Try
            Next
            dt.Rows.Add(row)
        Next
        Return dt
    End Function

    Protected Sub btnPullApiCampaigns_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPullApiCampaigns.Click
        ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType) ' TLS 1.2
        Dim userToken As String = "vIXoR3cvrwrS35bU"
        Dim url As String = "https://login.myadcampaigns.com/advertiser/api/Campaign/?userToken=" & userToken
        Dim logPathGrid As String = Server.MapPath("~/portal/api_error.log")
        File.AppendAllText(logPathGrid, DateTime.Now.ToString() & " - Button clicked. Starting API call." & Environment.NewLine)
        If gridApiCampaigns Is Nothing Then
            File.AppendAllText(logPathGrid, DateTime.Now.ToString() & " - gridApiCampaigns is Nothing before test property set." & Environment.NewLine)
            Return
        End If
        Try
            File.AppendAllText(logPathGrid, DateTime.Now.ToString() & " - Preparing API request: " & url & Environment.NewLine)
            Dim request As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
            request.Method = "GET"
            Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                File.AppendAllText(logPathGrid, DateTime.Now.ToString() & " - API response received. Status: " & response.StatusCode.ToString() & Environment.NewLine)
                Using reader As New StreamReader(response.GetResponseStream())
                    Dim json As String = reader.ReadToEnd()
                    Dim serializer As New JavaScriptSerializer()
                    Dim campaigns As Object = Nothing
                    Try
                        campaigns = serializer.DeserializeObject(json)
                        File.AppendAllText(logPathGrid, DateTime.Now.ToString() & " - JSON deserialization successful." & Environment.NewLine)
                    Catch deserEx As Exception
                        File.AppendAllText(logPathGrid, DateTime.Now.ToString() & " - JSON deserialization failed: " & deserEx.ToString() & Environment.NewLine)
                        Throw
                    End Try
                    Dim filtered As New System.Collections.Generic.List(Of Dictionary(Of String, Object))()
                    Dim includedIds As New List(Of String)()
                    Dim excludedIds As New List(Of String)()
                    If TypeOf campaigns Is Dictionary(Of String, Object) Then
                        Dim campaignsDict = DirectCast(campaigns, Dictionary(Of String, Object))
                        If campaignsDict.ContainsKey("response") AndAlso TypeOf campaignsDict("response") Is Dictionary(Of String, Object) Then
                            Dim responseDict = DirectCast(campaignsDict("response"), Dictionary(Of String, Object))
                            Dim hasStartDate As Boolean = dpStartDate.DateValue <> Date.MinValue
                            Dim hasEndDate As Boolean = dpEndDate.DateValue <> Date.MinValue
                            Dim startDateFilter As DateTime = dpStartDate.DateValue
                            Dim endDateFilter As DateTime = dpEndDate.DateValue
                            Dim isActiveFilter As String = ddlIsActive.SelectedValue
                            Dim pricingModelFilter As String = ddlPricingModel.SelectedValue
                            File.AppendAllText(logPathGrid, DateTime.Now.ToString() & " - Filtering campaigns. hasStartDate: " & hasStartDate.ToString() & ", hasEndDate: " & hasEndDate.ToString() & ", isActiveFilter: " & isActiveFilter & ", pricingModelFilter: " & pricingModelFilter & Environment.NewLine)
                            For Each entry In responseDict
                                If entry.Value Is Nothing Then
                                    File.AppendAllText(logPathGrid, DateTime.Now.ToString() & " - Null entry.Value in response for key: " & entry.Key & Environment.NewLine)
                                ElseIf TypeOf entry.Value Is Dictionary(Of String, Object) Then
                                    Dim dict = CType(entry.Value, Dictionary(Of String, Object))
                                    Dim startDateStr As String = If(dict.ContainsKey("start_date") AndAlso dict("start_date") IsNot Nothing, dict("start_date").ToString(), "")
                                    Dim endDateStr As String = If(dict.ContainsKey("end_date") AndAlso dict("end_date") IsNot Nothing, dict("end_date").ToString(), "")
                                    Dim campaignStart, campaignEnd As DateTime
                                    Dim dateFormat As String = "yyyy-MM-dd"
                                    Dim provider As System.Globalization.CultureInfo = System.Globalization.CultureInfo.InvariantCulture
                                    Dim validStart = DateTime.TryParseExact(startDateStr, dateFormat, provider, System.Globalization.DateTimeStyles.None, campaignStart)
                                    Dim validEnd = DateTime.TryParseExact(endDateStr, dateFormat, provider, System.Globalization.DateTimeStyles.None, campaignEnd)
                                    Dim include As Boolean = True

                                    ' Only filter if both filter dates are set and campaign dates are valid
                                    If hasStartDate AndAlso hasEndDate AndAlso validStart AndAlso validEnd Then
                                        ' Overlap logic: include if campaignEnd >= filterStart AND campaignStart <= filterEnd
                                        If campaignEnd < startDateFilter OrElse campaignStart > endDateFilter Then include = False
                                    Else
                                        ' Fallback to old logic if only one filter is set
                                        If hasStartDate AndAlso validEnd Then
                                            If campaignEnd < startDateFilter Then include = False
                                        End If
                                        If hasEndDate AndAlso validStart Then
                                            If campaignStart > endDateFilter Then include = False
                                        End If
                                    End If
                                    If isActiveFilter <> "" AndAlso dict.ContainsKey("is_active") Then
                                        Dim isActiveObj = dict("is_active")
                                        Dim isActiveVal As Boolean = False
                                        If TypeOf isActiveObj Is Boolean Then
                                            isActiveVal = CType(isActiveObj, Boolean)
                                        ElseIf isActiveObj IsNot Nothing Then
                                            Boolean.TryParse(isActiveObj.ToString(), isActiveVal)
                                        End If
                                        Dim filterVal As Boolean = (isActiveFilter.ToLower() = "true")
                                        If isActiveVal <> filterVal Then include = False
                                    End If
                                    If pricingModelFilter <> "" AndAlso dict.ContainsKey("pricing_model") Then
                                        Dim pricingModelObj = dict("pricing_model")
                                        Dim pricingModelStr = If(pricingModelObj IsNot Nothing, pricingModelObj.ToString().ToUpper(), "")
                                        If pricingModelStr <> pricingModelFilter.ToUpper() Then include = False
                                    End If
                                    If include Then
                                        filtered.Add(dict)
                                        includedIds.Add(dict("id").ToString())
                                    Else
                                        excludedIds.Add(dict("id").ToString())
                                    End If
                                Else
                                    File.AppendAllText(logPathGrid, DateTime.Now.ToString() & " - Unexpected value type in response: " & entry.Value.ToString() & Environment.NewLine)
                                End If
                            Next
                            File.AppendAllText(logPathGrid, DateTime.Now.ToString() & " - Included campaign IDs: " & String.Join(",", includedIds) & Environment.NewLine)
                            File.AppendAllText(logPathGrid, DateTime.Now.ToString() & " - Excluded campaign IDs: " & String.Join(",", excludedIds) & Environment.NewLine)
                            File.AppendAllText(logPathGrid, DateTime.Now.ToString() & " - Filtering complete. Filtered count: " & filtered.Count.ToString() & Environment.NewLine)
                        End If
                    ElseIf TypeOf campaigns Is String Then
                        File.AppendAllText(logPathGrid, DateTime.Now.ToString() & " - API returned string: " & campaigns.ToString() & Environment.NewLine)
                    End If
                    File.AppendAllText(logPathGrid, DateTime.Now.ToString() & " - About to set DataSource. Filtered count: " & filtered.Count.ToString() & Environment.NewLine)
                    If filtered.Count > 0 Then
                        gridApiCampaigns.DataSource = DictionariesToDataTable(filtered)
                    Else
                        gridApiCampaigns.DataSource = Nothing
                    End If
                    File.AppendAllText(logPathGrid, DateTime.Now.ToString() & " - About to call DataBind." & Environment.NewLine)
                    gridApiCampaigns.DataBind()
                    File.AppendAllText(logPathGrid, DateTime.Now.ToString() & " - DataBind completed successfully." & Environment.NewLine)
                End Using
            End Using
        Catch ex As Exception
            Try
                File.AppendAllText(logPathGrid, DateTime.Now.ToString() & " - Exception in btnPullApiCampaigns_Click: " & ex.ToString() & Environment.NewLine)
            Catch logEx As Exception
                ' Ignore logging errors
            End Try
            If gridApiCampaigns IsNot Nothing Then
                gridApiCampaigns.DataSource = Nothing
                gridApiCampaigns.DataBind()
            End If
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' Set default date pickers to current month
            dpStartDate.DateValue = New DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)
            dpEndDate.DateValue = New DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month))
        End If
    End Sub
End Class 