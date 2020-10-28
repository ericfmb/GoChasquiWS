using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class motorizado
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private string _tipo_operacion = "";
        private long _id_motorizado = 0;
        private string _placa = "";
        private bool _activo = true;
        private long _id_driver = 0;
        private int _id_tipomotorizado = 0;
        private string _color = "";
        private string _marca = "";
        private string _modelo = "";
        private DateTime _fecha = DateTime.Now;


        private string _error = "";
        //Propiedades públicas
        public string tipo_operacion { get { return _tipo_operacion; } set { _tipo_operacion = value; } }
        public long id_motorizado { get { return _id_motorizado; } set { _id_motorizado = value; } }
        public string placa { get { return _placa; } set { _placa = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }
        public long id_driver { get { return _id_driver; } set { _id_driver = value; } }
        public int id_tipomotorizado { get { return _id_tipomotorizado; } set { _id_tipomotorizado = value; } }
        public string color { get { return _color; } set { _color = value; } }
        public string marca { get { return _marca; } set { _marca = value; } }
        public string modelo { get { return _modelo; } set { _modelo = value; } }
        public DateTime fecha { get { return _fecha; } set { _fecha = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public motorizado(int id_motorizado)
        {
            _id_motorizado = id_motorizado;
            RecuperarDatos();
        }
        public motorizado(string Tipo_operacion, long Id_motorizado, string Placa, bool Activo, long Id_driver, int Id_tipomotorizado, string Color, string Marca, string Modelo, DateTime Fecha)
        {
            _tipo_operacion = Tipo_operacion;
            _id_motorizado = Id_motorizado;
            _placa = Placa;
            _activo = Activo;
            _id_driver = Id_driver;
            _id_tipomotorizado = Id_tipomotorizado;
            _color = Color;
            _marca = Marca;
            _modelo = Modelo;
            _fecha = Fecha;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_motorizado_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_motorizado_individual");
                db1.AddInParameter(cmd, "id_motorizado", DbType.Int64, _id_motorizado);
                db1.AddOutParameter(cmd, "placa", DbType.String, 20);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "id_driver", DbType.Int64, 64);
                db1.AddOutParameter(cmd, "id_tipomotorizado", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "color", DbType.String, 100);
                db1.AddOutParameter(cmd, "marca", DbType.String, 200);
                db1.AddOutParameter(cmd, "modelo", DbType.String, 200);
                db1.AddOutParameter(cmd, "fecha", DbType.DateTime, 30);
                db1.ExecuteNonQuery(cmd);

                _placa = (string)db1.GetParameterValue(cmd, "placa");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
                _id_driver = (long)db1.GetParameterValue(cmd, "id_driver");
                _id_tipomotorizado = (int)db1.GetParameterValue(cmd, "id_tipomotorizado");
                _color = (string)db1.GetParameterValue(cmd, "color");
                _marca = (string)db1.GetParameterValue(cmd, "marca");
                _modelo = (string)db1.GetParameterValue(cmd, "modelo");
                _fecha = (DateTime)db1.GetParameterValue(cmd, "fecha");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_motorizado");
                db1.AddInParameter(cmd, "tipo_operacion", DbType.String, _tipo_operacion);
                db1.AddInParameter(cmd, "id_motorizado", DbType.Int64, _id_motorizado);
                db1.AddInParameter(cmd, "placa", DbType.String, _placa);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "id_driver", DbType.Int64, _id_driver);
                db1.AddInParameter(cmd, "id_tipomotorizado", DbType.Int32, _id_tipomotorizado);
                db1.AddInParameter(cmd, "color", DbType.String, _color);
                db1.AddInParameter(cmd, "marca", DbType.String, _marca);
                db1.AddInParameter(cmd, "modelo", DbType.String, _modelo);
                db1.AddInParameter(cmd, "fecha", DbType.DateTime, _fecha);
                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_motorizado_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_motorizado = (int)db1.GetParameterValue(cmd, "id_motorizado_aux");
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