using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class horario
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private string _tipo_operacion = "";
        private long _id_horario = 0;
        private int _id_cliente = 0;
        private Byte _dia = 0;
        private DateTime _hora1 = DateTime.Now;
        private DateTime _hora2 = DateTime.Now;
        private bool _activo = true;
        private DateTime _fecha = DateTime.Now;


        private string _error = "";
        //Propiedades públicas
        public string tipo_operacion { get { return _tipo_operacion; } set { _tipo_operacion = value; } }
        public long id_horario { get { return _id_horario; } set { _id_horario = value; } }
        public int id_cliente { get { return _id_cliente; } set { _id_cliente = value; } }
        public Byte dia { get { return _dia; } set { _dia = value; } }
        public DateTime hora1 { get { return _hora1; } set { _hora1 = value; } }
        public DateTime hora2 { get { return _hora2; } set { _hora2 = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }
        public DateTime fecha { get { return _fecha; } set { _fecha = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public horario(long Id_horario)
        {
            _id_horario = Id_horario;
            RecuperarDatos();
        }
        public horario(string Tipo_operacion, long Id_horario, int Id_cliente, Byte Dia, DateTime Hora1, DateTime Hora2, bool Activo, DateTime Fecha)
        {
            _tipo_operacion = Tipo_operacion;
            _id_horario = Id_horario;
            _id_cliente = Id_cliente;
            _dia = Dia;
            _hora1 = Hora1;
            _hora2 = Hora2;
            _activo = Activo;
            _fecha = Fecha;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_horario_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_horario_individual");
                db1.AddInParameter(cmd, "id_horario", DbType.Int64, _id_horario);
                db1.AddOutParameter(cmd, "id_cliente", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "dia", DbType.Byte, 8);
                db1.AddOutParameter(cmd, "hora1", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "hora2", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "fecha", DbType.DateTime, 30);
                db1.ExecuteNonQuery(cmd);

                _id_cliente = (int)db1.GetParameterValue(cmd, "id_cliente");
                _dia = (Byte)db1.GetParameterValue(cmd, "dia");
                _hora1 = (DateTime)db1.GetParameterValue(cmd, "hora1");
                _hora2 = (DateTime)db1.GetParameterValue(cmd, "hora2");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
                _fecha = (DateTime)db1.GetParameterValue(cmd, "fecha");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_horario");
                db1.AddInParameter(cmd, "tipo_operacion", DbType.String, _tipo_operacion);
                db1.AddInParameter(cmd, "id_horario", DbType.Int64, _id_horario);
                db1.AddInParameter(cmd, "id_cliente", DbType.Int32, _id_cliente);
                db1.AddInParameter(cmd, "dia", DbType.Byte, _dia);
                db1.AddInParameter(cmd, "hora1", DbType.DateTime, _hora1);
                db1.AddInParameter(cmd, "hora2", DbType.DateTime, _hora2);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "fecha", DbType.DateTime, _fecha);
                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_horario_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_horario = (int)db1.GetParameterValue(cmd, "id_horario_aux");
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