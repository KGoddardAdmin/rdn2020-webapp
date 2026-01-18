' ddTime Values
'0 =Last Week
'1=Last 30 Days
'2=Current Month
'3=Last Month
'4=Custom Dates

Imports System.Data
Imports System.Data.SqlClient

Partial Class CampaignEdit
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Private StartDate As Date
    Private Enddate As Date = Date.Today
    Private Creative As String
    Private NewLink As String = "href={Replace}"
    Private ConvertLink As String
    Private rcreative As String
    Private replacestring As String
    Private LinkId As Integer = 0
    Private CampaignId As Integer
    Private ImpressionPath As String    
    Private CouponPath As String
    Private OpenLink As String
    Private ImageOpenLink As String
    Private OpenLink1 As String
    Private OpenLink2 As String
    Private OpenLink3 As String
    Private FullFillDate As Date
    Private CouponTableNumber As Integer
    Private Domain As String
    Private DefaultDomain As String = "http://www.rdn2020.com"
    Private ClickPage As String
    Private DefaultClickPage As String = "P.aspx"
    Private OpenPage As String
    Private DefaultOpenPage As String = "ImageTrack.aspx"
    Private CouponPage As String
    Private DefaultCouponPage As String = "CP.aspx"
    Private BodyStartPosition As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            radActive.Checked = True
            ddTime.SelectedIndex = 0
            GetDates()
            GetClients()
            GetStatus()
            If Request.QueryString("CampaignId") IsNot Nothing Then
                If Request.QueryString("CampaignId").ToString() <> "" Then
                    txtCampaignId.Text = Request.QueryString("CampaignId")
                    PullInfoById()
                End If
            End If
        End If
        lblmsg.Text = String.Empty
        lblmsg.Font.Bold = False
        lblmsg.ForeColor = Drawing.Color.Black
        lblHeading.Text = String.Empty
        lblHeading.Font.Bold = False
        lblHeading.ForeColor = Drawing.Color.Black
    End Sub

    Protected Sub ddTime_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddTime.SelectedIndexChanged
        ddIO.Items.Clear()
        ddCampaign.Items.Clear()
        txtCampaignName.Text = String.Empty
        txtEmailQuanity.Text = String.Empty
        txtCampaignId.Text = String.Empty
        ckIsActive.Checked = False
        If ddTime.SelectedIndex = 4 Then
            trDate.Visible = True
            DatePicker1.DateValue = DateAdd(DateInterval.Day, -7, Today())
            DatePicker2.DateValue = Date.Today
        Else
            trDate.Visible = False
        End If
        GetDates()
        If ddClient.SelectedIndex > 0 Then
            GetIO()
        End If
    End Sub

    Protected Sub DatePicker1_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs)
        If DatePicker1.DateValue.ToShortDateString() > DatePicker2.DateValue.ToShortDateString() Then
            lblHeading.Text = "The End date must be greater than the start date."
        Else
            GetDates()
            ddClient.SelectedIndex = 0
            If ddIO.SelectedIndex > 0 Then
                ddIO.Items.Clear()
                ddIO.SelectedIndex = 0
            End If
            ddCampaign.SelectedIndex = 0
        End If
    End Sub

    Protected Sub DatePicker2_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs)
        If DatePicker1.DateValue.ToShortDateString() > DatePicker2.DateValue.ToShortDateString() Then
            lblHeading.Text = "The End date must be greater than the start date."
        Else
            GetDates()
            ddClient.SelectedIndex = 0
            If ddIO.SelectedIndex > 0 Then
                ddIO.Items.Clear()
                ddIO.SelectedIndex = 0
            End If
            ddCampaign.SelectedIndex = 0
        End If
    End Sub

    Protected Sub ddClient_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddClient.SelectedIndexChanged
        ddIO.Items.Clear()
        ddCampaign.Items.Clear()
        txtCampaignName.Text = String.Empty
        txtCouponVariable.Text = String.Empty
        radRegYes.Checked = False
        radRegNo.Checked = False
        txtFullFill.Text = String.Empty
        txtEmailQuanity.Text = String.Empty
        txtSubject.Text = String.Empty
        ckIsActive.Checked = False
        txtCampaignId.Text = String.Empty
        txtETitle.Text = String.Empty
        txtEDiscription.Text = String.Empty
        txtEDisplay.Text = String.Empty
        txtOpenLink1.Text = String.Empty
        txtOpenLink2.Text = String.Empty
        txtOpenLink3.Text = String.Empty
        Domain = String.Empty
        ClickPage = String.Empty
        OpenPage = String.Empty
        CouponPage = String.Empty
        If ddClient.SelectedIndex > 0 Then
            GetDates()
            GetIO()
        Else
            lblHeading.Text = "You must select an Client to select an IO."
            lblHeading.Font.Bold = True
            ddIO.SelectedIndex = 0
            ddCampaign.SelectedIndex = 0
        End If

    End Sub

    Protected Sub ddIO_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddIO.SelectedIndexChanged
        ddCampaign.Items.Clear()
        txtCampaignName.Text = String.Empty
        txtCouponVariable.Text = String.Empty
        radRegYes.Checked = False
        radRegNo.Checked = False
        txtFullFill.Text = String.Empty
        txtEmailQuanity.Text = String.Empty
        txtSubject.Text = String.Empty
        ckIsActive.Checked = False
        txtCampaignId.Text = String.Empty
        txtETitle.Text = String.Empty
        txtEDiscription.Text = String.Empty
        txtEDisplay.Text = String.Empty
        txtOpenLink1.Text = String.Empty
        txtOpenLink2.Text = String.Empty
        txtOpenLink3.Text = String.Empty
        If ddIO.SelectedIndex > 0 Then
            GetDates()
            LoadCampiagnDD()
        Else
            lblHeading.Text = "You must select an IO to select a campaign."
            lblHeading.Font.Bold = True
            ddCampaign.SelectedIndex = 0
        End If

    End Sub

    Protected Sub ddCampaign_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddCampaign.SelectedIndexChanged
        GetCampaignToEdit()
    End Sub

    Protected Sub radRegYes_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radRegYes.CheckedChanged
        trcvar.Visible = False
        rfvCoupon.Enabled = False
    End Sub

    Protected Sub radRegNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radRegNo.CheckedChanged
        trcvar.Visible = True
        rfvCoupon.Enabled = True
    End Sub

    Protected Sub radActive_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radActive.CheckedChanged
        ddCampaign.Items.Clear()
        If ddIO.SelectedIndex > 0 Then
            GetDates()
            LoadCampiagnDD()
            txtCampaignName.Text = String.Empty
            txtEmailQuanity.Text = String.Empty
            txtCampaignId.Text = String.Empty
            ckIsActive.Checked = False
        Else
            lblHeading.Text = "You must select an IO before you can select a campaign."
            lblHeading.Font.Bold = True
        End If
    End Sub

    Protected Sub radNon_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radNon.CheckedChanged
        ddCampaign.Items.Clear()
        If ddIO.SelectedIndex > 0 Then
            GetDates()
            LoadCampiagnDD()
            txtCampaignName.Text = String.Empty
            txtEmailQuanity.Text = String.Empty
            txtCampaignId.Text = String.Empty
            ckIsActive.Checked = False
        Else
            lblHeading.Text = "You must select an IO before you can select a campaign."
            lblHeading.Font.Bold = True
        End If
    End Sub

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        PullInfoById()
    End Sub

    Protected Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
        Page.Validate("frmCampaign")
        If Page.IsValid Then
            If CheckForm() = True Then
                If Len(Trim(txtHtml.Text)) = 0 Then
                    UpdateCampaign()
                    If hfHasAd.Value = "Yes" Then
                        UpdateEzangaAdInfo()
                    Else
                        InsetEzangaInfo()
                    End If
                Else
                    DeleteOldCampaign()
                    If radRegNo.Checked = False Then
                        convert()
                    Else
                        ConvertSeedCampaign()
                    End If
                    UpdateCampaignAndCreative()
                    If hfHasAd.Value = "Yes" Then
                        UpdateEzangaAdInfo()
                    Else
                        InsetEzangaInfo()
                    End If
                End If
            End If
        End If
    End Sub

    Protected Sub cmdDeltet_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDeltet.Click
        Dim redurl As String = "CampaignDelete.aspx?c=@CampaignId&Name=@CampaignName"
        If ddCampaign.SelectedIndex > 0 Then
            redurl = redurl.Replace("@CampaignId", ddCampaign.SelectedValue)
            redurl = redurl.Replace("@CampaignName", Trim(txtCampaignName.Text))
            Response.Redirect(redurl)
        Else
            lblmsg.Text = "You must select a campaign to delete."
            lblmsg.Font.Bold = True
            lblmsg.ForeColor = Drawing.Color.Red
        End If

    End Sub

    Private Sub GetDates()
        If ddTime.SelectedIndex = 0 Then
            StartDate = DateAdd(DateInterval.Day, -7, Today())
            Enddate = Date.Today
        ElseIf ddTime.SelectedIndex = 1 Then
            StartDate = DateAdd(DateInterval.Day, -30, Today())
            Enddate = Date.Today
        ElseIf ddTime.SelectedIndex = 2 Then
            StartDate = New Date(Today.Year, Today.Month, 1)
            Dim FirstDayofNextMonth As Date = New Date(Today.Year, Today.Month + 1, 1)
            Enddate = FormatDateTime(FirstDayofNextMonth.AddDays(-1), DateFormat.ShortDate)
        ElseIf ddTime.SelectedIndex = 3 Then
            Dim FirstDayOfThisMonth As Date = New Date(Today.Year, Today.Month, 1)
            Enddate = FormatDateTime(FirstDayOfThisMonth.AddDays(-1), DateFormat.ShortDate)
            StartDate = New Date(Today.Year, Today.Month - 1, 1)
        ElseIf ddTime.SelectedIndex = 4 Then
            StartDate = DatePicker1.DateValue.ToShortDateString()
            Enddate = DatePicker2.DateValue.ToShortDateString()
        End If
    End Sub

    Private Sub GetCampaignToEdit()
        GetDomain()
        hfType.Value = String.Empty
        Dim UseInternal As Integer
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetCampaignToEdit As New SqlCommand("CampaignAdCopy_GetByCampaignId", cnn)
                cmdGetCampaignToEdit.CommandType = CommandType.StoredProcedure
                cmdGetCampaignToEdit.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = ddCampaign.SelectedValue
                Using dtrGetCampaignToEdit As SqlDataReader = cmdGetCampaignToEdit.ExecuteReader
                    While dtrGetCampaignToEdit.Read
                        If Not IsDBNull(dtrGetCampaignToEdit("CampaignName")) Then
                            txtCampaignName.Text = dtrGetCampaignToEdit("CampaignName")
                        Else
                            txtCampaignName.Text = String.Empty
                        End If

                        If Not IsDBNull(dtrGetCampaignToEdit("EmailsOrdered")) Then
                            txtEmailQuanity.Text = dtrGetCampaignToEdit("EmailsOrdered")
                        Else
                            txtEmailQuanity.Text = String.Empty
                        End If

                        If dtrGetCampaignToEdit("IsActive") = 1 Then
                            ckIsActive.Checked = True
                        Else
                            ckIsActive.Checked = False
                        End If

                        If Not IsDBNull(dtrGetCampaignToEdit("Status")) Or dtrGetCampaignToEdit("Status") <> 0 Then
                            ddStatus.SelectedValue = dtrGetCampaignToEdit("Status")
                        Else
                            ddStatus.SelectedIndex = 0
                        End If

                        If Not IsDBNull(dtrGetCampaignToEdit("Clicks")) Then
                            txtClicks.Text = dtrGetCampaignToEdit("Clicks")
                        Else
                            txtClicks.Text = 0
                        End If

                        If Not IsDBNull(dtrGetCampaignToEdit("Impressions")) Then
                            txtImpressions.Text = dtrGetCampaignToEdit("Impressions")
                        Else
                            txtImpressions.Text = 0
                        End If

                        If Not IsDBNull(dtrGetCampaignToEdit("SubjectLine")) Then
                            txtSubject.Text = dtrGetCampaignToEdit("SubjectLine")
                        Else
                            txtSubject.Text = String.Empty
                        End If

                        If Not IsDBNull(dtrGetCampaignToEdit("OpenLink1")) Then
                            txtOpenLink1.Text = dtrGetCampaignToEdit("OpenLink1")
                        Else
                            txtOpenLink1.Text = String.Empty
                        End If

                        If Not IsDBNull(dtrGetCampaignToEdit("OpenLink2")) Then
                            txtOpenLink2.Text = dtrGetCampaignToEdit("OpenLink2")
                        Else
                            txtOpenLink2.Text = String.Empty
                        End If

                        If Not IsDBNull(dtrGetCampaignToEdit("OpenLink3")) Then
                            txtOpenLink3.Text = dtrGetCampaignToEdit("OpenLink3")
                        Else
                            txtOpenLink3.Text = String.Empty
                        End If

                        If Not IsDBNull(dtrGetCampaignToEdit("CouponVariable")) Then
                            txtCouponVariable.Text = dtrGetCampaignToEdit("CouponVariable")
                        Else
                            txtCouponVariable.Text = String.Empty
                        End If

                        If Not IsDBNull(dtrGetCampaignToEdit("FullFillBy")) Then
                            txtFullFill.Text = dtrGetCampaignToEdit("FullFillBy")
                        Else
                            txtFullFill.Text = Date.Today
                        End If

                        If Not IsDBNull(dtrGetCampaignToEdit("InternalOpenLink")) Then
                            UseInternal = dtrGetCampaignToEdit("InternalOpenLink")
                        Else
                            UseInternal = 0
                        End If

                        If UseInternal = 0 Then
                            radInternalYes.Checked = False
                            radInternalNo.Checked = True
                        Else
                            radInternalYes.Checked = True
                            radInternalNo.Checked = False
                        End If

                        If txtCouponVariable.Text = String.Empty Then
                            radRegYes.Checked = True
                            radRegNo.Checked = False
                            trcvar.Visible = False
                            hfType.Value = "Reg"
                        Else
                            radRegYes.Checked = False
                            radRegNo.Checked = True
                            trcvar.Visible = True
                            hfType.Value = "Coupon"
                        End If

                    End While
                End Using
            End Using
        End Using
        GetEzangaInfo()

    End Sub

    Private Sub PullInfoById()
        Dim createdate As Date
        Dim client As String = String.Empty
        Dim io As String = String.Empty
        Dim ioStart As Date
        Dim quanity As Integer
        Dim active As Integer
        Dim campaignname As String = String.Empty
        Dim Status As Integer
        Dim clicks As Integer
        Dim impressions As Integer
        Dim subjectline As String = String.Empty
        Dim OpenLink1 As String = String.Empty
        Dim OpenLink2 As String = String.Empty
        Dim OpenLink3 As String = String.Empty
        Dim UseInternal As Integer
        Dim CouponVariable As String = String.Empty
        Dim FullFillBy As Date
        hfType.Value = String.Empty


        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdPullInfoById As New SqlCommand("CampaignAdCopy_GetCampaignForEditById", cnn)
                cmdPullInfoById.CommandType = CommandType.StoredProcedure
                cmdPullInfoById.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = txtCampaignId.Text
                Using dtrPullInfoById As SqlDataReader = cmdPullInfoById.ExecuteReader
                    If dtrPullInfoById.HasRows Then
                        While dtrPullInfoById.Read

                            If Not IsDBNull(dtrPullInfoById("CampaignName")) Then
                                campaignname = dtrPullInfoById("CampaignName")
                            Else
                                campaignname = String.Empty
                            End If

                            createdate = dtrPullInfoById("CreatedOn")
                            client = dtrPullInfoById("ClientUid").ToString
                            io = dtrPullInfoById("IOUId").ToString
                            ioStart = dtrPullInfoById("IOStartDate")

                            If Not IsDBNull(dtrPullInfoById("EmailsOrdered")) Then
                                quanity = dtrPullInfoById("EmailsOrdered")
                            Else
                                quanity = String.Empty
                            End If

                            active = dtrPullInfoById("IsActive")

                            If Not IsDBNull(dtrPullInfoById("Status")) Then
                                Status = dtrPullInfoById("Status")
                            Else
                                Status = 6
                            End If

                            If Not IsDBNull(dtrPullInfoById("Clicks")) Then
                                clicks = dtrPullInfoById("Clicks")
                            Else
                                clicks = 0
                            End If

                            If Not IsDBNull(dtrPullInfoById("impressions")) Then
                                impressions = dtrPullInfoById("impressions")
                            Else
                                impressions = 0
                            End If

                            If Not IsDBNull(dtrPullInfoById("Subjectline")) Then
                                subjectline = dtrPullInfoById("Subjectline")
                            Else
                                subjectline = String.Empty
                            End If

                            If Not IsDBNull(dtrPullInfoById("openlink1")) Then
                                OpenLink1 = dtrPullInfoById("openlink1")
                            Else
                                OpenLink1 = String.Empty
                            End If

                            If Not IsDBNull(dtrPullInfoById("openlink2")) Then
                                OpenLink2 = dtrPullInfoById("openlink2")
                            Else
                                OpenLink2 = String.Empty
                            End If

                            If Not IsDBNull(dtrPullInfoById("openlink3")) Then
                                OpenLink3 = dtrPullInfoById("openlink3")
                            Else
                                OpenLink3 = String.Empty
                            End If

                            If Not IsDBNull(dtrPullInfoById("InternalOpenLink")) Then
                                UseInternal = dtrPullInfoById("InternalOpenLink")
                            Else
                                UseInternal = 0
                            End If

                            If Not IsDBNull(dtrPullInfoById("CouponVariable")) Then
                                CouponVariable = dtrPullInfoById("CouponVariable")
                            Else
                                CouponVariable = String.Empty
                            End If

                            If Not IsDBNull(dtrPullInfoById("FullFillBy")) Then
                                FullFillBy = dtrPullInfoById("FullFillBy")
                            Else
                                FullFillBy = Date.Today
                            End If


                        End While
                    Else
                        lblmsg.Text = "There is no campaign created with that id, please try again."
                    End If
                End Using
            End Using
        End Using
        ddTime.SelectedIndex = 4
        trDate.Visible = True
        DatePicker1.DateValue = DateAdd(DateInterval.Day, -1, createdate)
        DatePicker2.DateValue = DateAdd(DateInterval.Day, 1, createdate)
        ddClient.SelectedValue = client
        ddIO.Items.Clear()
        GetDates()
        GetIOByCampaignIdInfo(ioStart, client)
        ddIO.SelectedValue = io
        txtCampaignName.Text = campaignname
        txtEmailQuanity.Text = quanity
        txtClicks.Text = clicks
        txtImpressions.Text = impressions
        txtSubject.Text = subjectline
        txtOpenLink1.Text = OpenLink1
        txtOpenLink2.Text = OpenLink2
        txtOpenLink3.Text = OpenLink3
        txtCouponVariable.Text = CouponVariable
        txtFullFill.Text = FullFillBy
        If CouponVariable = String.Empty Then
            radRegYes.Checked = True
            radRegNo.Checked = False
            trcvar.Visible = False
            hfType.Value = "Reg"
        Else
            radRegYes.Checked = False
            radRegNo.Checked = True
            trcvar.Visible = True
            hfType.Value = "Coupon"
        End If
        LoadCampiagnDD()
        ddCampaign.SelectedValue = txtCampaignId.Text
        ddStatus.SelectedValue = Status
        If active = 1 Then
            ckIsActive.Checked = True
        Else
            ckIsActive.Checked = False
        End If
        If UseInternal = 0 Then
            radInternalYes.Checked = False
            radInternalNo.Checked = True
        Else
            radInternalYes.Checked = True
            radInternalNo.Checked = False
        End If
        GetEzangaInfo()
    End Sub

    Private Sub GetIOByCampaignIdInfo(ByVal ddate As Date, ByVal client As String)
        Dim startdate As Date = DateAdd(DateInterval.Day, -1, ddate)
        Dim enddate As Date = DateAdd(DateInterval.Day, 1, ddate)
        Dim IsActive As String = "1"
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetIO As New SqlCommand("IO_GetActiveByClientIdAndStartDate", cnn)
                cmdGetIO.CommandType = CommandType.StoredProcedure
                cmdGetIO.Parameters.Add(New SqlParameter("@ClientUId", SqlDbType.UniqueIdentifier)).Value = SqlTypes.SqlGuid.Parse(client) 'ddClient.SelectedValue)
                cmdGetIO.Parameters.Add(New SqlParameter("@StartDate", SqlDbType.DateTime)).Value = startdate
                cmdGetIO.Parameters.Add(New SqlParameter("@EndDate", SqlDbType.DateTime)).Value = enddate
                Using dtrGetIO As SqlDataReader = cmdGetIO.ExecuteReader
                    If dtrGetIO.HasRows Then
                        ddIO.DataSource = dtrGetIO
                        ddIO.DataBind()
                    Else
                        lblHeading.Text = "There are no active IO's for this client created in this time span."
                        lblHeading.Font.Bold = True
                        lblHeading.ForeColor = Drawing.Color.Red
                    End If
                End Using
            End Using
        End Using
    End Sub

    Private Sub GetEzangaInfo()
        hfHasAd.Value = String.Empty
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetEzangaInfo As New SqlCommand("Ezanga_Ad_GetByCampaignId", cnn)
                cmdGetEzangaInfo.CommandType = CommandType.StoredProcedure
                cmdGetEzangaInfo.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = ddCampaign.SelectedValue
                Using dtrGetEzangaInfo As SqlDataReader = cmdGetEzangaInfo.ExecuteReader
                    If dtrGetEzangaInfo.HasRows Then
                        hfHasAd.Value = "Yes"
                        While dtrGetEzangaInfo.Read

                            If Not IsDBNull(dtrGetEzangaInfo("AdTitle")) Then
                                txtETitle.Text = dtrGetEzangaInfo("AdTitle")
                            Else
                                txtETitle.Text = String.Empty
                            End If

                            If Not IsDBNull(dtrGetEzangaInfo("AdDescription")) Then
                                txtEDiscription.Text = dtrGetEzangaInfo("AdDescription")
                            Else
                                txtEDiscription.Text = String.Empty
                            End If

                            If Not IsDBNull(dtrGetEzangaInfo("DisplayURL")) Then
                                txtEDisplay.Text = dtrGetEzangaInfo("DisplayURL")
                            Else
                                txtEDisplay.Text = String.Empty
                            End If
                        End While
                    Else
                        hfHasAd.Value = "No"
                    End If
                End Using
            End Using
        End Using
    End Sub

    Private Sub GetDomain()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetDomain As New SqlCommand("Client_GetByUId", cnn)
                cmdGetDomain.CommandType = CommandType.StoredProcedure
                cmdGetDomain.Parameters.Add(New SqlParameter("@ClientUId", SqlDbType.UniqueIdentifier)).Value = SqlTypes.SqlGuid.Parse(ddClient.SelectedValue)
                Using dtrGetDomain As SqlDataReader = cmdGetDomain.ExecuteReader
                    While dtrGetDomain.Read
                        If Not IsDBNull(dtrGetDomain("ClientDomain")) Then
                            Domain = dtrGetDomain("ClientDomain")
                        Else
                            Domain = DefaultDomain
                        End If

                        If Not IsDBNull(dtrGetDomain("ClientClick")) Then
                            ClickPage = dtrGetDomain("ClientClick")
                        Else
                            ClickPage = DefaultClickPage
                        End If


                        If Not IsDBNull(dtrGetDomain("ClientOpen")) Then
                            OpenPage = dtrGetDomain("ClientOpen")
                        Else
                            OpenPage = DefaultOpenPage
                        End If

                        If Not IsDBNull(dtrGetDomain("ClientCoupon")) Then
                            CouponPage = dtrGetDomain("ClientCoupon")
                        Else
                            CouponPage = DefaultCouponPage
                        End If
                    End While
                End Using
            End Using
        End Using

    End Sub

