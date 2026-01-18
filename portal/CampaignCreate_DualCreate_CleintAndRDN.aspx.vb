Imports System.Data
Imports System.Data.SqlClient
Imports lga4040

Partial Class portal_CampaignCreateForClientEmailReportSite
    Inherits Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Private ClientDefaultValue As String = "26b7f385-13d8-402c-bca7-30ee6757528f"
    Private dsClient As DataSet    
    Private Client As Client
    Private ClientStrut As Client.StructClient
    Private dsClientsClient As DataSet
    Private ClientsClient As Client
    Private ClientsClientSturt As Client.StructClient
    Private dsIO As DataSet
    Private _io As IO
    Private IOStrut As IO.StructIO
    Private CouponPath As String
    Private UseInternal As Integer
    Public CampaignType As Integer ' 0 = reg 1 = Seed    
    Private ImageOpenLink As String
    Private OpenLink1 As String
    Private OpenLink2 As String
    Private OpenLink3 As String
    Private URLMask As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblhmsg.Text = String.Empty
        lblhmsg.Font.Bold = False
        lblhmsg.ForeColor = Drawing.Color.Black
        If Not IsPostBack Then
            GetClientList()
            ddClient.SelectedValue = ClientDefaultValue  'This Will be removed once this is expanded to do all clients
            If ddClient.SelectedValue <> "Select Client" Then
                GetIOs()
                GetClientsClientList()
            End If
            GetCompanyInfo()
            ddfullfill.SelectedValue = 2
            If UseInternal = 0 Then
                radInternalYes.Checked = False
                radInternalNo.Checked = True
            Else
                radInternalYes.Checked = True
                radInternalNo.Checked = False
            End If
            radRegYes.Checked = True

            trcvar.Visible = False
        End If
        If radRegYes.Checked = True Then
            CampaignType = 0
        Else
            CampaignType = 1
        End If
    End Sub

    Protected Sub radRegYes_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radRegYes.CheckedChanged
        trcvar.Visible = False
        rfvCoupon.Enabled = False
        If radRegYes.Checked = True Then
            CampaignType = 0
        Else
            CampaignType = 1
        End If
    End Sub

    Protected Sub radRegNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radRegNo.CheckedChanged
        trcvar.Visible = True
        If radRegYes.Checked = True Then
            CampaignType = 0
        Else
            CampaignType = 1
        End If
        rfvCoupon.Enabled = True
    End Sub

    Protected Sub ddClient_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddClient.SelectedIndexChanged
        GetIOs()
        GetClientsClientList()
    End Sub

    Protected Sub ddClientsClient_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddClientsClient.SelectedIndexChanged
        GetClientsClientIO()
    End Sub

    Protected Sub cmdConvert_Click(sender As Object, e As System.EventArgs) Handles cmdConvert.Click
        Page.Validate("frmCampaign")
        If Page.IsValid Then
            If CheckForm() = True Then
                Dim c As New Campaign.StructCampaign
                Dim cS As New Campaign.StructCampaign
                Dim Nc As New Campaign
                c.CampaignType = CampaignType
                c.ClientUID = SqlTypes.SqlGuid.Parse(ddClient.SelectedValue)
                c.ClientsClientUID = SqlTypes.SqlGuid.Parse(ddClientsClient.SelectedValue)
                c.CampaignName = Trim(txtCampaignName.Text)
                c.IOUId = SqlTypes.SqlGuid.Parse(ddIO.SelectedValue)
                c.ClientsIOUID = SqlTypes.SqlGuid.Parse(ddClientsClientIO.SelectedValue)
                c.ClientsFriendlyFrom = Trim(txtClientsFriendlyFrom.Text)
                c.ClientsNotes = Trim(txtNote.Text)
                c.ClientsBroadcastDate = Date.Today
                c.ClientsCampaignCreative = Trim(txtHtml.Text)
                c.EmailsOrdered = Trim(txtEmailQuanity.Text)
                c.SubjectLine = Trim(txtSubject.Text)
                c.CouponVariable = Trim(txtCouponVariable.Text)
                If Len(Trim(txtClicks.Text)) = 0 Or Trim(txtClicks.Text) = String.Empty Then
                    c.Clicks = 0
                Else
                    c.Clicks = CInt(Trim(txtClicks.Text))
                End If
                If Len(Trim(txtImpressions.Text)) = 0 Or Trim(txtImpressions.Text) = String.Empty Then
                    c.Impressions = 0
                Else
                    c.Impressions = CInt(Trim(txtImpressions.Text))
                End If
                If Len(Trim(txtOpenLink1.Text)) > 0 Or Trim(txtOpenLink1.Text) <> String.Empty Then
                    c.OpenLink1 = Trim(txtOpenLink1.Text)
                Else
                    c.OpenLink1 = String.Empty
                End If

                If Len(Trim(txtOpenLink2.Text)) > 0 Or Trim(txtOpenLink2.Text) <> String.Empty Then
                    c.OpenLink2 = Trim(txtOpenLink2.Text)
                Else
                    c.OpenLink2 = String.Empty
                End If

                If Len(Trim(txtOpenLink3.Text)) > 0 Or Trim(txtOpenLink3.Text) <> String.Empty Then
                    c.OpenLink3 = Trim(txtOpenLink3.Text)
                Else
                    c.OpenLink3 = String.Empty
                End If
                If radInternalYes.Checked = True Then
                    c.InternalOpenLink = 1
                Else
                    c.InternalOpenLink = 0
                End If
                c.FullFillBy = DateAdd(DateInterval.Day, +ddfullfill.SelectedValue, Today())
                cS = Nc.InsertDualCampaigns(c)

                If cS.Message = String.Empty Then
                    txtConverted.Text = cS.ConvertedCreative
                    Response.Redirect("CampaignList.aspx")
                Else
                    lblhmsg.Text = cS.Message
                    lblhmsg.Font.Bold = True
                    lblhmsg.ForeColor = Drawing.Color.DarkRed
                End If
            End If
        End If
    End Sub

    Private Sub GetIOs()
        _io = New IO
        dsIO = _io.GetIOs(SqlTypes.SqlGuid.Parse(ddClient.SelectedValue))
        ddIO.DataTextField = "FullName"
        ddIO.DataValueField = "IOUId"
        ddIO.DataSource = dsIO
        ddIO.DataBind()
        ddIO.Items.Insert(0, "Select IO")
        dsIO.Dispose()
    End Sub

    Private Sub GetClientList()
        Client = New Client
        dsClient = Client.GetClients
        ddClient.DataTextField = "ClientName"
        ddClient.DataValueField = "ClientUId"
        ddClient.DataSource = dsClient
        ddClient.DataBind()
        ddClient.Items.Insert(0, "Select Client")
        dsClient.Dispose()

    End Sub

    Private Sub GetClientsClientList()
        ClientsClient = New Client
        dsClientsClient = ClientsClient.GetClientsClient(SqlTypes.SqlGuid.Parse(ddClient.SelectedValue))
        ddClientsClient.DataTextField = "ClientName"
        ddClientsClient.DataValueField = "ClientUId"
        ddClientsClient.DataSource = dsClientsClient
        ddClientsClient.DataBind()
        ddClientsClient.Items.Insert(0, "Select Client's Client")
        dsClientsClient.Dispose()
    End Sub

    Private Sub GetClientsClientIO()
        _io = New IO
        dsIO = _io.GetClientsClientIO(SqlTypes.SqlGuid.Parse(ddClient.SelectedValue), SqlTypes.SqlGuid.Parse(ddClientsClient.SelectedValue))
        ddClientsClientIO.DataTextField = "Name"
        ddClientsClientIO.DataValueField = "IOUID"
        ddClientsClientIO.DataSource = dsIO
        ddClientsClientIO.DataBind()
        ddClientsClientIO.Items.Insert(0, "Select Clients Client IO")
    End Sub
   

    Private Sub GetCompanyInfo()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetCompanyInfo As New SqlCommand("Company_GetInfo", cnn)
                cmdGetCompanyInfo.CommandType = Data.CommandType.StoredProcedure
                Using dtrGetCompanyInfo As SqlDataReader = cmdGetCompanyInfo.ExecuteReader
                    While dtrGetCompanyInfo.Read
                        'ConvertLink = dtrGetCompanyInfo("CreativeRedirectPath")
                        'ImpressionPath = dtrGetCompanyInfo("ImpressionPath")
                        CouponPath = dtrGetCompanyInfo("CouponPath")
                        UseInternal = dtrGetCompanyInfo("Useinternalopen")
                        URLMask = dtrGetCompanyInfo("URLMask")
                    End While
                End Using
            End Using
        End Using
    End Sub

