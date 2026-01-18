Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System
Imports System.Web
Imports System.Collections

Namespace lga4040
    Public Class Click
        Private Const MetaUrlLink As String = "http://localhost/operationinbox.net/MRProcess.aspx"
        Private ReadOnly _strConn As String = System.Configuration.ConfigurationSettings.AppSettings("cnn")
        Private ReadOnly _webPath As String = System.Configuration.ConfigurationSettings.AppSettings("WebPath")
        Private ReadOnly _ip As String = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
        Private ReadOnly _referrer As String = HttpContext.Current.Request.UrlReferrer.ToString()
        Private _arCampaignLinkInfo(,) As String
        Private _linkIndex As Integer
        Private _totalClicks As Integer
        Private _arProcessedClickInfo(,) As String
        Private _cs As StructClick = New StructClick
        Private _cookieStruct As StructClick = New StructClick
        Private _userTrack As String
        Private _detectScreenUrl As String
        Private _qString As String
        Private _strCampaignId As String = String.Empty
        Private _strLinkId As String = String.Empty
        Private _arraysFilled As Boolean = False
        'Variables for Open Routine
        Private _opensDatabase As String
        Private _opensIp As String
        Private _clientsCampaignId As Integer
        Private _existingOpens As Integer
        Private _opensToAdd As Integer
        Private _finalClicks As Integer
        Private _finalOpens As Integer
        Private _dataBase As String
        Private _tableName As String
        Private _countInCurrentMonth As Boolean = True
        'Variables for Coupon Routine
        Private _couponCode As String
        Private _couponGuid As String

#Region "Structures"

        Public Structure StructClick 'CampaignType = 0 Regular  1 Seed Campaign
            Public InLink As String
            Public OutLink As String
            Public Message As String
            Public CampaignId As Integer
            Public Link As String
            Public LinkId As Integer
            Public SubId As String
            Public CouponId As String
            Public ServerParams As String
            Public CampaignType As Integer 'CampaignType = 0 Regular  1 Seed Campaign
            Public CookieThere As Boolean
        End Structure

#End Region

        Public Function Process(ByVal struct As StructClick) As StructClick
            _cs = Struct
            If CheckIfInCampaignsWithOpensTable() = True Then
                GetImpressionNumber()
            End If
            InsertClick(_cs.LinkId)
            If _cs.CampaignType = 1 Then
                If InStr(_cs.OutLink, _cs.CouponId) > 0 Then
                    GetCouponId()
                    _cs.OutLink = Replace(_cs.OutLink, _cs.CouponId, _couponCode, 1, -1, CompareMethod.Text)
                    UpdateCoupon()
                End If
            End If
            _cs.Message = CType(_cs.CookieThere, String)

            _cs.OutLink = MetaUrlLink & "?c=" & HttpContext.Current.Server.UrlEncode(_cs.OutLink)

            Return _cs
        End Function

        Public Function GetCampaignInfo(ByVal struct As StructClick) As StructClick
            _cs = struct
            If InStr(_cs.InLink, "~") > 0 Then
                GetCampaignAndLinksWithTilde()
                GetOutLinkForLinkIdIncludedClicks(_cs.CampaignId, _cs.LinkId)
            ElseIf InStr(_cs.InLink, "&") > 0 Then
                GetCampaignAndLinksWithAmpersign()
                GetOutLinkForLinkIdIncludedClicks(_cs.CampaignId, _cs.LinkId)
            Else
                GetCampaignIdWithNoLinks()
                FillArrays() 'Fills the Arrays and gets the link and outlink
            End If
            Return _cs

        End Function


