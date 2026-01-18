Imports System.Data
Imports System.Data.SqlClient

Partial Class Portal_AutomatedOpenGen
    Inherits System.Web.UI.Page

    Private _cnn As String = ConfigurationSettings.AppSettings("cnn")
    Private _endDate As Date = DateAdd("d", 1, Today())
    Private ReadOnly _startDate As Date = _endDate.AddDays(-8)
    Private _dataBase As String
    Private Const MaxRan As Integer = 5
    Private _randomNumber As Integer
    Private _arNum As ArrayList
    Private Const Ip As String = "0.0.0.0"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _dataBase = Request.QueryString("Db")
        GetCampaignIds()
    End Sub

    Private Function GetImpressionNumber() As Integer
        'Create a new Random class in VB.NET
        Dim rndClass As New Random()
        _randomNumber = rndClass.Next(1, MaxRan)
        Return _randomNumber
    End Function

    Private Sub GetImpressions()
        _arNum = New ArrayList
        Dim intMonth As Integer
        Dim table As String
        intMonth = DatePart(DateInterval.Month, Date.Now)
        If intMonth = 1 Then
            intMonth = 12
        Else
            intMonth = intMonth - 1
        End If
        table = "Impression_" & intMonth

        _randomNumber = GetImpressionNumber()

        For x As Integer = 0 To _randomNumber
            Using cnn As New SqlConnection(_cnn)
                cnn.Open()
                Using cmdGetImpressions As New SqlCommand("sp_AutomaticOpenGen_GetIPs", cnn)
                    cmdGetImpressions.CommandType = CommandType.StoredProcedure
                    cmdGetImpressions.Parameters.Add(New SqlParameter("@Domain", SqlDbType.VarChar, 50)).Value = _dataBase
                    cmdGetImpressions.Parameters.Add(New SqlParameter("@Table", SqlDbType.VarChar, 50)).Value = table
                    Using dtrGetImpressions As SqlDataReader = cmdGetImpressions.ExecuteReader
                        If dtrGetImpressions.HasRows Then
                            While dtrGetImpressions.Read

                                If Not IsDBNull(dtrGetImpressions("IP")) Then
                                    _arNum.Add(dtrGetImpressions("IP"))
                                Else
                                    _arNum.Add(IP)
                                End If
                            End While
                        End If
                    End Using
                End Using
            End Using
        Next

    End Sub

    Private Sub GetCampaignIds()
        Using cnn As New SqlConnection(_cnn)
            cnn.Open()
            Using cmdGetCampaignIds As New SqlCommand("sp_AutomaticOpenGen_GetCampaigns", cnn)
                cmdGetCampaignIds.CommandType = CommandType.StoredProcedure
                cmdGetCampaignIds.Parameters.Add(New SqlParameter("@Domain", SqlDbType.VarChar, 50)).Value = _dataBase
                cmdGetCampaignIds.Parameters.Add(New SqlParameter("@StartDate", SqlDbType.DateTime)).Value = _startDate
                cmdGetCampaignIds.Parameters.Add(New SqlParameter("@EndDate", SqlDbType.DateTime)).Value = _endDate
                Using dtrGetCampaignIds As SqlDataReader = cmdGetCampaignIds.ExecuteReader
                    While dtrGetCampaignIds.Read
                        InsertOpens(dtrGetCampaignIds("CampaignId"))
                    End While
                End Using
            End Using
        End Using
    End Sub

    Private Sub InsertOpens(ByVal campaignId As Integer)
        GetImpressions()
        For x As Integer = 0 To _arNum.Count - 1
            InsertImpressions(_arNum(x), campaignId)
        Next
    End Sub

    Private Sub InsertImpressions(ByVal ip As String, ByVal campaignId As Integer)
        Dim intMonth As Integer
        Dim table As String
        intMonth = DatePart(DateInterval.Month, Date.Now)
        table = "Impression_" & intMonth
        Dim ip1 As String = String.Empty
        Dim ip2 As String = String.Empty
        Dim ip3 As String = String.Empty
        Dim ip4 As String = String.Empty
        Dim arrIp() As String
        arrIp = ip.Split(".")
        Try
            ip1 = arrIp(0)
            ip2 = arrIp(1)
            ip3 = arrIp(2)
            ip4 = arrIp(3)
        Catch ex As Exception
            '//Eat the exception
        End Try
        Using cnn As New SqlConnection(_cnn)
            cnn.Open()
            Using insert As New SqlCommand("sp_AutomaticOpenGen_InsertOpens", cnn)
                insert.CommandType = CommandType.StoredProcedure

                insert.Parameters.Add(New SqlParameter("@Domain", SqlDbType.VarChar, 50)).Value = _dataBase
                insert.Parameters.Add(New SqlParameter("@Table", SqlDbType.VarChar, 50)).Value = table
                insert.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = campaignId
                insert.Parameters.Add(New SqlParameter("@IP1", SqlDbType.Char, 3)).Value = ip1
                insert.Parameters.Add(New SqlParameter("@IP2", SqlDbType.Char, 3)).Value = ip2
                insert.Parameters.Add(New SqlParameter("@IP3", SqlDbType.Char, 3)).Value = ip3
                insert.Parameters.Add(New SqlParameter("@IP4", SqlDbType.Char, 3)).Value = ip4
                insert.ExecuteNonQuery()
            End Using
        End Using
    End Sub
End Class
