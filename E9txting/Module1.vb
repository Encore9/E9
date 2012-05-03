Imports System
Imports System.Net
Imports System.Net.Mail
Imports System.Data
Module Module1


    'Used for encrypting and decrypting the saved email and password
    Public cryptokey As String = "aC4Zwsx3jayNC5Uk01bgbsIh1JFEIteXl5hSLYdkCMZnXpMVJ9uCbxb0lOLLxHX55Tq0mRxRdW5ql0ANkkdxMG1TEHPV5ZJzTdDn3EuWLqaopaOvKTSgUjtKO0gJ2AI2LO5q6PbO0dpHO9BYPPyutkh1CYLT9XkUhOHCgo917djaLMiMpqG2EtUoElrMGeR5tgvHY7qysWi0ZoSyppi3mVckjMu4d43dzRZrkfgQtIMNUQO53XvWVoUhfIXyKjIg"
    Public user As String = Nothing
    Public password As String = Nothing
    Public crypto As New clsCrypto

    'total number of messages being sent
    Public msgnumber As Integer = 0
    'google smtp client
    Public mailClient = New SmtpClient("smtp.gmail.com", "587")

    Public first As Boolean = True

    'background color of the text boxs
    Public txtbackground As New SolidColorBrush


    Public gridrow As New DataGridRow
    Public ValFName As String
    Public ValLname As String
    Public ValNum As String
    Public ValProvider As String
    Public Function GetRow(ByVal grid As DataGrid, ByVal index As Integer) As DataGridRow

        Try
            Dim row As DataGridRow = DirectCast(grid.ItemContainerGenerator.ContainerFromIndex(index), DataGridRow)
            If row Is Nothing Then
                ' May be virtualized, bring into view and try again.
                grid.UpdateLayout()
                grid.ScrollIntoView(grid.Items(index))
                row = DirectCast(grid.ItemContainerGenerator.ContainerFromIndex(index), DataGridRow)
            End If
            Return row
        Catch ex As IndexOutOfRangeException
            Return Nothing


        End Try
    End Function
    '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    '!!!!!THIS WILL ONLY WORK WITH TEXTBOXES.!!!!!
    '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    'This function is designed to change the background color of text boxes. When GainFocus is true the text is changed to a blue color. If it is false
    'it will change it to white and there are also options to capitalize the first letter or to clear the text.
    Public Function KeyboardFocus(ByVal GainFocus As Boolean, ByVal Capitalize As Boolean, ByVal Clear As Boolean, ByVal e As System.Windows.Input.KeyboardFocusChangedEventArgs)
        If GainFocus = True Then
            Dim source As TextBox = TryCast(e.Source, TextBox)
            If source IsNot Nothing Then
                ' Change the TextBox color when it obtains focus.
                source.Background = txtbackground
                If Clear = True Then
                    ' Clear the TextBox.
                    source.Clear()
                End If
            End If
        Else
            Dim source As TextBox = TryCast(e.Source, TextBox)
            If source IsNot Nothing Then
                ' Change the TextBox color when it loses focus.
                source.Background = Brushes.White
            End If
            If Capitalize = True And source.Text.Length > 0 Then
                Dim tmp As String = source.Text.Chars(0)
                source.Text = tmp.ToUpper & source.Text.Remove(0, 1)
            End If
        End If
        Return Nothing
    End Function



End Module
