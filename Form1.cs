using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.IO;
using PowerBIExtensionMethods;
using System.Web.Script.Serialization;
using System.Net;


namespace PowerBIAPI
{
    public partial class Form1 : Form
    {

        private static string clientID = Properties.Settings.Default.ClientID;

        //RedirectUri you used when you registered your app.
        //For a client app, a redirect uri gives AAD more details on the specific application that it will authenticate.
        private static string redirectUri = "https://login.live.com/oauth20_desktop.srf";

        //Resource Uri for Power BI API
        private static string resourceUri = Properties.Settings.Default.PowerBiAPI;

        //OAuth2 authority Uri
        private static string authority = Properties.Settings.Default.AADAuthorityUri;

        private static AuthenticationContext authContext = null;
        private static string token = String.Empty;

        //Uri for Power BI datasets
        private static string datasetsUri = Properties.Settings.Default.PowerBiDataset;

        //Example dataset name and group name
        private static string datasetName = "SalesMarketing";
        private static string groupName = "Q1 Product Group";


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnCreateDataset_Click(object sender, EventArgs e)
        {
            string tableName = "Product";
            CreateDataset();

        }
        void CreateDataset()
        {
            //In a production application, use more specific exception handling.           
            try
            {
                //Create a POST web request to list all datasets
                HttpWebRequest request = DatasetRequest(String.Format("{0}/datasets", datasetsUri), "POST", AccessToken());

                //Get a list of datasets 
                dataset ds = GetDatasets().value.GetDataset(datasetName);

                if (ds == null)
                {
                    //POST request using the json schema from Product
                    Console.WriteLine(PostRequest(request, new Product().ToDatasetJson(datasetName)));
                }
                else
                {
                    Console.WriteLine("Dataset exists");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //Groups: The Create Dataset operation can also create a dataset in a group
        //POST https://api.PowerBI.com/v1.0/myorg/groups/{group_id}/datasets
        //Create Dataset operation: https://msdn.microsoft.com/en-US/library/mt203562(Azure.100).aspx
         void CreateDataset(string groupId)
        {
            //In a production application, use more specific exception handling.           
            try
            {
                //Create a POST web request to list all datasets
                HttpWebRequest request = DatasetRequest(String.Format("{0}/groups/{1}/datasets", datasetsUri, groupId), "POST", AccessToken());

                //Get a list of datasets in groupId
                dataset[] groupDatasets = GetDatasets(groupId).value;

                if (groupDatasets.Count() == 0)
                {
                    //POST request using the json schema from Product into groupId
                    Console.WriteLine(PostRequest(request, new Product().ToDatasetJson(datasetName)));
                }
                else
                {
                    Console.WriteLine("Dataset exists in this group.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //The Get Datasets operation returns a JSON list of all Dataset objects that includes a name and id.
        //GET https://api.powerbi.com/v1.0/myorg/datasets
        //Get Dataset operation: https://msdn.microsoft.com/en-US/library/mt203567.aspx
         Datasets GetDatasets()
        {
            Datasets response = null;

            //In a production application, use more specific exception handling.
            try
            {
                //Create a GET web request to list all datasets
                HttpWebRequest request = DatasetRequest(String.Format("{0}/datasets", datasetsUri), "GET", AccessToken());

                //Get HttpWebResponse from GET request
                string responseContent = GetResponse(request);

                JavaScriptSerializer json = new JavaScriptSerializer();
                response = (Datasets)json.Deserialize(responseContent, typeof(Datasets));
            }
            catch (Exception ex)
            {
                //In a production application, handle exception
            }

            return response;
        }

        //Groups: The Get Datasets operation can also get datasets in a group
        //GET https://api.powerbi.com/v1.0/myorg/groups/{group_id}/datasets
        //Get Dataset operation: https://msdn.microsoft.com/en-US/library/mt203567.aspx
         Datasets GetDatasets(string groupId)
        {
            Datasets response = null;

            //In a production application, use more specific exception handling.
            try
            {
                //Create a GET web request to list all datasets in a group
                HttpWebRequest request = DatasetRequest(String.Format("{0}/groups/{1}/datasets", datasetsUri, groupId), "GET", AccessToken());

                //Get HttpWebResponse from GET request
                string responseContent = GetResponse(request);

                JavaScriptSerializer json = new JavaScriptSerializer();
                response = (Datasets)json.Deserialize(responseContent, typeof(Datasets));
            }
            catch (Exception ex)
            {
                //In a production application, handle exception
            }

            return response;
        }

        //The Get Tables operation returns a JSON list of Tables for the specified Dataset.
        //GET https://api.powerbi.com/v1.0/myorg/datasets/{dataset_id}/tables
        //Get Tables operation: https://msdn.microsoft.com/en-US/library/mt203556.aspx
         Tables GetTables(string datasetId)
        {
            Tables response = null;

            //In a production application, use more specific exception handling.
            try
            {
                //Create a GET web request to list all datasets
                HttpWebRequest request = DatasetRequest(String.Format("{0}/datasets/{1}/tables", datasetsUri, datasetId), "GET", AccessToken());

                //Get HttpWebResponse from GET request
                string responseContent = GetResponse(request);

                JavaScriptSerializer json = new JavaScriptSerializer();
                response = (Tables)json.Deserialize(responseContent, typeof(Tables));
            }
            catch (Exception ex)
            {
                //In a production application, handle exception
            }

            return response;
        }

        //Groups: The Get Tables operation returns a JSON list of Tables for the specified Dataset in a Group.
        //GET https://api.powerbi.com/v1.0/myorg/groups/{group_id}/datasets/{dataset_id}/tables
        //Get Tables operation: https://msdn.microsoft.com/en-US/library/mt203556.aspx
         Tables GetTables(string groupId, string datasetId)
        {
            Tables response = null;

            //In a production application, use more specific exception handling.
            try
            {
                //Create a GET web request to list all datasets
                HttpWebRequest request = DatasetRequest(String.Format("{0}/groups/{1}/datasets/{2}/tables", datasetsUri, groupId, datasetId), "GET", AccessToken());

                //Get HttpWebResponse from GET request
                string responseContent = GetResponse(request);

                JavaScriptSerializer json = new JavaScriptSerializer();
                response = (Tables)json.Deserialize(responseContent, typeof(Tables));
            }
            catch (Exception ex)
            {
                //In a production application, handle exception
            }

            return response;
        }

        //The Add Rows operation adds Rows to a Table in a Dataset.
        //POST https://api.powerbi.com/v1.0/myorg/datasets/{dataset_id}/tables/{table_name}/rows
        //Add Rows operation: https://msdn.microsoft.com/en-US/library/mt203561.aspx
         void AddRows(string datasetId, string tableName)
        {
            //In a production application, use more specific exception handling. 
            try
            {
                HttpWebRequest request = DatasetRequest(String.Format("{0}/datasets/{1}/tables/{2}/rows", datasetsUri, datasetId, tableName), "POST", AccessToken());

                //Create a list of Product
                List<Product> products = new List<Product>
                {
                    new Product{ProductID =  Int32.Parse(textBox2.Text)*100+1, Name="Adjustable Race", Category="Components", IsCompete = true, ManufacturedOn = new DateTime(2014, 7, 30)},
                    new Product{ProductID = Int32.Parse(textBox2.Text)*100+2, Name="LL Crankarm", Category="Components", IsCompete = true, ManufacturedOn = new DateTime(2014, 7, 30)},
                    new Product{ProductID = Int32.Parse(textBox2.Text)*100+3, Name="HL Mountain Frame - Silver", Category="Bikes", IsCompete = true, ManufacturedOn = new DateTime(2014, 7, 30)},
                };

                //POST request using the json from a list of Product
                //NOTE: Posting rows to a model that is not created through the Power BI API is not currently supported. 
                //      Please create a dataset by posting it through the API following the instructions on http://dev.powerbi.com.
                textBox1.Text = (PostRequest(request, products.ToJson(JavaScriptConverter<Product>.GetSerializer())));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //Groups: The Add Rows operation adds Rows to a Table in a Dataset in a Group.
        //POST https://api.powerbi.com/v1.0/myorg/groups/{group_id}/datasets/{dataset_id}/tables/{table_name}/rows
        //Add Rows operation: https://msdn.microsoft.com/en-US/library/mt203561.aspx
         void AddRows(string groupId, string datasetId, string tableName)
        {
            //In a production application, use more specific exception handling. 
            try
            {
                HttpWebRequest request = DatasetRequest(String.Format("{0}/groups/{1}/datasets/{2}/tables/{3}/rows", datasetsUri, groupId, datasetId, tableName), "POST", AccessToken());

                //Create a list of Product
                List<Product> products = new List<Product>
                {
                    new Product{ProductID = 1, Name="Adjustable Race", Category="Components", IsCompete = true, ManufacturedOn = new DateTime(2014, 7, 30)},
                    new Product{ProductID = 2, Name="LL Crankarm", Category="Components", IsCompete = true, ManufacturedOn = new DateTime(2014, 7, 30)},
                    new Product{ProductID = 3, Name="HL Mountain Frame - Silver", Category="Bikes", IsCompete = true, ManufacturedOn = new DateTime(2014, 7, 30)},
                };

                //POST request using the json from a list of Product
                //NOTE: Posting rows to a model that is not created through the Power BI API is not currently supported. 
                //      Please create a dataset by posting it through the API following the instructions on http://dev.powerbi.com.
                Console.WriteLine(PostRequest(request, products.ToJson(JavaScriptConverter<Product>.GetSerializer())));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //The Delete Rows operation deletes Rows from a Table in a Dataset.
        //DELETE https://api.powerbi.com/v1.0/myorg/datasets/{dataset_id}/tables/{table_name}/rows
        //Delete Rows operation: https://msdn.microsoft.com/en-US/library/mt238041.aspx
         void DeleteRows(string datasetId, string tableName)
        {
            //In a production application, use more specific exception handling. 
            try
            {
                //Create a DELETE web request
                HttpWebRequest request = DatasetRequest(String.Format("{0}/datasets/{1}/tables/{2}/rows", datasetsUri, datasetId, tableName), "DELETE", AccessToken());
                request.ContentLength = 0;

                Console.WriteLine(GetResponse(request));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //Groups: The Delete Rows operation deletes Rows from a Table in a Dataset in a Group.
        //DELETE https://api.powerbi.com/v1.0/myorg/groups/{group_id}/datasets/{dataset_id}/tables/{table_name}/rows
        //Delete Rows operation: https://msdn.microsoft.com/en-US/library/mt238041.aspx
         void DeleteRows(string groupId, string datasetId, string tableName)
        {
            //In a production application, use more specific exception handling. 
            try
            {
                //Create a DELETE web request
                HttpWebRequest request = DatasetRequest(String.Format("{0}/groups/{1}/datasets/{2}/tables/{3}/rows", datasetsUri, groupId, datasetId, tableName), "DELETE", AccessToken());
                request.ContentLength = 0;

                Console.WriteLine(GetResponse(request));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //The Update Table Schema operation updates a Table schema in a Dataset.
        //PUT https://api.powerbi.com/v1.0/myorg/datasets/{dataset_id}/tables/{table_name}
        //Update Table Schema operation: https://msdn.microsoft.com/en-US/library/mt203560.aspx
         void UpdateTableSchema(string datasetId, string tableName)
        {
            //In a production application, use more specific exception handling.           
            try
            {
                //Create a POST web request to list all datasets
                HttpWebRequest request = DatasetRequest(String.Format("{0}/datasets/{1}/tables/{2}", datasetsUri, datasetId, tableName), "PUT", AccessToken());

                PostRequest(request, new Product2().ToTableSchema(tableName));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        //Groups: The Update Table Schema operation updates a Table schema in a Dataset in a Group.
        //PUT https://api.powerbi.com/v1.0/myorg/groups/{group_id}/datasets/{dataset_id}/tables/{table_name}
        //Update Table Schema operation: https://msdn.microsoft.com/en-US/library/mt203560.aspx
         void UpdateTableSchema(string groupId, string datasetId, string tableName)
        {
            //In a production application, use more specific exception handling.           
            try
            {
                //Create a POST web request to list all datasets
                HttpWebRequest request = DatasetRequest(String.Format("{0}/groups/{1}/datasets/{2}/tables/{3}", datasetsUri, groupId, datasetId, tableName), "PUT", AccessToken());

                PostRequest(request, new Product2().ToTableSchema(tableName));
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }

        //The Get Groups operation returns a JSON list of all the Groups that the signed in user is a member of. 
        //GET https://api.powerbi.com/v1.0/myorg/groups
        //Get Groups operation: https://msdn.microsoft.com/en-US/library/mt243842.aspx
         Groups GetGroups()
        {
            Groups response = null;

            //In a production application, use more specific exception handling.
            try
            {

                //Create a GET web request to list all datasets
                HttpWebRequest request = DatasetRequest(String.Format("{0}/groups", datasetsUri), "GET", AccessToken());

                //Get HttpWebResponse from GET request
                string responseContent = GetResponse(request);

                JavaScriptSerializer json = new JavaScriptSerializer();
                response = (Groups)json.Deserialize(responseContent, typeof(Groups));
            }
            catch (Exception ex)
            {
                //In a production application, handle exception
            }

            return response;
        }

        /// <summary>
        /// Use AuthenticationContext to get an access token
        /// </summary>
        /// <returns></returns>
         string AccessToken()
        {
            if (token == String.Empty)
            {
                //Get Azure access token
                // Create an instance of TokenCache to cache the access token
                TokenCache TC = new TokenCache();
                // Create an instance of AuthenticationContext to acquire an Azure access token
                authContext = new AuthenticationContext(authority, TC);
                // Call AcquireToken to get an Azure token from Azure Active Directory token issuance endpoint
                token = authContext.AcquireToken(resourceUri, clientID, new Uri(redirectUri), PromptBehavior.RefreshSession).AccessToken;
            }
            else
            {
                // Get the token in the cache
                token = authContext.AcquireTokenSilent(resourceUri, clientID).AccessToken;
            }

            return token;
        }

          string Import(string url, string fileName)
        {
            string responseStatusCode = string.Empty;

            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            request.KeepAlive = true;
            request.Headers.Add("Authorization", String.Format("Bearer {0}", AccessToken()));

            using (Stream rs = request.GetRequestStream())
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);

                string headerTemplate = "Content-Disposition: form-data; filename=\"{0}\"\r\nContent-Type: application / octet - stream\r\n\r\n";
                string header = string.Format(headerTemplate, fileName);
                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                rs.Write(headerbytes, 0, headerbytes.Length);

                using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead = 0;
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        rs.Write(buffer, 0, bytesRead);
                    }
                }

                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                rs.Write(trailer, 0, trailer.Length);
            }
            using (HttpWebResponse response = request.GetResponse() as System.Net.HttpWebResponse)
            {
                responseStatusCode = response.StatusCode.ToString();
            }

            return responseStatusCode;
        }

        private  string PostRequest(HttpWebRequest request, string json)
        {
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(json);
            request.ContentLength = byteArray.Length;

            //Write JSON byte[] into a Stream
            using (Stream writer = request.GetRequestStream())
            {
                writer.Write(byteArray, 0, byteArray.Length);
            }

            return GetResponse(request);
        }

        private  string GetResponse(HttpWebRequest request)
        {
            string response = string.Empty;

            using (HttpWebResponse httpResponse = request.GetResponse() as System.Net.HttpWebResponse)
            {
                //Get StreamReader that holds the response stream
                using (StreamReader reader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
                {
                    response = reader.ReadToEnd();
                }
            }

            return response;
        }

        private  HttpWebRequest DatasetRequest(string datasetsUri, string method, string accessToken)
        {
            HttpWebRequest request = System.Net.WebRequest.Create(datasetsUri) as System.Net.HttpWebRequest;
            request.KeepAlive = true;
            request.Method = method;
            request.ContentLength = 0;
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", String.Format("Bearer {0}", accessToken));

            return request;
        }

        private void btnAddNewRecord_Click(object sender, EventArgs e)
        {
            string datasetId = GetDatasets().value.GetDataset(datasetName).Id;
            dataset[] datasets = GetDatasets().value;

            table[] tables = GetTables(datasetId).value;
            string tableName = "Product";
            AddRows(datasetId, tableName);



        }

        private void btnclearAndAdd_Click(object sender, EventArgs e)
        {
            string datasetId = GetDatasets().value.GetDataset(datasetName).Id;
            dataset[] datasets = GetDatasets().value;

            table[] tables = GetTables(datasetId).value;
            string tableName = "Product";
            DeleteRows(datasetId, tableName);
            AddRows(datasetId, tableName);

        }
    }
}
