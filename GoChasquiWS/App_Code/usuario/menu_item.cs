using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace gochasqui
{
    public class menu_item
    {
        //Base de datos
        private static Database db1 = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["conn"]);

        #region Propiedades
        //Propiedades privadas
        private int _id_menuitem = 0;
        private int _id_menu = 0;
        private string _url = "";
        private bool _activo = true;

        private string _error = "";
        //Propiedades públicas
        public int id_menuitem { get { return _id_menuitem; } set { _id_menuitem = value; } }
        public int id_menu { get { return _id_menu; } set { _id_menu = value; } }
        public string url { get { return _url; } set { _url = value; } }
        public bool activo { get { return _activo; } set { _activo = value; } }

        public string error { get { return _error; } set { _error = value; } }
        #endregion

        #region Constructores
        public menu_item(int Id_menuitem)
        {
            _id_menuitem = Id_menuitem;
            RecuperarDatos();
        }
        public menu_item(int Id_menuitem, int Id_menu, string Url, bool Activo)
        {
            _id_menuitem = Id_menuitem;
            _id_menu = Id_menu;
            _url = Url;
            _activo = Activo;
        }
        #endregion

        #region Métodos que NO requieren constructor
        public static DataTable Lista(int Id_usuario)
        {
            DbCommand cmd = db1.GetStoredProcCommand("lista_menu_item_todos");
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
                DbCommand cmd = db1.GetStoredProcCommand("lista_menu_item_individual");
                db1.AddInParameter(cmd, "id_menuitem", DbType.Int32, _id_menuitem);
                db1.AddOutParameter(cmd, "id_menu", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "url", DbType.String, 500);
                db1.AddOutParameter(cmd, "activo", DbType.Boolean, 1);
                db1.ExecuteNonQuery(cmd);

                _id_menu = (int)db1.GetParameterValue(cmd, "id_menu");
                _url = (string)db1.GetParameterValue(cmd, "url");
                _activo = (Boolean)db1.GetParameterValue(cmd, "activo");
            }
            catch { }
        }

        public string ABM(int context_id_usuario)
        {
            string resultado = "";
            try
            {
                DbCommand cmd = db1.GetStoredProcCommand("abm_menu_item");
                db1.AddInParameter(cmd, "id_menuitem", DbType.Int32, _id_menuitem);
                db1.AddInParameter(cmd, "id_menu", DbType.Int32, _id_menu);
                db1.AddInParameter(cmd, "url", DbType.String, _url);
                db1.AddInParameter(cmd, "activo", DbType.Boolean, _activo);

                db1.AddInParameter(cmd, "id_usuario_aux", DbType.Int32, context_id_usuario);
                db1.AddOutParameter(cmd, "id_menuitem_aux", DbType.Int32, 32);
                db1.AddOutParameter(cmd, "descripcionpr", DbType.String, 250);
                db1.AddOutParameter(cmd, "error", DbType.String, 250);
                db1.ExecuteNonQuery(cmd);
                resultado = (string)db1.GetParameterValue(cmd, "descripcionpr");
                _id_menuitem = (int)db1.GetParameterValue(cmd, "id_menuitem_aux");
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