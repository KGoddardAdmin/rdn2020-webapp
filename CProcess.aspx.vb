Imports System.Data.SqlClient
Imports System.Data
Imports System.Net
Imports System.IO
Imports System
Imports System.Diagnostics
Imports System.ComponentModel
Imports System.Collections.Generic


Partial Class CProcess
    Inherits System.Web.UI.Page

    Public Link As String
    Public ShowTable As Boolean = False
    Public IsVista As Boolean = False
    Private WebPath As String = ConfigurationSettings.AppSettings("WebPath")
    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Private Ip As String = Context.Request.ServerVariables.Item("REMOTE_ADDR") 'Request.ServerVariables.Item("REMOTE_ADDRESS")  
    Private Referrer As String = Context.Request.ServerVariables("HTTP_REFERER")
    Private CampaignId As Integer
    Private LinkId As Integer
    Private fullOrigionalpath As String
    Private CouponCode As String = String.Empty
    Private strURL As String
    Private arURL As New ArrayList()
    'Private Pass As String = String.Empty
    'Private L As String = String.Empty
    'Private B As String = String.Empty


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        strURL = HttpContext.Current.Request.Url.AbsoluteUri
        Dim arrfullOrigionalpath() As String = New String() {}
        fullOrigionalpath = Request.QueryString("c")
        arrfullOrigionalpath = fullOrigionalpath.Split("~")
        'Pass = Request.QueryString("pwd")
        'L = Request.QueryString("l")
        'B = Request.QueryString("b")
        If Len(Trim(Request.QueryString.ToString)) > 0 Then
            CampaignId = CInt(arrfullOrigionalpath(0))
            LinkId = CInt(arrfullOrigionalpath(1))
            If arrfullOrigionalpath.Length = 3 Then
                CouponCode = arrfullOrigionalpath(2)
            End If
            ProcessClick(CampaignId, LinkId)
        Else
            Link = WebPath & "InactiveCampaign.aspx"
            Response.Redirect(Link)
        End If

    End Sub

    Private Sub ProcessClick(ByVal CampaignId As Integer, ByVal LinkId As Integer)
        Dim intMonth As Integer
        Dim spToUse As String
        intMonth = DatePart(DateInterval.Month, Date.Now)
        spToUse = "CCampaign_ProcessClick_" & intMonth
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdProcessClick As New SqlCommand(spToUse, cnn)
                cmdProcessClick.CommandType = Data.CommandType.StoredProcedure
                cmdProcessClick.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = CampaignId
                cmdProcessClick.Parameters.Add(New SqlParameter("@LinkId", Data.SqlDbType.Int)).Value = LinkId
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
        Link = GetUrlParameters()                
        Response.Redirect(Link)
    End Sub
    Private Function GetUrlParameters()
        Dim rtn As String
        Dim url As String = strURL
        ' consider only the querystring, that's the part after the ? char
        Dim qsStart As Integer = url.IndexOf("?")
        If qsStart > -1 Then url = url.Substring(qsStart + 1)
        ' split the querystring with the & char
        Dim params() As String = url.Split(New Char() {"&"c})
        Dim param As String
        ' for each param extract the param name (the part before the =) and the 
        ' value
        'Dim arlist As New ArrayList()
        For Each param In params
            Dim i As Integer = param.IndexOf("="c)
            If i > -1 Then
                Dim kv As New KeyValuePair(Of String, String)(param.Substring(0, i), param.Substring(i + 1))
                arURL.Add(kv)
            End If
        Next
        Dim counter As Integer = 0
        Dim kvl As KeyValuePair(Of String, String)
        rtn = WebPath & "CProcessPage.aspx?"
        For Each kvl In arURL
            If counter = 0 Then
                rtn = rtn & kvl.Key & "=" & kvl.Value
            Else
                rtn = rtn & "&" & kvl.Key & "=" & kvl.Value
            End If
            counter += 1
        Next
        Return rtn

    End Function
End Class
