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

Partial Class V2P1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Trim(Request.Url.Query).Length = 0 Then
            Response.Redirect("InactiveCampaign.aspx&url=" + Server.UrlEncode(Request.Url.AbsoluteUri))
            Response.End()
        End If

        Dim _mtRedirect As HtmlMeta = Page.FindControl("mtRedirect")
        _mtRedirect.Content = "0;URL=V2P2.aspx?l=" + Server.UrlEncode(Request.QueryString("l"))
    End Sub

End Class
