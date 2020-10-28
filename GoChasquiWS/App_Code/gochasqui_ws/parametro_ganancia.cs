using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class parametro_ganancia
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private string _tipo_operacion = "";
        private int _id_parametroganancia = 0;
        private decimal _monto_fijo_mes = 0;
        private decimal _monto_fijo_anio = 0;
        private decimal _porcentaje = 0;
        private decimal _desde_km = 0;
        private decimal _hasta_km = 0;
        private decimal _monto_rango_km = 0;
        private decimal _monto_distancia_km = 0;
        private decimal _monto_frecuencia = 0;
        private bool _activo = true;
        private DateTime _fecha = DateTime.Now;

        private string _error = "";
        //Propiedades públicas
        public string tipo_operacion { get { return _tipo_operacion; } set { _tipo_operacion = value; } }
        public int id_parametroganancia { get { return _id_parametroganancia; } set { _id_parametroganancia = value; } }
        public decimal monto_fijo_mes { get { return _monto_fijo_mes; } set { _monto_fijo_mes = value; } }
        public decimal monto_fijo_anio { get { return _monto_fijo_anio; } set { _monto_fijo_anio = value; } }
        public decimal porcentaje { get { return _porcentaje; } set { _porcentaje = value; } }
        public decimal desde_km { get { return _desde_km; } set { _desde_km = value; } }
        public decimal hasta_km { get { return _hasta_km; } set { _hasta_km = value; } }
        public decimal monto_rango_km { get { return _monto_rango_km; } set { _monto_rango_km = value; } }
        public decimal monto_distancia_km { get { return _monto_distancia_km; } set { _monto_distancia_km = value; } }
        public decimal monto_frecuencia { get { return _monto_frecuencia; } set { _monto_frecuencia = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }
        public DateTime fecha { get { return _fecha; } set { _fecha = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public parametro_ganancia(int Id_parametroganancia)
        {
            _id_parametroganancia = Id_parametroganancia;
            RecuperarDatos();
        }
        public parametro_ganancia(string Tipo_operacion, int Id_parametroganancia, decimal Monto_fijo_mes, decimal Monto_fijo_anio, decimal Porcentaje, decimal Desde_km, decimal Hasta_km, decimal Monto_rango_km, decimal Monto_distancia_km, decimal Monto_frecuencia, bool Activo, DateTime Fecha)
        {
            _tipo_operacion = Tipo_operacion;
            _id_parametroganancia = Id_parametroganancia;
            _monto_fijo_mes = Monto_fijo_mes;
            _monto_fijo_anio = Monto_fijo_anio;
            _porcentaje = Porcentaje;
            _desde_km = Desde_km;
            _hasta_km = Hasta_km;
            _monto_rango_km = Monto_rango_km;
            _monto_distancia_km = Monto_distancia_km;
            _monto_frecuencia = Monto_frecuencia;
            _activo = Activo;
            _fecha = Fecha;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_parametro_ganancia_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_parametro_ganancia_individual");
                DbParameter Parametro;

                db1.AddInParameter(cmd, "id_parametroganancia", DbType.Int32, _id_parametroganancia);
                Parametro = cmd.CreateParameter(); Parametro.ParameterName = "@monto_fijo_mes"; Parametro.DbType = DbType.Decimal; Parametro.Direction = ParameterDirection.Output; Parametro.Precision = 18; Parametro.Scale = 2; cmd.Parameters.Add(Parametro);
                Parametro = cmd.CreateParameter(); Parametro.ParameterName = "@monto_fijo_anio"; Parametro.DbType = DbType.Decimal; Parametro.Direction = ParameterDirection.Output; Parametro.Precision = 18; Parametro.Scale = 2; cmd.Parameters.Add(Parametro);
                Parametro = cmd.CreateParameter(); Parametro.ParameterName = "@porcentaje"; Parametro.DbType = DbType.Decimal; Parametro.Direction = ParameterDirection.Output; Parametro.Precision = 18; Parametro.Scale = 2; cmd.Parameters.Add(Parametro);
                Parametro = cmd.CreateParameter(); Parametro.ParameterName = "@desde_km"; Parametro.DbType = DbType.Decimal; Parametro.Direction = ParameterDirection.Output; Parametro.Precision = 18; Parametro.Scale = 2; cmd.Parameters.Add(Parametro);
                Parametro = cmd.CreateParameter(); Parametro.ParameterName = "@hasta_km"; Parametro.DbType = DbType.Decimal; Parametro.Direction = ParameterDirection.Output; Parametro.Precision = 18; Parametro.Scale = 2; cmd.Parameters.Add(Parametro);
                Parametro = cmd.CreateParameter(); Parametro.ParameterName = "@monto_rango_km"; Parametro.DbType = DbType.Decimal; Parametro.Direction = ParameterDirection.Output; Parametro.Precision = 18; Parametro.Scale = 2; cmd.Parameters.Add(Parametro);
                Parametro = cmd.CreateParameter(); Parametro.ParameterName = "@monto_distancia_km"; Parametro.DbType = DbType.Decimal; Parametro.Direction = ParameterDirection.Output; Parametro.Precision = 18; Parametro.Scale = 2; cmd.Parameters.Add(Parametro);
                Parametro = cmd.CreateParameter(); Parametro.ParameterName = "@monto_frecuencia"; Parametro.DbType = DbType.Decimal; Parametro.Direction = ParameterDirection.Output; Parametro.Precision = 18; Parametro.Scale = 2; cmd.Parameters.Add(Parametro);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.AddOutParameter(cmd, "fecha", DbType.DateTime, 30);
                db1.ExecuteNonQuery(cmd);

                _monto_fijo_mes = (decimal)db1.GetParameterValue(cmd, "monto_fijo_mes");
                _monto_fijo_anio = (decimal)db1.GetParameterValue(cmd, "monto_fijo_anio");
                _porcentaje = (decimal)db1.GetParameterValue(cmd, "porcentaje");
                _desde_km = (decimal)db1.GetParameterValue(cmd, "desde_km");
                _hasta_km = (decimal)db1.GetParameterValue(cmd, "hasta_km");
                _monto_rango_km = (decimal)db1.GetParameterValue(cmd, "monto_rango_km");
                _monto_distancia_km = (decimal)db1.GetParameterValue(cmd, "monto_distancia_km");
                _monto_frecuencia = (decimal)db1.GetParameterValue(cmd, "monto_frecuencia");
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
                DbCommand cmd = db1.GetStoredProcCommand("abm_parametro_ganancia");
                db1.AddInParameter(cmd, "tipo_operacion", DbType.String, _tipo_operacion);
                db1.AddInParameter(cmd, "id_parametroganancia", DbType.Int32, _id_parametroganancia);
                db1.AddInParameter(cmd, "monto_fijo_mes", DbType.Decimal, _monto_fijo_mes);
                db1.AddInParameter(cmd, "monto_fijo_anio", DbType.Decimal, _monto_fijo_anio);
                db1.AddInParameter(cmd, "porcentaje", DbType.Decimal, _porcentaje);
                db1.AddInParameter(cmd, "desde_km", DbType.Decimal, _desde_km);
                db1.AddInParameter(cmd, "hasta_km", DbType.Decimal, _hasta_km);
                db1.AddInParameter(cmd, "monto_rango_km", DbType.Decimal, _monto_rango_km);
                db1.AddInParameter(cmd, "monto_distancia_km", DbType.Decimal, _monto_distancia_km);
                db1.AddInParameter(cmd, "monto_frecuencia", DbType.Decimal, _monto_frecuencia);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);
                db1.AddInParameter(cmd, "fecha", DbType.DateTime, _fecha);
                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_parametroganancia_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_parametroganancia = (int)db1.GetParameterValue(cmd, "id_parametroganancia_aux");
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