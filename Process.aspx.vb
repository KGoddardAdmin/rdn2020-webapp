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
Imports System.IO
Imports System.Diagnostics
Imports System.ComponentModel

Partial Class Process
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load        
        If Len(Trim(Request.QueryString.ToString)) > 0 Then
            fullOrigionalpath = Request.QueryString("c")
            'Declare the array and fill with string elements       
            Dim arrfullOrigionalpath() As String = New String() {}
            arrfullOrigionalpath = fullOrigionalpath.Split("~")
            CampaignId = CInt(arrfullOrigionalpath(0))
            LinkId = CInt(arrfullOrigionalpath(1))
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
        spToUse = "Campaign_ProcessClick_" & intMonth
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

        Link = WebPath & "ProcessPage.aspx?c=" & Server.UrlEncode(fullOrigionalpath)
        Response.Redirect(Link)

    End Sub

End Class
