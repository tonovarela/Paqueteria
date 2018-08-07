using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPackWS.DAO
{
    public class PaqueteDAO:DAO
    {

        public void AgregarPaquete(PaqueteEnvio paquete)
        {
            this._context.PaqueteEnvio.Add(paquete);
            this._context.SaveChanges();
        }
        
    }
}
