Imports System.Net
Imports System.Net.Mail
Imports System.Data
Imports System.Windows.Threading


Class MainWindow
    Dim datamanager As New clsDataTables
    Dim htimer As New DispatcherTimer
    Dim mtimer As New DispatcherTimer
    Dim stimer As New DispatcherTimer

#Region "Buttons"
    Private Sub btnSend_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSend.Click
        Try
            Dim cred = New NetworkCredential(user, password)
            If Not (txtMsg1.Text = "" Or txtMsg1.Text = Nothing) Then
                If isNumeric(txtNumber.Text, Globalization.NumberStyles.Integer) = True And txtNumber.Text.Length = 10 Then
                    If Not (user Is Nothing) Then
                        'enables SSL
                        mailClient.EnableSsl = True
                        'sets up credentials
                        mailClient.Credentials = cred
                        'creates message
                        '3176268730@txt.att.net jason
                        Dim message As New MailMessage(user, txtNumber.Text & EmailSuffixComboBox.Text, "", txtMsg1.Text)
                        post("Begin sending Message 1 to: " & txtNumber.Text & EmailSuffixComboBox.Text)
                        mailClient.Send(message)
                        post("Sent Message 1 to: " & txtNumber.Text & EmailSuffixComboBox.Text)
                        If txtMsg2.Visibility = Windows.Visibility.Visible And Not (txtMsg2.Text Is Nothing) Then
                            Dim message2 As New MailMessage(user, txtNumber.Text & EmailSuffixComboBox.Text, "", txtMsg2.Text)
                            post("Begin sending Message 2 to: " & txtNumber.Text & EmailSuffixComboBox.Text)
                            mailClient.send(message2)
                            post("Sent Message 2 to: " & txtNumber.Text & EmailSuffixComboBox.Text)
                        End If
                        If txtMsg3.Visibility = Windows.Visibility.Visible And Not (txtMsg2.Text Is Nothing) Then
                            Dim message3 As New MailMessage(user, txtNumber.Text & EmailSuffixComboBox.Text, "", txtMsg3.Text)
                            post("Begin sending Message 3 to: " & txtNumber.Text & EmailSuffixComboBox.Text)
                            mailClient.send(message3)
                            post("Sent Message 3 to: " & txtNumber.Text & EmailSuffixComboBox.Text)
                        End If
                    Else
                        Dim frm As New Window1
                        frm.ShowInTaskbar = False
                        frm.ShowDialog()
                    End If
                Else
                    'MsgBox("You must provide a 10 digit number to send the message to!", MsgBoxStyle.Critical, "Error!")
                    post("You must provide a 10 digit number to send the message to!")
                End If
            Else
                'MsgBox("You must provide a message to be sent!", MsgBoxStyle.Critical, "Error!")
                post("You must provide a message to be sent!")
            End If
        Catch ex As SmtpException
            post("Bad user name or password. Try logging in again.")
            Dim frm As New Window1
            frm.ShowInTaskbar = False
            frm.ShowDialog()
        End Try
    End Sub
    Private Sub btnMass_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnMass.Click
        Dim cred = New NetworkCredential(user, password)
        For i As Integer = 0 To datamanager.RowCount - 1
            'gets number from the datatable in clsDataManager
            'MsgBox(datamanager.GetAddress(i))
            Try
                If datamanager.hasRows = True Then
                    If Not (user Is Nothing) Then
                        'enables SSL
                        mailClient.EnableSsl = True
                        'sets up credentials
                        mailClient.Credentials = cred
                        'creates message
                        '3176268730@txt.att.net jason
                        Dim message As New MailMessage(user, datamanager.GetAddress(i), "", txtMsg1.Text)
                        'sends message
                        post("Begin sending Message 1 to: " & datamanager.GetAddress(i))
                        mailClient.Send(message)
                        post("Sent Message 1 to: " & datamanager.GetAddress(i))
                        If txtMsg2.Visibility = Windows.Visibility.Visible And Not (txtMsg2.Text Is Nothing) Then
                            Dim message2 As New MailMessage(user, datamanager.GetAddress(i), "", txtMsg2.Text)
                            post("Begin sending Message 2 to: " & datamanager.GetAddress(i))
                            mailClient.send(message2)
                            post("Sent Message 2 to: " & datamanager.GetAddress(i))
                        End If
                        If txtMsg3.Visibility = Windows.Visibility.Visible And Not (txtMsg2.Text Is Nothing) Then
                            Dim message3 As New MailMessage(user, datamanager.GetAddress(i), "", txtMsg3.Text)
                            post("Begin sending Message 3 to: " & datamanager.GetAddress(i))
                            mailClient.send(message3)
                            post("Sent Message 3 to: " & datamanager.GetAddress(i))
                        End If
                    Else
                        Dim frm As New Window1
                        frm.ShowInTaskbar = False
                        frm.ShowDialog()
                    End If
                Else
                    'MsgBox("You must provide a message to be sent!", MsgBoxStyle.Critical, "Error!")
                    post("You must provide a message to be sent!")
                End If
            Catch ex As SmtpException

                post("Bad user name or password. Try logging in again.")
                Dim frm As New Window1
                frm.ShowInTaskbar = False
                frm.ShowDialog()
            End Try
        Next
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnAdd.Click
        Dim CellProvidersStudentsTableAdapter As E9txting.cellProvidersTableAdapters.StudentsTableAdapter = New E9txting.cellProvidersTableAdapters.StudentsTableAdapter()
        'Dim StudentsViewSource As System.Windows.Data.CollectionViewSource = CType(Me.FindResource("StudentsViewSource"), System.Windows.Data.CollectionViewSource)
        'gets number fo rows in dgstudents

        Dim count = dgStudents.ItemsSource.OfType(Of Object)().Count()
        post("number of rows in students: " & count)
        'checks to see if this is the first run
        If first = True Then
            post("Createing students datatable from Add student button")
            first = False
            'adds all rows in dgstudents to a datatable in clsdatamanager
            For i As Integer = 0 To count - 1
                'gets row i
                gridrow = GetRow(dgStudents, i)
                'gets column info
                ValFName = gridrow.Item(1).ToString 'first column
                ValLname = gridrow.Item(2).ToString  'second column
                ValNum = gridrow.Item(3).ToString    'third column
                ValProvider = gridrow.Item(4).ToString   'fourth column
                'adds infor to datatable in clsdatamanager
                post("Adding: " & ValFName & " " & ValLname & " " & ValNum & " " & ValProvider & " " & " to students datatable")
                datamanager.AddStudentsRow(ValFName, ValLname, ValNum, ValProvider)
            Next
        End If
        'adds a new row to the student database

        CellProvidersStudentsTableAdapter.AddStudent(count + 1, txtFname.Text, txtLname.Text, txtAddNum.Text, PrintText(lbProvider), PrintText(lbCommon))
        'adds new student to datatable in clsdatamanager
        post("New student added to database.")
        datamanager.AddStudentsRow(txtFname.Text, txtLname.Text, txtAddNum.Text, PrintText(lbProvider))
        'refreshes dgstudents
        dgStudents.DataContext = datamanager.setStudents
        post("dgstudents updated")
    End Sub
    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnClear.Click
        post("Begin clearing recipients list")
        datamanager.EmptyRecipients()
        dgRecipients.DataContext = datamanager.setRecipients
        post("Recipients list cleared")
    End Sub
