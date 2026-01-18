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

Partial Class V2P2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim _WebPath As String = ConfigurationManager.AppSettings("WebPath")
        Dim _Referrer As String = Request.ServerVariables("HTTP_REFERER")
        If (Trim(_Referrer) <> "") AndAlso (Left(_Referrer, _WebPath.Length) <> _WebPath) Then
            Response.Redirect("InactiveCampaign.aspx&ref=" + _Referrer + "&url=" + Server.UrlEncode(Request.Url.AbsoluteUri))
            Response.End()
        Else
            Response.Redirect(Request.QueryString("l"))
            Response.End()
        End If
    End Sub
End Class
