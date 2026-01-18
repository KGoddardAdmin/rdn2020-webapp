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

Partial Class PPage
    Inherits System.Web.UI.Page

    Public Link As String    
    Public ShowTable As Boolean = False    
    Private WebPath As String = ConfigurationSettings.AppSettings("WebPath")
    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Private strLink As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Len(Trim(Request.QueryString.ToString)) > 0 Then
            strLink = Request.QueryString("c")
            SetCookie(strLink)
            strLink = Server.UrlDecode(strLink)
            'Declare the array and fill with string elements       
            Link = strLink            
        Else
            Link = WebPath & "InactiveCampaign.aspx"
            Response.Redirect(Link)
        End If
    End Sub


    Private Sub SetCookie(ByVal campaign As String)

        Dim objCookie As HttpCookie
        Try
            objCookie = New HttpCookie("RDNCookie", campaign)
            objCookie.Expires = DateTime.Now.AddMinutes(30)
            Response.Cookies.Add(objCookie)
        Catch ex As Exception
            Link = WebPath & "InactiveCampaign.aspx"
            Response.Redirect(Link)
        End Try

    End Sub


End Class
