﻿using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class recurso
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private int _id_recurso = 0;
        private string _nombre = "";
        private bool _activo = true;

        private string _error = "";
        //Propiedades públicas
        public int id_recurso { get { return _id_recurso; } set { _id_recurso = value; } }
        public string nombre { get { return _nombre; } set { _nombre = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public recurso(int Id_recurso)
        {
            _id_recurso = Id_recurso;
            RecuperarDatos();
        }
        public recurso(int Id_recurso, string Nombre, bool Activo)
        {
            _id_recurso = Id_recurso;
            _nombre = Nombre;
            _activo = Activo;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_recurso_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_recurso_individual");
                db1.AddInParameter(cmd, "id_recurso", DbType.Int32, _id_recurso);
                db1.AddOutParameter(cmd, "nombre", DbType.String, 500);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.ExecuteNonQuery(cmd);

                _nombre = (string)db1.GetParameterValue(cmd, "nombre");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_recurso");
                db1.AddInParameter(cmd, "id_recurso", DbType.Int32, _id_recurso);
                db1.AddInParameter(cmd, "nombre", DbType.String, _nombre);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);

                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_recurso_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_recurso = (int)db1.GetParameterValue(cmd, "id_recurso_aux");
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