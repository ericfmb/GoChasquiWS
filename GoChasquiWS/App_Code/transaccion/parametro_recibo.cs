using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class parametro_recibo
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private int _id_parametrorecibo = 0;
        private int _num_sig = 0;
        private int _id_cliente = 0;

        private string _error = "";
        //Propiedades públicas
        public int id_parametrorecibo { get { return _id_parametrorecibo; } set { _id_parametrorecibo = value; } }
        public int num_sig { get { return _num_sig; } set { _num_sig = value; } }
        public int id_cliente { get { return _id_cliente; } set { _id_cliente = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public parametro_recibo(int Id_parametrorecibo)
        {
            _id_parametrorecibo = Id_parametrorecibo;
            RecuperarDatos();
        }
        public parametro_recibo(int Id_parametrorecibo, int Num_sig, int Id_cliente)
        {
            _id_parametrorecibo = Id_parametrorecibo;
            _num_sig = Num_sig;
            _id_cliente = Id_cliente;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_parametro_recibo_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_parametro_recibo_individual");
                db1.AddInParameter(cmd, "id_parametrorecibo", DbType.Int32, _id_parametrorecibo);
                db1.AddOutParameter(cmd, "num_sig", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "id_cliente", DbType.Int32, 32);
                db1.ExecuteNonQuery(cmd);

                _num_sig = (int)db1.GetParameterValue(cmd, "num_sig");
                _id_cliente = (int)db1.GetParameterValue(cmd, "id_cliente");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_parametro_recibo");
                db1.AddInParameter(cmd, "id_parametrorecibo", DbType.Int32, _id_parametrorecibo);
                db1.AddInParameter(cmd, "num_sig", DbType.Int32, _num_sig);
                db1.AddInParameter(cmd, "id_cliente", DbType.Int32, _id_cliente);

                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_parametrorecibo_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_parametrorecibo = (int)db1.GetParameterValue(cmd, "id_parametrorecibo_aux");
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