#Region "Fill Drop Downs"

    Private Sub GetClients()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetClient As New SqlCommand("Client_Get", cnn)
                cmdGetClient.CommandType = CommandType.StoredProcedure
                Using dtrGetClient As SqlDataReader = cmdGetClient.ExecuteReader
                    ddClient.DataSource = dtrGetClient
                    ddClient.DataBind()
                End Using
            End Using
        End Using
        ddClient.Items.Insert(0, "Select Client")
    End Sub

    Private Sub GetStatus()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetStatus As New SqlCommand("CAMPAIGNSTATUS_Get", cnn)
                cmdGetStatus.CommandType = CommandType.StoredProcedure
                Using dtrGetStatus As SqlDataReader = cmdGetStatus.ExecuteReader
                    ddStatus.DataSource = dtrGetStatus
                    ddStatus.DataBind()
                End Using
            End Using
        End Using
        ddStatus.Items.Insert(0, "Select Status")
    End Sub

    Private Sub GetIO()
        Dim IsActive As String = "1"
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetIO As New SqlCommand("IO_GetActiveByClientIdAndStartDate", cnn)
                cmdGetIO.CommandType = CommandType.StoredProcedure
                cmdGetIO.Parameters.Add(New SqlParameter("@ClientUId", SqlDbType.UniqueIdentifier)).Value = SqlTypes.SqlGuid.Parse(ddClient.SelectedValue)
                cmdGetIO.Parameters.Add(New SqlParameter("@StartDate", SqlDbType.DateTime)).Value = StartDate
                cmdGetIO.Parameters.Add(New SqlParameter("@EndDate", SqlDbType.DateTime)).Value = Enddate
                Using dtrGetIO As SqlDataReader = cmdGetIO.ExecuteReader
                    If dtrGetIO.HasRows Then
                        ddIO.DataSource = dtrGetIO
                        ddIO.DataBind()
                    Else
                        lblHeading.Text = "There are no active IO's for this client created in this time span."
                        lblHeading.Font.Bold = True
                        lblHeading.ForeColor = Drawing.Color.Red
                    End If
                End Using
            End Using
        End Using
        ddIO.Items.Insert(0, "Select IO")
    End Sub

    'Private Sub LoadCampiagnDD()
    '    Dim dstCampaign As New DataSet
    '    Dim IsActive As Integer
    '    If radActive.Checked = True Then
    '        IsActive = 1
    '    Else
    '        IsActive = 0
    '    End If
    '    Using cnn As New SqlConnection(strConn)
    '        cnn.Open()
    '        Using cmdLoadCampaignDD As New SqlCommand("CampaignAdCopy_GetByActiveStatusAndIOUIdAndStartDate", cnn)
    '            cmdLoadCampaignDD.CommandType = CommandType.StoredProcedure
    '            cmdLoadCampaignDD.Parameters.Add(New SqlParameter("@IsActive", SqlDbType.NVarChar, 25)).Value = IsActive
    '            cmdLoadCampaignDD.Parameters.Add(New SqlParameter("@IOUId", SqlDbType.UniqueIdentifier)).Value = SqlTypes.SqlGuid.Parse(ddIO.SelectedValue)
    '            cmdLoadCampaignDD.Parameters.Add(New SqlParameter("@StartDate", SqlDbType.DateTime)).Value = StartDate
    '            cmdLoadCampaignDD.Parameters.Add(New SqlParameter("@EndDate", SqlDbType.DateTime)).Value = Enddate
    '            Using dadLoadCampaignDD As New SqlDataAdapter(cmdLoadCampaignDD)
    '                dadLoadCampaignDD.Fill(dstCampaign, "Campaign")

    '                If dstCampaign.Tables("Campaign").Rows.Count = 0 Then
    '                    If IsActive = 1 Then
    '                        lblHeading.Text = "There are no active Campaigns for this IO created in this time span."
    '                    Else
    '                        lblHeading.Text = "There are no inactive Campaign for this IO created in this time span."
    '                    End If
    '                    lblHeading.Font.Bold = True
    '                    lblHeading.ForeColor = Drawing.Color.Red
    '                Else
    '                    Dim Dyncolumn As New DataColumn
    '                    With Dyncolumn
    '                        .ColumnName = "Campaign"
    '                        .DataType = System.Type.GetType("System.String")
    '                        .Expression = "CampaignId+'   '+CampaignName"
    '                    End With
    '                    dstCampaign.Tables("Campaign").Columns.Add(Dyncolumn)
    '                    ddCampaign.DataTextField = "Campaign"
    '                    ddCampaign.DataValueField = "CampaignId"
    '                    ddCampaign.DataSource = dstCampaign.Tables("Campaign").DefaultView
    '                    ddCampaign.DataBind()
    '                End If
    '                ddCampaign.Items.Insert(0, "Select Campaign")
    '            End Using
    '        End Using
    '    End Using
    'End Sub