#End Region

    Private Sub MainWindow_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory)
            post("DataDirectory Changed")

            Dim CellProviders As E9txting.cellProviders = CType(Me.FindResource("CellProviders"), E9txting.cellProviders)
            Dim CellProvidersProvidersTableAdapter As E9txting.cellProvidersTableAdapters.ProvidersTableAdapter = New E9txting.cellProvidersTableAdapters.ProvidersTableAdapter()
            Dim ProvidersViewSource As System.Windows.Data.CollectionViewSource = CType(Me.FindResource("ProvidersViewSource"), System.Windows.Data.CollectionViewSource)
            Dim CellProvidersStudentsTableAdapter As E9txting.cellProvidersTableAdapters.StudentsTableAdapter = New E9txting.cellProvidersTableAdapters.StudentsTableAdapter()
            Dim StudentsViewSource As System.Windows.Data.CollectionViewSource = CType(Me.FindResource("StudentsViewSource"), System.Windows.Data.CollectionViewSource)

            'Load data into the table Providers. You can modify this code as needed.
            CellProvidersProvidersTableAdapter.Fill(CellProviders.Providers)
            ProvidersViewSource.View.MoveCurrentToFirst()
            'Load data into the table Students. You can modify this code as needed.
            CellProvidersStudentsTableAdapter.Fill(CellProviders.Students)
            StudentsViewSource.View.MoveCurrentToFirst()
            'sets textbox background color

        Catch ex As Exception
            post("Error:" & ex.Message)
        Finally
            txtbackground.Color = Color.FromArgb(72, 0, 116, 255)
            post("Created Data sources, Making visible.")
        End Try

    End Sub
    
