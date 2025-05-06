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
    public partial class InfoProcessForm: Form
    {
        private int id;

        public InfoProcessForm()
        {
            InitializeComponent();
        }
        public InfoProcessForm( int id )
        {
            InitializeComponent();
            this.id = id;
        }

        private void PictureTrashClick_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Tem a certeza que deseja eliminar este processo?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                Database.DeleteProcess(id); // Make sure this method exists in your Database class
                this.Close();
            }
        }

        private void PictureBackClick_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }
    }
}
