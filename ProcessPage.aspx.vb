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

Partial Class ProcessPage
    Inherits System.Web.UI.Page

    Public Link As String
    Public ShowTable As Boolean = False   
    Private fullOrigionalpath As String
    Private WebPath As String = ConfigurationSettings.AppSettings("WebPath")
    Private strConn As String = ConfigurationSettings.AppSettings("cnn")    
    Private CampaignId As Integer
    Private LinkId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load        
        If Len(Trim(Request.QueryString.ToString)) > 0 Then
            fullOrigionalpath = Request.QueryString("c")
            SetCookie(fullOrigionalpath)
            fullOrigionalpath = Server.UrlDecode(fullOrigionalpath)
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

        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdProcessClick As New SqlCommand("campaign_getlink", cnn)
                cmdProcessClick.CommandType = Data.CommandType.StoredProcedure
                cmdProcessClick.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = CampaignId
                cmdProcessClick.Parameters.Add(New SqlParameter("@LinkId", Data.SqlDbType.Int)).Value = LinkId

                If Not IsDBNull(cmdProcessClick.ExecuteScalar) Then
                    Link = cmdProcessClick.ExecuteScalar
                Else
                    Link = WebPath & "InactiveCampaign.aspx"
                    Response.Redirect(Link)
                    Exit Sub
                End If

                If Len(Link) = 0 Or Link = String.Empty Then
                    Link = WebPath & "InactiveCampaign.aspx"
                    Response.Redirect(Link)
                    Exit Sub
                End If

            End Using
        End Using
    End Sub

    Private Sub SetCookie(ByVal campaign As String)

        Dim objCookie As HttpCookie
        Try
            objCookie = New HttpCookie("LmmCookie", campaign)
            objCookie.Expires = DateTime.Now.AddMinutes(30)
            Response.Cookies.Add(objCookie)
        Catch ex As Exception            
            Link = WebPath & "InactiveCampaign.aspx"
            Response.Redirect(Link)
        End Try
       
    End Sub

End Class
