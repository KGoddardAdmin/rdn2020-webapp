Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

Namespace lga4040
    Public Class IO
        Private ReadOnly _strConn As String = ConfigurationSettings.AppSettings("cnn")
        Private _ds As DataSet
        Private _dt As DataTable
        Private _ios As StructIO = New StructIO
        Private _sqlConn As SqlConnection
        Private _sqlCommand As SqlCommand
        Private _sqlDataAdapter As SqlDataAdapter
        Private _errorMsg As String = ""

        Public Property ErrorMsg() As String
            Get
                Return _errorMsg
            End Get
            Set(ByVal value As String)
                _errorMsg = value
            End Set
        End Property

#Region "Structures"
        Public Structure StructIO
            ' ReSharper disable InconsistentNaming
            Public LGIOId As String
            Public ClientsIOId As String
            Public LGIOName As String
            Public ClientsIOName As String
            Public LGIOUId As Guid
            Public ClientsIOUId As Guid
            Public LGIOStartDate As Date
            Public ClientsIOStartDate As Date
            Public LGClientUId As Guid
            Public ClientsClientUId As Guid
            Public LGContactUId As Guid
            Public ClientsContactUId As Guid
            Public LGSalesRepUId As Guid
            Public ClientsSalesRepUId As Guid
            Public LGCampaignId As Integer
            Public ClientsCampaign_Id As Integer
            Public LGIOStatusId As Integer
            Public ClientsIOStatusId As Integer
            Public LGIOTypeId As Integer
            Public ClientsIOTypeId As Integer
            Public LGIOQuanity As Integer
            Public ClientsIOQuanity As Integer
            Public LGIOAmount As Double
            Public ClientsIOAmount As Double
            Public LGIONote As String
            Public ClientsIONote As String
            Public LGIOCompletionDate As Date
            Public ClientsIOCompletionDate As Date
            Public LGIsActive As Integer
            Public ClientsIsActive As Integer
            Public Message As String
            ' ReSharper restore InconsistentNaming
        End Structure
#End Region

        Public Function GetIOs(ByVal clientUId As Guid) As DataSet
            Dim dsIOs As New DataSet
            _sqlConn = New SqlConnection(_strConn)
            _sqlCommand = New SqlCommand()
            _sqlCommand.CommandType = CommandType.StoredProcedure
            _sqlCommand.CommandText = "IO_Get_ForCampiagnCreation"
            _sqlDataAdapter = New SqlDataAdapter(_sqlCommand)

            Try
                If _sqlConn.State = ConnectionState.Closed Then _sqlConn.Open()
                _sqlCommand.Connection = _sqlConn
                _sqlCommand.Parameters.Add(New SqlParameter("@ClientUId", SqlDbType.UniqueIdentifier)).Value = clientUId
                _sqlDataAdapter.Fill(dsIOs)
            Catch ex As Exception
                ErrorMsg = "An error has occured while retrieving the Io list. &nbsp; Please contact your system administrator"
            Finally
                _sqlConn.Close()
                _sqlCommand.Dispose()
                dsIOs.Dispose()
            End Try

            Return dsIOs
        End Function

        Public Function GetClientsClientIO(ByVal clientUId As Guid, ByVal clientsClientUId As Guid) As DataSet
            Dim cl As New Client
            Dim dataBase As String
            dataBase = cl.GetClientsDataBase(clientUId)
            Dim dsClientsClientIO As New DataSet
            _sqlConn = New SqlConnection(_strConn)
            _sqlCommand = New SqlCommand()
            _sqlCommand.CommandType = CommandType.StoredProcedure
            _sqlCommand.CommandText = "IO_Get_ForClientsCampiagnCreation"
            _sqlDataAdapter = New SqlDataAdapter(_sqlCommand)

            Try
                If _sqlConn.State = ConnectionState.Closed Then _sqlConn.Open()
                _sqlCommand.Connection = _sqlConn
                _sqlCommand.Parameters.Add(New SqlParameter("@DataBase", SqlDbType.VarChar)).Value = dataBase
                _sqlCommand.Parameters.Add(New SqlParameter("@ClientUID", SqlDbType.UniqueIdentifier)).Value = clientsClientUId
                _sqlDataAdapter.Fill(dsClientsClientIO)
            Catch ex As Exception
                ErrorMsg = "An error has occured while retrieving the Io list. &nbsp; Please contact your system administrator"
            Finally
                _sqlConn.Close()
                _sqlCommand.Dispose()
                dsClientsClientIO.Dispose()
            End Try

            Return dsClientsClientIO
        End Function
    End Class
End Namespace