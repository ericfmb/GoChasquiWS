using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class ruta
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private string _tipo_operacion = "";
        private long _id_ruta = 0;
        private long _id_driver = 0;
        private string _latitud1 = "";
        private string _longitud1 = "";
        private string _direccion1 = "";
        private DateTime _fecha_inicio = DateTime.Now;
        private DateTime _fecha_fin = DateTime.Now;
        private bool _activo = true;
        private string _latitud2 = "";
        private string _longitud2 = "";
        private string _direccion2 = "";
        private long _id_pedido = 0;
        private decimal _distancia_km_aprox = 0;

        private string _error = "";
        //Propiedades públicas
        public string tipo_operacion { get { return _tipo_operacion; } set { _tipo_operacion = value; } }
        public long id_ruta { get { return _id_ruta; } set { _id_ruta = value; } }
        public long id_driver { get { return _id_driver; } set { _id_driver = value; } }
        public string latitud1 { get { return _latitud1; } set { _latitud1 = value; } }
        public string longitud1 { get { return _longitud1; } set { _longitud1 = value; } }
        public string direccion1 { get { return _direccion1; } set { _direccion1 = value; } }
        public DateTime fecha_inicio { get { return _fecha_inicio; } set { _fecha_inicio = value; } }
        public DateTime fecha_fin { get { return _fecha_fin; } set { _fecha_fin = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }
        public string latitud2 { get { return _latitud2; } set { _latitud2 = value; } }
        public string longitud2 { get { return _longitud2; } set { _longitud2 = value; } }
        public string direccion2 { get { return _direccion2; } set { _direccion2 = value; } }
        public long id_pedido { get { return _id_pedido; } set { _id_pedido = value; } }
        public decimal distancia_km_aprox { get { return _distancia_km_aprox; } set { _distancia_km_aprox = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public ruta(long Id_ruta)
        {
            _id_ruta = Id_ruta;
            RecuperarDatos();
        }
        public ruta(string Tipo_operacion, long Id_ruta, long Id_driver, string Latitud1, string Longitud1, string Direccion1, DateTime Fecha_inicio, DateTime Fecha_fin, bool Activo, string Latitud2, string Longitud2, string Direccion2, long Id_pedido, decimal Distancia_km_aprox)
        {
            _tipo_operacion = Tipo_operacion;
            _id_ruta = Id_ruta;
            _id_driver = Id_driver;
            _latitud1 = Latitud1;
            _longitud1 = Longitud1;
            _direccion1 = Direccion1;
            _fecha_inicio = Fecha_inicio;
            _fecha_fin = Fecha_fin;
            _activo = Activo;
            _latitud2 = Latitud2;
            _longitud2 = Longitud2;
            _direccion2 = Direccion2;
            _id_pedido = Id_pedido;
            _distancia_km_aprox = Distancia_km_aprox
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_ruta_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_ruta_individual");
                DbParameter Parametro;

                db1.AddInParameter(cmd, "id_ruta", DbType.Int64, _id_ruta);
                db1.AddOutParameter(cmd, "id_driver", DbType.Int64, 64);
                db1.AddOutParameter(cmd, "latitud1", DbType.String, 20);
                db1.AddOutParameter(cmd, "longitud1", DbType.String, 20);
                db1.AddOutParameter(cmd, "direccion1", DbType.String, 500);
                db1.AddOutParameter(cmd, "fecha_inicio", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "fecha_fin", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "latitud2", DbType.String, 20);
                db1.AddOutParameter(cmd, "longitud2", DbType.String, 20);
                db1.AddOutParameter(cmd, "direccion2", DbType.String, 500);
                db1.AddOutParameter(cmd, "id_pedido", DbType.Int64, 64);
                Parametro = cmd.CreateParameter(); Parametro.ParameterName = "@distancia_km_aprox"; Parametro.DbType = DbType.Decimal; Parametro.Direction = ParameterDirection.Output; Parametro.Precision = 18; Parametro.Scale = 2; cmd.Parameters.Add(Parametro);
                db1.ExecuteNonQuery(cmd);

                _id_driver = (long)db1.GetParameterValue(cmd, "id_driver");
                _latitud1 = (string)db1.GetParameterValue(cmd, "latitud1");
                _longitud1 = (string)db1.GetParameterValue(cmd, "longitud1");
                _direccion1 = (string)db1.GetParameterValue(cmd, "direccion1");
                _fecha_inicio = (DateTime)db1.GetParameterValue(cmd, "fecha_inicio");
                _fecha_fin = (DateTime)db1.GetParameterValue(cmd, "fecha_fin");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
                _latitud2 = (string)db1.GetParameterValue(cmd, "latitud2");
                _longitud2 = (string)db1.GetParameterValue(cmd, "longitud2");
                _direccion2 = (string)db1.GetParameterValue(cmd, "direccion2");
                _id_pedido = (long)db1.GetParameterValue(cmd, "id_pedido");
                _distancia_km_aprox = (decimal)db1.GetParameterValue(cmd, "distancia_km_aprox");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_ruta");
                db1.AddInParameter(cmd, "tipo_operacion", DbType.String, _tipo_operacion);
                db1.AddInParameter(cmd, "id_ruta", DbType.Int64, _id_ruta);
                db1.AddInParameter(cmd, "id_driver", DbType.Int64, _id_driver);
                db1.AddInParameter(cmd, "latitud1", DbType.String, _latitud1);
                db1.AddInParameter(cmd, "longitud1", DbType.String, _longitud1);
                db1.AddInParameter(cmd, "direccion1", DbType.String, _direccion1);
                db1.AddInParameter(cmd, "fecha_inicio", DbType.DateTime, _fecha_inicio);
                db1.AddInParameter(cmd, "fecha_fin", DbType.DateTime, _fecha_fin);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "latitud2", DbType.String, _latitud2);
                db1.AddInParameter(cmd, "longitud2", DbType.String, _longitud2);
                db1.AddInParameter(cmd, "direccion2", DbType.String, _direccion2);
                db1.AddInParameter(cmd, "id_pedido", DbType.Int64, _id_pedido);
                db1.AddInParameter(cmd, "distancia_km_aprox", DbType.Decimal, _distancia_km_aprox);
                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_ruta_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_ruta = (int)db1.GetParameterValue(cmd, "id_ruta_aux");
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