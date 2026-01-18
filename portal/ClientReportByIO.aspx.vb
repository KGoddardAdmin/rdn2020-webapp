Imports System.Data
Imports System.Data.SqlClient

Partial Class ClientReportByIO
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")    
    Private Client As String
    Private IO As String
    Private IOId As String
    Private StartDate As Date
    Private IOType As String
    Private TotalClicks As Integer
    Private TotalOpens As Integer
    Public SelectedMonth As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Client = Request.QueryString("Client")
        IO = Request.QueryString("IO")
        StartDate = Request.QueryString("Start")
        IOId = Request.QueryString("IOId")
        IOType = Request.QueryString("TypeName")
        lblClient.Text = Client
        lblIO.Text = IOId
        lblTotal.Text = String.Empty
        txtTotal.Text = String.Empty

        If Not IsPostBack Then
            ddMonth.SelectedValue = DatePart(DateInterval.Month, StartDate)            
            If IOType = "Click" Then
                trclick.Visible = True
                trimpression.Visible = False
                GetSameMonthUniqueReportData()
            Else
                trclick.Visible = False
                trimpression.Visible = True
                GetSameMonthUniqueViews()
            End If

        End If
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        If IOType = "Click" Then
            trclick.Visible = True
            trimpression.Visible = False
            GetSameMonthUniqueReportData()
        Else
            trclick.Visible = False
            trimpression.Visible = True
            GetSameMonthUniqueViews()
        End If
    End Sub


    Private Sub GetSameMonthUniqueReportData()
        SelectedMonth = ddMonth.SelectedValue
        Dim Table_Name As String
        Table_Name = "Track_Click_" & ddMonth.SelectedValue
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetReport As New SqlCommand("Track_Click_ReportUniqueClicks_ByIO", cnn)
                cmdGetReport.CommandType = CommandType.StoredProcedure
                cmdGetReport.CommandTimeout = 180
                cmdGetReport.Parameters.Add(New SqlParameter("@Table_Name", SqlDbType.NVarChar, 14)).Value = Table_Name
                cmdGetReport.Parameters.Add(New SqlParameter("@IO", SqlDbType.UniqueIdentifier)).Value = SqlTypes.SqlGuid.Parse(IO)
                Using dtrGetReport As SqlDataReader = cmdGetReport.ExecuteReader
                    gridReport.DataSource = dtrGetReport
                    gridReport.DataBind()
                End Using
            End Using
        End Using
        lblTotal.Text = "Total Clicks:"
        lblTotal.Font.Bold = True
        txtTotal.Text = TotalClicks
    End Sub

    Private Sub GetSameMonthUniqueViews()
        SelectedMonth = ddMonth.SelectedValue
        Dim Table_Name As String
        Table_Name = "Impression_" & ddMonth.SelectedValue
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetReport As New SqlCommand("Impression_ReportUniqueViews_ByIO", cnn)
                cmdGetReport.CommandType = CommandType.StoredProcedure
                cmdGetReport.Parameters.Add(New SqlParameter("@Table_Name", SqlDbType.NVarChar, 14)).Value = Table_Name
                cmdGetReport.Parameters.Add(New SqlParameter("@IO", SqlDbType.UniqueIdentifier)).Value = SqlTypes.SqlGuid.Parse(IO)
                Using dtrGetReport As SqlDataReader = cmdGetReport.ExecuteReader
                    gridImpressions.DataSource = dtrGetReport
                    gridImpressions.DataBind()
                End Using
            End Using
        End Using
        lblTotal.Text = "Total Opens:"
        lblTotal.Font.Bold = True
        txtTotal.Text = TotalOpens

    End Sub

    Public Function GetTotalClicks(ByVal Clicks As Integer) As Integer
        TotalClicks = TotalClicks + Clicks
        Return TotalClicks
    End Function

    Public Function GetTotalOpens(ByVal Opens As Integer) As Integer
        TotalOpens = TotalOpens + Opens
        Return TotalOpens
    End Function

    Public Function FormatLink(ByVal Id As Integer, ByVal Name As String)
        Dim link As String
        link = "<a href=""ClientReportByLink.aspx?c=" & Id & "&M=" & ddMonth.SelectedValue & ">" & Name & "</a>"
        Return link

    End Function

    Public Sub GetReport(ByVal src As Object, ByVal e As GridViewCommandEventArgs)
        'Dim dr As DataRow = Cart.NewRow()

        ' get the row index stored in the CommandArgument property 
        Dim index As Integer = Convert.ToInt32(e.CommandArgument)

        ' get the GridViewRow where the command is raised 
        Dim selectedRow As GridViewRow = DirectCast(e.CommandSource, GridView).Rows(index)

        ' for bound fields, values are stored in the Text property of Cells [ fieldIndex ] 
        Dim Month As Integer = ddMonth.SelectedValue
        Dim CampaignId As String = selectedRow.Cells(2).Text

        If e.CommandName = "GetId" Then
            Dim RedURL As String
            RedURL = "ClientReportByLink.aspx?c=@CampaignId&Month=@Month"
            RedURL = RedURL.Replace("@CampaignId", CampaignId)
            RedURL = RedURL.Replace("@Month", Month)
            Response.Redirect(RedURL)
        End If
    End Sub
End Class
