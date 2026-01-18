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

Partial Class P1Page
    Inherits System.Web.UI.Page

    Public ShowTable As Boolean = False
    Public Link As String
    Private ReadOnly _webPath As String = ConfigurationSettings.AppSettings("WebPath")
    Private _link As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Len(Trim(Request.QueryString.ToString)) > 0 Then
            _link = Request.QueryString("c")
            SetCookie(_link)
            _link = Server.UrlDecode(_link)
            'Declare the array and fill with string elements       
            Link = "http://cloakedlink.com/" & _link
        Else
            Link = _webPath & "InactiveCampaign.aspx"
            Response.Redirect(Link)
        End If
    End Sub

    Private Sub SetCookie(ByVal campaign As String)
        Dim cookie As HttpCookie
        Try
            cookie = New HttpCookie("RDNCookie", campaign)
            cookie.Expires = DateTime.Now.AddMinutes(30)
            Response.Cookies.Add(cookie)
        Catch ex As Exception
            Link = _webPath & "InactiveCampaign.aspx"
            Response.Redirect(Link)
        End Try
    End Sub
End Class
