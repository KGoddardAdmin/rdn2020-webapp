Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Web
Imports System.Web.Caching
Imports System.Web.Configuration
Imports System.Web.Profile
Imports System.Web.Security
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports Microsoft.VisualBasic
Imports lga4040
Imports rdn2020.Utilities

Partial Class P1
    Inherits System.Web.UI.Page

    Private WebPath As String = ConfigurationSettings.AppSettings("WebPath")
    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Private Ip As String = Context.Request.ServerVariables.Item("REMOTE_ADDR") 'Request.ServerVariables.Item("REMOTE_ADDRESS")  
    Private Referrer As String = Context.Request.ServerVariables("HTTP_REFERER")
    Private CampaignId As Integer
    Private LinkId As Integer
    Private fullOrigionalpath As String
    Public Link As String
    Private arProcessedClickInfo(,) As String
    Private strProcessedClickInfo As String = String.Empty
    Private arCampaignLinkInfo(,) As String
    Private strCampaignLinkInfo As String = String.Empty
    Private TotalClicks As Integer
    Private LinkIndex As Integer
    Private OpensDatabase As String
    Private OpensIP As String
    Private ClientsCampaignId As Integer
    Private CookieThere As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim userTrack As String
        userTrack = String.Concat("User Agent-" & Request.ServerVariables("http_user_agent") & "\\", "Remote Address ISP - " & Request.ServerVariables("remote_addr") & "\\", "Remote Host - " & Request.ServerVariables("remote_host") & "\\", "Screen Resolution - " & ScreenResolution())

        Response.Write("<br><br><B>USER Uncoded VARIABLE<BR> " & userTrack)
        Response.Write("<br><br><B>USER Encoded  VARIABLE<BR> " & lga4040.Utilities.ServerEncrypt(userTrack))

        'If Len(Trim(Request.QueryString.ToString)) > 0 Then
        '    fullOrigionalpath = Request.QueryString("c")
        '    'Declare the array and fill with string elements       
        '    Dim arrfullOrigionalpath() As String = New String() {}
        '    arrfullOrigionalpath = fullOrigionalpath.Split("~")
        '    CampaignId = CInt(arrfullOrigionalpath(0))
        '    If arrfullOrigionalpath.Length = 2 Then
        '        Link = WebPath & "Process1.aspx?c=" & fullOrigionalpath
        '        Response.Redirect(Link)
        '    End If
        '    Response.Write("Staying in P1")
        '    CheckCookie(CampaignId)
        '    SetCookie(CampaignId)
        '    FillArrays()
        'Else
        '    Link = WebPath & "InactiveCampaign.aspx"
        '    Response.Redirect(Link)
        'End If
    End Sub

    '    Private Sub FillArrays()
    '        FillCampaignInfoArray()
    '        FillProcessInfoArray()
    '        ProcessClick()
    '    End Sub

    '    Private Sub ProcessClick()
    '        Dim foundLink As Boolean = False
    '
    '        For a As Integer = 0 To arProcessedClickInfo.GetUpperBound(0)
    '            If arProcessedClickInfo(a, 0) = arCampaignLinkInfo(LinkIndex, 0) Then
    '                foundLink = True
    '                Dim processPercent As Integer = arProcessedClickInfo(a, 2)
    '                Dim linkPercent As Integer = arCampaignLinkInfo(LinkIndex, 2)
    '                If processPercent < linkPercent Then
    '                    Dim lid As Integer = arCampaignLinkInfo(LinkIndex, 0)
    '                    InsertClick(lid, arCampaignLinkInfo(LinkIndex, 1))
    '                    Exit Sub
    '                End If
    '                Exit For
    '            End If
    '        Next
    '
    '        If foundLink = False Then
    '            Dim lid As Integer = arCampaignLinkInfo(LinkIndex, 0)
    '            InsertClick(lid, arCampaignLinkInfo(LinkIndex, 1))
    '            Exit Sub
    '        End If
    '
    '        'Check if all links have been processed at least once
    '        For b As Integer = 0 To arCampaignLinkInfo.GetUpperBound(0)
    '            Dim linkProcessed As Boolean = False
    '            For c As Integer = 0 To arProcessedClickInfo.GetUpperBound(0)
    '                If arCampaignLinkInfo(b, 0) = arProcessedClickInfo(c, 0) Then
    '                    linkProcessed = True
    '                End If
    '            Next
    '            If b < arCampaignLinkInfo.GetUpperBound(0) Then
    '                If linkProcessed = False Then
    '                    Dim lid As Integer = arCampaignLinkInfo(b, 0)
    '                    InsertClick(lid, arCampaignLinkInfo(b, 1))
    '                    Exit Sub
    '                End If
    '            End If
    '        Next
    '
    '        For d As Integer = 0 To arCampaignLinkInfo.GetUpperBound(0)
    '            For e As Integer = 0 To arProcessedClickInfo.GetUpperBound(0)
    '                If arCampaignLinkInfo(d, 0) = arProcessedClickInfo(e, 0) Then
    '                    Dim linkPercent As Integer = arCampaignLinkInfo(d, 2)
    '                    Dim processPercent As Integer = arProcessedClickInfo(e, 2)
    '                    If processPercent < linkPercent Then
    '                        Dim lid As Integer = arCampaignLinkInfo(d, 0)
    '                        InsertClick(lid, arCampaignLinkInfo(d, 1))
    '                        Exit Sub
    '                    End If
    '                End If
    '            Next
    '        Next
    '        InsertClick(arCampaignLinkInfo(LinkIndex, 0), arCampaignLinkInfo(LinkIndex, 1))
    '    End Sub


