using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class cliente_usuario
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private string _tipo_operacion = "";
        private long _id_clienteusuario = 0;
        private DateTime _fecha = DateTime.Now;
        private DateTime _fecha_fin = DateTime.Now;
        private bool _activo = true;
        private int _id_cliente = 0;
        private long _id_usuario = 0;

        private string _error = "";
        //Propiedades públicas
        public string tipo_operacion { get { return _tipo_operacion; } set { _tipo_operacion = value; } }
        public long id_clienteusuario { get { return _id_clienteusuario; } set { _id_clienteusuario = value; } }
        public DateTime fecha { get { return _fecha; } set { _fecha = value; } }
        public DateTime fecha_fin { get { return _fecha_fin; } set { _fecha_fin = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }
        public int id_cliente { get { return _id_cliente; } set { _id_cliente = value; } }
        public long id_usuario { get { return _id_usuario; } set { _id_usuario = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public cliente_usuario(long Id_clienteusuario)
        {
            _id_clienteusuario = Id_clienteusuario;
            RecuperarDatos();
        }
        public cliente_usuario(string Tipo_operacion, long Id_clienteusuario, DateTime Fecha, DateTime Fecha_fin, bool Activo, int Id_cliente, long Id_usuario)
        {
            _tipo_operacion = Tipo_operacion;
            _id_clienteusuario = Id_clienteusuario;
            _fecha = Fecha;
            _fecha_fin = Fecha_fin;
            _activo = Activo;
            _id_cliente = Id_cliente;
            _id_usuario = Id_usuario;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_cliente_usuario_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_cliente_usuario_individual");
                db1.AddInParameter(cmd, "id_clienteusuario", DbType.Int64, _id_clienteusuario);
                db1.AddOutParameter(cmd, "fecha", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "fecha_fin", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "id_cliente", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "id_usuario", DbType.Int64, 64);
                db1.ExecuteNonQuery(cmd);

                _fecha = (DateTime)db1.GetParameterValue(cmd, "fecha");
                _fecha_fin = (DateTime)db1.GetParameterValue(cmd, "fecha_fin");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
                _id_cliente = (int)db1.GetParameterValue(cmd, "id_cliente");
                _id_usuario = (long)db1.GetParameterValue(cmd, "id_usuario");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_cliente_usuario");
                db1.AddInParameter(cmd, "tipo_operacion", DbType.String, _tipo_operacion);
                db1.AddInParameter(cmd, "id_clienteusuario", DbType.Int64, _id_clienteusuario);
                db1.AddInParameter(cmd, "fecha", DbType.DateTime, _fecha);
                db1.AddInParameter(cmd, "fecha_fin", DbType.DateTime, _fecha_fin);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "id_cliente", DbType.Int32, _id_cliente);
                db1.AddInParameter(cmd, "id_usuario", DbType.Int64, _id_usuario);
                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_clienteusuario_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_clienteusuario = (int)db1.GetParameterValue(cmd, "id_clienteusuario_aux");
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