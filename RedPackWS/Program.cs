using RedPackWS.DAO;
using RedPackWS.ViewModel;
using RedPackWS.WSRedpack;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPackWS
{
    class Program
    {
        static void Main(string[] args)
        {











            GuiaDAO guiaDAO = new GuiaDAO();
            var d = guiaDAO.ObtenerGuiaDisponibleExpress();
            Console.WriteLine(d);
            Console.ReadLine();
            return;

            
            ClienteDAO clienteDAO = new ClienteDAO();
            OrdenDAO ordenDAO = new OrdenDAO();


            GuiaViewModel model = new GuiaViewModel();

            model.observaciones = "Cajas con contenido frágil";
            model.paquetes = new List<PaqueteViewModel>() {
                new PaqueteViewModel() { peso = 3.10f },
                new PaqueteViewModel() { peso = 2.10f }
            };
            model.numeroOrden = "400000249";

            
            var orden = ordenDAO.ObtenerPorID(model.numeroOrden);
            var cliente = clienteDAO.ObtenerPorID(orden.id_cliente);
            Guia guia = new Guia();





            guia.numeroDeGuia = guiaDAO.ObtenerGuiaDisponibleExpress();
                       
            guia.referencia = model.numeroOrden;
            guia.observaciones = model.observaciones;
            List<Paquete> paquetes = new List<Paquete>();
            int i = 1;
            model.paquetes.ToList().ForEach(item =>
            {
                paquetes.Add(new Paquete()
                {
                    alto = 1,
                    ancho = 1,
                    largo = 1,
                    largoSpecified = true,
                    altoSpecified = true,
                    anchoSpecified = true,
                    pesoSpecified = true,
                    descripcion = "Paquete " + (i++),
                    peso = item.peso,
                });
            });

            guia.paquetes = paquetes.ToArray();
            

            guia.consignatario = new Direccion()
            {
                calle =$"{orden.calle.ToUpper()}  {orden.estado.ToUpper()}",
                codigoPostal = int.Parse(orden.codigo_postal),
                codigoPostalSpecified = true,
                email = cliente.email.ToLower(),
                contacto = $"{cliente.nombre.ToUpper()} {cliente.apellidos.ToUpper()}",

            };


            RedPack redpack = new RedPack(guia);            
            redpack.Predocumentar();
            Console.WriteLine($"{model.numeroOrden} procesada");
            Console.ReadLine();
        }



    }
}