#Region "Fill Arrays"

    Private Sub FillCampaignInfoArray()

        Dim randObj As New Random
        Dim arclink As ArrayList = New ArrayList
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdFillCampaignInfoArray As New SqlCommand("Campaign_GetForProcessingByCampaignId", cnn)
                cmdFillCampaignInfoArray.CommandType = CommandType.StoredProcedure
                cmdFillCampaignInfoArray.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = CampaignId
                Using dtrFillCampaignInfoArray As SqlDataReader = cmdFillCampaignInfoArray.ExecuteReader
                    If dtrFillCampaignInfoArray.HasRows Then
                        While dtrFillCampaignInfoArray.Read
                            Dim item As String
                            item = dtrFillCampaignInfoArray("LinkId") & "|" & dtrFillCampaignInfoArray("Link") & "|" & dtrFillCampaignInfoArray("LinkPercent")
                            arclink.Add(item)
                        End While
                    Else
                        Link = WebPath & "InactiveCampaign.aspx"
                        Response.Redirect(Link)
                    End If
                End Using
            End Using
        End Using

        arCampaignLinkInfo = New String(arclink.Count, 2) {}
        'Set Random Link Index
        LinkIndex = randObj.Next(0, arclink.Count)

        For x As Integer = 0 To arclink.Count - 1
            Dim arlinks() As String
            arlinks = arclink(x).Split("|")
            arCampaignLinkInfo(x, 0) = arlinks(0)
            arCampaignLinkInfo(x, 1) = arlinks(1)
            arCampaignLinkInfo(x, 2) = arlinks(2)
            Array.Clear(arlinks, 0, arlinks.Length)
        Next
        arclink.Clear()
    End Sub

    Private Sub FillProcessInfoArray()
        Dim arlink As New ArrayList
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdtest As New SqlCommand("Track_Click_ProcessingCounts", cnn)
                cmdtest.CommandType = CommandType.StoredProcedure
                cmdtest.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = CampaignId
                Using dtrtest As SqlDataReader = cmdtest.ExecuteReader
                    While dtrtest.Read
                        Dim item As String
                        TotalClicks += dtrtest("TotalClicks")
                        item = dtrtest("linkid") & "|" & dtrtest("TotalClicks")
                        arlink.Add(item)
                    End While
                End Using
            End Using
        End Using
        arProcessedClickInfo = New String(arlink.Count, 2) {}
        For x As Integer = 0 To arlink.Count - 1
            Dim arlinks() As String
            arlinks = arlink(x).Split("|")
            arProcessedClickInfo(x, 0) = arlinks(0)
            arProcessedClickInfo(x, 1) = arlinks(1)
            Dim percent As Double = (arlinks(1) / TotalClicks) * 100
            percent = percent \ 1
            arProcessedClickInfo(x, 2) = percent
            Array.Clear(arlinks, 0, arlinks.Length)
        Next
        arlink.Clear()
    End Sub

#End Region

