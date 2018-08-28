using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPackWS.DAO
{
    public class GuiaDAO:DAO
    {

        public string ObtenerGuiaDisponible(int tipoServicio)
        {
            var guia= this._context.GuiaPaqueteria
                .Where(x => x.asignada == false && x.id_tipoServicio==tipoServicio)
                .FirstOrDefault();           
            return guia.numero_guia;
            

        }

        



        public void MarcarGuiaAsignada(string numero_guia)
        {
            try
            {
                var guia = this._context.GuiaPaqueteria.Where(x => x.numero_guia == numero_guia).FirstOrDefault();
                guia.asignada = true;
                this._context.SaveChanges();
            }
             catch(Exception e)
            {
               
            }

        }


        public void AgregarFolioRecoleccion(string guia,string folioRecoleccion)
        {
            PaqueteEnvio p = this._context.PaqueteEnvio.Where(x => x.numero_guia == guia).FirstOrDefault();
            p.folio_recoleccion = folioRecoleccion;
            this._context.SaveChanges();
        }

    }
}
