﻿using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class tipo_archivo
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private int _id_tipoarchivo = 0;
        private string _nombre = "";
        private string _extenciones = "";
        private bool _activo = true;

        private string _error = "";
        //Propiedades públicas
        public int id_tipoarchivo { get { return _id_tipoarchivo; } set { _id_tipoarchivo = value; } }
        public string nombre { get { return _nombre; } set { _nombre = value; } }
        public string extenciones { get { return _extenciones; } set { _extenciones = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public tipo_archivo(int Id_tipoarchivo)
        {
            _id_tipoarchivo = Id_tipoarchivo;
            RecuperarDatos();
        }
        public tipo_archivo(int Id_tipoarchivo, string Nombre, string Extenciones, bool Activo)
        {
            _id_tipoarchivo = Id_tipoarchivo;
            _nombre = Nombre;
            _extenciones = Extenciones;
            _activo = Activo;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_tipo_archivo_todos");
            db1.AddInParameter(cmd, "id_usuario", DbType.Int32, Id_usuario); // Enviar el código del usuario conectado
            cmd.CommandTimeout = int.Parse(ConfigurationManager.AppSettings["CommandTimeout"]);
            return db1.ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #region Métodos que requieren constructor
        private void RecuperarDatos()
        {
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("lista_tipo_archivo_individual");
                db1.AddInParameter(cmd, "id_tipoarchivo", DbType.Int32, _id_tipoarchivo);
                db1.AddOutParameter(cmd, "nombre", DbType.String, 200);
                db1.AddOutParameter(cmd, "extenciones", DbType.String, 100);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.ExecuteNonQuery(cmd);

                _nombre = (string)db1.GetParameterValue(cmd, "nombre");
                _extenciones = (string)db1.GetParameterValue(cmd, "extenciones");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_tipo_archivo");
                db1.AddInParameter(cmd, "id_tipoarchivo", DbType.Int32, _id_tipoarchivo);
                db1.AddInParameter(cmd, "nombre", DbType.String, _nombre);
                db1.AddInParameter(cmd, "extenciones", DbType.String, _extenciones);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);

                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_tipoarchivo_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_tipoarchivo = (int)db1.GetParameterValue(cmd, "id_tipoarchivo_aux");
                _error = (string)db1.GetParameterValue(cmd, "error");
                return resultado;
            }
            catch (Exception ex)
            {
                _error = ex.Message;
                resultado = "Se produjo un error al registrar";
                return resultado;
            }
        }
        #endregion
    }
}