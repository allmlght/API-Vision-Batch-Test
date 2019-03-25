using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google;
using Google.Apis.Vision.v1.Data;

namespace Cloud_Vision_Test_Batch
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IniciarServicio();
            }
            catch (Exception ex)
            {
                throw;
            }
            Console.ReadKey();
        }
        public static FileInfo[] ObtenerInfoArchivos() {
            DirectoryInfo directorio = new DirectoryInfo(@"C:\Users\nrivera\source\repos\Cloud Vision Test Batch\Cloud Vision Test Batch\Imagenes\Bch\CARTA DE VENDOR AVILES QUINTANILLA\");
            var infoImagenes = directorio.GetFiles(@"*.jpg");
            return infoImagenes;
        }
        public static void IniciarServicio() {

           //Preparacion Servicio
            VisionSettings vs = new VisionSettings();
            var credential = vs.CreateCredential();
            var service = vs.CreateService(credential);

           //Preparacion de la peticion
            BatchAnnotateImagesRequest batchRequest = new BatchAnnotateImagesRequest();
            batchRequest.Requests = new List<AnnotateImageRequest>();


            var infoImagenes = ObtenerInfoArchivos();

            foreach (var imagen in infoImagenes)
            {
                byte[] file = File.ReadAllBytes(imagen.FullName.ToString());
                batchRequest.Requests.Add(new AnnotateImageRequest()
                {
                    Features = new List<Feature>() { new Feature() { Type = "TEXT_DETECTION", MaxResults = 1 }, },
                    ImageContext = new ImageContext() { LanguageHints = new List<string>() { "es" } },
                    Image = new Image() { Content = Convert.ToBase64String(file) }
                });
            }
            var annotate = service.Images.Annotate(batchRequest);
            BatchAnnotateImagesResponse batchAnnotateImagesResponse = annotate.Execute();
            var cantidadRespuestasImagenes = batchAnnotateImagesResponse.Responses.Count();
            for (int i = 0; i < cantidadRespuestasImagenes; i++)
            {
                AnnotateImageResponse annotateImageResponse = batchAnnotateImagesResponse.Responses[i];
                if (annotateImageResponse.TextAnnotations != null)
                {
                    var texto = annotateImageResponse.TextAnnotations[0].Description;
                    using (var tw = new StreamWriter(@"D:\" + infoImagenes[i].Name.ToString() + ".txt", true))
                    {
                        tw.WriteLine(texto);
                    }
                    Console.WriteLine(texto);
                }
            }
        }
    }
}