#Region "Get Campaign Ids"

        Private Sub GetCampaignAndLinksWithAmpersign()
            Dim arrQSting() As String
            arrQSting = _cs.InLink.Split(CType("&", Char))
            If arrQSting.Length = 3 Then
                _cs.CampaignType = 0
            Else
                _cs.CampaignType = 1
            End If
            For x As Integer = 0 To arrQSting.GetUpperBound(0)
                Dim arrvariablevalues() As String
                arrvariablevalues = arrQSting(x).Split(CType("=", Char))
                If _cs.CampaignType = 0 Then
                    Select Case x
                        Case 0
                            _strCampaignId = arrvariablevalues(1)
                        Case 1
                            _strLinkId = arrvariablevalues(1)
                        Case 2
                            _cs.SubId = arrvariablevalues(1)
                    End Select
                Else
                    Select Case x
                        Case 0
                            _strCampaignId = arrvariablevalues(1)
                        Case 1
                            _strLinkId = arrvariablevalues(1)
                        Case 2
                            _cs.SubId = arrvariablevalues(1)
                        Case 3
                            _cs.CouponId = arrvariablevalues(1)
                    End Select
                End If
            Next
            _cs.CampaignId = CType(Right(_strCampaignId, Len(_strCampaignId) - 8), Integer)
            _cs.LinkId = CType(Right(_strLinkId, Len(_strLinkId) - 8), Integer)

        End Sub

        Private Sub GetCampaignIdWithNoLinks()
            Dim arrqString() As String
            arrqString = _cs.InLink.Split(CType("=", Char))
            If Len(arrqString(1)) > 8 Then
                _cs.CampaignId = CType(Right(arrqString(1), Len(arrqString(1)) - 8), Integer)
            Else
                _cs.CampaignId = CType(arrqString(1), Integer)
            End If
        End Sub

        Private Sub GetCampaignAndLinksWithTilde()
            _qString = Right(_cs.InLink, Len(_cs.InLink) - 2)
            'Declare the array and fill with string elements       
            Dim arrqSting() As String
            arrqSting = _cs.InLink.Split(CType("~", Char))
            Dim arCId() As String
            arCId = arrqSting(0).Split(CType("=", Char))
            _cs.CampaignId = CType(arCId(1), Integer)
            _cs.LinkId = CInt(arrqSting(1))
        End Sub

#End Region

#Region "Get LinkIds And/OR OutLinks"

        Private Sub GetOutLinkForLinkIdIncludedClicks(ByVal campaignId As Integer, ByVal linkId As Integer)
            Using cnn As New SqlConnection(_strConn)
                cnn.Open()
                Using cmdProcessClick As New SqlCommand("campaign_getlink", cnn)
                    cmdProcessClick.CommandType = CommandType.StoredProcedure
                    cmdProcessClick.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = campaignId
                    cmdProcessClick.Parameters.Add(New SqlParameter("@LinkId", SqlDbType.Int)).Value = linkId

                    If Not IsDBNull(cmdProcessClick.ExecuteScalar) Then
                        _cs.OutLink = CType(cmdProcessClick.ExecuteScalar, String)
                    Else
                        _cs.OutLink = _webPath & "InactiveCampaign.aspx"
                        Exit Sub
                    End If

                    If Len(_cs.OutLink) = 0 Or _cs.OutLink = String.Empty Then
                        _cs.OutLink = _webPath & "InactiveCampaign.aspx"
                        Exit Sub
                    End If

                End Using
            End Using
        End Sub

        Private Sub GetLinkIdAndOutLinkForCampaignOnlyClicks()
            Dim foundlink As Boolean = False
            'Dim foundlinkindex As Integer

            For a As Integer = 0 To _arProcessedClickInfo.GetUpperBound(0)
                If _arProcessedClickInfo(a, 0) = _arCampaignLinkInfo(_linkIndex, 0) Then
                    foundlink = True
                    'foundlinkindex = a
                    Dim processPercent As Integer = CType(_arProcessedClickInfo(a, 2), Integer)
                    Dim linkPercent As Integer = CType(_arCampaignLinkInfo(_linkIndex, 2), Integer)
                    If processPercent < linkPercent Then
                        Dim lid As Integer = CType(_arCampaignLinkInfo(_linkIndex, 0), Integer)
                        _cs.LinkId = lid
                        _cs.OutLink = _arCampaignLinkInfo(_linkIndex, 1)
                        Exit Sub
                    End If
                    Exit For
                End If
            Next

            If foundlink = False Then
                Dim lid As Integer = CType(_arCampaignLinkInfo(_linkIndex, 0), Integer)
                _cs.LinkId = lid
                _cs.OutLink = _arCampaignLinkInfo(_linkIndex, 1)
                Exit Sub
            End If

            'Check if all links have been processed at least once
            For b As Integer = 0 To _arCampaignLinkInfo.GetUpperBound(0)
                Dim linkProcessed As Boolean = False
                For c As Integer = 0 To _arProcessedClickInfo.GetUpperBound(0)
                    If _arCampaignLinkInfo(b, 0) = _arProcessedClickInfo(c, 0) Then
                        linkProcessed = True
                    End If
                Next
                If b < _arCampaignLinkInfo.GetUpperBound(0) Then
                    If linkProcessed = False Then
                        Dim lid As Integer = CType(_arCampaignLinkInfo(b, 0), Integer)
                        _cs.LinkId = lid
                        _cs.OutLink = _arCampaignLinkInfo(_linkIndex, 1)
                        Exit Sub
                    End If
                End If
            Next

            For d As Integer = 0 To _arCampaignLinkInfo.GetUpperBound(0)
                For e As Integer = 0 To _arProcessedClickInfo.GetUpperBound(0)
                    If _arCampaignLinkInfo(d, 0) = _arProcessedClickInfo(e, 0) Then
                        Dim linkPercent As Integer = CType(_arCampaignLinkInfo(d, 2), Integer)
                        Dim processPercent As Integer = CType(_arProcessedClickInfo(e, 2), Integer)
                        If processPercent < linkPercent Then
                            Dim lid As Integer = CType(_arCampaignLinkInfo(d, 0), Integer)
                            _cs.LinkId = lid
                            _cs.OutLink = _arCampaignLinkInfo(_linkIndex, 1)
                            Exit Sub
                        End If
                    End If
                Next
            Next
            _cs.LinkId = CType(_arCampaignLinkInfo(_linkIndex, 0), Integer)
            _cs.OutLink = _arCampaignLinkInfo(_linkIndex, 1)
        End Sub

