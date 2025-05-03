using funeraria.Entities;
using funeraria.Forms;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace funeraria
{
    public partial class LoginForm : Form
    {
        public string username;
        public static int userId;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            Database db = Database.GetDatabase();

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please enter a username!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter a password!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (db.AuthenticateUser(username, password))
            {
                MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.username = username;

                this.Hide();

                MainForm mainPage = new MainForm(this);
                mainPage.Show();
            }
            else
            {
                txtUsername.Clear();
                txtPassword.Clear();
                txtUsername.Focus();
            }
        }

        public int getUserId()
        {
            return userId;
        }

        public void clear()
        {
            txtPassword.Clear();
            txtUsername.Clear();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm();
            registerForm.Show();
            this.Hide();
        }
    }
}