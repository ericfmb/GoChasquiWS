using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class forma_pago
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private int _id_formapago = 0;
        private int _id_transaccion = 0;
        private int _id_tipopago = 0;
        private decimal _monto = 0;
        private DateTime _fecha = DateTime.Now;
        private bool _anulado = true;
        private bool _activo = true;
        private DateTime _fecha_anulado = DateTime.Now;

        private string _error = "";
        //Propiedades públicas
        public int id_formapago { get { return _id_formapago; } set { _id_formapago = value; } }
        public int id_transaccion { get { return _id_transaccion; } set { _id_transaccion = value; } }
        public int id_tipopago { get { return _id_tipopago; } set { _id_tipopago = value; } }
        public decimal monto { get { return _monto; } set { _monto = value; } }
        public DateTime fecha { get { return _fecha; } set { _fecha = value; } }
        public bool anulado { get { return _anulado; } set { _anulado = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }
        public DateTime fecha_anulado { get { return _fecha_anulado; } set { _fecha_anulado = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public forma_pago(int Id_formapago)
        {
            _id_formapago = Id_formapago;
            RecuperarDatos();
        }
        public forma_pago(int Id_formapago, int Id_transaccion, int Id_tipopago, decimal Monto, DateTime Fecha, bool Anulado, bool Activo, DateTime Fecha_anulado)
        {
            _id_formapago = Id_formapago;
            _id_transaccion = Id_transaccion;
            _id_tipopago = Id_tipopago;
            _monto = Monto;
            _fecha = Fecha;
            _anulado = Anulado;
            _activo = Activo;
            _fecha_anulado = Fecha_anulado;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_forma_pago_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_forma_pago_individual");
                db1.AddInParameter(cmd, "id_formapago", DbType.Int32, _id_formapago);
                db1.AddOutParameter(cmd, "id_transaccion", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "id_tipopago", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "monto", DbType.Decimal, 32);
                db1.AddOutParameter(cmd, "fecha", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "anulado", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "fecha_anulado", DbType.DateTime, 30);
                db1.ExecuteNonQuery(cmd);

                _id_transaccion = (int)db1.GetParameterValue(cmd, "id_transaccion");
                _id_tipopago = (int)db1.GetParameterValue(cmd, "id_tipopago");
                _monto = (decimal)db1.GetParameterValue(cmd, "monto");
                _fecha = (DateTime)db1.GetParameterValue(cmd, "fecha");
                _anulado = (Boolean)db1.GetParameterValue(cmd, "anulado");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
                _fecha_anulado = (DateTime)db1.GetParameterValue(cmd, "fecha_anulado");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_forma_pago");
                db1.AddInParameter(cmd, "id_formapago", DbType.Int32, _id_formapago);
                db1.AddInParameter(cmd, "id_transaccion", DbType.Int32, _id_transaccion);
                db1.AddInParameter(cmd, "id_tipopago", DbType.Int32, _id_tipopago);
                db1.AddInParameter(cmd, "monto", DbType.Decimal, _monto);
                db1.AddInParameter(cmd, "fecha", DbType.DateTime, _fecha);
                db1.AddInParameter(cmd, "anulado", DbType.Boolean, _anulado);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "fecha_anulado", DbType.DateTime, _fecha_anulado);

                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_formapago_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_formapago = (int)db1.GetParameterValue(cmd, "id_formapago_aux");
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