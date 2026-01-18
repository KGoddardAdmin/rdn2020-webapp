Imports System.Data
Imports System.Data.SqlClient

Partial Class SalesRepNew
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            txtFName.Focus()
            GetStateDropDown()
        End If
    End Sub

    Protected Sub cmdAddSalesRep_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddSalesRep.Click
        Page.Validate("frmSalesRep")
        If Page.IsValid Then
            CreateSalesRep()
        End If
    End Sub

    Private Sub CreateSalesRep()
        Dim MyGuid As Guid = Guid.NewGuid()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdCreateSalesRep As New SqlCommand("SalesRep_CreateNew", cnn)
                cmdCreateSalesRep.CommandType = CommandType.StoredProcedure
                cmdCreateSalesRep.Parameters.Add(New SqlParameter("@SalesRepUId", SqlDbType.UniqueIdentifier)).Value = MyGuid
                cmdCreateSalesRep.Parameters.Add(New SqlParameter("@SalesRepFName", SqlDbType.NVarChar, 50)).Value = Trim(txtFName.Text)
                If Len(Trim(txtMI.Text)) > 0 Or Trim(txtMI.Text) <> String.Empty Then
                    cmdCreateSalesRep.Parameters.Add(New SqlParameter("@SalesRepMI", SqlDbType.Char)).Value = Trim(txtMI.Text)
                Else
                    cmdCreateSalesRep.Parameters.Add(New SqlParameter("@SalesRepMI", SqlDbType.Char)).Value = DBNull.Value
                End If
                cmdCreateSalesRep.Parameters.Add(New SqlParameter("@SalesRepLName", SqlDbType.NVarChar, 50)).Value = Trim(txtLName.Text)
                cmdCreateSalesRep.Parameters.Add(New SqlParameter("@SalesRepBirthDate", SqlDbType.SmallDateTime)).Value = Trim(txtBDate.Text)
                cmdCreateSalesRep.Parameters.Add(New SqlParameter("@SalesRepTaxId", SqlDbType.NVarChar, 11)).Value = Trim(txtTaxId.Text)
                cmdCreateSalesRep.Parameters.Add(New SqlParameter("@SalesRepAddr1", SqlDbType.NVarChar, 50)).Value = Trim(txtAddr1.Text)
                If Len(Trim(txtAddr2.Text)) > 0 Or Trim(txtAddr2.Text) <> String.Empty Then
                    cmdCreateSalesRep.Parameters.Add(New SqlParameter("@SalesRepAddr2", SqlDbType.NVarChar, 50)).Value = Trim(txtAddr2.Text)
                Else
                    cmdCreateSalesRep.Parameters.Add(New SqlParameter("@SalesRepAddr2", SqlDbType.NVarChar, 50)).Value = DBNull.Value
                End If
                cmdCreateSalesRep.Parameters.Add(New SqlParameter("@SalesRepCity", SqlDbType.NVarChar, 50)).Value = Trim(txtCity.Text)
                cmdCreateSalesRep.Parameters.Add(New SqlParameter("@SalesRepState", SqlDbType.Int)).Value = ddState.SelectedValue
                cmdCreateSalesRep.Parameters.Add(New SqlParameter("@SalesRepZip", SqlDbType.NVarChar, 10)).Value = Trim(txtZip.Text)
                cmdCreateSalesRep.Parameters.Add(New SqlParameter("@SalesRepPhone", SqlDbType.NVarChar, 15)).Value = Trim(txtPhone.Text)
                If Len(Trim(txtCell.Text)) > 0 Or Trim(txtCell.Text) <> String.Empty Then
                    cmdCreateSalesRep.Parameters.Add(New SqlParameter("@SalesRepCell", SqlDbType.NVarChar, 15)).Value = Trim(txtCell.Text)
                Else
                    cmdCreateSalesRep.Parameters.Add(New SqlParameter("@SalesRepCell", SqlDbType.NVarChar, 15)).Value = DBNull.Value
                End If
                If Len(Trim(txtFax.Text)) > 0 Or Trim(txtFax.Text) <> String.Empty Then
                    cmdCreateSalesRep.Parameters.Add(New SqlParameter("@SalesRepFax", SqlDbType.NVarChar, 18)).Value = Trim(txtFax.Text)
                Else
                    cmdCreateSalesRep.Parameters.Add(New SqlParameter("@SalesRepFax", SqlDbType.NVarChar, 18)).Value = DBNull.Value
                End If
                cmdCreateSalesRep.Parameters.Add(New SqlParameter("@SalesRepEmail", SqlDbType.NVarChar, 100)).Value = Trim(txtEmail.Text)
                cmdCreateSalesRep.Parameters.Add(New SqlParameter("@SalesRepComm", SqlDbType.SmallMoney)).Value = "0.00"
                cmdCreateSalesRep.ExecuteNonQuery()
            End Using
        End Using
        lblmsg.Text = "Sales Rep Created"
        lblmsg.Font.Bold = True
        cmdAddSalesRep.Visible = False
    End Sub

    Private Sub GetStateDropDown()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetState As New SqlCommand("State_GetState", cnn)
                cmdGetState.CommandType = CommandType.StoredProcedure
                Using dtrGetState As SqlDataReader = cmdGetState.ExecuteReader
                    ddstate.DataSource = dtrGetState
                    ddstate.DataBind()
                End Using
            End Using
        End Using
    End Sub

End Class
