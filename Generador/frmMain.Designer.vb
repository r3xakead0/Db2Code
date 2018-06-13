<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits MaterialSkin.Controls.MaterialForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.txtTexto = New System.Windows.Forms.TextBox()
        Me.imlIconos = New System.Windows.Forms.ImageList(Me.components)
        Me.fbdCarpetaDestino = New System.Windows.Forms.FolderBrowserDialog()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnCarpetaDestino = New System.Windows.Forms.Button()
        Me.btnBaseDatos = New System.Windows.Forms.Button()
        Me.btnGenerar = New System.Windows.Forms.Button()
        Me.chkGenerarBe = New System.Windows.Forms.CheckBox()
        Me.chkGenerarLn = New System.Windows.Forms.CheckBox()
        Me.chkGenerarSql = New System.Windows.Forms.CheckBox()
        Me.chkAutentificacion = New System.Windows.Forms.CheckBox()
        Me.txtServidor = New System.Windows.Forms.TextBox()
        Me.txtUsuario = New System.Windows.Forms.TextBox()
        Me.txtClave = New System.Windows.Forms.TextBox()
        Me.txtCarpetaDestino = New System.Windows.Forms.TextBox()
        Me.cboBaseDatos = New System.Windows.Forms.ComboBox()
        Me.rdbVBNET = New System.Windows.Forms.RadioButton()
        Me.rdbCSHARP = New System.Windows.Forms.RadioButton()
        Me.grpLenguaje = New System.Windows.Forms.GroupBox()
        Me.grpCodigo = New System.Windows.Forms.GroupBox()
        Me.grpTablas = New System.Windows.Forms.GroupBox()
        Me.dgvTablas = New System.Windows.Forms.DataGridView()
        Me.grpConfiguracion = New System.Windows.Forms.GroupBox()
        Me.btnServidores = New System.Windows.Forms.Button()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.lblContact = New System.Windows.Forms.Label()
        Me.grpLenguaje.SuspendLayout()
        Me.grpCodigo.SuspendLayout()
        Me.grpTablas.SuspendLayout()
        CType(Me.dgvTablas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpConfiguracion.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtTexto
        '
        resources.ApplyResources(Me.txtTexto, "txtTexto")
        Me.txtTexto.BackColor = System.Drawing.Color.Black
        Me.txtTexto.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.txtTexto.Name = "txtTexto"
        Me.txtTexto.ReadOnly = True
        '
        'imlIconos
        '
        Me.imlIconos.ImageStream = CType(resources.GetObject("imlIconos.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imlIconos.TransparentColor = System.Drawing.Color.Transparent
        Me.imlIconos.Images.SetKeyName(0, "folder.ico")
        Me.imlIconos.Images.SetKeyName(1, "load.ico")
        Me.imlIconos.Images.SetKeyName(2, "View.png")
        '
        'fbdCarpetaDestino
        '
        resources.ApplyResources(Me.fbdCarpetaDestino, "fbdCarpetaDestino")
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        '
        'btnCarpetaDestino
        '
        resources.ApplyResources(Me.btnCarpetaDestino, "btnCarpetaDestino")
        Me.btnCarpetaDestino.ImageList = Me.imlIconos
        Me.btnCarpetaDestino.Name = "btnCarpetaDestino"
        Me.btnCarpetaDestino.UseVisualStyleBackColor = True
        '
        'btnBaseDatos
        '
        resources.ApplyResources(Me.btnBaseDatos, "btnBaseDatos")
        Me.btnBaseDatos.ImageList = Me.imlIconos
        Me.btnBaseDatos.Name = "btnBaseDatos"
        Me.btnBaseDatos.UseVisualStyleBackColor = True
        '
        'btnGenerar
        '
        resources.ApplyResources(Me.btnGenerar, "btnGenerar")
        Me.btnGenerar.Name = "btnGenerar"
        Me.btnGenerar.UseVisualStyleBackColor = True
        '
        'chkGenerarBe
        '
        resources.ApplyResources(Me.chkGenerarBe, "chkGenerarBe")
        Me.chkGenerarBe.Name = "chkGenerarBe"
        Me.chkGenerarBe.UseVisualStyleBackColor = True
        '
        'chkGenerarLn
        '
        resources.ApplyResources(Me.chkGenerarLn, "chkGenerarLn")
        Me.chkGenerarLn.Name = "chkGenerarLn"
        Me.chkGenerarLn.UseVisualStyleBackColor = True
        '
        'chkGenerarSql
        '
        resources.ApplyResources(Me.chkGenerarSql, "chkGenerarSql")
        Me.chkGenerarSql.Name = "chkGenerarSql"
        Me.chkGenerarSql.UseVisualStyleBackColor = True
        '
        'chkAutentificacion
        '
        resources.ApplyResources(Me.chkAutentificacion, "chkAutentificacion")
        Me.chkAutentificacion.Name = "chkAutentificacion"
        Me.chkAutentificacion.UseVisualStyleBackColor = True
        '
        'txtServidor
        '
        resources.ApplyResources(Me.txtServidor, "txtServidor")
        Me.txtServidor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtServidor.Name = "txtServidor"
        '
        'txtUsuario
        '
        resources.ApplyResources(Me.txtUsuario, "txtUsuario")
        Me.txtUsuario.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtUsuario.Name = "txtUsuario"
        '
        'txtClave
        '
        resources.ApplyResources(Me.txtClave, "txtClave")
        Me.txtClave.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtClave.Name = "txtClave"
        '
        'txtCarpetaDestino
        '
        resources.ApplyResources(Me.txtCarpetaDestino, "txtCarpetaDestino")
        Me.txtCarpetaDestino.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCarpetaDestino.Name = "txtCarpetaDestino"
        Me.txtCarpetaDestino.ReadOnly = True
        '
        'cboBaseDatos
        '
        resources.ApplyResources(Me.cboBaseDatos, "cboBaseDatos")
        Me.cboBaseDatos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBaseDatos.FormattingEnabled = True
        Me.cboBaseDatos.Name = "cboBaseDatos"
        '
        'rdbVBNET
        '
        resources.ApplyResources(Me.rdbVBNET, "rdbVBNET")
        Me.rdbVBNET.Checked = True
        Me.rdbVBNET.Name = "rdbVBNET"
        Me.rdbVBNET.TabStop = True
        Me.rdbVBNET.UseVisualStyleBackColor = True
        '
        'rdbCSHARP
        '
        resources.ApplyResources(Me.rdbCSHARP, "rdbCSHARP")
        Me.rdbCSHARP.Name = "rdbCSHARP"
        Me.rdbCSHARP.TabStop = True
        Me.rdbCSHARP.UseVisualStyleBackColor = True
        '
        'grpLenguaje
        '
        resources.ApplyResources(Me.grpLenguaje, "grpLenguaje")
        Me.grpLenguaje.Controls.Add(Me.rdbVBNET)
        Me.grpLenguaje.Controls.Add(Me.rdbCSHARP)
        Me.grpLenguaje.Name = "grpLenguaje"
        Me.grpLenguaje.TabStop = False
        '
        'grpCodigo
        '
        resources.ApplyResources(Me.grpCodigo, "grpCodigo")
        Me.grpCodigo.Controls.Add(Me.chkGenerarSql)
        Me.grpCodigo.Controls.Add(Me.chkGenerarLn)
        Me.grpCodigo.Controls.Add(Me.chkGenerarBe)
        Me.grpCodigo.Name = "grpCodigo"
        Me.grpCodigo.TabStop = False
        '
        'grpTablas
        '
        resources.ApplyResources(Me.grpTablas, "grpTablas")
        Me.grpTablas.Controls.Add(Me.dgvTablas)
        Me.grpTablas.Name = "grpTablas"
        Me.grpTablas.TabStop = False
        '
        'dgvTablas
        '
        resources.ApplyResources(Me.dgvTablas, "dgvTablas")
        Me.dgvTablas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvTablas.Name = "dgvTablas"
        '
        'grpConfiguracion
        '
        resources.ApplyResources(Me.grpConfiguracion, "grpConfiguracion")
        Me.grpConfiguracion.Controls.Add(Me.btnServidores)
        Me.grpConfiguracion.Controls.Add(Me.chkAutentificacion)
        Me.grpConfiguracion.Controls.Add(Me.Label1)
        Me.grpConfiguracion.Controls.Add(Me.Label2)
        Me.grpConfiguracion.Controls.Add(Me.Label3)
        Me.grpConfiguracion.Controls.Add(Me.Label4)
        Me.grpConfiguracion.Controls.Add(Me.cboBaseDatos)
        Me.grpConfiguracion.Controls.Add(Me.Label5)
        Me.grpConfiguracion.Controls.Add(Me.txtCarpetaDestino)
        Me.grpConfiguracion.Controls.Add(Me.btnCarpetaDestino)
        Me.grpConfiguracion.Controls.Add(Me.txtClave)
        Me.grpConfiguracion.Controls.Add(Me.btnBaseDatos)
        Me.grpConfiguracion.Controls.Add(Me.txtUsuario)
        Me.grpConfiguracion.Controls.Add(Me.txtServidor)
        Me.grpConfiguracion.Name = "grpConfiguracion"
        Me.grpConfiguracion.TabStop = False
        '
        'btnServidores
        '
        resources.ApplyResources(Me.btnServidores, "btnServidores")
        Me.btnServidores.ImageList = Me.imlIconos
        Me.btnServidores.Name = "btnServidores"
        Me.btnServidores.UseVisualStyleBackColor = True
        '
        'lblVersion
        '
        resources.ApplyResources(Me.lblVersion, "lblVersion")
        Me.lblVersion.Name = "lblVersion"
        '
        'lblContact
        '
        resources.ApplyResources(Me.lblContact, "lblContact")
        Me.lblContact.Name = "lblContact"
        '
        'frmMain
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.lblContact)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.grpConfiguracion)
        Me.Controls.Add(Me.grpTablas)
        Me.Controls.Add(Me.grpCodigo)
        Me.Controls.Add(Me.grpLenguaje)
        Me.Controls.Add(Me.btnGenerar)
        Me.Controls.Add(Me.txtTexto)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMain"
        Me.grpLenguaje.ResumeLayout(False)
        Me.grpLenguaje.PerformLayout()
        Me.grpCodigo.ResumeLayout(False)
        Me.grpCodigo.PerformLayout()
        Me.grpTablas.ResumeLayout(False)
        CType(Me.dgvTablas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpConfiguracion.ResumeLayout(False)
        Me.grpConfiguracion.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtTexto As System.Windows.Forms.TextBox
    Friend WithEvents fbdCarpetaDestino As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents imlIconos As System.Windows.Forms.ImageList
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnCarpetaDestino As System.Windows.Forms.Button
    Friend WithEvents btnBaseDatos As System.Windows.Forms.Button
    Friend WithEvents btnGenerar As System.Windows.Forms.Button
    Friend WithEvents chkGenerarBe As System.Windows.Forms.CheckBox
    Friend WithEvents chkGenerarLn As System.Windows.Forms.CheckBox
    Friend WithEvents chkGenerarSql As System.Windows.Forms.CheckBox
    Friend WithEvents chkAutentificacion As System.Windows.Forms.CheckBox
    Friend WithEvents txtServidor As System.Windows.Forms.TextBox
    Friend WithEvents txtUsuario As System.Windows.Forms.TextBox
    Friend WithEvents txtClave As System.Windows.Forms.TextBox
    Friend WithEvents txtCarpetaDestino As System.Windows.Forms.TextBox
    Friend WithEvents cboBaseDatos As System.Windows.Forms.ComboBox
    Friend WithEvents rdbVBNET As System.Windows.Forms.RadioButton
    Friend WithEvents rdbCSHARP As System.Windows.Forms.RadioButton
    Friend WithEvents grpLenguaje As System.Windows.Forms.GroupBox
    Friend WithEvents grpCodigo As System.Windows.Forms.GroupBox
    Friend WithEvents grpTablas As System.Windows.Forms.GroupBox
    Friend WithEvents dgvTablas As System.Windows.Forms.DataGridView
    Friend WithEvents grpConfiguracion As System.Windows.Forms.GroupBox
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    Friend WithEvents btnServidores As System.Windows.Forms.Button
    Friend WithEvents lblContact As Label
End Class
