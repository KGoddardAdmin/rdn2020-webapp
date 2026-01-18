Imports System.Data
Imports System.Data.SqlClient

Partial Class ReportDailClick
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Private TotalClicks As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblTotal.Text = String.Empty
        TotalClicks = 0
        If Not IsPostBack Then
            ddMonth.SelectedValue = Date.Today.Month
            GetDailyUniqueClicks()
        End If
    End Sub

    Protected Sub cmdGetReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGetReport.Click
        lblTotal.Text = String.Empty
        TotalClicks = 0
        GetDailyUniqueClicks()
    End Sub

    Private Sub GetDailyUniqueClicks()
        Dim Table_Name As String
        Table_Name = "Track_Click_" & ddMonth.SelectedValue
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetReport As New SqlCommand("Track_Click_ReportDailyUniqueClicksTotal", cnn)
                cmdGetReport.CommandType = CommandType.StoredProcedure
                cmdGetReport.Parameters.Add(New SqlParameter("@Table_Name", SqlDbType.NVarChar, 14)).Value = Table_Name
                Using dtrGetReport As SqlDataReader = cmdGetReport.ExecuteReader
                    While dtrGetReport.Read
                        GetTotalClicks(dtrGetReport("Ips"))
                    End While
                End Using
            End Using
        End Using
        lblTotal.Text = TotalClicks
    End Sub


    Public Function GetTotalClicks(ByVal Clicks As Integer) As Integer
        TotalClicks = TotalClicks + Clicks
        Return TotalClicks
    End Function

   
End Class
