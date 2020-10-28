using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class auditoria
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private int _id_auditoria = 0;
        private decimal _monto = 0;
        private decimal _monto_facturado = 0;
        private DateTime _fecha = DateTime.Now;
        private DateTime _fecha_pago = DateTime.Now;
        private int _id_pedido = 0;
        private bool _anulado = true;
        private DateTime _fecha_anulado = DateTime.Now;

        private string _error = "";
        //Propiedades públicas
        public int id_auditoria { get { return _id_auditoria; } set { _id_auditoria = value; } }
        public decimal monto { get { return _monto; } set { _monto = value; } }
        public decimal monto_facturado { get { return _monto_facturado; } set { _monto_facturado = value; } }
        public DateTime fecha { get { return _fecha; } set { _fecha = value; } }
        public DateTime fecha_pago { get { return _fecha_pago; } set { _fecha_pago = value; } }
        public int id_pedido { get { return _id_pedido; } set { _id_pedido = value; } }
        public bool anulado { get { return _anulado; } set { _anulado = value; } }
        public DateTime fecha_anulado { get { return _fecha_anulado; } set { _fecha_anulado = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        //public auditoria(int Id_auditoria)
        //{
        //    _id_auditoria = Id_auditoria;
        //    RecuperarDatos();
        //}
        //public auditoria(int Id_auditoria, decimal Monto, decimal Monto_facturado, DateTime Fecha, DateTime Fecha_pago, int Id_pedido, bool Anulado, DateTime Fecha_anulado)
        //{
        //    _id_auditoria = Id_auditoria;
        //    _monto = Monto;
        //    _monto_facturado = Monto_facturado;
        //    _fecha = Fecha;
        //    _fecha_pago = Fecha_pago;
        //    _id_pedido = Id_pedido;
        //    _anulado = Anulado;
        //    _fecha_anulado = Fecha_anulado;
        //}
        //#endregion

        //#region Métodos que NO requieren constructor
        //public static DataTable Lista(int Id_usuario)
        //{
        //    DbCommand cmd = db1.GetStoredProcCommand("lista_auditoria_todos");
        //    db1.AddInParameter(cmd, "id_usuario", DbType.Int32, Id_usuario); // Enviar el código del usuario conectado
        //    cmd.CommandTimeout = int.Parse(ConfigurationManager.AppSettings["CommandTimeout"]);
        //    return db1.ExecuteDataSet(cmd).Tables[0];
        //}
        //#endregion

        //#region Métodos que requieren constructor
        //private void RecuperarDatos()
        //{
        //    try
        //    {
        //        DbCommand cmd = db1.GetStoredProcCommand("lista_auditoria_individual");
        //        db1.AddInParameter(cmd, "id_auditoria", DbType.Int32, _id_auditoria);
        //        db1.AddOutParameter(cmd, "monto", DbType.Decimal, 32);
        //        db1.AddOutParameter(cmd, "monto_facturado", DbType.Decimal, 32);
        //        db1.AddOutParameter(cmd, "fecha", DbType.DateTime, 30);
        //        db1.AddOutParameter(cmd, "fecha_pago", DbType.DateTime, 30);
        //        db1.AddOutParameter(cmd, "id_pedido", DbType.Int32, 32);
        //        db1.AddOutParameter(cmd, "anulado", DbType.Boolean, 1);
        //        db1.AddOutParameter(cmd, "fecha_anulado", DbType.DateTime, 30);
        //        db1.ExecuteNonQuery(cmd);

        //        _monto = (decimal)db1.GetParameterValue(cmd, "monto");
        //        _monto_facturado = (decimal)db1.GetParameterValue(cmd, "monto_facturado");
        //        _fecha = (DateTime)db1.GetParameterValue(cmd, "fecha");
        //        _fecha_pago = (DateTime)db1.GetParameterValue(cmd, "fecha_pago");
        //        _id_pedido = (int)db1.GetParameterValue(cmd, "id_pedido");
        //        _anulado = (Boolean)db1.GetParameterValue(cmd, "anulado");
        //        _fecha_anulado = (DateTime)db1.GetParameterValue(cmd, "fecha_anulado");
        //    }
        //    catch { }
        //}

        //public string ABM(int context_id_usuario)
        //{
        //    string resultado = "";
        //    try
        //    {
        //        DbCommand cmd = db1.GetStoredProcCommand("abm_auditoria");
        //        db1.AddInParameter(cmd, "id_auditoria", DbType.Int32, _id_auditoria);
        //        db1.AddInParameter(cmd, "monto", DbType.Decimal, _monto);
        //        db1.AddInParameter(cmd, "monto_facturado", DbType.Decimal, _monto_facturado);
        //        db1.AddInParameter(cmd, "fecha", DbType.DateTime, _fecha);
        //        db1.AddInParameter(cmd, "fecha_pago", DbType.DateTime, _fecha_pago);
        //        db1.AddInParameter(cmd, "id_pedido", DbType.Int32, _id_pedido);
        //        db1.AddInParameter(cmd, "anulado", DbType.Boolean, _anulado);
        //        db1.AddInParameter(cmd, "fecha_anulado", DbType.DateTime, _fecha_anulado);

        //        db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
        //        db1.AddOutParameter(cmd, "id_auditoria_aux", DbType.Int32, 32);
        //        db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
        //        db1.AddOutParameter(cmd, "error", DbType.String, 250);
        //        db1.ExecuteNonQuery(cmd);
        //        resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
        //        _id_auditoria = (int)db1.GetParameterValue(cmd, "id_auditoria_aux");
        //        _error = (string)db1.GetParameterValue(cmd, "error");
        //        return resultado;
        //    }
        //    catch (Exception ex)
        //    {
        //        _error = ex.Message;
        //        resultado = "Se produjo un error al registrar";
        //        return resultado;
        //    }
        //}
        #endregion
    }
}