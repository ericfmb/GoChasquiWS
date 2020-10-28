using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class recibo
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private int _id_recibo = 0;
        private int _id_parametrorecibo = 0;
        private int _num_recibo = 0;
        private string _nombre_cliente = "";
        private string _nit = "";
        private string _concepto = "";
        private decimal _monto_pagado = 0;
        private bool _anulado = true;
        private bool _activo = true;
        private int _id_transaccion = 0;

        private string _error = "";
        //Propiedades públicas
        public int id_recibo { get { return _id_recibo; } set { _id_recibo = value; } }
        public int id_parametrorecibo { get { return _id_parametrorecibo; } set { _id_parametrorecibo = value; } }
        public int num_recibo { get { return _num_recibo; } set { _num_recibo = value; } }
        public string nombre_cliente { get { return _nombre_cliente; } set { _nombre_cliente = value; } }
        public string nit { get { return _nit; } set { _nit = value; } }
        public string concepto { get { return _concepto; } set { _concepto = value; } }
        public decimal monto_pagado { get { return _monto_pagado; } set { _monto_pagado = value; } }
        public bool anulado { get { return _anulado; } set { _anulado = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }
        public int id_transaccion { get { return _id_transaccion; } set { _id_transaccion = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public recibo(int Id_parametrorecibo)
        {
            _id_parametrorecibo = Id_parametrorecibo;
            RecuperarDatos();
        }
        public recibo(int Id_recibo, int Id_parametrorecibo, int Num_recibo, string Nombre_cliente, string Nit, string Concepto, decimal Monto_pagado, bool Anulado, bool Activo, int Id_transaccion)
        {
            _id_recibo = Id_recibo;
            _id_parametrorecibo = Id_parametrorecibo;
            _num_recibo = Num_recibo;
            _nombre_cliente = Nombre_cliente;
            _nit = Nit;
            _concepto = Concepto;
            _monto_pagado = Monto_pagado;
            _anulado = Anulado;
            _activo = Activo;
            _id_transaccion = Id_transaccion;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_recibo_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_recibo_individual");
                db1.AddInParameter(cmd, "id_recibo", DbType.Int32, _id_recibo);
                db1.AddOutParameter(cmd, "id_parametrorecibo", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "num_recibo", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "nombre_cliente", DbType.String, 500);
                db1.AddOutParameter(cmd, "nit", DbType.String, 20);
                db1.AddOutParameter(cmd, "concepto", DbType.String, 500);
                db1.AddOutParameter(cmd, "monto_pagado", DbType.Decimal, 32);
                db1.AddOutParameter(cmd, "anulado", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "id_transaccion", DbType.Int32, 32);
                db1.ExecuteNonQuery(cmd);

                _id_recibo = (int)db1.GetParameterValue(cmd, "id_recibo");
                _id_parametrorecibo = (int)db1.GetParameterValue(cmd, "id_parametrorecibo");
                _num_recibo = (int)db1.GetParameterValue(cmd, "num_recibo");
                _nombre_cliente = (string)db1.GetParameterValue(cmd, "nombre_cliente");
                _nit = (string)db1.GetParameterValue(cmd, "nit");
                _concepto = (string)db1.GetParameterValue(cmd, "concepto");
                _monto_pagado = (decimal)db1.GetParameterValue(cmd, "monto_pagado");
                _anulado = (Boolean)db1.GetParameterValue(cmd, "anulado");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
                _id_transaccion = (int)db1.GetParameterValue(cmd, "id_transaccion");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_recibo");
                db1.AddInParameter(cmd, "id_recibo", DbType.Int32, _id_recibo);
                db1.AddInParameter(cmd, "id_parametrorecibo", DbType.Int32, _id_parametrorecibo);
                db1.AddInParameter(cmd, "num_recibo", DbType.Int32, _num_recibo);
                db1.AddInParameter(cmd, "nombre_cliente", DbType.String, _nombre_cliente);
                db1.AddInParameter(cmd, "nit", DbType.String, _nit);
                db1.AddInParameter(cmd, "concepto", DbType.String, _concepto);
                db1.AddInParameter(cmd, "monto_pagado", DbType.Decimal, _monto_pagado);
                db1.AddInParameter(cmd, "anulado", DbType.Boolean, _anulado);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "id_transaccion", DbType.Int32, _id_transaccion);

                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_recibo_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_recibo = (int)db1.GetParameterValue(cmd, "id_recibo_aux");
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