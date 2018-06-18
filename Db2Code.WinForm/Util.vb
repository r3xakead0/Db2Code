Public Class Util

    Public Shared Sub PointerLoad(ByVal form As Form)
        form.Cursor = Cursors.WaitCursor
    End Sub

    Public Shared Sub PointerReady(ByVal form As Form)
        form.Cursor = Cursors.Arrow
    End Sub

    Public Shared Sub ErrorMessage(ByVal title As String, ByVal message As String)
        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub

    Public Shared Sub CriticalMessage(ByVal title As String, ByVal message As String)
        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Public Shared Sub InformationMessage(ByVal title As String, ByVal message As String)
        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Public Shared Function ConfirmationMessage(ByVal title As String, ByVal message As String) As Boolean
        Dim rpta As DialogResult = MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Information)

        If rpta = DialogResult.Yes Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Sub FormatDatagridview(ByRef dgv As DataGridView)
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy
        dgv.[ReadOnly] = True
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv.AllowUserToAddRows = False
        dgv.AllowUserToDeleteRows = False
        dgv.MultiSelect = False
        dgv.AllowDrop = False
        dgv.RowHeadersVisible = False
    End Sub

    Public Shared Sub AutoWidthColumn(ByRef dgv As DataGridView, ByVal columnName As String, ByVal Optional scroll As Boolean = True)
        Dim width As Integer = dgv.Width - (If(scroll = True, 20, 3))
        If dgv.RowHeadersVisible = True Then width = width - dgv.RowHeadersWidth

        For c As Integer = 0 To dgv.ColumnCount - 1

            If c <> dgv.Columns(columnName).Index Then

                If dgv.Columns(c).Visible Then
                    width = width - dgv.Columns(c).Width
                End If
            End If
        Next

        dgv.Columns(columnName).Width = width
    End Sub

End Class
