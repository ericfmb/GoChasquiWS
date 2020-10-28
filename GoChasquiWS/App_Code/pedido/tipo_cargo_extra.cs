using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class tipo_cargo_extra
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private string _tipo_operacion = "";
        private int _id_tipocargoextra = 0;
        private string _nombre = "";
        private bool _activo = true;
        private decimal _porcentaje = 0;
        private decimal _monto_fijo = 0;

        private string _error = "";
        //Propiedades públicas
        public string tipo_operacion { get { return _tipo_operacion; } set { _tipo_operacion = value; } }
        public int id_tipocargoextra { get { return _id_tipocargoextra; } set { _id_tipocargoextra = value; } }
        public string nombre { get { return _nombre; } set { _nombre = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }
        public decimal porcentaje { get { return _porcentaje; } set { _porcentaje = value; } }
        public decimal monto_fijo { get { return _monto_fijo; } set { _monto_fijo = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public tipo_cargo_extra(int Id_tipocargoextra)
        {
            _id_tipocargoextra = Id_tipocargoextra;
            RecuperarDatos();
        }
        public tipo_cargo_extra(string Tipo_operacion, int Id_tipocargoextra, string Nombre, bool Activo, decimal Porcentaje, decimal Monto_fijo)
        {
            _tipo_operacion = Tipo_operacion;
            _id_tipocargoextra = Id_tipocargoextra;
            _nombre = Nombre;
            _activo = Activo;
            _porcentaje = Porcentaje;
            _monto_fijo = Monto_fijo;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_tipo_cargo_extra_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_tipo_cargo_extra_individual");
                DbParameter Parametro;

                db1.AddInParameter(cmd, "id_tipocargoextra", DbType.Int32, _id_tipocargoextra);
                db1.AddOutParameter(cmd, "nombre", DbType.String, 200);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                Parametro = cmd.CreateParameter(); Parametro.ParameterName = "@porcentaje"; Parametro.DbType = DbType.Decimal; Parametro.Direction = ParameterDirection.Output; Parametro.Precision = 18; Parametro.Scale = 2; cmd.Parameters.Add(Parametro);
                Parametro = cmd.CreateParameter(); Parametro.ParameterName = "@monto_fijo"; Parametro.DbType = DbType.Decimal; Parametro.Direction = ParameterDirection.Output; Parametro.Precision = 18; Parametro.Scale = 2; cmd.Parameters.Add(Parametro);
                db1.ExecuteNonQuery(cmd);

                _nombre = (string)db1.GetParameterValue(cmd, "nombre");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
                _porcentaje = (decimal)db1.GetParameterValue(cmd, "porcentaje");
                _monto_fijo = (decimal)db1.GetParameterValue(cmd, "monto_fijo");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_tipo_cargo_extra");
                db1.AddInParameter(cmd, "tipo_operacion", DbType.String, _tipo_operacion);
                db1.AddInParameter(cmd, "id_tipocargoextra", DbType.Int32, _id_tipocargoextra);
                db1.AddInParameter(cmd, "nombre", DbType.String, _nombre);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "porcentaje", DbType.Decimal, _porcentaje);
                db1.AddInParameter(cmd, "monto_fijo", DbType.Decimal, _monto_fijo);
                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_tipocargoextra_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_tipocargoextra = (int)db1.GetParameterValue(cmd, "id_tipocargoextra_aux");
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