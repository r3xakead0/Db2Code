Imports System.Globalization
Imports System.ComponentModel
Imports System.Reflection
Imports MaterialSkin
Imports Db2Code.Library

Public Class FrmMain

    Public Shared ReadOnly Property Instance As FrmMain
        Get
            Static _instance As FrmMain = New FrmMain
            Return _instance
        End Get
    End Property

    Private database As Mssql = Nothing
    Private dtTablas As DataTable = Nothing
    Private bSelected As Boolean = False

#Region "Form methods"

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            'Skin de formulario
            Dim SkinManager As MaterialSkinManager = MaterialSkinManager.Instance
            SkinManager.AddFormToManage(Me)
            SkinManager.Theme = MaterialSkinManager.Themes.LIGHT
            SkinManager.ColorScheme = New ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE)

            'Cambiar lenguaje de formulario
            Dim lenguaje As String = CultureInfo.CurrentCulture.Parent.Name
            Me.SuspendLayout()
            Dim resources As ComponentResourceManager = New ComponentResourceManager(GetType(FrmMain))
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

            'Mostrar la version del sofware
            Me.lblVersion.Text = $"Version {Assembly.GetExecutingAssembly().GetName().Version.ToString()}"

            'Load table 
            dtTablas = New DataTable
            dtTablas.Columns.Add("Id", System.Type.GetType("System.Int32"))
            dtTablas.Columns.Add("Nombre", System.Type.GetType("System.String"))

            For i As Integer = 0 To 10
                Dim drTabla As DataRow = Me.dtTablas.NewRow
                drTabla.Item("Id") = 0
                drTabla.Item("Nombre") = ""
                Me.dtTablas.Rows.Add(drTabla)
            Next

            Dim chkEdit As New DataGridViewCheckBoxColumn
            With chkEdit
                .TrueValue = 1
                .FalseValue = 0
                .Name = ""
                .ReadOnly = False
                .Width = 20
            End With
            Me.dgvTables.Columns.Insert(0, chkEdit)

            For Each col As DataGridViewColumn In Me.dgvTables.Columns
                col.SortMode = DataGridViewColumnSortMode.NotSortable
            Next


            Me.dgvTables.DataSource = Me.dtTablas
            Util.FormatDatagridview(Me.dgvTables)

            With Me.dgvTables
                .ReadOnly = False

                .Columns("Id").Visible = False
                .Columns("Id").ReadOnly = True

                .Columns("Nombre").Width = 400
                .Columns("Nombre").Visible = True
                .Columns("Nombre").ReadOnly = True
            End With

            Util.AutoWidthColumn(Me.dgvTables, "Nombre")

        Catch ex As Exception
            Util.ErrorMessage(Me.Text, ex.Message)
        End Try
    End Sub

    Private Sub btnGenerate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Try

            'Validaciones
            '=====================================================================================================
            If Me.txtTargetFolder.TextLength = 0 Then Throw New Exception("No ingreso la Carpeta Destino donde se generaran los archivo, porfavor verifique.")

            If Me.txtServer.TextLength = 0 Then Throw New Exception("No ingreso el Servidor de Base de Datos, porfavor verifique.")
            If Me.txtUsername.TextLength = 0 Then Throw New Exception("No ingreso el Usuario de Base de Datos, porfavor verifique.")
            If Me.txtPassword.TextLength = 0 Then Throw New Exception("No ingreso la Clave de Base de Datos, porfavor verifique.")

            If Me.cboDatabases.DataSource Is Nothing Then Throw New Exception("No se cargo la lista de Base de Datos, porfavor verifique.")
            If Me.cboDatabases.SelectedIndex = 0 Then Throw New Exception("No se selecciono la Base de Datos, porfavor verifique.")
            '=====================================================================================================

            'Me.strConexion = "Data Source=" & Me.txtServer.Text & ";Initial Catalog=" & CType(Me.cboDatabases.SelectedItem, DataRowView).Item(1).ToString & ";User Id=" & Me.txtUsername.Text & ";Password=" & Me.txtPassword.Text & ";"

            Util.PointerLoad(Me)

            Me.txtConsole.Clear()

            Dim generate As IGenerate
            Dim ruta As String = Me.txtTargetFolder.Text.Trim()

            If Me.rdbVBNET.Checked Then
                generate = New Vbnet(ruta, Me.database)
            ElseIf Me.rdbCSHARP.Checked Then
                generate = New Csharp(ruta, Me.database)
            Else
                Throw New Exception("No existe el lenguaje seleccionado, porfavor verifique.")
            End If

            Me.txtConsole.Text = Me.txtConsole.Text & "Inicio de la Generacion de Codigo " & vbCrLf


            Dim lstSelectedTables As New List(Of Poco.Table)
            For Each drv As DataGridViewRow In Me.dgvTables.Rows
                If CType(drv.Cells(0).Value, Boolean) = True Then 'Si esta con CHECK

                    Dim objSelectedTable As New Poco.Table()
                    objSelectedTable.Id = CInt(drv.Cells("Id").Value)
                    objSelectedTable.Name = drv.Cells("Nombre").Value.ToString

                    lstSelectedTables.Add(objSelectedTable)
                End If
            Next


            For Each objSelectedTable As Poco.Table In lstSelectedTables

                If Me.chkGenerarSql.Checked Then
                    Me.database.StoredProcedure(objSelectedTable, ruta)
                End If

                If Me.chkGenerarBe.Checked Then
                    generate.BusinessEntity(objSelectedTable)
                End If

                If Me.chkGenerarLn.Checked Then
                    generate.BusinessLogic(objSelectedTable)
                End If

                If Me.chkGenerarForm.Checked Then
                    generate.WindowsForm(objSelectedTable)
                End If

            Next

            If Me.chkGenerarSql.Checked Then Me.txtConsole.Text = Me.txtConsole.Text & "- Se genero scripts de SQL" & vbCrLf
            If Me.chkGenerarBe.Checked Then Me.txtConsole.Text = Me.txtConsole.Text & "- Se genero clases de BE" & vbCrLf
            If Me.chkGenerarLn.Checked Then Me.txtConsole.Text = Me.txtConsole.Text & "- Se genero clases de LN" & vbCrLf
            If Me.chkGenerarForm.Checked Then Me.txtConsole.Text = Me.txtConsole.Text & "- Se genero clases de Formularios" & vbCrLf

            Me.txtConsole.Text = Me.txtConsole.Text & "Fin de la Generacion de Codigo"

        Catch ex As Exception
            Util.ErrorMessage(Me.Text, ex.Message)
        Finally
            Util.PointerReady(Me)
        End Try
    End Sub

    Private Sub btnBaseDatos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDatabases.Click
        Try

            If Me.txtServer.TextLength = 0 Then Throw New Exception("No ingreso el Servidor de Base de Datos, porfavor verifique.")
            If Me.txtUsername.TextLength = 0 Then Throw New Exception("No ingreso el Usuario de Base de Datos, porfavor verifique.")
            If Me.txtPassword.TextLength = 0 Then Throw New Exception("No ingreso la Clave de Base de Datos, porfavor verifique.")

            Util.PointerLoad(Me)

            Dim strServer As String = Me.txtServer.Text

            If Me.chkAutentification.Checked Then
                database = New Mssql(strServer)
            Else
                Dim strUser As String = Me.txtUsername.Text
                Dim strPass As String = Me.txtPassword.Text
                database = New Mssql(strServer, strUser, strPass)
            End If

            Dim lstDatabases As List(Of Poco.DataBase) = database.GetDatabases()
            Dim objDatabase As New Poco.DataBase()
            objDatabase.Id = 0
            objDatabase.Name = "Seleccione una Base de Datos"
            lstDatabases.Insert(0, objDatabase)

            Me.cboDatabases.DataSource = lstDatabases
            Me.cboDatabases.DisplayMember = "Name"
            Me.cboDatabases.ValueMember = "Id"

        Catch ex As Exception
            Util.ErrorMessage(Me.Text, ex.Message)
        Finally
            Util.PointerReady(Me)
        End Try
    End Sub

    Private Sub btnCarpetaDestino_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTargetFolder.Click
        Try

            With Me.fbdCarpetaDestino
                .RootFolder = Environment.SpecialFolder.Desktop
                .Description = "Seleccione la Carpeta Destino"
                If .ShowDialog = DialogResult.OK Then
                    Me.txtTargetFolder.Text = .SelectedPath
                End If
            End With

        Catch ex As Exception
            Util.ErrorMessage(Me.Text, ex.Message)
        End Try
    End Sub

    Private Sub chkAutentificacion_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAutentification.CheckedChanged
        Try
            Me.txtUsername.ReadOnly = Me.chkAutentification.Checked
            Me.txtPassword.ReadOnly = Me.chkAutentification.Checked
        Catch ex As Exception
            Util.ErrorMessage(Me.Text, ex.Message)
        End Try
    End Sub

    Private Sub cboBaseDatos_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboDatabases.SelectionChangeCommitted
        Try

            Me.bSelected = False

            Util.PointerLoad(Me)

            Me.dtTablas.Rows.Clear()

            If Me.cboDatabases.SelectedIndex = 0 Then

                For i As Integer = 0 To 10
                    Dim drTabla As DataRow = Me.dtTablas.NewRow
                    drTabla.Item("Id") = 0
                    drTabla.Item("Nombre") = ""
                    Me.dtTablas.Rows.Add(drTabla)
                Next

                Me.dtTablas.AcceptChanges()

                Me.txtRowsCount.Text = "0"

            Else

                Dim objDatabase As Poco.DataBase = CType(Me.cboDatabases.SelectedItem, Poco.DataBase)
                Dim lstTables As List(Of Poco.Table) = Me.database.GetTables(objDatabase)

                If lstTables.Count > 0 Then

                    For Each objTable As Poco.Table In lstTables
                        Dim drTablas As DataRow = Me.dtTablas.NewRow
                        drTablas.Item("Id") = objTable.Id
                        drTablas.Item("Nombre") = objTable.Name
                        Me.dtTablas.Rows.Add(drTablas)
                    Next

                    Me.dtTablas.AcceptChanges()

                    Me.txtRowsCount.Text = lstTables.Count.ToString()

                Else

                    For i As Integer = 0 To 10
                        Dim drTabla As DataRow = Me.dtTablas.NewRow
                        drTabla.Item("Id") = 0
                        drTabla.Item("Nombre") = ""
                        Me.dtTablas.Rows.Add(drTabla)
                    Next

                    Me.dtTablas.AcceptChanges()

                    Me.txtRowsCount.Text = "0"

                End If

            End If

        Catch ex As Exception
            Util.ErrorMessage(Me.Text, ex.Message)
        Finally
            Util.PointerReady(Me)
        End Try
    End Sub

    Private Sub btnServidores_Click(sender As Object, e As EventArgs) Handles btnServer.Click
        Try
            Dim frm As FrmServers = FrmServers.Instance
            If frm.ShowDialog() = DialogResult.OK Then
                Me.txtServer.Text = frm.Server
            End If
        Catch ex As Exception
            Util.ErrorMessage(Me.Text, ex.Message)
        Finally
            Util.PointerReady(Me)
        End Try
    End Sub

    Private Sub FrmMain_ResizeEnd(sender As Object, e As EventArgs) Handles MyBase.ResizeEnd
        Try
            If Not Me.dgvTables.DataSource Is Nothing Then
                Util.AutoWidthColumn(Me.dgvTables, "Nombre")
            End If
        Catch ex As Exception
            Util.ErrorMessage(Me.Text, ex.Message)
        End Try
    End Sub

    Private Sub btnSelectAll_Click(sender As Object, e As EventArgs) Handles btnSelectAll.Click
        Try

            Me.bSelected = Not Me.bSelected

            For Each drv As DataGridViewRow In Me.dgvTables.Rows
                drv.Cells(0).Value = Me.bSelected
            Next

        Catch ex As Exception
            Util.ErrorMessage(Me.Text, ex.Message)
        End Try
    End Sub

#End Region

End Class
