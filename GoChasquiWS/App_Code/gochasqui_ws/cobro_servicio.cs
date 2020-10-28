using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class cobro_servicio
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private string _tipo_operacion = "";
        private long _id_cobroservicio = 0;
        private long _id_pedido = 0;
        private int _id_parametroganancia = 0;
        private decimal _monto = 0;
        private DateTime _fecha = DateTime.Now;
        private bool _pagado = true;
        private bool _activo = true;
        private bool _anulado = true;
        private DateTime _fecha_anulado = DateTime.Now;

        private string _error = "";
        //Propiedades públicas
        public string tipo_operacion { get { return _tipo_operacion; } set { _tipo_operacion = value; } }
        public long id_cobroservicio { get { return _id_cobroservicio; } set { _id_cobroservicio = value; } }
        public long id_pedido { get { return _id_pedido; } set { _id_pedido = value; } }
        public int id_parametroganancia { get { return _id_parametroganancia; } set { _id_parametroganancia = value; } }
        public decimal monto { get { return _monto; } set { _monto = value; } }
        public DateTime fecha { get { return _fecha; } set { _fecha = value; } }
        public bool pagado { get { return _pagado; } set { _pagado = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }
        public bool anulado { get { return _anulado; } set { _anulado = value; } }
        public DateTime fecha_anulado { get { return _fecha_anulado; } set { _fecha_anulado = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public cobro_servicio(int id_cobroservicio)
        {
            _id_cobroservicio = id_cobroservicio;
            RecuperarDatos();
        }
        public cobro_servicio(string Tipo_operacion, long Id_cobroservicio, long Id_pedido, int Id_parametroganancia, decimal Monto, DateTime Fecha, bool Pagado, bool Activo, bool Anulado, DateTime Fecha_anulado)
        {
            _tipo_operacion = Tipo_operacion;
            _id_cobroservicio = Id_cobroservicio;
            _id_pedido = Id_pedido;
            _id_parametroganancia = Id_parametroganancia;
            _monto = Monto;
            _fecha = Fecha;
            _pagado = Pagado;
            _activo = Activo;
            _anulado = Anulado;
            _fecha_anulado = Fecha_anulado;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_cobro_servicio_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_cobro_servicio_individual");
                DbParameter Parametro;

                db1.AddInParameter(cmd, "id_cobroservicio", DbType.Int64, _id_cobroservicio);
                db1.AddOutParameter(cmd, "id_pedido", DbType.Int64, 64);
                db1.AddOutParameter(cmd, "id_parametroganancia", DbType.Int32, 32);
                Parametro = cmd.CreateParameter(); Parametro.ParameterName = "@monto"; Parametro.DbType = DbType.Decimal; Parametro.Direction = ParameterDirection.Output; Parametro.Precision = 18; Parametro.Scale = 2; cmd.Parameters.Add(Parametro);
                db1.AddOutParameter(cmd, "fecha", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "pagado", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "anulado", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "fecha_anulado", DbType.DateTime, 30);
                db1.ExecuteNonQuery(cmd);

                _id_pedido = (long)db1.GetParameterValue(cmd, "id_pedido");
                _id_parametroganancia = (int)db1.GetParameterValue(cmd, "id_parametroganancia");
                _monto = (decimal)db1.GetParameterValue(cmd, "monto");
                _fecha = (DateTime)db1.GetParameterValue(cmd, "fecha");
                _pagado = (Boolean)db1.GetParameterValue(cmd, "pagado");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
                _anulado = (Boolean)db1.GetParameterValue(cmd, "anulado");
                _fecha_anulado = (DateTime)db1.GetParameterValue(cmd, "fecha_anulado");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_cobro_servicio");
                db1.AddInParameter(cmd, "tipo_operacion", DbType.String, _tipo_operacion);
                db1.AddInParameter(cmd, "id_cobroservicio", DbType.Int64, _id_cobroservicio);
                db1.AddInParameter(cmd, "id_pedido", DbType.Int64, _id_pedido);
                db1.AddInParameter(cmd, "id_parametroganancia", DbType.Int32, _id_parametroganancia);
                db1.AddInParameter(cmd, "monto", DbType.Decimal, _monto);
                db1.AddInParameter(cmd, "fecha", DbType.DateTime, _fecha);
                db1.AddInParameter(cmd, "pagado", DbType.Boolean, _pagado);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "anulado", DbType.Boolean, _anulado);
                db1.AddInParameter(cmd, "fecha_anulado", DbType.DateTime, _fecha_anulado);
                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_cobroservicio_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_cobroservicio = (int)db1.GetParameterValue(cmd, "id_cobroservicio_aux");
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