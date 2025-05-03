using funeraria.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace funeraria.Forms
{
    public partial class RegisterForm: Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {

            string username = txtUsername.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Text;
            string confPassword = txtPasswordConfirm.Text;
            string name = txtName.Text;
            MemoryStream ms = new MemoryStream();
            byte[] img = new byte[0];

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (profilePicture.Image != null)
            {
                profilePicture.Image.Save(ms, profilePicture.Image.RawFormat);
                img = ms.ToArray();
            }

            if (password != confPassword)
            {
                MessageBox.Show("Passwords do not match.");
            }
            else
            {
                Database db = Database.GetDatabase();

                if (db.RegisterUser(username, email, password, name, img))
                {
                    this.Hide();
                    LoginForm loginForm = new LoginForm();
                    loginForm.Show();
                }
            }
        }

        private void btnPicture_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        profilePicture.Image = new Bitmap(dlg.FileName);
                        profilePicture.Visible = true; 
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
