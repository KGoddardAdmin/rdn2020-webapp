Imports System.Data
Imports System.Data.SqlClient

Partial Class ReportClicksByCampaign
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Private StartDate As Date
    Private EndDate As Date
    Private ResultCount As Integer
    Private PageSize As Integer = 50


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblMsg.Text = String.Empty
        lblMsg.Font.Bold = False
        lblMsg.ForeColor = Drawing.Color.Black
        If Not IsPostBack Then            
            radUniqueNo.Checked = True
            GetDates()
            GetClients()            
        End If

    End Sub

    Protected Sub cmdViewDD_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdViewDD.Click
        GetDates()
        GetClients()        
    End Sub

    Protected Sub cmdGetReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGetReport.Click
        Page.Validate("frmReport")
        If Page.IsValid Then
            BindSQL()
        End If
    End Sub

    
#Region "Fill Grid"

    Sub BindSQL()
        GetDates()

        Dim dadCampaigns As New SqlDataAdapter
        Dim DS As New DataSet
        Dim sp As String = String.Empty
        Dim RcdCount As Integer

        If radUniqueYes.Checked = True Then
            sp = "Track_Click_ReportUniqueClicks"
        Else
            sp = "Track_Click_ReportTotalClicks"
        End If

        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdCampaign As New SqlCommand(sp, cnn)
                cmdCampaign.CommandType = CommandType.StoredProcedure
                cmdCampaign.Parameters.Add(New SqlParameter("@ClientUId", SqlDbType.UniqueIdentifier)).Value = SqlTypes.SqlGuid.Parse(ddClient.SelectedValue)
                cmdCampaign.Parameters.Add(New SqlParameter("@StartDate", SqlDbType.DateTime)).Value = StartDate
                cmdCampaign.Parameters.Add(New SqlParameter("@EndDate", SqlDbType.DateTime)).Value = EndDate
                dadCampaigns.SelectCommand = cmdCampaign
                dadCampaigns.Fill(DS, "Campaign")
                RcdCount = DS.Tables("Campaign").Rows.Count.ToString()                
            End Using
        End Using

        dgCampaigns.PageSize = PageSize

        If Not Page.IsPostBack Then
            dgCampaigns.CurrentPageIndex = 0
        End If

        ResultCount = RcdCount
        RecordCount.Text = "<b><font color=red>" & RcdCount & "</font> records found"

        If ResultCount > 0 Then
            trgrid.Visible = True
        End If

        'Now we assign the dataview to the datasource of the datagrid and we send it right on it
        Try
            dgCampaigns.DataSource = DS
            dgCampaigns.DataBind()
        Catch e As Exception
            dgCampaigns.CurrentPageIndex = 0
        End Try

        If dgCampaigns.CurrentPageIndex <> 0 Then
            Call Prev_Buttons()
            Firstbutton.Visible = True
            Prevbutton.Visible = True
        Else
            Firstbutton.Visible = False
            Prevbutton.Visible = False
        End If

        If dgCampaigns.CurrentPageIndex <> (dgCampaigns.PageCount - 1) Then
            Call Next_Buttons()
            Nextbutton.Visible = True
            Lastbutton.Visible = True
        Else
            Nextbutton.Visible = False
            Lastbutton.Visible = False
        End If

        lblPageCount.Text = "Page " & dgCampaigns.CurrentPageIndex + 1 & " of " & dgCampaigns.PageCount
    End Sub

    Sub PagerButtonClick(ByVal sender As Object, ByVal e As EventArgs)
        'used by external paging UI
        Dim arg As String = sender.CommandArgument

        Select Case arg
            Case "next"
                If (dgCampaigns.CurrentPageIndex < (dgCampaigns.PageCount - 1)) Then
                    dgCampaigns.CurrentPageIndex += 1
                End If

            Case "prev"
                If (dgCampaigns.CurrentPageIndex > 0) Then
                    dgCampaigns.CurrentPageIndex -= 1
                End If

            Case "last"
                dgCampaigns.CurrentPageIndex = (dgCampaigns.PageCount - 1)

            Case Else
                dgCampaigns.CurrentPageIndex = Convert.ToInt32(arg)
        End Select

        BindSQL()
    End Sub

    Sub Prev_Buttons()
        Dim PrevSet As String

        If dgCampaigns.CurrentPageIndex + 1 <> 1 And ResultCount <> -1 Then
            PrevSet = dgCampaigns.PageSize
            Prevbutton.Text = ("< Prev " & PrevSet)

            If dgCampaigns.CurrentPageIndex + 1 = dgCampaigns.PageCount Then
                Firstbutton.Text = ("<< 1st Page")
            End If
        End If
    End Sub

    Sub Next_Buttons()
        Dim NextSet As String

        If dgCampaigns.CurrentPageIndex + 1 < dgCampaigns.PageCount Then
            NextSet = dgCampaigns.PageSize
            Nextbutton.Text = ("Next " & NextSet & " >")
        End If

        If dgCampaigns.CurrentPageIndex + 1 = dgCampaigns.PageCount - 1 Then
            Dim EndCount As Integer = ResultCount - (dgCampaigns.PageSize * (dgCampaigns.CurrentPageIndex + 1))
            Nextbutton.Text = ("Next " & EndCount & " >")
        End If
    End Sub

