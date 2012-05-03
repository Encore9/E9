
Imports System.Data



Public Class clsDataTables


    Public ds As New DataSet
    Private Recipients As New DataTable
    Private students As New DataTable
    Public Sub New()
        Recipients.Columns.Add("Lname")
        Recipients.Columns.Add("Number")
        Recipients.Columns.Add("Provider")
        Recipients.TableName = "recipients"
        students.Columns.Add("Fname")
        students.Columns.Add("Lname")
        students.Columns.Add("Number")
        students.Columns.Add("Provider")
        students.TableName = "students"
        ds.Tables.Add(Recipients)
        ds.Tables.Add(students)

    End Sub
    Public Sub AddRecipientRow(ByVal name As String, ByVal num As String, ByVal provider As String)

        Dim datarow As DataRow = Recipients.NewRow()
        datarow(0) = name
        datarow(1) = num
        datarow(2) = provider
        Recipients.Rows.Add(datarow)
    End Sub
    Public Sub EmptyRecipients()
        Recipients.Rows.Clear()
    End Sub
    ReadOnly Property setRecipients()
        Get
            setRecipients = ds.Tables("recipients").DefaultView
        End Get
    End Property
    ReadOnly Property RowCount
        Get
            RowCount = ds.Tables("recipients").Rows.Count
        End Get
    End Property
    ReadOnly Property GetNum(ByVal row) As String
        Get
            GetNum = ds.Tables("recipients").Rows(row).Item(1)
        End Get

    End Property
    ReadOnly Property GetAddress(ByVal row) As String
        Get
            GetAddress = ds.Tables("recipients").Rows(row).Item(1) & ds.Tables("recipients").Rows(row).Item(2)
        End Get
    End Property
    ReadOnly Property hasRows As Boolean
        Get
            If ds.Tables("recipients").Rows(0).IsNull(0) = True Then
                hasRows = False
            Else
                hasRows = True
            End If
        End Get
    End Property
    Public Sub AddStudentsRow(ByVal Fname As String, ByVal Lname As String, ByVal number As String, ByVal provider As String)
        Dim datarow As DataRow = students.NewRow()
        datarow(0) = Fname
        datarow(1) = Lname
        datarow(2) = number
        datarow(3) = provider
        students.Rows.Add(datarow)
    End Sub
    ReadOnly Property setStudents()
        Get
            setStudents = ds.Tables("students").DefaultView
        End Get
    End Property
    ReadOnly Property GetRowsOfStudents As String
        Get
            GetRowsOfStudents = ds.Tables("students").Rows.Count
        End Get

    End Property
End Class
