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
    public partial class EditProfileForm: Form
    {
        private int userId;
        public EditProfileForm(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            ShowUser();
        }

        public void ShowUser()
        {
            Database db = Database.GetDatabase();

            DataRow userInfo = db.GetUserbyId(this.userId);
            txtEmail.Text = userInfo["mail"].ToString();
            txtUsername.Text = userInfo["username"].ToString();
            txtName.Text = userInfo["name"].ToString();
            txtPassword.Text = userInfo["password"].ToString();

            // Profile Picture
            if (userInfo["ProfilePicture"] != DBNull.Value)
            {
                byte[] imageData = (byte[])userInfo["ProfilePicture"];
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(imageData))
                {
                    pictureProfile.Image = Image.FromStream(ms);
                }
            }
            else
            {
                pictureProfile.Image = global::funeraria.Properties.Resources.user_male;
            }
        }

        private void btnSaveProfile_Click(object sender, EventArgs e)
        {
            String name = txtName.Text;
            String email = txtEmail.Text;
            String username = txtUsername.Text;
            String password = txtPassword.Text;
            String passConfirm = txtPasswordConfirm.Text;

            MemoryStream ms = new MemoryStream();
            byte[] img = new byte[0];

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (pictureProfile.Image != null)
            {
                pictureProfile.Image.Save(ms, pictureProfile.Image.RawFormat);
                img = ms.ToArray();
            }

            if (password != passConfirm)
            {
                MessageBox.Show("Passwords do not match.");
            } else
            {
                Database db = Database.GetDatabase();
                if (db.UpdateUser(this.userId, name, email, username, password, img)) {
                    txtEmail.Clear();
                    txtName.Clear();
                    txtPassword.Clear();
                    txtPasswordConfirm.Clear();
                    txtUsername.Clear();

                    this.Close();
                }
            }

            
        }

        private void btnExitProfile_Click(object sender, EventArgs e)
        {
            txtEmail.Clear();
            txtName.Clear();
            txtPassword.Clear();
            txtPasswordConfirm.Clear();
            txtUsername.Clear();
            this.Close();
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
                        pictureProfile.Image = new Bitmap(dlg.FileName);
                        pictureProfile.Visible = true;
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