#Region "Add Messages"
    Private Sub txtMsg_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles txtMsg1.TextChanged

        'when 140 characters is reached show a message box that askes if you want to add a message
        If txtMsg1.Text.Length < 140 Then
            'shows length of message 1
            lblLen1.Content = "Length: " & txtMsg1.Text.Length
            'number of messages created used for keeping track of sent messages
            msgnumber = 1
        Else
            If MsgBox("Add another message?", MsgBoxStyle.YesNo, "More?") = MsgBoxResult.Yes Then
                'show second message
                txtMsg2.Visibility = Windows.Visibility.Visible
                lblLen2.Visibility = Windows.Visibility.Visible
                post("Second message created!")
            End If
            'set second message focus
            txtMsg2.Focus()
        End If
    End Sub
    Private Sub txtMsg2_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles txtMsg2.TextChanged

        'if 140 characters  reached then ask if you want antoher message
        If txtMsg2.Text.Length < 140 Then
            'shows length of message
            lblLen2.Content = "Length: " & txtMsg2.Text.Length
            'number of messages created used for keeping track of sent messages
            msgnumber = 2
        Else
            If MsgBox("Add another message?", MsgBoxStyle.YesNo, "More?") = MsgBoxResult.Yes Then
                'show message 3
                txtMsg3.Visibility = Windows.Visibility.Visible
                lblLen3.Visibility = Windows.Visibility.Visible
                post("Third message created!")
            End If
            'set focus to message 3
            txtMsg3.Focus()
        End If
    End Sub
    Private Sub txtMsg3_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles txtMsg3.TextChanged

        If txtMsg3.Text.Length < 140 Then
            'length of message
            lblLen3.Content = "Length: " & txtMsg3.Text.Length
            'message number. used to keep track of sent messages
            msgnumber = 3
        Else
            'cant send more than 3 messages
            MsgBox("Max length reached!", MsgBoxStyle.Information, "Field Full!")
            post("Can't add more messages!")
        End If
    End Sub
#End Region
#Region "Expanders"
    Private Sub Expander1_Expanded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Expander1.Expanded
        Expander2.IsExpanded = False
        Expander3.IsExpanded = False
    End Sub

    Private Sub Expander2_Expanded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Expander2.Expanded
        Expander1.IsExpanded = False
        Expander3.IsExpanded = False
    End Sub

    Private Sub Expander3_Expanded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Expander3.Expanded
        Expander1.IsExpanded = False
        Expander2.IsExpanded = False
        txtoutput2.ScrollToEnd()
    End Sub
