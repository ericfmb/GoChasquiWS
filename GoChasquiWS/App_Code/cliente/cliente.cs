using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class cliente
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private string _tipo_operacion = "";
        private int _id_cliente = 0;
        private string _razon_social = "";
        private string _nit = "";
        private string _paterno = "";
        private string _materno = "";
        private string _nombre = "";
        private bool _activo = true;
        private int _id_tipocliente = 0;
        private int _id_tiponegocio = 0;
        private DateTime _fecha_ini = DateTime.Now;
        private bool _abierto = true;

        private string _error = "";
        //Propiedades públicas
        public int id_cliente { get { return _id_cliente; } set { _id_cliente = value; } }
        public string razon_social { get { return _razon_social; } set { _razon_social = value; } }
        public string nit { get { return _nit; } set { _nit = value; } }
        public string paterno { get { return _paterno; } set { _paterno = value; } }
        public string materno { get { return _materno; } set { _materno = value; } }
        public string nombre { get { return _nombre; } set { _nombre = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }
        public int id_tipocliente { get { return _id_tipocliente; } set { _id_tipocliente = value; } }
        public int id_tiponegocio { get { return _id_tiponegocio; } set { _id_tiponegocio = value; } }
        public DateTime fecha_ini { get { return _fecha_ini; } set { _fecha_ini = value; } }
        public bool abierto { get { return _abierto; } set { _abierto = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public cliente(int Id_cliente)
        {
            _id_cliente = Id_cliente;
            RecuperarDatos();
        }
        public cliente(string Tipo_operacion, int Id_cliente, string Razon_social, string Nit, string Paterno, string Materno, string Nombre, bool Activo, int Id_tipocliente, int Id_tiponegocio, DateTime Fecha_ini, bool Abierto)
        {
            _tipo_operacion = Tipo_operacion;
            _id_cliente = Id_cliente;
            _razon_social = Razon_social;
            _nit = Nit;
            _paterno = Paterno;
            _materno = Materno;
            _nombre = Nombre;
            _activo = Activo;
            _id_tipocliente = Id_tipocliente;
            _id_tiponegocio = Id_tiponegocio;
            _fecha_ini = Fecha_ini;
            _abierto = Abierto;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_cliente_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_cliente_individual");
                db1.AddInParameter(cmd, "id_cliente", DbType.Int32, _id_cliente);
                db1.AddOutParameter(cmd, "razon_social", DbType.String, 500);
                db1.AddOutParameter(cmd, "nit", DbType.String, 100);
                db1.AddOutParameter(cmd, "paterno", DbType.String, 200);
                db1.AddOutParameter(cmd, "materno", DbType.String, 200);
                db1.AddOutParameter(cmd, "nombre", DbType.String, 200);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "id_tipocliente", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "id_tiponegocio", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "fecha_ini", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "abierto", DbType.Boolean, 1);
                db1.ExecuteNonQuery(cmd);

                _razon_social = (string)db1.GetParameterValue(cmd, "razon_social");
                _nit = (string)db1.GetParameterValue(cmd, "nit");
                _paterno = (string)db1.GetParameterValue(cmd, "paterno");
                _materno = (string)db1.GetParameterValue(cmd, "materno");
                _nombre = (string)db1.GetParameterValue(cmd, "nombre");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
                _id_tipocliente = (int)db1.GetParameterValue(cmd, "id_tipocliente");
                _id_tiponegocio = (int)db1.GetParameterValue(cmd, "id_tiponegocio");
                _fecha_ini = (DateTime)db1.GetParameterValue(cmd, "fecha_ini");
                _abierto = (Boolean)db1.GetParameterValue(cmd, "abierto");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_cliente");
                db1.AddInParameter(cmd, "tipo_operacion", DbType.String, _tipo_operacion);
                db1.AddInParameter(cmd, "id_cliente", DbType.Int32, _id_cliente);
                db1.AddInParameter(cmd, "razon_social", DbType.String, _razon_social);
                db1.AddInParameter(cmd, "nit", DbType.String, _nit);
                db1.AddInParameter(cmd, "paterno", DbType.String, _paterno);
                db1.AddInParameter(cmd, "materno", DbType.String, _materno);
                db1.AddInParameter(cmd, "nombre", DbType.String, _nombre);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "id_tipocliente", DbType.Int32, _id_tipocliente);
                db1.AddInParameter(cmd, "id_tiponegocio", DbType.Int32, _id_tiponegocio);
                db1.AddInParameter(cmd, "fecha_ini", DbType.DateTime, _fecha_ini);
                db1.AddInParameter(cmd, "abierto", DbType.Boolean, _abierto);
                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_cliente_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_cliente = (int)db1.GetParameterValue(cmd, "id_cliente_aux");
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