#End Region

#Region "Fill Arrays"

        Private Sub FillArrays()
            _arraysFilled = True
            FillCampaignInfoArray()
            FillProcessInfoArray()
            GetLinkIdAndOutLinkForCampaignOnlyClicks()
        End Sub

        Private Sub FillCampaignInfoArray()

            Dim randObj As New Random
            Dim arclink As ArrayList = New ArrayList
            Using cnn As New SqlConnection(_strConn)
                cnn.Open()
                Using cmdFillCampaignInfoArray As New SqlCommand("Campaign_GetForProcessingByCampaignId", cnn)
                    cmdFillCampaignInfoArray.CommandType = CommandType.StoredProcedure
                    cmdFillCampaignInfoArray.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = _cs.CampaignId
                    Using dtrFillCampaignInfoArray As SqlDataReader = cmdFillCampaignInfoArray.ExecuteReader
                        If dtrFillCampaignInfoArray.HasRows Then
                            While dtrFillCampaignInfoArray.Read
                                Dim item As String
                                item = CType((dtrFillCampaignInfoArray("LinkId") & "|" & dtrFillCampaignInfoArray("Link") & "|" & dtrFillCampaignInfoArray("LinkPercent")), String)
                                arclink.Add(item)
                            End While
                        Else
                            _cs.OutLink = _webPath & "InactiveCampaign.aspx"
                            _cs.Message = "Error"
                        End If
                    End Using
                End Using
            End Using

            _arCampaignLinkInfo = New String(arclink.Count, 2) {}
            'Set Random Link Index
            _linkIndex = randObj.Next(0, arclink.Count)

            For x As Integer = 0 To arclink.Count - 1
                Dim arlinks() As String
                arlinks = CType(arclink(x).Split("|"), String())
                _arCampaignLinkInfo(x, 0) = arlinks(0)
                _arCampaignLinkInfo(x, 1) = arlinks(1)
                _arCampaignLinkInfo(x, 2) = arlinks(2)
                Array.Clear(arlinks, 0, arlinks.Length)
            Next
            arclink.Clear()
        End Sub

        Private Sub FillProcessInfoArray()
            Dim arlink As New ArrayList
            Using cnn As New SqlConnection(_strConn)
                cnn.Open()
                Using cmdFillProcessInfoArray As New SqlCommand("Track_Click_ProcessingCounts", cnn)
                    cmdFillProcessInfoArray.CommandType = CommandType.StoredProcedure
                    cmdFillProcessInfoArray.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = _cs.CampaignId
                    Using dtrFillProcessInfoArray As SqlDataReader = cmdFillProcessInfoArray.ExecuteReader
                        While dtrFillProcessInfoArray.Read
                            Dim item As String
                            If dtrFillProcessInfoArray.HasRows Then
                                _totalClicks += CType(dtrFillProcessInfoArray("TotalClicks"), Integer)
                                item = CType((dtrFillProcessInfoArray("linkid") & "|" & dtrFillProcessInfoArray("TotalClicks")), String)
                            Else
                                item = "0|0"
                            End If

                            arlink.Add(item)
                        End While
                    End Using
                End Using
            End Using
            _arProcessedClickInfo = New String(arlink.Count, 2) {}
            For x As Integer = 0 To arlink.Count - 1
                Dim arlinks() As String
                arlinks = CType(arlink(x).Split("|"), String())
                _arProcessedClickInfo(x, 0) = arlinks(0)
                _arProcessedClickInfo(x, 1) = arlinks(1)
                Dim percent As Double = (arlinks(1) / _totalClicks) * 100
                percent = percent \ 1
                _arProcessedClickInfo(x, 2) = CType(percent, String)
                Array.Clear(arlinks, 0, arlinks.Length)
            Next
            arlink.Clear()
        End Sub

