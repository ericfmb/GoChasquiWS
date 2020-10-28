using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class servicio
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private int _id_servicio = 0;
        private string _nombre = "";
        private bool _activo = true;
        private DateTime _fecha = DateTime.Now;
        private decimal _monto_mes = 0;
        private decimal _porcentaje = 0;
        private decimal _monto_anual = 0;
        private decimal _monto_semestral = 0;

        private string _error = "";
        //Propiedades públicas
        public int id_servicio { get { return _id_servicio; } set { _id_servicio = value; } }
        public string nombre { get { return _nombre; } set { _nombre = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }
        public DateTime fecha { get { return _fecha; } set { _fecha = value; } }
        public decimal monto_mes { get { return _monto_mes; } set { _monto_mes = value; } }
        public decimal porcentaje { get { return _porcentaje; } set { _porcentaje = value; } }
        public decimal monto_anual { get { return _monto_anual; } set { _monto_anual = value; } }
        public decimal monto_semestral { get { return _monto_semestral; } set { _monto_semestral = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public servicio(int Id_servicio)
        {
            _id_servicio = Id_servicio;
            RecuperarDatos();
        }
        public servicio(int Id_servicio, string Nombre, bool Activo, DateTime Fecha, decimal Monto_mes, decimal Porcentaje, decimal Monto_anual, decimal Monto_semestral)
        {
            _id_servicio = Id_servicio;
            _nombre = Nombre;
            _activo = Activo;
            _fecha = Fecha;
            _monto_mes = Monto_mes;
            _porcentaje = Porcentaje;
            _monto_anual = Monto_anual;
            _monto_semestral = Monto_semestral;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_servicio_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_servicio_individual");
                db1.AddInParameter(cmd, "id_servicio", DbType.Int32, _id_servicio);
                db1.AddOutParameter(cmd, "nombre", DbType.String, 500);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "fecha", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "monto_mes", DbType.Decimal, 32);
                db1.AddOutParameter(cmd, "porcentaje", DbType.Decimal, 32);
                db1.AddOutParameter(cmd, "monto_anual", DbType.Decimal, 32);
                db1.AddOutParameter(cmd, "monto_semestral", DbType.Decimal, 32);
                db1.ExecuteNonQuery(cmd);

                _nombre = (string)db1.GetParameterValue(cmd, "nombre");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
                _fecha = (DateTime)db1.GetParameterValue(cmd, "fecha");
                _monto_mes = (decimal)db1.GetParameterValue(cmd, "monto_mes");
                _porcentaje = (decimal)db1.GetParameterValue(cmd, "porcentaje");
                _monto_anual = (decimal)db1.GetParameterValue(cmd, "monto_anual");
                _monto_semestral = (decimal)db1.GetParameterValue(cmd, "monto_semestral");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_servicio");
                db1.AddInParameter(cmd, "id_servicio", DbType.Int32, _id_servicio);
                db1.AddInParameter(cmd, "nombre", DbType.String, _nombre);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "fecha", DbType.DateTime, _fecha);
                db1.AddInParameter(cmd, "monto_mes", DbType.Decimal, _monto_mes);
                db1.AddInParameter(cmd, "porcentaje", DbType.Decimal, _porcentaje);
                db1.AddInParameter(cmd, "monto_anual", DbType.Decimal, _monto_anual);
                db1.AddInParameter(cmd, "monto_semestral", DbType.Decimal, _monto_semestral);

                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_servicio_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_servicio = (int)db1.GetParameterValue(cmd, "id_servicio_aux");
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