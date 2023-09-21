//------------------------------------------------------------------------------
//MIT License

//Copyright(c) 2023 Microsoft Corporation. All rights reserved.

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;
using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Nito.AspNetBackgroundTasks;

namespace Storage.Blob.Dotnet.Quickstart.V12
{

    public static class Program
    {
        public static void Main()
        {
            Console.WriteLine("Azure Blob Storage - .NET QuickStart sample");
            Console.WriteLine();
            var url = UploadImage();
            Console.WriteLine(url);

            Console.WriteLine("Press enter to exit the sample application.");
            Console.ReadLine();
        }

        private static String UploadImage()
        {
            // create a temp image
            var base64encodedstring = "iVBORw0KGgoAAAANSUhEUgAAAQAAAAEACAIAAADTED8xAAADMElEQVR4nOzVwQnAIBQFQYXff81RUkQCOyDj1YOPnbXWPmeTRef+/3O/OyBjzh3CD95BfqICMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMK0CMO0TAAD//2Anhf4QtqobAAAAAElFTkSuQmCC";
            var bytes = Convert.FromBase64String(base64encodedstring);
            var fileStream = new MemoryStream(bytes);

            // add blon info
            var AccountName = "<account_name>";
            var Container = "<container_name>";
            var fileName = "<image_name.extension>";
            var AccountKey = "<account_key>";
            try
            {
                // Create a URI to the blob
                Uri blobUri = new Uri(string.Format("https://{0}.blob.core.windows.net/{1}/{2}", AccountName, Container, fileName));
                StorageSharedKeyCredential storageCredentials =
                    new StorageSharedKeyCredential(AccountName, AccountKey);

                // Create the blob client.
                BlobClient blobClient = new BlobClient(blobUri, storageCredentials);
                /*BackgroundTaskManager.Run(() =>
                {
                    var relativeURL = GetDocumentsLocationForContact(contactId);
                    //relativeURL = "/contact/" + relativeURL;
                    var res = _sharepointFileRepository.UploadFileOrFolderInSPForACS(contactId, "", "/contact/" + relativeURL, relativeURL, "Chatting", flist);
                });*/
                // Upload the file
                var respose = blobClient.Upload(fileStream);

                Azure.Storage.Sas.BlobSasBuilder blobSasBuilder = new Azure.Storage.Sas.BlobSasBuilder()
                {
                    BlobContainerName = Container,
                    BlobName = fileName,
                    ExpiresOn = DateTime.UtcNow.AddMinutes(5),
                    ContentDisposition = "inline; filename=\"example.png\"",
                    ContentType = "image/png",
                    Protocol = Azure.Storage.Sas.SasProtocol.Https
                };
                blobSasBuilder.SetPermissions(Azure.Storage.Sas.BlobSasPermissions.Read);
                Uri sasUri = blobClient.GenerateSasUri(blobSasBuilder);
                // string fileUrl = blobClient.Uri.AbsoluteUri;
                //Added .ToString()
                // string shortUrl = ShortenUrl.ShortenUrlAsync(fileUrl);
                return sasUri.ToString();
            }
            catch (Exception e)
            { 
                Console.Write("error");
                Console.WriteLine(e);
                return "";
            }
        }
    }
}
