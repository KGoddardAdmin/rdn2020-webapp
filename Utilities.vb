Imports System.IO
Imports System.Text
Imports System.Security.Cryptography
Imports System.Web

Namespace lga4040
    Public Class Utilities
        Private Shared ReadOnly Key() As Byte = {36, 87, 14, 8, 221, 95, 75, 18, 59, 167, 131, 162, 183, 114, 185, 146, 127, 228, 154, 220, 243, 156, 193, 87}
        Private Shared ReadOnly Iv() As Byte = {34, 127, 54, 84, 25, 187, 210, 125}

        Public Shared Function ServerEncrypt(ByVal text As String) As String
            Return HttpServerUtility.UrlTokenEncode(Encrypt(text))
        End Function

        Public Shared Function ServerDecrypt(ByVal text As String) As String
            Return Decrypt(HttpServerUtility.UrlTokenDecode(text))
        End Function

        Public Shared Function Encrypt(ByVal plainText As String) As Byte()
            ' Declare a UTF8Encoding object so we may use the GetByte method to transform the plainText into a Byte array. 
            Dim utf8Encoder As UTF8Encoding = New UTF8Encoding()
            Dim inputInBytes() As Byte = utf8Encoder.GetBytes(plainText)

            ' Create a new TripleDES service provider 
            Dim tdesProvider As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider()

            ' The ICryptTransform interface uses the TripleDES crypt provider along with encryption key and init vector information 
            Dim cryptoTransform As ICryptoTransform = tdesProvider.CreateEncryptor(key, iv)

            ' All cryptographic functions need a stream to output the encrypted information. Here we declare a memory stream for this purpose. 
            Dim encryptedStream As MemoryStream = New MemoryStream()
            Dim cryptStream As CryptoStream = New CryptoStream(encryptedStream, cryptoTransform, _
                                                               CryptoStreamMode.Write)

            ' Write the encrypted information to the stream. Flush the information when done to ensure everything is out of the buffer. 
            cryptStream.Write(inputInBytes, 0, inputInBytes.Length)
            cryptStream.FlushFinalBlock()
            encryptedStream.Position = 0

            ' Read the stream back into a Byte array and return it to the calling method. 
            Dim result(encryptedStream.Length - 1) As Byte
            encryptedStream.Read(result, 0, CType(encryptedStream.Length, Integer))
            cryptStream.Close()
            Return result
        End Function

        Public Shared Function Decrypt(ByVal inputInBytes() As Byte) As String
            ' UTFEncoding is used to transform the decrypted Byte Array information back into a string.

            'Dim utf8Encoder As UTF8Encoding = New UTF8Encoding()
            Dim tdesProvider As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider()

            ' As before we must provide the encryption/decryption key along with the init vector. 
            Dim cryptoTransform As ICryptoTransform = tdesProvider.CreateDecryptor(key, iv)

            ' Provide a memory stream to decrypt information into 
            Dim decryptedStream As MemoryStream = New MemoryStream()
            Dim cryptStream As CryptoStream = New CryptoStream(decryptedStream, cryptoTransform, _
                                                               CryptoStreamMode.Write)
            cryptStream.Write(inputInBytes, 0, inputInBytes.Length)
            cryptStream.FlushFinalBlock()
            decryptedStream.Position = 0

            ' Read the memory stream and convert it back into a string 
            Dim result(decryptedStream.Length - 1) As Byte
            decryptedStream.Read(result, 0, CType(decryptedStream.Length, Integer))
            cryptStream.Close()
            Dim myutf As UTF8Encoding = New UTF8Encoding()
            Return myutf.GetString(result)
        End Function
    End Class
End Namespace