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

Partial Class Home
    Inherits Page

    Private WebPath As String = ConfigurationManager.AppSettings("WebPath")
    Private strConn As String = ConfigurationManager.AppSettings("cnn")
    Private Ip As String = Context.Request.ServerVariables.Item("REMOTE_ADDR")
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load        
        If Len(Trim(Request.QueryString.ToString)) > 0 Then
            fullOrigionalpath = Request.QueryString("c")
            'Declare the array and fill with string elements       
            Dim arrfullOrigionalpath() As String = New String() {}
            arrfullOrigionalpath = fullOrigionalpath.Split("~")
            CampaignId = CInt(arrfullOrigionalpath(0))
            If arrfullOrigionalpath.Length = 2 Then
                Link = WebPath & "Process.aspx?c=" & fullOrigionalpath
                Response.Redirect(Link)
            End If
            FillArrays()
        Else
            Link = WebPath & "InactiveCampaign.aspx"
            Response.Redirect(Link)
        End If
    End Sub

    Private Sub FillArrays()
        FillCampaignInfoArray()
        FillProcessInfoArray()
        ProcessClick()
    End Sub

    Private Sub ProcessClick()
        Dim foundlink As Boolean = False
        Dim foundlinkindex As Integer

        For a As Integer = 0 To arProcessedClickInfo.GetUpperBound(0)
            If arProcessedClickInfo(a, 0) = arCampaignLinkInfo(LinkIndex, 0) Then
                foundlink = True
                foundlinkindex = a
                Dim ProcessPercent As Integer = arProcessedClickInfo(a, 2)
                Dim LinkPercent As Integer = arCampaignLinkInfo(LinkIndex, 2)
                Dim ClickCount As Integer = arCampaignLinkInfo(LinkIndex, 3)               
                Dim TotalClicks As Integer = arProcessedClickInfo(a, 1)
                If ProcessPercent < LinkPercent Or ClickCount > 0 Then
                    Dim lid As Integer = arCampaignLinkInfo(LinkIndex, 0)
                    InsertClick(lid, arCampaignLinkInfo(LinkIndex, 1))
                    Exit Sub                   
                End If                
                Exit For
            End If
        Next

        If foundlink = False Then
            Dim lid As Integer = arCampaignLinkInfo(LinkIndex, 0)
            InsertClick(lid, arCampaignLinkInfo(LinkIndex, 1))
            Exit Sub
        End If

        'Check if all links have been processed at least once
        For b As Integer = 0 To arCampaignLinkInfo.GetUpperBound(0)
            Dim linkProcessed As Boolean = False
            For c As Integer = 0 To arProcessedClickInfo.GetUpperBound(0)
                If arCampaignLinkInfo(b, 0) = arProcessedClickInfo(c, 0) Then
                    linkProcessed = True
                End If
            Next
            If b < arCampaignLinkInfo.GetUpperBound(0) Then
                If linkProcessed = False Then
                    Dim lid As Integer = arCampaignLinkInfo(b, 0)
                    InsertClick(lid, arCampaignLinkInfo(b, 1))
                    Exit Sub
                End If
            End If
        Next

        For d As Integer = 0 To arCampaignLinkInfo.GetUpperBound(0)
            For e As Integer = 0 To arProcessedClickInfo.GetUpperBound(0)
                If arCampaignLinkInfo(d, 0) = arProcessedClickInfo(e, 0) Then
                    Dim LinkPercent As Integer = arCampaignLinkInfo(d, 2)
                    Dim ProcessPercent As Integer = arProcessedClickInfo(e, 2)
                    If ProcessPercent < LinkPercent Then
                        Dim lid As Integer = arCampaignLinkInfo(d, 0)
                        InsertClick(lid, arCampaignLinkInfo(d, 1))
                        Exit Sub
                    End If
                End If
            Next
        Next
        InsertClick(arCampaignLinkInfo(LinkIndex, 0), arCampaignLinkInfo(LinkIndex, 1))
    End Sub