#End Region

#Region "Convert Creative"

    Public Sub convert()
        Creative = txtHtml.Text

        If Trim(Len(Creative)) > 0 Or Creative <> String.Empty Then
            CampaignId = ddCampaign.SelectedValue
            'GetCompanyInfo()
            GetDomain()
            ImpressionPath = Domain & "/" & OpenPage & "?c=" & CampaignId
            OpenLink &= "<img src=""" & ImpressionPath & """ alt="""" width=""0"" height=""0""/>"
            Creative = Replace(Creative, "href = ", "href=", 1, -1, CompareMethod.Text)
            Creative = Replace(Creative, "href= ", "href=", 1, -1, CompareMethod.Text)
            Creative = Replace(Creative, "HREF = ", "href=", 1, -1, CompareMethod.Text)
            Creative = Replace(Creative, "HREF= ", "href=", 1, -1, CompareMethod.Text)
            Creative = Replace(Creative, "HREF =", "href=", 1, -1, CompareMethod.Text)
            Creative = Replace(Creative, "href =", "href=", 1, -1, CompareMethod.Text)
            BodyStartPosition = InStr(Creative, "<body")
            If InStr(Creative, "href=") = 0 Then
                txtHtml.Text = "Creative Has No Click Link In It!!"
                txtHtml.Font.Bold = True
                txtHtml.ForeColor = Drawing.Color.Red
                txtHtml.Font.Size = 24
                Exit Sub
            End If
            Creative = Replace(Creative, "href=", NewLink, 1, -1, CompareMethod.Text)
            Dim StartOfLink As Integer = 1
            Dim endoflink As Integer = 1

            Do Until StartOfLink = 0
                StartOfLink = InStr(Creative, NewLink)
                If StartOfLink <> 0 Then
                    rcreative = Right(Creative, Len(Creative) - StartOfLink - 14)
                    endoflink = InStr(rcreative, """")
                    replacestring = Left(rcreative, endoflink - 1)
                    Transform(replacestring, StartOfLink, Len(replacestring))
                End If
            Loop
            Creative = Replace(Creative, "href={Replaced}", "href=", 1, -1, CompareMethod.Text)
            Creative = Creative & OpenLink
        Else
            txtHtml.Text = "You need to enter a creative"
            txtHtml.Font.Bold = True
            txtHtml.ForeColor = Drawing.Color.Red
            txtHtml.Font.Size = 24
        End If
        txtConverted.Text &= Creative
    End Sub

    Public Sub ConvertSeedCampaign()

        Creative = txtHtml.Text

        If InStr(Creative, txtCouponVariable.Text) = 0 Then
            lblmsg.Text = "The coupon variable is not in the creative, please check your variable or the creative and try again."
            lblmsg.Font.Bold = True
            lblmsg.ForeColor = Drawing.Color.Red
        Else
            Creative = Replace(Creative, txtCouponVariable.Text, "{CPLink}", 1, -1, CompareMethod.Text)
        End If

        If Trim(Len(Creative)) > 0 Or Creative <> String.Empty Then
            CampaignId = ddCampaign.SelectedValue
            'GetCompanyInfo()
            GetDomain()
            CreateCouponTable()
            ImpressionPath = Domain & "/" & OpenPage & "?c=" & CampaignId
            OpenLink &= "<img src=""" & ImpressionPath & """ alt="""" width=""0"" height=""0""/>"
            Creative = Replace(Creative, "href = ", "href=", 1, -1, CompareMethod.Text)
            Creative = Replace(Creative, "href= ", "href=", 1, -1, CompareMethod.Text)
            Creative = Replace(Creative, "HREF = ", "href=", 1, -1, CompareMethod.Text)
            Creative = Replace(Creative, "HREF= ", "href=", 1, -1, CompareMethod.Text)
            Creative = Replace(Creative, "HREF =", "href=", 1, -1, CompareMethod.Text)
            Creative = Replace(Creative, "href =", "href=", 1, -1, CompareMethod.Text)
            BodyStartPosition = InStr(Creative, "<body")
            If InStr(Creative, "href=") = 0 Then
                txtHtml.Text = "Creative Has No Click Link In It!!"
                txtHtml.Font.Bold = True
                txtHtml.ForeColor = Drawing.Color.Red
                txtHtml.Font.Size = 24
                Exit Sub
            End If
            Creative = Replace(Creative, "href=", NewLink, 1, -1, CompareMethod.Text)
            Dim StartOfLink As Integer = 1
            Dim endoflink As Integer = 1

            Do Until StartOfLink = 0
                StartOfLink = InStr(Creative, NewLink)
                If StartOfLink <> 0 Then
                    rcreative = Right(Creative, Len(Creative) - StartOfLink - 14)
                    endoflink = InStr(rcreative, """")
                    replacestring = Left(rcreative, endoflink - 1)
                    TransformSeed(replacestring, StartOfLink, Len(replacestring))
                End If
            Loop
            Creative = Replace(Creative, "href={Replaced}", "href=", 1, -1, CompareMethod.Text)
            Creative = Creative & OpenLink
        Else
            txtHtml.Text = "You need to enter a creative"
            txtHtml.Font.Bold = True
            txtHtml.ForeColor = Drawing.Color.Red
            txtHtml.Font.Size = 24
        End If
        txtConverted.Text &= Creative
    End Sub

    Private Sub TransformSeed(ByVal link As String, ByVal startpos As Integer, ByVal length As Integer)
        If startpos > BodyStartPosition Then
            If InStr(link, "http") > 0 Then
                If InStr(link.ToLower, "mailto:") = 0 Then
                    CouponPath = Domain & "/" & CouponPage & "?c=@CampaignId~@LinkId"
                    LinkId = LinkId + 1
                    Dim nstr As String = Trim(CouponPath)
                    nstr = Replace(nstr, "@CampaignId", Trim(ddCampaign.SelectedValue))
                    nstr = Replace(nstr, "@LinkId", Trim(LinkId))
                    'nstr = nstr & "~" & txtCouponVariable.Text
                    nstr = Trim(nstr)
                    nstr = Replace(nstr, Chr(9), "", 1, -1, CompareMethod.Text)
                    Creative = Creative.Remove(startpos + 14, length)
                    Creative = Creative.Insert(startpos + 14, nstr)
                    InsertCampaign(LinkId, link)
                End If
            End If
        End If
        Creative = Creative.Insert(startpos + 12, "d")
    End Sub

    Private Sub Transform(ByVal link As String, ByVal startpos As Integer, ByVal length As Integer)
        If startpos > BodyStartPosition Then
            If InStr(link, "http") > 0 Then
                If InStr(link.ToLower, "mailto:") = 0 Then
                    ConvertLink = Domain & "/" & ClickPage & "?c=@CampaignId~@LinkId"
                    LinkId = LinkId + 1
                    Dim nstr As String = Trim(ConvertLink)
                    nstr = Replace(nstr, "@CampaignId", Trim(ddCampaign.SelectedValue))
                    nstr = Replace(nstr, "@LinkId", Trim(LinkId))
                    nstr = Trim(nstr)
                    nstr = Replace(nstr, Chr(9), "", 1, -1, CompareMethod.Text)
                    Creative = Creative.Remove(startpos + 14, length)
                    Creative = Creative.Insert(startpos + 14, Trim(nstr))
                    If InStr(link.ToLower, "trackingreport") > 0 Then
                        link = Replace(link.ToLower, "process", "RProcess", 1, -1, CompareMethod.Text)
                    End If
                    InsertCampaign(LinkId, link)
                End If
            End If
        End If
        Creative = Creative.Insert(startpos + 12, "d")
    End Sub

    Private Sub InsertCampaign(ByVal linkId As Integer, ByVal Link As String)
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdInsertCampaign As New SqlCommand("Campaign_CreateNew", cnn)
                cmdInsertCampaign.CommandType = Data.CommandType.StoredProcedure
                cmdInsertCampaign.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = CampaignId
                cmdInsertCampaign.Parameters.Add(New SqlParameter("@LinkId", Data.SqlDbType.Int)).Value = linkId
                cmdInsertCampaign.Parameters.Add(New SqlParameter("@Link", Data.SqlDbType.NVarChar, 200)).Value = Link
                cmdInsertCampaign.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Sub UpdateCampaign()

        Dim IsActive As Integer
        Dim useinternal As Integer
        If ckIsActive.Checked = True Then
            IsActive = 1
        Else
            IsActive = 0
        End If

        If radInternalYes.Checked = True Then
            useinternal = 1
        Else
            useinternal = 0
        End If

        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdUpdateCampaign As New SqlCommand("CampaignAdCopy_UpdateCampaign", cnn)
                Try
                    cmdUpdateCampaign.CommandType = CommandType.StoredProcedure
                    cmdUpdateCampaign.Parameters.Add(New SqlParameter("@CampaignName", Data.SqlDbType.NVarChar, 100)).Value = Trim(txtCampaignName.Text)
                    cmdUpdateCampaign.Parameters.Add(New SqlParameter("@EmailsOrdered", Data.SqlDbType.Int)).Value = Trim(txtEmailQuanity.Text)
                    cmdUpdateCampaign.Parameters.Add(New SqlParameter("@IsActive", Data.SqlDbType.TinyInt)).Value = IsActive
                    cmdUpdateCampaign.Parameters.Add(New SqlParameter("@Status", Data.SqlDbType.TinyInt)).Value = ddStatus.SelectedValue
                    If radRegNo.Checked = False Then
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@CouponVariable", Data.SqlDbType.NVarChar, 50)).Value = Trim(txtCouponVariable.Text)
                    Else
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@CouponVariable", Data.SqlDbType.NVarChar, 50)).Value = DBNull.Value
                    End If

                    If Len(Trim(txtSubject.Text)) > 0 Or Trim(txtSubject.Text) <> String.Empty Then
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@SubjectLine", Data.SqlDbType.NVarChar, 250)).Value = Trim(txtSubject.Text)
                    Else
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@SubjectLine", Data.SqlDbType.NVarChar, 250)).Value = DBNull.Value
                    End If
                    cmdUpdateCampaign.Parameters.Add(New SqlParameter("@Clicks", Data.SqlDbType.Int)).Value = CInt(txtClicks.Text)
                    cmdUpdateCampaign.Parameters.Add(New SqlParameter("@Impressions", Data.SqlDbType.Int)).Value = CInt(txtImpressions.Text)

                    If Len(Trim(txtOpenLink1.Text)) > 0 Or Trim(txtOpenLink1.Text) <> String.Empty Then
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@OpenLink1", Data.SqlDbType.NVarChar, 250)).Value = Trim(txtOpenLink1.Text)
                    Else
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@OpenLink1", Data.SqlDbType.NVarChar, 250)).Value = DBNull.Value
                    End If

                    If Len(Trim(txtOpenLink2.Text)) > 0 Or Trim(txtOpenLink2.Text) <> String.Empty Then
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@OpenLink2", Data.SqlDbType.NVarChar, 250)).Value = Trim(txtOpenLink2.Text)
                    Else
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@OpenLink2", Data.SqlDbType.NVarChar, 250)).Value = DBNull.Value
                    End If

                    If Len(Trim(txtOpenLink3.Text)) > 0 Or Trim(txtOpenLink3.Text) <> String.Empty Then
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@OpenLink3", Data.SqlDbType.NVarChar, 250)).Value = Trim(txtOpenLink3.Text)
                    Else
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@OpenLink3", Data.SqlDbType.NVarChar, 250)).Value = DBNull.Value
                    End If
                    cmdUpdateCampaign.Parameters.Add(New SqlParameter("@InternalOpenLink", Data.SqlDbType.TinyInt)).Value = useinternal
                    cmdUpdateCampaign.Parameters.Add(New SqlParameter("@FullFillBy", Data.SqlDbType.DateTime)).Value = Trim(txtFullFill.Text)
                    cmdUpdateCampaign.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = ddCampaign.SelectedValue
                    cmdUpdateCampaign.ExecuteNonQuery()
                Catch ex As Exception
                    lblmsg.Text = "There was a problem with the update please try agian in a few min."
                    lblmsg.Font.Bold = True
                    lblmsg.ForeColor = Drawing.Color.Red
                Finally
                    lblmsg.Text = "Campaign " & ddCampaign.SelectedItem.Text & " has Successfully been updated."
                    lblmsg.Font.Bold = True
                End Try
            End Using
        End Using

    End Sub

    Private Sub UpdateCampaignAndCreative()
        Dim IsActive As Integer
        Dim useinternal As Integer
        If ckIsActive.Checked = True Then
            IsActive = 1
        Else
            IsActive = 0
        End If

        If radInternalYes.Checked = True Then
            useinternal = 1
        Else
            useinternal = 0
        End If


        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdUpdateCampaign As New SqlCommand("CampaignAdCopy_UpdateCampaignAndCreative", cnn)
                Try
                    cmdUpdateCampaign.CommandType = CommandType.StoredProcedure
                    cmdUpdateCampaign.Parameters.Add(New SqlParameter("@CampaignName", Data.SqlDbType.NVarChar, 100)).Value = Trim(txtCampaignName.Text)
                    cmdUpdateCampaign.Parameters.Add(New SqlParameter("@CampaignCreative", Data.SqlDbType.NVarChar)).Value = Trim(txtHtml.Text)
                    cmdUpdateCampaign.Parameters.Add(New SqlParameter("@ConvertedCreative", Data.SqlDbType.NVarChar)).Value = Trim(Creative)
                    cmdUpdateCampaign.Parameters.Add(New SqlParameter("@EmailsOrdered", Data.SqlDbType.Int)).Value = Trim(txtEmailQuanity.Text)
                    cmdUpdateCampaign.Parameters.Add(New SqlParameter("@IsActive", Data.SqlDbType.TinyInt)).Value = IsActive
                    cmdUpdateCampaign.Parameters.Add(New SqlParameter("@Status", Data.SqlDbType.TinyInt)).Value = ddStatus.SelectedValue
                    If radRegNo.Checked = False Then
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@CouponVariable", Data.SqlDbType.NVarChar, 50)).Value = Trim(txtCouponVariable.Text)
                    Else
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@CouponVariable", Data.SqlDbType.NVarChar, 50)).Value = DBNull.Value
                    End If

                    If Len(Trim(txtSubject.Text)) > 0 Or Trim(txtSubject.Text) <> String.Empty Then
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@SubjectLine", Data.SqlDbType.NVarChar, 250)).Value = Trim(txtSubject.Text)
                    Else
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@SubjectLine", Data.SqlDbType.NVarChar, 250)).Value = DBNull.Value
                    End If
                    cmdUpdateCampaign.Parameters.Add(New SqlParameter("@Clicks", Data.SqlDbType.Int)).Value = CInt(txtClicks.Text)
                    cmdUpdateCampaign.Parameters.Add(New SqlParameter("@Impressions", Data.SqlDbType.Int)).Value = CInt(txtImpressions.Text)

                    If Len(Trim(txtOpenLink1.Text)) > 0 Or Trim(txtOpenLink1.Text) <> String.Empty Then
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@OpenLink1", Data.SqlDbType.NVarChar, 250)).Value = Trim(txtOpenLink1.Text)
                    Else
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@OpenLink1", Data.SqlDbType.NVarChar, 250)).Value = DBNull.Value
                    End If

                    If Len(Trim(txtOpenLink2.Text)) > 0 Or Trim(txtOpenLink2.Text) <> String.Empty Then
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@OpenLink2", Data.SqlDbType.NVarChar, 250)).Value = Trim(txtOpenLink2.Text)
                    Else
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@OpenLink2", Data.SqlDbType.NVarChar, 250)).Value = DBNull.Value
                    End If

                    If Len(Trim(txtOpenLink3.Text)) > 0 Or Trim(txtOpenLink3.Text) <> String.Empty Then
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@OpenLink3", Data.SqlDbType.NVarChar, 250)).Value = Trim(txtOpenLink3.Text)
                    Else
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@OpenLink3", Data.SqlDbType.NVarChar, 250)).Value = DBNull.Value
                    End If
                    cmdUpdateCampaign.Parameters.Add(New SqlParameter("@InternalOpenLink", Data.SqlDbType.TinyInt)).Value = useinternal
                    cmdUpdateCampaign.Parameters.Add(New SqlParameter("@FullFillBy", Data.SqlDbType.DateTime)).Value = Trim(txtFullFill.Text)
                    cmdUpdateCampaign.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = ddCampaign.SelectedValue
                    cmdUpdateCampaign.ExecuteNonQuery()
                Catch ex As Exception
                    lblmsg.Text = "There was a problem with the update please try agian in a few min."
                    lblmsg.Font.Bold = True
                    lblmsg.ForeColor = Drawing.Color.Red
                Finally
                    lblmsg.Text = "Campaign " & ddCampaign.SelectedItem.Text & " has Successfully been updated."
                    lblmsg.Font.Bold = True
                End Try
            End Using
        End Using

    End Sub

    Private Sub UpdateEzangaAdInfo()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdUpdateEzangaAdInfo As New SqlCommand("Ezanga_Ad_UpdateAd", cnn)
                cmdUpdateEzangaAdInfo.CommandType = CommandType.StoredProcedure
                cmdUpdateEzangaAdInfo.Parameters.Add(New SqlParameter("@AdTitle", Data.SqlDbType.NVarChar, 70)).Value = Trim(txtETitle.Text)
                cmdUpdateEzangaAdInfo.Parameters.Add(New SqlParameter("@AdDescription", Data.SqlDbType.NVarChar, 200)).Value = Trim(txtEDiscription.Text)
                cmdUpdateEzangaAdInfo.Parameters.Add(New SqlParameter("@DisplayURL", Data.SqlDbType.NVarChar, 70)).Value = Trim(txtEDisplay.Text)
                cmdUpdateEzangaAdInfo.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = ddCampaign.SelectedValue
            End Using
        End Using
    End Sub

    Private Sub InsetEzangaInfo()
        If Len(Trim(txtETitle.Text)) > 0 Then
            Using cnn As New SqlConnection(strConn)
                cnn.Open()
                Using cmdInsertEzangaInfo As New SqlCommand("Ezanga_Ad_New", cnn)
                    cmdInsertEzangaInfo.CommandType = CommandType.StoredProcedure
                    cmdInsertEzangaInfo.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = CampaignId
                    cmdInsertEzangaInfo.Parameters.Add(New SqlParameter("@AdTitle", SqlDbType.NVarChar)).Value = Trim(txtETitle.Text)
                    cmdInsertEzangaInfo.Parameters.Add(New SqlParameter("@AdDescription", SqlDbType.NVarChar)).Value = Trim(txtEDiscription.Text)
                    cmdInsertEzangaInfo.Parameters.Add(New SqlParameter("@DisplayURL", SqlDbType.NVarChar)).Value = Trim(txtEDisplay.Text)
                    cmdInsertEzangaInfo.ExecuteNonQuery()
                End Using
            End Using
        End If
    End Sub

    Private Sub DeleteOldCampaign()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdDeleteOldCampaign As New SqlCommand("Campaign_DeleteCampaign", cnn)
                cmdDeleteOldCampaign.CommandType = CommandType.StoredProcedure
                cmdDeleteOldCampaign.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = ddCampaign.SelectedValue
                cmdDeleteOldCampaign.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Sub GetCompanyInfo()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetCompanyInfo As New SqlCommand("Company_GetInfo", cnn)
                cmdGetCompanyInfo.CommandType = Data.CommandType.StoredProcedure
                Using dtrGetCompanyInfo As SqlDataReader = cmdGetCompanyInfo.ExecuteReader
                    While dtrGetCompanyInfo.Read
                        ConvertLink = dtrGetCompanyInfo("CreativeRedirectPath")
                        ImpressionPath = dtrGetCompanyInfo("ImpressionPath")
                        CouponPath = dtrGetCompanyInfo("CouponPath")
                    End While
                End Using
            End Using
        End Using
    End Sub

    Private Sub GetCouponTableNumber()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetCouponTableNumber As New SqlCommand("Company_CouponTableNumber_GetNext", cnn)
                cmdGetCouponTableNumber.CommandType = CommandType.StoredProcedure
                CouponTableNumber = cmdGetCouponTableNumber.ExecuteScalar
            End Using
        End Using
    End Sub

#End Region

#Region "Formm Check"

    Private Function CheckForm() As Boolean
        Dim GoodForm As Boolean = False
        Dim clicksOk As Boolean = False
        Dim ImpressionsOk As Boolean = False
        Dim Impressions1Ok As Boolean = False
        Dim Impressions2Ok As Boolean = False
        Dim Impressions3Ok As Boolean = False
        Dim Clicks As Integer
        Dim Impressions As Integer



        If Len(Trim(txtClicks.Text)) = 0 And Len(Trim(txtImpressions.Text)) = 0 Then
            lblmsg.Text = "You must enter either a click or impression amount or both."
            lblmsg.Font.Bold = True
            lblmsg.ForeColor = Drawing.Color.Red
            Return GoodForm
            Exit Function
        End If


        If Len(Trim(txtClicks.Text)) > 0 Then
            If IsNumeric(txtClicks.Text) = True Then
                If wholeNumberCheck(txtClicks.Text) = True Then
                    clicksOk = True
                Else
                    lblmsg.Text = "Clicks must be a numeric whole number."
                    lblmsg.Font.Bold = True
                    lblmsg.ForeColor = Drawing.Color.Red
                    Return GoodForm
                    Exit Function
                End If
            Else
                lblmsg.Text = "Clicks must be a numeric number."
                lblmsg.Font.Bold = True
                lblmsg.ForeColor = Drawing.Color.Red
                Return GoodForm
                Exit Function
            End If
        Else
            clicksOk = True
        End If

        If Len(Trim(txtImpressions.Text)) > 0 Then
            If IsNumeric(txtImpressions.Text) = True Then
                If wholeNumberCheck(txtImpressions.Text) = True Then
                    ImpressionsOk = True
                Else
                    lblmsg.Text = "Impressions must be a numeric whole number."
                    lblmsg.Font.Bold = True
                    lblmsg.ForeColor = Drawing.Color.Red
                    Return GoodForm
                    Exit Function
                End If
            Else
                lblmsg.Text = "Impressions must be a numeric number."
                lblmsg.Font.Bold = True
                lblmsg.ForeColor = Drawing.Color.Red
                Return GoodForm
                Exit Function
            End If
        Else
            ImpressionsOk = True
        End If

        If Len(Trim(txtClicks.Text)) = 0 Then
            Clicks = 0
        Else
            Clicks = CInt(txtClicks.Text)
        End If
        If Len(Trim(txtImpressions.Text)) = 0 Then
            Impressions = 0
        Else
            Impressions = CInt(txtImpressions.Text)
        End If

        If Impressions = 0 And Clicks = 0 Then
            lblmsg.Text = "Either Clicks or Impression must be a numeric whole number greater than 0."
            lblmsg.Font.Bold = True
            lblmsg.ForeColor = Drawing.Color.Red
            Return GoodForm
            Exit Function
        End If


        If Len(Trim(txtImpressions.Text)) > 0 And txtImpressions.Text <> 0 And radInternalNo.Checked = True Then
            If Len(Trim(txtOpenLink1.Text)) > 0 Then
                If OpenLinkCheck(Trim(txtOpenLink1.Text), 1) = True Then
                    Impressions1Ok = True
                Else
                    Return GoodForm
                    Exit Function
                End If
            Else
                lblmsg.Text = "You must enter an open link."
                lblmsg.Font.Bold = True
                lblmsg.ForeColor = Drawing.Color.Red
                Return GoodForm
                Exit Function
            End If

            If Len(Trim(txtOpenLink2.Text)) > 0 Then
                If OpenLinkCheck(Trim(txtOpenLink2.Text), 2) = True Then
                    Impressions2Ok = True
                Else
                    Return GoodForm
                    Exit Function
                End If
            Else
                Impressions2Ok = True
            End If

            If Len(Trim(txtOpenLink3.Text)) > 0 Then
                If OpenLinkCheck(Trim(txtOpenLink3.Text), 3) = True Then
                    Impressions3Ok = True
                Else
                    Return GoodForm
                    Exit Function
                End If
            Else
                Impressions3Ok = True
            End If
        Else
            Impressions1Ok = True
            Impressions2Ok = True
            Impressions3Ok = True
        End If

        If clicksOk = True And Impressions1Ok = True And Impressions2Ok = True And Impressions3Ok = True Then
            GoodForm = True
        End If

        If GoodForm = True Then
            GoodForm = False
            Dim PercentIsSet As Boolean
            PercentIsSet = HasPercentBeenSet()
            Select Case ddStatus.SelectedValue
                Case Is <= 3
                    If PercentIsSet = False Then
                        GoodForm = True
                        Exit Select
                    Else
                        ResetPercent()
                        GoodForm = True
                        Exit Select
                    End If
                Case 4
                    If PercentIsSet = False Then
                        lblmsg.Text = "You cannot set the status of this campaign to Percentage set the link percentages have NOT BEEN SET!!."
                        lblmsg.Font.Bold = True
                        lblmsg.ForeColor = Drawing.Color.Red
                        Return GoodForm
                        Exit Function
                    End If
                Case Is > 4
                    If PercentIsSet = True Then
                        GoodForm = True
                        Exit Select
                    Else
                        lblmsg.Text = "You cannot set the status of this campaign to anything above seed sent, the percentages have not been set!!."
                        lblmsg.Font.Bold = True
                        lblmsg.ForeColor = Drawing.Color.Red
                        Return GoodForm
                        Exit Function
                    End If
            End Select
        End If
        Return GoodForm

    End Function

    Private Sub ResetPercent()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdResetPercent As New SqlCommand("Campaign_ResetLinkPercent", cnn)
                cmdResetPercent.CommandType = CommandType.StoredProcedure
                cmdResetPercent.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = ddCampaign.SelectedValue
                cmdResetPercent.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Function HasPercentBeenSet() As Boolean
        Dim rtn As Boolean = False
        Dim percent As Integer = 0
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdHasPercentBeenSet As New SqlCommand("Campaign_GetByCampaignId", cnn)
                cmdHasPercentBeenSet.CommandType = CommandType.StoredProcedure
                cmdHasPercentBeenSet.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = ddCampaign.SelectedValue
                Using dtrHasPercentBeenSet As SqlDataReader = cmdHasPercentBeenSet.ExecuteReader
                    While dtrHasPercentBeenSet.Read
                        If dtrHasPercentBeenSet("LinkPercent") > 0 Then
                            rtn = True
                            Return rtn
                            Exit Function
                        End If
                    End While
                End Using
            End Using
        End Using
        Return rtn
    End Function



    Private Function wholeNumberCheck(ByVal num As Decimal) As Boolean
        Dim wholenumber As Boolean = False
        Dim i As Integer = 1
        If (num / i).ToString.Contains(".") = False Then
            wholenumber = True
        End If

        Return wholenumber

    End Function

    Private Function OpenLinkCheck(ByVal link As String, ByVal id As Integer) As Boolean
        Dim ValidLink As Boolean = False
        ImageOpenLink = link
        ImageOpenLink = Replace(ImageOpenLink, "'", """", 1, -1, CompareMethod.Text)
        ImageOpenLink = Replace(ImageOpenLink, "/>", ">", 1, -1, CompareMethod.Text)
        ImageOpenLink = Replace(ImageOpenLink, "/ >", ">", 1, -1, CompareMethod.Text)
        ImageOpenLink = Replace(ImageOpenLink, "BORDER", "border", 1, -1, CompareMethod.Text)
        ImageOpenLink = Replace(ImageOpenLink, "Border", "border", 1, -1, CompareMethod.Text)
        ImageOpenLink = Replace(ImageOpenLink, "border = ", "border=", 1, -1, CompareMethod.Text)
        ImageOpenLink = Replace(ImageOpenLink, "border= """, "border=""", 1, -1, CompareMethod.Text)
        ImageOpenLink = Replace(ImageOpenLink, "border=  """, "border=""", 1, -1, CompareMethod.Text)

        Dim myRegex As New Regex( _
        "^<img[^>]*src\s*=\s*['|""]?([^>]*?)['|""]?[^>]*>$")

        Dim bordercheck As String = "border="
        Dim bRegex As New Regex( _
        bordercheck, RegexOptions.IgnoreCase)
        Dim myMatch As Match = Regex.Match(ImageOpenLink, bordercheck)

        If myRegex.IsMatch(ImageOpenLink) = True Then
            ValidLink = True
            If myMatch.Success Then
                Dim startofborder As Integer = InStr(ImageOpenLink, bordercheck)
                Dim rcreative As String = Right(ImageOpenLink, Len(ImageOpenLink) - startofborder - 7)
                Dim endoflink As Integer = InStr(rcreative, """")
                Dim replacestring As String = Left(rcreative, endoflink - 1)
                ConvertBorder(replacestring, startofborder, Len(replacestring), id)
            Else
                InsertBorder(id)
            End If
        Else
            lblmsg.Text = "Open link " & id & " is not a valid html image tag."
            lblmsg.Font.Bold = True
            lblmsg.ForeColor = Drawing.Color.Red
        End If
        Return ValidLink
    End Function

    Private Sub ConvertBorder(ByVal link As String, ByVal startpos As Integer, ByVal length As Integer, ByVal id As Integer)
        Dim nstr As String = "0"
        ImageOpenLink = ImageOpenLink.Remove(startpos + 7, length)
        ImageOpenLink = ImageOpenLink.Insert(startpos + 7, nstr)
        Select Case id
            Case 1
                OpenLink1 = ImageOpenLink
            Case 2
                OpenLink2 = ImageOpenLink
            Case 3
                OpenLink3 = ImageOpenLink
        End Select

    End Sub

    Private Sub InsertBorder(ByVal id As Integer)
        Dim link As String = " border=""0"""
        ImageOpenLink = ImageOpenLink.Insert(ImageOpenLink.Length - 1, link)
        Select Case id
            Case 1
                OpenLink1 = ImageOpenLink
            Case 2
                OpenLink2 = ImageOpenLink
            Case 3
                OpenLink3 = ImageOpenLink
        End Select
    End Sub


    Private Sub CreateCouponTable()
        Try
            Using cnn As New SqlConnection(strConn)
                cnn.Open()
                Using cmdCreateCouponTable As New SqlCommand("Coupon_TableNumber_Create", cnn)
                    cmdCreateCouponTable.CommandType = CommandType.StoredProcedure
                    cmdCreateCouponTable.Parameters.Add(New SqlParameter("@Table_Number", Data.SqlDbType.NVarChar)).Value = CampaignId
                    cmdCreateCouponTable.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception

        End Try
    End Sub
#End Region

End Class
