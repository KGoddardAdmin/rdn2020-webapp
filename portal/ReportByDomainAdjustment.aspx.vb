Imports System.Data
Imports System.Data.SqlClient

Partial Class portal_ReportByDomainAdjustment
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Private strRTConn As String = ConfigurationSettings.AppSettings("trconstr")
    Private CampaignName As String
    Private DomainId As Integer
    Private Domain As String
    Private msg As String
    Private UrlOPens As Integer
    Private UrlClics As Integer
    Private ExistingOpens As Integer
    Private ExistingClicks As Integer
    Private ClicksToAdd As Integer
    Private OpensToAdd As Integer
    Private FinalClicks As Integer
    Private FinalOpens As Integer
    Private DataBase As String
    Private TableName As String
    Private CountInCurrentMonth As Boolean = True

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        CampaignName = Request.QueryString("Name")
        DomainId = Request.QueryString("DomainID")
        Domain = Request.QueryString("Domain")       
        If Not IsPostBack Then            
            If Request.QueryString("Opens") <> "" Then
                UrlOPens = Request.QueryString("Opens")
            Else
                UrlOPens = 0
            End If
            If Request.QueryString("Clicks") <> "" Then
                UrlClics = Request.QueryString("Clicks")
            Else
                UrlClics = 0
            End If
            msg = "Enter the counts to add to " & Domain & " campaign " & DomainId

            If UrlOPens > 0 Or UrlClics > 0 Then
                lblOpens.Text = "Opens"
                txtOpens.Enabled = True
            Else
                lblOpens.Text = "You cannot ad any opens to this campaign"
                txtOpens.Enabled = False
            End If

            If UrlClics > 0 Then
                lblClicks.Text = "Clicks"
                txtClicks.Enabled = True
            Else
                lblClicks.Text = "You canont ad any clicks to this campaign"
                txtClicks.Enabled = False
            End If

            If UrlOPens > 0 Or UrlClics > 0 Then
                cmdAdd.Enabled = True
            Else
                cmdAdd.Enabled = False
            End If

            lblCampiagnName.Text = CampaignName
            lblDomain.Text = Domain
            lblCount.Text = msg
        End If

    End Sub

    Protected Sub cmdAdd_Click(sender As Object, e As System.EventArgs) Handles cmdAdd.Click
        cmdAdd.Enabled = False
        If txtClicks.Text <> String.Empty Then
            ClickRoutine()
        End If

        If txtOpens.Text <> String.Empty Then
            OpenRoutine()
        End If
    End Sub

    Private Sub OpenRoutine()
        Dim CurrentOpens As Integer
        ExistingOpens = GetCounts(0)
        If ExistingOpens = 0 Then
            CountInCurrentMonth = False
            ExistingOpens = GetCounts(0, True)
        End If
        If ExistingOpens = 0 Then
            lblOpenmsg.Text = "Cannot add any Opens to " & Domain & " campaign " & DomainId & " there are no Opens recorded for the present or last month"
            lblOpenmsg.Font.Bold = True
            Exit Sub
        End If

        If CountInCurrentMonth = True Then
            FinalOpens = ExistingOpens + Integer.Parse(txtOpens.Text)
        Else
            FinalOpens = Integer.Parse(txtOpens.Text)
        End If
        'Response.Write("<br>Existincg Opens  " & ExistingOpens)
        'Response.Write("<br>Final Opens  " & FinalOpens)
        'Response.Write("<br>Table Name " & TableName)

        'New - For reducing clicks
        If Integer.Parse(txtOpens.Text) < 0 Then
            If ExistingOpens + (Integer.Parse(txtOpens.Text)) < 0 Then
                lblOpenmsg.Text = "Cannot subtract opens from " & Domain & " - campaign " & DomainId & ". Cannot have less than 0 opens."
                lblOpenmsg.Font.Bold = True
                cmdAdd.Enabled = True
                Exit Sub
            Else
                DeleteCount(0, Integer.Parse(txtOpens.Text))
            End If
        Else
            InsertCount(0, Integer.Parse(txtOpens.Text))
        End If

        CurrentOpens = GetCounts(0)
        'Response.Write("<br>Current Opens " & CurrentOpens)
        'Response.Write("<br><hr>")
        If CurrentOpens < FinalOpens Then
            While CurrentOpens < FinalOpens
                'Response.Write("<br>Final Opens  " & FinalOpens)
                'Response.Write("<br>Table Name " & TableName)
                Dim Count As Integer = FinalOpens - CurrentOpens
                InsertCount(0, Count)
                CurrentOpens = GetCounts(0)
                'Response.Write("<br>Current Opens  " & CurrentOpens)
                'Response.Write("<br><hr>")
            End While
        End If

        Dim intMonth As Integer
        intMonth = DatePart(DateInterval.Month, Date.Now)
        lblOpenmsg.Text = txtOpens.Text & " Opens have been added to Impression_" & intMonth
    End Sub

    Private Sub ClickRoutine()
        Dim CurrentClicks As Integer
        ExistingClicks = GetCounts(1)
        If ExistingClicks = 0 Then
            CountInCurrentMonth = False
            ExistingClicks = GetCounts(1, True)
        End If
        If ExistingClicks = 0 Then
            lblClickmsg.Text = "Cannot add any clicks to " & Domain & " campaign " & DomainId & " there are no clicks recorded for the present or last month"
            lblClickmsg.Font.Bold = True
            Exit Sub
        End If

        If CountInCurrentMonth = True Then
            FinalClicks = ExistingClicks + Integer.Parse(txtClicks.Text)
        Else
            FinalClicks = Integer.Parse(txtClicks.Text)
        End If
        'Response.Write("<br>Existincg Clicks  " & ExistingClicks)
        'Response.Write("<br>Final Clicks  " & FinalClicks)
        'Response.Write("<br>Table Name " & TableName)

        'New - For reducing clicks
        If Integer.Parse(txtClicks.Text) < 0 Then
            If ExistingClicks + (Integer.Parse(txtClicks.Text)) < 0 Then
                lblClickmsg.Text = "Cannot subtract clicks from " & Domain & " - campaign " & DomainId & ". Cannot have less than 0 clicks."
                lblClickmsg.Font.Bold = True
                cmdAdd.Enabled = True
                Exit Sub
            Else
                DeleteCount(1, Integer.Parse(txtClicks.Text))
            End If
        Else
            InsertCount(1, Integer.Parse(txtClicks.Text))
        End If

        CurrentClicks = GetCounts(1)
        'Response.Write("<br>Current Clicks " & CurrentClicks)
        'Response.Write("<br><hr>")
        If CurrentClicks < FinalClicks Then
            While CurrentClicks < FinalClicks
                'Response.Write("<br>Final Clicks  " & FinalClicks)
                'Response.Write("<br>Table Name " & TableName)
                Dim Count As Integer = FinalClicks - CurrentClicks
                InsertCount(1, Count)
                CurrentClicks = GetCounts(1)
                'Response.Write("<br>Current Clicks  " & CurrentClicks)
                'Response.Write("<br><hr>")
            End While
        End If

        Dim intMonth As Integer
        intMonth = DatePart(DateInterval.Month, Date.Now)
        lblClickmsg.Text = txtClicks.Text & " Clicks have been added to Track_Click_" & intMonth
    End Sub

    Private Function GetCounts(ByVal Type As Integer, Optional ByVal Reset As Boolean = False) As Integer '- 0 = Opens 1 = clicks       
        GetDataBase()       
        Dim rtn As Integer
        Dim Clause As String
        If Reset = False Then
            TableName = GetTableName(Type)
        Else
            TableName = GetTableName(Type, True)
        End If

        If InStr(TableName, "Track") > 0 Then
            Clause = "CampaignId"
        Else
            Clause = "ImpressionCampaignId"
        End If
        If Trim(LCase(Domain)) = "trackingreport.net" Then
            Using cnn As New SqlConnection(strRTConn)
                cnn.Open()
                Using cmdGetCounts As New SqlCommand("sp_DomainReportAdjustment_GetDomainCounts", cnn)
                    cmdGetCounts.CommandType = CommandType.StoredProcedure
                    cmdGetCounts.Parameters.Add(New SqlParameter("@Domain", SqlDbType.VarChar)).Value = Domain
                    cmdGetCounts.Parameters.Add(New SqlParameter("@Table", SqlDbType.VarChar)).Value = TableName
                    cmdGetCounts.Parameters.Add(New SqlParameter("@Clause", SqlDbType.VarChar)).Value = Clause
                    cmdGetCounts.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = DomainId
                    Using dtrGetCounts As SqlDataReader = cmdGetCounts.ExecuteReader
                        While dtrGetCounts.Read
                            rtn = dtrGetCounts("Count")
                        End While
                    End Using
                End Using
            End Using
        Else
            Using cnn As New SqlConnection(strConn)
                cnn.Open()
                Using cmdGetCounts As New SqlCommand("sp_DomainReportAdjustment_GetDomainCounts", cnn)
                    cmdGetCounts.CommandType = CommandType.StoredProcedure
                    cmdGetCounts.Parameters.Add(New SqlParameter("@Domain", SqlDbType.VarChar)).Value = DataBase
                    cmdGetCounts.Parameters.Add(New SqlParameter("@Table", SqlDbType.VarChar)).Value = TableName
                    cmdGetCounts.Parameters.Add(New SqlParameter("@Clause", SqlDbType.VarChar)).Value = Clause
                    cmdGetCounts.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = DomainId
                    Using dtrGetCounts As SqlDataReader = cmdGetCounts.ExecuteReader
                        While dtrGetCounts.Read
                            rtn = dtrGetCounts("Count")
                        End While
                    End Using
                End Using
            End Using
        End If
        Return rtn
    End Function

    Private Function GetTableName(ByVal Type As Integer, Optional ByVal Reset As Boolean = False) As String ' 0 = OPens 1 = Clicks
        Dim intMonth As Integer
        Dim TableName As String
        If Type = 0 Then
            TableName = "Impression_"
        Else
            TableName = "Track_Click_"
        End If

        If Reset = False Then
            intMonth = DatePart(DateInterval.Month, Date.Now)
        Else
            Dim dt As Date = DateAdd(DateInterval.Month, -1, Today())
            intMonth = DatePart(DateInterval.Month, dt)
        End If

        TableName = TableName & intMonth

        Return TableName

    End Function

    Private Sub InsertCount(ByVal Type As Integer, ByVal Count As Integer) ' 0=Opens 1 = Clicks
        Dim Clause As String
        Dim intMonth As Integer
        Dim InsertTable As String
        intMonth = DatePart(DateInterval.Month, Date.Now)
        If Type = 0 Then
            InsertTable = "Impression_" & intMonth
            Clause = "ImpressionCampaignId"
        Else
            InsertTable = "Track_Click_" & intMonth
            Clause = "CampaignId"
        End If

        If Trim(LCase(Domain)) = "trackingreport.net" Then
            Using cnn As New SqlConnection(strRTConn)
                cnn.Open()
                Using cmdInsertClicks As New SqlCommand("sp_DomainReportAdjustment_AdjustCounts", cnn)
                    cmdInsertClicks.CommandType = CommandType.StoredProcedure
                    cmdInsertClicks.Parameters.Add(New SqlParameter("@InsertTable", SqlDbType.VarChar)).Value = InsertTable
                    cmdInsertClicks.Parameters.Add(New SqlParameter("@Count", SqlDbType.Int)).Value = Count
                    cmdInsertClicks.Parameters.Add(New SqlParameter("@PullTable", SqlDbType.VarChar)).Value = TableName
                    cmdInsertClicks.Parameters.Add(New SqlParameter("@Clause", SqlDbType.VarChar)).Value = Clause
                    cmdInsertClicks.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = DomainId
                    cmdInsertClicks.ExecuteNonQuery()
                End Using
            End Using
        Else
            Using cnn As New SqlConnection(strConn)
                cnn.Open()
                Using cmdInsertClicks As New SqlCommand("sp_DomainReportAdjustment_AdjustCounts", cnn)
                    cmdInsertClicks.CommandType = CommandType.StoredProcedure
                    cmdInsertClicks.Parameters.Add(New SqlParameter("@Domain", SqlDbType.VarChar)).Value = DataBase
                    cmdInsertClicks.Parameters.Add(New SqlParameter("@InsertTable", SqlDbType.VarChar)).Value = InsertTable
                    cmdInsertClicks.Parameters.Add(New SqlParameter("@Count", SqlDbType.Int)).Value = Count
                    cmdInsertClicks.Parameters.Add(New SqlParameter("@PullTable", SqlDbType.VarChar)).Value = TableName
                    cmdInsertClicks.Parameters.Add(New SqlParameter("@Clause", SqlDbType.VarChar)).Value = Clause
                    cmdInsertClicks.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = DomainId
                    cmdInsertClicks.ExecuteNonQuery()
                End Using
            End Using
        End If
    End Sub

    Private Sub DeleteCount(ByVal Type As Integer, ByVal Count As Integer) ' 0=Opens 1 = Clicks
        Dim Clause As String
        Dim intMonth As Integer
        Dim DeleteTable As String

        intMonth = DatePart(DateInterval.Month, Date.Now)
        If Type = 0 Then
            DeleteTable = "Impression_" & intMonth
            Clause = "ImpressionCampaignId"
        Else
            DeleteTable = "Track_Click_" & intMonth
            Clause = "CampaignId"
        End If

        If Trim(LCase(Domain)) = "trackingreport.net" Then
            Using cnn As New SqlConnection(strRTConn)
                cnn.Open()
                Using cmdInsertClicks As New SqlCommand("sp_DomainReportAdjustment_DeleteCounts", cnn)
                    cmdInsertClicks.CommandType = CommandType.StoredProcedure
                    cmdInsertClicks.Parameters.Add(New SqlParameter("@DeleteTable", SqlDbType.VarChar)).Value = DeleteTable
                    cmdInsertClicks.Parameters.Add(New SqlParameter("@Count", SqlDbType.Int)).Value = (Count * -1) 'Convert count to positive number
                    cmdInsertClicks.Parameters.Add(New SqlParameter("@PullTable", SqlDbType.VarChar)).Value = TableName
                    cmdInsertClicks.Parameters.Add(New SqlParameter("@Clause", SqlDbType.VarChar)).Value = Clause
                    cmdInsertClicks.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = DomainId
                    cmdInsertClicks.ExecuteNonQuery()
                End Using
            End Using
        Else
            Using cnn As New SqlConnection(strConn)
                cnn.Open()
                Using cmdInsertClicks As New SqlCommand("sp_DomainReportAdjustment_DeleteCounts", cnn)
                    cmdInsertClicks.CommandType = CommandType.StoredProcedure
                    cmdInsertClicks.Parameters.Add(New SqlParameter("@Domain", SqlDbType.VarChar)).Value = DataBase
                    cmdInsertClicks.Parameters.Add(New SqlParameter("@DeleteTable", SqlDbType.VarChar)).Value = DeleteTable
                    cmdInsertClicks.Parameters.Add(New SqlParameter("@Count", SqlDbType.Int)).Value = (Count * -1) 'Convert count to positive number
                    cmdInsertClicks.Parameters.Add(New SqlParameter("@PullTable", SqlDbType.VarChar)).Value = TableName
                    cmdInsertClicks.Parameters.Add(New SqlParameter("@Clause", SqlDbType.VarChar)).Value = Clause
                    cmdInsertClicks.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = DomainId
                    cmdInsertClicks.ExecuteNonQuery()
                End Using
            End Using
        End If
    End Sub

    Private Sub GetDataBase()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetDataBase As New SqlCommand("ClientCompany_GetByDomain", cnn)
                cmdGetDataBase.CommandType = CommandType.StoredProcedure
                cmdGetDataBase.Parameters.Add(New SqlParameter("@Domain", SqlDbType.VarChar)).Value = Domain
                Using dtrGetDataBase As SqlDataReader = cmdGetDataBase.ExecuteReader
                    If dtrGetDataBase.HasRows Then
                        While dtrGetDataBase.Read
                            DataBase = dtrGetDataBase("DataBase")
                        End While
                    End If
                End Using
            End Using
        End Using


    End Sub

End Class
