Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System
Imports System.Configuration

Namespace lga4040
    Public Class Client
        Private ReadOnly _strConn As String = ConfigurationSettings.AppSettings("cnn")
        Private _ds As DataSet
        Private _cds As DataSet
        Private _dt As DataTable
        Private _cdt As DataTable
        Private _cs As StructClient = New StructClient
        Private _ccs As StructClient = New StructClient
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

        Public Structure StructClient
            Public LGAClientId As Integer
            Public ClientsClientId As Integer
            Public LGAClientUId As Guid
            Public ClientsClientUId As Guid
            Public LGAClientName As String
            Public ClientsClientName As String
            Public LGAClientAddr1 As String
            Public ClientsClientAddr1 As String
            Public LGAClientAddr2 As String
            Public ClientsClientAddr2 As String
            Public LGAClientCity As String
            Public ClientsClientCity As String
            Public LGAClientState As Integer
            Public ClientsClientState As Integer
            Public LGAClientZip As String
            Public ClientsClientZip As String
            Public LGAClientPhone As String
            Public ClientsClientPhone As String
            Public LGAClientFax As String
            Public ClientsClientFax As String
            Public LGAClientWebsite As String
            Public ClientsClientWebsite As String
            Public LGAClientIsActive As Integer
            Public ClientsClientIsActive As Integer
            Public LGAClientDomain As String
            Public ClientsClientDomain As String
            Public LGAClientClick As String
            Public ClientsClientClick As String
            Public LGAClientOpen As String
            Public ClientsClientOpen As String
            Public LGAClientCoupon As String
            Public ClientsClientCoupon As String
            Public LGAClientDataBase As String
            Public ClientsClientDataBase As String
            Public Message As String
        End Structure

#End Region

        Public Function GetClients() As DataSet
            Dim dsClients As New DataSet
            _sqlConn = New SqlConnection(_strConn)
            _sqlCommand = New SqlCommand()
            _sqlCommand.CommandType = CommandType.StoredProcedure
            _sqlCommand.CommandText = "Client_Get"
            _sqlDataAdapter = New SqlDataAdapter(_sqlCommand)

            Try

                If _sqlConn.State = ConnectionState.Closed Then
                    _sqlConn.Open()
                End If

                _sqlCommand.Connection = _sqlConn
                _sqlDataAdapter.Fill(dsClients)
            Catch ex As Exception
                ErrorMsg = "An error has occured while retrieving the Client list. &nbsp; Please contact your system administrator"
            Finally
                _sqlConn.Close()
                _sqlCommand.Dispose()
                dsClients.Dispose()
            End Try

            Return dsClients

        End Function

#Region "Client's Client Functions"
        Public Function GetClientsClient(ByVal clientUId As Guid) As DataSet
            Dim dataBase As String = GetClientsDataBase(clientUId)
            Dim dsClientsClient As New DataSet
            _sqlConn = New SqlConnection(_strConn)
            _sqlCommand = New SqlCommand()
            _sqlCommand.CommandType = CommandType.StoredProcedure
            _sqlCommand.CommandText = "sp_ClientsClient_Get"
            _sqlDataAdapter = New SqlDataAdapter(_sqlCommand)

            Try
                If _sqlConn.State = ConnectionState.Closed Then _sqlConn.Open()
                _sqlCommand.Connection = _sqlConn
                _sqlCommand.Parameters.Add(New SqlParameter("@DataBase", SqlDbType.VarChar)).Value = dataBase
                _sqlDataAdapter.Fill(dsClientsClient)
            Catch ex As Exception
                ErrorMsg = "An error has occured while retrieving the Io list. &nbsp; Please contact your system administrator"
            Finally
                _sqlConn.Close()
                _sqlCommand.Dispose()
                dsClientsClient.Dispose()
            End Try

            Return dsClientsClient
        End Function

        Public Function GetClientsDataBase(ByVal clientUId As Guid) As String
            Dim dataBase As String = String.Empty
            Using cnn As New SqlConnection(_strConn)
                cnn.Open()
                Using cmdGetClientsDataBase As New SqlCommand("ClientCompany_GetByClientUID", cnn)
                    cmdGetClientsDataBase.CommandType = CommandType.StoredProcedure
                    cmdGetClientsDataBase.Parameters.Add(New SqlParameter("@ClientUid", SqlDbType.UniqueIdentifier)).Value = ClientUId
                    Using dtrGetClientsDataBase As SqlDataReader = cmdGetClientsDataBase.ExecuteReader
                        If dtrGetClientsDataBase.HasRows Then
                            While dtrGetClientsDataBase.Read
                                If Not IsDBNull(dtrGetClientsDataBase("DataBase")) Then
                                    dataBase = CType(dtrGetClientsDataBase("DataBase"), String)
                                Else
                                    ErrorMsg = "An error has occured while retrieving the Client's Data Base. &nbsp; Please contact your system administrator"
                                End If
                            End While
                        Else
                            ErrorMsg = "An error has occured while retrieving the Client's Data Base. &nbsp; Please contact your system administrator"
                        End If
                    End Using
                End Using
            End Using
            Return dataBase
        End Function
#End Region

    End Class
End Namespace