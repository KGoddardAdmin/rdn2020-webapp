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

Partial Class V2P
    Inherits System.Web.UI.Page

    Protected ClickLink As String = "V2P1.aspx?l="

    Private m_arCampaignLinkInfo(,) As String
    Private m_Referrer As String
    Private m_Ip As String
    Private m_DCS As String
    Private m_CampaignId As Integer
    Private m_arProcessedClickInfo(,) As String

    'Private Const HARD_CODED_LINK As String = "http://www.namesafe.com"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If True Then
        '    ClickLink += Server.UrlEncode(HARD_CODED_LINK)
        '    Exit Sub
        'End If

        m_Referrer = Request.ServerVariables("HTTP_REFERER")
        m_Ip = Request.ServerVariables.Item("REMOTE_ADDR")
        m_DCS = ConfigurationManager.AppSettings("cnn")

        If Trim(Request.Url.Query).Length = 0 Then
            GoTo _INACTIVE
        End If

        Dim _CData As String = Request.QueryString("c")
        Dim _CParts() As String = _CData.Split("~")
        m_CampaignId = Integer.Parse(_CParts(0))

        If _CParts.Length = 2 Then
            Dim _LinkId As Integer = Integer.Parse(_CParts(1))

            Using _Cn As SqlConnection = New SqlConnection(m_DCS)
                Using _Cmd As SqlCommand = New SqlCommand("Campaign_ProcessClick_" + DatePart(DateInterval.Month, Date.Now).ToString(), _Cn)
                    _Cmd.CommandType = Data.CommandType.StoredProcedure
                    _Cmd.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = m_CampaignId
                    _Cmd.Parameters.Add(New SqlParameter("@LinkId", Data.SqlDbType.Int)).Value = _LinkId
                    If Trim(Len(m_Referrer)) > 0 Or Trim(m_Referrer) <> String.Empty Then
                        _Cmd.Parameters.Add(New SqlParameter("@Referrer", Data.SqlDbType.NVarChar, 100)).Value = m_Referrer
                    Else
                        _Cmd.Parameters.Add(New SqlParameter("@Referrer", Data.SqlDbType.NVarChar, 100)).Value = DBNull.Value
                    End If
                    _Cmd.Parameters.Add(New SqlParameter("@Ip", Data.SqlDbType.NVarChar, 15)).Value = m_Ip
                    _Cn.Open()
                    _Cmd.ExecuteNonQuery()
                End Using
            End Using

            Dim _HasResult As Boolean = False
            Using _Cn As SqlConnection = New SqlConnection(m_DCS)
                Using _Cmd As SqlCommand = New SqlCommand("campaign_getlink", _Cn)
                    _Cmd.CommandType = Data.CommandType.StoredProcedure
                    _Cmd.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = m_CampaignId
                    _Cmd.Parameters.Add(New SqlParameter("@LinkId", Data.SqlDbType.Int)).Value = _LinkId

                    _Cn.Open()
                    Dim _ExecResult As Object = _Cmd.ExecuteScalar()
                    If (Not IsDBNull(_ExecResult)) AndAlso _ExecResult.Length > 0 Then
                        ClickLink += Server.UrlEncode(_ExecResult)
                        _HasResult = True
                    End If
                End Using
            End Using
            _HasResult = True
            If _HasResult Then
                Exit Sub
            Else
                GoTo _INACTIVE
            End If
        Else
            Dim _List As ArrayList = New ArrayList
            Dim _HasRows As Boolean = False
            Using _Cn As SqlConnection = New SqlConnection(m_DCS)
                Using _Cmd As SqlCommand = New SqlCommand("Campaign_GetForProcessingByCampaignId", _Cn)
                    _Cmd.CommandType = CommandType.StoredProcedure
                    _Cmd.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = m_CampaignId

                    _Cn.Open()
                    Using _Dr As SqlDataReader = _Cmd.ExecuteReader
                        If _Dr.HasRows Then
                            _HasRows = True
                            While _Dr.Read
                                _List.Add(_Dr("LinkId").ToString() + "|" + _Dr("Link").ToString() + "|" + _Dr("LinkPercent").ToString())
                            End While
                        End If
                    End Using
                End Using
            End Using
            If _HasRows Then
                Dim randObj As New Random
                Dim LinkIndex As Integer = randObj.Next(0, _List.Count)

                m_arCampaignLinkInfo = New String(_List.Count, 2) {}
                For _ListIndex As Integer = 0 To _List.Count - 1
                    Dim arlinks() As String = New String() {}
                    arlinks = _List(_ListIndex).Split("|")
                    m_arCampaignLinkInfo(_ListIndex, 0) = arlinks(0)
                    m_arCampaignLinkInfo(_ListIndex, 1) = arlinks(1)
                    m_arCampaignLinkInfo(_ListIndex, 2) = arlinks(2)
                    Array.Clear(arlinks, 0, arlinks.Length)
                Next
                _List.Clear()

                Dim TotalClicks As Integer
                Dim arlink As New ArrayList
                Using cnn As New SqlConnection(m_DCS)
                    cnn.Open()
                    Using cmdtest As New SqlCommand("Track_Click_ProcessingCounts", cnn)
                        cmdtest.CommandType = CommandType.StoredProcedure
                        cmdtest.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = m_CampaignId
                        Using dtrtest As SqlDataReader = cmdtest.ExecuteReader
                            While dtrtest.Read
                                Dim item As String
                                TotalClicks += dtrtest("TotalClicks")
                                item = dtrtest("linkid") & "|" & dtrtest("TotalClicks")
                                arlink.Add(item)
                            End While
                        End Using
                    End Using
                End Using
                m_arProcessedClickInfo = New String(arlink.Count, 2) {}
                For x As Integer = 0 To arlink.Count - 1
                    Dim arlinks() As String = New String() {}
                    arlinks = arlink(x).Split("|")
                    m_arProcessedClickInfo(x, 0) = arlinks(0)
                    m_arProcessedClickInfo(x, 1) = arlinks(1)
                    Dim percent As Double = (arlinks(1) / TotalClicks) * 100
                    percent = percent \ 1
                    m_arProcessedClickInfo(x, 2) = percent
                    Array.Clear(arlinks, 0, arlinks.Length)
                Next
                arlink.Clear()

                Dim foundlink As Boolean = False
                Dim foundlinkindex As Integer

                For a As Integer = 0 To m_arProcessedClickInfo.GetUpperBound(0)
                    If m_arProcessedClickInfo(a, 0) = m_arCampaignLinkInfo(LinkIndex, 0) Then
                        foundlink = True
                        foundlinkindex = a
                        Dim ProcessPercent As Integer = m_arProcessedClickInfo(a, 2)
                        Dim LinkPercent As Integer = m_arCampaignLinkInfo(LinkIndex, 2)
                        If ProcessPercent < LinkPercent Then
                            Dim lid As Integer = m_arCampaignLinkInfo(LinkIndex, 0)
                            InsertClick(lid, m_arCampaignLinkInfo(LinkIndex, 1))
                            Exit Sub
                        End If
                        Exit For
                    End If
                Next

                If foundlink = False Then
                    Dim lid As Integer = m_arCampaignLinkInfo(LinkIndex, 0)
                    InsertClick(lid, m_arCampaignLinkInfo(LinkIndex, 1))
                    Exit Sub
                End If

                'Check if all links have been processed at least once
                For b As Integer = 0 To m_arCampaignLinkInfo.GetUpperBound(0)
                    Dim linkProcessed As Boolean = False
                    For c As Integer = 0 To m_arProcessedClickInfo.GetUpperBound(0)
                        If m_arCampaignLinkInfo(b, 0) = m_arProcessedClickInfo(c, 0) Then
                            linkProcessed = True
                        End If
                    Next
                    If b < m_arCampaignLinkInfo.GetUpperBound(0) Then
                        If linkProcessed = False Then
                            Dim lid As Integer = m_arCampaignLinkInfo(b, 0)
                            InsertClick(lid, m_arCampaignLinkInfo(b, 1))
                            Exit Sub
                        End If
                    End If
                Next

                For d As Integer = 0 To m_arCampaignLinkInfo.GetUpperBound(0)
                    For _e As Integer = 0 To m_arProcessedClickInfo.GetUpperBound(0)
                        If m_arCampaignLinkInfo(d, 0) = m_arProcessedClickInfo(_e, 0) Then
                            Dim LinkPercent As Integer = m_arCampaignLinkInfo(d, 2)
                            Dim ProcessPercent As Integer = m_arProcessedClickInfo(_e, 2)
                            If ProcessPercent < LinkPercent Then
                                Dim lid As Integer = m_arCampaignLinkInfo(d, 0)
                                InsertClick(lid, m_arCampaignLinkInfo(d, 1))
                                Exit Sub
                            End If
                        End If
                    Next
                Next
                InsertClick(m_arCampaignLinkInfo(LinkIndex, 0), m_arCampaignLinkInfo(LinkIndex, 1))
                Exit Sub
            End If
        End If

