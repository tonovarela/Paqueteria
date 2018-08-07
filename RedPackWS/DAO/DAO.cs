using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPackWS.DAO
{
    public class DAO
    {

        protected CrecePlus2Entities _context;
        public DAO()
        {
            this._context = new CrecePlus2Entities();
        }



    }
}
