using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class usuario
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private string _tipo_operacion = "";
        private long _id_usuario = 0;
        private string _email = "";
        private string _nombre = "";
        private string _paterno = "";
        private string _materno = "";
        private string _clave = "";
        private bool _activo = true;
        private string _url_foto = "";
        private string _token_redes = "";
        private DateTime _fecha_nacimiento = DateTime.Now;
        private string _celular = "";

        private string _error = "";
        //Propiedades públicas
        public string tipo_operacion { get { return _tipo_operacion; } set { _tipo_operacion = value; } }
        public long id_usuario { get { return _id_usuario; } set { _id_usuario = value; } }
        public string email { get { return _email; } set { _email = value; } }
        public string nombre { get { return _nombre; } set { _nombre = value; } }
        public string paterno { get { return _paterno; } set { _paterno = value; } }
        public string materno { get { return _materno; } set { _materno = value; } }
        public string clave { get { return _clave; } set { _clave = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }
        public string url_foto { get { return _url_foto; } set { _url_foto = value; } }
        public string token_redes { get { return _token_redes; } set { _token_redes = value; } }
        public DateTime fecha_nacimiento { get { return _fecha_nacimiento; } set { _fecha_nacimiento = value; } }
        public string celular { get { return _celular; } set { _celular = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public usuario(long Id_usuario)
        {
            _id_usuario = Id_usuario;
            RecuperarDatos();
        }
        public usuario(string Tipo_operacion, long Id_usuario, string Email, string Nombre, string Paterno, string Materno, string Clave, bool Activo, string Url_foto, string Token_redes, DateTime Fecha_nacimiento, string Celular)
        {
            _tipo_operacion = Tipo_operacion;
            _id_usuario = Id_usuario;
            _email = Email;
            _nombre = Nombre;
            _paterno = Paterno;
            _materno = Materno;
            _clave = Clave;
            _activo = Activo;
            _url_foto = Url_foto;
            _token_redes = Token_redes;
            _fecha_nacimiento = Fecha_nacimiento;
            _celular = Celular;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_usuario_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_usuario_individual");
                db1.AddInParameter(cmd, "id_usuario", DbType.Int64, _id_usuario);
                db1.AddOutParameter(cmd, "email", DbType.String, 500);
                db1.AddOutParameter(cmd, "nombre", DbType.String, 200);
                db1.AddOutParameter(cmd, "paterno", DbType.String, 200);
                db1.AddOutParameter(cmd, "materno", DbType.String, 200);
                db1.AddOutParameter(cmd, "clave", DbType.String, 500);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "url_foto", DbType.String, 500);
                db1.AddOutParameter(cmd, "token_redes", DbType.String, 256);
                db1.AddOutParameter(cmd, "fecha_nacimiento", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "celular", DbType.String, 20);
                db1.ExecuteNonQuery(cmd);

                _email = (string)db1.GetParameterValue(cmd, "email");
                _nombre = (string)db1.GetParameterValue(cmd, "nombre");
                _paterno = (string)db1.GetParameterValue(cmd, "paterno");
                _materno = (string)db1.GetParameterValue(cmd, "materno");
                _clave = (string)db1.GetParameterValue(cmd, "clave");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
                _url_foto = (string)db1.GetParameterValue(cmd, "url_foto");
                _token_redes = (string)db1.GetParameterValue(cmd, "token_redes");
                _fecha_nacimiento = (DateTime)db1.GetParameterValue(cmd, "fecha_nacimiento");
                _celular = (string)db1.GetParameterValue(cmd, "celular");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_usuario");
                db1.AddInParameter(cmd, "tipo_operacion", DbType.String, _tipo_operacion);
                db1.AddInParameter(cmd, "id_usuario", DbType.Int64, _id_usuario);
                db1.AddInParameter(cmd, "email", DbType.String, _email);
                db1.AddInParameter(cmd, "nombre", DbType.String, _nombre);
                db1.AddInParameter(cmd, "paterno", DbType.String, _paterno);
                db1.AddInParameter(cmd, "materno", DbType.String, _materno);
                db1.AddInParameter(cmd, "clave", DbType.String, _clave);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "url_foto", DbType.String, _url_foto);
                db1.AddInParameter(cmd, "token_redes", DbType.String, _token_redes);
                db1.AddInParameter(cmd, "fecha_nacimiento", DbType.DateTime, _fecha_nacimiento);
                db1.AddInParameter(cmd, "celular", DbType.String, _celular);
                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_usuario1_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_usuario = (int)db1.GetParameterValue(cmd, "id_usuario1_aux");
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