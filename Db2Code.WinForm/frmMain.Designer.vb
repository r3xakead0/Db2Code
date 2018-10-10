<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmMain
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmMain))
        Me.txtConsole = New System.Windows.Forms.TextBox()
        Me.imlIconos = New System.Windows.Forms.ImageList(Me.components)
        Me.fbdCarpetaDestino = New System.Windows.Forms.FolderBrowserDialog()
        Me.lblTargetFolder = New System.Windows.Forms.Label()
        Me.lblServer = New System.Windows.Forms.Label()
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.lblPassword = New System.Windows.Forms.Label()
        Me.lblDatabases = New System.Windows.Forms.Label()
        Me.btnTargetFolder = New System.Windows.Forms.Button()
        Me.btnDatabases = New System.Windows.Forms.Button()
        Me.btnGenerate = New System.Windows.Forms.Button()
        Me.chkGenerarBe = New System.Windows.Forms.CheckBox()
        Me.chkGenerarLn = New System.Windows.Forms.CheckBox()
        Me.chkGenerarSql = New System.Windows.Forms.CheckBox()
        Me.chkAutentification = New System.Windows.Forms.CheckBox()
        Me.txtServer = New System.Windows.Forms.TextBox()
        Me.txtUsername = New System.Windows.Forms.TextBox()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.txtTargetFolder = New System.Windows.Forms.TextBox()
        Me.cboDatabases = New System.Windows.Forms.ComboBox()
        Me.rdbVBNET = New System.Windows.Forms.RadioButton()
        Me.rdbCSHARP = New System.Windows.Forms.RadioButton()
        Me.grpLanguage = New System.Windows.Forms.GroupBox()
        Me.grpCode = New System.Windows.Forms.GroupBox()
        Me.chkGenerarForm = New System.Windows.Forms.CheckBox()
        Me.grpTables = New System.Windows.Forms.GroupBox()
        Me.lblRowsCount = New System.Windows.Forms.Label()
        Me.btnSelectAll = New System.Windows.Forms.Button()
        Me.txtRowsCount = New System.Windows.Forms.TextBox()
        Me.dgvTables = New System.Windows.Forms.DataGridView()
        Me.grpConfiguration = New System.Windows.Forms.GroupBox()
        Me.btnServer = New System.Windows.Forms.Button()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.lblContact = New System.Windows.Forms.Label()
        Me.txtContact = New System.Windows.Forms.TextBox()
        Me.grpLanguage.SuspendLayout()
        Me.grpCode.SuspendLayout()
        Me.grpTables.SuspendLayout()
        CType(Me.dgvTables, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpConfiguration.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtConsole
        '
        resources.ApplyResources(Me.txtConsole, "txtConsole")
        Me.txtConsole.BackColor = System.Drawing.Color.Black
        Me.txtConsole.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.txtConsole.Name = "txtConsole"
        Me.txtConsole.ReadOnly = True
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
        'lblTargetFolder
        '
        resources.ApplyResources(Me.lblTargetFolder, "lblTargetFolder")
        Me.lblTargetFolder.Name = "lblTargetFolder"
        '
        'lblServer
        '
        resources.ApplyResources(Me.lblServer, "lblServer")
        Me.lblServer.Name = "lblServer"
        '
        'lblUsername
        '
        resources.ApplyResources(Me.lblUsername, "lblUsername")
        Me.lblUsername.Name = "lblUsername"
        '
        'lblPassword
        '
        resources.ApplyResources(Me.lblPassword, "lblPassword")
        Me.lblPassword.Name = "lblPassword"
        '
        'lblDatabases
        '
        resources.ApplyResources(Me.lblDatabases, "lblDatabases")
        Me.lblDatabases.Name = "lblDatabases"
        '
        'btnTargetFolder
        '
        resources.ApplyResources(Me.btnTargetFolder, "btnTargetFolder")
        Me.btnTargetFolder.ImageList = Me.imlIconos
        Me.btnTargetFolder.Name = "btnTargetFolder"
        Me.btnTargetFolder.UseVisualStyleBackColor = True
        '
        'btnDatabases
        '
        resources.ApplyResources(Me.btnDatabases, "btnDatabases")
        Me.btnDatabases.ImageList = Me.imlIconos
        Me.btnDatabases.Name = "btnDatabases"
        Me.btnDatabases.UseVisualStyleBackColor = True
        '
        'btnGenerate
        '
        resources.ApplyResources(Me.btnGenerate, "btnGenerate")
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.UseVisualStyleBackColor = True
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
        'chkAutentification
        '
        resources.ApplyResources(Me.chkAutentification, "chkAutentification")
        Me.chkAutentification.Name = "chkAutentification"
        Me.chkAutentification.UseVisualStyleBackColor = True
        '
        'txtServer
        '
        resources.ApplyResources(Me.txtServer, "txtServer")
        Me.txtServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtServer.Name = "txtServer"
        '
        'txtUsername
        '
        resources.ApplyResources(Me.txtUsername, "txtUsername")
        Me.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtUsername.Name = "txtUsername"
        '
        'txtPassword
        '
        resources.ApplyResources(Me.txtPassword, "txtPassword")
        Me.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPassword.Name = "txtPassword"
        '
        'txtTargetFolder
        '
        resources.ApplyResources(Me.txtTargetFolder, "txtTargetFolder")
        Me.txtTargetFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTargetFolder.Name = "txtTargetFolder"
        Me.txtTargetFolder.ReadOnly = True
        '
        'cboDatabases
        '
        resources.ApplyResources(Me.cboDatabases, "cboDatabases")
        Me.cboDatabases.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDatabases.FormattingEnabled = True
        Me.cboDatabases.Name = "cboDatabases"
        '
        'rdbVBNET
        '
        resources.ApplyResources(Me.rdbVBNET, "rdbVBNET")
        Me.rdbVBNET.Name = "rdbVBNET"
        Me.rdbVBNET.UseVisualStyleBackColor = True
        '
        'rdbCSHARP
        '
        resources.ApplyResources(Me.rdbCSHARP, "rdbCSHARP")
        Me.rdbCSHARP.Checked = True
        Me.rdbCSHARP.Name = "rdbCSHARP"
        Me.rdbCSHARP.TabStop = True
        Me.rdbCSHARP.UseVisualStyleBackColor = True
        '
        'grpLanguage
        '
        resources.ApplyResources(Me.grpLanguage, "grpLanguage")
        Me.grpLanguage.Controls.Add(Me.rdbVBNET)
        Me.grpLanguage.Controls.Add(Me.rdbCSHARP)
        Me.grpLanguage.Name = "grpLanguage"
        Me.grpLanguage.TabStop = False
        '
        'grpCode
        '
        resources.ApplyResources(Me.grpCode, "grpCode")
        Me.grpCode.Controls.Add(Me.chkGenerarForm)
        Me.grpCode.Controls.Add(Me.chkGenerarSql)
        Me.grpCode.Controls.Add(Me.chkGenerarLn)
        Me.grpCode.Controls.Add(Me.chkGenerarBe)
        Me.grpCode.Name = "grpCode"
        Me.grpCode.TabStop = False
        '
        'chkGenerarForm
        '
        resources.ApplyResources(Me.chkGenerarForm, "chkGenerarForm")
        Me.chkGenerarForm.Name = "chkGenerarForm"
        Me.chkGenerarForm.UseVisualStyleBackColor = True
        '
        'grpTables
        '
        resources.ApplyResources(Me.grpTables, "grpTables")
        Me.grpTables.Controls.Add(Me.lblRowsCount)
        Me.grpTables.Controls.Add(Me.btnSelectAll)
        Me.grpTables.Controls.Add(Me.txtRowsCount)
        Me.grpTables.Controls.Add(Me.dgvTables)
        Me.grpTables.Name = "grpTables"
        Me.grpTables.TabStop = False
        '
        'lblRowsCount
        '
        resources.ApplyResources(Me.lblRowsCount, "lblRowsCount")
        Me.lblRowsCount.Name = "lblRowsCount"
        '
        'btnSelectAll
        '
        resources.ApplyResources(Me.btnSelectAll, "btnSelectAll")
        Me.btnSelectAll.Name = "btnSelectAll"
        Me.btnSelectAll.UseVisualStyleBackColor = True
        '
        'txtRowsCount
        '
        resources.ApplyResources(Me.txtRowsCount, "txtRowsCount")
        Me.txtRowsCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtRowsCount.Name = "txtRowsCount"
        Me.txtRowsCount.ReadOnly = True
        '
        'dgvTables
        '
        resources.ApplyResources(Me.dgvTables, "dgvTables")
        Me.dgvTables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvTables.Name = "dgvTables"
        '
        'grpConfiguration
        '
        resources.ApplyResources(Me.grpConfiguration, "grpConfiguration")
        Me.grpConfiguration.Controls.Add(Me.btnServer)
        Me.grpConfiguration.Controls.Add(Me.chkAutentification)
        Me.grpConfiguration.Controls.Add(Me.lblTargetFolder)
        Me.grpConfiguration.Controls.Add(Me.lblServer)
        Me.grpConfiguration.Controls.Add(Me.lblUsername)
        Me.grpConfiguration.Controls.Add(Me.lblPassword)
        Me.grpConfiguration.Controls.Add(Me.cboDatabases)
        Me.grpConfiguration.Controls.Add(Me.lblDatabases)
        Me.grpConfiguration.Controls.Add(Me.txtTargetFolder)
        Me.grpConfiguration.Controls.Add(Me.btnTargetFolder)
        Me.grpConfiguration.Controls.Add(Me.txtPassword)
        Me.grpConfiguration.Controls.Add(Me.btnDatabases)
        Me.grpConfiguration.Controls.Add(Me.txtUsername)
        Me.grpConfiguration.Controls.Add(Me.txtServer)
        Me.grpConfiguration.Name = "grpConfiguration"
        Me.grpConfiguration.TabStop = False
        '
        'btnServer
        '
        resources.ApplyResources(Me.btnServer, "btnServer")
        Me.btnServer.ImageList = Me.imlIconos
        Me.btnServer.Name = "btnServer"
        Me.btnServer.UseVisualStyleBackColor = True
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
        'txtContact
        '
        resources.ApplyResources(Me.txtContact, "txtContact")
        Me.txtContact.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtContact.Name = "txtContact"
        Me.txtContact.ReadOnly = True
        Me.txtContact.TabStop = False
        '
        'FrmMain
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.txtContact)
        Me.Controls.Add(Me.lblContact)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.grpConfiguration)
        Me.Controls.Add(Me.grpTables)
        Me.Controls.Add(Me.grpCode)
        Me.Controls.Add(Me.grpLanguage)
        Me.Controls.Add(Me.btnGenerate)
        Me.Controls.Add(Me.txtConsole)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmMain"
        Me.grpLanguage.ResumeLayout(False)
        Me.grpLanguage.PerformLayout()
        Me.grpCode.ResumeLayout(False)
        Me.grpCode.PerformLayout()
        Me.grpTables.ResumeLayout(False)
        Me.grpTables.PerformLayout()
        CType(Me.dgvTables, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpConfiguration.ResumeLayout(False)
        Me.grpConfiguration.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtConsole As System.Windows.Forms.TextBox
    Friend WithEvents fbdCarpetaDestino As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents imlIconos As System.Windows.Forms.ImageList
    Friend WithEvents lblTargetFolder As System.Windows.Forms.Label
    Friend WithEvents lblServer As System.Windows.Forms.Label
    Friend WithEvents lblUsername As System.Windows.Forms.Label
    Friend WithEvents lblPassword As System.Windows.Forms.Label
    Friend WithEvents lblDatabases As System.Windows.Forms.Label
    Friend WithEvents btnTargetFolder As System.Windows.Forms.Button
    Friend WithEvents btnDatabases As System.Windows.Forms.Button
    Friend WithEvents btnGenerate As System.Windows.Forms.Button
    Friend WithEvents chkGenerarBe As System.Windows.Forms.CheckBox
    Friend WithEvents chkGenerarLn As System.Windows.Forms.CheckBox
    Friend WithEvents chkGenerarSql As System.Windows.Forms.CheckBox
    Friend WithEvents chkAutentification As System.Windows.Forms.CheckBox
    Friend WithEvents txtServer As System.Windows.Forms.TextBox
    Friend WithEvents txtUsername As System.Windows.Forms.TextBox
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtTargetFolder As System.Windows.Forms.TextBox
    Friend WithEvents cboDatabases As System.Windows.Forms.ComboBox
    Friend WithEvents rdbVBNET As System.Windows.Forms.RadioButton
    Friend WithEvents rdbCSHARP As System.Windows.Forms.RadioButton
    Friend WithEvents grpLanguage As System.Windows.Forms.GroupBox
    Friend WithEvents grpCode As System.Windows.Forms.GroupBox
    Friend WithEvents grpTables As System.Windows.Forms.GroupBox
    Friend WithEvents dgvTables As System.Windows.Forms.DataGridView
    Friend WithEvents grpConfiguration As System.Windows.Forms.GroupBox
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    Friend WithEvents btnServer As System.Windows.Forms.Button
    Friend WithEvents lblContact As Label
    Friend WithEvents txtContact As TextBox
    Friend WithEvents lblRowsCount As Label
    Friend WithEvents btnSelectAll As Button
    Friend WithEvents txtRowsCount As TextBox
    Friend WithEvents chkGenerarForm As CheckBox
End Class
