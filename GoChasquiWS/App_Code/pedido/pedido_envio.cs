using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class pedido_envio
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private string _tipo_operacion = "";
        private long _id_pedidoenvio = 0;
        private long _id_pedido = 0;
        private string _nombre_destinatario = "";
        private string _celular_destinatario = "";
        private string _contenido = "";
        private Int16 _alto = 0;
        private Int16 _ancho = 0;
        private string _recomendaciones = "";

        private string _error = "";
        //Propiedades públicas
        public string tipo_operacion { get { return _tipo_operacion; } set { _tipo_operacion = value; } }
        public long id_pedidoenvio { get { return _id_pedidoenvio; } set { _id_pedidoenvio = value; } }
        public long id_pedido { get { return _id_pedido; } set { _id_pedido = value; } }
        public string nombre_destinatario { get { return _nombre_destinatario; } set { _nombre_destinatario = value; } }
        public string celular_destinatario { get { return _celular_destinatario; } set { _celular_destinatario = value; } }
        public string contenido { get { return _contenido; } set { _contenido = value; } }
        public Int16 alto { get { return _alto; } set { _alto = value; } }
        public Int16 ancho { get { return _ancho; } set { _ancho = value; } }
        public string recomendaciones { get { return _recomendaciones; } set { _recomendaciones = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public pedido_envio(int id_pedidoenvio)
        {
            _id_pedidoenvio = id_pedidoenvio;
            RecuperarDatos();
        }
        public pedido_envio(string Tipo_operacion, long Id_pedidoenvio, long Id_pedido, string Nombre_destinatario, string Celular_destinatario, string Contenido, Int16 Alto, Int16 Ancho, string Recomendaciones)
        {
            _tipo_operacion = Tipo_operacion;
            _id_pedidoenvio = Id_pedidoenvio;
            _id_pedido = Id_pedido;
            _nombre_destinatario = Nombre_destinatario;
            _celular_destinatario = Celular_destinatario;
            _contenido = Contenido;
            _alto = Alto;
            _ancho = Ancho;
            _recomendaciones = Recomendaciones;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_pedido_envio_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_pedido_envio_individual");
                db1.AddInParameter(cmd, "id_pedidoenvio", DbType.Int64, _id_pedidoenvio);
                db1.AddOutParameter(cmd, "id_pedido", DbType.Int64, 64);
                db1.AddOutParameter(cmd, "nombre_destinatario", DbType.String, 500);
                db1.AddOutParameter(cmd, "celular_destinatario", DbType.String, 20);
                db1.AddOutParameter(cmd, "contenido", DbType.String, 500);
                db1.AddOutParameter(cmd, "alto", DbType.Int16, 16);
                db1.AddOutParameter(cmd, "ancho", DbType.Int16, 16);
                db1.AddOutParameter(cmd, "recomendaciones", DbType.String, 500);
                db1.ExecuteNonQuery(cmd);

                _id_pedido = (long)db1.GetParameterValue(cmd, "id_pedido");
                _nombre_destinatario = (string)db1.GetParameterValue(cmd, "nombre_destinatario");
                _celular_destinatario = (string)db1.GetParameterValue(cmd, "celular_destinatario");
                _contenido = (string)db1.GetParameterValue(cmd, "contenido");
                _alto = (Int16)db1.GetParameterValue(cmd, "alto");
                _ancho = (Int16)db1.GetParameterValue(cmd, "ancho");
                _recomendaciones = (string)db1.GetParameterValue(cmd, "recomendaciones");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_pedido_envio");
                db1.AddInParameter(cmd, "tipo_operacion", DbType.String, _tipo_operacion);
                db1.AddInParameter(cmd, "id_pedidoenvio", DbType.Int64, _id_pedidoenvio);
                db1.AddInParameter(cmd, "id_pedido", DbType.Int64, _id_pedido);
                db1.AddInParameter(cmd, "nombre_destinatario", DbType.String, _nombre_destinatario);
                db1.AddInParameter(cmd, "celular_destinatario", DbType.String, _celular_destinatario);
                db1.AddInParameter(cmd, "contenido", DbType.String, _contenido);
                db1.AddInParameter(cmd, "alto", DbType.Int16, _alto);
                db1.AddInParameter(cmd, "ancho", DbType.Int16, _ancho);
                db1.AddInParameter(cmd, "recomendaciones", DbType.String, _recomendaciones);
                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_pedidoenvio_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_pedidoenvio = (int)db1.GetParameterValue(cmd, "id_pedidoenvio_aux");
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