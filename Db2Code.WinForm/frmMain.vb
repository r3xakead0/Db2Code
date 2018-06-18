Imports System.IO
Imports System.Data.SqlClient
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

    Private strConexion As String = ""
    Private sLenguaje As String = ""
    Private dtTablas As DataTable = Nothing
    Private bSelected As Boolean = False

    Private Sub GenerateClassBE_VBNET(ByRef drTable As DataRow)
        Try

            Dim strUbicacion As String = Me.txtTargetFolder.Text & "\BE"

            If Not Directory.Exists(strUbicacion) Then
                Directory.CreateDirectory(strUbicacion)
            End If

            Dim nameTable As String = drTable.Item("name").ToString
            nameTable = nameTable.Substring(0, 1).ToUpper & nameTable.Substring(1)

            Dim dtColumns As DataTable = GetColumns(CInt(drTable.Item("object_id")))

            Dim objReader As New StreamWriter(strUbicacion & "\clsBe" & nameTable & ".vb")
            objReader.WriteLine("Public Class clsBe" & nameTable)
            objReader.WriteLine(vbTab & "Implements ICloneable")

            objReader.WriteLine("")

            For i As Integer = 0 To dtColumns.Rows.Count - 1
                Dim drColumns As DataRow = dtColumns.Rows(i)
                Dim drType As DataRow = GetTypeColumn(CInt(drColumns.Item("system_type_id")))

                Dim nameColumn As String = drColumns.Item("name").ToString
                nameColumn = nameColumn.Substring(0, 1).ToUpper & nameColumn.Substring(1)

                Dim valueColumn As String = GetDefaultValue(drType.Item("name"))

                objReader.WriteLine(vbTab & "Private m" & nameColumn & " As " & GetTranslateType(drType.Item("name")) & " = " & valueColumn)
            Next

            objReader.WriteLine("")

            For i As Integer = 0 To dtColumns.Rows.Count - 1
                Dim drColumns As DataRow = dtColumns.Rows(i)
                Dim drType As DataRow = GetTypeColumn(CInt(drColumns.Item("system_type_id")))

                Dim nameColumn As String = drColumns.Item("name").ToString
                nameColumn = nameColumn.Substring(0, 1).ToUpper & nameColumn.Substring(1)

                Dim typeColumn As String = GetTranslateType(drType.Item("name"))

                objReader.WriteLine(vbTab & "Public Property " & nameColumn & "() As " & typeColumn)
                objReader.WriteLine(vbTab & vbTab & "Get")
                objReader.WriteLine(vbTab & vbTab & vbTab & "Return m" & nameColumn)
                objReader.WriteLine(vbTab & vbTab & "End Get")
                objReader.WriteLine(vbTab & vbTab & "Set(ByVal Value As " & typeColumn & ")")
                objReader.WriteLine(vbTab & vbTab & vbTab & "m" & nameColumn & " = Value")
                objReader.WriteLine(vbTab & vbTab & "End Set")
                objReader.WriteLine(vbTab & "End Property")

                objReader.WriteLine("")
            Next

            'Definimos Metodos Constructores

            'Metodo Construsctor Limpio
            objReader.WriteLine(vbTab & "Sub New()")
            objReader.WriteLine(vbTab & "End Sub")

            objReader.WriteLine("")

            'Metodo Construsctor con parametros
            Dim headFunction As String = vbTab & "Sub New("
            Dim footFunction As String = vbTab & "End Sub"

            For i As Integer = 0 To dtColumns.Rows.Count - 1
                Dim drColumns As DataRow = dtColumns.Rows(i)
                Dim drType As DataRow = GetTypeColumn(CInt(drColumns.Item("system_type_id")))
                headFunction = headFunction &
                                IIf(i = 0, "ByRef ", "ByVal ") &
                                drColumns.Item("name").ToString &
                                " As " &
                                GetTranslateType(drType.Item("name")) &
                                IIf(i = dtColumns.Rows.Count - 1, ")", ", ")
            Next

            objReader.WriteLine(headFunction)

            For i As Integer = 0 To dtColumns.Rows.Count - 1
                Dim drColumns As DataRow = dtColumns.Rows(i)

                Dim nameColumn As String = drColumns.Item("name").ToString
                nameColumn = nameColumn.Substring(0, 1).ToUpper & nameColumn.Substring(1)

                objReader.WriteLine(vbTab & vbTab & "m" & nameColumn & " = " & nameColumn)
            Next

            objReader.WriteLine(footFunction)

            objReader.WriteLine("")

            objReader.WriteLine(vbTab & "Public Function Clone() As Object Implements System.ICloneable.Clone")
            objReader.WriteLine(vbTab & vbTab & "Return MyBase.MemberwiseClone()")
            objReader.WriteLine(vbTab & "End Function")

            objReader.WriteLine("")

            objReader.WriteLine("End Class")
            objReader.Close()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub GenerateClassBE_CSHARP(ByRef drTable As DataRow)
        Try

            Dim strUbicacion As String = Me.txtTargetFolder.Text & "\BE"

            If Not Directory.Exists(strUbicacion) Then
                Directory.CreateDirectory(strUbicacion)
            End If

            Dim nameTable As String = drTable.Item("name").ToString
            nameTable = nameTable.Substring(0, 1).ToUpper & nameTable.Substring(1)

            Dim dtColumns As DataTable = GetColumns(CInt(drTable.Item("object_id")))

            Dim objReader As New StreamWriter(strUbicacion & "\clsBe" & nameTable & ".cs")

            objReader.WriteLine("using System;")
            objReader.WriteLine("")

            objReader.WriteLine("public class clsBe" & nameTable & " : ICloneable {")

            objReader.WriteLine("")

            For i As Integer = 0 To dtColumns.Rows.Count - 1
                Dim drColumns As DataRow = dtColumns.Rows(i)
                Dim drType As DataRow = GetTypeColumn(CInt(drColumns.Item("system_type_id")))

                Dim nameColumn As String = drColumns.Item("name").ToString
                nameColumn = nameColumn.Substring(0, 1).ToUpper & nameColumn.Substring(1)

                Dim valueColumn As String = GetDefaultValue(drType.Item("name"), "CSHARP")

                objReader.WriteLine(vbTab & "private " & GetTranslateType(drType.Item("name"), "CSHARP") & " m" & nameColumn & " = " & valueColumn & ";")
            Next

            objReader.WriteLine("")

            For i As Integer = 0 To dtColumns.Rows.Count - 1
                Dim drColumns As DataRow = dtColumns.Rows(i)
                Dim drType As DataRow = GetTypeColumn(CInt(drColumns.Item("system_type_id")))

                Dim nameColumn As String = drColumns.Item("name").ToString
                nameColumn = nameColumn.Substring(0, 1).ToUpper & nameColumn.Substring(1)

                Dim typeColumn As String = GetTranslateType(drType.Item("name"), "CSHARP")

                objReader.WriteLine(vbTab & "public " & typeColumn & " " & nameColumn & " {")
                objReader.WriteLine(vbTab & vbTab & "get {")
                objReader.WriteLine(vbTab & vbTab & vbTab & "return m" & nameColumn & ";")
                objReader.WriteLine(vbTab & vbTab & "}")
                objReader.WriteLine(vbTab & vbTab & "set {")
                objReader.WriteLine(vbTab & vbTab & vbTab & "m" & nameColumn & " = value;")
                objReader.WriteLine(vbTab & vbTab & "}")
                objReader.WriteLine(vbTab & "}")

                objReader.WriteLine("")
            Next

            'Definimos Metodos Constructores

            'Metodo Construsctor Limpio
            objReader.WriteLine(vbTab & "public clsBe" & nameTable & "(){")
            objReader.WriteLine(vbTab & "}")

            objReader.WriteLine("")

            'Metodo Construsctor con parametros
            Dim headFunction As String = vbTab & "public clsBe" & nameTable & "("
            Dim footFunction As String = vbTab & "}"

            For i As Integer = 0 To dtColumns.Rows.Count - 1
                Dim drColumns As DataRow = dtColumns.Rows(i)
                Dim drType As DataRow = GetTypeColumn(CInt(drColumns.Item("system_type_id")))
                headFunction = headFunction &
                                IIf(i = 0, "ref ", "") &
                                GetTranslateType(drType.Item("name"), "CSHARP") & " " &
                                drColumns.Item("name").ToString &
                                IIf(i = dtColumns.Rows.Count - 1, ")", ", ")
            Next

            objReader.WriteLine(headFunction)
            objReader.WriteLine(vbTab & "{")

            For i As Integer = 0 To dtColumns.Rows.Count - 1
                Dim drColumns As DataRow = dtColumns.Rows(i)

                Dim nameColumnCamel As String = drColumns.Item("name").ToString
                nameColumnCamel = nameColumnCamel.Substring(0, 1).ToUpper & nameColumnCamel.Substring(1)

                Dim nameColumnNormal As String = drColumns.Item("name").ToString

                objReader.WriteLine(vbTab & vbTab & "m" & nameColumnCamel & " = " & nameColumnNormal & ";")
            Next

            objReader.WriteLine(footFunction)

            objReader.WriteLine("")

            objReader.WriteLine(vbTab & "public object Clone() {")
            objReader.WriteLine(vbTab & vbTab & "return base.MemberwiseClone();")
            objReader.WriteLine(vbTab & "}")

            objReader.WriteLine("")

            objReader.WriteLine("}")
            objReader.Close()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Private Sub GenerateClassLN_VBNET(ByRef drTable As DataRow)
        Try

            Dim strUbicacion As String = Me.txtTargetFolder.Text & "\LN"

            If Not Directory.Exists(strUbicacion) Then
                Directory.CreateDirectory(strUbicacion)
            End If

            Dim nameTable As String = drTable.Item("name").ToString
            nameTable = nameTable.Substring(0, 1).ToUpper & nameTable.Substring(1)

            Dim objReader As New StreamWriter(strUbicacion & "\clsLn" & nameTable & ".vb")

            objReader.WriteLine("Imports BE")
            objReader.WriteLine("Imports System.Data.SqlClient")
            objReader.WriteLine("Imports System.Data")
            objReader.WriteLine("")

            objReader.WriteLine("Public Class clsLn" & nameTable)
            objReader.WriteLine("")

            objReader.WriteLine(vbTab & "Dim oConexion As ConnectionManager = Nothing")
            objReader.WriteLine("")
            objReader.WriteLine(vbTab & "Public Sub New(ByVal oConexion As ConnectionManager)")
            objReader.WriteLine(vbTab & vbTab & "Try")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Me.oConexion = oConexion")
            objReader.WriteLine(vbTab & vbTab & "Catch ex As Exception")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Throw ex")
            objReader.WriteLine(vbTab & vbTab & "End Try")
            objReader.WriteLine(vbTab & "End Sub")
            objReader.WriteLine("")

            'Obtenemos las columnas de la tabla
            Dim dtColumns As DataTable = GetColumns(CInt(drTable.Item("object_id")))

            'Obtiene el PK de la tabla, en caso de no tener PK se obtiene la primera columna
            Dim drPrimaryKey As DataRow = GetPrimaryKey(CInt(drTable.Item("object_id")))
            Dim nameColumnPK As String = ""
            Dim typeColumnPK As String = ""
            If drPrimaryKey Is Nothing Then
                nameColumnPK = dtColumns.Rows(0).Item("name").ToString
                typeColumnPK = GetTypeColumn(CInt(dtColumns.Rows(0).Item("system_type_id"))).Item("name")
            Else
                nameColumnPK = drPrimaryKey.Item("name").ToString
                typeColumnPK = GetTypeColumn(CInt(drPrimaryKey.Item("system_type_id"))).Item("name")
            End If
            nameColumnPK = nameColumnPK.Substring(0, 1).ToUpper & nameColumnPK.Substring(1)

            Dim strParameter As String = ""

            'Cargar
            objReader.WriteLine(vbTab & "Public Sub Cargar(ByRef oBe" & nameTable & " As clsBe" & nameTable & ", ByRef dr As DataRow)")
            objReader.WriteLine(vbTab & vbTab & "Try")
            objReader.WriteLine(vbTab & vbTab & vbTab & "With oBe" & nameTable)
            For i As Integer = 0 To dtColumns.Rows.Count - 1
                Dim drColumns As DataRow = dtColumns.Rows(i)
                Dim drType As DataRow = GetTypeColumn(CInt(drColumns.Item("system_type_id")))

                Dim nameColumn As String = drColumns.Item("name").ToString
                Dim nameColumnCamel As String = nameColumn.Substring(0, 1).ToUpper & nameColumn.Substring(1)

                Dim valueColumn As String = GetDefaultValue(drType.Item("name"))

                objReader.WriteLine(vbTab & vbTab & vbTab & vbTab & "." & nameColumnCamel & " = IIf(IsDBNull(dr.Item(""" & nameColumn & """)), " & valueColumn & ", dr.Item(""" & nameColumn & """))")
            Next
            objReader.WriteLine(vbTab & vbTab & vbTab & "End With")
            objReader.WriteLine(vbTab & vbTab & "Catch ex As Exception")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Throw ex")
            objReader.WriteLine(vbTab & vbTab & "End Try")
            objReader.WriteLine(vbTab & "End Sub")

            objReader.WriteLine("")

            'Insert
            objReader.WriteLine(vbTab & "Public Function Insertar(ByRef oBe" & nameTable & " As clsBe" & nameTable & ") As Integer")
            objReader.WriteLine(vbTab & vbTab & "Try")

            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim sp As String = ""Sp" & nameTable & "Insertar""")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim cnn As New SqlConnection(oConexion.ConexionLocal)")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim cmd As New SqlClient.SqlCommand(sp, cnn)")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim rowsAffected As Integer = 0")
            objReader.WriteLine(vbTab & vbTab & vbTab & "cmd.Open()")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")

            For i As Integer = 0 To dtColumns.Rows.Count - 1
                Dim drColumns As DataRow = dtColumns.Rows(i)
                Dim nameColumn As String = drColumns.Item("name").ToString
                Dim nameColumnUpper As String = nameColumn.ToUpper
                Dim nameColumnCamel As String = nameColumn.Substring(0, 1).ToUpper & nameColumn.Substring(1)

                objReader.WriteLine(vbTab & vbTab & vbTab & "cmd.Parameters.Add(New SqlClient.SqlParameter(""@" & nameColumnUpper & """, oBe" & nameTable & "." & nameColumnCamel & "))")

                If nameColumnPK = nameColumn Then
                    objReader.WriteLine(vbTab & vbTab & vbTab & "cmd.Parameters(""@" & nameColumnUpper & """).Direction = ParameterDirection.Output")
                End If
            Next
            objReader.WriteLine(vbTab & vbTab & vbTab & "")

            objReader.WriteLine(vbTab & vbTab & vbTab & "rowsAffected = cmd.ExecuteNonQuery()")
            If nameColumnPK.Length > 0 Then
                Dim nameColumnPKUpper As String = nameColumnPK.ToUpper
                Dim nameColumnPKCamel As String = nameColumnPK.Substring(0, 1).ToUpper & nameColumnPK.Substring(1)
                Dim convertType As String = GetConvertType(typeColumnPK)

                If convertType.Length > 0 Then
                    objReader.WriteLine(vbTab & vbTab & vbTab & "oBe" & nameTable & "." & nameColumnPKCamel & " = " & convertType & "(cmd.Parameters(""@" & nameColumnPKUpper & """).Value)")
                Else
                    objReader.WriteLine(vbTab & vbTab & vbTab & "oBe" & nameTable & "." & nameColumnPKCamel & " = " & "cmd.Parameters(""@" & nameColumnPKUpper & """).Value")
                End If
            End If
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Return rowsAffected")

            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & "Catch ex As Exception")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Throw ex")
            objReader.WriteLine(vbTab & vbTab & "End Try")
            objReader.WriteLine(vbTab & "End Function")

            objReader.WriteLine("")

            'Update
            objReader.WriteLine(vbTab & "Public Function Actualizar(ByRef oBe" & nameTable & " As clsBe" & nameTable & ") As Integer")
            objReader.WriteLine(vbTab & vbTab & "Try")

            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim sp As String = ""Sp" & nameTable & "Actualizar""")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim cnn As New SqlConnection(oConexion.ConexionLocal)")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim cmd As New SqlClient.SqlCommand(sp, cnn)")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim rowsAffected As Integer = 0")
            objReader.WriteLine(vbTab & vbTab & vbTab & "cmd.Open()")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")

            For i As Integer = 0 To dtColumns.Rows.Count - 1
                Dim drColumns As DataRow = dtColumns.Rows(i)
                Dim nameColumn As String = drColumns.Item("name").ToString
                Dim nameColumnUpper As String = nameColumn.ToUpper
                Dim nameColumnCamel As String = nameColumn.Substring(0, 1).ToUpper & nameColumn.Substring(1)

                objReader.WriteLine(vbTab & vbTab & vbTab & "cmd.Parameters.Add(New SqlClient.SqlParameter(""@" & nameColumnUpper & """, oBe" & nameTable & "." & nameColumnCamel & "))")
            Next
            objReader.WriteLine(vbTab & vbTab & vbTab & "")

            objReader.WriteLine(vbTab & vbTab & vbTab & "rowsAffected = cmd.ExecuteNonQuery()")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Return rowsAffected")

            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & "Catch ex As Exception")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Throw ex")
            objReader.WriteLine(vbTab & vbTab & "End Try")
            objReader.WriteLine(vbTab & "End Function")

            objReader.WriteLine("")

            'Delete
            objReader.WriteLine(vbTab & "Public Function Eliminar(ByRef oBe" & nameTable & " As clsBe" & nameTable & ") As Integer")
            objReader.WriteLine(vbTab & vbTab & "Try")

            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim sp As String = ""Sp" & nameTable & "Eliminar""")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim cnn As New SqlConnection(oConexion.ConexionLocal)")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim cmd As New SqlClient.SqlCommand(sp, cnn)")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim rowsAffected As Integer = 0")
            objReader.WriteLine(vbTab & vbTab & vbTab & "cmd.Open()")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")

            If nameColumnPK.Length > 0 Then
                Dim nameColumnPKUpper As String = nameColumnPK.ToUpper
                Dim nameColumnPKCamel As String = nameColumnPK.Substring(0, 1).ToUpper & nameColumnPK.Substring(1)

                objReader.WriteLine(vbTab & vbTab & vbTab & "cmd.Parameters.Add(New SqlClient.SqlParameter(""@" & nameColumnPKUpper & """, oBe" & nameTable & "." & nameColumnPKCamel & "))")
            End If

            objReader.WriteLine(vbTab & vbTab & vbTab & "")

            objReader.WriteLine(vbTab & vbTab & vbTab & "rowsAffected = cmd.ExecuteNonQuery()")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Return rowsAffected")

            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & "Catch ex As Exception")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Throw ex")
            objReader.WriteLine(vbTab & vbTab & "End Try")
            objReader.WriteLine(vbTab & "End Function")

            objReader.WriteLine("")

            'List
            objReader.WriteLine(vbTab & "Public Function Listar() As DataTable")
            objReader.WriteLine(vbTab & vbTab & "Try")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim sp As String = ""Sp" & nameTable & "Listar""")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")

            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim cnn As New SqlConnection(oConexion.ConexionLocal)")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim cmd As New SqlCommand(sp, cnn)")
            objReader.WriteLine(vbTab & vbTab & vbTab & "cmd.CommandType = CommandType.StoredProcedure")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim dad As New SqlDataAdapter(cmd)")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim dt As New DataTable")
            objReader.WriteLine(vbTab & vbTab & vbTab & "dad.Fill(dt)")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Return dt")
            objReader.WriteLine(vbTab & vbTab & "Catch ex As Exception")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Throw ex")
            objReader.WriteLine(vbTab & vbTab & "End Try")
            objReader.WriteLine(vbTab & "End Function")

            objReader.WriteLine("")

            'Get
            objReader.WriteLine(vbTab & "Public Function Obtener(ByRef oBe" & nameTable & " As clsBe" & nameTable & ") As Boolean")
            objReader.WriteLine(vbTab & vbTab & "Try")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim sp As String = ""Sp" & nameTable & "Obtener""")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim cnn As New SqlConnection(oConexion.ConexionLocal)")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim cmd As New SqlCommand(sp, cnn)")
            objReader.WriteLine(vbTab & vbTab & vbTab & "cmd.CommandType = CommandType.StoredProcedure")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim dad As New SqlDataAdapter(cmd)")
            If nameColumnPK.Length > 0 Then
                Dim nameColumnPKUpper As String = nameColumnPK.ToUpper
                Dim nameColumnPKCamel As String = nameColumnPK.Substring(0, 1).ToUpper & nameColumnPK.Substring(1)

                objReader.WriteLine(vbTab & vbTab & vbTab & "dad.SelectCommand.Parameters.Add(New SqlClient.SqlParameter(""@" & nameColumnPKUpper & """, oBe" & nameTable & "." & nameColumnPKCamel & "))")
            End If

            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim dt As New DataTable")
            objReader.WriteLine(vbTab & vbTab & vbTab & "dad.Fill(dt)")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "If dt.Rows.Count = 1 Then")
            objReader.WriteLine(vbTab & vbTab & vbTab & vbTab & "Cargar(oBe" & nameTable & ", dt.Rows(0))")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Else")
            objReader.WriteLine(vbTab & vbTab & vbTab & vbTab & "Throw New Exception(""No se pudo obtener el registro"")")
            objReader.WriteLine(vbTab & vbTab & vbTab & "End If")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Return True")
            objReader.WriteLine(vbTab & vbTab & "Catch ex As Exception")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Throw ex")
            objReader.WriteLine(vbTab & vbTab & "End Try")
            objReader.WriteLine(vbTab & "End Function")

            objReader.WriteLine("")

            'First
            objReader.WriteLine(vbTab & "Public Function Primero(ByRef oBe" & nameTable & " As clsBe" & nameTable & ") As Boolean")
            objReader.WriteLine(vbTab & vbTab & "Try")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim sp As String = ""Sp" & nameTable & "Primero""")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim cnn As New SqlConnection(oConexion.ConexionLocal)")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim cmd As New SqlCommand(sp, cnn)")
            objReader.WriteLine(vbTab & vbTab & vbTab & "cmd.CommandType = CommandType.StoredProcedure")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim dad As New SqlDataAdapter(cmd)")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim dt As New DataTable")
            objReader.WriteLine(vbTab & vbTab & vbTab & "dad.Fill(dt)")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "If dt.Rows.Count = 1 Then")
            objReader.WriteLine(vbTab & vbTab & vbTab & vbTab & "Cargar(oBe" & nameTable & ", dt.Rows(0))")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Else")
            objReader.WriteLine(vbTab & vbTab & vbTab & vbTab & "Throw New Exception(""No se pudo obtener el primer registro"")")
            objReader.WriteLine(vbTab & vbTab & vbTab & "End If")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Return True")
            objReader.WriteLine(vbTab & vbTab & "Catch ex As Exception")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Throw ex")
            objReader.WriteLine(vbTab & vbTab & "End Try")
            objReader.WriteLine(vbTab & "End Function")

            objReader.WriteLine("")

            'Last
            objReader.WriteLine(vbTab & "Public Function Ultimo(ByRef oBe" & nameTable & " As clsBe" & nameTable & ") As Boolean")
            objReader.WriteLine(vbTab & vbTab & "Try")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim sp As String = ""Sp" & nameTable & "Ultimo""")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim cnn As New SqlConnection(oConexion.ConexionLocal)")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim cmd As New SqlCommand(sp, cnn)")
            objReader.WriteLine(vbTab & vbTab & vbTab & "cmd.CommandType = CommandType.StoredProcedure")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim dad As New SqlDataAdapter(cmd)")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim dt As New DataTable")
            objReader.WriteLine(vbTab & vbTab & vbTab & "dad.Fill(dt)")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "If dt.Rows.Count = 1 Then")
            objReader.WriteLine(vbTab & vbTab & vbTab & vbTab & "Cargar(oBe" & nameTable & ", dt.Rows(0))")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Else")
            objReader.WriteLine(vbTab & vbTab & vbTab & vbTab & "Throw New Exception(""No se pudo obtener el ultimo registro"")")
            objReader.WriteLine(vbTab & vbTab & vbTab & "End If")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Return True")
            objReader.WriteLine(vbTab & vbTab & "Catch ex As Exception")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Throw ex")
            objReader.WriteLine(vbTab & vbTab & "End Try")
            objReader.WriteLine(vbTab & "End Function")

            objReader.WriteLine("")

            'Previeus
            objReader.WriteLine(vbTab & "Public Function Anterior(ByRef oBe" & nameTable & " As clsBe" & nameTable & ") As Boolean")
            objReader.WriteLine(vbTab & vbTab & "Try")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim sp As String = ""Sp" & nameTable & "Anterior""")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim cnn As New SqlConnection(oConexion.ConexionLocal)")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim cmd As New SqlCommand(sp, cnn)")
            objReader.WriteLine(vbTab & vbTab & vbTab & "cmd.CommandType = CommandType.StoredProcedure")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim dad As New SqlDataAdapter(cmd)")
            If nameColumnPK.Length > 0 Then
                Dim nameColumnPKUpper As String = nameColumnPK.ToUpper
                Dim nameColumnPKCamel As String = nameColumnPK.Substring(0, 1).ToUpper & nameColumnPK.Substring(1)

                objReader.WriteLine(vbTab & vbTab & vbTab & "dad.SelectCommand.Parameters.Add(New SqlClient.SqlParameter(""@" & nameColumnPKUpper & """, oBe" & nameTable & "." & nameColumnPKCamel & "))")
            End If
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim dt As New DataTable")
            objReader.WriteLine(vbTab & vbTab & vbTab & "dad.Fill(dt)")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "If dt.Rows.Count = 1 Then")
            objReader.WriteLine(vbTab & vbTab & vbTab & vbTab & "Cargar(oBe" & nameTable & ", dt.Rows(0))")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Else")
            objReader.WriteLine(vbTab & vbTab & vbTab & vbTab & "Throw New Exception(""No se pudo obtener el anterior registro"")")
            objReader.WriteLine(vbTab & vbTab & vbTab & "End If")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Return True")
            objReader.WriteLine(vbTab & vbTab & "Catch ex As Exception")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Throw ex")
            objReader.WriteLine(vbTab & vbTab & "End Try")
            objReader.WriteLine(vbTab & "End Function")

            objReader.WriteLine("")

            'Next
            objReader.WriteLine(vbTab & "Public Function Siguiente(ByRef oBe" & nameTable & " As clsBe" & nameTable & ") As Boolean")
            objReader.WriteLine(vbTab & vbTab & "Try")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim sp As String = ""Sp" & nameTable & "Siguiente""")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim cnn As New SqlConnection(oConexion.ConexionLocal)")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim cmd As New SqlCommand(sp, cnn)")
            objReader.WriteLine(vbTab & vbTab & vbTab & "cmd.CommandType = CommandType.StoredProcedure")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim dad As New SqlDataAdapter(cmd)")
            If nameColumnPK.Length > 0 Then
                Dim nameColumnPKUpper As String = nameColumnPK.ToUpper
                Dim nameColumnPKCamel As String = nameColumnPK.Substring(0, 1).ToUpper & nameColumnPK.Substring(1)

                objReader.WriteLine(vbTab & vbTab & vbTab & "dad.SelectCommand.Parameters.Add(New SqlClient.SqlParameter(""@" & nameColumnPKUpper & """, oBe" & nameTable & "." & nameColumnPKCamel & "))")
            End If
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Dim dt As New DataTable")
            objReader.WriteLine(vbTab & vbTab & vbTab & "dad.Fill(dt)")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "If dt.Rows.Count = 1 Then")
            objReader.WriteLine(vbTab & vbTab & vbTab & vbTab & "Cargar(oBe" & nameTable & ", dt.Rows(0))")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Else")
            objReader.WriteLine(vbTab & vbTab & vbTab & vbTab & "Throw New Exception(""No se pudo obtener el siguiente registro"")")
            objReader.WriteLine(vbTab & vbTab & vbTab & "End If")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Return True")
            objReader.WriteLine(vbTab & vbTab & "Catch ex As Exception")
            objReader.WriteLine(vbTab & vbTab & vbTab & "Throw ex")
            objReader.WriteLine(vbTab & vbTab & "End Try")
            objReader.WriteLine(vbTab & "End Function")

            objReader.WriteLine("")

            objReader.WriteLine("End Class")
            objReader.Close()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub GenerateClassLN_CSHARP(ByRef drTable As DataRow)
        Try

            Dim strUbicacion As String = Me.txtTargetFolder.Text & "\LN"

            If Not Directory.Exists(strUbicacion) Then
                Directory.CreateDirectory(strUbicacion)
            End If

            Dim nameTable As String = drTable.Item("name").ToString
            nameTable = nameTable.Substring(0, 1).ToUpper & nameTable.Substring(1)

            Dim objReader As New StreamWriter(strUbicacion & "\clsLn" & nameTable & ".cs")

            objReader.WriteLine("using BE;")
            objReader.WriteLine("using System.Data.SqlClient;")
            objReader.WriteLine("using System.Data;")
            objReader.WriteLine("using System;")
            objReader.WriteLine("")

            objReader.WriteLine("public class clsLn" & nameTable & " {")
            objReader.WriteLine("")

            objReader.WriteLine(vbTab & "private ConnectionManager oConexion = null;")
            objReader.WriteLine("")
            objReader.WriteLine(vbTab & " public clsLn" & nameTable & "(ConnectionManager oConexion) {")
            objReader.WriteLine(vbTab & vbTab & "try {")
            objReader.WriteLine(vbTab & vbTab & vbTab & "this.oConexion = oConexion;")
            objReader.WriteLine(vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & vbTab & "catch (Exception ex) {")
            objReader.WriteLine(vbTab & vbTab & vbTab & "throw ex;")
            objReader.WriteLine(vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & "}")
            objReader.WriteLine("")

            'Obtenemos las columnas de la tabla
            Dim dtColumns As DataTable = GetColumns(CInt(drTable.Item("object_id")))

            'Obtiene el PK de la tabla, en caso de no tener PK se obtiene la primera columna
            Dim drPrimaryKey As DataRow = GetPrimaryKey(CInt(drTable.Item("object_id")))
            Dim nameColumnPK As String = ""
            Dim typeColumnPK As String = ""
            If drPrimaryKey Is Nothing Then
                nameColumnPK = dtColumns.Rows(0).Item("name").ToString
                typeColumnPK = GetTypeColumn(CInt(dtColumns.Rows(0).Item("system_type_id"))).Item("name")
            Else
                nameColumnPK = drPrimaryKey.Item("name").ToString
                typeColumnPK = GetTypeColumn(CInt(drPrimaryKey.Item("system_type_id"))).Item("name")
            End If
            nameColumnPK = nameColumnPK.Substring(0, 1).ToUpper & nameColumnPK.Substring(1)

            Dim strParameter As String = ""

            'Cargar
            objReader.WriteLine(vbTab & "public void Cargar(ref clsBe" & nameTable & " oBe" & nameTable & ", ref DataRow dr) {")
            objReader.WriteLine(vbTab & vbTab & "try {")
            For i As Integer = 0 To dtColumns.Rows.Count - 1
                Dim drColumns As DataRow = dtColumns.Rows(i)
                Dim drType As DataRow = GetTypeColumn(CInt(drColumns.Item("system_type_id")))

                Dim nameColumn As String = drColumns.Item("name").ToString
                Dim nameColumnCamel As String = nameColumn.Substring(0, 1).ToUpper & nameColumn.Substring(1)

                Dim valueColumn As String = GetDefaultValue(drType.Item("name"), "CSHARP")

                objReader.WriteLine(vbTab & vbTab & vbTab & vbTab & "oBe" & nameTable & "." & nameColumnCamel & " = dr[""" & nameColumn & """] == DBNull.Value ? " & valueColumn & " : dr[""" & nameColumn & """].ToString();")
            Next

            objReader.WriteLine(vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & vbTab & "catch (Exception ex) {")
            objReader.WriteLine(vbTab & vbTab & vbTab & "throw ex;")
            objReader.WriteLine(vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & "}")

            objReader.WriteLine("")

            'Insert
            'public int Insertar(ref clsBeTbUsuarios oBeTbUsuarios) {
            objReader.WriteLine(vbTab & "public int Insertar(ref clsBe" & nameTable & " oBe" & nameTable & ") {")
            objReader.WriteLine(vbTab & vbTab & "try {")

            objReader.WriteLine(vbTab & vbTab & vbTab & "string sp = ""Sp" & nameTable & "Insertar"";")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "SqlConnection cnn = new SqlConnection(oConexion.ConexionLocal);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "SqlCommand cmd = new SqlCommand(sp, cnn);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "cmd.CommandType = CommandType.StoredProcedure;")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "int rowsAffected = 0;")
            objReader.WriteLine(vbTab & vbTab & vbTab & "cnn.Open();")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")

            For i As Integer = 0 To dtColumns.Rows.Count - 1
                Dim drColumns As DataRow = dtColumns.Rows(i)
                Dim nameColumn As String = drColumns.Item("name").ToString
                Dim nameColumnUpper As String = nameColumn.ToUpper
                Dim nameColumnCamel As String = nameColumn.Substring(0, 1).ToUpper & nameColumn.Substring(1)

                objReader.WriteLine(vbTab & vbTab & vbTab & "cmd.Parameters.Add(new SqlParameter(""@" & nameColumnUpper & """, oBe" & nameTable & "." & nameColumnCamel & "));")

                If nameColumnPK = nameColumn Then
                    objReader.WriteLine(vbTab & vbTab & vbTab & "cmd.Parameters[""@" & nameColumnUpper & """].Direction = ParameterDirection.Output;")
                End If
            Next
            objReader.WriteLine(vbTab & vbTab & vbTab & "")

            objReader.WriteLine(vbTab & vbTab & vbTab & "rowsAffected = cmd.ExecuteNonQuery();")
            If nameColumnPK.Length > 0 Then
                Dim nameColumnPKUpper As String = nameColumnPK.ToUpper
                Dim nameColumnPKCamel As String = nameColumnPK.Substring(0, 1).ToUpper & nameColumnPK.Substring(1)
                Dim convertType As String = GetConvertType(typeColumnPK, "CSHARP")

                If convertType.Length > 0 Then
                    objReader.WriteLine(vbTab & vbTab & vbTab & "oBe" & nameTable & "." & nameColumnPKCamel & " = " & convertType & "(cmd.Parameters[""@" & nameColumnPKUpper & """].Value.ToString());")
                Else
                    objReader.WriteLine(vbTab & vbTab & vbTab & "oBe" & nameTable & "." & nameColumnPKCamel & " = " & "cmd.Parameters[""@" & nameColumnPKUpper & """].Value.ToString();")
                End If
            End If
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "return rowsAffected;")

            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & vbTab & "catch (Exception ex) {")
            objReader.WriteLine(vbTab & vbTab & vbTab & "throw ex;")
            objReader.WriteLine(vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & "}")

            objReader.WriteLine("")

            'Update
            objReader.WriteLine(vbTab & "public int Actualizar(ref clsBe" & nameTable & " oBe" & nameTable & ") {")
            objReader.WriteLine(vbTab & vbTab & "try {")

            objReader.WriteLine(vbTab & vbTab & vbTab & "string sp = ""Sp" & nameTable & "Actualizar"";")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "SqlConnection cnn = new SqlConnection(oConexion.ConexionLocal);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "SqlCommand cmd = new SqlCommand(sp, cnn);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "cmd.CommandType = CommandType.StoredProcedure;")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "int rowsAffected = 0;")
            objReader.WriteLine(vbTab & vbTab & vbTab & "cnn.Open();")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")

            For i As Integer = 0 To dtColumns.Rows.Count - 1
                Dim drColumns As DataRow = dtColumns.Rows(i)
                Dim nameColumn As String = drColumns.Item("name").ToString
                Dim nameColumnUpper As String = nameColumn.ToUpper
                Dim nameColumnCamel As String = nameColumn.Substring(0, 1).ToUpper & nameColumn.Substring(1)

                objReader.WriteLine(vbTab & vbTab & vbTab & "cmd.Parameters.Add(new SqlParameter(""@" & nameColumnUpper & """, oBe" & nameTable & "." & nameColumnCamel & "));")
            Next
            objReader.WriteLine(vbTab & vbTab & vbTab & "")

            objReader.WriteLine(vbTab & vbTab & vbTab & "rowsAffected = cmd.ExecuteNonQuery();")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "return rowsAffected;")

            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & vbTab & "catch (Exception ex) {")
            objReader.WriteLine(vbTab & vbTab & vbTab & "throw ex;")
            objReader.WriteLine(vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & "}")

            objReader.WriteLine("")

            'Delete
            objReader.WriteLine(vbTab & "public int Eliminar(ref clsBe" & nameTable & " oBe" & nameTable & ") {")
            objReader.WriteLine(vbTab & vbTab & "try {")

            objReader.WriteLine(vbTab & vbTab & vbTab & "string sp = ""Sp" & nameTable & "Eliminar"";")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "SqlConnection cnn = new SqlConnection(oConexion.ConexionLocal);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "SqlCommand cmd = new SqlCommand(sp, cnn);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "cmd.CommandType = CommandType.StoredProcedure;")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "int rowsAffected = 0;")
            objReader.WriteLine(vbTab & vbTab & vbTab & "cnn.Open();")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")

            If nameColumnPK.Length > 0 Then
                Dim nameColumnPKUpper As String = nameColumnPK.ToUpper
                Dim nameColumnPKCamel As String = nameColumnPK.Substring(0, 1).ToUpper & nameColumnPK.Substring(1)

                objReader.WriteLine(vbTab & vbTab & vbTab & "cmd.Parameters.Add(new SqlParameter(""@" & nameColumnPKUpper & """, oBe" & nameTable & "." & nameColumnPKCamel & "));")
            End If

            objReader.WriteLine(vbTab & vbTab & vbTab & "")

            objReader.WriteLine(vbTab & vbTab & vbTab & "rowsAffected = cmd.ExecuteNonQuery();")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "return rowsAffected;")

            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & vbTab & "catch (Exception ex) {")
            objReader.WriteLine(vbTab & vbTab & vbTab & "throw ex;")
            objReader.WriteLine(vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & "}")

            objReader.WriteLine("")

            'List,
            objReader.WriteLine(vbTab & "public DataTable Listar() {")
            objReader.WriteLine(vbTab & vbTab & "try {")
            objReader.WriteLine(vbTab & vbTab & vbTab & "string sp = ""Sp" & nameTable & "Listar"";")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")

            objReader.WriteLine(vbTab & vbTab & vbTab & "SqlConnection cnn = new SqlConnection(oConexion.ConexionLocal);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "SqlCommand cmd = new SqlCommand(sp, cnn);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "cmd.CommandType = CommandType.StoredProcedure;")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "SqlDataAdapter dad = new SqlDataAdapter(cmd);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "DataTable dt = new DataTable();")
            objReader.WriteLine(vbTab & vbTab & vbTab & "dad.Fill(dt);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "return dt;")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & vbTab & "catch (Exception ex) {")
            objReader.WriteLine(vbTab & vbTab & vbTab & "throw ex;")
            objReader.WriteLine(vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & "}")

            objReader.WriteLine("")

            'Get
            objReader.WriteLine(vbTab & "public bool Obtener(ref clsBe" & nameTable & " oBe" & nameTable & ") {")
            objReader.WriteLine(vbTab & vbTab & "try {")
            objReader.WriteLine(vbTab & vbTab & vbTab & "string sp = ""Sp" & nameTable & "Obtener"";")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "SqlConnection cnn = new SqlConnection(oConexion.ConexionLocal);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "SqlCommand cmd = new SqlCommand(sp, cnn);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "cmd.CommandType = CommandType.StoredProcedure;")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "SqlDataAdapter dad = new SqlDataAdapter(cmd);")
            If nameColumnPK.Length > 0 Then
                Dim nameColumnPKUpper As String = nameColumnPK.ToUpper
                Dim nameColumnPKCamel As String = nameColumnPK.Substring(0, 1).ToUpper & nameColumnPK.Substring(1)

                objReader.WriteLine(vbTab & vbTab & vbTab & "dad.SelectCommand.Parameters.Add(new SqlParameter(""@" & nameColumnPKUpper & """, oBe" & nameTable & "." & nameColumnPKCamel & "));")
            End If

            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "DataTable dt = new DataTable();")
            objReader.WriteLine(vbTab & vbTab & vbTab & "dad.Fill(dt);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "if ((dt.Rows.Count == 1)) {")
            objReader.WriteLine(vbTab & vbTab & vbTab & vbTab & "DataRow dr = dt.Rows[0];")
            objReader.WriteLine(vbTab & vbTab & vbTab & vbTab & "Cargar(ref oBe" & nameTable & ", ref dr);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & vbTab & vbTab & "else {")
            objReader.WriteLine(vbTab & vbTab & vbTab & vbTab & "throw new Exception(""No se pudo obtener el registro"");")
            objReader.WriteLine(vbTab & vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "return true;")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & vbTab & "catch (Exception ex) {")
            objReader.WriteLine(vbTab & vbTab & vbTab & "throw ex;")
            objReader.WriteLine(vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & "}")

            objReader.WriteLine("")

            'First
            objReader.WriteLine(vbTab & "public bool Primero(ref clsBe" & nameTable & " oBe" & nameTable & ") {")
            objReader.WriteLine(vbTab & vbTab & "try {")
            objReader.WriteLine(vbTab & vbTab & vbTab & "string sp = ""Sp" & nameTable & "Primero"";")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "SqlConnection cnn = new SqlConnection(oConexion.ConexionLocal);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "SqlCommand cmd = new SqlCommand(sp, cnn);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "cmd.CommandType = CommandType.StoredProcedure;")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "SqlDataAdapter dad = new SqlDataAdapter(cmd);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "DataTable dt = new DataTable();")
            objReader.WriteLine(vbTab & vbTab & vbTab & "dad.Fill(dt);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "if ((dt.Rows.Count == 1)) {")
            objReader.WriteLine(vbTab & vbTab & vbTab & vbTab & "DataRow dr = dt.Rows[0];")
            objReader.WriteLine(vbTab & vbTab & vbTab & vbTab & "Cargar(ref oBe" & nameTable & ", ref dr);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & vbTab & vbTab & "else {")
            objReader.WriteLine(vbTab & vbTab & vbTab & vbTab & "throw new Exception(""No se pudo obtener el primer registro"");")
            objReader.WriteLine(vbTab & vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "return true;")
            objReader.WriteLine(vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & vbTab & "catch (Exception ex) {")
            objReader.WriteLine(vbTab & vbTab & vbTab & "throw ex;")
            objReader.WriteLine(vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & "}")

            objReader.WriteLine("")

            'Last
            objReader.WriteLine(vbTab & "public bool Ultimo(ref clsBe" & nameTable & " oBe" & nameTable & ") {")
            objReader.WriteLine(vbTab & vbTab & "try {")
            objReader.WriteLine(vbTab & vbTab & vbTab & "string sp = ""Sp" & nameTable & "Ultimo"";")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "SqlConnection cnn = new SqlConnection(oConexion.ConexionLocal);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "SqlCommand cmd = new SqlCommand(sp, cnn);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "cmd.CommandType = CommandType.StoredProcedure;")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "SqlDataAdapter dad = new SqlDataAdapter(cmd);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "DataTable dt = new DataTable();")
            objReader.WriteLine(vbTab & vbTab & vbTab & "dad.Fill(dt);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "if ((dt.Rows.Count == 1)) {")
            objReader.WriteLine(vbTab & vbTab & vbTab & vbTab & "DataRow dr = dt.Rows[0];")
            objReader.WriteLine(vbTab & vbTab & vbTab & vbTab & "Cargar(ref oBe" & nameTable & ", ref dr);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & vbTab & vbTab & "else {")
            objReader.WriteLine(vbTab & vbTab & vbTab & vbTab & "throw new Exception(""No se pudo obtener el ultimo registro"");")
            objReader.WriteLine(vbTab & vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "return true;")
            objReader.WriteLine(vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & vbTab & "catch (Exception ex) {")
            objReader.WriteLine(vbTab & vbTab & vbTab & "throw ex;")
            objReader.WriteLine(vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & "}")

            objReader.WriteLine("")

            'Previeus
            objReader.WriteLine(vbTab & "public bool Anterior(ref clsBe" & nameTable & " oBe" & nameTable & ") {")
            objReader.WriteLine(vbTab & vbTab & "try {")
            objReader.WriteLine(vbTab & vbTab & vbTab & "string sp = ""Sp" & nameTable & "Anterior"";")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "SqlConnection cnn = new SqlConnection(oConexion.ConexionLocal);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "SqlCommand cmd = new SqlCommand(sp, cnn);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "cmd.CommandType = CommandType.StoredProcedure;")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "SqlDataAdapter dad = new SqlDataAdapter(cmd);")
            If nameColumnPK.Length > 0 Then
                Dim nameColumnPKUpper As String = nameColumnPK.ToUpper
                Dim nameColumnPKCamel As String = nameColumnPK.Substring(0, 1).ToUpper & nameColumnPK.Substring(1)

                objReader.WriteLine(vbTab & vbTab & vbTab & "dad.SelectCommand.Parameters.Add(new SqlParameter(""@" & nameColumnPKUpper & """, oBe" & nameTable & "." & nameColumnPKCamel & "));")
            End If
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "DataTable dt = new DataTable();")
            objReader.WriteLine(vbTab & vbTab & vbTab & "dad.Fill(dt);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "if ((dt.Rows.Count == 1)) {")
            objReader.WriteLine(vbTab & vbTab & vbTab & vbTab & "DataRow dr = dt.Rows[0];")
            objReader.WriteLine(vbTab & vbTab & vbTab & vbTab & "Cargar(ref oBe" & nameTable & ", ref dr);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & vbTab & vbTab & "else {")
            objReader.WriteLine(vbTab & vbTab & vbTab & vbTab & "throw new Exception(""No se pudo obtener el anterior registro"");")
            objReader.WriteLine(vbTab & vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "return true;")
            objReader.WriteLine(vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & vbTab & "catch (Exception ex) {")
            objReader.WriteLine(vbTab & vbTab & vbTab & "throw ex;")
            objReader.WriteLine(vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & "}")

            objReader.WriteLine("")

            'Next
            objReader.WriteLine(vbTab & "public bool Siguiente(ref clsBe" & nameTable & " oBe" & nameTable & ") {")
            objReader.WriteLine(vbTab & vbTab & "try {")
            objReader.WriteLine(vbTab & vbTab & vbTab & "string sp = ""Sp" & nameTable & "Siguiente"";")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "SqlConnection cnn = new SqlConnection(oConexion.ConexionLocal);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "SqlCommand cmd = new SqlCommand(sp, cnn);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "cmd.CommandType = CommandType.StoredProcedure;")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "SqlDataAdapter dad = new SqlDataAdapter(cmd);")
            If nameColumnPK.Length > 0 Then
                Dim nameColumnPKUpper As String = nameColumnPK.ToUpper
                Dim nameColumnPKCamel As String = nameColumnPK.Substring(0, 1).ToUpper & nameColumnPK.Substring(1)

                objReader.WriteLine(vbTab & vbTab & vbTab & "dad.SelectCommand.Parameters.Add(new SqlParameter(""@" & nameColumnPKUpper & """, oBe" & nameTable & "." & nameColumnPKCamel & "));")
            End If
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "DataTable dt = new DataTable();")
            objReader.WriteLine(vbTab & vbTab & vbTab & "dad.Fill(dt);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "if ((dt.Rows.Count == 1)) {")
            objReader.WriteLine(vbTab & vbTab & vbTab & vbTab & "DataRow dr = dt.Rows[0];")
            objReader.WriteLine(vbTab & vbTab & vbTab & vbTab & "Cargar(ref oBe" & nameTable & ", ref dr);")
            objReader.WriteLine(vbTab & vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & vbTab & vbTab & "else {")
            objReader.WriteLine(vbTab & vbTab & vbTab & vbTab & "throw new Exception(""No se pudo obtener el siguiente registro"");")
            objReader.WriteLine(vbTab & vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & vbTab & vbTab & "")
            objReader.WriteLine(vbTab & vbTab & vbTab & "return true;")
            objReader.WriteLine(vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & vbTab & "catch (Exception ex) {")
            objReader.WriteLine(vbTab & vbTab & vbTab & "throw ex;")
            objReader.WriteLine(vbTab & vbTab & "}")
            objReader.WriteLine(vbTab & "}")

            objReader.WriteLine("")

            objReader.WriteLine("}")
            objReader.Close()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub GenerateScriptsSQL(ByRef drTable As DataRow)
        Try

            Dim strUbicacion As String = Me.txtTargetFolder.Text & "\SQL"

            If Not Directory.Exists(strUbicacion) Then
                Directory.CreateDirectory(strUbicacion)
            End If

            Dim objReader As New StreamWriter(strUbicacion & "\" & drTable.Item("name").ToString & ".sql")
            objReader.WriteLine("set ANSI_NULLS ON")
            objReader.WriteLine("set QUOTED_IDENTIFIER ON")
            objReader.WriteLine("GO")

            objReader.WriteLine("")

            'Obtenemos las columnas de la tabla
            Dim dtColumns As DataTable = GetColumns(CInt(drTable.Item("object_id")))

            'Obtiene el PK de la tabla, en caso de no tener PK se obtiene la primera columna
            Dim drPrimaryKey As DataRow = GetPrimaryKey(CInt(drTable.Item("object_id")))
            Dim nameColumnPK As String = ""
            If drPrimaryKey Is Nothing Then
                drPrimaryKey = dtColumns.Rows(0)
                nameColumnPK = dtColumns.Rows(0).Item("name").ToString
            Else
                nameColumnPK = drPrimaryKey.Item("name").ToString
            End If
            nameColumnPK = nameColumnPK.Substring(0, 1).ToUpper & nameColumnPK.Substring(1)

            Dim nameTable As String = drTable.Item("name").ToString
            nameTable = nameTable.Substring(0, 1).ToUpper & nameTable.Substring(1)

            Dim head As String = ""
            Dim foot As String = ""

            'Insert
            objReader.WriteLine("CREATE PROCEDURE Sp" & nameTable & "Insertar")
            For i As Integer = 0 To dtColumns.Rows.Count - 1
                Dim drColumns As DataRow = dtColumns.Rows(i)
                Dim drType As DataRow = GetTypeColumn(CInt(drColumns.Item("system_type_id")))
                objReader.WriteLine("@" & drColumns.Item("name").ToString & " AS " & GetFormatColumn(drColumns).ToUpper & IIf(i = dtColumns.Rows.Count - 1, "", ","))
            Next
            objReader.WriteLine("AS")
            objReader.WriteLine("BEGIN")

            head = "INSERT INTO " & nameTable & " ("
            For i As Integer = 1 To dtColumns.Rows.Count - 1
                Dim drColumns As DataRow = dtColumns.Rows(i)
                head = head & drColumns.Item("name").ToString.Trim & IIf(i = dtColumns.Rows.Count - 1, ")", ",")
            Next
            objReader.WriteLine(head)

            foot = "VALUES ("
            For i As Integer = 1 To dtColumns.Rows.Count - 1
                Dim drColumns As DataRow = dtColumns.Rows(i)
                foot = foot & "@" & drColumns.Item("name").ToString.Trim & IIf(i = dtColumns.Rows.Count - 1, ")", ",")
            Next

            objReader.WriteLine(foot)
            objReader.WriteLine("SET @" & nameColumnPK & " = @@IDENTITY")
            objReader.WriteLine("END")
            objReader.WriteLine("GO")

            objReader.WriteLine("")

            'Update
            objReader.WriteLine("CREATE PROCEDURE Sp" & nameTable & "Actualizar")
            For i As Integer = 0 To dtColumns.Rows.Count - 1
                Dim drColumns As DataRow = dtColumns.Rows(i)
                Dim drType As DataRow = GetTypeColumn(CInt(drColumns.Item("system_type_id")))
                objReader.WriteLine("@" & drColumns.Item("name").ToString & " AS " & GetFormatColumn(drColumns).ToUpper & IIf(i = dtColumns.Rows.Count - 1, "", ","))
            Next
            objReader.WriteLine("AS")
            objReader.WriteLine("BEGIN")

            objReader.WriteLine("UPDATE " & nameTable)
            head = "SET "
            For i As Integer = 1 To dtColumns.Rows.Count - 1
                Dim drColumns As DataRow = dtColumns.Rows(i)
                head = head & drColumns.Item("name").ToString.Trim & " = @" & drColumns.Item("name").ToString.Trim & IIf(i = dtColumns.Rows.Count - 1, "", ",")
                objReader.WriteLine(head)
                head = ""
            Next
            objReader.WriteLine("WHERE " & dtColumns.Rows(0).Item("name").ToString.Trim & " = @" & dtColumns.Rows(0).Item("name").ToString.Trim)
            objReader.WriteLine("END")
            objReader.WriteLine("GO")

            objReader.WriteLine("")

            'Delete
            objReader.WriteLine("CREATE PROCEDURE Sp" & nameTable & "Eliminar")
            objReader.WriteLine("@" & nameColumnPK & " AS " & GetFormatColumn(drPrimaryKey).ToUpper)
            objReader.WriteLine("AS")
            objReader.WriteLine("BEGIN")
            objReader.WriteLine("DELETE FROM " & nameTable)
            objReader.WriteLine("WHERE " & nameColumnPK & " = @" & nameColumnPK)
            objReader.WriteLine("END")
            objReader.WriteLine("GO")

            objReader.WriteLine("")

            'List
            objReader.WriteLine("CREATE PROCEDURE Sp" & nameTable & "Listar")
            objReader.WriteLine("AS")
            objReader.WriteLine("BEGIN")
            head = "SELECT "
            For i As Integer = 0 To dtColumns.Rows.Count - 1
                Dim drColumns As DataRow = dtColumns.Rows(i)
                head = head & drColumns.Item("name").ToString.Trim & IIf(i = dtColumns.Rows.Count - 1, "", ",")
                objReader.WriteLine(head)
                head = ""
            Next
            objReader.WriteLine("FROM " & nameTable)
            objReader.WriteLine("END")
            objReader.WriteLine("GO")

            objReader.WriteLine("")

            'Get
            objReader.WriteLine("CREATE PROCEDURE Sp" & nameTable & "Obtener")
            objReader.WriteLine("@" & nameColumnPK & " AS " & GetFormatColumn(drPrimaryKey).ToUpper)
            objReader.WriteLine("AS")
            objReader.WriteLine("BEGIN")
            objReader.WriteLine("SELECT TOP 1 ")
            For i As Integer = 0 To dtColumns.Rows.Count - 1
                Dim drColumns As DataRow = dtColumns.Rows(i)
                objReader.WriteLine(drColumns.Item("name").ToString.Trim & IIf(i = dtColumns.Rows.Count - 1, "", ","))
            Next
            objReader.WriteLine("FROM " & nameTable)
            objReader.WriteLine("WHERE " & nameColumnPK & " = @" & nameColumnPK)
            objReader.WriteLine("END")
            objReader.WriteLine("GO")

            objReader.WriteLine("")

            'First
            objReader.WriteLine("CREATE PROCEDURE Sp" & nameTable & "Primero")
            objReader.WriteLine("AS")
            objReader.WriteLine("BEGIN")
            objReader.WriteLine("SELECT TOP 1 ")
            For i As Integer = 0 To dtColumns.Rows.Count - 1
                Dim drColumns As DataRow = dtColumns.Rows(i)
                objReader.WriteLine(drColumns.Item("name").ToString.Trim & IIf(i = dtColumns.Rows.Count - 1, "", ","))
            Next
            objReader.WriteLine("FROM " & nameTable)
            objReader.WriteLine("ORDER BY  " & nameColumnPK & " ASC")
            objReader.WriteLine("END")
            objReader.WriteLine("GO")

            objReader.WriteLine("")

            'Last
            objReader.WriteLine("CREATE PROCEDURE Sp" & nameTable & "Ultimo")
            objReader.WriteLine("AS")
            objReader.WriteLine("BEGIN")
            objReader.WriteLine("SELECT TOP 1 ")
            For i As Integer = 0 To dtColumns.Rows.Count - 1
                Dim drColumns As DataRow = dtColumns.Rows(i)
                objReader.WriteLine(drColumns.Item("name").ToString.Trim & IIf(i = dtColumns.Rows.Count - 1, "", ","))
            Next
            objReader.WriteLine("FROM " & nameTable)
            objReader.WriteLine("ORDER BY  " & nameColumnPK & " DESC")
            objReader.WriteLine("END")
            objReader.WriteLine("GO")

            objReader.WriteLine("")

            'Previous 
            objReader.WriteLine("CREATE PROCEDURE Sp" & nameTable & "Anterior")
            objReader.WriteLine("@" & nameColumnPK & " AS " & GetFormatColumn(drPrimaryKey).ToUpper)
            objReader.WriteLine("AS")
            objReader.WriteLine("IF(SELECT COUNT(" & nameColumnPK & ") FROM " & nameTable & " WHERE " & nameColumnPK & " < @" & nameColumnPK & ") > 0")
            objReader.WriteLine("BEGIN")
            objReader.WriteLine("SELECT TOP 1 ")
            For i As Integer = 0 To dtColumns.Rows.Count - 1
                Dim drColumns As DataRow = dtColumns.Rows(i)
                objReader.WriteLine(drColumns.Item("name").ToString.Trim & IIf(i = dtColumns.Rows.Count - 1, "", ","))
            Next
            objReader.WriteLine("FROM " & nameTable)
            objReader.WriteLine("WHERE " & nameColumnPK & " < @" & nameColumnPK)
            objReader.WriteLine("ORDER BY  " & nameColumnPK & " DESC")
            objReader.WriteLine("END")
            objReader.WriteLine("ELSE")
            objReader.WriteLine("BEGIN")
            objReader.WriteLine("SELECT TOP 1 ")
            For i As Integer = 0 To dtColumns.Rows.Count - 1
                Dim drColumns As DataRow = dtColumns.Rows(i)
                objReader.WriteLine(drColumns.Item("name").ToString.Trim & IIf(i = dtColumns.Rows.Count - 1, "", ","))
            Next
            objReader.WriteLine("FROM " & nameTable)
            objReader.WriteLine("ORDER BY  " & nameColumnPK & " ASC")
            objReader.WriteLine("END")
            objReader.WriteLine("GO")
            objReader.WriteLine("")

            objReader.WriteLine("")

            'NEXT 
            objReader.WriteLine("CREATE PROCEDURE Sp" & nameTable & "Siguiente")
            objReader.WriteLine("@" & nameColumnPK & " AS " & GetFormatColumn(drPrimaryKey).ToUpper)
            objReader.WriteLine("AS")
            objReader.WriteLine("IF(SELECT COUNT(" & nameColumnPK & ") FROM " & nameTable & " WHERE " & nameColumnPK & " > @" & nameColumnPK & ") > 0")
            objReader.WriteLine("BEGIN")
            objReader.WriteLine("SELECT TOP 1 ")
            For i As Integer = 0 To dtColumns.Rows.Count - 1
                Dim drColumns As DataRow = dtColumns.Rows(i)
                objReader.WriteLine(drColumns.Item("name").ToString.Trim & IIf(i = dtColumns.Rows.Count - 1, "", ","))
            Next
            objReader.WriteLine("FROM " & nameTable)
            objReader.WriteLine("WHERE " & nameColumnPK & " > @" & nameColumnPK)
            objReader.WriteLine("ORDER BY  " & nameColumnPK & " ASC")
            objReader.WriteLine("END")
            objReader.WriteLine("ELSE")
            objReader.WriteLine("BEGIN")
            objReader.WriteLine("SELECT TOP 1 ")
            For i As Integer = 0 To dtColumns.Rows.Count - 1
                Dim drColumns As DataRow = dtColumns.Rows(i)
                objReader.WriteLine(drColumns.Item("name").ToString.Trim & IIf(i = dtColumns.Rows.Count - 1, "", ","))
            Next
            objReader.WriteLine("FROM " & nameTable)
            objReader.WriteLine("ORDER BY  " & nameColumnPK & " DESC")
            objReader.WriteLine("END")
            objReader.WriteLine("GO")
            objReader.WriteLine("")

            objReader.Close()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function GetDataBases() As DataTable
        Try
            Dim sql As String = "SELECT database_id, name FROM sys.databases"
            Dim dt As DataTable = ListSql(sql)

            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function GetTables() As DataTable
        Try
            Dim sql As String = "SELECT object_id, name FROM sys.Tables ORDER BY name"
            Dim dt As DataTable = ListSql(sql)

            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function GetColumns(ByVal object_id As Integer) As DataTable
        Try
            Dim sql As String = "SELECT * FROM sys.columns where object_id = " & object_id
            Dim dt As DataTable = ListSql(sql)

            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function GetPrimaryKey(ByVal object_id As Integer) As DataRow
        Try
            Dim sql As String = "SELECT * FROM sys.columns where name in (SELECT TOP 1 COL_NAME(t0.OBJECT_ID,t0.column_id) FROM sys.index_columns t0 where object_id = " & object_id & ") and object_id = " & object_id
            Dim dt As DataTable = ListSql(sql)

            If dt.Rows.Count = 1 Then
                Return dt.Rows(0)
            Else
                Return Nothing
                'Throw New Exception("GetPrimaryKey")
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function GetFormatColumn(ByRef drColumns As DataRow) As String
        Try

            Dim drType As DataRow = GetTypeColumn(CInt(drColumns.Item("system_type_id")))

            Dim array1() As String = New String() {"varchar", "char", "nvarchar", "nchar", "float"}
            Dim array2() As String = New String() {"decimal", "numeric"}

            For Each str As String In array1
                If drType.Item("name").ToString = str Then
                    Return drType.Item("name").ToString.Trim & "(" & drColumns.Item("max_length").ToString.Trim & ")"
                End If
            Next

            For Each str As String In array2
                If drType.Item("name").ToString = str Then
                    Return drType.Item("name").ToString.Trim & "(" & drColumns.Item("precision").ToString.Trim & "," & drColumns.Item("scale").ToString.Trim & ")"
                End If
            Next

            Return drType.Item("name").ToString.Trim

        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Private Function GetTypeColumn(ByVal system_type_id As Integer) As DataRow
        Try
            Dim sql As String = "SELECT * FROM sys.types where system_type_id = " & system_type_id
            Dim dt As DataTable = ListSql(sql)

            If dt.Rows.Count > 0 Then
                Return dt.Rows(0)
            Else
                Throw New Exception("GetTypeColumn")
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function GetTranslateType(ByVal type As String, Optional ByVal language As String = "VBNET") As String
        Try
            Dim translate As String = ""

            If language = "VBNET" Then
                Select Case type
                    Case "text", "varchar", "char" 'Character Strings
                        translate = "String"
                    Case "ntext", "nvarchar", "nchar" 'Unicode Character Strings
                        translate = "String"
                    Case "tinyint", "smallint", "int", "bigint"
                        translate = "Integer"
                    Case "bit"
                        translate = "Boolean"
                    Case "decimal", "numeric", "money", "smallmoney"
                        translate = "Double"
                    Case "float", "real" 'Approximate Numerics
                        translate = "Double"
                    Case "smalldatetime", "datetime" 'Date and Time
                        translate = "Date"
                    Case "binary", "varbinary", "image" 'Binary Strings
                        translate = "String"
                    Case Else 'Other Data Types
                        translate = "String"
                End Select

            ElseIf language = "CSHARP" Then
                Select Case type
                    Case "text", "varchar", "char" 'Character Strings
                        translate = "string"
                    Case "ntext", "nvarchar", "nchar" 'Unicode Character Strings
                        translate = "string"
                    Case "tinyint", "smallint", "int", "bigint"
                        translate = "int"
                    Case "bit"
                        translate = "bool"
                    Case "decimal", "numeric", "money", "smallmoney"
                        translate = "double"
                    Case "float", "real" 'Approximate Numerics
                        translate = "double"
                    Case "smalldatetime", "datetime" 'Date and Time
                        translate = "DateTime"
                    Case "binary", "varbinary", "image" 'Binary Strings
                        translate = "string"
                    Case Else 'Other Data Types
                        translate = "string"
                End Select

            End If
            Return translate
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function GetDefaultValue(ByVal type As String, Optional ByVal language As String = "VBNET") As String
        Try
            Dim translate As String = ""

            If language = "VBNET" Then
                Select Case type
                    Case "text", "varchar", "char" 'Character Strings
                        translate = """"""
                    Case "ntext", "nvarchar", "nchar" 'Unicode Character Strings
                        translate = """"""
                    Case "tinyint", "smallint", "int", "bigint"
                        translate = "0"
                    Case "bit"
                        translate = "False"
                    Case "decimal", "numeric", "money", "smallmoney"
                        translate = "0.0"
                    Case "float", "real" 'Approximate Numerics
                        translate = "0.0"
                    Case "smalldatetime", "datetime" 'Date and Time
                        translate = "Date.Now"
                    Case "binary", "varbinary", "image" 'Binary Strings
                        translate = """"""
                    Case Else 'Other Data Types
                        translate = ""
                End Select

            ElseIf language = "CSHARP" Then
                Select Case type
                    Case "text", "varchar", "char" 'Character Strings
                        translate = """"""
                    Case "ntext", "nvarchar", "nchar" 'Unicode Character Strings
                        translate = """"""
                    Case "tinyint", "smallint", "int", "bigint"
                        translate = "0"
                    Case "bit"
                        translate = "false"
                    Case "decimal", "numeric", "money", "smallmoney"
                        translate = "0.0"
                    Case "float", "real" 'Approximate Numerics
                        translate = "0.0"
                    Case "smalldatetime", "datetime" 'Date and Time
                        translate = "DateTime.Now"
                    Case "binary", "varbinary", "image" 'Binary Strings
                        translate = "''"
                    Case Else 'Other Data Types
                        translate = ""
                End Select
            End If

            Return translate
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function GetConvertType(ByVal type As String, Optional ByVal language As String = "VBNET") As String
        Try
            Dim convert As String = ""

            If language = "VBNET" Then
                Select Case type
                    Case "text", "varchar", "char" 'Character Strings
                        convert = "CStr"
                    Case "ntext", "nvarchar", "nchar" 'Unicode Character Strings
                        convert = "CStr"
                    Case "tinyint", "smallint", "int", "bigint"
                        convert = "CInt"
                    Case "bit"
                        convert = "CBool"
                    Case "decimal", "numeric", "money", "smallmoney"
                        convert = "CDbl"
                    Case "float", "real" 'Approximate Numerics
                        convert = "CDbl"
                    Case "smalldatetime", "datetime" 'Date and Time
                        convert = "CDate"
                    Case "binary", "varbinary", "image" 'Binary Strings
                        convert = "CStr"
                    Case Else 'Other Data Types
                        convert = ""
                End Select

            ElseIf language = "CSHARP" Then
                Select Case type
                    Case "text", "varchar", "char" 'Character Strings
                        convert = "Convert.ToString"
                    Case "ntext", "nvarchar", "nchar" 'Unicode Character Strings
                        convert = "Convert.ToString"
                    Case "tinyint", "smallint", "int", "bigint"
                        convert = "int.Parse"
                    Case "bit"
                        convert = "bool.Parse"
                    Case "decimal", "numeric", "money", "smallmoney"
                        convert = "double.Parse"
                    Case "float", "real" 'Approximate Numerics
                        convert = "double.Parse"
                    Case "smalldatetime", "datetime" 'Date and Time
                        convert = "DateTime.Parse"
                    Case "binary", "varbinary", "image" 'Binary Strings
                        convert = "Convert.ToString"
                    Case Else 'Other Data Types
                        convert = ""
                End Select

            End If
            Return convert
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ListSql(ByVal sql As String, Optional ByVal params() As SqlParameter = Nothing) As DataTable
        Try
            Dim cnx As New SqlConnection(Me.strConexion)

            Dim cmd As New SqlCommand(sql, cnx)
            cmd.CommandType = CommandType.Text

            Dim dad As New SqlDataAdapter(cmd)

            If Not params Is Nothing Then
                For i As Integer = 0 To params.Length - 1
                    dad.SelectCommand.Parameters.Add(params(i))
                Next
            End If

            Dim dt As New DataTable
            dad.Fill(dt)

            Return dt

        Catch ex As SqlException
            Throw ex
        Catch ex As Exception
            Throw ex
        End Try

    End Function

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

            Me.strConexion = "Data Source=" & Me.txtServer.Text & ";Initial Catalog=" & CType(Me.cboDatabases.SelectedItem, DataRowView).Item(1).ToString & ";User Id=" & Me.txtUsername.Text & ";Password=" & Me.txtPassword.Text & ";"

            Util.PointerLoad(Me)

            Me.txtConsole.Clear()

            If Me.rdbVBNET.Checked Then
                Me.sLenguaje = "VBNET"
            ElseIf Me.rdbCSHARP.Checked Then
                Me.sLenguaje = "CSHARP"
            Else
                Throw New Exception("No existe el lenguaje seleccionado, porfavor verifique.")
            End If

            Me.txtConsole.Text = Me.txtConsole.Text & "Inicio de la Generacion de Codigo " & vbCrLf

            'Dim dtTables As DataTable = GetTables()
            '==================================================================================
            Dim dtTables As New DataTable
            dtTables.Columns.Add("object_id", System.Type.GetType("System.Int32"))
            dtTables.Columns.Add("name", System.Type.GetType("System.String"))
            For Each drv As DataGridViewRow In Me.dgvTables.Rows
                If CType(drv.Cells(0).Value, Boolean) = True Then 'Si esta con CHECK
                    Dim drTable As DataRow = dtTables.NewRow
                    drTable.Item("object_id") = CInt(drv.Cells("Id").Value)
                    drTable.Item("name") = drv.Cells("Nombre").Value.ToString
                    dtTables.Rows.Add(drTable)
                End If
            Next
            dtTables.AcceptChanges()
            '==================================================================================

            For Each drTable As DataRow In dtTables.Rows

                If Me.chkGenerarSql.Checked Then GenerateScriptsSQL(drTable)

                If Me.chkGenerarBe.Checked Then
                    If Me.sLenguaje = "VBNET" Then
                        GenerateClassBE_VBNET(drTable)
                    ElseIf Me.sLenguaje = "CSHARP" Then
                        GenerateClassBE_CSHARP(drTable)
                    End If
                End If

                If Me.chkGenerarLn.Checked Then
                    If Me.sLenguaje = "VBNET" Then
                        GenerateClassLN_VBNET(drTable)
                    ElseIf Me.sLenguaje = "CSHARP" Then
                        GenerateClassLN_CSHARP(drTable)
                    End If
                End If

            Next

            If Me.chkGenerarSql.Checked Then Me.txtConsole.Text = Me.txtConsole.Text & "- Se genero scripts de SQL" & vbCrLf
            If Me.chkGenerarBe.Checked Then Me.txtConsole.Text = Me.txtConsole.Text & "- Se genero clases de BE" & vbCrLf
            If Me.chkGenerarLn.Checked Then Me.txtConsole.Text = Me.txtConsole.Text & "- Se genero clases de LN" & vbCrLf

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

            Dim strServer As String = Me.txtServer.Text
            Dim strDB As String = "master"

            If Me.chkAutentification.Checked Then
                Me.strConexion = $"Server={strServer};Database={strDB};Trusted_Connection=True;"
            Else
                Dim strUser As String = Me.txtUsername.Text
                Dim strPass As String = Me.txtPassword.Text
                Me.strConexion = $"Server={strServer};Database={strDB};User Id={strUser};Password={strPass};"
            End If

            Util.PointerLoad(Me)

            Dim dtDbs As DataTable = GetDataBases()

            Dim dr As DataRow
            dr = dtDbs.NewRow()
            dr(0) = 0
            dr(1) = "Seleccione una Base de Datos"
            dtDbs.Rows.InsertAt(dr, 0)
            dtDbs.AcceptChanges()

            Me.cboDatabases.DataSource = dtDbs
            Me.cboDatabases.DisplayMember = dtDbs.Columns(1).Caption
            Me.cboDatabases.ValueMember = dtDbs.Columns(0).Caption

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

            Me.dtTablas.Rows.Clear()

            If Me.cboDatabases.SelectedIndex = 0 Then
                For i As Integer = 0 To 10
                    Dim drTabla As DataRow = Me.dtTablas.NewRow
                    drTabla.Item("Id") = 0
                    drTabla.Item("Nombre") = ""
                    Me.dtTablas.Rows.Add(drTabla)
                Next
                Me.dtTablas.AcceptChanges()
                Exit Sub
            End If

            Dim strServer As String = Me.txtServer.Text
            Dim strDB As String = CType(Me.cboDatabases.SelectedItem, DataRowView).Item(1).ToString

            If Me.chkAutentification.Checked Then
                Me.strConexion = $"Server={strServer};Database={strDB};Trusted_Connection=True;"
            Else
                Dim strUser As String = Me.txtUsername.Text
                Dim strPass As String = Me.txtPassword.Text
                Me.strConexion = $"Server={strServer};Database={strDB};User Id={strUser};Password={strPass};"
            End If


            Util.PointerLoad(Me)

            Dim dt As DataTable = GetTables()
            If dt.Rows.Count = 0 Then
                For i As Integer = 0 To 10
                    Dim drTabla As DataRow = Me.dtTablas.NewRow
                    drTabla.Item("Id") = 0
                    drTabla.Item("Nombre") = ""
                    Me.dtTablas.Rows.Add(drTabla)
                Next
                Me.txtRowsCount.Text = "0"
            Else
                For Each dr As DataRow In dt.Rows
                    Dim drTablas As DataRow = Me.dtTablas.NewRow
                    drTablas.Item("Id") = dr.Item("object_id")
                    drTablas.Item("Nombre") = dr.Item("name")
                    Me.dtTablas.Rows.Add(drTablas)
                Next
                Me.txtRowsCount.Text = dt.Rows.Count.ToString()
            End If
            Me.dtTablas.AcceptChanges()

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
