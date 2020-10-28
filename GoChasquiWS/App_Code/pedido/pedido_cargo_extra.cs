using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class pedido_cargo_extra
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private string _tipo_operacion = "";
        private long _id_pedidocargoextra = 0;
        private int _id_tipocargoextra = 0;
        private decimal _monto = 0;
        private bool _activo = true;
        private DateTime _fecha = DateTime.Now;
        private long _id_pedido = 0;
        private int _id_parametroganancia = 0;

        private string _error = "";
        //Propiedades públicas
        public string tipo_operacion { get { return _tipo_operacion; } set { _tipo_operacion = value; } }
        public long id_pedidocargoextra { get { return _id_pedidocargoextra; } set { _id_pedidocargoextra = value; } }
        public int id_tipocargoextra { get { return _id_tipocargoextra; } set { _id_tipocargoextra = value; } }
        public decimal monto { get { return _monto; } set { _monto = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }
        public DateTime fecha { get { return _fecha; } set { _fecha = value; } }
        public long id_pedido { get { return _id_pedido; } set { _id_pedido = value; } }
        public int id_parametroganancia { get { return _id_parametroganancia; } set { _id_parametroganancia = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public pedido_cargo_extra(int id_pedidocargoextra)
        {
            _id_pedidocargoextra = id_pedidocargoextra;
            RecuperarDatos();
        }
        public pedido_cargo_extra(string Tipo_operacion, long Id_pedidocargoextra, int Id_tipocargoextra, decimal Monto, bool Activo, DateTime Fecha, long Id_pedido, int Id_parametroganancia)
        {
            _tipo_operacion = Tipo_operacion;
            _id_pedidocargoextra = Id_pedidocargoextra;
            _id_tipocargoextra = Id_tipocargoextra;
            _monto = Monto;
            _activo = Activo;
            _fecha = Fecha;
            _id_pedido = Id_pedido;
            _id_parametroganancia = Id_parametroganancia;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_pedido_cargo_extra_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_pedido_cargo_extra_individual");
                DbParameter Parametro;

                db1.AddInParameter(cmd, "id_pedidocargoextra", DbType.Int64, _id_pedidocargoextra);
                db1.AddOutParameter(cmd, "id_tipocargoextra", DbType.Int32, 32);
                Parametro = cmd.CreateParameter(); Parametro.ParameterName = "@monto"; Parametro.DbType = DbType.Decimal; Parametro.Direction = ParameterDirection.Output; Parametro.Precision = 18; Parametro.Scale = 2; cmd.Parameters.Add(Parametro);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "fecha", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "id_pedido", DbType.Int64, 64);
                db1.AddOutParameter(cmd, "id_parametroganancia", DbType.Int32, 32);
                db1.ExecuteNonQuery(cmd);

                _id_tipocargoextra = (int)db1.GetParameterValue(cmd, "id_tipocargoextra");
                _monto = (decimal)db1.GetParameterValue(cmd, "monto");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
                _fecha = (DateTime)db1.GetParameterValue(cmd, "fecha");
                _id_pedido = (long)db1.GetParameterValue(cmd, "id_pedido");
                _id_parametroganancia = (int)db1.GetParameterValue(cmd, "id_parametroganancia");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_pedido_cargo_extra");
                db1.AddInParameter(cmd, "tipo_operacion", DbType.String, _tipo_operacion);
                db1.AddInParameter(cmd, "id_pedidocargoextra", DbType.Int64, _id_pedidocargoextra);
                db1.AddInParameter(cmd, "id_tipocargoextra", DbType.Int32, _id_tipocargoextra);
                db1.AddInParameter(cmd, "monto", DbType.Decimal, _monto);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "fecha", DbType.DateTime, _fecha);
                db1.AddInParameter(cmd, "id_pedido", DbType.Int64, _id_pedido);
                db1.AddInParameter(cmd, "id_parametroganancia", DbType.Int32, _id_parametroganancia);
                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_pedidocargoextra_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_pedidocargoextra = (int)db1.GetParameterValue(cmd, "id_pedidocargoextra_aux");
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