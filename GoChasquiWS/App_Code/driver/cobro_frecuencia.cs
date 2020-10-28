using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class cobro_frecuencia
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private string _tipo_operacion = "";
        private int _id_cobrofrecuencia = 0;
        private int _id_parametroganancia = 0;
        private long _id_driver = 0;
        private DateTime _periodo = DateTime.Now;
        private decimal _monto = 0;
        private bool _activo = true;
        private DateTime _fecha = DateTime.Now;
        private bool _pagado = true;


        private string _error = "";
        //Propiedades públicas
        public string tipo_operacion { get { return _tipo_operacion; } set { _tipo_operacion = value; } }
        public int id_cobrofrecuencia { get { return _id_cobrofrecuencia; } set { _id_cobrofrecuencia = value; } }
        public int id_parametroganancia { get { return _id_parametroganancia; } set { _id_parametroganancia = value; } }
        public long id_driver { get { return _id_driver; } set { _id_driver = value; } }
        public DateTime periodo { get { return _periodo; } set { _periodo = value; } }
        public decimal monto { get { return _monto; } set { _monto = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }
        public DateTime fecha { get { return _fecha; } set { _fecha = value; } }
        public bool pagado { get { return _pagado; } set { _pagado = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public cobro_frecuencia(int Id_cobrofrecuencia)
        {
            _id_cobrofrecuencia = Id_cobrofrecuencia;
            RecuperarDatos();
        }
        public cobro_frecuencia(string Tipo_operacion, int Id_cobrofrecuencia, int Id_parametroganancia, long Id_driver, DateTime Periodo, decimal Monto, bool Activo, DateTime Fecha, bool Pagado)
        {
            _tipo_operacion = Tipo_operacion;
            _id_cobrofrecuencia = Id_cobrofrecuencia;
            _id_parametroganancia = Id_parametroganancia;
            _id_driver = Id_driver;
            _periodo = Periodo;
            _monto = Monto;
            _activo = Activo;
            _fecha = Fecha;
            _pagado = Pagado;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_cobro_frecuencia_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_cobro_frecuencia_individual");
                DbParameter Parametro;

                db1.AddInParameter(cmd, "id_cobrofrecuencia", DbType.Int32, _id_cobrofrecuencia);
                db1.AddOutParameter(cmd, "id_parametroganancia", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "id_driver", DbType.Int64, 64);
                db1.AddOutParameter(cmd, "periodo", DbType.DateTime, 30);
                Parametro = cmd.CreateParameter(); Parametro.ParameterName = "@monto"; Parametro.DbType = DbType.Decimal; Parametro.Direction = ParameterDirection.Output; Parametro.Precision = 18; Parametro.Scale = 2; cmd.Parameters.Add(Parametro);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "fecha", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "pagado", DbType.Boolean, 1);
                db1.ExecuteNonQuery(cmd);

                _id_parametroganancia = (int)db1.GetParameterValue(cmd, "id_parametroganancia");
                _id_driver = (long)db1.GetParameterValue(cmd, "id_driver");
                _periodo = (DateTime)db1.GetParameterValue(cmd, "periodo");
                _monto = (decimal)db1.GetParameterValue(cmd, "monto");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
                _fecha = (DateTime)db1.GetParameterValue(cmd, "fecha");
                _pagado = (Boolean)db1.GetParameterValue(cmd, "pagado");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_cobro_frecuencia");
                db1.AddInParameter(cmd, "tipo_operacion", DbType.String, _tipo_operacion);
                db1.AddInParameter(cmd, "id_cobrofrecuencia", DbType.Int32, _id_cobrofrecuencia);
                db1.AddInParameter(cmd, "id_parametroganancia", DbType.Int32, _id_parametroganancia);
                db1.AddInParameter(cmd, "id_driver", DbType.Int64, _id_driver);
                db1.AddInParameter(cmd, "periodo", DbType.DateTime, _periodo);
                db1.AddInParameter(cmd, "monto", DbType.Decimal, _monto);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "fecha", DbType.DateTime, _fecha);
                db1.AddInParameter(cmd, "pagado", DbType.Boolean, _pagado);
                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_cobrofrecuencia_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_cobrofrecuencia = (int)db1.GetParameterValue(cmd, "id_cobrofrecuencia_aux");
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