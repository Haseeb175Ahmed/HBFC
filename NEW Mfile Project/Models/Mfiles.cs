using MFaaP.MFWSClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace NEW_Mfile_Project.Models
{
    public class Mfiles
    {

        public static string domain = "http://127.0.0.1/";

        public static async Task<string> Signed(string code, string name, string password)
        {
            string signatureImage = "403";
            // Build the url to request.
            // NOTE: http://developer.m-files.com/APIs/REST-API/#iis-compatibility
            try
            {
                
                var url =
                new Uri("http://127.0.0.1/REST/objects.aspx?o=161");

                // Build the request.
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("X-Authentication",
                    code);
                // Start the request.
                var responseBody = await httpClient.GetStringAsync(url).ConfigureAwait(false);

                // Attempt to parse it.  For this we will use the Json.NET library, but you could use others.
                var results = JsonConvert.DeserializeObject<Results<ObjectVersion>>(responseBody);

                //Console.WriteLine($"There were {} results returned.");



                
                for (int i = 0; i < results.Items.Count; i++)
                {
                    string url1 = "http://127.0.0.1/REST/objects/" + results.Items[i].ObjVer.Type + "/" + results.Items[i].ObjVer.ID + "/" + results.Items[i].ObjVer.Version + "/properties";

                    var responseBody1 = await httpClient.GetStringAsync(url1);
                    var results1 = JsonConvert.DeserializeObject<List<PropertyValue>>(responseBody1);
                    //var naa = results1.Where(x => x.PropertyDef == 0 || x.PropertyDef == 1147 || x.PropertyDef == 1148);

                   
                    var naa1 = results1.Where(x => x.PropertyDef == 0).FirstOrDefault();
                  
                    var naa2 = results1.Where(x => x.PropertyDef == 1147).FirstOrDefault();
                  
                    var naa3 = results1.Where(x => x.PropertyDef == 1148).FirstOrDefault();
                    string xname = "", sign = "";
                    


                    xname = naa1.TypedValue.Value.ToString();
                    sign = naa2.TypedValue.Value.ToString();

                    if (naa3.PropertyDef == 1148 && naa3.TypedValue.Value.ToString() == password && xname == name)
                    {
                        signatureImage = sign;
                        break;
                       
                    }

                }
            }
            catch (Exception ex)
            {
                CreateLogFiles.ErrorLog("Exception"+" "+ex.Message+" "+ex.StackTrace);


                if (ex.Message == "Response status code does not indicate success: 403 (Forbidden).")
                {
                    return "403";
                }
                return signatureImage;
            }

            if (signatureImage != "403")
            {
                signatureImage = DownloadFileAsync(signatureImage,code).Result;
            }
            return signatureImage;
        }

        public static async Task<string> DownloadFileAsync(string ID, string token)
        {
            var str = "403";

            try
            {
                var url =
            new Uri(domain+"REST/objects.aspx?o=0");

                // Build the request.
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("X-Authentication",
                    token);
                // Start the request.
                var responseBody = await httpClient.GetStringAsync(url).ConfigureAwait(false);

                // Attempt to parse it.  For this we will use the Json.NET library, but you could use others.
                var results = JsonConvert.DeserializeObject<Results<ObjectVersion>>(responseBody);


                string url1 = "";
                foreach (var createdObject in results.Items)
                {
                    if (createdObject.DisplayID == ID)
                    {
                        url1 = string.Format(
                            "http://127.0.0.1/REST/objects/{0}/{1}/{2}/files/{3}/content",
                            createdObject.ObjVer.Type, createdObject.ObjVer.ID, createdObject.ObjVer.Version, createdObject.Files[0].ID);
                        break;
                    }
                }
             
                var df = DownloadFile(url1, token);
                var bytes = ReadToEnd(df);

                 str = Convert.ToBase64String(bytes);
            }
            catch (Exception ex)
            {

                CreateLogFiles.ErrorLog("Exception" + " " + ex.Message + " " + ex.StackTrace);

            }

            return str;
        }

        public static async Task<ObjectVersion> AddFilesToMF_Object(Stream filestream, string Authcode, string objID, string filename)
        {

            ObjectVersion Newobject = new ObjectVersion();
            try
            {
                // Create a HttpClient.
                var client = new System.Net.Http.HttpClient();

                // Authenticate.
                client.DefaultRequestHeaders.Add("X-Authentication",
                    Authcode);

                // Which file do we need to upload?
                //var localFileToUpload = new System.IO.FileInfo(@"F:\PARA4.Html");

                // Create the content for the checkout request.
                // NOTE: 2 == "CheckedOutToMe" from https://developer.m-files.com/APIs/REST-API/Reference/enumerations/mfcheckoutstatus.html.
                var httpContent = new System.Net.Http.StringContent("{ \"Value\" : \"2\" }");

                // Check out the document with ID 551.
                string url = "http://127.0.0.1/REST/objects/0/" + objID + "/latest/checkedout.aspx?_method=PUT";

                // NOTE: http://developer.m-files.com/APIs/REST-API/#iis-compatibility
                var checkedOutObjectVersion = Newtonsoft.Json.JsonConvert.DeserializeObject<ObjectVersion>(
                    await (await client.PostAsync(new Uri(url), httpContent).ConfigureAwait(false)).Content.ReadAsStringAsync());

                Uri uri;

                // If it's a single-file-document, update it to multi-file-document.
                if (checkedOutObjectVersion.SingleFile)
                {
                    // Create the content for the "Single file" property.
                    httpContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new PropertyValue()
                    {
                        PropertyDef = 22, // The built-in "Single file" property.
                        TypedValue = new TypedValue()
                        {
                            DataType = MFDataType.Boolean,
                            Value = false // If "single file" is false, then it is a multi-file-document.
                        }
                    }));

                    // Update the property.
                    // NOTE: http://developer.m-files.com/APIs/REST-API/#iis-compatibility
                    uri =
                        new Uri($"http://127.0.0.1/REST/objects/0/{checkedOutObjectVersion.ObjVer.ID}/{checkedOutObjectVersion.ObjVer.Version}/properties/22.aspx?_method=PUT");
                    await client.PostAsync(uri, httpContent);
                }

                // Upload the new file to a temporary location.
                // NOTE: http://developer.m-files.com/APIs/REST-API/#iis-compatibility
                uri =
                    new Uri($"http://127.0.0.1/REST/files.aspx");
                var uploadedFile = Newtonsoft.Json.JsonConvert.DeserializeObject<UploadInfo>(
                    await (await client.PostAsync(uri, new System.Net.Http.StreamContent(filestream))).Content.ReadAsStringAsync());

                // Ensure the extension is correct.
                uploadedFile.Extension = "pdf";
                // Ensure that the name is correct.
                uploadedFile.Title = filename;

                // Create the content.
                httpContent = new System.Net.Http.StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new[] { uploadedFile }));

                // Add the file.
                // NOTE: http://developer.m-files.com/APIs/REST-API/#iis-compatibility
                await
                    client.PostAsync(new Uri($"http://127.0.0.1/REST/objects/0/{checkedOutObjectVersion.ObjVer.ID}/{checkedOutObjectVersion.ObjVer.Version}/files/upload.aspx"), httpContent);

                // Create the content for the checkin request.
                // NOTE: 0 == "CheckedIn" from https://developer.m-files.com/APIs/REST-API/Reference/enumerations/mfcheckoutstatus.html.
                httpContent = new System.Net.Http.StringContent("{ \"Value\" : \"0\" }");

                // Check in the object.
                // NOTE: http://developer.m-files.com/APIs/REST-API/#iis-compatibility
                string url1 = $"http://127.0.0.1/REST/objects/0/" + objID + "/" + checkedOutObjectVersion.ObjVer.Version + "/checkedout.aspx?_method=PUT";

                Newobject = Newtonsoft.Json.JsonConvert.DeserializeObject<ObjectVersion>(
                                        await (await client.PostAsync(new Uri(url1), httpContent)).Content.ReadAsStringAsync());




            }
            catch (Exception ex)
            {

                CreateLogFiles.ErrorLog(ex.Message + " " + ex.StackTrace);

            }

            return Newobject;
        }

        public static async Task<ObjectVersion> UpdateComments(Stream filestream, string Authcode, string objID)
        {
            ObjectVersion Newobject = new ObjectVersion();
            // Create a HttpClient.
            try
            {
                var client = new System.Net.Http.HttpClient();

                // Authenticate.
                client.DefaultRequestHeaders.Add("X-Authentication", Authcode);

                // Which file do we need to upload?
               

                // Create the content for the checkout request.
                // NOTE: 2 == "CheckedOutToMe" from https://developer.m-files.com/APIs/REST-API/Reference/enumerations/mfcheckoutstatus.html.
                var httpContent = new System.Net.Http.StringContent("{ \"Value\" : \"2\" }");

                // Check out the document with ID 551.
                // NOTE: http://developer.m-files.com/APIs/REST-API/#iis-compatibility
                var checkedOutObjectVersion = Newtonsoft.Json.JsonConvert.DeserializeObject<ObjectVersion>(
                    await (await client.PostAsync(new Uri("http://127.0.0.1/REST/objects/0/" + objID + "/latest/checkedout.aspx?_method=PUT"), httpContent).ConfigureAwait(false)).Content.ReadAsStringAsync());

                // Upload the file.
                // NOTE: http://developer.m-files.com/APIs/REST-API/#iis-compatibility
                int ID = 0;
                foreach (var item in checkedOutObjectVersion.Files)
                {
                    if (item.Name == "Comments")
                    {
                        ID = item.ID;
                    }
                }

                var uri =
                   new Uri($"http://127.0.0.1/REST/objects/0/{checkedOutObjectVersion.ObjVer.ID}/files/{ID}/content.aspx?_method=PUT");
                await client.PostAsync(uri, new System.Net.Http.StreamContent(filestream));


                // Create the content for the checkin request.
                // NOTE: 0 == "CheckedIn" from https://developer.m-files.com/APIs/REST-API/Reference/enumerations/mfcheckoutstatus.html.
                httpContent = new System.Net.Http.StringContent("{ \"Value\" : \"0\" }");

                // Check in the object.
                // NOTE: http://developer.m-files.com/APIs/REST-API/#iis-compatibility
                Newobject = Newtonsoft.Json.JsonConvert.DeserializeObject<ObjectVersion>(
                    await (await client.PostAsync(new Uri("http://127.0.0.1/REST/objects/0/" + objID + "/latest/checkedout.aspx?_method=PUT"), httpContent)).Content.ReadAsStringAsync());

            }
            catch (Exception ex)
            {

                return Newobject;
            }
            return Newobject;
        }
        public static Stream DownloadFile(string url, string AuthenticationToken)
        {


            WebRequest request = WebRequest.Create(url);

            // Fill the authentication information.
            request.Headers["X-Authentication"] = AuthenticationToken;

            // Receive the response.
            var response = request.GetResponse();
            var aa = new StreamReader(response.GetResponseStream()).BaseStream;

            return aa;
        }

        public static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

        public static string AuthToken()
        {
            string Token = "";
            try
            {
                var jsonSerializer = JsonSerializer.CreateDefault();

                var auth = new
                {
                    Username = "Admin",
                    Password = "123",
                    VaultGuid = "{E30EEB1E-891D-4BC8-B171-218DCCBE9035}" // Use GUID format with {braces}.
                };
                string url = "http://127.0.0.1/REST/server/authenticationtokens.aspx";
                var authenticationRequest = (HttpWebRequest)WebRequest.Create(url);
                authenticationRequest.Method = "POST";

                // Add the authentication details to the request stream.
                using (var streamWriter = new StreamWriter(authenticationRequest.GetRequestStream()))
                {
                    using (var jsonTextWriter = new JsonTextWriter(streamWriter))
                    {
                        jsonSerializer.Serialize(jsonTextWriter, auth);
                    }
                }

                // Execute the request.
                var authenticationResponse = (HttpWebResponse)authenticationRequest.GetResponse();

                // Extract the authentication token.
                string authenticationToken = null;
                using (var streamReader = new StreamReader(authenticationResponse.GetResponseStream()))
                {
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                    {
                        authenticationToken = ((dynamic)jsonSerializer.Deserialize(jsonTextReader)).Value;
                        Token = authenticationToken;
                    }
                }

            }
            catch (Exception ex)
            {

                CreateLogFiles.ErrorLog("Exception" + " " + ex.Message + " " + ex.StackTrace);

            }
            return Token;
        }
        public static string AuthToken(string name, string vpassword)
        {
            string Token = "";
            try
            {
                var jsonSerializer = JsonSerializer.CreateDefault();

                var auth = new
                {
                    Username = name,
                    Password = vpassword,
                    VaultGuid = "{E30EEB1E-891D-4BC8-B171-218DCCBE9035}" // Use GUID format with {braces}.
                };
                string url = "http://127.0.0.1/REST/server/authenticationtokens.aspx";
                var authenticationRequest = (HttpWebRequest)WebRequest.Create(url);
                authenticationRequest.Method = "POST";

                // Add the authentication details to the request stream.
                using (var streamWriter = new StreamWriter(authenticationRequest.GetRequestStream()))
                {
                    using (var jsonTextWriter = new JsonTextWriter(streamWriter))
                    {
                        jsonSerializer.Serialize(jsonTextWriter, auth);
                    }
                }

                // Execute the request.
                var authenticationResponse = (HttpWebResponse)authenticationRequest.GetResponse();

                // Extract the authentication token.
                string authenticationToken = null;
                using (var streamReader = new StreamReader(authenticationResponse.GetResponseStream()))
                {
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                    {
                        authenticationToken = ((dynamic)jsonSerializer.Deserialize(jsonTextReader)).Value;
                        Token = authenticationToken;
                    }
                }

            }
            catch (Exception ex)
            {
                return "Invalid UserName/Login Password";

            }
            return Token;
        }

    }
}