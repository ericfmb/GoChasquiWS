using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class driver
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private string _tipo_operacion = "";
        private long _id_driver = 0;
        private string _nombre = "";
        private string _paterno = "";
        private string _materno = "";
        private string _ci = "";
        private bool _activo = true;
        private DateTime _fecha = DateTime.Now;
        private string _nro_licencia = "";
        private DateTime _ven_licencia = DateTime.Now;
        private DateTime _fecha_nacimiento = DateTime.Now;
        private string _genero = "";

        private string _error = "";
        //Propiedades públicas
        public string tipo_operacion { get { return _tipo_operacion; } set { _tipo_operacion = value; } }
        public long id_driver { get { return _id_driver; } set { _id_driver = value; } }
        public string nombre { get { return _nombre; } set { _nombre = value; } }
        public string paterno { get { return _paterno; } set { _paterno = value; } }
        public string materno { get { return _materno; } set { _materno = value; } }
        public string ci { get { return _ci; } set { _ci = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }
        public DateTime fecha { get { return _fecha; } set { _fecha = value; } }
        public string nro_licencia { get { return _nro_licencia; } set { _nro_licencia = value; } }
        public DateTime ven_licencia { get { return _ven_licencia; } set { _ven_licencia = value; } }
        public DateTime fecha_nacimiento { get { return _fecha_nacimiento; } set { _fecha_nacimiento = value; } }
        public string genero { get { return _genero; } set { _genero = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public driver(long Id_driver)
        {
            _id_driver = id_driver;
            RecuperarDatos();
        }
        public driver(string Tipo_operacion, long Id_driver, string Nombre, string Paterno, string Materno, string Ci, bool Activo, DateTime Fecha, string Nro_licencia, DateTime Ven_licencia, DateTime Fecha_nacimiento, string Genero)
        {
            _tipo_operacion = Tipo_operacion;
            _id_driver = Id_driver;
            _nombre = Nombre;
            _paterno = Paterno;
            _materno = Materno;
            _ci = Ci;
            _activo = Activo;
            _fecha = Fecha;
            _nro_licencia = Nro_licencia;
            _ven_licencia = Ven_licencia;
            _fecha_nacimiento = Fecha_nacimiento;
            _genero = Genero;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_driver_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_driver_individual");
                db1.AddInParameter(cmd, "id_driver", DbType.Int64, _id_driver);
                db1.AddOutParameter(cmd, "nombre", DbType.String, 200);
                db1.AddOutParameter(cmd, "paterno", DbType.String, 200);
                db1.AddOutParameter(cmd, "materno", DbType.String, 200);
                db1.AddOutParameter(cmd, "ci", DbType.String, 20);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "fecha", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "nro_licencia", DbType.String, 20);
                db1.AddOutParameter(cmd, "ven_licencia", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "fecha_nacimiento", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "genero", DbType.String, 1);
                db1.ExecuteNonQuery(cmd);

                _nombre = (string)db1.GetParameterValue(cmd, "nombre");
                _paterno = (string)db1.GetParameterValue(cmd, "paterno");
                _materno = (string)db1.GetParameterValue(cmd, "materno");
                _ci = (string)db1.GetParameterValue(cmd, "ci");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
                _fecha = (DateTime)db1.GetParameterValue(cmd, "fecha");
                _nro_licencia = (string)db1.GetParameterValue(cmd, "nro_licencia");
                _ven_licencia = (DateTime)db1.GetParameterValue(cmd, "ven_licencia");
                _fecha_nacimiento = (DateTime)db1.GetParameterValue(cmd, "fecha_nacimiento");
                _genero = (string)db1.GetParameterValue(cmd, "genero");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_driver");
                db1.AddInParameter(cmd, "tipo_operacion", DbType.String, _tipo_operacion);
                db1.AddInParameter(cmd, "id_driver", DbType.Int64, _id_driver);
                db1.AddInParameter(cmd, "nombre", DbType.String, _nombre);
                db1.AddInParameter(cmd, "paterno", DbType.String, _paterno);
                db1.AddInParameter(cmd, "materno", DbType.String, _materno);
                db1.AddInParameter(cmd, "ci", DbType.String, _ci);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "fecha", DbType.DateTime, _fecha);
                db1.AddInParameter(cmd, "nro_licencia", DbType.String, _nro_licencia);
                db1.AddInParameter(cmd, "ven_licencia", DbType.DateTime, _ven_licencia);
                db1.AddInParameter(cmd, "fecha_nacimiento", DbType.DateTime, _fecha_nacimiento);
                db1.AddInParameter(cmd, "genero", DbType.String, _genero);
                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_driver_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_driver = (int)db1.GetParameterValue(cmd, "id_driver_aux");
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