using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPackWS.ViewModel
{
    public class GuiaViewModel
    {
        

        public string numeroOrden { get; set; }

        public string  observaciones { get; set; }

        public IEnumerable<PaqueteViewModel> paquetes { get; set; }

        



    }
}
