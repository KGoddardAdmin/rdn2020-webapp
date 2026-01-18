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

Partial Class PNonJavaScriptProcess
    Inherits System.Web.UI.Page

    Public Link As String
    Public ShowTable As Boolean = False
    Public NonJavaLink As String
    Private fullOrigionalpath As String
    Private WebPath As String = ConfigurationSettings.AppSettings("WebPath")
    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Private strLink As String
    Public Referrer As String
    Private CampaignId As Integer
    Private LinkId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.Cookies("RDNCookie") Is Nothing Then
            Response.Write("We are sorry we are experiencing some technical difficulties please try clicking through again in a few minutes.")
        Else
            strLink = Server.UrlDecode(Request.Cookies("RDNCookie").Value)
            Dim i As Integer
            Dim RDNCookie As HttpCookie
            'Dim cookieName As String
            Dim limit As Integer = Request.Cookies.Count - 1
            For i = 0 To limit
                RDNCookie = Request.Cookies(i)
                RDNCookie.Expires = DateTime.Now.AddDays(-1)
            Next
            Link = strLink
        End If
    End Sub


End Class
