using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class password_reset
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private int _id_passwordreset = 0;
        private int _id_usuario = 0;
        private string _clave_nueva = "";
        private string _clave_anterior = "";
        private string _url = "";
        private bool _activo = true;
        private bool _procesado = true;
        private DateTime _fecha = DateTime.Now;

        private string _error = "";
        //Propiedades públicas
        public int id_passwordreset { get { return _id_passwordreset; } set { _id_passwordreset = value; } }
        public int id_usuario { get { return _id_usuario; } set { _id_usuario = value; } }
        public string clave_nueva { get { return _clave_nueva; } set { _clave_nueva = value; } }
        public string clave_anterior { get { return _clave_anterior; } set { _clave_anterior = value; } }
        public string url { get { return _url; } set { _url = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }
        public bool procesado { get { return _procesado; } set { _procesado = value; } }
        public DateTime fecha { get { return _fecha; } set { _fecha = value; } }
        #endregion

        #region Constructores
        public password_reset(int Id_passwordreset)
        {
            _id_passwordreset = Id_passwordreset;
            RecuperarDatos();
        }
        public password_reset(int Id_passwordreset, int Id_usuario, string Clave_nueva, string Clave_anterior, string Url, bool Activo, bool Procesado, DateTime Fecha)
        {
            _id_passwordreset = Id_passwordreset;
            _id_usuario = Id_usuario;
            _clave_nueva = Clave_nueva;
            _clave_anterior = Clave_anterior;
            _url = Url;
            _activo = Activo;
            _procesado = Procesado;
            _fecha = Fecha;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_password_reset_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_password_reset_individual");
                db1.AddInParameter(cmd, "id_passwordreset", DbType.Int32, _id_passwordreset);
                db1.AddOutParameter(cmd, "id_usuario", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "clave_nueva", DbType.String, 256);
                db1.AddOutParameter(cmd, "clave_anterior", DbType.String, 256);
                db1.AddOutParameter(cmd, "url", DbType.String, 500);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "procesado", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "fecha", DbType.DateTime, 30);
                db1.ExecuteNonQuery(cmd);

                _id_usuario = (int)db1.GetParameterValue(cmd, "id_usuario");
                _clave_nueva = (string)db1.GetParameterValue(cmd, "clave_nueva");
                _clave_anterior = (string)db1.GetParameterValue(cmd, "clave_anterior");
                _url = (string)db1.GetParameterValue(cmd, "url");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
                _procesado = (Boolean)db1.GetParameterValue(cmd, "procesado");
                _fecha = (DateTime)db1.GetParameterValue(cmd, "fecha");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_passwordreset");
                db1.AddInParameter(cmd, "id_passwordreset", DbType.Int32, _id_passwordreset);
                db1.AddInParameter(cmd, "id_usuario", DbType.Int32, _id_usuario);
                db1.AddInParameter(cmd, "clave_nueva", DbType.String, _clave_nueva);
                db1.AddInParameter(cmd, "clave_anterior", DbType.String, _clave_anterior);
                db1.AddInParameter(cmd, "url", DbType.String, _url);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "procesado", DbType.Boolean, _procesado);
                db1.AddInParameter(cmd, "fecha", DbType.DateTime, _fecha);

                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_passwordreset_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_passwordreset = (int)db1.GetParameterValue(cmd, "id_passwordreset_aux");
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