#End Region
#Region "Update Data"
    Private Sub dgStudents_MouseEnter(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles dgStudents.MouseEnter
        Try
            If first = True Then
                post("Creating student datatable from dg MouseEnter")
                first = False
                Dim count = dgStudents.ItemsSource.OfType(Of Object)().Count()
                'adds all rows in dgStudents to a datatable 
                post("Number of rows in students: " & count)
                For i As Integer = 0 To count - 1
                    'gets row i 
                    gridrow = GetRow(dgStudents, i)
                    'gets column info
                    ValFName = gridrow.Item(1).ToString 'first column
                    ValLname = gridrow.Item(2).ToString  'second column
                    ValNum = gridrow.Item(3).ToString    'third column
                    ValProvider = gridrow.Item(4).ToString   'fourth column
                    'adds row to the datatable in clsdatamanager
                    post("Adding: " & ValFName & " " & ValLname & " " & ValNum & " " & ValProvider & " " & " to students datatable")
                    datamanager.AddStudentsRow(ValFName, ValLname, ValNum, ValProvider)
                Next
            End If
            If datamanager.GetRowsOfStudents = 1 Then
                dgStudents.SelectedIndex = -1
            End If
        Catch ex As Exception
            post(ex.Message)
        End Try
    End Sub
    Private Sub dgStudents_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles dgStudents.SelectionChanged
        Try
            Dim row As New DataGridRow
            Dim ValLname As String
            Dim ValNum As String
            Dim ValProvider As String
            'checks to see if expander3 is expanded and if this is the first time the program is shown.
            If Expander3.IsExpanded = False And first = False And dgStudents.SelectedIndex >= 0 Then
                'gets the current row
                row = GetRow(dgStudents, dgStudents.SelectedIndex.ToString)
                'gets specific column info
                ValLname = row.Item(2).ToString  'second column
                ValNum = row.Item(3).ToString   'third column
                ValProvider = row.Item(4).ToString 'fourth column
                'adds info to recipient datatable in clsdatamanage
                post("Adding Recipient: " & ValLname & " " & ValNum & " " & ValProvider)
                datamanager.AddRecipientRow(ValLname, ValNum, ValProvider)
                'resets the datacontext for the recipients data grid
                dgRecipients.DataContext = datamanager.setRecipients
                post("dgrecipients updated")
            End If
        Catch ex As Exception
            post(ex.Message)
        End Try
    End Sub
#End Region
#Region "Functions"
    Private Function PrintText(ByVal sender As Object)
        Dim lbsender As ListBox
        Dim li As DataRowView
        lbsender = CType(sender, ListBox)
        li = CType(lbsender.SelectedValue, DataRowView)
        If lbsender Is lbProvider Then
            Return li.Item(1).ToString
        Else
            Return li.Item(0).ToString
        End If
    End Function
    Public Function isNumeric(ByVal val As String, ByVal NumberStyle As System.Globalization.NumberStyles) As Boolean
        Dim result As [Double]
        Return [Double].TryParse(val, NumberStyle, System.Globalization.CultureInfo.CurrentCulture, result)
    End Function
    Private Function post(ByVal text As String, Optional ByVal first As Boolean = False)
        If first = True Then
            txtoutput.Text += text
            txtoutput.ScrollToEnd()
            txtoutput2.Text += text
            txtoutput2.ScrollToEnd()
        Else
            txtoutput.Text += vbNewLine & text
            txtoutput.ScrollToEnd()
            txtoutput2.Text += vbNewLine & text
            txtoutput2.ScrollToEnd()
        End If
        Return Nothing
    End Function
#End Region
#Region "Keyboard Focus"
    Private Sub txtFname_GotKeyboardFocus(ByVal sender As Object, ByVal e As System.Windows.Input.KeyboardFocusChangedEventArgs) Handles txtFname.GotKeyboardFocus
        KeyboardFocus(True, False, True, e)
    End Sub
    Private Sub txtFname_LostKeyboardFocus(ByVal sender As Object, ByVal e As System.Windows.Input.KeyboardFocusChangedEventArgs) Handles txtFname.LostKeyboardFocus
        KeyboardFocus(False, True, False, e)
    End Sub
    Private Sub txtLname_GotKeyboardFocus(ByVal sender As Object, ByVal e As System.Windows.Input.KeyboardFocusChangedEventArgs) Handles txtLname.GotKeyboardFocus
        KeyboardFocus(True, False, True, e)
    End Sub
    Private Sub txtLname_LostKeyboardFocus(ByVal sender As Object, ByVal e As System.Windows.Input.KeyboardFocusChangedEventArgs) Handles txtLname.LostKeyboardFocus
        KeyboardFocus(False, True, False, e)
    End Sub
    Private Sub txtAddNum_GotKeyboardFocus(ByVal sender As Object, ByVal e As System.Windows.Input.KeyboardFocusChangedEventArgs) Handles txtAddNum.GotKeyboardFocus
        KeyboardFocus(True, False, True, e)
    End Sub
    Private Sub txtAddNum_LostKeyboardFocus(ByVal sender As Object, ByVal e As System.Windows.Input.KeyboardFocusChangedEventArgs) Handles txtAddNum.LostKeyboardFocus
        KeyboardFocus(False, False, False, e)
    End Sub
    Private Sub txtNumber_GotKeyboardFocus(ByVal sender As Object, ByVal e As System.Windows.Input.KeyboardFocusChangedEventArgs) Handles txtNumber.GotKeyboardFocus
        KeyboardFocus(True, False, True, e)
    End Sub
    Private Sub txtNumber_LostKeyboardFocus(ByVal sender As Object, ByVal e As System.Windows.Input.KeyboardFocusChangedEventArgs) Handles txtNumber.LostKeyboardFocus
        KeyboardFocus(False, False, False, e)
    End Sub
    Private Sub txtMsg1_GotKeyboardFocus(ByVal sender As Object, ByVal e As System.Windows.Input.KeyboardFocusChangedEventArgs) Handles txtMsg1.GotKeyboardFocus
        KeyboardFocus(True, False, False, e)
    End Sub
    Private Sub txtMsg1_LostKeyboardFocus(ByVal sender As Object, ByVal e As System.Windows.Input.KeyboardFocusChangedEventArgs) Handles txtMsg1.LostKeyboardFocus
        KeyboardFocus(False, False, False, e)
    End Sub
    Private Sub txtMsg2_GotKeyboardFocus(ByVal sender As Object, ByVal e As System.Windows.Input.KeyboardFocusChangedEventArgs) Handles txtMsg2.GotKeyboardFocus
        KeyboardFocus(True, False, False, e)
    End Sub
    Private Sub txtMsg2_LostKeyboardFocus(ByVal sender As Object, ByVal e As System.Windows.Input.KeyboardFocusChangedEventArgs) Handles txtMsg2.LostKeyboardFocus
        KeyboardFocus(False, False, False, e)
    End Sub
    Private Sub txtMsg3_GotKeyboardFocus(ByVal sender As Object, ByVal e As System.Windows.Input.KeyboardFocusChangedEventArgs) Handles txtMsg3.GotKeyboardFocus
        KeyboardFocus(True, False, False, e)
    End Sub
    Private Sub txtMsg3_LostKeyboardFocus(ByVal sender As Object, ByVal e As System.Windows.Input.KeyboardFocusChangedEventArgs) Handles txtMsg3.LostKeyboardFocus
        KeyboardFocus(False, False, False, e)
    End Sub
#End Region

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        My.Settings.Reset()
    End Sub


End Class