#Region "Insert"
    Private Sub InsertClick(ByVal linkId As Integer, ByVal rUrlLink As String)
        Dim intMonth As Integer
        Dim spToUse As String
        intMonth = DatePart(DateInterval.Month, Date.Now)
        spToUse = "Campaign_ProcessClick_" & intMonth

        If CheckIfInCampaignsWithOpensTable() = True Then
            GetImpressionNumber()
        End If

        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdProcessClick As New SqlCommand(spToUse, cnn)
                cmdProcessClick.CommandType = CommandType.StoredProcedure
                cmdProcessClick.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = CampaignId
                cmdProcessClick.Parameters.Add(New SqlParameter("@LinkId", SqlDbType.Int)).Value = linkId
                cmdProcessClick.Parameters.Add(New SqlParameter("@Ip", SqlDbType.NVarChar, 15)).Value = Ip
                If Trim(Len(Referrer)) > 0 Or Trim(Referrer) <> String.Empty Then
                    cmdProcessClick.Parameters.Add(New SqlParameter("@Referrer", SqlDbType.NVarChar, 100)).Value = Referrer
                Else
                    cmdProcessClick.Parameters.Add(New SqlParameter("@Referrer", SqlDbType.NVarChar, 100)).Value = DBNull.Value
                End If
                cmdProcessClick.Parameters.Add(New SqlParameter("@CookiePresent", SqlDbType.Bit)).Value = CookieThere
                cmdProcessClick.ExecuteNonQuery()
                If cnn.State = ConnectionState.Open Then
                    cnn.Close()
                End If
                If cnn.State = ConnectionState.Open Then
                    cnn.Close()
                End If
            End Using
        End Using

        Array.Clear(arCampaignLinkInfo, 0, arCampaignLinkInfo.Length)
        Array.Clear(arProcessedClickInfo, 0, arProcessedClickInfo.Length)
        Link = WebPath & "PPage1.aspx?c=" & Server.UrlEncode(rUrlLink)
        Response.Redirect(Link)
    End Sub
#End Region

