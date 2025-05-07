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
        private int panelScrollPosition = 0;
        private int totalContentHeight = 0;

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

        public void AddControlToPanel(Control control, int positionY)
        {
            // Adiciona o controlo ao painel
            panel1.Controls.Add(control);

            // Calcula a nova altura do conteúdo
            CalculateContentHeight();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            
            // Update scroll bar when form is resized
            if (vScrollBar1 != null)
            {
                vScrollBar1.Maximum = Math.Max(0, totalContentHeight - panel1.Height);
                vScrollBar1.LargeChange = panel1.Height / 2;
            }
        }



        private void InfoProcessForm_Load(object sender, EventArgs e)
        {
            // Calculate total height of content in panel
            CalculateContentHeight();
            
            // Configure scroll bar properties
            vScrollBar1.Minimum = 0;
            vScrollBar1.Maximum = Math.Max(0, totalContentHeight - panel1.Height);
            vScrollBar1.SmallChange = 20;
            vScrollBar1.LargeChange = panel1.Height / 2;
            
            // Initialize panel scroll position
            panelScrollPosition = 0;
            UpdatePanelPosition();
        }

        private void CalculateContentHeight()
        {
            int maxBottom = 0;
            foreach (Control c in panel1.Controls)
            {
                maxBottom = Math.Max(maxBottom, c.Bottom);
            }

            // Define o tamanho mínimo necessário para o scroll
            panel1.AutoScrollMinSize = new Size(panel1.ClientSize.Width, maxBottom + 20);
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
