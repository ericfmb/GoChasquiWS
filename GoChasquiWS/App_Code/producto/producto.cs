using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class producto
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private int _id_producto = 0;
        private int _id_tipoproducto = 0;
        private string _nombre = "";
        private string _descripcion = "";
        private decimal _precio_unidad = 0;
        private bool _activo = true;
        private string _link = "";
        private DateTime _fecha = DateTime.Now;
        private DateTime _fecha_inicio = DateTime.Now;
        private DateTime _fecha_fin = DateTime.Now;
        private decimal _stock_min = 0;
        private decimal _stock_max = 0;
        private int _id_cliente = 0;
        private bool _disponible = true;

        private string _error = "";
        //Propiedades públicas
        public int id_producto { get { return _id_producto; } set { _id_producto = value; } }
        public int id_tipoproducto { get { return _id_tipoproducto; } set { _id_tipoproducto = value; } }
        public string nombre { get { return _nombre; } set { _nombre = value; } }
        public string descripcion { get { return _descripcion; } set { _descripcion = value; } }
        public decimal precio_unidad { get { return _precio_unidad; } set { _precio_unidad = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }
        public string link { get { return _link; } set { _link = value; } }
        public DateTime fecha { get { return _fecha; } set { _fecha = value; } }
        public DateTime fecha_inicio { get { return _fecha_inicio; } set { _fecha_inicio = value; } }
        public DateTime fecha_fin { get { return _fecha_fin; } set { _fecha_fin = value; } }
        public decimal stock_min { get { return _stock_min; } set { _stock_min = value; } }
        public decimal stock_max { get { return _stock_max; } set { _stock_max = value; } }
        public int id_cliente { get { return _id_cliente; } set { _id_cliente = value; } }
        public bool disponible { get { return _disponible; } set { _disponible = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public producto(int Id_producto)
        {
            _id_producto = Id_producto;
            RecuperarDatos();
        }
        public producto(int Id_producto, int Id_tipoproducto, string Nombre, string Descripcion, decimal Precio_unidad, bool Activo, string Link, DateTime Fecha, DateTime Fecha_inicio, DateTime Fecha_fin, decimal Stock_min, decimal Stock_max, int Id_cliente, bool Disponible)
        {
            _id_producto = Id_producto;
            _id_tipoproducto = Id_tipoproducto;
            _nombre = Nombre;
            _descripcion = Descripcion;
            _precio_unidad = Precio_unidad;
            _activo = Activo;
            _link = Link;
            _fecha = Fecha;
            _fecha_inicio = Fecha_inicio;
            _fecha_fin = Fecha_fin;
            _stock_min = Stock_min;
            _stock_max = Stock_max;
            _id_cliente = Id_cliente;
            _disponible = Disponible;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_producto_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_producto_individual");
                db1.AddInParameter(cmd, "id_producto", DbType.Int32, _id_producto);
                db1.AddOutParameter(cmd, "id_tipoproducto", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "nombre", DbType.String, 500);
                db1.AddOutParameter(cmd, "descripcion", DbType.String, 2000);
                db1.AddOutParameter(cmd, "precio_unidad", DbType.Decimal, 32);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "link", DbType.String, 500);
                db1.AddOutParameter(cmd, "fecha", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "fecha_inicio", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "fecha_fin", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "stock_min", DbType.Decimal, 32);
                db1.AddOutParameter(cmd, "stock_max", DbType.Decimal, 32);
                db1.AddOutParameter(cmd, "id_cliente", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "disponible", DbType.Boolean, 1);
                db1.ExecuteNonQuery(cmd);

                _id_tipoproducto = (int)db1.GetParameterValue(cmd, "id_tipoproducto");
                _nombre = (string)db1.GetParameterValue(cmd, "nombre");
                _descripcion = (string)db1.GetParameterValue(cmd, "descripcion");
                _precio_unidad = (decimal)db1.GetParameterValue(cmd, "precio_unidad");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
                _link = (string)db1.GetParameterValue(cmd, "link");
                _fecha = (DateTime)db1.GetParameterValue(cmd, "fecha");
                _fecha_inicio = (DateTime)db1.GetParameterValue(cmd, "fecha_inicio");
                _fecha_fin = (DateTime)db1.GetParameterValue(cmd, "fecha_fin");
                _stock_min = (decimal)db1.GetParameterValue(cmd, "stock_min");
                _stock_max = (decimal)db1.GetParameterValue(cmd, "stock_max");
                _id_cliente = (int)db1.GetParameterValue(cmd, "id_cliente");
                _disponible = (Boolean)db1.GetParameterValue(cmd, "disponible");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_producto");
                db1.AddInParameter(cmd, "id_producto", DbType.Int32, _id_producto);
                db1.AddInParameter(cmd, "id_tipoproducto", DbType.Int32, _id_tipoproducto);
                db1.AddInParameter(cmd, "nombre", DbType.String, _nombre);
                db1.AddInParameter(cmd, "descripcion", DbType.String, _descripcion);
                db1.AddInParameter(cmd, "precio_unidad", DbType.Decimal, _precio_unidad);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "link", DbType.String, _link);
                db1.AddInParameter(cmd, "fecha", DbType.DateTime, _fecha);
                db1.AddInParameter(cmd, "fecha_inicio", DbType.DateTime, _fecha_inicio);
                db1.AddInParameter(cmd, "fecha_fin", DbType.DateTime, _fecha_fin);
                db1.AddInParameter(cmd, "stock_min", DbType.Decimal, _stock_min);
                db1.AddInParameter(cmd, "stock_max", DbType.Decimal, _stock_max);
                db1.AddInParameter(cmd, "id_cliente", DbType.Int32, _id_cliente);
                db1.AddInParameter(cmd, "disponible", DbType.Boolean, _disponible);
                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_producto_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_producto = (int)db1.GetParameterValue(cmd, "id_producto_aux");
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