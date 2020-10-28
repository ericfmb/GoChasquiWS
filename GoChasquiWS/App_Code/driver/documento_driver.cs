using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class documento_driver
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private string _tipo_operacion = "";
        private int _id_documentodriver = 0;
        private long _id_driver = 0;
        private string _url = "";
        private bool _activo = true;
        private DateTime _fecha = DateTime.Now;

        private string _error = "";
        //Propiedades públicas
        public string tipo_operacion { get { return _tipo_operacion; } set { _tipo_operacion = value; } }
        public int id_documentodriver { get { return _id_documentodriver; } set { _id_documentodriver = value; } }
        public long id_driver { get { return _id_driver; } set { _id_driver = value; } }
        public string url { get { return _url; } set { _url = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }
        public DateTime fecha { get { return _fecha; } set { _fecha = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public documento_driver(int Id_documentodriver)
        {
            _id_documentodriver = Id_documentodriver;
            RecuperarDatos();
        }
        public documento_driver(string Tipo_operacion, int Id_documentodriver, long Id_driver, string Url, bool Activo, DateTime Fecha)
        {
            _tipo_operacion = Tipo_operacion;
            _id_documentodriver = Id_documentodriver;
            _id_driver = Id_driver;
            _url = Url;
            _activo = Activo;
            _fecha = Fecha;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_documento_driver_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_documento_driver_individual");
                db1.AddInParameter(cmd, "id_documentodriver", DbType.Int32, _id_documentodriver);
                db1.AddOutParameter(cmd, "id_driver", DbType.Int64, 64);
                db1.AddOutParameter(cmd, "url", DbType.String, 500);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "fecha", DbType.DateTime, 30);
                db1.ExecuteNonQuery(cmd);

                _id_driver = (long)db1.GetParameterValue(cmd, "id_driver");
                _url = (string)db1.GetParameterValue(cmd, "url");
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
                DbCommand cmd = db1.GetStoredProcCommand("abm_documento_driver");
                db1.AddInParameter(cmd, "tipo_operacion", DbType.String, _tipo_operacion);
                db1.AddInParameter(cmd, "id_documentodriver", DbType.Int32, _id_documentodriver);
                db1.AddInParameter(cmd, "id_driver", DbType.Int64, _id_driver);
                db1.AddInParameter(cmd, "url", DbType.String, _url);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "fecha", DbType.DateTime, _fecha);
                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_documentodriver_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_documentodriver = (int)db1.GetParameterValue(cmd, "id_documentodriver_aux");
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