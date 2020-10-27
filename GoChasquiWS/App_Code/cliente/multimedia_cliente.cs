using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class multimedia_cliente
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private string _tipo_operacion = "";
        private long _id_multimediacliente = 0;
        private int _id_tipoarchivo = 0;
        private int _id_cliente = 0;
        private string _nombre = "";
        private string _ruta = "";
        private bool _activo = true;
        private DateTime _fecha = DateTime.Now;
        private DateTime _fecha_fin = DateTime.Now;

        private string _error = "";
        //Propiedades públicas
        public string tipo_operacion { get { return _tipo_operacion; } set { _tipo_operacion = value; } }
        public long id_multimediacliente { get { return _id_multimediacliente; } set { _id_multimediacliente = value; } }
        public int id_tipoarchivo { get { return _id_tipoarchivo; } set { _id_tipoarchivo = value; } }
        public int id_cliente { get { return _id_cliente; } set { _id_cliente = value; } }
        public string nombre { get { return _nombre; } set { _nombre = value; } }
        public string ruta { get { return _ruta; } set { _ruta = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }
        public DateTime fecha { get { return _fecha; } set { _fecha = value; } }
        public DateTime fecha_fin { get { return _fecha_fin; } set { _fecha_fin = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public multimedia_cliente(long Id_multimediacliente)
        {
            _id_multimediacliente = Id_multimediacliente;
            RecuperarDatos();
        }
        public multimedia_cliente(string Tipo_operacion, long Id_multimediacliente, int Id_tipoarchivo, int Id_cliente, string Nombre, string Ruta, bool Activo, DateTime Fecha, DateTime Fecha_fin)
        {
            _tipo_operacion = Tipo_operacion;
            _id_multimediacliente = Id_multimediacliente;
            _id_tipoarchivo = Id_tipoarchivo;
            _id_cliente = Id_cliente;
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
            DbCommand cmd = db1.GetStoredProcCommand("lista_multimedia_cliente_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_multimedia_cliente_individual");
                db1.AddInParameter(cmd, "id_multimediacliente", DbType.Int64, _id_multimediacliente);
                db1.AddOutParameter(cmd, "id_tipoarchivo", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "id_cliente", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "nombre", DbType.String, 200);
                db1.AddOutParameter(cmd, "ruta", DbType.String, 500);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "fecha", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "fecha_fin", DbType.DateTime, 30);
                db1.ExecuteNonQuery(cmd);

                _id_tipoarchivo = (int)db1.GetParameterValue(cmd, "id_tipoarchivo");
                _id_cliente = (int)db1.GetParameterValue(cmd, "id_cliente");
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
                DbCommand cmd = db1.GetStoredProcCommand("abm_multimedia_cliente");
                db1.AddInParameter(cmd, "tipo_operacion", DbType.String, _tipo_operacion);
                db1.AddInParameter(cmd, "id_multimediacliente", DbType.Int64, _id_multimediacliente);
                db1.AddInParameter(cmd, "id_tipoarchivo", DbType.Int32, _id_tipoarchivo);
                db1.AddInParameter(cmd, "id_cliente", DbType.Int32, _id_cliente);
                db1.AddInParameter(cmd, "nombre", DbType.String, _nombre);
                db1.AddInParameter(cmd, "ruta", DbType.String, _ruta);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "fecha", DbType.DateTime, _fecha);
                db1.AddInParameter(cmd, "fecha_fin", DbType.DateTime, _fecha_fin);
                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_multimediacliente_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_multimediacliente = (int)db1.GetParameterValue(cmd, "id_multimediacliente_aux");
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