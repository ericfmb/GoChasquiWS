﻿using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class menu_rol
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private int _id_menu = 0;
        private int _id_rol = 0;
        private bool _activo = true;

        private string _error = "";
        //Propiedades públicas
        public int id_menu { get { return _id_menu; } set { _id_menu = value; } }
        public int id_rol { get { return _id_rol; } set { _id_rol = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public menu_rol(int Id_menu,int Id_rol)
        {
            _id_menu = Id_menu;
            _id_rol = Id_rol;
            RecuperarDatos();
        }
        public menu_rol(int Id_menu, int Id_rol, bool Activo)
        {
            _id_menu = Id_menu;
            _id_rol = Id_rol;
            _activo = Activo;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_menu_rol_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_menu_rol_individual");
                db1.AddInParameter(cmd, "id_menu", DbType.Int32, 32);
                db1.AddInParameter(cmd, "id_rol", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.ExecuteNonQuery(cmd);

                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_menu_rol");
                db1.AddInParameter(cmd, "id_menu", DbType.Int32, _id_menu);
                db1.AddInParameter(cmd, "id_rol", DbType.Int32, _id_rol);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);

                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_menurol_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_menu = (int)db1.GetParameterValue(cmd, "id_menurol_aux");
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