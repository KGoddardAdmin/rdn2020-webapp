Imports System.Data.SqlClient
Imports System.Data

Partial Class CP
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
    Private CouponCode As String
    Private CouponGuid As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Len(Trim(Request.QueryString.ToString)) > 0 Then
            fullOrigionalpath = Request.QueryString("c")            
            Dim arrfullOrigionalpath() As String = New String() {}
            arrfullOrigionalpath = fullOrigionalpath.Split("~")
            CampaignId = arrfullOrigionalpath(0) 'CInt(arrfullOrigionalpath(0))
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
                If ProcessPercent < LinkPercent Then
                    Dim lid As Integer = arCampaignLinkInfo(LinkIndex, 0)
                    InsertClick(lid, arCampaignLinkInfo(LinkIndex, 1), arCampaignLinkInfo(LinkIndex, 3))                    
                    Exit Sub
                End If
                Exit For
            End If
        Next

        If foundlink = False Then
            Dim lid As Integer = arCampaignLinkInfo(LinkIndex, 0)
            InsertClick(lid, arCampaignLinkInfo(LinkIndex, 1), arCampaignLinkInfo(LinkIndex, 3))
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
                    InsertClick(lid, arCampaignLinkInfo(b, 1), arCampaignLinkInfo(b, 3))
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
                        InsertClick(lid, arCampaignLinkInfo(d, 1), arCampaignLinkInfo(d, 3))
                        Exit Sub
                    End If
                End If
            Next
        Next
        InsertClick(arCampaignLinkInfo(LinkIndex, 0), arCampaignLinkInfo(LinkIndex, 1), arCampaignLinkInfo(LinkIndex, 3))
    End Sub


#Region "Fill Arrays"

    Private Sub FillCampaignInfoArray()

        Dim randObj As New Random
        Dim arclink As ArrayList = New ArrayList
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdFillCampaignInfoArray As New SqlCommand("Campaign_GetForProcessingByCampaignIdWithCoupon", cnn)
                cmdFillCampaignInfoArray.CommandType = CommandType.StoredProcedure
                cmdFillCampaignInfoArray.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = CampaignId
                Using dtrFillCampaignInfoArray As SqlDataReader = cmdFillCampaignInfoArray.ExecuteReader
                    If dtrFillCampaignInfoArray.HasRows Then
                        While dtrFillCampaignInfoArray.Read
                            Dim item As String
                            item = dtrFillCampaignInfoArray("LinkId") & "|" & dtrFillCampaignInfoArray("Link") & "|" & dtrFillCampaignInfoArray("LinkPercent") & "|" & dtrFillCampaignInfoArray("CouponVariable")
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
    Private Sub InsertClick(ByVal linkId As Integer, ByVal RURLink As String, ByVal cid As String)
        'Response.Write("LInk Id = " & linkId)
        'Response.Write("<br>Redirect link = " & RURLink)
        'Response.Write("<br> coupon id = " & cid)
        'Exit Sub

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

        If InStr(RURLink, cid) > 0 Then
            GetCouponId()
            RURLink = Replace(RURLink, cid, CouponCode, 1, -1, CompareMethod.Text)
            UpdateCoupon()
        End If
        Link = WebPath & "CPPage.aspx?c=" & Server.UrlEncode(RURLink)

        Response.Redirect(Link)

    End Sub

#End Region

#Region "Coupon Functions"

    Private Function GetCouponIPCount() As String
        Dim Count As Integer
        Dim rtn As String
        Dim tablename As String = "Coupon_" & CampaignId
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetCouponIPCount As New SqlCommand("Coupon_GetIPCount", cnn)
                cmdGetCouponIPCount.CommandType = CommandType.StoredProcedure
                cmdGetCouponIPCount.Parameters.Add(New SqlParameter("@Table_Name", SqlDbType.NVarChar)).Value = tablename
                cmdGetCouponIPCount.Parameters.Add(New SqlParameter("@Ip", SqlDbType.NVarChar)).Value = Ip
                Count = cmdGetCouponIPCount.ExecuteScalar
            End Using
        End Using
        If Count > 0 Then
            rtn = "Y"
        Else
            rtn = "N'"
        End If
        Return rtn
    End Function

    Private Sub GetCouponId()

        Dim tablename As String = "Coupon_" & CampaignId
        Dim IPUsed As String
        IPUsed = GetCouponIPCount()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetCouponId As New SqlCommand("Coupon_GetCouponCode", cnn)
                cmdGetCouponId.CommandType = CommandType.StoredProcedure
                cmdGetCouponId.Parameters.Add(New SqlParameter("@Table_Name", SqlDbType.NVarChar)).Value = tablename
                cmdGetCouponId.Parameters.Add(New SqlParameter("@IP", SqlDbType.NVarChar)).Value = Ip
                cmdGetCouponId.Parameters.Add(New SqlParameter("@IPUsed", SqlDbType.NVarChar)).Value = IPUsed                
                Using dtrGetCouponId As SqlDataReader = cmdGetCouponId.ExecuteReader
                    While dtrGetCouponId.Read
                        CouponCode = dtrGetCouponId("CouponCode")
                        CouponGuid = dtrGetCouponId("CouponGuid").ToString
                    End While
                End Using
            End Using
        End Using
        UpdateCoupon()

    End Sub

    Private Sub UpdateCoupon()
        Dim tablename As String = "Coupon_" & CampaignId

        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdUpdateCoupon As New SqlCommand("Coupon_MarkAsUsed", cnn)
                cmdUpdateCoupon.CommandType = CommandType.StoredProcedure
                cmdUpdateCoupon.Parameters.Add(New SqlParameter("@Table_Name", SqlDbType.NVarChar)).Value = tablename
                cmdUpdateCoupon.Parameters.Add(New SqlParameter("@CouponGuid", SqlDbType.UniqueIdentifier)).Value = SqlTypes.SqlGuid.Parse(CouponGuid)
                cmdUpdateCoupon.Parameters.Add(New SqlParameter("@Ip", SqlDbType.VarChar)).Value = Ip
                cmdUpdateCoupon.ExecuteNonQuery()
            End Using
        End Using
    End Sub

#End Region



End Class
