using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class ruta_detalle
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private string _tipo_operacion = "";
        private long _id_rutadetalle = 0;
        private string _latitud = "";
        private string _longitud = "";
        private DateTime _fecha = DateTime.Now;
        private long _id_ruta = 0;

        private string _error = "";
        //Propiedades públicas
        public string tipo_operacion { get { return _tipo_operacion; } set { _tipo_operacion = value; } }
        public long id_rutadetalle { get { return _id_rutadetalle; } set { _id_rutadetalle = value; } }
        public string latitud { get { return _latitud; } set { _latitud = value; } }
        public string longitud { get { return _longitud; } set { _longitud = value; } }
        public DateTime fecha { get { return _fecha; } set { _fecha = value; } }
        public long id_ruta { get { return _id_ruta; } set { _id_ruta = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public ruta_detalle(int id_rutadetalle)
        {
            _id_rutadetalle = id_rutadetalle;
            RecuperarDatos();
        }
        public ruta_detalle(string Tipo_operacion, long Id_rutadetalle, string Latitud, string Longitud, DateTime Fecha, long Id_ruta)
        {
            _tipo_operacion = Tipo_operacion;
            _id_rutadetalle = Id_rutadetalle;
            _latitud = Latitud;
            _longitud = Longitud;
            _fecha = Fecha;
            _id_ruta = Id_ruta;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_ruta_detalle_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_ruta_detalle_individual");
                db1.AddInParameter(cmd, "id_rutadetalle", DbType.Int64, _id_rutadetalle);
                db1.AddOutParameter(cmd, "latitud", DbType.String, 20);
                db1.AddOutParameter(cmd, "longitud", DbType.String, 20);
                db1.AddOutParameter(cmd, "fecha", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "id_ruta", DbType.Int64, 64);
                db1.ExecuteNonQuery(cmd);

                _latitud = (string)db1.GetParameterValue(cmd, "latitud");
                _longitud = (string)db1.GetParameterValue(cmd, "longitud");
                _fecha = (DateTime)db1.GetParameterValue(cmd, "fecha");
                _id_ruta = (long)db1.GetParameterValue(cmd, "id_ruta");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_ruta_detalle");
                db1.AddInParameter(cmd, "tipo_operacion", DbType.String, _tipo_operacion);
                db1.AddInParameter(cmd, "id_rutadetalle", DbType.Int64, _id_rutadetalle);
                db1.AddInParameter(cmd, "latitud", DbType.String, _latitud);
                db1.AddInParameter(cmd, "longitud", DbType.String, _longitud);
                db1.AddInParameter(cmd, "fecha", DbType.DateTime, _fecha);
                db1.AddInParameter(cmd, "id_ruta", DbType.Int64, _id_ruta);
                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_rutadetalle_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_rutadetalle = (int)db1.GetParameterValue(cmd, "id_rutadetalle_aux");
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