#End Region

#Region "Time Functions"

    Protected Sub ddTime_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddTime.SelectedIndexChanged
        ddClient.Items.Clear()
        dgCampaigns.DataSource = ""
        trgrid.Visible = False
        If ddTime.SelectedIndex = 5 Then
            trDate.Visible = True
            DatePicker1.DateValue = DateAdd(DateInterval.Day, -7, Today())
            DatePicker2.DateValue = Date.Today
        Else
            trDate.Visible = False
            GetDates()
            GetClients()
        End If
    End Sub

    Protected Sub DatePicker1_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs)
        If DatePicker1.DateValue.ToShortDateString() > DatePicker2.DateValue.ToShortDateString() Then
            lblMsg.Text = "The End date must be greater than the start date."
        Else
            GetDates()
            ddClient.SelectedIndex = 0
        End If
    End Sub

    Protected Sub DatePicker2_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs)
        If DatePicker1.DateValue.ToShortDateString() > DatePicker2.DateValue.ToShortDateString() Then
            lblMsg.Text = "The End date must be greater than the start date."
        Else
            GetDates()
            ddClient.SelectedIndex = 0
        End If
    End Sub

    Private Sub GetDates()
        If ddTime.SelectedIndex = 0 Then 'Yesterday
            StartDate = DateAdd(DateInterval.Day, -1, Today())
            EndDate = Today()
        ElseIf ddTime.SelectedIndex = 1 Then 'Today
            StartDate = Today()
            EndDate = DateAdd(DateInterval.Day, 1, Today())
        ElseIf ddTime.SelectedIndex = 2 Then 'Last 7 Days
            StartDate = DateAdd(DateInterval.Day, -7, Today())
            EndDate = DateAdd(DateInterval.Day, 1, Today())
        ElseIf ddTime.SelectedIndex = 3 Then
            StartDate = DateAdd(DateInterval.Day, -10, Today())
            EndDate = DateAdd(DateInterval.Day, 1, Today())
        ElseIf ddTime.SelectedIndex = 4 Then
            StartDate = DateAdd(DateInterval.Day, -30, Today())
            EndDate = DateAdd(DateInterval.Day, 1, Today())
        ElseIf ddTime.SelectedIndex = 5 Then
            StartDate = DatePicker1.DateValue.ToShortDateString()
            EndDate = DatePicker2.DateValue.ToShortDateString()
        End If
    End Sub

#End Region

