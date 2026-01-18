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
Imports System.Net

Partial Class TP
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
    Private IsUnique As Integer
    Public Open1 As String
    Public Open2 As String
    Public Open3 As String
    Protected PostBackStr As String
    Public RedirectScript As String



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            PostBackStr = Page.ClientScript.GetPostBackEventReference(Me, "MyCustomArgument")
        End If

        If Len(Trim(Request.QueryString.ToString)) > 0 Then
            If String.IsNullOrEmpty(Request.QueryString("l")) Then
                fullOrigionalpath = Server.UrlDecode(Request.QueryString("c"))
                GetIds()
                GetThirdPartyLink()
            Else
                CampaignId = Request.QueryString("c")
                LinkId = Request.QueryString("l")
                GetThirdPartyLink()
            End If
            GetImpressionNumber()
        Else
            Link = WebPath & "InactiveCampaign.aspx?url=" + Request.QueryString.ToString
            Response.Redirect(Link)
        End If

        If Page.IsPostBack Then
            Dim eventArg As String = Request("__EVENTARGUMENT")
            If eventArg = "MyCustomArgument" Then
                FillArrays()               
            End If
        End If

        
    End Sub

   
    Private Sub GetIds()
        'Declare the array and fill with string elements       
        Dim arrfullOrigionalpath() As String = New String() {}
        If InStr(fullOrigionalpath, "&") > 0 Then
            arrfullOrigionalpath = fullOrigionalpath.Split("&")
            CampaignId = CInt(arrfullOrigionalpath(0))
        Else
            arrfullOrigionalpath = fullOrigionalpath.Split("~")
            CampaignId = CInt(arrfullOrigionalpath(0))
        End If
    End Sub

    Private Sub FillArrays()        
        Dim Count As Integer
        FillCampaignInfoArray()
        If IsUnique = 1 Then
            Count = GetUniqueCounts()
            If Count = 0 Then
                FillProcessInfoArray()
                ProcessClick()
            Else
                Link = WebPath & "InactiveCampaign.aspx?url=" + Request.QueryString.ToString
                Response.Redirect(Link)
            End If
        Else
            FillProcessInfoArray()
            ProcessClick()
        End If
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
                If ProcessPercent < LinkPercent Then
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
                            item = dtrFillCampaignInfoArray("LinkId") & "|" & dtrFillCampaignInfoArray("Link") & "|" & dtrFillCampaignInfoArray("LinkPercent") & "|" & dtrFillCampaignInfoArray("UniqueOnly")
                            arclink.Add(item)
                        End While
                    Else
                        Link = WebPath & "InactiveCampaign.aspx"
                        Response.Redirect(Link)
                    End If
                End Using
            End Using
        End Using
        arCampaignLinkInfo = New String(arclink.Count, 3) {}
        'Set Random Link Index
        LinkIndex = randObj.Next(0, arclink.Count)
        For x As Integer = 0 To arclink.Count - 1
            Dim arlinks() As String = New String() {}
            arlinks = arclink(x).Split("|")
            arCampaignLinkInfo(x, 0) = arlinks(0)
            arCampaignLinkInfo(x, 1) = arlinks(1)
            arCampaignLinkInfo(x, 2) = arlinks(2)
            IsUnique = arlinks(3)
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
        arProcessedClickInfo = New String(arlink.Count, 2) {}
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

#End Region

#Region "Insert"
    Private Sub InsertClick(ByVal linkId As Integer, ByVal RURLlink As String)
        Dim intMonth As Integer
        Dim spToUse As String
        intMonth = DatePart(DateInterval.Month, Date.Now)
        spToUse = "Campaign_ProcessClick_" & intMonth
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
        Link = WebPath & "PPage.aspx?c=" & RURLlink 'Server.UrlEncode(RURLlink)
        Response.AddHeader("Location", Link)
        Response.StatusCode = 301
        Response.Redirect(Link)
    End Sub

#End Region

#Region "Uniques"

    Private Function GetUniqueCounts() As Integer
        Dim dt As Date = Today
        Dim mdt As Integer = dt.Month
        Dim TableName As String = "Track_Click_" & mdt
        Dim usedate As Date = DateAdd(DateInterval.Day, -1, dt)
        Dim count As Integer

        Dim sqlGetUniqueCounts As String
        sqlGetUniqueCounts = "Select count(*) " & _
                        " FROM @TableName" & _
                        " WHERE CampaignId = @CampaignId AND IP = '@IP' AND ClickDate > '@ClickDate'"

        sqlGetUniqueCounts = sqlGetUniqueCounts.Replace("@TableName", TableName)
        sqlGetUniqueCounts = sqlGetUniqueCounts.Replace("@CampaignId", CampaignId)
        sqlGetUniqueCounts = sqlGetUniqueCounts.Replace("@IP", Ip)
        sqlGetUniqueCounts = sqlGetUniqueCounts.Replace("@ClickDate", usedate)

        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetUniqueCounts As New SqlCommand(sqlGetUniqueCounts, cnn)
                count = cmdGetUniqueCounts.ExecuteScalar
            End Using
        End Using

        Return count

    End Function


#End Region

#Region "Impressions"

    Private Sub GetImpressionNumber()
        'Create a new Random class in VB.NET
        Dim RandomClass As New Random()
        Dim RandomNumber As Integer
        RandomNumber = RandomClass.Next(3, 8)
        Dim Count As Integer = 0

        While Count < RandomNumber
            Count += 1
            'InsertImpressions()
        End While


        Exit Sub

    End Sub

    Private Sub InsertImpressions()
        Dim intMonth As Integer
        Dim spToUse As String
        intMonth = DatePart(DateInterval.Month, Date.Now)
        spToUse = "Impression_ProcessView_" & intMonth

        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdInsertView As New SqlCommand(spToUse, cnn)
                cmdInsertView.CommandType = CommandType.StoredProcedure
                cmdInsertView.Parameters.Add(New SqlParameter("@ImpressionCampaignId", Data.SqlDbType.Int)).Value = CampaignId
                cmdInsertView.Parameters.Add(New SqlParameter("@Ip", Data.SqlDbType.NVarChar, 15)).Value = Ip
                cmdInsertView.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Sub GetThirdPartyLink()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetLink As New SqlCommand("CampaignAdCopy_GetByCampaignId", cnn)
                cmdGetLink.CommandType = CommandType.StoredProcedure
                cmdGetLink.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = CampaignId
                Using dtrGetLink As SqlDataReader = cmdGetLink.ExecuteReader
                    While dtrGetLink.Read
                        If Not IsDBNull(dtrGetLink("OpenLink1")) Then
                            Open1 = Server.HtmlEncode(dtrGetLink("OpenLink1"))
                        End If
                        If Not IsDBNull(dtrGetLink("OpenLink2")) Then
                            Open2 = Server.HtmlEncode(dtrGetLink("OpenLink2"))
                        End If
                        If Not IsDBNull(dtrGetLink("OpenLink3")) Then
                            Open3 = Server.HtmlEncode(dtrGetLink("OpenLink3"))
                        End If
                    End While
                End Using
            End Using
        End Using
        Label1.Text = Server.HtmlDecode(Open1)
        Label2.Text = Server.HtmlDecode(Open2)
        Label3.Text = Server.HtmlDecode(Open3)

    End Sub

#End Region



End Class
