Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Text.RegularExpressions

Namespace lga4040
    Public Class Campaign
        Private ReadOnly _strConn As String = ConfigurationSettings.AppSettings("cnn")
        Private Const SId As String = "SIDTest"
        Private _dm As DataManager
        Private _ds As DataSet
        Private _cs As StructCampaign = New StructCampaign
        Private _creative As String
        Private _linkId As Integer = 0
        Private Const NewLink As String = "href={Replace}"
        Private _convertLink As String
        Private _impressionPath As String
        Private _couponPath As String
        Private _clientsConvertLink As String
        Private _clientsImpressionPath As String
        Private _clientsCouponPath As String
        Private _urlMask As String
        Private _openLink As String
        Private _clientsOpenLink As String
        Private _rCreative As String
        Private _replaceString As String
        Private _bodyStartPosition As Integer
        Private _bodyEndPosition As Integer
        Private _domain As String
        Private Const DefaultDomain As String = "http://www.rdn2020.com"
        Private _clickPage As String
        Private Const DefaultClickPage As String = "P.aspx."
        Private _openPage As String
        Private Const DefaultOpenPage As String = "ImageTrack.aspx"
        Private _couponPage As String
        Private Const DefaultCouponPage As String = "CP.aspx"
        Private Const ClientsImpressionOnly As Boolean = False
        Private _myGuid As Guid = Guid.NewGuid()

        Private Enum CampaingnUIdType
            CampaignUId = 0
            IOUId = 1
        End Enum

#Region "Structures"

        Public Structure StructCampaign  'CampaignType = 0 Regular  1 Coupon Campaign   
            Public CampaignType As Integer
            Public ClientUId As Guid
            Public ClientsClientUId As Guid
            Public CampaignId As Integer
            Public ClientsCampaignId As Integer
            Public CampaignUId As Guid
            Public ClientsCampaignUId As Guid
            Public CampaignName As String
            Public IOUId As Guid
            Public ClientsIOUId As Guid
            Public CampaignCreative As String
            Public ClientsCampaignCreative As String
            Public ConvertedCreative As String
            Public AdId As Integer
            Public CreatedOn As Date
            Public EmailsOrdered As Integer
            Public SubjectLine As String
            Public Status As Integer
            Public ClientsStatus As Integer
            Public CouponVariable As String
            Public IsActive As Integer
            Public OpenLinkId As Integer
            Public Clicks As Integer
            Public Impressions As Integer
            Public OpenLink1 As String
            Public OpenLink2 As String
            Public OpenLink3 As String
            Public InternalOpenLink As Integer
            Public FullFillBy As Date
            Public UniqueOnly As Integer
            Public MaxClickPercent As Double
            Public ClientsDataBase As String
            Public ClientsFriendlyFrom As String
            Public ClientsNotes As String
            Public ClientsBroadcastDate As Date
            Public Message As String
        End Structure

#End Region

