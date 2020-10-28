using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class multimedia_producto
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private int _id_multimediaproducto = 0;
        private int _id_tipoarchivo = 0;
        private int _id_producto = 0;
        private string _nombre = "";
        private string _ruta = "";
        private bool _activo = true;
        private DateTime _fecha = DateTime.Now;
        private DateTime _fecha_fin = DateTime.Now;

        private string _error = "";
        //Propiedades públicas
        public int id_multimediaproducto { get { return _id_multimediaproducto; } set { _id_multimediaproducto = value; } }
        public int id_tipoarchivo { get { return _id_tipoarchivo; } set { _id_tipoarchivo = value; } }
        public int id_producto { get { return _id_producto; } set { _id_producto = value; } }
        public string nombre { get { return _nombre; } set { _nombre = value; } }
        public string ruta { get { return _ruta; } set { _ruta = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }
        public DateTime fecha { get { return _fecha; } set { _fecha = value; } }
        public DateTime fecha_fin { get { return _fecha_fin; } set { _fecha_fin = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public multimedia_producto(int Id_multimediaproducto)
        {
            _id_multimediaproducto = Id_multimediaproducto;
            RecuperarDatos();
        }
        public multimedia_producto(int Id_multimediaproducto, int Id_tipoarchivo, int Id_producto, string Nombre, string Ruta, bool Activo, DateTime Fecha, DateTime Fecha_fin)
        {
            _id_multimediaproducto = Id_multimediaproducto;
            _id_tipoarchivo = Id_tipoarchivo;
            _id_producto = Id_producto;
            _nombre = Nombre;
            _ruta = Ruta;
            _activo = Activo;
            _fecha = Fecha;
            _fecha_fin = Fecha_fin;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_multimedia_producto_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_multimedia_producto_individual");
                db1.AddInParameter(cmd, "id_multimediaproducto", DbType.Int32, _id_multimediaproducto);
                db1.AddOutParameter(cmd, "id_tipoarchivo", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "id_producto", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "nombre", DbType.String, 200);
                db1.AddOutParameter(cmd, "ruta", DbType.String, 500);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "fecha", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "fecha_fin", DbType.DateTime, 30);
                db1.ExecuteNonQuery(cmd);

                _id_tipoarchivo = (int)db1.GetParameterValue(cmd, "id_tipoarchivo");
                _id_producto = (int)db1.GetParameterValue(cmd, "id_producto");
                _nombre = (string)db1.GetParameterValue(cmd, "nombre");
                _ruta = (string)db1.GetParameterValue(cmd, "ruta");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
                _fecha = (DateTime)db1.GetParameterValue(cmd, "fecha");
                _fecha_fin = (DateTime)db1.GetParameterValue(cmd, "fecha_fin");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_multimedia_producto");
                db1.AddInParameter(cmd, "id_multimediaproducto", DbType.Int32, _id_multimediaproducto);
                db1.AddInParameter(cmd, "id_tipoarchivo", DbType.Int32, _id_tipoarchivo);
                db1.AddInParameter(cmd, "id_producto", DbType.Int32, _id_producto);
                db1.AddInParameter(cmd, "nombre", DbType.String, _nombre);
                db1.AddInParameter(cmd, "ruta", DbType.String, _ruta);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "fecha", DbType.DateTime, _fecha);
                db1.AddInParameter(cmd, "fecha_fin", DbType.DateTime, _fecha_fin);
                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_multimediaproducto_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_multimediaproducto = (int)db1.GetParameterValue(cmd, "id_multimediaproducto_aux");
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