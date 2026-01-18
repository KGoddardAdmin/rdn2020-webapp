Imports System.Data
Imports System.Data.SqlClient

Partial Class CampaignListSetLinkPercentage
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Private AId As Integer = 883840
    Private myToken As String = "0bcda1e349711040d213e98d1da71479"
    Public Converted As String
    Private CampaignId As Integer
    Private TotalLinksChecked As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblmsg.Text = String.Empty
        lblmsg.Font.Bold = False
        lblmsg.ForeColor = Drawing.Color.Black
        CheckSessionVariable()
        If Not IsPostBack Then
            GetCreative()
            MarkAsSelected()
            GetCheckedLinks()
            SetPercent()

        End If
    End Sub

    Private Sub CheckSessionVariable()
        If Session("CampaignSetPercentage") IsNot Nothing Then
            Dim arrseed() As String = New String() {}
            arrseed = Session("CampaignSetPercentage").Split("~")
            CampaignId = arrseed(0)
        Else
            Response.Redirect("campaignlist.aspx")
        End If
    End Sub

    Protected Sub cmdConnect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdConnect.Click
        If CheckPercent() = True Then
            For index As Integer = 0 To gridLinks.Rows.Count - 1
                'Programmatically access the CheckBox from the TemplateField
                Dim cb As CheckBox = CType(gridLinks.Rows(index).FindControl("RowLevelCheckBox"), CheckBox)
                'If cb.Checked Then
                'Dim category As Integer
                Dim row As GridViewRow = gridLinks.Rows(index)
                'Dim lstCategory As DropDownList = DirectCast(row.FindControl("ddlinks"), DropDownList)
                'category = Integer.Parse(lstCategory.SelectedValue.ToString())
                Dim txtpercent As TextBox = DirectCast(row.FindControl("txtpercent"), TextBox)
                Dim Percent As String
                If txtpercent.Text = String.Empty Then
                    Percent = 0
                Else
                    Percent = txtpercent.Text
                End If

                Dim txtclicks As TextBox = DirectCast(row.FindControl("txtclicks"), TextBox)
                Dim Clicks As String
                If txtclicks.Text = String.Empty Then
                    Clicks = 0
                Else
                    Clicks = txtclicks.Text
                End If

                'InsertLinks(cb.Text, category)
                SetPercent(Percent, cb.Text, Clicks)
                'End If
            Next
            lblmsg.Text = "Campaign Link Percentages Set For Campaign Id " & CampaignId & " .!!"
            lblmsg.Font.Bold = True
            EndSession()
            UpdateStatus(CampaignId)
        End If
        ReLoadCreative()

    End Sub

    Protected Sub cmdReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        ClearPercentText()
        SetPercent()
        ReLoadCreative()
    End Sub

    Private Function CheckPercent() As Boolean
        Dim TotalPercent As Integer
        Dim percentck As Boolean = False
        For index As Integer = 0 To gridLinks.Rows.Count - 1
            Dim cb As CheckBox = CType(gridLinks.Rows(index).FindControl("RowLevelCheckBox"), CheckBox)
            If cb.Checked Then
                Dim row As GridViewRow = gridLinks.Rows(index)
                Dim txtpercent As TextBox = DirectCast(row.FindControl("txtpercent"), TextBox)
                Dim Percent As String
                If txtpercent.Text = String.Empty Then
                    Percent = 0
                Else
                    Percent = txtpercent.Text
                End If
                TotalPercent = TotalPercent + percent
            End If
        Next

        Select Case TotalPercent
            Case Is < 100
                lblmsg.Text = "Your percentages do not add up to 100% Your current Percentage is " & TotalPercent
                lblmsg.Font.Bold = True
                lblmsg.ForeColor = Drawing.Color.Red
            Case 100
                percentck = True
            Case Is > 100
                lblmsg.Text = "Your percentages are over 100%, Your current Percentage is " & TotalPercent
                lblmsg.Font.Bold = True
                lblmsg.ForeColor = Drawing.Color.Red
        End Select

        Return percentck
    End Function

    Private Sub ReLoadCreative()
        Dim creative As String = String.Empty
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetCreative As New SqlCommand("CampaignAdCopy_GetCreative", cnn)
                cmdGetCreative.CommandType = CommandType.StoredProcedure
                cmdGetCreative.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = CampaignId
                Using dtrGetCreative As SqlDataReader = cmdGetCreative.ExecuteReader
                    While dtrGetCreative.Read
                        creative = dtrGetCreative("ConvertedCreative")
                    End While
                End Using
            End Using
        End Using

        Creative = Replace(Creative, "<a", "<a style="" border:2px solid rgb(255,100,18);"" ", 1, -1, CompareMethod.Text)
        Converted = Creative
    End Sub

    Private Sub GetCreative()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetCreative As New SqlCommand("CampaignAdCopy_GetCreative", cnn)
                cmdGetCreative.CommandType = CommandType.StoredProcedure
                cmdGetCreative.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = CampaignId
                Using dtrGetCreative As SqlDataReader = cmdGetCreative.ExecuteReader
                    While dtrGetCreative.Read
                        If Not IsDBNull(dtrGetCreative("ConvertedCreative")) Then
                            DisplayCreative(dtrGetCreative("ConvertedCreative"))
                        Else
                            lblmsg.Text = "Problem retrieving creative using campaign id " & CampaignId
                        End If
                    End While
                End Using
            End Using
        End Using
    End Sub

    Private Sub DisplayCreative(ByVal Creative As String)
        Creative = Replace(Creative, "<a ", "<a style="" border:2px solid rgb(255,100,18);"" ", 1, -1, CompareMethod.Text)
        Converted = Creative
        GetLinks()
    End Sub

    Public Sub GetLinks()
        'Dim dslinks As DataSet
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetLinks As New SqlCommand("Campaign_GetByCampaignId", cnn)
                cmdGetLinks.CommandType = CommandType.StoredProcedure
                cmdGetLinks.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = CampaignId
                Using dtrGetLinks As SqlDataReader = cmdGetLinks.ExecuteReader
                    gridLinks.DataSource = dtrGetLinks
                    gridLinks.DataBind()
                End Using
            End Using
        End Using
    End Sub

    Private Sub MarkAsSelected()

        For index As Integer = 0 To gridlinks.rows.count - 1
            Dim cb As CheckBox = CType(gridLinks.Rows(index).FindControl("rowlevelcheckbox"), CheckBox)
            cb.Checked = True
        Next
    End Sub

    Private Sub GetCheckedLinks()
        Dim percentck As Boolean = False
        For index As Integer = 0 To gridLinks.Rows.Count - 1
            Dim cb As CheckBox = CType(gridLinks.Rows(index).FindControl("RowLevelCheckBox"), CheckBox)
            If cb.Checked Then
                TotalLinksChecked += 1
            End If
        Next
    End Sub

    Private Sub ClearPercentText()
        For index As Integer = 0 To gridLinks.Rows.Count - 1
            Dim row As GridViewRow = gridLinks.Rows(index)
            Dim txtpercent As TextBox = DirectCast(row.FindControl("txtpercent"), TextBox)
            txtpercent.Text = String.Empty
        Next
    End Sub


    Private Sub SetPercent()

        Dim RandomClass As New random()
        Dim RandomNumber As Integer
        Dim TotalPercent As Integer
        Dim MaxValue As Integer
        Dim TotalLinks As Integer = gridLinks.Rows.Count
        Dim LinksLeft As Integer
        Dim Counter As Integer
        MaxValue = 100 - TotalLinks
        Dim Checkedlinks As Integer
        Dim MaxedOut As Boolean = False

        For index As Integer = 0 To gridlinks.rows.count - 1
            Dim cb As CheckBox = CType(gridLinks.Rows(index).FindControl("RowLevelCheckBox"), CheckBox)
            If cb.Checked = True Then
                If MaxValue > 1 Then
                    RandomNumber = RandomClass.Next(1, MaxValue)
                    Dim row As GridViewRow = gridLinks.Rows(index)
                    Dim txtpercent As TextBox = DirectCast(row.FindControl("txtpercent"), TextBox)
                    txtpercent.Text = RandomNumber
                    TotalPercent += RandomNumber
                    MaxValue -= RandomNumber
                Else
                    Exit For
                    MaxedOut = True
                End If
            End If
            LinksLeft = gridLinks.Rows.Count - 1 - index
            Counter = index
        Next

        For index As Integer = Counter + 1 To gridLinks.Rows.Count - 1
            Dim cb As CheckBox = CType(gridLinks.Rows(index).FindControl("RowLevelCheckBox"), CheckBox)
            If cb.Checked = True Then
                Checkedlinks += 1
            End If
        Next
        Checkedlinks -= 1
        For index As Integer = Counter + 1 To gridLinks.Rows.Count - 1
            Dim cb As CheckBox = CType(gridLinks.Rows(index).FindControl("RowLevelCheckBox"), CheckBox)
            If cb.Checked = True Then
                RandomNumber = (100 - TotalPercent) - Checkedlinks
                Dim row As GridViewRow = gridLinks.Rows(index)
                Dim txtpercent As TextBox = DirectCast(row.FindControl("txtpercent"), TextBox)
                txtpercent.Text = RandomNumber
            End If
            TotalPercent += RandomNumber
            MaxValue -= RandomNumber
            Checkedlinks -= 1
        Next
        If TotalPercent < 100 Then
            If MaxedOut = True Then
                For index As Integer = Counter + 1 To gridLinks.Rows.Count - 1
                    Dim cb As CheckBox = CType(gridLinks.Rows(index).FindControl("RowLevelCheckBox"), CheckBox)
                    If cb.Checked = True And TotalPercent < 100 Then
                        Dim row As GridViewRow = gridLinks.Rows(index)
                        Dim txtpercent As TextBox = DirectCast(row.FindControl("txtpercent"), TextBox)
                        RandomNumber = (100 - TotalPercent) + txtpercent.Text
                        txtpercent.Text = RandomNumber
                        TotalPercent += RandomNumber
                    End If

                    If TotalPercent = 100 Then
                        Exit For
                    End If
                Next
            Else
                For index As Integer = 0 To gridLinks.Rows.Count - 1
                    Dim cb As CheckBox = CType(gridLinks.Rows(index).FindControl("RowLevelCheckBox"), CheckBox)
                    If cb.Checked = True And TotalPercent < 100 Then
                        Dim row As GridViewRow = gridLinks.Rows(index)
                        Dim txtpercent As TextBox = DirectCast(row.FindControl("txtpercent"), TextBox)
                        RandomNumber = (100 - TotalPercent) + txtpercent.Text
                        txtpercent.Text = RandomNumber
                        TotalPercent += RandomNumber
                    End If
                    If TotalPercent = 100 Then
                        Exit For
                    End If
                Next
            End If
        End If
        For index As Integer = 0 To gridLinks.Rows.Count - 1
            Dim row As GridViewRow = gridLinks.Rows(index)
            Dim txtClicks As TextBox = DirectCast(row.FindControl("txtClicks"), TextBox)
            txtClicks.Text = 0
        Next
    End Sub



    Public Function PopulateCategory() As DataSet
        Dim dsLink As New DataSet
        Using cnn As New SqlConnection(strConn)
            Using cmdPopulateCategory As New SqlCommand("Ezanga_Category_Get", cnn)
                cmdPopulateCategory.CommandType = CommandType.StoredProcedure
                Using dadPopulateCategory As New SqlDataAdapter(cmdPopulateCategory)
                    dadPopulateCategory.Fill(dsLink)
                    PopulateCategory = dsLink
                End Using
            End Using
        End Using
    End Function

    Public Function GetId(ByVal Id As Integer) As Integer
        Return ID
    End Function

    Private Sub InsertLinks(ByVal lid As Integer, ByVal catid As Integer)
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdInsertlinks As New SqlCommand("Ezanga_Link_New", cnn)
                cmdInsertlinks.CommandType = CommandType.StoredProcedure
                'cmdInsertlinks.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = CampaignId
                'cmdInsertlinks.Parameters.Add(New SqlParameter("@EzangaCampaignid", SqlDbType.Int)).Value = ECampaignId
                cmdInsertlinks.Parameters.Add(New SqlParameter("@LinkId", SqlDbType.Int)).Value = lid
                cmdInsertlinks.Parameters.Add(New SqlParameter("@CategoryId", SqlDbType.Int)).Value = catid
                cmdInsertlinks.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Sub SetPercent(ByVal percent As Integer, ByVal id As Integer, ByVal clicks As Integer)
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdSetPercent As New SqlCommand("Campaign_SetLinkPercentage", cnn)
                cmdSetPercent.CommandType = CommandType.StoredProcedure
                cmdSetPercent.Parameters.Add(New SqlParameter("@LinkPercent", SqlDbType.Int)).Value = percent
                cmdSetPercent.Parameters.Add(New SqlParameter("@LinkId", SqlDbType.Int)).Value = id
                cmdSetPercent.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = CampaignId
                cmdSetPercent.Parameters.Add(New SqlParameter("@ClickCount", SqlDbType.Int)).Value = clicks
                cmdSetPercent.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Sub UpdateStatus(ByVal Id As Integer)
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdUpdateStatus As New SqlCommand("CampaignAdCopy_UpdateStatus", cnn)
                cmdUpdateStatus.CommandType = CommandType.StoredProcedure
                cmdUpdateStatus.Parameters.Add(New SqlParameter("@Status", SqlDbType.Int)).Value = 4
                cmdUpdateStatus.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = Id
                cmdUpdateStatus.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Sub EndSession()
        If Session("CampaignSetPercentage") IsNot Nothing Then
            Session.Remove("CampaignSetPercentage")
        End If
    End Sub

End Class
