using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Phone_Book
{
    public partial class Form1 : Form
    {
        string connectionString = @"Data Source=DESKTOP-IR7UMSC;Initial Catalog=PhoneBookDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        int PhoneBookID = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GridFill();
            buttonDelete.Enabled = false;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (textFName.Text.Trim() != "" && textLName.Text.Trim() != "" && textContact.Text.Trim() != "")
            {
                Regex reg = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
                Match match = reg.Match(textEmail.Text.Trim());
                if (match.Success)
                {
                    using (SqlConnection sqlCon = new SqlConnection(connectionString))
                    {
                        sqlCon.Open();
                        SqlCommand sqlCmd = new SqlCommand("ContactAddOrEdit", sqlCon);
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@PhoneBookID", PhoneBookID);
                        sqlCmd.Parameters.AddWithValue("@FirstName", textFName.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@LastName", textLName.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@Contact", textContact.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@Email", textEmail.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@Address", textAddress.Text.Trim());
                        sqlCmd.ExecuteNonQuery();
                        MessageBox.Show("Submitted Successfully");
                        Clear();
                        GridFill();
                    }
                }
                else
                    MessageBox.Show("Email Address is not valid");
            }
            else
                MessageBox.Show("Please Fill Mandatory Fields.");
        }

        void Clear()
        {
            textFName.Text 
                = textLName.Text 
                = textContact.Text 
                = textEmail.Text 
                = textAddress.Text 
                = textSearch.Text = "";
            PhoneBookID = 0;
            buttonSave.Text = "Save";
            buttonDelete.Enabled = false;

        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        void GridFill()
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("ContactViewAll", sqlCon);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                GridPhoneBook.DataSource = dtbl;
            }
        }

        private void GridPhoneBook_DoubleClick(object sender, EventArgs e)
        {
            if(GridPhoneBook.CurrentRow.Index != -1)
            {
                textFName.Text = GridPhoneBook.CurrentRow.Cells[1].Value.ToString();
                textLName.Text = GridPhoneBook.CurrentRow.Cells[2].Value.ToString();
                textContact.Text = GridPhoneBook.CurrentRow.Cells[3].Value.ToString();
                textEmail.Text = GridPhoneBook.CurrentRow.Cells[4].Value.ToString();
                textAddress.Text = GridPhoneBook.CurrentRow.Cells[5].Value.ToString();
                PhoneBookID = Convert.ToInt32(GridPhoneBook.CurrentRow.Cells[0].Value.ToString());

                buttonSave.Text = "Update";
                buttonDelete.Enabled = true;
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlCommand sqlCmd = new SqlCommand("ContactDeleteByID", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@PhoneBookID", PhoneBookID);
                sqlCmd.ExecuteNonQuery();
                MessageBox.Show("Deleted Successfully");
                Clear();
                GridFill();
            }
        }

        private void textSearch_TextChanged(object sender, EventArgs e)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("ContactSearchByValue", sqlCon);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("@SearchValue", textSearch.Text.Trim());
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                GridPhoneBook.DataSource = dtbl;
            }
        }
    }
}