#End Region

#Region "Process Click"

        Private Sub InsertClick(ByVal linkId As Integer)
            Dim intMonth As Integer
            Dim spToUse As String
            intMonth = DatePart(DateInterval.Month, Date.Now)
            spToUse = "Campaign_ProcessClick_" & intMonth

            Dim usertrack As String = _cs.ServerParams
            usertrack = Utilities.ServerEncrypt(usertrack)

            Using cnn As New SqlConnection(_strConn)
                cnn.Open()
                Using cmdProcessClick As New SqlCommand(spToUse, cnn)
                    cmdProcessClick.CommandType = CommandType.StoredProcedure
                    cmdProcessClick.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = _cs.CampaignId
                    cmdProcessClick.Parameters.Add(New SqlParameter("@LinkId", SqlDbType.Int)).Value = linkId
                    cmdProcessClick.Parameters.Add(New SqlParameter("@Ip", SqlDbType.NVarChar, 15)).Value = _ip
                    If Trim(CType(Len(_referrer), String)) > 0 Or Trim(_referrer) <> String.Empty Then
                        cmdProcessClick.Parameters.Add(New SqlParameter("@Referrer", SqlDbType.NVarChar, 100)).Value = _referrer
                    Else
                        cmdProcessClick.Parameters.Add(New SqlParameter("@Referrer", SqlDbType.NVarChar, 100)).Value = DBNull.Value
                    End If
                    cmdProcessClick.Parameters.Add(New SqlParameter("@CookiePresent", SqlDbType.Bit, 100)).Value = _cs.CookieThere
                    cmdProcessClick.Parameters.Add(New SqlParameter("@UserTrack", SqlDbType.VarChar, 250)).Value = usertrack
                    cmdProcessClick.ExecuteNonQuery()
                End Using
            End Using
            If _arraysFilled = True Then
                Array.Clear(_arCampaignLinkInfo, 0, _arCampaignLinkInfo.Length)
                Array.Clear(_arProcessedClickInfo, 0, _arProcessedClickInfo.Length)
            End If

        End Sub

#End Region

