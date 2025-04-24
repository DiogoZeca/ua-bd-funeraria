using System;
using System.Windows.Forms;

namespace funeraria
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validação para garantir que os campos não estejam vazios
                if (string.IsNullOrWhiteSpace(txtUsuario.Text))
                {
                    MessageBox.Show("Por favor, digite o nome de usuário.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsuario.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtSenha.Text))
                {
                    MessageBox.Show("Por favor, digite a senha.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSenha.Focus();
                    return;
                }

                // Simulação de validação de credenciais (substituir por lógica real)
                if (ValidarCredenciais(txtUsuario.Text, txtSenha.Text))
                {
                    MessageBox.Show("Login realizado com sucesso!", "Sucesso",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close(); // Fecha o formulário após login bem-sucedido
                }
                else
                {
                    MessageBox.Show("Usuário ou senha incorretos!", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSenha.Clear();
                    txtUsuario.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            // Define o resultado como Cancel e fecha o formulário
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool ValidarCredenciais(string usuario, string senha)
        {
            // Substituir por lógica real, como consulta a um banco de dados
            return usuario == "admin" && senha == "123456";
        }
    }
}