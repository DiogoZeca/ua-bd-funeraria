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
    public partial class PriestForm: Form
    {
        private string priestBi;
        private bool editMode = false;
        public PriestForm()
        {
            InitializeComponent();
        }

        public PriestForm(string bi) : this()
        {
            this.priestBi = bi;
            this.editMode = true;
            this.Text = "Edit Priest";
            label1.Text = "EDIT PRIEST";
            LoadPriestData(bi);
        }

        private void LoadPriestData(string bi)
        {
            Database database = Database.GetDatabase();
            DataTable dt = database.GetPriestById(bi);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtName.Text = row["Name"].ToString();
                    txtContact.Text = row["Contact"].ToString();
                    txtBi.Text = row["BI"].ToString();
                    txtPrice.Text = Convert.ToDecimal(row["Price"]).ToString("F2");
                    comboBox1.SelectedItem = row["Title"].ToString();
                }
                else
                {
                    MessageBox.Show("Priest not found.");
                }
            }
            else
            {
                MessageBox.Show("Priest not found.");
            }
        }

        private void btnExitPriest_Click(object sender, EventArgs e)
        {
            txtBi.Clear();
            txtName.Clear();
            txtContact.Clear();
            txtPrice.Clear();

            comboBox1.SelectedIndex = -1;
            this.Close();
        }

        private void btnSavePriest_Click(object sender, EventArgs e)
        {
            Database database = Database.GetDatabase();

            string name = txtName.Text;
            string contact = txtContact.Text;
            string priceText = txtPrice.Text;
            string bi = txtBi.Text;
            string title = comboBox1.SelectedItem?.ToString() ?? string.Empty;
            bool success;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(contact) || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(bi))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (!decimal.TryParse(priceText, out decimal price))
            {
                MessageBox.Show("Invalid price format. Please enter a valid decimal number.");
                return;
            }

            if (this.editMode)
            {
                success = database.UpdatePriest(bi, name, price, contact, title);
                MessageBox.Show(success ? "Priest updated successfully." : "Failed to update Priest.");
            }
            else
            {
                if (database.PriestExists(bi))
                {
                    MessageBox.Show("Priest with this BI already exists.");
                    return;
                }

                success = database.AddPriest(bi, name, price, contact, title);
                MessageBox.Show(success ? "Priest added successfully." : "Failed to add Priest.");
            }

            if (success)
            {
                txtContact.Clear();
                txtName.Clear();
                txtBi.Clear();
                txtPrice.Clear();
                comboBox1.SelectedIndex = -1;
                this.Close();
            }
        }
    }
}
