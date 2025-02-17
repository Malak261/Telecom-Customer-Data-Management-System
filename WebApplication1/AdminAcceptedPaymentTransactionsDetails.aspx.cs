using System;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AdminAcceptedPaymentTransactionsDetails : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gridName2.Visible = false;
            hello.Text = string.Empty;
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string connStr = WebConfigurationManager.ConnectionStrings["MyDatabaseConnection"].ToString();
        SqlConnection conn = new SqlConnection(connStr);

        string mobileInput = TextBox1.Text.Trim();

        // Validate mobile number length
        if (mobileInput.Length != 11)
        {
            hello.Text = "Please enter a valid 11-digit mobile number.";
            gridName2.Visible = false;
            return;
        }

        hello.Text = ""; // Clear error message

        try
        {
            using (SqlCommand cmd = new SqlCommand("Account_Payment_Points", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mobile_num", mobileInput);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    if (dt.Rows.Count > 0)
                    {
                        // Bind the DataTable directly to the GridView
                        gridName2.DataSource = dt;
                        gridName2.DataBind();
                        gridName2.Visible = true;
                        hello.Text = ""; // Clear any error message
                    }
                    else
                    {
                        hello.Text = "No payments found for the given mobile number in the last year.";
                        gridName2.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            hello.Text = "An error occurred: " + ex.Message;
            gridName2.Visible = false;
        }
        finally
        {
            conn.Close();
        }
    }
}
