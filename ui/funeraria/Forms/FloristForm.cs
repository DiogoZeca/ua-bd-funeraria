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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace funeraria.Forms
{
    public partial class FloristForm: Form
    {
        private int? nif;
        private bool editMode = false;
        public FloristForm()
        {
            InitializeComponent();
        }

        public FloristForm(int nif) : this()
        {
            this.nif = nif;
            editMode = true;
            this.Text = "Edit Florist";
            label1.Text = "EDIT FLORIST";
            LoadFloristData(nif);
        }

        private void LoadFloristData(int nif)
        {
            Database database = Database.GetDatabase();
            DataTable dt = database.GetFloristByNif(nif);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtName.Text = row["Name"].ToString();
                    txtContact.Text = row["Contact"].ToString();
                    txtNif.Text = row["Nif"].ToString();
                    txtAddress.Text = row["Address"].ToString();
                }
                else
                {
                    MessageBox.Show("Florist not found.");
                }
            }
            else
            {
                MessageBox.Show("Florist not found.");
            }
        }

        private void btnExitFlorist_Click(object sender, EventArgs e)
        {
            txtAddress.Clear();
            txtContact.Clear();
            txtNif.Clear();
            txtName.Clear();

            this.Close();
        }

        private void btnSaveFlorist_Click(object sender, EventArgs e)
        {
            Database database = Database.GetDatabase();

            string name = txtName.Text;
            string contact = txtContact.Text;
            string nifText = txtNif.Text;
            string address = txtAddress.Text;
            bool success;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(contact) || string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (!int.TryParse(nifText, out int nif))
            {
                MessageBox.Show("Invalid nif format. Please enter a valid decimal number.");
                return;
            }

            if (this.editMode)
            {
                success = database.UpdateFlorist(nif, name, contact, address);
                MessageBox.Show(success ? "Florist updated successfully." : "Failed to update Florist.");
            }
            else
            {
                if (database.FloristExists(nif))
                {
                    MessageBox.Show("Florist with this NIF already exists.");
                    return;
                }

                success = database.AddFlorist(nif, name, contact, address);
                MessageBox.Show(success ? "Florist added successfully." : "Failed to add Florist.");
            }

            if (success)
            {
                txtContact.Clear();
                txtName.Clear();
                txtNif.Clear();
                txtAddress.Clear();
                this.Close();
            }
        }
    }
}
