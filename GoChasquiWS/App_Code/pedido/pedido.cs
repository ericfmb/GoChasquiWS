using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class pedido
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private string _tipo_operacion = "";
        private long _id_pedido = 0;
        private DateTime _fecha_pedido = DateTime.Now;
        private DateTime _fecha_entrega = DateTime.Now;
        private bool _pagado = true;
        private decimal _monto = 0;
        private bool _entregado = true;
        private bool _activo = true;
        private string _latitud = "";
        private string _longitud = "";
        private long _id_usuario = 0;
        private bool _delivery = true;

        private string _error = "";
        //Propiedades públicas
        public string tipo_operacion { get { return _tipo_operacion; } set { _tipo_operacion = value; } }
        public long id_pedido { get { return _id_pedido; } set { _id_pedido = value; } }
        public DateTime fecha_pedido { get { return _fecha_pedido; } set { _fecha_pedido = value; } }
        public DateTime fecha_entrega { get { return _fecha_entrega; } set { _fecha_entrega = value; } }
        public bool pagado { get { return _pagado; } set { _pagado = value; } }
        public decimal monto { get { return _monto; } set { _monto = value; } }
        public bool entregado { get { return _entregado; } set { _entregado = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }
        public string latitud { get { return _latitud; } set { _latitud = value; } }
        public string longitud { get { return _longitud; } set { _longitud = value; } }
        public long id_usuario { get { return _id_usuario; } set { _id_usuario = value; } }
        public bool delivery { get { return _delivery; } set { _delivery = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public pedido(long Id_pedido)
        {
            _id_pedido = Id_pedido;
            RecuperarDatos();
        }
        public pedido(string Tipo_operacion, long Id_pedido, DateTime Fecha_pedido, DateTime Fecha_entrega, bool Pagado, decimal Monto, bool Entregado, bool Activo, string Latitud, string Longitud, long Id_usuario, bool Delivery)
        {
            _tipo_operacion = Tipo_operacion;
            _id_pedido = Id_pedido;
            _fecha_pedido = Fecha_pedido;
            _fecha_entrega = Fecha_entrega;
            _pagado = Pagado;
            _monto = Monto;
            _entregado = Entregado;
            _activo = Activo;
            _latitud = Latitud;
            _longitud = Longitud;
            _id_usuario = Id_usuario;
            _delivery = Delivery;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_pedido_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_pedido_individual");
                DbParameter Parametro;

                db1.AddInParameter(cmd, "id_pedido", DbType.Int64, _id_pedido);
                db1.AddOutParameter(cmd, "fecha_pedido", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "fecha_entrega", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "pagado", DbType.Boolean, 1);
                Parametro = cmd.CreateParameter(); Parametro.ParameterName = "@monto"; Parametro.DbType = DbType.Decimal; Parametro.Direction = ParameterDirection.Output; Parametro.Precision = 18; Parametro.Scale = 2; cmd.Parameters.Add(Parametro);
                db1.AddOutParameter(cmd, "entregado", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "latitud", DbType.String, 20);
                db1.AddOutParameter(cmd, "longitud", DbType.String, 20);
                db1.AddOutParameter(cmd, "id_usuario", DbType.Int64, 64);
                db1.AddOutParameter(cmd, "delivery", DbType.Boolean, 1);
                db1.ExecuteNonQuery(cmd);

                _fecha_pedido = (DateTime)db1.GetParameterValue(cmd, "fecha_pedido");
                _fecha_entrega = (DateTime)db1.GetParameterValue(cmd, "fecha_entrega");
                _pagado = (Boolean)db1.GetParameterValue(cmd, "pagado");
                _monto = (decimal)db1.GetParameterValue(cmd, "monto");
                _entregado = (Boolean)db1.GetParameterValue(cmd, "entregado");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
                _latitud = (string)db1.GetParameterValue(cmd, "latitud");
                _longitud = (string)db1.GetParameterValue(cmd, "longitud");
                _id_usuario = (long)db1.GetParameterValue(cmd, "id_usuario");
                _delivery = (Boolean)db1.GetParameterValue(cmd, "delivery");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_pedido");
                db1.AddInParameter(cmd, "tipo_operacion", DbType.String, _tipo_operacion);
                db1.AddInParameter(cmd, "id_pedido", DbType.Int64, _id_pedido);
                db1.AddInParameter(cmd, "fecha_pedido", DbType.DateTime, _fecha_pedido);
                db1.AddInParameter(cmd, "fecha_entrega", DbType.DateTime, _fecha_entrega);
                db1.AddInParameter(cmd, "pagado", DbType.Boolean, _pagado);
                db1.AddInParameter(cmd, "monto", DbType.Decimal, _monto);
                db1.AddInParameter(cmd, "entregado", DbType.Boolean, _entregado);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "latitud", DbType.String, _latitud);
                db1.AddInParameter(cmd, "longitud", DbType.String, _longitud);
                db1.AddInParameter(cmd, "id_usuario", DbType.Int64, _id_usuario);
                db1.AddInParameter(cmd, "delivery", DbType.Boolean, _delivery);
                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_pedido_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_pedido = (int)db1.GetParameterValue(cmd, "id_pedido_aux");
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