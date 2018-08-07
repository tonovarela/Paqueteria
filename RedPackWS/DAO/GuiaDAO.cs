using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPackWS.DAO
{
    public class GuiaDAO:DAO
    {

        public string ObtenerGuiaDisponibleExpress()
        {
            return this._context.Database.SqlQuery<string>(@"select 
                                                            a.numero_guia
                                                            from GuiaPaqueteria a
                                                            left join PaqueteEnvio b on (a.numero_guia = b.numero_guia)
                                                            where b.numero_guia is null and a.id_tipoServicio=2 ").First();

        }
    }
}
