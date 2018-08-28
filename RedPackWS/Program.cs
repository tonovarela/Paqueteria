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



            GuiaViewModel model = new GuiaViewModel();
            ClienteDAO clienteDAO = new ClienteDAO();
            OrdenDAO ordenDAO = new OrdenDAO();
            GuiaDAO guiaDAO = new GuiaDAO();
            Guia guia = new Guia();


            model.numeroOrden = "400000255";
            model.observaciones = "Cajas con contenido frágil";





            var orden = ordenDAO.ObtenerPorID(model.numeroOrden);

            if (orden == null)
            {
                Console.WriteLine($"La orden {model.numeroOrden} no esta registrada");
                return;
            }
            //if (ordenDAO.TieneGuiaAsignada(model.numeroOrden))
            //{
            //    Console.WriteLine($"La orden {model.numeroOrden} se le asignó una guia anteriormente");
            //    return;
            //}


            model.paquetes = new List<PaqueteViewModel>() {
                new PaqueteViewModel() { peso = 1f },
                new PaqueteViewModel() { peso = 1f },
                new PaqueteViewModel() { peso = 1f },
                new PaqueteViewModel() { peso = 1f },
                new PaqueteViewModel() { peso = 1f },
                new PaqueteViewModel() { peso = 1f },
                new PaqueteViewModel() { peso = 1f },
                new PaqueteViewModel() { peso = 1f }
            };


            var cliente = clienteDAO.ObtenerPorID(orden.id_cliente);


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
                calle = $"{orden.calle.ToUpper()}  {orden.estado.ToUpper()}",
                codigoPostal = int.Parse(orden.codigo_postal),
                codigoPostalSpecified = true,
                email = cliente.email.ToLower(),
                contacto = $"{cliente.nombre.ToUpper()} {cliente.apellidos.ToUpper()}",
                telefonos = new List<Telefono> { new Telefono { telefono = orden.telefono } }.ToArray()

            };


            RedPack redpack = new RedPack(guia);

            guia.numeroDeGuia = guiaDAO.ObtenerGuiaDisponible(1);
            guia.tipoServicio = new IdDesc() { id = 1, idSpecified = true, descripcion = "EXPRESS", equivalencia = "EXPRESS" };
            //guia.tipoServicio = new IdDesc() { id = 2, idSpecified = true, descripcion = "ECOEXPRESS", equivalencia = "ECOEXPRESS" };



            //guia.numeroDeGuia = guiaDAO.ObtenerGuiaDisponible(2);
            //guia.tipoServicio = new IdDesc() { id = 2, idSpecified = true, descripcion = "ECOEXPRESS", equivalencia = "ECOEXPRESS" };


            //int res=  redpack.Predocumentar();
            //   Console.WriteLine(res);
            while (redpack.Predocumentar() == 58)
            {
                Console.WriteLine($"Consumiendo el numero de guia {guia.numeroDeGuia}");
                guia.numeroDeGuia = guiaDAO.ObtenerGuiaDisponible(1);
                guiaDAO.MarcarGuiaAsignada(guia.numeroDeGuia);

            }

            string folioRecoleccion=redpack.SolicitarRecoleccionCF();

            Console.WriteLine($"Generacion de Guia {guia.numeroDeGuia} con el folio { folioRecoleccion} para la orden {model.numeroOrden}");            

            Console.ReadLine();
        }



    }
}