#Region "Clients"

    Private Sub GetClients()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetClients As New SqlCommand("CampaignAdCopy_GetClientByCreatedDate", cnn)
                cmdGetClients.CommandType = CommandType.StoredProcedure
                cmdGetClients.Parameters.Add(New SqlParameter("@StartDate", SqlDbType.DateTime)).Value = StartDate
                cmdGetClients.Parameters.Add(New SqlParameter("@EndDate", SqlDbType.DateTime)).Value = EndDate
                Using dtrGetClients As SqlDataReader = cmdGetClients.ExecuteReader
                    ddClient.DataSource = dtrGetClients
                    ddClient.DataBind()
                End Using
            End Using
        End Using
        ddClient.Items.Insert(0, "Select Client")
    End Sub
#End Region

#Region "Grid Functions"

    Public Function GetCampaignName(ByVal CampaignId As Integer) As String
        Dim CampaignName As String = String.Empty
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetCampaignName As New SqlCommand("CampaignAdCopy_GetByCampaignId", cnn)
                cmdGetCampaignName.CommandType = CommandType.StoredProcedure
                cmdGetCampaignName.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = CampaignId
                Using dtrGetCampaignName As SqlDataReader = cmdGetCampaignName.ExecuteReader
                    While dtrGetCampaignName.Read
                        CampaignName = dtrGetCampaignName("CampaignName")
                    End While
                End Using
            End Using
        End Using
        Return CampaignName
    End Function

    Public Function GetQuanityOrdered(ByVal CampaignId As Integer) As Integer
        Dim quanity As Integer
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetQuanity As New SqlCommand("CampaignAdCopy_GetByCampaignId", cnn)
                cmdGetQuanity.CommandType = CommandType.StoredProcedure
                cmdGetQuanity.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = CampaignId
                Using dtrGetQuanity As SqlDataReader = cmdGetQuanity.ExecuteReader
                    While dtrGetQuanity.Read
                        quanity = dtrGetQuanity("EmailsOrdered")
                    End While
                End Using
            End Using
        End Using
        Return quanity
    End Function

    Public Function GetOpens(ByVal CampaignId As Integer) As Integer
        Dim opens As Integer
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetOpens As New SqlCommand("Impression_ReportOpens", cnn)
                cmdGetOpens.CommandType = CommandType.StoredProcedure
                cmdGetOpens.Parameters.Add(New SqlParameter("@ImpressionCampaignId", SqlDbType.Int)).Value = CampaignId
                Using dtrGetOpens As SqlDataReader = cmdGetOpens.ExecuteReader
                    While dtrGetOpens.Read
                        opens = dtrGetOpens("Opens")
                    End While
                End Using
            End Using
        End Using
        Return (opens)
    End Function

    Public Function GetOpenThroughRate(ByVal id As Integer) As String
        Dim count As Integer
        Dim OpenThroughs As Double
        count = GetOpens(id)
        If count = 0 Then
            Return 0
            Exit Function
        End If
        OpenThroughs = (count / GetQuanityOrdered(id)) * 100
        Return FormatNumber(OpenThroughs, 4, TriState.False, TriState.True).ToString & "%"
    End Function

    Public Function GetClickThroughs(ByVal clicks As Integer, ByVal CampaignId As Integer) As String
        Dim ClickThrough As Double
        ClickThrough = (clicks / GetQuanityOrdered(CampaignId)) * 100
        Return FormatNumber(ClickThrough, 4, TriState.False, TriState.True).ToString & "%"
    End Function

    Protected Sub dgCampaigns_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgCampaigns.ItemCommand
        GetDates()
        Dim CampaignId As String = dgCampaigns.DataKeys(e.Item.ItemIndex).ToString()
        Response.Write("<br>CampaignId = " & CampaignId)
        If e.CommandName = "GetId" Then
            Dim RedURL As String
            RedURL = "Reportlinkbycampaign.aspx?c=@CampaignId" '&Client=@ClientUId" '&TimeFrame=@TimeFrame&Start=@StartDate&EndDate=@EndDate"
            RedURL = RedURL.Replace("@CampaignId", CampaignId)                       
            Response.Redirect(RedURL)
        End If
    End Sub

#End Region

    
End Class
