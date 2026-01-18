Imports System.Data
Imports System.Data.SqlClient

Partial Class CampaignCreateCreative
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
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
    Private UseInternal As Integer
    Public CampaignType As Integer ' 0 = reg 1 = Seed
    Private CouponTableNumber As Integer
    Private ImageOpenLink As String
    Private OpenLink1 As String
    Private OpenLink2 As String
    Private OpenLink3 As String
    Private FullFillDate As Date
    Private Domain As String
    Private DefaultDomain As String = "http://www.rdn2020.com"
    Private ClickPage As String
    Private DefaultClickPage As String = "P.aspx."
    Private OpenPage As String
    Private DefaultOpenPage As String = "ImageTrack.aspx"
    Private CouponPage As String
    Private DefaultCouponPage As String = "CP.aspx"
    Private BodyStartPosition As Integer


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblhmsg.Text = String.Empty
        lblhmsg.Font.Bold = False
        lblhmsg.ForeColor = Drawing.Color.Black
        If Not IsPostBack Then
            GetClientList()
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
    End Sub

    Protected Sub radRegYes_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radRegYes.CheckedChanged
        trcvar.Visible = False
        rfvCoupon.Enabled = False
        CampaignType = 0
    End Sub

    Protected Sub radRegNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radRegNo.CheckedChanged
        trcvar.Visible = True
        CampaignType = 1
        rfvCoupon.Enabled = True
    End Sub

    Protected Sub ddClient_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddClient.SelectedIndexChanged
        GetIOs()        
    End Sub

    Protected Sub cmdConvert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdConvert.Click
        Page.Validate("frmCampaign")
        If Page.IsValid Then
            If CheckForm() = True Then
                If radRegYes.Checked = True Then
                    CampaignType = 0
                    convert()
                Else
                    CampaignType = 1
                    ConvertSeedCampaign()
                End If           
            End If
        End If
    End Sub

    Protected Sub txtOpenLink1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOpenLink1.TextChanged
        If Trim(Len(txtOpenLink1.Text)) > 0 Then
            txtOpenLink2.Enabled = True
        Else
            txtOpenLink2.Enabled = False
        End If
    End Sub

    Protected Sub txtOpenLink3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOpenLink3.TextChanged

        If Trim(Len(txtOpenLink2.Text)) > 0 Then
            txtOpenLink3.Enabled = True
        Else
            txtOpenLink3.Enabled = False
        End If

    End Sub


    Public Sub ConvertSeedCampaign()
        Creative = txtHtml.Text
        If InStr(Creative, txtCouponVariable.Text) = 0 Then
            lblhmsg.Text = "The coupon variable is not in the creative, please check your variable or the creative and try again."
            lblhmsg.Font.Bold = True
            lblhmsg.ForeColor = Drawing.Color.Red
            'Else
            'Creative = Replace(Creative, txtCouponVariable.Text, "{CPLink}", 1, -1, CompareMethod.Text)
        End If

        If Trim(Len(Creative)) > 0 Or Creative <> String.Empty Then
            GetCampaignId()
            GetCompanyInfo()
            CreateCouponTable()
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
                    TransformSeed(replacestring, StartOfLink, Len(replacestring))
                End If
            Loop
            Creative = Replace(Creative, "href={Replaced}", "href=", 1, -1, CompareMethod.Text)
            Creative = Creative & OpenLink
            Insertcreative()
            InsetEzangaInfo()
        Else
            txtHtml.Text = "You need to enter a creative"
            txtHtml.Font.Bold = True
            txtHtml.ForeColor = Drawing.Color.Red
            txtHtml.Font.Size = 24
        End If
        txtConverted.Text &= Creative
        Response.Redirect("CampaignList.aspx")
    End Sub

    Public Sub convert()
        Creative = txtHtml.Text        
        If Trim(Len(Creative)) > 0 Or Creative <> String.Empty Then
            GetCampaignId()
            GetCompanyInfo()
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
            Insertcreative()
            InsetEzangaInfo()
        Else
            txtHtml.Text = "You need to enter a creative"
            txtHtml.Font.Bold = True
            txtHtml.ForeColor = Drawing.Color.Red
            txtHtml.Font.Size = 24
        End If
        txtConverted.Text &= Creative
        Response.Redirect("CampaignList.aspx")
    End Sub

    Private Sub TransformSeed(ByVal link As String, ByVal startpos As Integer, ByVal length As Integer)
        If startpos > BodyStartPosition Then
            If InStr(link, "http") > 0 Then
                If InStr(link.ToLower, "mailto:") = 0 Then
                    CouponPath = Domain & "/" & CouponPage & "?c=@CampaignId~@LinkId"
                    LinkId = LinkId + 1
                    Dim nstr As String = Trim(CouponPath)
                    nstr = Replace(nstr, "@CampaignId", Trim(CampaignId))
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
                    nstr = Replace(nstr, "@CampaignId", Trim(CampaignId))
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

    Private Sub Insertcreative()        
        Dim MyGuid As Guid = Guid.NewGuid()       
        FullFillDate = DateAdd(DateInterval.Day, +ddfullfill.SelectedValue, Today())
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdInsertcreative As New SqlCommand("CampaignAdCopy_InsertNew", cnn)
                cmdInsertcreative.CommandType = Data.CommandType.StoredProcedure
                cmdInsertcreative.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = CampaignId
                cmdInsertcreative.Parameters.Add(New SqlParameter("@CampaignUId", SqlDbType.UniqueIdentifier)).Value = MyGuid
                cmdInsertcreative.Parameters.Add(New SqlParameter("@CampaignName", SqlDbType.NVarChar, 100)).Value = Trim(txtCampaignName.Text)
                cmdInsertcreative.Parameters.Add(New SqlParameter("@IOUId", SqlDbType.UniqueIdentifier)).Value = SqlTypes.SqlGuid.Parse(ddIO.SelectedValue)
                cmdInsertcreative.Parameters.Add(New SqlParameter("@CampaignCreative", Data.SqlDbType.NVarChar)).Value = Trim(txtHtml.Text)
                cmdInsertcreative.Parameters.Add(New SqlParameter("@ConvertedCreative", Data.SqlDbType.NVarChar)).Value = Trim(Creative)
                cmdInsertcreative.Parameters.Add(New SqlParameter("@EmailsOrdered", Data.SqlDbType.NVarChar)).Value = Trim(txtEmailQuanity.Text)
                If CampaignType = 1 Then
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@CouponVariable", Data.SqlDbType.NVarChar)).Value = Trim(txtCouponVariable.Text)
                Else
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@CouponVariable", Data.SqlDbType.NVarChar)).Value = DBNull.Value
                End If
                cmdInsertcreative.Parameters.Add(New SqlParameter("@SubjectLine", Data.SqlDbType.NVarChar)).Value = Trim(txtSubject.Text)
                If Len(Trim(txtClicks.Text)) > 0 Or Trim(txtClicks.Text) <> String.Empty Then
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@Clicks", Data.SqlDbType.Int)).Value = CInt(Trim(txtClicks.Text))
                Else
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@Clicks", Data.SqlDbType.Int)).Value = 0
                End If

                If Len(Trim(txtImpressions.Text)) > 0 Or Trim(txtImpressions.Text) <> String.Empty Then
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@Impressions", Data.SqlDbType.Int)).Value = CInt(Trim(txtImpressions.Text))
                Else
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@Impressions", Data.SqlDbType.Int)).Value = 0
                End If

                If Len(Trim(txtOpenLink1.Text)) > 0 Or Trim(txtOpenLink1.Text) <> String.Empty Then
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@OpenLink1", Data.SqlDbType.NVarChar)).Value = Trim(OpenLink1)
                Else
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@OpenLink1", Data.SqlDbType.NVarChar)).Value = DBNull.Value
                End If

                If Len(Trim(txtOpenLink2.Text)) > 0 Or Trim(txtOpenLink2.Text) <> String.Empty Then
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@OpenLink2", Data.SqlDbType.NVarChar)).Value = Trim(OpenLink2)
                Else
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@OpenLink2", Data.SqlDbType.NVarChar)).Value = DBNull.Value
                End If

                If Len(Trim(txtOpenLink3.Text)) > 0 Or Trim(txtOpenLink3.Text) <> String.Empty Then
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@OpenLink3", Data.SqlDbType.NVarChar)).Value = Trim(OpenLink3)
                Else
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@OpenLink3", Data.SqlDbType.NVarChar)).Value = DBNull.Value
                End If

                If radInternalYes.Checked = True Then
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@InternalOpenLink", Data.SqlDbType.TinyInt)).Value = 1
                Else
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@InternalOpenLink", Data.SqlDbType.TinyInt)).Value = 0
                End If
                cmdInsertcreative.Parameters.Add(New SqlParameter("@FullFillBy", Data.SqlDbType.DateTime)).Value = FullFillDate
                cmdInsertcreative.ExecuteNonQuery()
            End Using
        End Using
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

    Private Sub InsetEzangaInfo()
        If Len(Trim(txtETitle.Text)) > 0 Then
            Using cnn As New SqlConnection(strConn)
                cnn.Open()
                Using cmdInsertEzangaInfo As New SqlCommand("Ezanga_Ad_New", cnn)
                    cmdInsertEzangaInfo.CommandType = CommandType.StoredProcedure
                    cmdInsertEzangaInfo.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = CampaignId
                    cmdInsertEzangaInfo.Parameters.Add(New SqlParameter("@AdTitle", SqlDbType.NVarChar)).Value = Trim(txtETitle.Text)
                    cmdInsertEzangaInfo.Parameters.Add(New SqlParameter("@AdDescription", SqlDbType.NVarChar)).Value = Trim(txtEDescription.Text)
                    cmdInsertEzangaInfo.Parameters.Add(New SqlParameter("@DisplayURL", SqlDbType.NVarChar)).Value = Trim(txtEDisplay.Text)
                    cmdInsertEzangaInfo.ExecuteNonQuery()
                End Using
            End Using
        End If
    End Sub

    Private Sub GetCampaignId()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetCampaignId As New SqlCommand("Campaign_CampaignId_GetNext", cnn)
                cmdGetCampaignId.CommandType = Data.CommandType.StoredProcedure
                CampaignId = cmdGetCampaignId.ExecuteScalar
            End Using
        End Using
    End Sub


    Private Sub GetIOs()
        Dim dadGetIOs As New SqlDataAdapter
        Dim dstGetIOs As New DataSet
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetIOs As New SqlCommand("IO_Get_ForCampiagnCreation", cnn)
                cmdGetIOs.CommandType = CommandType.StoredProcedure
                cmdGetIOs.Parameters.Add(New SqlParameter("@ClientUId", SqlDbType.UniqueIdentifier)).Value = SqlTypes.SqlGuid.Parse(ddClient.SelectedValue)
                dadGetIOs.SelectCommand = cmdGetIOs
                dadGetIOs.Fill(dstGetIOs, "IOs")
                If dstGetIOs.Tables("IOs").Rows.Count = 0 Then
                    txtHtml.Text = "There are no active IO's." & vbCrLf & _
                    "Please create an IO for this Client."
                    txtHtml.Font.Bold = True
                    cmdConvert.Enabled = False
                Else
                    Dim Dyncolumn As New DataColumn
                    With Dyncolumn
                        .ColumnName = "Name"
                        .DataType = System.Type.GetType("System.String")
                        .Expression = "IOId+'   '+IOName"
                    End With
                    dstGetIOs.Tables("IOs").Columns.Add(Dyncolumn)
                    ddIO.DataTextField = "Name"
                    ddIO.DataValueField = "IOUId"
                    ddIO.DataSource = dstGetIOs.Tables("IOs").DefaultView
                    ddIO.DataBind()
                End If
            End Using
        End Using
        ddIO.Items.Insert(0, "Select IO")
        dadGetIOs.Dispose()
        dstGetIOs.Dispose()
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
                    End While
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

    Private Sub GetClientList()
        ddClient.Items.Add(New ListItem("Select Client", 0))
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetClientList As New SqlCommand("Client_Get", cnn)
                Using dtrGetClientList As SqlDataReader = cmdGetClientList.ExecuteReader
                    ddClient.DataSource = dtrGetClientList
                    ddClient.DataBind()
                End Using
            End Using
        End Using
        ddClient.Items.Insert(0, "Select Client")
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

    Private Sub CreateCouponTable()       
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdCreateCouponTable As New SqlCommand("Coupon_TableNumber_Create", cnn)
                cmdCreateCouponTable.CommandType = CommandType.StoredProcedure
                cmdCreateCouponTable.Parameters.Add(New SqlParameter("@Table_Number", Data.SqlDbType.NVarChar)).Value = CampaignId
                cmdCreateCouponTable.ExecuteNonQuery()
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