#Region "Add Opens"

        Private Function CheckIfInCampaignsWithOpensTable() As Boolean
            Dim rtn As Boolean = False
            Using cnn As New SqlConnection(_strConn)
                cnn.Open()
                Using cmdCheckIfInCampaignsWithOpensTable As New SqlCommand("CampaignsWithOpens_GetByCampaignId", cnn)
                    cmdCheckIfInCampaignsWithOpensTable.CommandType = CommandType.StoredProcedure
                    cmdCheckIfInCampaignsWithOpensTable.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = _cs.CampaignId
                    Using dtrCheckIfInCampaignsWithOpensTable As SqlDataReader = cmdCheckIfInCampaignsWithOpensTable.ExecuteReader
                        If dtrCheckIfInCampaignsWithOpensTable.HasRows Then
                            rtn = True
                            While dtrCheckIfInCampaignsWithOpensTable.Read
                                _opensDatabase = CType(dtrCheckIfInCampaignsWithOpensTable("DataBase"), String)
                            End While
                        End If
                    End Using
                End Using
            End Using
            Return rtn
        End Function

        Private Sub GetImpressionNumber()
            'Create a new Random class in VB.NET
            Dim randomClass As New Random()
            Dim randomNumber As Integer
            Dim openRate As Double
            _clientsCampaignId = GetClientCampaignId()
            openRate = GetOpenRate(_clientsCampaignId)
            Select Case openRate
                Case Is > 7
                    randomNumber = randomClass.Next(1, 3)
                Case 6 To 7
                    randomNumber = randomClass.Next(3, 8)
                Case Is < 6
                    randomNumber = randomClass.Next(3, 12)
            End Select
            _opensToAdd = randomNumber
            OpenRoutine()
        End Sub

        Private Function GetOpenRate(ByVal campaignId As Integer) As Double
            Dim rtn As Double = 0
            Using cnn As New SqlConnection(_strConn)
                cnn.Open()
                Using cmdGetOpenRate As New SqlCommand("sp_ClientsOpenRates", cnn)
                    cmdGetOpenRate.CommandType = CommandType.StoredProcedure
                    cmdGetOpenRate.Parameters.Add(New SqlParameter("@Domain", SqlDbType.VarChar, 25)).Value = _opensDatabase
                    cmdGetOpenRate.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = campaignId
                    Using dtrGetOpenRate As SqlDataReader = cmdGetOpenRate.ExecuteReader
                        While dtrGetOpenRate.Read
                            rtn = CType(dtrGetOpenRate("OpenRate"), Double)
                        End While
                    End Using
                End Using
            End Using
            Return rtn
        End Function

        Private Function GetClientCampaignId() As Integer
            Dim rtn As Integer
            Using cnn As New SqlConnection(_strConn)
                cnn.Open()
                Using cmdGetClientCampaignId As New SqlCommand("Campaign_GetClientCampaignId_ByCampaignId", cnn)
                    cmdGetClientCampaignId.CommandType = CommandType.StoredProcedure
                    cmdGetClientCampaignId.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = _cs.CampaignId
                    Using dtrGetClientCampaignId As SqlDataReader = cmdGetClientCampaignId.ExecuteReader
                        While dtrGetClientCampaignId.Read
                            rtn = CType(dtrGetClientCampaignId("ClientCampaignId"), Integer)
                        End While
                    End Using
                End Using
                Return rtn
            End Using
        End Function

        Private Sub OpenRoutine()
            Dim currentOpens As Integer
            _existingOpens = GetCounts(False)
            If _existingOpens = 0 Then
                _countInCurrentMonth = False
                _existingOpens = GetCounts(True)
            End If
            'cS.Message = "Database to use " & OpensDatabase & "  Existing Opens " & ExistingOpens & " Count in Current Month " & CountInCurrentMonth & " Opens to add " & OpensToAdd & " Campaign Type " & cS.CampaignType
            If _existingOpens = 0 Then
                Exit Sub
            End If

            If _countInCurrentMonth = True Then
                _finalOpens = _existingOpens + _opensToAdd
            Else
                _finalOpens = _opensToAdd
            End If
            InsertCount(_opensToAdd)

            currentOpens = GetCounts()
            If currentOpens < _finalOpens Then
                While currentOpens < _finalOpens
                    Dim count As Integer = _finalOpens - currentOpens
                    InsertCount(count)
                    currentOpens = GetCounts(False)
                End While
            End If
        End Sub

        Private Sub InsertCount(ByVal count As Integer) ' 0=Opens 1 = Clicks
            Dim clause As String
            Dim intMonth As Integer
            Dim insertTable As String
            intMonth = DatePart(DateInterval.Month, Date.Now)
            insertTable = "Impression_" & intMonth
            clause = "ImpressionCampaignId"

            Using cnn As New SqlConnection(_strConn)
                cnn.Open()
                Using cmdInsertClicks As New SqlCommand("sp_DomainReportAdjustment_AdjustCounts", cnn)
                    cmdInsertClicks.CommandType = CommandType.StoredProcedure
                    cmdInsertClicks.Parameters.Add(New SqlParameter("@Domain", SqlDbType.VarChar)).Value = _opensDatabase
                    cmdInsertClicks.Parameters.Add(New SqlParameter("@InsertTable", SqlDbType.VarChar)).Value = insertTable
                    cmdInsertClicks.Parameters.Add(New SqlParameter("@Count", SqlDbType.Int)).Value = Count
                    cmdInsertClicks.Parameters.Add(New SqlParameter("@PullTable", SqlDbType.VarChar)).Value = _tableName
                    cmdInsertClicks.Parameters.Add(New SqlParameter("@Clause", SqlDbType.VarChar)).Value = clause
                    cmdInsertClicks.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = _clientsCampaignId
                    cmdInsertClicks.ExecuteNonQuery()
                End Using
            End Using

        End Sub

        Private Function GetCounts(Optional ByVal reset As Boolean = False) As Integer '- 0 = Opens 1 = clicks       

            Dim rtn As Integer
            Dim clause As String
            If reset = False Then
                _tableName = GetTableName()
            Else
                _tableName = GetTableName(True)
            End If
            clause = "ImpressionCampaignId"
            Using cnn As New SqlConnection(_strConn)
                cnn.Open()
                Using cmdGetCounts As New SqlCommand("sp_DomainReportAdjustment_GetDomainCounts", cnn)
                    cmdGetCounts.CommandType = CommandType.StoredProcedure
                    cmdGetCounts.Parameters.Add(New SqlParameter("@Domain", SqlDbType.VarChar)).Value = _opensDatabase
                    cmdGetCounts.Parameters.Add(New SqlParameter("@Table", SqlDbType.VarChar)).Value = _tableName
                    cmdGetCounts.Parameters.Add(New SqlParameter("@Clause", SqlDbType.VarChar)).Value = clause
                    cmdGetCounts.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = _clientsCampaignId
                    Using dtrGetCounts As SqlDataReader = cmdGetCounts.ExecuteReader
                        While dtrGetCounts.Read
                            rtn = CType(dtrGetCounts("Count"), Integer)
                        End While
                    End Using
                End Using
            End Using
            Return rtn
        End Function

        Private Function GetTableName(Optional ByVal reset As Boolean = False) As String ' 0 = OPens 1 = Clicks
            Dim intMonth As Integer
            Dim tableName As String
            tableName = "Impression_"

            If reset = False Then
                intMonth = DatePart(DateInterval.Month, Date.Now)
            Else
                Dim dt As Date = DateAdd(DateInterval.Month, -1, Today())
                intMonth = DatePart(DateInterval.Month, dt)
            End If

            tableName = tableName & intMonth

            Return tableName

        End Function

