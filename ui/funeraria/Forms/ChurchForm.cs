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
    public partial class ChurchForm: Form
    {
        private int? churchId;
        public ChurchForm()
        {
            InitializeComponent();
        }

        public ChurchForm(int id) : this()
        {
            this.churchId = id;
            this.Text = "Edit Church";
            label1.Text = "EDIT CHURCH";
            LoadChurchData(id);
        }

        private void LoadChurchData(int id)
        {
            Database database = Database.GetDatabase();
            DataTable dt = database.GetChurchById(id);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtLocation.Text = row["Location"].ToString();
                    txtName.Text = row["Name"].ToString();
                }
                else
                {
                    MessageBox.Show("Church not found.");
                }
            }
            else
            {
                MessageBox.Show("Church not found.");
            } 
        }

        private void btnSaveChurch_Click_1(object sender, EventArgs e)
        {
            Database database = Database.GetDatabase();

            string location = txtLocation.Text;
            string name = txtName.Text;

            if (string.IsNullOrEmpty(location) || string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            bool success;
            if (churchId.HasValue)
            {
                success = database.UpdateChurch(churchId.Value, location, name);
                MessageBox.Show(success ? "Church updated successfully." : "Failed to update Church.");
            }
            else
            {
                success = database.AddChurch(location, name);
                MessageBox.Show(success ? "Church added successfully." : "Failed to add Church.");
            }

            if (success)
            {
                txtLocation.Clear();
                txtName.Clear();
                this.Close();
            }
        }

        private void btnExitChurch_Click_1(object sender, EventArgs e)
        {
            txtName.Clear();
            txtLocation.Clear();

            this.Close();
        }
    }
}
