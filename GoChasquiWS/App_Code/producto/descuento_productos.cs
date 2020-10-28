using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class descuento_productos
    {//Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private int _id_descuento = 0;
        private int _id_producto = 0;
        private DateTime _fecha_inicio = DateTime.Now;
        private DateTime _fecha_fin = DateTime.Now;
        private decimal _porcentaje = 0;
        private bool _activo = true;
        private int _nro_ventas = 0;

        private string _error = "";
        //Propiedades públicas
        public int id_descuento { get { return _id_descuento; } set { _id_descuento = value; } }
        public int id_producto { get { return _id_producto; } set { _id_producto = value; } }
        public DateTime fecha_inicio { get { return _fecha_inicio; } set { _fecha_inicio = value; } }
        public DateTime fecha_fin { get { return _fecha_fin; } set { _fecha_fin = value; } }
        public decimal porcentaje { get { return _porcentaje; } set { _porcentaje = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }
        public int nro_ventas { get { return _nro_ventas; } set { _nro_ventas = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public descuento_productos(int Id_descuento)
        {
            _id_descuento = Id_descuento;
            RecuperarDatos();
        }
        public descuento_productos(int Id_descuento, int Id_producto, DateTime Fecha_inicio, DateTime Fecha_fin, decimal Porcentaje, bool Activo, int Nro_ventas)
        {
            _id_descuento = Id_descuento;
            _id_producto = Id_producto;
            _fecha_inicio = Fecha_inicio;
            _fecha_fin = Fecha_fin;
            _porcentaje = Porcentaje;
            _activo = Activo;
            _nro_ventas = Nro_ventas;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_descuento_productos_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_descuento_productos_individual");
                db1.AddInParameter(cmd, "id_descuento", DbType.Int32, _id_descuento);
                db1.AddOutParameter(cmd, "id_producto", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "fecha_inicio", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "fecha_fin", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "porcentaje", DbType.Decimal, 32);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "nro_ventas", DbType.Int32, 32);
                db1.ExecuteNonQuery(cmd);

                _id_descuento = (int)db1.GetParameterValue(cmd, "id_descuento");
                _id_producto = (int)db1.GetParameterValue(cmd, "id_producto");
                _fecha_inicio = (DateTime)db1.GetParameterValue(cmd, "fecha_inicio");
                _fecha_fin = (DateTime)db1.GetParameterValue(cmd, "fecha_fin");
                _porcentaje = (Decimal)db1.GetParameterValue(cmd, "porcentaje");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
                _nro_ventas = (int)db1.GetParameterValue(cmd, "nro_ventas");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_descuento_productos");
                db1.AddInParameter(cmd, "id_descuento", DbType.Int32, _id_descuento);
                db1.AddInParameter(cmd, "id_producto", DbType.Int32, _id_producto);
                db1.AddInParameter(cmd, "fecha_inicio", DbType.DateTime, _fecha_inicio);
                db1.AddInParameter(cmd, "fecha_fin", DbType.DateTime, _fecha_fin);
                db1.AddInParameter(cmd, "porcentaje", DbType.Decimal, _porcentaje);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "nro_ventas", DbType.Int32, _nro_ventas);
                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_descuento_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_descuento = (int)db1.GetParameterValue(cmd, "id_descuento_aux");
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