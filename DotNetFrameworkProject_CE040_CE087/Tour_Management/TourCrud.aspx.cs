using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Npgsql;
using System.Configuration;

namespace Tour_Management
{
    public partial class TourCrud : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                refreshdata();
            }
        }
        public void refreshdata()
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString);
            conn.Open();
            string selectQuery = "select * from Tour";
            NpgsqlCommand com = new NpgsqlCommand(selectQuery, conn);
          // GridView1.DataSource = selectQuery;
           // GridView1.DataBind();


            // NpgsqlConnection con = new NpgsqlConnection("Host=localhost;Port=5432;Database=tourdb;Username=postgres;Password=postgres;");
        //    NpgsqlCommand cmd = new NpgsqlCommand("select * from tbl_data", con);
         //   NpgsqlDataAdapter sda = new NpgsqlDataAdapter(cmd);
           // DataTable dt = new DataTable();
            //sda.Fill(dt);
           // GridView1.DataSource = dt;
            //GridView1.DataBind();


        }

       
    }
}