#Region "Fill Arrays"

    Private Sub FillCampaignInfoArray()

        Dim randObj As New Random
        Dim arclink As ArrayList = New ArrayList
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdFillCampaignInfoArray As New SqlCommand("Campaign_GetForProcessingByCampaignId", cnn)
                cmdFillCampaignInfoArray.CommandType = CommandType.StoredProcedure
                cmdFillCampaignInfoArray.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = CampaignId
                Using dtrFillCampaignInfoArray As SqlDataReader = cmdFillCampaignInfoArray.ExecuteReader
                    If dtrFillCampaignInfoArray.HasRows Then
                        While dtrFillCampaignInfoArray.Read
                            Dim item As String
                            item = dtrFillCampaignInfoArray("LinkId") & "|" & dtrFillCampaignInfoArray("Link") & "|" & dtrFillCampaignInfoArray("LinkPercent") & "|" & dtrFillCampaignInfoArray("ClickCount")
                            arclink.Add(item)
                        End While
                    Else
                        Link = WebPath & "InactiveCampaign.aspx"
                        Response.Redirect(Link)
                    End If
                End Using
            End Using
        End Using

        arCampaignLinkInfo = New String(arclink.Count - 1, 3) {}
        'Set Random Link Index
        LinkIndex = randObj.Next(0, arclink.Count)

        For x As Integer = 0 To arclink.Count - 1
            Dim arlinks() As String = New String() {}
            arlinks = arclink(x).Split("|")           
            arCampaignLinkInfo(x, 0) = arlinks(0)
            arCampaignLinkInfo(x, 1) = arlinks(1)
            arCampaignLinkInfo(x, 2) = arlinks(2)
            arCampaignLinkInfo(x, 3) = arlinks(3)
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
                cmdtest.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = CampaignId
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
        arProcessedClickInfo = New String(arlink.Count - 1, 2) {}
        For x As Integer = 0 To arlink.Count - 1
            Dim arlinks() As String = New String() {}
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

    Private Function GetLinksToalClicks(link As Integer) As Integer
        Dim rtn As Integer
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetClientCampaignId As New SqlCommand("Campaign_GetCampaignLinkTotalClicks", cnn)
                cmdGetClientCampaignId.CommandType = CommandType.StoredProcedure
                cmdGetClientCampaignId.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = CampaignId
                cmdGetClientCampaignId.Parameters.Add(New SqlParameter("@LinkId", Data.SqlDbType.Int)).Value = link
                Using dtrGetClientCampaignId As SqlDataReader = cmdGetClientCampaignId.ExecuteReader
                    While dtrGetClientCampaignId.Read
                        rtn = dtrGetClientCampaignId("TotalClicks")
                    End While
                End Using
            End Using
            Return rtn
        End Using

    End Function

#End Region

