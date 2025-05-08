using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using funeraria.Entities;

namespace funeraria.Forms
{
    public partial class InfoProcessForm : Form
    {
        private int id;
        private int panelScrollPosition = 0;
        private int totalContentHeight = 0;

        public InfoProcessForm()
        {
            InitializeComponent();
            comboFuneralTypeBox.SelectedIndexChanged += ComboFuneralTypeBox_SelectedIndexChanged;
        }
        public InfoProcessForm(int id)
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
        }



        private void InfoProcessForm_Load(object sender, EventArgs e)
        {
            // Calculate total height of content in panel
            CalculateContentHeight();

            // Load data for all ComboBoxes
            LoadComboBoxData();

            // If editing an existing process, select the appropriate values
            if (id > 0)
            {
                //LoadProcessData(id);
            }

        }

        private void CalculateContentHeight()
        {
            int maxBottom = 0;
            foreach (Control c in panel1.Controls)
            {
                maxBottom = Math.Max(maxBottom, c.Bottom);
            }

            panel1.AutoScrollMinSize = new Size(panel1.ClientSize.Width, maxBottom + 20);
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void LoadComboBoxData()
        {
            // Get instance of Database
            Database db = Database.GetDatabase();

            // Load funeral types
            LoadFuneralTypes();

            // Load priests
            LoadPriests();

            // Load churches
            LoadChurches();

            // Load urns
            LoadUrns();

            // Load coffins
            LoadCoffins();
        }

        private void LoadFuneralTypes()
        {
            comboFuneralTypeBox.Items.Clear();
            comboFuneralTypeBox.Items.Add("Burial");
            comboFuneralTypeBox.Items.Add("Cremation");
        }

        private void LoadPriests()
        {
            Database db = Database.GetDatabase();
            comboPriestBox.Items.Clear();

            DataTable priests = db.GetAllPriestList();
            foreach (DataRow row in priests.Rows)
            {
                string priestBi = row["representative_bi"].ToString();
                string priestName = db.GetPriestNameByPriestBi(priestBi);
                string priestTitle = row["title"].ToString();
                comboPriestBox.Items.Add($"{priestTitle} {priestName} ({priestBi})");
                comboPriestBox.Tag = priestBi; // Store the BI for reference
            }
        }

        private void LoadChurches()
        {
            Database db = Database.GetDatabase();
            comboCerimonyPlaceBox.Items.Clear();

            DataTable churches = db.GetAllChurcList();
            foreach (DataRow row in churches.Rows)
            {
                string churchId = row["id"].ToString();
                string churchName = row["name"].ToString();
                string churchLocation = row["location"].ToString();
                comboCerimonyPlaceBox.Items.Add($"{churchName} - {churchLocation}");
                comboCerimonyPlaceBox.Tag = churchId;
            }
        }

        private void LoadUrns()
        {
            Database db = Database.GetDatabase();
            comboUrnBox.Items.Clear();

            DataTable products = db.GetAllProductId();
            foreach (DataRow row in products.Rows)
            {
                decimal productId = Convert.ToDecimal(row["id"]);
                string type = db.GetProductTypeById(productId);
                if (type == "Urn")
                {
                    comboUrnBox.Items.Add($"(ID: {productId})");
                }
            }
        }

        private void LoadCoffins()
        {
            Database db = Database.GetDatabase();
            comboCoffinBox.Items.Clear();

            DataTable products = db.GetAllProductId();
            foreach (DataRow row in products.Rows)
            {
                decimal productId = Convert.ToDecimal(row["id"]);
                string type = db.GetProductTypeById(productId);
                if (type == "Coffin")
                {
                    comboCoffinBox.Items.Add($"(ID: {productId })");
                }
            }
        }

        private void LoadProcessData(int processId)
        {
            Database db = Database.GetDatabase();

            // Load process details and set the combobox selected items
            // This depends on your database structure

            // Example for funeral type:
            // string funeralType = db.GetFuneralTypeByProcessId(processId);
            // comboBox1.SelectedItem = funeralType;

            // Example for priest:
            // string priestBi = db.GetPriestBiByProcessId(processId);
            // SelectItemWithBi(comboBox2, priestBi);

            // Similar code for other comboboxes
        }

        // Helper method to select an item containing a specific value
        private void SelectItemWithBi(ComboBox comboBox, string bi)
        {
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                string item = comboBox.Items[i].ToString();
                if (item.Contains($"({bi})"))
                {
                    comboBox.SelectedIndex = i;
                    break;
                }
            }
        }

        private string GetSelectedFuneralType()
        {
            return comboFuneralTypeBox.SelectedItem?.ToString();
        }

        private string GetSelectedPriestBi()
        {
            string priestSelection = comboPriestBox.SelectedItem?.ToString() ?? string.Empty;
            if (!string.IsNullOrEmpty(priestSelection))
            {
                int startIndex = priestSelection.LastIndexOf("(") + 1;
                int endIndex = priestSelection.LastIndexOf(")");
                if (startIndex > 0 && endIndex > startIndex)
                {
                    return priestSelection.Substring(startIndex, endIndex - startIndex);
                }
            }
            return string.Empty;
        }

        private string GetSelectedChurchId()
        {
            // Retrieve the church ID from the Tag property
            if (comboCerimonyPlaceBox.SelectedIndex >= 0)
            {
                return comboCerimonyPlaceBox.Tag?.ToString() ?? string.Empty;
            }
            return string.Empty;
        }

