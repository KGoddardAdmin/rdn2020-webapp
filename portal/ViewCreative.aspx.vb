Imports System.Data
Imports System.Data.SqlClient

Partial Class ViewCreative
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Private InValidLoginRedirectURL As String = ConfigurationSettings.AppSettings("InvalidLoginURL")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        txtCreative.Text = String.Empty
        If Not IsPostBack Then
            GetActiveCreatives()
        End If

    End Sub

    Protected Sub cmdGetCreative_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGetCreative.Click

        If Page.IsValid Then
            GetCreative()
        End If

    End Sub

    Private Sub GetCreative()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetCreative As New SqlCommand("CampaignCreative_GetCreative", cnn)
                cmdGetCreative.CommandType = CommandType.StoredProcedure
                cmdGetCreative.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = ddCreative.SelectedValue
                txtCreative.Text = cmdGetCreative.ExecuteScalar
            End Using
        End Using
    End Sub


    Private Sub GetActiveCreatives()
        Dim dstGetActiveCreatives As DataSet
        Dim dadGetActiveCreatives As SqlDataAdapter
        Dim cnn As New SqlConnection(strConn)
        dstGetActiveCreatives = New DataSet
        dadGetActiveCreatives = New SqlDataAdapter("CampaignCreative_GetActive", cnn)
        dadGetActiveCreatives.Fill(dstGetActiveCreatives, "Creatives")
        If dstGetActiveCreatives.Tables("Creatives").Rows.Count = 0 Then
            txtCreative.Text = "There are no active creatives."
            txtCreative.Font.Bold = True
        Else
            Dim Dyncolumn As New DataColumn
            With Dyncolumn
                .ColumnName = "Name"
                .DataType = System.Type.GetType("System.String")
                .Expression = "CampaignId+'  '+CreatedOn"
            End With
            dstGetActiveCreatives.Tables("Creatives").Columns.Add(Dyncolumn)
            ddCreative.DataTextField = "Name"
            ddCreative.DataValueField = "CampaignId"
            ddCreative.DataSource = dstGetActiveCreatives.Tables("Creatives").DefaultView
            ddCreative.DataBind()
        End If
        ddCreative.Items.Insert(0, "Select Creative")
    End Sub

End Class
