Imports System.Data
Imports System.Data.SqlClient

Partial Class CampaignList
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Public ViewCreative As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

        End If      
    End Sub



#Region "Grid"

    Protected Sub SetgridCampaignBGColor(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Select Case (e.Row.DataItem("Status"))
                Case 1
                    e.Row.BackColor = Drawing.Color.Yellow
                    Dim myButton As Button = Nothing
                    myButton = DirectCast(e.Row.Cells(0).Controls(0), Button)

                    myButton.Text = "Set %"
                Case 2
                    e.Row.BackColor = Drawing.Color.Yellow
                    Dim myButton As Button = Nothing
                    myButton = DirectCast(e.Row.Cells(0).Controls(0), Button)
                    myButton.Text = "Set %"

                Case 4
                    e.Row.BackColor = Drawing.Color.Yellow
                    Dim myButton As Button = Nothing
                    myButton = DirectCast(e.Row.Cells(0).Controls(0), Button)
                    myButton.Text = "Approve" '"Send For Fullfillment" 
                Case 5
                    e.Row.BackColor = Drawing.Color.Green
                    Dim myButton As Button = Nothing
                    myButton = DirectCast(e.Row.Cells(0).Controls(0), Button)
                    myButton.Text = "Fullfill" '"Send For Fullfillment" 

                Case 6
                    e.Row.BackColor = Drawing.Color.Gold

                Case 9
                    e.Row.BackColor = Drawing.Color.Red
            End Select
        End If
        
    End Sub

    Public Sub ManageList(ByVal src As Object, ByVal e As GridViewCommandEventArgs)

        ' get the row index stored in the CommandArgument property 
        Dim index As Integer = Convert.ToInt32(e.CommandArgument)

        ' get the GridViewRow where the command is raised 
        Dim selectedRow As GridViewRow = DirectCast(e.CommandSource, GridView).Rows(index)
        Dim CVariable As String
        Dim CampaignId As Integer = gridCampaigns.DataKeys(index).Values("CampaignId")
        Dim Status As Integer = gridCampaigns.DataKeys(index).Values("Status")



        ' for bound fields, values are stored in the Text property of Cells [ fieldIndex ] 
        If e.CommandName = "ManageList" Then

            If Not IsDBNull(gridCampaigns.DataKeys(index).Values("CouponVariable")) Then
                CVariable = gridCampaigns.DataKeys(index).Values("CouponVariable")
            Else
                CVariable = String.Empty
            End If

            Select Case gridCampaigns.DataKeys(index).Values("Status")
                'Case 1
                '   If CVariable <> String.Empty Then
                '      Session("CampaignSeed") = CampaignId & "~" & Status & "~" & CVariable
                ' Else
                '    Session("CampiagnSeed") = CampaignId & "~" & Status
                'End If
                '   Response.Redirect("CampaignListSeed.aspx")
                Case Is < 3
                    If CVariable <> String.Empty Then
                        Session("CampaignSetPercentage") = CampaignId & "~" & Status & "~" & CVariable
                    Else
                        Session("CampaignSetPercentage") = CampaignId & "~" & Status
                    End If
                    Response.Redirect("CampaignListSetLinkPercentage.aspx")
                Case 4
                    UpdateStatus(CampaignId)
                    Response.Redirect("CampaignList.aspx")                    
                    'GetCampaigns()
                Case 5
                    If CVariable <> String.Empty Then
                        Session("CampaignFullfill") = CampaignId & "~" & Status & "~" & CVariable
                    Else
                        Session("CampaignFullfill") = CampaignId & "~" & Status
                    End If
                    Response.Redirect("CampaignListFullfill.aspx")
            End Select
        End If

        If e.CommandName = "SendSeed" Then            
            If Not IsDBNull(gridCampaigns.DataKeys(index).Values("CouponVariable")) Then
                CVariable = gridCampaigns.DataKeys(index).Values("CouponVariable")
            Else
                CVariable = String.Empty
            End If

            If CVariable <> String.Empty Then
                Session("CampaignSeed") = CampaignId & "~" & Status & "~" & CVariable
            Else
                Session("CampaignSeed") = CampaignId & "~" & Status
            End If
            Response.Redirect("CampaignListSeed.aspx")
        End If
    End Sub

    Public Function FormatStartDate(ByVal Start As Date) As String
        Dim NewDate As Date = Start
        NewDate = FormatDateTime(NewDate, DateFormat.ShortDate)
        Return NewDate
    End Function

    Private Sub UpdateStatus(ByVal Id As Integer)
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdUpdateStatus As New SqlCommand("CampaignAdCopy_UpdateStatus", cnn)
                cmdUpdateStatus.CommandType = CommandType.StoredProcedure
                cmdUpdateStatus.Parameters.Add(New SqlParameter("@Status", SqlDbType.Int)).Value = 5
                cmdUpdateStatus.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = Id
                cmdUpdateStatus.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Function FormatViewCreativeLink(ByVal id As Integer) As String
        ' get the row index stored in the CommandArgument property 

        Dim link As String

        link = "<a href=""CampaignListViewCreative.aspx?CampaignId=""" & id & ">"
        Return link

    End Function

    Public Function FormatId(ByVal id As Integer) As String
        Dim cid As String
        cid = id.ToString
        Return cid

    End Function

#End Region

End Class