        private string GetSelectedUrnId()
        {
            string urnSelection = comboUrnBox.SelectedItem?.ToString() ?? string.Empty;
            if (!string.IsNullOrEmpty(urnSelection))
            {
                int startIndex = urnSelection.LastIndexOf("ID: ") + 4;
                int endIndex = urnSelection.LastIndexOf(")");
                if (startIndex > 0 && endIndex > startIndex)
                {
                    return urnSelection.Substring(startIndex, endIndex - startIndex);
                }
            }
            return string.Empty;
        }

        private string GetSelectedCoffinId()
        {
            string coffinSelection = comboCoffinBox.SelectedItem?.ToString() ?? string.Empty;
            if (!string.IsNullOrEmpty(coffinSelection))
            {
                int startIndex = coffinSelection.LastIndexOf("ID: ") + 4;
                int endIndex = coffinSelection.LastIndexOf(")");
                if (startIndex > 0 && endIndex > startIndex)
                {
                    return coffinSelection.Substring(startIndex, endIndex - startIndex);
                }
            }
            return string.Empty;
        }


        private void ComboFuneralTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check which funeral type is selected
            if (comboFuneralTypeBox.SelectedItem?.ToString() == "Burial")
            {
                // If Burial is selected, disable Urn selection
                comboUrnBox.Enabled = false;
                comboUrnBox.SelectedIndex = -1; // Clear any existing selection
                
                // Optionally add a placeholder text
                comboUrnBox.Text = "Not applicable for burial";
            }
            else // "Cremation" is selected
            {
                // If Cremation is selected, enable Urn selection
                comboUrnBox.Enabled = true;
                comboUrnBox.Text = "";
            }
        }




        private void SaveButtonProcess_Click(object sender, EventArgs e)
        {
            try
            {
                // Your existing code for personal details
                string fullName = textFullNameBox.Text;
                string bi = textIDNumberBox.Text;
                DateTime birthDate;
                if (!DateTime.TryParse(textBirthDateBox.Text, out birthDate))
                {
                    MessageBox.Show("Invalid birth date format. Please use a valid date format (e.g., DD/MM/YYYY).",
                                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                char sex = textSexBox.Text[0];
                string maritalStatus = textMaritalStatusBox.Text;
                string address = textAddressBox.Text;
                string nationality = textNationalityBox.Text;

                // Get selected values from combo boxes
                string funeralType = GetSelectedFuneralType();
                string priestBi = GetSelectedPriestBi();
                string churchId = GetSelectedChurchId();
                string urnId = GetSelectedUrnId();
                string coffinId = GetSelectedCoffinId();

                // Get client and process info
                string clientName = textClientNameBox.Text;
                string relationship = textRelationshipBox.Text;
                string processNumber = textProccessBox.Text;

                // Get funeral information
                DateTime funeralDate;
                if (!DateTime.TryParse(textFuneralDateBox.Text, out funeralDate))
                {
                    MessageBox.Show("Invalid funeral date format. Please use a valid date format (e.g., DD/MM/YYYY).",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                // Handle urn ID based on funeral type
                if (funeralType == "Cremation")
                {
                    urnId = GetSelectedUrnId();
                    if (string.IsNullOrEmpty(urnId))
                    {
                        MessageBox.Show("Please select an urn for cremation.", 
                                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }


                string local = textLocalBox.Text;


                // Fix the typo and convert all IDs to integers in one step
                int churchId;
                int urnId = 0; // Default to 0 for burial case
                int coffinId;

                // Parse churchId - update GetSelectedChurchId() first
                string churchIdStr = GetSelectedChurchId();
                if (!int.TryParse(churchIdStr, out churchId))
                {
                    MessageBox.Show("Invalid church selection.");
                    return;
                }

                // For urnId, only parse if cremation is selected
                if (funeralType == "Cremation")
                {
                    string urnIdStr = GetSelectedUrnId();
                    if (!int.TryParse(urnIdStr, out urnId))
                    {
                        MessageBox.Show("Invalid urn selection for cremation.");
                        return;
                    }
                }

                // Parse coffinId
                string coffinIdStr = GetSelectedCoffinId(); // Fix the typo
                if (!int.TryParse(coffinIdStr, out coffinId))
                {
                    MessageBox.Show("Invalid coffin selection.");
                    return;
                }

                
                // Call database method to save the process
                Database db = Database.GetDatabase();
                bool success = false;
                
                if (db.ProcessExist(processNumber)) {
                    MessageBox.Show("Process with this Number already exists.");
                    return;
                } 

                success = db.AddProcess(processNumber, fullName, bi, sex, local, funeralDate, relationship, clientName, coffinId, urnId, churchId, priestBi, funeralType, nationality, address, maritalStatus, birthDate);
                MessageBox.Show(success ? "Process added successfully." : "Failed to add Process.");
                
                if (success)
                {
                    MessageBox.Show("Process saved successfully!");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonChangeIcon_Click(object sender, EventArgs e)
        {
            // Open file dialog to select an image
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Load the selected image into the PictureBox
                    pictureBoxIcon.Image = Image.FromFile(openFileDialog.FileName);
                }
            }
        }
    }
}
