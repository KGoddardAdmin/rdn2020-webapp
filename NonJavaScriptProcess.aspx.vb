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

Partial Class NonJavaScriptProcess
    Inherits System.Web.UI.Page

    Public Link As String
    Public ShowTable As Boolean = False
    Public NonJavaLink As String
    Private fullOrigionalpath As String
    Private WebPath As String = ConfigurationSettings.AppSettings("WebPath")
    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Public Referrer As String
    Private CampaignId As Integer
    Private LinkId As Integer


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.Cookies("LmmCookie") Is Nothing Then
            Response.Write("We are sorry we are experiencing some technical difficulties please try clicking through again in a few minutes.")
        Else
            fullOrigionalpath = Server.UrlDecode(Request.Cookies("LmmCookie").Value)
            Dim arrfullOrigionalpath() As String = New String() {}
            arrfullOrigionalpath = fullOrigionalpath.Split("~")
            CampaignId = CInt(arrfullOrigionalpath(0))
            LinkId = CInt(arrfullOrigionalpath(1))
            'Response.Write("Campaign Id = " & CampaignId)
            'Response.Write("<br>Link Id = " & LinkId)

            Dim i As Integer
            Dim LmmCookie As HttpCookie
            'Dim cookieName As String
            Dim limit As Integer = Request.Cookies.Count - 1
            For i = 0 To limit
                LmmCookie = Request.Cookies(i)
                LmmCookie.Expires = DateTime.Now.AddDays(-1)
            Next                        
        End If
        ProcessClick(CampaignId, LinkId)

    End Sub

    Private Sub ProcessClick(ByVal CampaignId As Integer, ByVal LinkId As Integer)
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdProcessClick As New SqlCommand("campaign_getlink", cnn)
                cmdProcessClick.CommandType = Data.CommandType.StoredProcedure
                cmdProcessClick.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = CampaignId
                cmdProcessClick.Parameters.Add(New SqlParameter("@LinkId", Data.SqlDbType.Int)).Value = LinkId

                If Not IsDBNull(cmdProcessClick.ExecuteScalar) Then
                    Link = cmdProcessClick.ExecuteScalar
                Else
                    Link = "InactiveCampaign.aspx"
                    Response.Redirect(Link)
                    Exit Sub
                End If

                If Len(Link) = 0 Or Link = String.Empty Then
                    Link = "InactiveCampaign.aspx"
                    Response.Redirect(Link)
                    Exit Sub
                End If

            End Using
        End Using
    End Sub
End Class
