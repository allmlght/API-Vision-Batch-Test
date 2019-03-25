using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Vision.v1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Vision_Test_Batch
{
    class VisionSettings
    {
        public string ApplicationName { get { return "Ocr"; } }
        private string JsonKeypath
        {
            get
            {
                return @"C:\Users\nrivera\source\repos\Cloud Vision Test Batch\Cloud Vision Test Batch\TestOCR-7d6c151ad812.json";
            }
        }
        public GoogleCredential CreateCredential()
        {
            using (var stream = new System.IO.FileStream(JsonKeypath, FileMode.Open, FileAccess.Read))
            {
                string[] scopes = { VisionService.Scope.CloudPlatform };
                var credential = GoogleCredential.FromStream(stream);
                credential = credential.CreateScoped(scopes);
                return credential;
            }
        }
        public VisionService CreateService(GoogleCredential credential)
        {
            var service = new VisionService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
                GZipEnabled = true,
            });
            return service;
        }
    }
}
