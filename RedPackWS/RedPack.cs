using iTextSharp.text;
using iTextSharp.text.pdf;
using RedPackWS.DAO;
using RedPackWS.WSRedpack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace RedPackWS
{
    public class RedPack
    {
        private const int IDUSUARIO = 824;
        private const string PIN = "QA /0fW+oHP6u6zutlbhpUj1pFvmVsC+UfAs63CWoouTfs=";
        private RedpackWSPortTypeClient _client = new RedpackWSPortTypeClient();
        private Guia _guia;
        private Guia _guiaResponse;
        
        public RedPack(Guia guia)
        {
            _client = new RedpackWSPortTypeClient();
            this._guia = guia;
            this._guia.remitente = new Direccion()
            {
                calle = "SAN FRANCISCO CUAUTLALPAN ",
                ciudad = "MÉXICO",
                codigoPostal = 53569,
                codigoPostalSpecified = true,
                contacto = "PRINTOMATIK",
                email = "litoprocess@litoprocess.com",
                estado = "ESTADO DE MÉXICO",
                nombre_Compania = "LITOPROCESS S.A DE C.V",
                numeroExterior = "102A",                
                pais = "MÉXICO",
                telefonos = new List<Telefono> { new Telefono { telefono="4555555",LADA="55", extension="1" } }.ToArray()
            };
          
            this._guia.moneda = "1";
            this._guia.flag = 1;
            this._guia.tipoEntrega = new IdDesc() { id = 1, equivalencia = "PAQ", descripcion = "PACKAGE" };
            
            this._guia.tipoEnvio = new IdDesc() { id = 2, equivalencia = "DOM", descripcion = "DOMICILIO" };
            
        }

        public int Predocumentar() 
        {
         
            Guia[] guias = { this._guia };
            PaqueteDAO paqueteDAO = new PaqueteDAO();
            GuiaDAO guiaDAO = new GuiaDAO();
            List<byte[]> paginas = new List<byte[]>();
            this._guiaResponse= this._client.predocumentacion(PIN, IDUSUARIO, guias).FirstOrDefault();

            int res= this.verificaResultado();
            if (res == 58)
                return res;

           
            this._guiaResponse.paquetes.ToList().ForEach(x => paginas.Add(x.formatoEtiqueta));
            byte[] archivoUnido = this.concatAndAddContent(paginas);

            PaqueteEnvio paqueteDTO = new PaqueteEnvio()
            {
               numero_guia = this._guia.numeroDeGuia,                                  
               detalle_impresion = archivoUnido,
               numero_orden = this._guia.referencia,                                
            };

            guiaDAO.MarcarGuiaAsignada(this._guia.numeroDeGuia);
            paqueteDAO.AgregarPaquete(paqueteDTO);
            return 0;
        }


        private int verificaResultado()
        {            
            if (_guiaResponse.resultadoConsumoWS.Any(x => x.gravedad == 1))
            {
                var resultado = _guiaResponse.resultadoConsumoWS.ToList().First();
                return resultado.estatus;
                //throw new Exception($"Descripcion {resultado.descripcion} estatus {resultado.estatus}");                            
            }
            return 0;
        }


        public string SolicitarRecoleccionCF()
        {
            GuiaDAO guiaDAO = new GuiaDAO();
            Guia[] guias = { this._guia };
            //var resultado = this._client.busquedaCodigoPostal(PIN, IDUSUARIO, guias);
            this._guia.fechaDocumentacion = DateTime.Now;
            this._guia.personaRecibio = "Printomatik";
            this._guia.costoSeguro = this._guia.paquetes.Sum(x=>x.peso);  // Total en peso
            this._guia.costoSeguroSpecified = true;
            this._guia.referencia = null;
            this._guia.fechaSituacion = DateTime.Now;
            this._guia.fechaDocumentacionSpecified = true;
            this._guia.situacion = new IdDesc { id = 0, idSpecified = true };    //Numero de sobres a recolectar
            this._guia.tipoEntrega = new IdDesc { id = this._guia.paquetes.Count(), idSpecified = true };  //Numero de paquetes a recolectar
            this._guia.remitente.codigoPostal = 33332;
            this._guia.remitente.email = "DE 4 A 5 DE LA TARDE";
            this._guia.moneda = "1x1x1";
            this._guiaResponse = this._client.solicitudRecoleccionCF(PIN, IDUSUARIO, guias).FirstOrDefault();

            this.verificaResultado();            
            string folioRecoleccion = this._guiaResponse.referencia;
            guiaDAO.AgregarFolioRecoleccion(this._guia.numeroDeGuia,folioRecoleccion);
            return folioRecoleccion;
            
        }

        #region Unir paginas a un archivo
        private byte[] concatAndAddContent(List<byte[]> pdfByteContent)
        {


            using (var ms = new MemoryStream())
            {
                using (var doc = new Document())
                {
                    using (var copy = new PdfSmartCopy(doc, ms))
                    {

                        doc.Open();

                        //Loop through each byte array
                        foreach (var p in pdfByteContent)
                        {

                            //Create a PdfReader bound to that byte array
                            using (var reader = new PdfReader(p))
                            {

                                //Add the entire document instead of page-by-page
                                copy.AddDocument(reader);

                            }
                        }

                        doc.Close();

                    }
                }

                //Return just before disposing
                return ms.ToArray();
            }
        }
        #endregion

        #region  Catalogo de Redpack

        

    
       

        //public IdDesc GetTipoServicioExpress()
        //{
        //    return this._client.obtieneCatalogoTipoServicio(PIN, IDUSUARIO)
        //        .FirstOrDefault()
        //        .auxiliar
        //        .Where(x => x.descripcion == "EXPRESS")
        //        .FirstOrDefault();
        //}


        //public IdDesc GetTipoServicioEcoExpress()
        //{
        //    return this._client.obtieneCatalogoTipoServicio(PIN, IDUSUARIO)
        //        .FirstOrDefault()
        //        .auxiliar
        //        .Where(x => x.descripcion == "ECOEXPRESS")
        //        .FirstOrDefault();

        //}

        //private IdDesc GetTipoEnvio()
        //{
        //    return this._client.obtieneCatalogoTipoEnvio(PIN, IDUSUARIO)
        //        .FirstOrDefault()
        //        .auxiliar
        //        .Where(x => x.descripcion == "PACKAGE")
        //        .FirstOrDefault();
        //}
        #endregion


    }
}
