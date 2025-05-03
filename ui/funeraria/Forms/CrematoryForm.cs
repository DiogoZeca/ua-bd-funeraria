using funeraria.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace funeraria.Forms
{
    public partial class CrematoryForm: Form
    {

        private int? crematoryId;
        public CrematoryForm()
        {
            InitializeComponent();
        }

        public CrematoryForm(int id) : this()
        {
            this.crematoryId = id;
            this.Text = "Edit Crematory";
            label1.Text = "EDIT CREMATORY";
            LoadCrematoryData(id);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            txtContact.Clear();
            txtLocation.Clear();
            txtContact.Clear();

            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Database database = Database.GetDatabase();

            string location = txtLocation.Text;
            string contact = txtContact.Text;
            string priceText = txtPrice.Text;

            if (string.IsNullOrEmpty(location) || string.IsNullOrEmpty(contact))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (!decimal.TryParse(priceText, out decimal price))
            {
                MessageBox.Show("Invalid price format. Please enter a valid decimal number.");
                return;
            }

            bool success;
            if (crematoryId.HasValue)
            {
                success = database.UpdateCrematory(crematoryId.Value, location, contact, price);
            }
            else
            {
                success = database.AddCrematory(location, contact, price);
            }

            if (success)
            {
                txtLocation.Clear();
                txtContact.Clear();
                txtPrice.Clear();
                this.Close();
            }
        }

        private void LoadCrematoryData(int id)
        {
            Database database = Database.GetDatabase();
            DataTable dt = database.GetCrematoryById(id);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtLocation.Text = row["Location"].ToString();
                    txtContact.Text = row["Contact"].ToString();
                    txtPrice.Text = Convert.ToDecimal(row["Price"]).ToString("F2");
                }
                else
                {
                    MessageBox.Show("Crematory not found.");
                }
            }
            else
            {
                MessageBox.Show("Crematory not found.");
            }
        }
    }
}