#Region "Form Check"

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
            lblhmsg.Text = "You must enter either a click or impression amount or both."
            lblhmsg.Font.Bold = True
            lblhmsg.ForeColor = Drawing.Color.Red
            Return GoodForm
            Exit Function
        End If


        If Len(Trim(txtClicks.Text)) > 0 Then
            If IsNumeric(txtClicks.Text) = True Then
                If wholeNumberCheck(txtClicks.Text) = True Then
                    clicksOk = True
                Else
                    lblhmsg.Text = "Clicks must be a numeric whole number."
                    lblhmsg.Font.Bold = True
                    lblhmsg.ForeColor = Drawing.Color.Red
                    Return GoodForm
                    Exit Function
                End If
            Else
                lblhmsg.Text = "Clicks must be a numeric number."
                lblhmsg.Font.Bold = True
                lblhmsg.ForeColor = Drawing.Color.Red
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
                    lblhmsg.Text = "Impressions must be a numeric whole number."
                    lblhmsg.Font.Bold = True
                    lblhmsg.ForeColor = Drawing.Color.Red
                    Return GoodForm
                    Exit Function
                End If
            Else
                lblhmsg.Text = "Impressions must be a numeric number."
                lblhmsg.Font.Bold = True
                lblhmsg.ForeColor = Drawing.Color.Red
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
            lblhmsg.Text = "Either Clicks or Impression must be a numeric whole number greater than 0."
            lblhmsg.Font.Bold = True
            lblhmsg.ForeColor = Drawing.Color.Red
            Return GoodForm
            Exit Function
        End If


        If Len(Trim(txtImpressions.Text)) > 0 And radInternalNo.Checked = True Then
            If Len(Trim(txtOpenLink1.Text)) > 0 Then
                If OpenLinkCheck(Trim(txtOpenLink1.Text), 1) = True Then
                    Impressions1Ok = True
                Else
                    Return GoodForm
                    Exit Function
                End If
            Else
                lblhmsg.Text = "You must enter an open link."
                lblhmsg.Font.Bold = True
                lblhmsg.ForeColor = Drawing.Color.Red
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

        Return GoodForm

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
            lblhmsg.Text = "Open link " & id & " is not a valid html image tag."
            lblhmsg.Font.Bold = True
            lblhmsg.ForeColor = Drawing.Color.Red
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
#End Region

    
End Class
