using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Azure.Identity;
using Azure.Storage.Blobs;

namespace Tour_Management
{
    public partial class AddTour : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
       
        protected void Register_Click(object sender, EventArgs e)
        {
            // Replaced ConfigurationManager.ConnectionStrings (Web.config XDT transform) with
            // environment variable to support AKS ConfigMaps / Azure Key Vault CSI Driver (cz-dotnet-0055)
            string connectionString = System.Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string insertQuery = "insert into Tour(TOUR_NAME,PLACE,DAYS,PRICE,LOCATIONS,TOUR_INFO,pic) values(@TOUR_NAME,@PLACE,@DAYS,@PRICE,@LOCATIONS,@TOUR_INFO,@pic)";
            SqlCommand com = new SqlCommand(insertQuery, conn);
            
            com.Parameters.AddWithValue("@TOUR_NAME", tour_name.Text);
            com.Parameters.AddWithValue("@PLACE", place.Text);
            com.Parameters.AddWithValue("@DAYS", days.Text); 
            com.Parameters.AddWithValue("@PRICE", price.Text);
            com.Parameters.AddWithValue("@LOCATIONS", locations.Text);
            com.Parameters.AddWithValue("@TOUR_INFO", tour_info.Text);

            // cz-dotnet-1032: Replaced local container filesystem write (Server.MapPath("~/Tour_pics/"))
            // with Azure Blob Storage upload using AKS Workload Identity (DefaultAzureCredential).
            // Files now persist across pod restarts/evictions in Azure Blob Storage.
            string storageAccountUrl = System.Environment.GetEnvironmentVariable("AZURE_STORAGE_ACCOUNT_URL");
            string containerName = System.Environment.GetEnvironmentVariable("AZURE_BLOB_CONTAINER_NAME") ?? "tour-pics";
            var blobServiceClient = new BlobServiceClient(new Uri(storageAccountUrl), new DefaultAzureCredential());
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            blobContainerClient.CreateIfNotExists();
            var blobClient = blobContainerClient.GetBlobClient(FileUpload1.FileName);
            using (Stream fileStream = FileUpload1.FileContent)
            {
                blobClient.Upload(fileStream, overwrite: true);
            }

            com.Parameters.AddWithValue("@pic", FileUpload1.FileName);

            com.ExecuteNonQuery();
            Response.Write("ADD  Successful");
            //Response.Redirect("a.aspx");
            //Server.Transfer("a.aspx");
            conn.Close();
        }
    }
}
