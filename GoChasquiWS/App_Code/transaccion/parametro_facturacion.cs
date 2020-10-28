using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class parametro_facturacion
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private int _id_parametrofacturacion = 0;
        private int _id_cliente = 0;
        private string _nro_autorizacion = "";
        private string _llave_dosificacion = "";
        private DateTime _fecha_limite = DateTime.Now;
        private string _eticket = "";
        private bool _ciclo_cerrado = true;
        private bool _activo = true;
        private int _num_sig = 0;
        private DateTime _fecha_activacion = DateTime.Now;

        private string _error = "";
        //Propiedades públicas
        public int id_parametrofacturacion { get { return _id_parametrofacturacion; } set { _id_parametrofacturacion = value; } }
        public int id_cliente { get { return _id_cliente; } set { _id_cliente = value; } }
        public string nro_autorizacion { get { return _nro_autorizacion; } set { _nro_autorizacion = value; } }
        public string llave_dosificacion { get { return _llave_dosificacion; } set { _llave_dosificacion = value; } }
        public DateTime fecha_limite { get { return _fecha_limite; } set { _fecha_limite = value; } }
        public string eticket { get { return _eticket; } set { _eticket = value; } }
        public bool ciclo_cerrado { get { return _ciclo_cerrado; } set { _ciclo_cerrado = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }
        public int num_sig { get { return _num_sig; } set { _num_sig = value; } }
        public DateTime fecha_activacion { get { return _fecha_activacion; } set { _fecha_activacion = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public parametro_facturacion(int Id_parametrofacturacion)
        {
            _id_parametrofacturacion = Id_parametrofacturacion;
            RecuperarDatos();
        }
        public parametro_facturacion(int Id_parametrofacturacion, int Id_cliente, string Nro_autorizacion, string Llave_dosificacion, DateTime Fecha_limite, string Eticket, bool Ciclo_cerrado, bool Activo, int Num_sig, DateTime Fecha_activacion)
        {
            _id_parametrofacturacion = Id_parametrofacturacion;
            _id_cliente = Id_cliente;
            _nro_autorizacion = Nro_autorizacion;
            _llave_dosificacion = Llave_dosificacion;
            _fecha_limite = Fecha_limite;
            _eticket = Eticket;
            _ciclo_cerrado = Ciclo_cerrado;
            _activo = Activo;
            _num_sig = Num_sig;
            _fecha_activacion = Fecha_activacion;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_parametro_facturacion_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_parametro_facturacion_individual");
                db1.AddInParameter(cmd, "id_parametrofacturacion", DbType.Int32, _id_parametrofacturacion);
                db1.AddOutParameter(cmd, "id_cliente", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "nro_autorizacion", DbType.String, 200);
                db1.AddOutParameter(cmd, "llave_dosificacion", DbType.String, 256);
                db1.AddOutParameter(cmd, "fecha_limite", DbType.DateTime, 30);
                db1.AddOutParameter(cmd, "eticket", DbType.String, 256);
                db1.AddOutParameter(cmd, "ciclo_cerrado", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "num_sig", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "fecha_activacion", DbType.DateTime, 30);
                db1.ExecuteNonQuery(cmd);

                _id_parametrofacturacion = (int)db1.GetParameterValue(cmd, "id_parametrofacturacion");
                _id_cliente = (int)db1.GetParameterValue(cmd, "id_cliente");
                _nro_autorizacion = (string)db1.GetParameterValue(cmd, "nro_autorizacion");
                _llave_dosificacion = (string)db1.GetParameterValue(cmd, "llave_dosificacion");
                _fecha_limite = (DateTime)db1.GetParameterValue(cmd, "fecha_limite");
                _eticket = (string)db1.GetParameterValue(cmd, "eticket");
                _ciclo_cerrado = (Boolean)db1.GetParameterValue(cmd, "ciclo_cerrado");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
                _num_sig = (int)db1.GetParameterValue(cmd, "num_sig");
                _fecha_activacion = (DateTime)db1.GetParameterValue(cmd, "fecha_activacion");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_parametro_facturacion");
                db1.AddInParameter(cmd, "id_parametrofacturacion", DbType.Int32, _id_parametrofacturacion);
                db1.AddInParameter(cmd, "id_cliente", DbType.Int32, _id_cliente);
                db1.AddInParameter(cmd, "nro_autorizacion", DbType.String, _nro_autorizacion);
                db1.AddInParameter(cmd, "llave_dosificacion", DbType.String, _llave_dosificacion);
                db1.AddInParameter(cmd, "fecha_limite", DbType.DateTime, _fecha_limite);
                db1.AddInParameter(cmd, "eticket", DbType.String, _eticket);
                db1.AddInParameter(cmd, "ciclo_cerrado", DbType.Boolean, _ciclo_cerrado);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "num_sig", DbType.Int32, _num_sig);
                db1.AddInParameter(cmd, "fecha_activacion", DbType.DateTime, _fecha_activacion);

                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_parametrofacturacion_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_parametrofacturacion = (int)db1.GetParameterValue(cmd, "id_parametrofacturacion_aux");
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