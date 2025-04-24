using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace funeraria
{
    static class Program
    {
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            LoginForm loginForm = new LoginForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                // Usuário logou com sucesso, abrir o formulário principal
                Application.Run(new MainForm()); // Substitua MainForm pelo nome do seu formulário principal
            }
            // Se cancelar, o aplicativo simplesmente termina
        }
    }
}