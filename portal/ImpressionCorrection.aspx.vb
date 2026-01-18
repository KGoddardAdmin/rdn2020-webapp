Imports System.Data
Imports System.Data.SqlClient

Partial Class ImpressionCorrection
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Public ShowForm As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblmsg.Text = String.Empty
        lblmsg.Font.Bold = False
        lblmsg.ForeColor = Drawing.Color.Black
    End Sub

    Protected Sub cmdGetCount_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGetCount.Click
        If Len(Trim(txtOLdId.Text)) > 0 Then
            GetCount()
            trOldId.Visible = False
            ShowForm = True
        Else
            lblmsg.Text = "You must enter the ImpressionCampiagnId you are wanting to clone."
            lblmsg.Font.Bold = True
            lblmsg.ForeColor = Drawing.Color.Red
        End If
    End Sub

    Protected Sub cmdClone_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClone.Click        
        If CInt(lblCount.Text) < CInt(txtCount.Text) Then
            lblmsg.Text = "You cannot create that many records."
            lblmsg.Font.Bold = True
            lblmsg.ForeColor = Drawing.Color.Red
        ElseIf Len(Trim(txtId.Text)) = 0 Or Trim(txtId.Text) = String.Empty Then
            lblmsg.Text = "You cannot create that many records."
            lblmsg.Font.Bold = True
            lblmsg.ForeColor = Drawing.Color.Red
        Else
            GetRecords()
            ShowForm = True
        End If
    End Sub

    Private Sub GetRecords()
        Dim sqlGetRecords As String
        Dim intMonth As Integer        
        Dim Table_Name As String
        intMonth = DatePart(DateInterval.Month, Date.Now)
        Table_Name = "Impression_" & intMonth

        sqlGetRecords = "SELECT Top @Count * " & _
                        "FROM @Table_Name " & _
                        "WHERE ImpressionCampaignId = @ImpressionCampaignId " & _
                        "ORDER BY  dateviewed DESC"

        sqlGetRecords = sqlGetRecords.Replace("@Count", Trim(txtCount.Text))
        sqlGetRecords = sqlGetRecords.Replace("@Table_Name", Table_Name)
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetRecords As New SqlCommand(sqlGetRecords, cnn)
                cmdGetRecords.Parameters.Add(New SqlParameter("@ImpressionCampaignId", SqlDbType.Int)).Value = Trim(txtOLdId.Text)
                Using dtrGetRecords As SqlDataReader = cmdGetRecords.ExecuteReader
                    While dtrGetRecords.Read
                        InsertRecord(dtrGetRecords("Ip"), dtrGetRecords("DateViewed"))
                    End While
                End Using
            End Using
        End Using
        lblmsg.Text = "Records have SUCCESSFULY been added to " & Table_Name
        trOldId.Visible = False
        ShowForm = False

    End Sub

    Private Sub InsertRecord(ByVal Ip As String, ByVal dateviewed As Date)
        Dim sqlInsertRecord As String
        Dim intMonth As Integer
        Dim Table_Name As String
        intMonth = DatePart(DateInterval.Month, Date.Now)
        Table_Name = "Impression_" & intMonth

        sqlInsertRecord = "INSERT INTO @Table_Name(ImpressionCampaignId, Ip, DateViewed) " & _
                        "VALUES(@ImpressionCampaignId, '@Ip', '@DateViewed')"

        sqlInsertRecord = sqlInsertRecord.Replace("@Table_Name", Table_Name)
        sqlInsertRecord = sqlInsertRecord.Replace("@ImpressionCampaignId", Trim(txtId.Text))
        sqlInsertRecord = sqlInsertRecord.Replace("@Ip", Ip)
        sqlInsertRecord = sqlInsertRecord.Replace("@DateViewed", dateviewed)

        Using cn As New SqlConnection(strConn)
            cn.Open()
            Using cmdInsertRecord As New SqlCommand(sqlInsertRecord, cn)
                cmdInsertRecord.ExecuteNonQuery()
            End Using
        End Using
    End Sub


    Private Sub GetCount()
        Dim intMonth As Integer
        Dim sqlGetCount As String
        Dim Table_Name As String
        intMonth = DatePart(DateInterval.Month, Date.Now)
        Table_Name = "Impression_" & intMonth
        sqlGetCount = "SELECT Count(*) " & _
                   "FROM @Table_Name " & _
                  "WHERE ImpressionCampaignId = @ImpressionCampaignId"

        sqlGetCount = sqlGetCount.Replace("@Table_Name", Table_Name)
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetCount As New SqlCommand(sqlGetCount, cnn)
                cmdGetCount.Parameters.Add(New SqlParameter("@ImpressionCampaignId", SqlDbType.Int)).Value = Trim(txtOLdId.Text)
                lblCount.Text = cmdGetCount.ExecuteScalar
            End Using
        End Using
    End Sub

End Class
