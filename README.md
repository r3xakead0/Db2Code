# Db2Code
Poder automatizar el proceso de generación de código VB.NET y/o C# basado en la estructura y relaciones de las tablas de una base de datos. , para esto se desarrollo esta herramienta que genera el código de toda las tablas o las que se seleccione y asi facilitando la labor de desarrollo y el ahorro de tiempo en implementar la lógica para la guardar, leer, actualizar y borrar (CRUD) la información en la base de datos y facilitando su desarrollo ya que es manejado como objetos y en capas.

El programa puede generar código de 3 tipos que al final conforman la estructura de 3 capas necesarias para empezar el desarrollo :

Entidades (BE) :  
Genera las clases que manejan la estructura de la tabla y se usara como medio de almacenamiento en memoria de datos de un registro a la vez.

Lógicas (LN) : 
Genera las clases que manejaran los eventos como guardar, actualizar, eliminar, listar y navegar. Estas clases interactuan directamente con las clases de BE ya que necesitan colocar en memoria la información extraída de la base de datos.

Datos (SQL) : 
Genera el código sql para crear los Stored Procedure en la base de datos. Estos son necesarios para que la capa de Lógica pueda ejecutar el sp y poder guardar, actualizar, eliminar y listar la información de la tabla y capturarla con la capa Entidad.

Versiones

Versión 2.0 - Estable
Soporta la traducción en Ingles y Español.
Esta desarrollada en VS 2010 con VB.NET. 
No necesita ningún framework externo para la interactuar con la base de datos .
Solo soporta como base de datos SQL SERVER 2005 y 2008.
Te permite escoger que tablas de la base de datos van a ser la que genere el código fuente.
Genera código fuente para VB.NET, C# y SQL.
