using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class factura
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private int _id_factura = 0;
        private int _id_transaccion = 0;
        private int _num_factura = 0;
        private string _codigo_control = "";
        private decimal _monto = 0;
        private decimal _monto_facturado = 0;
        private string _concepto = "";
        private string _nombre_cliente = "";
        private string _nit = "";
        private string _nro_autorizacion = "";
        private string _llave_dosificacion = "";
        private bool _anulado = true;
        private bool _activo = true;
        private int _id_parametrofacturacion = 0;

        private string _error = "";
        //Propiedades públicas
        public int id_factura { get { return _id_factura; } set { _id_factura = value; } }
        public int id_transaccion { get { return _id_transaccion; } set { _id_transaccion = value; } }
        public int num_factura { get { return _num_factura; } set { _num_factura = value; } }
        public string codigo_control { get { return _codigo_control; } set { _codigo_control = value; } }
        public decimal monto { get { return _monto; } set { _monto = value; } }
        public decimal monto_facturado { get { return _monto_facturado; } set { _monto_facturado = value; } }
        public string concepto { get { return _concepto; } set { _concepto = value; } }
        public string nombre_cliente { get { return _nombre_cliente; } set { _nombre_cliente = value; } }
        public string nit { get { return _nit; } set { _nit = value; } }
        public string nro_autorizacion { get { return _nro_autorizacion; } set { _nro_autorizacion = value; } }
        public string llave_dosificacion { get { return _llave_dosificacion; } set { _llave_dosificacion = value; } }
        public bool anulado { get { return _anulado; } set { _anulado = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }
        public int id_parametrofacturacion { get { return _id_parametrofacturacion; } set { _id_parametrofacturacion = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public factura(int Id_factura)
        {
            _id_factura = Id_factura;
            RecuperarDatos();
        }
        public factura(int Id_factura, int Id_transaccion, int Num_factura, string Codigo_control, decimal Monto, decimal Monto_facturado, string Concepto, string Nombre_cliente, string Nit, string Nro_autorizacion, string Llave_dosificacion, bool Anulado, bool Activo, int Id_parametrofacturacion)
        {
            _id_factura = Id_factura;
            _id_transaccion = Id_transaccion;
            _num_factura = Num_factura;
            _codigo_control = Codigo_control;
            _monto = Monto;
            _monto_facturado = Monto_facturado;
            _concepto = Concepto;
            _nombre_cliente = Nombre_cliente;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_factura_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_factura_individual");
                db1.AddInParameter(cmd, "id_factura", DbType.Int32, _id_factura);
                db1.AddOutParameter(cmd, "id_transaccion", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "num_factura", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "codigo_control", DbType.String, 50);
                db1.AddOutParameter(cmd, "monto", DbType.Decimal, 32);
                db1.AddOutParameter(cmd, "monto_facturado", DbType.Decimal, 32);
                db1.AddOutParameter(cmd, "concepto", DbType.String, 500);
                db1.AddOutParameter(cmd, "nombre_cliente", DbType.String, 500);
                db1.AddOutParameter(cmd, "nit", DbType.String, 20);
                db1.AddOutParameter(cmd, "nro_autorizacion", DbType.String, 20);
                db1.AddOutParameter(cmd, "llave_dosificacion", DbType.String, 256);
                db1.AddOutParameter(cmd, "anulado", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "id_parametrofacturacion", DbType.Int32, 32);
                db1.ExecuteNonQuery(cmd);

                _id_transaccion = (int)db1.GetParameterValue(cmd, "id_transaccion");
                _num_factura = (int)db1.GetParameterValue(cmd, "num_factura");
                _codigo_control = (string)db1.GetParameterValue(cmd, "codigo_control");
                _monto = (decimal)db1.GetParameterValue(cmd, "monto");
                _monto_facturado = (decimal)db1.GetParameterValue(cmd, "monto_facturado");
                _concepto = (string)db1.GetParameterValue(cmd, "concepto");
                _nombre_cliente = (string)db1.GetParameterValue(cmd, "nombre_cliente");
                _nit = (string)db1.GetParameterValue(cmd, "nit");
                _nro_autorizacion = (string)db1.GetParameterValue(cmd, "nro_autorizacion");
                _llave_dosificacion = (string)db1.GetParameterValue(cmd, "llave_dosificacion");
                _anulado = (Boolean)db1.GetParameterValue(cmd, "anulado");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
                _id_parametrofacturacion = (int)db1.GetParameterValue(cmd, "id_parametrofacturacion");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_factura");
                db1.AddInParameter(cmd, "id_factura", DbType.Int32, _id_factura);
                db1.AddInParameter(cmd, "id_transaccion", DbType.Int32, _id_transaccion);
                db1.AddInParameter(cmd, "num_factura", DbType.Int32, _num_factura);
                db1.AddInParameter(cmd, "codigo_control", DbType.String, _codigo_control);
                db1.AddInParameter(cmd, "monto", DbType.Decimal, _monto);
                db1.AddInParameter(cmd, "monto_facturado", DbType.Decimal, _monto_facturado);
                db1.AddInParameter(cmd, "concepto", DbType.String, _concepto);
                db1.AddInParameter(cmd, "nombre_cliente", DbType.String, _nombre_cliente);
                db1.AddInParameter(cmd, "nit", DbType.String, _nit);
                db1.AddInParameter(cmd, "nro_autorizacion", DbType.String, _nro_autorizacion);
                db1.AddInParameter(cmd, "llave_dosificacion", DbType.String, _llave_dosificacion);
                db1.AddInParameter(cmd, "anulado", DbType.Boolean, _anulado);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "id_parametrofacturacion", DbType.Int32, _id_parametrofacturacion);

                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_factura_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_factura = (int)db1.GetParameterValue(cmd, "id_factura_aux");
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