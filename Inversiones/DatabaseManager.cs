using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using Dapper;



namespace Inversiones
{


    public class Usuario
    {
        public int Id { get; set; }
        public decimal Efectivo { get; set; }
        public decimal Tarjeta { get; set; }
        public decimal IngresosMensuales { get; set; }
    }

    public class DatabaseManager
    {
        private string _nombreBaseDatos;

        public DatabaseManager(string nombreBaseDatos)
        {
            _nombreBaseDatos = nombreBaseDatos;
            CrearBaseDatos();
        }

        private SQLiteConnection ObtenerConexion()
        {
            return new SQLiteConnection($"Data Source={_nombreBaseDatos};Version=3;");
        }

        public void CrearBaseDatos()
        {
            if (!File.Exists(_nombreBaseDatos))
            {
                SQLiteConnection.CreateFile(_nombreBaseDatos);
                using (var conn = ObtenerConexion())
                {
                    conn.Open();
                    conn.Execute(
                        @"CREATE TABLE Usuario (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Efectivo REAL NOT NULL,
                        Tarjeta REAL NOT NULL,
                        IngresosMensuales REAL NOT NULL
                    );"
                    );
                }
            }
        }

        public Usuario ObtenerPrimerUsuario()
        {
            using (var conn = ObtenerConexion())
            {
                conn.Open();
                var usuario = conn.QueryFirstOrDefault<Usuario>("SELECT * FROM Usuario");
                if (usuario == null)
                {
                    usuario = new Usuario { Efectivo = 0.0m, Tarjeta = 0.0m, IngresosMensuales = 0.0m };
                    var sql = "INSERT INTO Usuario (Efectivo, Tarjeta, IngresosMensuales) VALUES (@Efectivo, @Tarjeta, @IngresosMensuales)";
                    conn.Execute(sql, usuario);
                    usuario.Id = (int)conn.LastInsertRowId;
                }
                return usuario;
            }
        }

        public void AgregarSaldo(string tipo, float cantidad, int ingreso)
        {
            using (var conn = ObtenerConexion())
            {
                conn.Open();
                var usuario = ObtenerPrimerUsuario();
                string actualizar;

                string columnaActualizar = "";
                if (tipo == "Tarjeta")
                {
                    columnaActualizar = "Tarjeta";
                }
                else if (tipo == "Efectivo")
                {
                    columnaActualizar = "Efectivo";
                }
                else
                {
                    throw new ArgumentException("Tipo de saldo no válido.");
                }
                if(ingreso == 1)
                {
                    actualizar = $"UPDATE Usuario SET {columnaActualizar} = {columnaActualizar} + @cantidad WHERE Id = @id";
                    conn.Execute(actualizar, new { cantidad, id = usuario.Id });
                }
                else if (ingreso == 2)
                {
                    actualizar = $"UPDATE Usuario SET {columnaActualizar} = {columnaActualizar} - @cantidad WHERE Id = @id";
                    conn.Execute(actualizar, new { cantidad, id = usuario.Id });
                }
                
               
            }
        }


    }
}