#Region "Insert Functions"

        Public Function InsertCampaign(ByVal cstrut As StructCampaign) As StructCampaign
            _cs = cstrut
            _creative = _cs.CampaignCreative
            StandardizeLinks()
            If _bodyStartPosition = 0 Then
                _cs.Message = "ERROR: THERE IS NO BODY TAG!! Creative must contain Body tags."
                Return _cs
            End If

            GetCampaignAssignedInformation()
            GetCompanyInfo()
            GetDomain()
            ConvertCreative()
            Insertcreative()

            _cs.ConvertedCreative = _creative
            Return _cs

        End Function

        Private Sub ConvertCreative()
            _impressionPath = Replace(_impressionPath, "@CampaignId", CType(_cs.CampaignId, String))
            _openLink &= "<img src=""" & _impressionPath & """ alt="""" width=""0"" height=""0""/>"
            Dim startOfLink As Integer = 1
            Dim endoflink As Integer

            Do Until startOfLink = 0
                startOfLink = InStr(_creative, NewLink)
                If startOfLink <> 0 Then
                    _rCreative = Right(_creative, Len(_creative) - startOfLink - 14)
                    endoflink = InStr(_rCreative, """")
                    _replaceString = Left(_rCreative, endoflink - 1)
                    TransformLinks(_replaceString, startOfLink, Len(_replaceString))
                End If
            Loop
            _creative = Replace(_creative, "href={Replaced}", " href=", 1, -1, CompareMethod.Text)
            _creative = _creative & _openLink
            If _cs.CampaignType = 1 Then
                CreateCouponTable()
            End If

        End Sub

        Private Sub Insertcreative()

            Using cnn As New SqlConnection(_strConn)
                cnn.Open()
                Using cmdInsertcreative As New SqlCommand("CampaignAdCopy_InsertNew", cnn)
                    cmdInsertcreative.CommandType = CommandType.StoredProcedure
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = _cs.CampaignId
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@CampaignUId", SqlDbType.UniqueIdentifier)).Value = _myGuid
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@CampaignName", SqlDbType.NVarChar, 100)).Value = _cs.CampaignName
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@IOUId", SqlDbType.UniqueIdentifier)).Value = _cs.IOUId
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@CampaignCreative", SqlDbType.NVarChar)).Value = _cs.CampaignCreative
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@ConvertedCreative", SqlDbType.NVarChar)).Value = Trim(_creative)
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@EmailsOrdered", SqlDbType.NVarChar)).Value = _cs.EmailsOrdered
                    If _cs.CampaignType = 1 Then
                        cmdInsertcreative.Parameters.Add(New SqlParameter("@CouponVariable", SqlDbType.NVarChar)).Value = _cs.CouponVariable
                    Else
                        cmdInsertcreative.Parameters.Add(New SqlParameter("@CouponVariable", SqlDbType.NVarChar)).Value = DBNull.Value
                    End If
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@SubjectLine", SqlDbType.NVarChar)).Value = _cs.SubjectLine
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@Clicks", SqlDbType.Int)).Value = _cs.Clicks
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@Impressions", SqlDbType.Int)).Value = _cs.Impressions
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@OpenLink1", SqlDbType.NVarChar)).Value = _cs.OpenLink1
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@OpenLink2", SqlDbType.NVarChar)).Value = _cs.OpenLink2
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@OpenLink3", SqlDbType.NVarChar)).Value = _cs.OpenLink3
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@InternalOpenLink", SqlDbType.TinyInt)).Value = _cs.InternalOpenLink
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@FullFillBy", SqlDbType.DateTime)).Value = _cs.FullFillBy
                    cmdInsertcreative.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Private Sub GetCampaignAssignedInformation()
            Using cnn As New SqlConnection(_strConn)
                cnn.Open()
                Try
                    Using cmdGetCampaignId As New SqlCommand("Campaign_CampaignId_GetNext", cnn)
                        cmdGetCampaignId.CommandType = CommandType.StoredProcedure
                        _cs.CampaignId = CType(cmdGetCampaignId.ExecuteScalar, Integer)
                    End Using
                Catch ex As Exception
                    _cs.Message = "We are Sorry The application could not get the campaign Id for this campaign, Please contact the support team."
                End Try
            End Using
            Dim myGuid As Guid = Guid.NewGuid()
            _cs.CampaignUId = myGuid
        End Sub

        Private Sub GetDomain()
            Using cnn As New SqlConnection(_strConn)
                cnn.Open()
                Using cmdGetDomain As New SqlCommand("Client_GetByUId", cnn)
                    cmdGetDomain.CommandType = CommandType.StoredProcedure
                    cmdGetDomain.Parameters.Add(New SqlParameter("@ClientUId", SqlDbType.UniqueIdentifier)).Value = _cs.ClientUId 'SqlTypes.SqlGuid.Parse(ddClient.SelectedValue)
                    Using dtrGetDomain As SqlDataReader = cmdGetDomain.ExecuteReader
                        While dtrGetDomain.Read
                            If Not IsDBNull(dtrGetDomain("ClientDomain")) Then
                                _domain = CType(dtrGetDomain("ClientDomain"), String)
                            Else
                                _domain = DefaultDomain
                            End If

                            If Not IsDBNull(dtrGetDomain("ClientClick")) Then
                                _clickPage = CType(dtrGetDomain("ClientClick"), String)
                            Else
                                _clickPage = DefaultClickPage
                            End If

                            If Not IsDBNull(dtrGetDomain("ClientOpen")) Then
                                _openPage = CType(dtrGetDomain("ClientOpen"), String)
                            Else
                                _openPage = DefaultOpenPage
                            End If

                            If Not IsDBNull(dtrGetDomain("ClientCoupon")) Then
                                _couponPage = CType(dtrGetDomain("ClientCoupon"), String)
                            Else
                                _couponPage = DefaultCouponPage
                            End If
                        End While
                    End Using
                End Using
            End Using
        End Sub

        Private Sub GetCompanyInfo()
            Using cnn As New SqlConnection(_strConn)
                cnn.Open()
                Try
                    Using cmdGetCompanyInfo As New SqlCommand("Company_GetInfo", cnn)
                        cmdGetCompanyInfo.CommandType = CommandType.StoredProcedure
                        Using dtrGetCompanyInfo As SqlDataReader = cmdGetCompanyInfo.ExecuteReader
                            While dtrGetCompanyInfo.Read
                                'ConvertLink = dtrGetCompanyInfo("CreativeRedirectPath")
                                _impressionPath = CType(dtrGetCompanyInfo("ImpressionPath"), String)
                                _couponPath = CType(dtrGetCompanyInfo("CouponPath"), String)
                                _urlMask = CType(dtrGetCompanyInfo("URLMask"), String)
                            End While
                        End Using
                    End Using
                Catch ex As Exception
                    _cs.Message = "We are Sorry The application could not get the company info, Please contact the support team."
                End Try
            End Using
        End Sub

        Private Sub StandardizeLinks()
            While _creative.IndexOf("< ", StringComparison.Ordinal) <> -1
                _creative = _creative.Replace("< ", "<")
            End While
            _creative = Regex.Replace(_creative, "<BODY", "<body", RegexOptions.IgnoreCase)
            _creative = Regex.Replace(_creative, "</BODY", "</body", RegexOptions.IgnoreCase)
            _creative = Regex.Replace(_creative, "HREF", "href", RegexOptions.IgnoreCase)
            _creative = Regex.Replace(_creative, "MAILTO", "mailto", RegexOptions.IgnoreCase)
            _creative = Regex.Replace(_creative, "<A", "<a", RegexOptions.IgnoreCase)
            While _creative.IndexOf("href ", StringComparison.Ordinal) <> -1
                _creative = _creative.Replace("href ", "href")
            End While
            _creative = Regex.Replace(_creative, "href= ", "href=", RegexOptions.IgnoreCase)

            '//R. Joe Reich Fix...9/11/2013
            _creative = System.Text.RegularExpressions.Regex.Replace(_creative, "href='", "href=", Text.RegularExpressions.RegexOptions.IgnoreCase)
            _creative = System.Text.RegularExpressions.Regex.Replace(_creative, "' ", """ ", Text.RegularExpressions.RegexOptions.IgnoreCase)

            _creative = Replace(_creative, "href=", NewLink, 1, -1, CompareMethod.Text)
            _bodyStartPosition = InStr(_creative, "<body")
            _bodyEndPosition = InStr(_creative, "</body>")
        End Sub

        Private Sub TransformLinks(ByVal link As String, ByVal startpos As Integer, ByVal length As Integer)
            Dim strCampaignId As String
            Dim strLinkId As String
            strCampaignId = Left(_myGuid.ToString, 8) & _cs.CampaignId
            If startpos > _bodyStartPosition Then
                If InStr(link, "http") > 0 Then
                    If InStr(link.ToLower, "mailto:") = 0 Then
                        If _cs.CampaignType = 0 Then
                            Dim arrMask() As String
                            arrMask = _urlMask.Split(CType("=", Char))
                            Dim ar3() As String
                            ar3 = arrMask(3).Split(CType("&", Char))
                            _urlMask = arrMask(0) & "=" & arrMask(1) & "=" & arrMask(2) & "=" & ar3(0)
                            _convertLink = _domain & "/" & _clickPage & "?" & _urlMask
                        Else
                            _convertLink = _domain & "/" & _clickPage & "?" & _urlMask
                        End If
                        _linkId = _linkId + 1
                        strLinkId = Right(_myGuid.ToString, 8) & _linkId
                        Dim nstr As String = Trim(_convertLink)
                        nstr = Replace(nstr, "@CampaignId", Trim(strCampaignId))
                        nstr = Replace(nstr, "@LinkId", Trim(strLinkId))
                        nstr = Replace(nstr, "@SubId", Trim(SId))
                        nstr = Replace(nstr, "@Coupon", Trim(_cs.CouponVariable))
                        nstr = Trim(nstr)
                        nstr = Replace(nstr, Chr(9), "", 1, -1, CompareMethod.Text)
                        _creative = _creative.Remove(startpos + 14, length)
                        _creative = _creative.Insert(startpos + 14, Trim(nstr))
                        InsertCampaign(_linkId, link)
                    End If
                End If
            End If
            _creative = _creative.Insert(startpos + 12, "d")
            'BodyEndPosition = InStr(Creative, "</body>")
        End Sub

        Private Sub InsertCampaign(ByVal linkId As Integer, ByVal link As String)

            Using cnn As New SqlConnection(_strConn)
                cnn.Open()
                Using cmdInsertCampaign As New SqlCommand("Campaign_CreateNew", cnn)
                    cmdInsertCampaign.CommandType = CommandType.StoredProcedure
                    cmdInsertCampaign.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = _cs.CampaignId
                    cmdInsertCampaign.Parameters.Add(New SqlParameter("@LinkId", SqlDbType.Int)).Value = linkId
                    cmdInsertCampaign.Parameters.Add(New SqlParameter("@Link", SqlDbType.NVarChar, 500)).Value = link
                    cmdInsertCampaign.ExecuteNonQuery()
                    cnn.Close()
                End Using
            End Using
        End Sub

        Private Sub CreateCouponTable()
            Using cnn As New SqlConnection(_strConn)
                cnn.Open()
                Using cmdCreateCouponTable As New SqlCommand("Coupon_TableNumber_Create", cnn)
                    cmdCreateCouponTable.CommandType = CommandType.StoredProcedure
                    cmdCreateCouponTable.Parameters.Add(New SqlParameter("@Table_Number", SqlDbType.NVarChar)).Value = _cs.CampaignId
                    cmdCreateCouponTable.ExecuteNonQuery()
                End Using
            End Using
        End Sub

#End Region

#Region "Dual Insert Functions"

        Public Function InsertDualCampaigns(ByVal struct As StructCampaign) As StructCampaign
            _cs = struct
            _creative = _cs.ClientsCampaignCreative
            StandardizeLinks()
            If _bodyStartPosition = 0 Then
                _cs.Message = "ERROR: THERE IS NO BODY TAG!! Creative must contain Body tags."
                Return _cs
            End If

            GetClientsCampiagnId()
            GetClientsCompanyInfo()
            ConvertClientsCreative()
            SetClientsStatus()
            InsertClientsCreative()
            _cs.CampaignCreative = _creative
            _linkId = 0
            _creative = _cs.CampaignCreative
            StandardizeLinks()
            If _bodyStartPosition = 0 Then
                _cs.Message = "ERROR: THERE IS NO BODY TAG!! Creative must contain Body tags."
                Return _cs
            End If
            GetCampaignAssignedInformation()
            GetCompanyInfo()
            GetDomain()
            ConvertLGDualCreative()
            Insertcreative()
            _cs.CampaignUId = _myGuid

            _cs.ConvertedCreative = _creative

            Return _cs
        End Function

        Private Sub GetClientsCampiagnId()
            Dim c As New Client
            _cs.ClientsDataBase = c.GetClientsDataBase(_cs.ClientUId)
            Using cnn As New SqlConnection(_strConn)
                cnn.Open()
                Using cmdGetClientsCampaignId As New SqlCommand("Campaign_ClientsCampaignId_GetNext", cnn)
                    cmdGetClientsCampaignId.CommandType = CommandType.StoredProcedure
                    cmdGetClientsCampaignId.Parameters.Add(New SqlParameter("@DataBase", SqlDbType.VarChar)).Value = _cs.ClientsDataBase
                    _cs.ClientsCampaignId = CType(cmdGetClientsCampaignId.ExecuteScalar, Integer)
                End Using
            End Using
        End Sub

        Private Sub GetClientsCompanyInfo()
            Using cnn As New SqlConnection(_strConn)
                cnn.Open()
                Using cmdGetClientsCompanyInfo As New SqlCommand("Company_ClientCompanyGetInfo", cnn)
                    cmdGetClientsCompanyInfo.CommandType = CommandType.StoredProcedure
                    cmdGetClientsCompanyInfo.Parameters.Add(New SqlParameter("@DataBase", SqlDbType.VarChar)).Value = _cs.ClientsDataBase
                    Using dtrGetCompanyInfo As SqlDataReader = cmdGetClientsCompanyInfo.ExecuteReader
                        While dtrGetCompanyInfo.Read
                            _clientsConvertLink = CType(dtrGetCompanyInfo("CreativeRedirectPath"), String)
                            _clientsImpressionPath = CType(dtrGetCompanyInfo("ImpressionPath"), String)
                            _clientsCouponPath = CType(dtrGetCompanyInfo("CouponPath"), String)
                        End While
                    End Using
                End Using
            End Using
        End Sub

        Private Sub ConvertClientsCreative()
            _clientsImpressionPath = Replace(_clientsImpressionPath, "@CampaignId", CType(_cs.ClientsCampaignId, String))
            _clientsOpenLink &= "<img src=""" & _clientsImpressionPath & """ alt="""" width=""0"" height=""0""/>"
            Dim startOfLink As Integer = 1
            Dim endoflink As Integer

            Do Until startOfLink = 0
                startOfLink = InStr(_creative, NewLink)
                If startOfLink <> 0 Then
                    _rCreative = Right(_creative, Len(_creative) - startOfLink - 14)
                    endoflink = InStr(_rCreative, """")
                    _replaceString = Left(_rCreative, endoflink - 1)
                    TransformClientsLinks(_replaceString, startOfLink, Len(_replaceString))
                End If
            Loop
            _creative = Replace(_creative, "href={Replaced}", " href=", 1, -1, CompareMethod.Text)
            _creative = _creative & _clientsOpenLink
            'If cS.CampaignType = 1 Then
            '    CreateCouponTable()
            'End If
        End Sub

        Private Sub SetClientsStatus()
            _cs.ClientsStatus = 6
        End Sub

        Private Sub TransformClientsLinks(ByVal link As String, ByVal startpos As Integer, ByVal length As Integer)
            If startpos > _bodyStartPosition Then 'And startpos < BodyEndPosition Then
                If InStr(link, "http") > 0 Then
                    If InStr(link.ToLower, "mailto:") = 0 Then
                        _linkId = _linkId + 1
                        Dim nstr As String = Trim(_clientsConvertLink)
                        nstr = Replace(nstr, "@CampaignId", Trim(CType(_cs.ClientsCampaignId, String)))
                        nstr = Replace(nstr, "@LinkId", Trim(CType(_linkId, String)))
                        nstr = Trim(nstr)
                        nstr = Replace(nstr, Chr(9), "", 1, -1, CompareMethod.Text)
                        _creative = _creative.Remove(startpos + 14, length)
                        _creative = _creative.Insert(startpos + 14, Trim(nstr))
                        InsertClientsCampaign(_linkId, link)
                    End If
                End If
            End If
            _creative = _creative.Insert(startpos + 12, "d")
        End Sub

        Private Sub InsertClientsCampaign(ByVal linkId As Integer, ByVal link As String)
            Using cnn As New SqlConnection(_strConn)
                cnn.Open()
                Using cmdInsertCampaign As New SqlCommand("Campaign_ClientsCampaignCreateNew", cnn)
                    cmdInsertCampaign.CommandType = CommandType.StoredProcedure
                    cmdInsertCampaign.Parameters.Add(New SqlParameter("@DataBase", SqlDbType.VarChar)).Value = _cs.ClientsDataBase
                    cmdInsertCampaign.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = _cs.ClientsCampaignId
                    cmdInsertCampaign.Parameters.Add(New SqlParameter("@LinkId", SqlDbType.Int)).Value = linkId
                    cmdInsertCampaign.Parameters.Add(New SqlParameter("@Link", SqlDbType.NVarChar, 500)).Value = link
                    cmdInsertCampaign.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Private Sub InsertClientsCreative()
            Dim impressionYes As Integer
            If ClientsImpressionOnly = True Then
                impressionYes = 1
            Else
                impressionYes = 0
            End If

            Dim myGuid As Guid = Guid.NewGuid()

            Using cnn As New SqlConnection(_strConn)
                cnn.Open()
                Using cmdInsertcreative As New SqlCommand("CampaignAdCopy_ClientsCampagnInsertNew", cnn)
                    cmdInsertcreative.CommandType = CommandType.StoredProcedure
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@DataBase", SqlDbType.VarChar)).Value = _cs.ClientsDataBase
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = _cs.ClientsCampaignId
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@CampaignUId", SqlDbType.UniqueIdentifier)).Value = myGuid
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@CampaignName", SqlDbType.NVarChar, 100)).Value = _cs.CampaignName
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@IOUId", SqlDbType.UniqueIdentifier)).Value = _cs.ClientsIOUId
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@CampaignCreative", SqlDbType.NVarChar)).Value = _cs.ClientsCampaignCreative
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@ConvertedCreative", SqlDbType.NVarChar)).Value = Trim(_creative)
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@EmailsOrdered", SqlDbType.NVarChar)).Value = Trim(CType(_cs.EmailsOrdered, String))
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@CouponVariable", SqlDbType.NVarChar)).Value = _cs.CouponVariable
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@SubjectLine", SqlDbType.NVarChar)).Value = Trim(_cs.SubjectLine)
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@Status", SqlDbType.TinyInt)).Value = Trim(CType(_cs.ClientsStatus, String))
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@BroadcastDate", SqlDbType.DateTime)).Value = _cs.ClientsBroadcastDate
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@FriendlyFrom", SqlDbType.NVarChar)).Value = _cs.ClientsFriendlyFrom
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@Notes", SqlDbType.NVarChar)).Value = Trim(_cs.ClientsNotes)
                    cmdInsertcreative.Parameters.Add(New SqlParameter("@ImpressionOnly", SqlDbType.NVarChar)).Value = impressionYes
                    cmdInsertcreative.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Private Sub ConvertLGDualCreative()
            _impressionPath = Replace(_impressionPath, "@CampaignId", CType(_cs.CampaignId, String))
            _openLink &= "<img src=""" & _impressionPath & """ alt="""" width=""0"" height=""0""/>"
            Dim startOfLink As Integer = 1
            Dim endoflink As Integer
            Do Until startOfLink = 0
                startOfLink = InStr(_creative, NewLink)
                If startOfLink <> 0 Then
                    _rCreative = Right(_creative, Len(_creative) - startOfLink - 14)
                    endoflink = InStr(_rCreative, """")
                    _replaceString = Left(_rCreative, endoflink - 1)
                    TransformLGDualLinks(_replaceString, startOfLink, Len(_replaceString))
                End If
            Loop
            _creative = Replace(_creative, "href={Replaced}", " href=", 1, -1, CompareMethod.Text)
            _creative = _creative & _openLink
            'If cS.CampaignType = 1 Then
            '    CreateCouponTable()
            'End If
        End Sub

        Private Sub TransformLGDualLinks(ByVal link As String, ByVal startpos As Integer, ByVal length As Integer)
            If startpos > _bodyStartPosition Then 'And startpos < BodyEndPosition Then
                If InStr(link, "http") > 0 Then
                    If InStr(link.ToLower, "mailto:") = 0 Then
                        _convertLink = _domain & "/" & _clickPage & "?c=@CampaignId~@LinkId"
                        _linkId = _linkId + 1
                        Dim nstr As String = Trim(_convertLink)
                        nstr = Replace(nstr, "@CampaignId", Trim(CType(_cs.CampaignId, String)))
                        nstr = Replace(nstr, "@LinkId", Trim(CType(_linkId, String)))
                        nstr = Trim(nstr)
                        nstr = Replace(nstr, Chr(9), "", 1, -1, CompareMethod.Text)
                        _creative = _creative.Remove(startpos + 14, length)
                        _creative = _creative.Insert(startpos + 14, Trim(nstr))
                        InsertCampaign(_linkId, link)
                    End If
                End If
            End If
            _creative = _creative.Insert(startpos + 12, "d")
        End Sub


#End Region

#Region "Edit Functions"
        Public Function EditCampaign(ByVal strut As StructCampaign) As StructCampaign
            _cs = strut
            _creative = _cs.CampaignCreative
            If _cs.CampaignCreative = String.Empty Then
                UpdateCampaign()
                'cS.Message = "Update campaign only"
            Else
                UpdateCampaignAndCreative()
                'cS.Message = "Recreate campaign"
            End If
            Return _cs
        End Function

        Private Sub UpdateCampaignAndCreative()
            StandardizeLinks()
            If _bodyStartPosition = 0 Then
                _cs.Message = "ERROR: THERE IS NO BODY TAG!! Creative must contain Body tags."
                Exit Sub
            End If
            DeleteOldCampaign()
            GetDomain()
            GetCompanyInfo()
            ConvertCreative()

            Using cnn As New SqlConnection(_strConn)
                cnn.Open()
                Using cmdUpdateCampaign As New SqlCommand("CampaignAdCopy_UpdateCampaignAndCreative", cnn)
                    Try
                        cmdUpdateCampaign.CommandType = CommandType.StoredProcedure
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@CampaignName", SqlDbType.NVarChar, 100)).Value = Trim(_cs.CampaignName)
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@CampaignCreative", SqlDbType.NVarChar)).Value = Trim(_cs.CampaignCreative)
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@ConvertedCreative", SqlDbType.NVarChar)).Value = Trim(_creative)
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@EmailsOrdered", SqlDbType.Int)).Value = Trim(CType(_cs.EmailsOrdered, String))
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@IsActive", SqlDbType.TinyInt)).Value = _cs.IsActive
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@Status", SqlDbType.TinyInt)).Value = _cs.Status
                        If _cs.CampaignType = 1 Then
                            cmdUpdateCampaign.Parameters.Add(New SqlParameter("@CouponVariable", SqlDbType.NVarChar, 50)).Value = Trim(_cs.CouponVariable)
                        Else
                            cmdUpdateCampaign.Parameters.Add(New SqlParameter("@CouponVariable", SqlDbType.NVarChar, 50)).Value = DBNull.Value
                        End If

                        If Len(Trim(_cs.SubjectLine)) > 0 Or Trim(_cs.SubjectLine) <> String.Empty Then
                            cmdUpdateCampaign.Parameters.Add(New SqlParameter("@SubjectLine", SqlDbType.NVarChar, 250)).Value = Trim(_cs.SubjectLine)
                        Else
                            cmdUpdateCampaign.Parameters.Add(New SqlParameter("@SubjectLine", SqlDbType.NVarChar, 250)).Value = DBNull.Value
                        End If
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@Clicks", SqlDbType.Int)).Value = _cs.Clicks
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@Impressions", SqlDbType.Int)).Value = _cs.Impressions

                        If Len(Trim(_cs.OpenLink1)) > 0 Or Trim(_cs.OpenLink1) <> String.Empty Then
                            cmdUpdateCampaign.Parameters.Add(New SqlParameter("@OpenLink1", SqlDbType.NVarChar, 250)).Value = Trim(_cs.OpenLink1)
                        Else
                            cmdUpdateCampaign.Parameters.Add(New SqlParameter("@OpenLink1", SqlDbType.NVarChar, 250)).Value = DBNull.Value
                        End If

                        If Len(Trim(_cs.OpenLink2)) > 0 Or Trim(_cs.OpenLink2) <> String.Empty Then
                            cmdUpdateCampaign.Parameters.Add(New SqlParameter("@OpenLink2", SqlDbType.NVarChar, 250)).Value = Trim(_cs.OpenLink2)
                        Else
                            cmdUpdateCampaign.Parameters.Add(New SqlParameter("@OpenLink2", SqlDbType.NVarChar, 250)).Value = DBNull.Value
                        End If

                        If Len(Trim(_cs.OpenLink3)) > 0 Or Trim(_cs.OpenLink3) <> String.Empty Then
                            cmdUpdateCampaign.Parameters.Add(New SqlParameter("@OpenLink3", SqlDbType.NVarChar, 250)).Value = Trim(_cs.OpenLink3)
                        Else
                            cmdUpdateCampaign.Parameters.Add(New SqlParameter("@OpenLink3", SqlDbType.NVarChar, 250)).Value = DBNull.Value
                        End If
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@InternalOpenLink", SqlDbType.TinyInt)).Value = _cs.InternalOpenLink
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@FullFillBy", SqlDbType.DateTime)).Value = Trim(CType(_cs.FullFillBy, String))
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = _cs.CampaignId
                        cmdUpdateCampaign.ExecuteNonQuery()
                    Catch ex As Exception
                        _cs.Message = "There was a problem with the update please try agian in a few min."
                    Finally
                        _cs.Message = "Campaign " & _cs.CampaignId & " has Successfully been updated."
                    End Try
                End Using
            End Using

        End Sub


        Private Sub UpdateCampaign()
            Using cnn As New SqlConnection(_strConn)
                cnn.Open()
                Using cmdUpdateCampaign As New SqlCommand("CampaignAdCopy_UpdateCampaign", cnn)
                    Try
                        cmdUpdateCampaign.CommandType = CommandType.StoredProcedure
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@CampaignName", SqlDbType.NVarChar, 100)).Value = Trim(_cs.CampaignName)
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@EmailsOrdered", SqlDbType.Int)).Value = Trim(CType(_cs.EmailsOrdered, String))
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@IsActive", SqlDbType.TinyInt)).Value = _cs.IsActive
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@Status", SqlDbType.TinyInt)).Value = _cs.Status
                        If _cs.CampaignType = 1 Then
                            cmdUpdateCampaign.Parameters.Add(New SqlParameter("@CouponVariable", SqlDbType.NVarChar, 50)).Value = Trim(_cs.CouponVariable)
                        Else
                            cmdUpdateCampaign.Parameters.Add(New SqlParameter("@CouponVariable", SqlDbType.NVarChar, 50)).Value = DBNull.Value
                        End If

                        If Len(Trim(_cs.SubjectLine)) > 0 Or Trim(_cs.SubjectLine) <> String.Empty Then
                            cmdUpdateCampaign.Parameters.Add(New SqlParameter("@SubjectLine", SqlDbType.NVarChar, 250)).Value = Trim(_cs.SubjectLine)
                        Else
                            cmdUpdateCampaign.Parameters.Add(New SqlParameter("@SubjectLine", SqlDbType.NVarChar, 250)).Value = DBNull.Value
                        End If
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@Clicks", SqlDbType.Int)).Value = _cs.Clicks
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@Impressions", SqlDbType.Int)).Value = _cs.Impressions

                        If Len(Trim(_cs.OpenLink1)) > 0 Or Trim(_cs.OpenLink1) <> String.Empty Then
                            cmdUpdateCampaign.Parameters.Add(New SqlParameter("@OpenLink1", SqlDbType.NVarChar, 250)).Value = Trim(_cs.OpenLink1)
                        Else
                            cmdUpdateCampaign.Parameters.Add(New SqlParameter("@OpenLink1", SqlDbType.NVarChar, 250)).Value = DBNull.Value
                        End If

                        If Len(Trim(_cs.OpenLink2)) > 0 Or Trim(_cs.OpenLink2) <> String.Empty Then
                            cmdUpdateCampaign.Parameters.Add(New SqlParameter("@OpenLink2", SqlDbType.NVarChar, 250)).Value = Trim(_cs.OpenLink2)
                        Else
                            cmdUpdateCampaign.Parameters.Add(New SqlParameter("@OpenLink2", SqlDbType.NVarChar, 250)).Value = DBNull.Value
                        End If

                        If Len(Trim(_cs.OpenLink3)) > 0 Or Trim(_cs.OpenLink3) <> String.Empty Then
                            cmdUpdateCampaign.Parameters.Add(New SqlParameter("@OpenLink3", SqlDbType.NVarChar, 250)).Value = Trim(_cs.OpenLink3)
                        Else
                            cmdUpdateCampaign.Parameters.Add(New SqlParameter("@OpenLink3", SqlDbType.NVarChar, 250)).Value = DBNull.Value
                        End If
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@InternalOpenLink", SqlDbType.TinyInt)).Value = _cs.InternalOpenLink
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@FullFillBy", SqlDbType.DateTime)).Value = Trim(CType(_cs.FullFillBy, String))
                        cmdUpdateCampaign.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = _cs.CampaignId
                        cmdUpdateCampaign.ExecuteNonQuery()
                    Catch ex As Exception
                        _cs.Message = "There was a problem with the update please try agian in a few min."
                    Finally
                        _cs.Message = "Campaign " & _cs.CampaignId & " has Successfully been updated."
                    End Try
                End Using
            End Using

        End Sub

        Private Sub DeleteOldCampaign()
            Using cnn As New SqlConnection(_strConn)
                cnn.Open()
                Using cmdDeleteOldCampaign As New SqlCommand("Campaign_DeleteCampaign", cnn)
                    cmdDeleteOldCampaign.CommandType = CommandType.StoredProcedure
                    cmdDeleteOldCampaign.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = _cs.CampaignId
                    cmdDeleteOldCampaign.ExecuteNonQuery()
                End Using
            End Using
        End Sub


#End Region


        Public Sub DeleteCampaign(ByVal campaignUId As Guid)

        End Sub

        Public Sub UpdateCampaign(ByVal campaign As Campaign)

        End Sub

        Public Sub UpdateCampaignStatus(ByVal campaignId As Int32)
            _dm = New DataManager()
            _dm.UpdateCampaignStatus(campaignId)
        End Sub

        Public Function GetCampaigns() As DataSet
            _dm = New DataManager()
            _ds = New DataSet()

            _ds = _dm.GetCampaigns()

            Return _ds
        End Function

        Public Function GetCampaigns(ByVal status As Int32) As DataSet
            _dm = New DataManager()
            _ds = New DataSet()

            Dim campaigns As New List(Of Campaign)
            _ds = _dm.GetCampaignsByStatus(status)

            Return _ds
        End Function
    End Class
End Namespace