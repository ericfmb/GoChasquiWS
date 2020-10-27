using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class direccion
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private string _tipo_operacion = "";
        private int _id_direccion = 0;
        private int _id_cliente = 0;
        private string _latitud = "";
        private string _longitud = "";
        private string _direccion = "";
        private DateTime _fecha = DateTime.Now;
        private bool _activo = true;

        private string _error = "";
        //Propiedades públicas
        public int id_direccion { get { return _id_direccion; } set { _id_direccion = value; } }
        public int id_cliente { get { return _id_cliente; } set { _id_cliente = value; } }
        public string latitud { get { return _latitud; } set { _latitud = value; } }
        public string longitud { get { return _longitud; } set { _longitud = value; } }
        public string direccion1 { get { return _direccion; } set { _direccion = value; } }
        public DateTime fecha { get { return _fecha; } set { _fecha = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public direccion(int Id_direccion)
        {
            _id_direccion = Id_direccion;
            RecuperarDatos();
        }
        public direccion(string Tipo_operacion, int Id_direccion, int Id_cliente, string Latitud, string Longitud, string Direccion, DateTime Fecha, bool Activo)
        {
            _tipo_operacion = Tipo_operacion;
            _id_direccion = Id_direccion;
            _id_cliente = Id_cliente;
            _latitud = Latitud;
            _longitud = Longitud;
            _direccion = Direccion;
            _fecha = Fecha;
            _activo = Activo;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_direccion_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_direccion_individual");
                db1.AddInParameter(cmd, "id_direccion", DbType.Int32, _id_direccion);
                db1.AddOutParameter(cmd, "id_cliente", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "latitud", DbType.String, 20);
                db1.AddOutParameter(cmd, "longitud", DbType.String, 20);
                db1.AddOutParameter(cmd, "direccion", DbType.String, 500);
                db1.AddOutParameter(cmd, "fecha", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.ExecuteNonQuery(cmd);

                _id_cliente = (int)db1.GetParameterValue(cmd, "id_cliente");
                _latitud = (string)db1.GetParameterValue(cmd, "latitud");
                _longitud = (string)db1.GetParameterValue(cmd, "longitud");
                _direccion = (string)db1.GetParameterValue(cmd, "direccion");
                _fecha = (DateTime)db1.GetParameterValue(cmd, "fecha");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_direccion");
                db1.AddInParameter(cmd, "tipo_operacion", DbType.String, _tipo_operacion);
                db1.AddInParameter(cmd, "id_direccion", DbType.Int32, _id_direccion);
                db1.AddInParameter(cmd, "id_cliente", DbType.Int32, _id_cliente);
                db1.AddInParameter(cmd, "latitud", DbType.String, _latitud);
                db1.AddInParameter(cmd, "longitud", DbType.String, _longitud);
                db1.AddInParameter(cmd, "direccion", DbType.String, _direccion);
                db1.AddInParameter(cmd, "fecha", DbType.DateTime, _fecha);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_direccion_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_direccion = (int)db1.GetParameterValue(cmd, "id_direccion_aux");
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