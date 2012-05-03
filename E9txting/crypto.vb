Imports System.Security.Cryptography
Imports System.IO
Imports System.Text

Public Class clsCrypto
    'Byte vector required for Rijndael.  This is randomly generated and recommended you change it on a per-application basis.
    'It is 16 bytes.
    Private bytIV() As Byte = {121, 241, 10, 1, 132, 74, 11, 39, 255, 91, 45, 78, 14, 211, 22, 62}

    'Character to pad keys with to make them at least intMinKeySize.
    Private Const chrKeyFill As Char = "X"c

    'String to display on error for functions that return strings. {0} is Exception.Message.
    Private Const strTextErrorString As String = "#ERROR - {0}"

    'Min size in bytes of randomly generated salt.
    Private Const intMinSalt As Integer = 4

    'Max size in bytes of randomly generated salt.
    Private Const intMaxSalt As Integer = 8

    'Size in bytes of Hash result.  MD5 returns a 128 bit hash.
    Private Const intHashSize As Integer = 16

    'Size in bytes of the key length.  Rijndael takes either a 128, 192, or 256 bit key.  
    'If it is under this, pad with chrKeyFill. If it is over this, truncate to the length.
    Private Const intKeySize As Integer = 32

    'Encrypt a String with Rijndael symmetric encryption.
    Public Function EncryptString128Bit(ByVal strPlainText As String, ByVal strKey As String) As String
        Try
            Dim bytPlainText() As Byte
            Dim bytKey() As Byte
            Dim bytEncoded() As Byte
            Dim objMemoryStream As New MemoryStream
            Dim objRijndaelManaged As New RijndaelManaged

            strPlainText = strPlainText.Replace(vbNullChar, String.Empty)

            bytPlainText = Encoding.UTF8.GetBytes(strPlainText)
            bytKey = ConvertKeyToBytes(strKey)

            Dim objCryptoStream As New CryptoStream(objMemoryStream, _
                objRijndaelManaged.CreateEncryptor(bytKey, bytIV), _
                CryptoStreamMode.Write)

            objCryptoStream.Write(bytPlainText, 0, bytPlainText.Length)
            objCryptoStream.FlushFinalBlock()

            bytEncoded = objMemoryStream.ToArray
            objMemoryStream.Close()
            objCryptoStream.Close()

            Return Convert.ToBase64String(bytEncoded)
        Catch ex As Exception
            Return String.Format(strTextErrorString, ex.Message)
        End Try
    End Function

    'Decrypt a String with Rijndael symmetric encryption.
    Public Function DecryptString128Bit(ByVal strCryptText As String, ByVal strKey As String) As String
        Try
            Dim bytCryptText() As Byte
            Dim bytKey() As Byte

            Dim objRijndaelManaged As New RijndaelManaged

            bytCryptText = Convert.FromBase64String(strCryptText)
            bytKey = ConvertKeyToBytes(strKey)

            Dim bytTemp(bytCryptText.Length) As Byte
            Dim objMemoryStream As New MemoryStream(bytCryptText)

            Dim objCryptoStream As New CryptoStream(objMemoryStream, _
                objRijndaelManaged.CreateDecryptor(bytKey, bytIV), _
                CryptoStreamMode.Read)

            objCryptoStream.Read(bytTemp, 0, bytTemp.Length)

            objMemoryStream.Close()
            objCryptoStream.Close()

            Return Encoding.UTF8.GetString(bytTemp).Replace(vbNullChar, String.Empty)

        Catch ex As Exception
            Return String.Format(strTextErrorString, ex.Message)
        End Try

    End Function

    'Compute an MD5 hash code from a string and append any salt-bytes used/generated to the end.
    Public Function ComputeMD5Hash(ByVal strPlainText As String, Optional ByVal bytSalt() As Byte = Nothing) As String
        Try
            Dim bytPlainText As Byte() = Encoding.UTF8.GetBytes(strPlainText)
            Dim hash As HashAlgorithm = New MD5CryptoServiceProvider()

            If bytSalt Is Nothing Then
                Dim rand As New Random
                Dim intSaltSize As Integer = rand.Next(intMinSalt, intMaxSalt)

                bytSalt = New Byte(intSaltSize - 1) {}

                Dim rng As New RNGCryptoServiceProvider
                rng.GetNonZeroBytes(bytSalt)
            End If

            Dim bytPlainTextWithSalt() As Byte = New Byte(bytPlainText.Length + bytSalt.Length - 1) {}

            bytPlainTextWithSalt = ConcatBytes(bytPlainText, bytSalt)

            Dim bytHash As Byte() = hash.ComputeHash(bytPlainTextWithSalt)
            Dim bytHashWithSalt() As Byte = New Byte(bytHash.Length + bytSalt.Length - 1) {}

            bytHashWithSalt = ConcatBytes(bytHash, bytSalt)

            Return Convert.ToBase64String(bytHashWithSalt)
        Catch ex As Exception
            Return String.Format(strTextErrorString, ex.Message)
        End Try
    End Function

    'Verify a string against a hash generated with the ComputeMD5Hash function above.
    Public Function VerifyHash(ByVal strPlainText As String, ByVal strHashValue As String) As Boolean
        Try
            Dim bytWithSalt As Byte() = Convert.FromBase64String(strHashValue)

            If bytWithSalt.Length < intHashSize Then Return False

            Dim bytSalt() As Byte = New Byte(bytWithSalt.Length - intHashSize - 1) {}

            Array.Copy(bytWithSalt, intHashSize, bytSalt, 0, bytWithSalt.Length - intHashSize)

            Dim strExpectedHashString As String = ComputeMD5Hash(strPlainText, bytSalt)

            Return strHashValue.Equals(strExpectedHashString)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    'Simple function to concatenate two byte arrays. 
    Private Function ConcatBytes(ByVal bytA() As Byte, ByVal bytB() As Byte) As Byte()
        Try
            Dim bytX() As Byte = New Byte(((bytA.Length + bytB.Length)) - 1) {}

            Array.Copy(bytA, bytX, bytA.Length)
            Array.Copy(bytB, 0, bytX, bytA.Length, bytB.Length)

            Return bytX
        Catch ex As Exception
            Return Nothing
        End Try

    End Function

    'A function to convert a string into a 32 byte key. 
    Private Function ConvertKeyToBytes(ByVal strKey As String) As Byte()
        Try
            Dim intLength As Integer = strKey.Length

            If intLength < intKeySize Then
                strKey &= Strings.StrDup(intKeySize - intLength, chrKeyFill)
            Else
                strKey = strKey.Substring(0, intKeySize)
            End If

            Return Encoding.UTF8.GetBytes(strKey)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

End Class