_INACTIVE:
        Response.Redirect("InactiveCampaign.aspx&url=" + Server.UrlEncode(Request.Url.AbsoluteUri))
        Response.End()
    End Sub

    Private Sub InsertClick(ByVal inLinkId As Integer, ByVal inLink As String)
        Dim intMonth As Integer = DatePart(DateInterval.Month, Date.Now)
        Using cnn As New SqlConnection(m_DCS)
            cnn.Open()
            Using cmdProcessClick As New SqlCommand("Campaign_ProcessClick_" & intMonth, cnn)
                cmdProcessClick.CommandType = Data.CommandType.StoredProcedure
                cmdProcessClick.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = m_CampaignId
                cmdProcessClick.Parameters.Add(New SqlParameter("@LinkId", Data.SqlDbType.Int)).Value = inLinkId
                cmdProcessClick.Parameters.Add(New SqlParameter("@Ip", Data.SqlDbType.NVarChar, 15)).Value = m_Ip
                If Trim(Len(m_Referrer)) > 0 Or Trim(m_Referrer) <> String.Empty Then
                    cmdProcessClick.Parameters.Add(New SqlParameter("@Referrer", Data.SqlDbType.NVarChar, 100)).Value = m_Referrer
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

        Array.Clear(m_arCampaignLinkInfo, 0, m_arCampaignLinkInfo.Length)
        Array.Clear(m_arProcessedClickInfo, 0, m_arProcessedClickInfo.Length)

        ClickLink += Server.UrlEncode(inLink)
    End Sub
End Class
