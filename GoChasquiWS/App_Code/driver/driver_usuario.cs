using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class driver_usuario
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private string _tipo_operacion = "";
        private long _id_driver = 0;
        private long _id_usuario = 0;
        private DateTime _fecha = DateTime.Now;
        private DateTime _fecha_fin = DateTime.Now;
        private bool _activo = true;


        private string _error = "";
        //Propiedades públicas
        public string tipo_operacion { get { return _tipo_operacion; } set { _tipo_operacion = value; } }
        public long id_driver { get { return _id_driver; } set { _id_driver = value; } }
        public long id_usuario { get { return _id_usuario; } set { _id_usuario = value; } }
        public DateTime fecha { get { return _fecha; } set { _fecha = value; } }
        public DateTime fecha_fin { get { return _fecha_fin; } set { _fecha_fin = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public driver_usuario(long Id_driver, long Id_usuario)
        {
            _id_driver = Id_driver;
            _id_usuario = Id_usuario;
            RecuperarDatos();
        }
        public driver_usuario(string Tipo_operacion, long Id_driver, long Id_usuario, DateTime Fecha, DateTime Fecha_fin, bool Activo)
        {
            _tipo_operacion = Tipo_operacion;
            _id_driver = Id_driver;
            _id_usuario = Id_usuario;
            _fecha = Fecha;
            _fecha_fin = Fecha_fin;
            _activo = Activo;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_driver_usuario_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_driver_usuario_individual");
                db1.AddInParameter(cmd, "id_driver", DbType.Int64, _id_driver);
                db1.AddInParameter(cmd, "id_usuario", DbType.Int64, _id_usuario);
                db1.AddOutParameter(cmd, "fecha", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "fecha_fin", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.ExecuteNonQuery(cmd);

                _fecha = (DateTime)db1.GetParameterValue(cmd, "fecha");
                _fecha_fin = (DateTime)db1.GetParameterValue(cmd, "fecha_fin");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_driver_usuario");
                db1.AddInParameter(cmd, "tipo_operacion", DbType.String, _tipo_operacion);
                db1.AddInParameter(cmd, "id_driver", DbType.Int64, _id_driver);
                db1.AddInParameter(cmd, "id_usuario", DbType.Int64, _id_usuario);
                db1.AddInParameter(cmd, "fecha", DbType.DateTime, _fecha);
                db1.AddInParameter(cmd, "fecha_fin", DbType.DateTime, _fecha_fin);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_driverusuario_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_driver = (int)db1.GetParameterValue(cmd, "id_driverusuario_aux");
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