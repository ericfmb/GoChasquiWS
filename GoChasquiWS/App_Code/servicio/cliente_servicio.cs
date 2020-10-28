using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class cliente_servicio
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private int _id_clienteservicio = 0;
        private int _id_cliente = 0;
        private int _id_servicio = 0;
        private DateTime _fecha = DateTime.Now;
        private bool _activo = true;

        private string _error = "";
        //Propiedades públicas
        public int id_clienteservicio { get { return _id_clienteservicio; } set { _id_clienteservicio = value; } }
        public int id_cliente { get { return _id_cliente; } set { _id_cliente = value; } }
        public int id_servicio { get { return _id_servicio; } set { _id_servicio = value; } }
        public DateTime fecha { get { return _fecha; } set { _fecha = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public cliente_servicio(int Id_clienteservicio)
        {
            _id_clienteservicio = Id_clienteservicio;
            RecuperarDatos();
        }
        public cliente_servicio(int Id_clienteservicio, int Id_cliente, int Id_servicio, DateTime Fecha, bool Activo)
        {
            _id_clienteservicio = Id_clienteservicio;
            _id_cliente = Id_cliente;
            _id_servicio = Id_servicio;
            _fecha = Fecha;
            _activo = Activo;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_cliente_servicio_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_cliente_servicio_individual");
                db1.AddInParameter(cmd, "id_clienteservicio", DbType.Int32, _id_clienteservicio);
                db1.AddOutParameter(cmd, "id_cliente", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "id_servicio", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "fecha", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.ExecuteNonQuery(cmd);

                _id_cliente = (int)db1.GetParameterValue(cmd, "id_cliente");
                _id_servicio = (int)db1.GetParameterValue(cmd, "id_servicio");
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
                DbCommand cmd = db1.GetStoredProcCommand("abm_cliente_servicio");
                db1.AddInParameter(cmd, "id_clienteservicio", DbType.Int32, _id_clienteservicio);
                db1.AddInParameter(cmd, "id_cliente", DbType.Int32, _id_cliente);
                db1.AddInParameter(cmd, "id_servicio", DbType.Int32, _id_servicio);
                db1.AddInParameter(cmd, "fecha", DbType.DateTime, _fecha);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);

                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_clienteservicio_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_clienteservicio = (int)db1.GetParameterValue(cmd, "id_clienteservicio_aux");
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