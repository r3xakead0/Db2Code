Imports System.ComponentModel
Imports System.Globalization
Imports MaterialSkin

Public Class FrmServers

    Public Shared ReadOnly Property Instance As FrmServers
        Get
            Static _instance As FrmServers = New FrmServers
            Return _instance
        End Get
    End Property

    Public Property Server As String = ""

#Region "Form Methods"

    Private Sub FrmServers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try

            'Skin de formulario
            Dim SkinManager As MaterialSkinManager = MaterialSkinManager.Instance
            SkinManager.AddFormToManage(Me)
            SkinManager.Theme = MaterialSkinManager.Themes.LIGHT
            SkinManager.ColorScheme = New ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE)

            'Cambiar lenguaje de formulario
            Dim lenguaje As String = CultureInfo.CurrentCulture.Parent.Name
            Me.SuspendLayout()
            Dim resources As ComponentResourceManager = New ComponentResourceManager(GetType(FrmServers))
            resources.ApplyResources(Me, "$this", New CultureInfo(lenguaje))
            For Each c As Control In Me.Controls
                resources.ApplyResources(c, c.Name, New CultureInfo(lenguaje))
                If TypeOf (c) Is GroupBox Then
                    For Each cg As Control In CType(c, GroupBox).Controls
                        resources.ApplyResources(cg, cg.Name, New CultureInfo(lenguaje))
                    Next
                End If
            Next c
            Me.ResumeLayout()

            'Load data in grid
            Me.LoadGrid()

        Catch ex As Exception
            Util.ErrorMessage(Me.Text, ex.Message)
        End Try
    End Sub

    Private Sub btnSelect_Click(sender As Object, e As EventArgs) Handles btnSelect.Click
        Try

            Dim server As String = ""

            If Not Me.dgvServers.CurrentRow Is Nothing Then

                Dim servername As String = Me.dgvServers.CurrentRow.Cells("ServerName").Value.ToString()
                Dim instancename As String = Me.dgvServers.CurrentRow.Cells("InstanceName").Value.ToString()

                If instancename.Length > 0 Then
                    server = $"{servername}\{instancename}"
                Else
                    server = servername
                End If

            End If

            Me.Server = server
            Me.DialogResult = DialogResult.OK

        Catch ex As Exception
            Util.ErrorMessage(Me.Text, ex.Message)
        End Try
    End Sub

    Private Sub dgvServers_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvServers.CellDoubleClick
        Try

            Dim server As String = ""

            Dim servername As String = Me.dgvServers.Item("ServerName", e.RowIndex).Value.ToString()
            Dim instancename As String = Me.dgvServers.Item("InstanceName", e.RowIndex).Value.ToString()

            If instancename.Length > 0 Then
                server = $"{servername}\{instancename}"
            Else
                server = servername
            End If

            Me.Server = server
            Me.DialogResult = DialogResult.OK

        Catch ex As Exception
            Util.ErrorMessage(Me.Text, ex.Message)
        End Try
    End Sub

    Private Sub FrmServers_ResizeEnd(sender As Object, e As EventArgs) Handles MyBase.ResizeEnd
        Try
            If Not Me.dgvServers.DataSource Is Nothing Then
                Util.AutoWidthColumn(Me.dgvServers, "ServerName")
            End If
        Catch ex As Exception
            Util.ErrorMessage(Me.Text, ex.Message)
        End Try
    End Sub

#End Region

    Private Sub LoadGrid()
        Try

            Dim dtServers As DataTable = System.Data.Sql.SqlDataSourceEnumerator.Instance.GetDataSources()
            Me.dgvServers.DataSource = dtServers

            Util.FormatDatagridview(Me.dgvServers)

            With Me.dgvServers
                .Columns("ServerName").Width = 120
                .Columns("ServerName").Visible = True
                .Columns("ServerName").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Columns("InstanceName").Width = 120
                .Columns("InstanceName").Visible = True
                .Columns("InstanceName").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Columns("IsClustered").Width = 0
                .Columns("IsClustered").Visible = False
                .Columns("Version").Width = 120
                .Columns("Version").Visible = True
                .Columns("Version").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End With

            Util.AutoWidthColumn(Me.dgvServers, "ServerName")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

End Class