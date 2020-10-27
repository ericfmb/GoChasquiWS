using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class tipo_pago
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private string _tipo_operacion = "";
        private int _id_tipopago = 0;
        private string _nombre = "";
        private bool _activo = true;


        private string _error = "";
        //Propiedades públicas
        public string tipo_operacion { get { return _tipo_operacion; } set { _tipo_operacion = value; } }
        public int id_tipopago { get { return _id_tipopago; } set { _id_tipopago = value; } }
        public string nombre { get { return _nombre; } set { _nombre = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public tipo_pago(int Id_tipopago)
        {
            _id_tipopago = Id_tipopago;
            RecuperarDatos();
        }
        public tipo_pago(string Tipo_operacion, int Id_tipopago, string Nombre, bool Activo)
        {
            _tipo_operacion = Tipo_operacion;
            _id_tipopago = Id_tipopago;
            _nombre = Nombre;
            _activo = Activo;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_tipo_pago_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_tipo_pago_individual");
                db1.AddInParameter(cmd, "id_tipopago", DbType.Int32, _id_tipopago);
                db1.AddOutParameter(cmd, "nombre", DbType.String, 100);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.ExecuteNonQuery(cmd);

                _nombre = (string)db1.GetParameterValue(cmd, "nombre");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_tipo_pago");
                db1.AddInParameter(cmd, "tipo_operacion", DbType.String, _tipo_operacion);
                db1.AddInParameter(cmd, "id_tipopago", DbType.Int32, _id_tipopago);
                db1.AddInParameter(cmd, "nombre", DbType.String, _nombre);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_tipopago_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_tipopago = (int)db1.GetParameterValue(cmd, "id_tipopago_aux");
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