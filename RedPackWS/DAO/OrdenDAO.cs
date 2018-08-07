using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPackWS.DAO
{
    public class OrdenDAO : DAO
    {



        public Orden ObtenerPorID(string numero_orden)
        {
            return this._context.Orden
                        .Where(x=>x.numero_orden==numero_orden)
                        .FirstOrDefault();
        }
    }

}
