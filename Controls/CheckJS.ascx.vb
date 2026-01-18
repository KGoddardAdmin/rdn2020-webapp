Imports System.Collections
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Collections.Specialized
Imports System.Text
Imports System
Imports Microsoft.VisualBasic

Namespace Controls
    Public Class Controls_CheckJS
        Inherits System.Web.UI.UserControl
        Protected Shared JSQRYPARAM As String = "jse"
        Protected Shared JSENABLED As String = "1"
        Protected Shared JSDISABLED As String = "0"

        Protected Overloads Overrides Sub OnInit(ByVal e As EventArgs)
            MyBase.OnInit(e)
            Dim testJS As Boolean = IsJSEnabled
            If Request.QueryString(JSQRYPARAM) IsNot Nothing Then
                IsJSEnabled = Request.QueryString(JSQRYPARAM) = JSENABLED
            End If
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                Dim cookie As HttpCookie = Request.Cookies("jsEnabled")
                If cookie IsNot Nothing Then
                    ' JavaScript is enabled
                    Response.Cookies("jsEnabled").Expires = DateTime.Now.AddDays(-1)
                End If
            End If
        End Sub
        Protected Function GetAppendedUrl(ByVal newParam As String, ByVal newParamValue As String) As String
            Dim targeturl As String = String.Empty
            Dim url As Uri = IIf((String.IsNullOrEmpty(ResolveUrl(NonJSTargetURL))), New Uri(ResolveUrl(JSTargetURL)), New Uri(ResolveUrl(NonJSTargetURL)))
            If url Is Nothing Then
                url = Request.Url
            End If

            Dim qry As String() = url.Query.Replace("?", "").Split("&"c)

            Dim sb As New StringBuilder()
            For Each s As String In qry
                If Not s.ToLower().Contains(newParam.ToLower()) Then
                    sb.Append(s & "&")
                End If
            Next

            If sb.Length > 0 Then
                sb.Remove(sb.Length - 1, 1)
                targeturl = String.Format("{0}?{1}&{2}={3}", url.AbsolutePath, sb.ToString(), newParam, newParamValue)
            Else
                targeturl = String.Format("{0}?{1}={2}", url.AbsolutePath, newParam, newParamValue)
            End If
            Return targeturl
        End Function
        Protected Overloads Overrides Sub OnPreRender(ByVal e As EventArgs)
            MyBase.OnPreRender(e)
            If IsJSEnabled Then
                Dim targeturl As String = GetAppendedUrl(JSQRYPARAM, JSDISABLED)
                Dim ctrl As New HtmlGenericControl("NOSCRIPT")
                ctrl.InnerHtml = String.Format("<meta http-equiv=REFRESH content=0;URL={0}>", targeturl)
                Page.Header.Controls.Add(ctrl)
            Else
                If Not String.IsNullOrEmpty(NonJSTargetURL) Then
                    Response.Redirect(NonJSTargetURL)
                End If
                Dim ctrl As New HtmlGenericControl("NOSCRIPT")
                ctrl.InnerHtml = String.Empty
                Page.Header.Controls.Add(ctrl)
            End If
        End Sub
        Protected Property IsJSEnabled() As Boolean
            Get
                If Session("JS") Is Nothing Then
                    Session("JS") = True
                End If

                Return CBool(Session("JS"))
            End Get
            Set(ByVal value As Boolean)
                Session("JS") = value
            End Set
        End Property
        Protected ReadOnly Property JSTargetURL() As String
            Get
                Return Request.Url.ToString()
            End Get
        End Property
        Public Property NonJSTargetURL() As String
            Get
                Return IIf((ViewState("NONJSURL") IsNot Nothing), ViewState("NONJSURL").ToString(), String.Empty)
            End Get
            Set(ByVal value As String)
                Try
                    ViewState("NONJSURL") = ResolveServerUrl(value, False)
                Catch
                    Throw New ApplicationException("Invalid URL. '" & value & "'")
                End Try
            End Set
        End Property
        Public Function ResolveServerUrl(ByVal serverUrl As String, ByVal forceHttps As Boolean) As String
            If serverUrl.IndexOf("://") > -1 Then

                Return serverUrl
            End If
            Dim newUrl As String = ResolveUrl(serverUrl)
            Dim originalUri As Uri = HttpContext.Current.Request.Url


            newUrl = ((IIf(forceHttps, "https", originalUri.Scheme)) & "://") + originalUri.Authority + newUrl
            Return newUrl
        End Function
    End Class

    Public Class CheckJavaScriptHelper
        Public Shared ReadOnly Property IsJavascriptEnabled() As Boolean
            Get
                If HttpContext.Current.Session("JS") Is Nothing Then
                    HttpContext.Current.Session("JS") = True
                End If

                Return CBool(HttpContext.Current.Session("JS"))
            End Get
        End Property

    End Class

End Namespace
