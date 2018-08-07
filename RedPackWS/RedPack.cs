using iTextSharp.text;
using iTextSharp.text.pdf;
using RedPackWS.DAO;
using RedPackWS.WSRedpack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPackWS
{
    public class RedPack
    {
        private const int IDUSUARIO = 1023;
        private const string PIN = "QA nj2qIrjl3Ml/TAZkiTLtv9YprmKiGTflPyzB/KTN21Y=";
        private RedpackWSPortTypeClient _client = new RedpackWSPortTypeClient();
        private Guia _guia;

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
                pais = "MÉXICO"
            };
            this._guia.moneda = "1";
            this._guia.flag = 1;
            this._guia.tipoEntrega = new IdDesc() { id = 1, equivalencia = "PAQ", descripcion = "PACKAGE" };
            this._guia.tipoServicio = this.GetTipoServicioExpress();                                               
            this._guia.tipoEnvio = new IdDesc() { id = 2, equivalencia = "DOM", descripcion = "DOMICILIO" };
            
        }

        public void Predocumentar()
        {
            Guia[] guias = { this._guia };
            var predocumentacion = this._client.predocumentacion(PIN, IDUSUARIO, guias);
            PaqueteDAO paqueteDAO = new PaqueteDAO();
            List<byte[]> paginas = new List<byte[]>();


            var resultadoConsumoWS = predocumentacion.FirstOrDefault().resultadoConsumoWS;

            if (resultadoConsumoWS.Any(x => x.gravedad == 1))
            {
                resultadoConsumoWS.ToList().ForEach(x =>
                {
                    Console.WriteLine($"Descripcion {x.descripcion} estatus {x.estatus}");
                });
                return;
            }
            predocumentacion.FirstOrDefault().paquetes.ToList().ForEach(x => paginas.Add(x.formatoEtiqueta));
            byte[] archivoUnido = this.concatAndAddContent(paginas);
            PaqueteEnvio paqueteDTO = new PaqueteEnvio()
            {
               numero_guia = this._guia.numeroDeGuia,                                  
               detalle_impresion = archivoUnido,
               numero_orden = this._guia.referencia,                                
            };

            paqueteDAO.AgregarPaquete(paqueteDTO);
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

        //private IdDesc GetTiposEntregaDomicilio()
        //{
        //    return this._client.obtieneCatalogoTipoEntrega(PIN, IDUSUARIO)
        //                        .FirstOrDefault()
        //                        .auxiliar
        //                        .Where(x => x.descripcion == "DOMICILIO")
        //                        .FirstOrDefault();
        //}

        private IdDesc GetTipoServicioExpress()
        {
            return this._client.obtieneCatalogoTipoServicio(PIN, IDUSUARIO)
                .FirstOrDefault()
                .auxiliar
                .Where(x => x.descripcion == "EXPRESS")
                .FirstOrDefault();
        }

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
