Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic

Partial Class Portal_ShortedCampaigns
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Private strRTConn As String = ConfigurationSettings.AppSettings("trconstr")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GetClients()
            PopulateMonths()
        End If
    End Sub

    Private Sub GetClients()
        Dim sqlstr As String = "SELECT * FROM Client ORDER BY ClientName"
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetClients As New SqlCommand(sqlstr, cnn)
                Using dtrGetClients As SqlDataReader = cmdGetClients.ExecuteReader
                    ddlClient.DataSource = dtrGetClients
                    ddlClient.DataTextField = "ClientName"
                    ddlClient.DataValueField = "ClientUId"
                    ddlClient.DataBind()
                End Using
            End Using
        End Using
        ddlClient.Items.Insert(0, New ListItem("Select Client", ""))
    End Sub

    Private Sub PopulateMonths()
        ddlMonth.Items.Clear()
        ddlMonth.Items.Add(New ListItem("Select Month", ""))
        For i As Integer = 1 To 12
            ddlMonth.Items.Add(New ListItem(MonthName(i), i.ToString()))
        Next
    End Sub

    Protected Sub cmdGetReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGetReport.Click
        If ddlClient.SelectedIndex > 0 AndAlso ddlMonth.SelectedIndex > 0 Then
            ' Lookup the database name for the selected client
            Dim databaseName As String = GetDatabaseNameForClient(ddlClient.SelectedValue)
            If Not String.IsNullOrEmpty(databaseName) Then
                LoadShortedCampaigns(ddlClient.SelectedValue, ddlMonth.SelectedValue, databaseName)
            Else
                gridShortedCampaigns.DataSource = Nothing
                gridShortedCampaigns.DataBind()
            End If
        Else
            gridShortedCampaigns.DataSource = Nothing
            gridShortedCampaigns.DataBind()
        End If
    End Sub

    Private Function GetDatabaseNameForClient(clientUId As String) As String
        Dim dbName As String = String.Empty
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmd As New SqlCommand("SELECT [DataBase] FROM [ClientCompany] WHERE [ClientUId] = @ClientUId", cnn)
                cmd.Parameters.Add(New SqlParameter("@ClientUId", SqlDbType.UniqueIdentifier)).Value = New Guid(clientUId)
                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing AndAlso Not Convert.IsDBNull(result) Then
                    dbName = result.ToString()
                End If
            End Using
        End Using
        Return dbName
    End Function

    Private Function BuildConnectionStringForDatabase(databaseName As String) As String
        Dim builder As New SqlConnectionStringBuilder(strConn)
        builder.InitialCatalog = databaseName
        Return builder.ConnectionString
    End Function

    Protected Sub gridShortedCampaigns_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        Dim dt As DataTable = TryCast(gridShortedCampaigns.DataSource, DataTable)
        If dt Is Nothing AndAlso gridShortedCampaigns.DataSource Is Nothing AndAlso Not IsNothing(ViewState("ShortedCampaignsData")) Then
            dt = CType(ViewState("ShortedCampaignsData"), DataTable)
        End If
        If dt IsNot Nothing Then
            Dim sortDirection As String = GetSortDirection(e.SortExpression)
            dt.DefaultView.Sort = e.SortExpression & " " & sortDirection
            gridShortedCampaigns.DataSource = dt
            gridShortedCampaigns.DataBind()
        End If
    End Sub

    Private Function GetSortDirection(ByVal column As String) As String
        Dim sortDirection As String = "ASC"
        Dim sortExpression As String = TryCast(ViewState("SortExpression"), String)
        If sortExpression IsNot Nothing Then
            If sortExpression = column Then
                Dim lastDirection As String = TryCast(ViewState("SortDirection"), String)
                If lastDirection IsNot Nothing AndAlso lastDirection = "ASC" Then
                    sortDirection = "DESC"
                End If
            End If
        End If
        ViewState("SortDirection") = sortDirection
        ViewState("SortExpression") = column
        Return sortDirection
    End Function

    Private Function GetDomainForClient(clientUId As String) As String
        Dim domain As String = String.Empty
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmd As New SqlCommand("SELECT [Domain] FROM [ClientCompany] WHERE [ClientUId] = @ClientUId", cnn)
                cmd.Parameters.Add(New SqlParameter("@ClientUId", SqlDbType.UniqueIdentifier)).Value = New Guid(clientUId)
                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing AndAlso Not Convert.IsDBNull(result) Then
                    domain = result.ToString()
                End If
            End Using
        End Using
        Return domain
    End Function

    Private Sub LoadShortedCampaigns(ByVal clientUId As String, ByVal month As String, ByVal databaseName As String)
        Dim ds As New DataSet()
        Dim domainConnStr As String = BuildConnectionStringForDatabase(databaseName)
        Using cnn As New SqlConnection(domainConnStr)
            cnn.Open()
            Using cmd As New SqlCommand("sp_ShortedCampaigns", cnn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add(New SqlParameter("@Month", SqlDbType.Int)).Value = Integer.Parse(month)
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(ds)
            End Using
        End Using
        If ds.Tables.Count > 0 Then
            Dim dt As DataTable = ds.Tables(0)
            If dt.Columns.Contains("RdnCampaignId") = False Then
                dt.Columns.Add("RdnCampaignId", GetType(Integer))
            End If
            If dt.Columns.Contains("Domain") = False Then
                dt.Columns.Add("Domain", GetType(String))
            End If

            ' Get the domain for the selected client
            Dim domainValue As String = GetDomainForClient(clientUId)

            ' Load all CampaignId/Link mappings from rdn2020.Campaign into a dictionary
            Dim campaignMap As New Dictionary(Of String, Integer)()
            Using cnnRdn As New SqlConnection(strConn)
                cnnRdn.Open()
                Using cmd As New SqlCommand("SELECT CampaignId, Link FROM Campaign WHERE Link LIKE '%?c=%&%'", cnnRdn)
                    Using rdr As SqlDataReader = cmd.ExecuteReader()
                        While rdr.Read()
                            Dim link As String = rdr("Link").ToString()
                            Dim rdnCampaignId As Integer = Convert.ToInt32(rdr("CampaignId"))
                            ' Extract the domain campaign id from the link
                            Dim cIndex As Integer = link.IndexOf("?c=")
                            If cIndex >= 0 Then
                                Dim startIdx As Integer = cIndex + 3
                                Dim endIdx As Integer = link.IndexOf("&", startIdx)
                                If endIdx > startIdx Then
                                    Dim domainCampaignId As String = link.Substring(startIdx, endIdx - startIdx)
                                    If Not campaignMap.ContainsKey(domainCampaignId) Then
                                        campaignMap.Add(domainCampaignId, rdnCampaignId)
                                    End If
                                End If
                            End If
                        End While
                    End Using
                End Using
            End Using

            ' For each row in your DataTable, assign the RdnCampaignId and Domain
            For Each row As DataRow In dt.Rows
                Dim domainCampaignId As String = row("CampaignId").ToString()
                If campaignMap.ContainsKey(domainCampaignId) Then
                    row("RdnCampaignId") = campaignMap(domainCampaignId)
                End If
                row("Domain") = domainValue
            Next
            ViewState("ShortedCampaignsData") = dt
            gridShortedCampaigns.DataSource = dt
            gridShortedCampaigns.DataBind()
        Else
            gridShortedCampaigns.DataSource = Nothing
            gridShortedCampaigns.DataBind()
        End If
    End Sub
End Class 