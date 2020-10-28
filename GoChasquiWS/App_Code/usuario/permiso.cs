using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class permiso
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private int _id_permiso = 0;
        private int _id_recurso = 0;
        private int _id_rol = 0;
        private bool _permitido = true;
        private bool _activo = true;
        private DateTime _fecha = DateTime.Now;

        private string _error = "";
        //Propiedades públicas
        public int id_permiso { get { return _id_permiso; } set { _id_permiso = value; } }
        public int id_recurso { get { return _id_recurso; } set { _id_recurso = value; } }
        public int id_rol { get { return _id_rol; } set { _id_rol = value; } }
        public bool permitido { get { return _permitido; } set { _permitido = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }
        public DateTime fecha { get { return _fecha; } set { _fecha = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public permiso(int Id_permiso)
        {
            _id_permiso = Id_permiso;
            RecuperarDatos();
        }
        public permiso(int Id_permiso, int Id_recurso, int Id_rol, bool Permitido, bool Activo, DateTime Fecha)
        {
            _id_permiso = Id_permiso;
            _id_recurso = Id_recurso;
            _id_rol = Id_rol;
            _permitido = Permitido;
            _activo = Activo;
            _fecha = Fecha;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_permiso_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_permiso_individual");
                db1.AddInParameter(cmd, "id_permiso", DbType.Int32, _id_permiso);
                db1.AddOutParameter(cmd, "id_recurso", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "id_rol", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "permitido", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "fecha", DbType.DateTime, 30);
                db1.ExecuteNonQuery(cmd);

                _id_permiso = (int)db1.GetParameterValue(cmd, "id_permiso");
                _id_recurso = (int)db1.GetParameterValue(cmd, "id_recurso");
                _id_rol = (int)db1.GetParameterValue(cmd, "id_rol");
                _permitido = (Boolean)db1.GetParameterValue(cmd, "permitido");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
                _fecha = (DateTime)db1.GetParameterValue(cmd, "fecha");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_permiso");
                db1.AddInParameter(cmd, "id_permiso", DbType.Int32, _id_permiso);
                db1.AddInParameter(cmd, "id_recurso", DbType.Int32, _id_recurso);
                db1.AddInParameter(cmd, "id_rol", DbType.Int32, _id_rol);
                db1.AddInParameter(cmd, "permitido", DbType.Boolean, _permitido);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "fecha", DbType.DateTime, _fecha);

                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_permiso_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_permiso = (int)db1.GetParameterValue(cmd, "id_permiso_aux");
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