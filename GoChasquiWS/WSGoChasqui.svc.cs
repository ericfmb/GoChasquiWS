using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace gochasqui
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "WSGoChasqui" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione WSGoChasqui.svc o WSGoChasqui.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class WSGoChasqui : IWSGoChasqui
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }
    }
}
