using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPackWS.DAO
{
    public class ClienteDAO:DAO
    {

        public Cliente ObtenerPorID(string id_cliente)
        {
            return _context.Cliente.Where(x=>x.id_cliente==id_cliente).FirstOrDefault();
        }
    }
}