#Region "Insert"
    Private Sub InsertClick(ByVal linkId As Integer, ByVal RURLlink As String)
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
                cmdProcessClick.CommandType = Data.CommandType.StoredProcedure
                cmdProcessClick.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = CampaignId
                cmdProcessClick.Parameters.Add(New SqlParameter("@LinkId", Data.SqlDbType.Int)).Value = linkId
                cmdProcessClick.Parameters.Add(New SqlParameter("@Ip", Data.SqlDbType.NVarChar, 15)).Value = Ip
                If Trim(Len(Referrer)) > 0 Or Trim(Referrer) <> String.Empty Then
                    cmdProcessClick.Parameters.Add(New SqlParameter("@Referrer", Data.SqlDbType.NVarChar, 100)).Value = Referrer
                Else
                    cmdProcessClick.Parameters.Add(New SqlParameter("@Referrer", Data.SqlDbType.NVarChar, 100)).Value = DBNull.Value
                End If
                cmdProcessClick.ExecuteNonQuery()
                If cnn.State = Data.ConnectionState.Open Then
                    cnn.Close()
                End If
                If cnn.State = ConnectionState.Open Then
                    cnn.Close()
                End If
            End Using
        End Using

        Array.Clear(arCampaignLinkInfo, 0, arCampaignLinkInfo.Length)
        Array.Clear(arProcessedClickInfo, 0, arProcessedClickInfo.Length)
        Link = WebPath & "PPage.aspx?c=" & RURLlink       
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
                cmdGetClientCampaignId.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = CampaignId
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
                cmdCheckIfInCampaignsWithOpensTable.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = CampaignId
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
        Dim RandomClass As New Random()
        Dim RandomNumber As Integer
        Dim OpenRate As Double
        ClientsCampaignId = GetClientCampaignId()
        OpenRate = GetOpenRate(ClientsCampaignId)

        Select Case OpenRate
            Case Is > 15
                RandomNumber = RandomClass.Next(2, 5)
            Case 6 To 15
                RandomNumber = RandomClass.Next(14, 35)
            Case Is < 6
                RandomNumber = RandomClass.Next(20, 41)
        End Select

        Dim Count As Integer = 0

        While Count < RandomNumber
            GetIPs()
            InsertOpens(OpensIP)
            Count += 1
        End While
        Response.Write("<br><hr>")

    End Sub

    Private Function GetOpenRate(ByVal campaignId As Integer) As Double
        Dim rtn As Double = 0
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetOpenRate As New SqlCommand("sp_ClientsOpenRates", cnn)
                cmdGetOpenRate.CommandType = CommandType.StoredProcedure
                cmdGetOpenRate.Parameters.Add(New SqlParameter("@Domain", Data.SqlDbType.VarChar, 25)).Value = OpensDatabase
                cmdGetOpenRate.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = campaignId
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
        Dim IPTable = "Impression_" & mdt
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetIPs As New SqlCommand("CampaignsWithOpens_GetIPsForOPens", cnn)
                cmdGetIPs.CommandType = CommandType.StoredProcedure
                cmdGetIPs.Parameters.Add(New SqlParameter("@Table", Data.SqlDbType.VarChar, 25)).Value = IPTable
                Using dtrGetIps As SqlDataReader = cmdGetIPs.ExecuteReader
                    While dtrGetIps.Read
                        If Not IsDBNull(dtrGetIps("IP")) Or Trim(Len(dtrGetIps("IP"))) > 0 Then
                            OpensIP = dtrGetIps("IP")
                        Else
                            OpensIP = "0.0.0.0"
                        End If
                    End While
                End Using
            End Using
        End Using
    End Sub

    Private Sub InsertOpens(ByVal ip As String)
        Dim dt As Date = Now()
        Dim mdt As Integer = dt.Month
        Dim OpensTable = "Impression_" & mdt
        Dim IP1 As String = String.Empty
        Dim IP2 As String = String.Empty
        Dim IP3 As String = String.Empty
        Dim IP4 As String = String.Empty
        Dim arrIp() As String = New String() {}
        arrIp = ip.Split(".")
        For x As Integer = 0 To arrIp.GetUpperBound(0)
            Select Case x
                Case 0
                    IP1 = arrIp(x)
                Case 1
                    IP2 = arrIp(x)
                Case 2
                    IP3 = arrIp(x)
                Case 3
                    IP4 = arrIp(x)
            End Select
        Next
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdInsertImpressions As New SqlCommand("sp_AutomaticOpenGen_InsertOpens", cnn)
                cmdInsertImpressions.CommandType = CommandType.StoredProcedure
                cmdInsertImpressions.Parameters.Add(New SqlParameter("@Domain", Data.SqlDbType.VarChar, 50)).Value = OpensDatabase
                cmdInsertImpressions.Parameters.Add(New SqlParameter("@Table", Data.SqlDbType.VarChar, 50)).Value = OpensTable
                cmdInsertImpressions.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = ClientsCampaignId
                cmdInsertImpressions.Parameters.Add(New SqlParameter("@IP1", Data.SqlDbType.Char, 3)).Value = IP1
                cmdInsertImpressions.Parameters.Add(New SqlParameter("@IP2", Data.SqlDbType.Char, 3)).Value = IP2
                cmdInsertImpressions.Parameters.Add(New SqlParameter("@IP3", Data.SqlDbType.Char, 3)).Value = IP3
                cmdInsertImpressions.Parameters.Add(New SqlParameter("@IP4", Data.SqlDbType.Char, 3)).Value = IP4
                cmdInsertImpressions.ExecuteNonQuery()
            End Using
        End Using

    End Sub


#End Region

End Class 