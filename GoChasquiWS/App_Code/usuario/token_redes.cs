using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class token_redes
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private int _id_token = 0;
        private int _id_tiporedes = 0;
        private string _token = "";
        private DateTime _fecha = DateTime.Now;
        private bool _activo = true;
        private int _id_usuario = 0;

        private string _error = "";
        //Propiedades públicas
        public int id_token { get { return _id_token; } set { _id_token = value; } }
        public int id_tiporedes { get { return _id_tiporedes; } set { _id_tiporedes = value; } }
        public string token { get { return _token; } set { _token = value; } }
        public DateTime fecha { get { return _fecha; } set { _fecha = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }
        public int id_usuario { get { return _id_usuario; } set { _id_usuario = value; } }
        #endregion

        #region Constructores
        public token_redes(int Id_token)
        {
            _id_token = Id_token;
            RecuperarDatos();
        }
        public token_redes(int Id_token, int Id_tiporedes, string Token, DateTime Fecha, bool Activo, int Id_usuario)
        {
            _id_token = Id_token;
            _id_tiporedes = Id_tiporedes;
            _token = Token;
            _fecha = Fecha;
            _activo = Activo;
            _id_usuario = Id_usuario;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_token_redes_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_token_redes_individual");
                db1.AddInParameter(cmd, "id_token", DbType.Int32, _id_token);
                db1.AddOutParameter(cmd, "id_tiporedes", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "token", DbType.String, 500);
                db1.AddOutParameter(cmd, "fecha", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "id_usuario", DbType.Int32, 32);
                db1.ExecuteNonQuery(cmd);

                _id_tiporedes = (int)db1.GetParameterValue(cmd, "id_tiporedes");
                _token = (string)db1.GetParameterValue(cmd, "token");
                _fecha = (DateTime)db1.GetParameterValue(cmd, "fecha");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
                _id_usuario = (int)db1.GetParameterValue(cmd, "id_usuario");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_token");
                db1.AddInParameter(cmd, "id_token", DbType.Int32, _id_token);
                db1.AddInParameter(cmd, "id_tiporedes", DbType.Int32, _id_tiporedes);
                db1.AddInParameter(cmd, "token", DbType.String, _token);
                db1.AddInParameter(cmd, "fecha", DbType.DateTime, _fecha);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "id_usuario", DbType.Int32, _id_usuario);

                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_token_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_token = (int)db1.GetParameterValue(cmd, "id_token_aux");
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