Imports System.Data
Imports System.Data.SqlClient

Partial Class IONew
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GetClientList()
            GetSalesRep()            
            GetIOType()
            GetIOStatus()
        End If
    End Sub

    Protected Sub cmdAddIO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddIO.Click


        Page.Validate("frmIO")
        If Page.IsValid Then
            CreateIO()
        End If

    End Sub

    Private Sub CreateIO()
        Dim MyGuid As Guid = Guid.NewGuid()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdCreateIO As New SqlCommand("IO_CreateNew", cnn)
                cmdCreateIO.CommandType = CommandType.StoredProcedure
                cmdCreateIO.Parameters.Add(New SqlParameter("@IOId", SqlDbType.NVarChar, 100)).Value = Trim(txtIOId.Text)
                cmdCreateIO.Parameters.Add(New SqlParameter("@IOName", SqlDbType.NVarChar, 100)).Value = Trim(txtIOName.Text)
                cmdCreateIO.Parameters.Add(New SqlParameter("@IOUId", SqlDbType.UniqueIdentifier)).Value = MyGuid
                cmdCreateIO.Parameters.Add(New SqlParameter("@ClientUId", SqlDbType.UniqueIdentifier)).Value = SqlTypes.SqlGuid.Parse(ddClient.SelectedValue)
                cmdCreateIO.Parameters.Add(New SqlParameter("@ContactUId", SqlDbType.UniqueIdentifier)).Value = SqlTypes.SqlGuid.Parse(ddContact.SelectedValue)
                cmdCreateIO.Parameters.Add(New SqlParameter("@SalesRepUid", SqlDbType.UniqueIdentifier)).Value = SqlTypes.SqlGuid.Parse(ddSalesRep.SelectedValue)                
                cmdCreateIO.Parameters.Add(New SqlParameter("@IOStatus", SqlDbType.TinyInt)).Value = ddStatus.SelectedValue
                cmdCreateIO.Parameters.Add(New SqlParameter("@IOType", SqlDbType.TinyInt)).Value = ddType.SelectedValue
                If Len(Trim(txtUnitCost.Text)) > 0 Or Trim(txtUnitCost.Text) <> String.Empty Then
                    cmdCreateIO.Parameters.Add(New SqlParameter("@IOUnitCost", SqlDbType.Decimal)).Value = Trim(txtUnitCost.Text)
                Else
                    cmdCreateIO.Parameters.Add(New SqlParameter("@IOClickCost", SqlDbType.Decimal)).Value = DBNull.Value
                End If
                cmdCreateIO.Parameters.Add(New SqlParameter("@IOQuanity", SqlDbType.Int)).Value = Trim(txtQuanity.Text)
                cmdCreateIO.Parameters.Add(New SqlParameter("@IOAmount", SqlDbType.Decimal)).Value = Trim(txtAmount.Text)
                If Len(Trim(txtNote.Text)) > 0 Or Trim(txtNote.Text) <> String.Empty Then
                    cmdCreateIO.Parameters.Add(New SqlParameter("@IONote", SqlDbType.NVarChar, 500)).Value = Trim(txtNote.Text)
                Else
                    cmdCreateIO.Parameters.Add(New SqlParameter("@IONote", SqlDbType.NVarChar, 500)).Value = DBNull.Value
                End If
                cmdCreateIO.ExecuteNonQuery()
            End Using
        End Using
        lblmsg.Text = "IO created."
        lblmsg.Font.Bold = True
        cmdAddIO.Visible = False
    End Sub

    Private Sub GetIOType()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetIOType As New SqlCommand("IOType_Get", cnn)
                cmdGetIOType.CommandType = CommandType.StoredProcedure
                Using dtrGetIOType As SqlDataReader = cmdGetIOType.ExecuteReader
                    ddType.DataSource = dtrGetIOType
                    ddType.DataBind()
                End Using
            End Using
        End Using
    End Sub

    Private Sub GetIOStatus()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetIOStatus As New SqlCommand("IOStatus_Get", cnn)
                cmdGetIOStatus.CommandType = CommandType.StoredProcedure
                Using dtrGetIOType As SqlDataReader = cmdGetIOStatus.ExecuteReader
                    ddStatus.DataSource = dtrGetIOType
                    ddStatus.DataBind()
                End Using
            End Using
        End Using

    End Sub


    Private Sub GetSalesRep()
        Dim dstGetSalesRep As DataSet
        Dim dadGetSalesRep As SqlDataAdapter
        Dim cnn As New SqlConnection(strConn)
        dstGetSalesRep = New DataSet
        dadGetSalesRep = New SqlDataAdapter("SalesRep_Get", cnn)
        dadGetSalesRep.Fill(dstGetSalesRep, "AccountReps")
        If dstGetSalesRep.Tables("AccountReps").Rows.Count = 0 Then
            lblmsg.Text = "Need to add sales rep for this client."
            lblmsg.Font.Bold = True
        Else
            Dim Dyncolumn As New DataColumn
            With Dyncolumn
                .ColumnName = "Name"
                .DataType = System.Type.GetType("System.String")
                .Expression = "SalesRepFName+'  '+SalesRepLName"
            End With
            dstGetSalesRep.Tables("AccountReps").Columns.Add(Dyncolumn)
            ddSalesRep.DataTextField = "Name"
            ddSalesRep.DataValueField = "SalesRepUId"
            ddSalesRep.DataSource = dstGetSalesRep.Tables("AccountReps").DefaultView
            ddSalesRep.DataBind()
        End If
        ddSalesRep.Items.Insert(0, "Select Account Rep")

    End Sub

    Private Sub GetContact()
        Dim dadGetContact As New SqlDataAdapter
        Dim dstGetContact As New DataSet
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetContact As New SqlCommand("ClientContact_GetContact_ForIO", cnn)
                cmdGetContact.CommandType = CommandType.StoredProcedure
                cmdGetContact.Parameters.Add(New SqlParameter("@ClientUId", SqlDbType.UniqueIdentifier)).Value = SqlTypes.SqlGuid.Parse(ddClient.SelectedValue)
                dadGetContact.SelectCommand = cmdGetContact
                dadGetContact.Fill(dstGetContact, "Contacts")
                If dstGetContact.Tables("Contacts").Rows.Count = 0 Then
                    lblmsg.Text = "Need to add sales rep for this client."
                    lblmsg.Font.Bold = True
                Else
                    Dim Dyncolumn As New DataColumn
                    With Dyncolumn
                        .ColumnName = "Name"
                        .DataType = System.Type.GetType("System.String")
                        .Expression = "ContactFName+'  '+ContactLName"
                    End With
                    dstGetContact.Tables("Contacts").Columns.Add(Dyncolumn)
                    ddContact.DataTextField = "Name"
                    ddContact.DataValueField = "ContactUId"
                    ddContact.DataSource = dstGetContact.Tables("Contacts").DefaultView
                    ddContact.DataBind()
                End If
            End Using
        End Using
        ddContact.Items.Insert(0, "Select Contact")
        dadGetContact.Dispose()
        dstGetContact.Dispose()        
    End Sub

    Private Sub GetClientList()
        ddClient.Items.Add(New ListItem("Select Client", 0))
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetClientList As New SqlCommand("Client_Get", cnn)
                Using dtrGetClientList As SqlDataReader = cmdGetClientList.ExecuteReader
                    ddClient.DataSource = dtrGetClientList
                    ddClient.DataBind()
                End Using
            End Using
        End Using
        ddClient.Items.Insert(0, "Select Client")
    End Sub

    Protected Sub ddClient_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddClient.SelectedIndexChanged        
        GetContact()
    End Sub

End Class
