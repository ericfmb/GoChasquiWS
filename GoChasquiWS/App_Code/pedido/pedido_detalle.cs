using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class pedido_detalle
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private string _tipo_operacion = "";
        private long _id_pedidodetalle = 0;
        private long _id_pedido = 0;
        private long _id_producto = 0;
        private decimal _monto = 0;
        private int _unidades = 0;
        private DateTime _fecha = DateTime.Now;
        private bool _activo = true;
        private string _detalle_extra = "";

        private string _error = "";
        //Propiedades públicas
        public string tipo_operacion { get { return _tipo_operacion; } set { _tipo_operacion = value; } }
        public long id_pedidodetalle { get { return _id_pedidodetalle; } set { _id_pedidodetalle = value; } }
        public long id_pedido { get { return _id_pedido; } set { _id_pedido = value; } }
        public long id_producto { get { return _id_producto; } set { _id_producto = value; } }
        public decimal monto { get { return _monto; } set { _monto = value; } }
        public int unidades { get { return _unidades; } set { _unidades = value; } }
        public DateTime fecha { get { return _fecha; } set { _fecha = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }
        public string detalle_extra { get { return _detalle_extra; } set { _detalle_extra = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public pedido_detalle(long Id_pedidodetalle)
        {
            _id_pedidodetalle = Id_pedidodetalle;
            RecuperarDatos();
        }
        public pedido_detalle(string Tipo_operacion, long Id_pedidodetalle, long Id_pedido, long Id_producto, decimal Monto, int Unidades, DateTime Fecha, bool Activo, string Detalle_extra)
        {
            _tipo_operacion = Tipo_operacion;
            _id_pedidodetalle = Id_pedidodetalle;
            _id_pedido = Id_pedido;
            _id_producto = Id_producto;
            _monto = Monto;
            _unidades = Unidades;
            _fecha = Fecha;
            _activo = Activo;
            _detalle_extra = Detalle_extra;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_pedido_detalle_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_pedido_detalle_individual");
                DbParameter Parametro;

                db1.AddInParameter(cmd, "id_pedidodetalle", DbType.Int64, _id_pedidodetalle);
                db1.AddOutParameter(cmd, "id_pedido", DbType.Int64, 64);
                db1.AddOutParameter(cmd, "id_producto", DbType.Int64, 64);
                Parametro = cmd.CreateParameter(); Parametro.ParameterName = "@monto"; Parametro.DbType = DbType.Decimal; Parametro.Direction = ParameterDirection.Output; Parametro.Precision = 18; Parametro.Scale = 2; cmd.Parameters.Add(Parametro);
                db1.AddOutParameter(cmd, "unidades", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "fecha", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "detalle_extra", DbType.String, 100);
                db1.ExecuteNonQuery(cmd);

                _id_pedido = (long)db1.GetParameterValue(cmd, "id_pedido");
                _id_producto = (long)db1.GetParameterValue(cmd, "id_producto");
                _monto = (decimal)db1.GetParameterValue(cmd, "monto");
                _unidades = (int)db1.GetParameterValue(cmd, "unidades");
                _fecha = (DateTime)db1.GetParameterValue(cmd, "fecha");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
                _detalle_extra = (string)db1.GetParameterValue(cmd, "detalle_extra");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_pedido_detalle");
                db1.AddInParameter(cmd, "tipo_operacion", DbType.String, _tipo_operacion);
                db1.AddInParameter(cmd, "id_pedidodetalle", DbType.Int64, _id_pedidodetalle);
                db1.AddInParameter(cmd, "id_pedido", DbType.Int64, _id_pedido);
                db1.AddInParameter(cmd, "id_producto", DbType.Int64, _id_producto);
                db1.AddInParameter(cmd, "monto", DbType.Decimal, _monto);
                db1.AddInParameter(cmd, "unidades", DbType.Int32, _unidades);
                db1.AddInParameter(cmd, "fecha", DbType.DateTime, _fecha);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "detalle_extra", DbType.String, _detalle_extra);
                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_pedidodetalle_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_pedidodetalle = (int)db1.GetParameterValue(cmd, "id_pedidodetalle_aux");
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