#Region "Opens Check and Insert"
    Private Function GetClientCampaignId() As Integer
        Dim rtn As Integer
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetClientCampaignId As New SqlCommand("Campaign_GetClientCampaignId_ByCampaignId", cnn)
                cmdGetClientCampaignId.CommandType = CommandType.StoredProcedure
                cmdGetClientCampaignId.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = CampaignId
                Using dtrGetClientCampaignId As SqlDataReader = cmdGetClientCampaignId.ExecuteReader
                    While dtrGetClientCampaignId.Read
                        rtn = dtrGetClientCampaignId("ClientCampaignId")
                    End While
                End Using
            End Using
            Return rtn
        End Using

    End Function

    Private Function CheckIfInCampaignsWithOpensTable() As Boolean
        Dim rtn As Boolean
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdCheckIfInCampaignsWithOpensTable As New SqlCommand("CampaignsWithOpens_GetByCampaignId", cnn)
                cmdCheckIfInCampaignsWithOpensTable.CommandType = CommandType.StoredProcedure
                cmdCheckIfInCampaignsWithOpensTable.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = CampaignId
                Using dtrCheckIfInCampaignsWithOpensTable As SqlDataReader = cmdCheckIfInCampaignsWithOpensTable.ExecuteReader
                    If dtrCheckIfInCampaignsWithOpensTable.HasRows Then
                        rtn = True
                        While dtrCheckIfInCampaignsWithOpensTable.Read
                            OpensDatabase = dtrCheckIfInCampaignsWithOpensTable("DataBase")
                        End While
                    End If
                End Using
            End Using
        End Using
        Return rtn
    End Function

    Private Sub GetImpressionNumber()
        'Create a new Random class in VB.NET
        Dim rndClass As New Random()
        Dim rndNumber As Integer
        Dim openRate As Double
        ClientsCampaignId = GetClientCampaignId()
        openRate = GetOpenRate(ClientsCampaignId)

        Select Case openRate
            Case Is > 7
                rndNumber = rndClass.Next(1, 3)
            Case 6 To 7
                rndNumber = rndClass.Next(3, 8)
            Case Is < 6
                rndNumber = rndClass.Next(3, 12)
        End Select

        Dim count As Integer = 0

        While count < rndNumber
            GetIPs()
            InsertOpens(OpensIP)
            count += 1
        End While
        Response.Write("<br><hr>")

    End Sub

    Private Function GetOpenRate(ByVal campaignId As Integer) As Double
        Dim rtn As Double = 0
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetOpenRate As New SqlCommand("sp_ClientsOpenRates", cnn)
                cmdGetOpenRate.CommandType = CommandType.StoredProcedure
                cmdGetOpenRate.Parameters.Add(New SqlParameter("@Domain", SqlDbType.VarChar, 25)).Value = OpensDatabase
                cmdGetOpenRate.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = campaignId
                Using dtrGetOpenRate As SqlDataReader = cmdGetOpenRate.ExecuteReader
                    While dtrGetOpenRate.Read
                        rtn = dtrGetOpenRate("OpenRate")
                    End While
                End Using
            End Using
        End Using
        Return rtn
    End Function

    Private Sub GetIPs()
        Dim dt As Date = Now()
        Dim mdt As Integer = dt.Month
        Dim ipTable = "Impression_" & mdt
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetIPs As New SqlCommand("CampaignsWithOpens_GetIPsForOPens", cnn)
                cmdGetIPs.CommandType = CommandType.StoredProcedure
                cmdGetIPs.Parameters.Add(New SqlParameter("@Table", SqlDbType.VarChar, 25)).Value = ipTable
                Using dtrGetIps As SqlDataReader = cmdGetIPs.ExecuteReader
                    While dtrGetIps.Read
                        OpensIP = dtrGetIps("IP")
                    End While
                End Using
            End Using
        End Using
    End Sub

    Private Sub InsertOpens(ByVal ip As String)
        Dim dt As Date = Now()
        Dim mdt As Integer = dt.Month
        Dim opensTable = "Impression_" & mdt
        Dim ip1 As String = String.Empty
        Dim ip2 As String = String.Empty
        Dim ip3 As String = String.Empty
        Dim ip4 As String = String.Empty
        Dim arrIp() As String
        arrIp = ip.Split(".")
        For x As Integer = 0 To arrIp.GetUpperBound(0)
            Select Case x
                Case 0
                    ip1 = arrIp(x)
                Case 1
                    ip2 = arrIp(x)
                Case 2
                    ip3 = arrIp(x)
                Case 3
                    ip4 = arrIp(x)
            End Select
        Next
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using insert As New SqlCommand("sp_AutomaticOpenGen_InsertOpens", cnn)
                insert.CommandType = CommandType.StoredProcedure
                insert.Parameters.Add(New SqlParameter("@Domain", SqlDbType.VarChar, 50)).Value = OpensDatabase
                insert.Parameters.Add(New SqlParameter("@Table", SqlDbType.VarChar, 50)).Value = opensTable
                insert.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = ClientsCampaignId
                insert.Parameters.Add(New SqlParameter("@IP1", SqlDbType.Char, 3)).Value = ip1
                insert.Parameters.Add(New SqlParameter("@IP2", SqlDbType.Char, 3)).Value = ip2
                insert.Parameters.Add(New SqlParameter("@IP3", SqlDbType.Char, 3)).Value = ip3
                insert.Parameters.Add(New SqlParameter("@IP4", SqlDbType.Char, 3)).Value = ip4
                insert.ExecuteNonQuery()
            End Using
        End Using

    End Sub


#End Region

    '    Private Sub CheckCookie(ByVal Id As Integer)
    '        Dim cookieName As String = "lga4040Cookie_" & Id
    '        If Not Request.Cookies(cookieName) Is Nothing Then
    '            CookieThere = True
    '        End If
    '
    '    End Sub

    '    Private Sub SetCookie(ByVal campaign As String)
    '        Dim cookie As HttpCookie
    '        Dim cookieName As String = "lga4040Cookie_" & campaign
    '        Try
    '            cookie = New HttpCookie(cookieName, campaign)
    '            cookie.Expires = DateTime.Now.AddMinutes(15)
    '            Response.Cookies.Add(cookie)
    '        Catch ex As Exception
    '            Link = WebPath & "InactiveCampaign.aspx"
    '            Response.Redirect(Link)
    '        End Try
    '
    '    End Sub


    Private Function ScreenResolution() As String
        'Dim intX As Integer = Screen.PrimaryScreen.Bounds.Width
        'Dim intY As Integer = Screen.PrimaryScreen.Bounds.Height
        Return "0 X 0"
    End Function


End Class