#End Region

#Region "Cookie Checks"

        'Private Sub CheckCookie(ByVal Id As Integer)

        '    Dim aCookie As HttpCookie
        '    For i = 0 To Request.Cookies.Count - 1
        '        aCookie = Request.Cookies(i)
        '        output &= "Cookie name = " & Server.HtmlEncode(aCookie.Name) & "<br>"
        '        output &= "Cookie value = " & Server.HtmlEncode(aCookie.Value) & _
        '        "<br><br>"
        '    Next



        '    Dim CookieName As String = "lga4040Cookie_" & Id
        '    aCookie = HttpRequest.Cookies(CookieName)


        '    If Not aCookie = HttpRequest.Cookies(CookieName) Is Nothing Then
        '        CookieThere = True
        '    End If



        'End Sub

        Public Sub SetCookies()
            'Dim ctxt As HttpContext
            'dim objCookie As HttpCookie
            Dim cookieName As String = "rdn2020Cookie_" & _cs.CampaignId
            Dim cookie As New HttpCookie(cookieName, CType(_cs.CampaignId, String))
            cookie.Expires = DateTime.Now.AddDays(1)
            HttpContext.Current.Response.Cookies.Add(cookie)
        End Sub



#End Region


#Region "Coupon Functions"

        Private Sub GetCouponId()
            Dim tablename As String = "Coupon_" & _cs.CampaignId

            Using cnn As New SqlConnection(_strConn)
                cnn.Open()
                Using cmdGetCouponId As New SqlCommand("Coupon_GetCouponCode", cnn)
                    cmdGetCouponId.CommandType = CommandType.StoredProcedure
                    cmdGetCouponId.Parameters.Add(New SqlParameter("@Table_Name", SqlDbType.NVarChar)).Value = tablename
                    Using dtrGetCouponId As SqlDataReader = cmdGetCouponId.ExecuteReader
                        While dtrGetCouponId.Read
                            _couponCode = CType(dtrGetCouponId("CouponCode"), String)
                            _couponGuid = dtrGetCouponId("CouponGuid").ToString
                        End While
                    End Using
                End Using
            End Using
            UpdateCoupon()

        End Sub

        Private Sub UpdateCoupon()
            Dim tablename As String = "Coupon_" & _cs.CampaignId

            Using cnn As New SqlConnection(_strConn)
                cnn.Open()
                Using cmdUpdateCoupon As New SqlCommand("Coupon_MarkAsUsed", cnn)
                    cmdUpdateCoupon.CommandType = CommandType.StoredProcedure
                    cmdUpdateCoupon.Parameters.Add(New SqlParameter("@Table_Name", SqlDbType.NVarChar)).Value = tablename
                    cmdUpdateCoupon.Parameters.Add(New SqlParameter("@CouponGuid", SqlDbType.UniqueIdentifier)).Value = SqlTypes.SqlGuid.Parse(_couponGuid)
                    cmdUpdateCoupon.ExecuteNonQuery()
                End Using
            End Using
        End Sub

#End Region

    End Class
End Namespace