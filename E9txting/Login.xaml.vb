Imports System.Text
Imports System.Security.Cryptography
Imports System.IO

Public Class Window1

    Private Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnLogin.Click
        user = txtUser.Text
        password = txtPass.Password
        If chkRem.IsChecked = True Then
            My.Settings.User = crypto.EncryptString128Bit(user, cryptokey)
            My.Settings.Pass = crypto.EncryptString128Bit(password, cryptokey)
            My.Settings.remember = True
        End If
        Me.Close()
    End Sub

    Private Sub Window1_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Me.Closing
        My.Settings.Save()
    End Sub

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If My.Settings.remember = True Then
            txtUser.Text = crypto.DecryptString128Bit(My.Settings.User, cryptokey)
            txtPass.Password = crypto.DecryptString128Bit(My.Settings.Pass, cryptokey)
            chkRem.IsChecked = True
        End If
    End Sub

    Private Sub txtUser_GotKeyboardFocus(ByVal sender As Object, ByVal e As System.Windows.Input.KeyboardFocusChangedEventArgs) Handles txtUser.GotKeyboardFocus
        KeyboardFocus(True, False, True, e)
    End Sub
    Private Sub txtUser_LostKeyboardFocus(ByVal sender As Object, ByVal e As System.Windows.Input.KeyboardFocusChangedEventArgs) Handles txtUser.LostKeyboardFocus
        KeyboardFocus(False, False, False, e)
    End Sub
    Private Sub txtPass_GotKeyboardFocus(ByVal sender As Object, ByVal e As System.Windows.Input.KeyboardFocusChangedEventArgs) Handles txtPass.GotKeyboardFocus
        Dim source As PasswordBox = TryCast(e.Source, PasswordBox)
        If source IsNot Nothing Then
            ' Change the TextBox color when it obtains focus.
            source.Background = txtbackground
            ' Clear the TextBox.
            source.Clear()
        End If
    End Sub
    Private Sub txtPass_LostKeyboardFocus(ByVal sender As Object, ByVal e As System.Windows.Input.KeyboardFocusChangedEventArgs) Handles txtPass.LostKeyboardFocus
        Dim source As PasswordBox = TryCast(e.Source, PasswordBox)
        If source IsNot Nothing Then
            ' Change the TextBox color when it loses focus.
            source.Background = Brushes.White
            'source.Clear()
        End If
    End Sub


End Class
