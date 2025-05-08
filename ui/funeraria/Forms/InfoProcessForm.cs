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
        private int numFunc;
        private int panelScrollPosition = 0;
        private int totalContentHeight = 0;

        public InfoProcessForm(int numFunc)
        {
            this.numFunc = numFunc;
            InitializeComponent();
            comboFuneralTypeBox.SelectedIndexChanged += ComboFuneralTypeBox_SelectedIndexChanged;
        }
        public InfoProcessForm(int id, int numFunc)
        {
            this.numFunc = numFunc;
            InitializeComponent();
            this.id = id;
        }

        private void PictureTrashClick_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Tem a certeza que deseja eliminar este processo?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                // Database.DeleteProcess(id); // Make sure this method exists in your Database class
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

            if (id > 0)
            {
                LoadProcessData(id);
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
                
                // Create ComboboxItem to properly store the BI
                ComboboxItem item = new ComboboxItem
                {
                    Text = $"{priestTitle} {priestName} ({priestBi})",
                    Value = priestBi
                };
                
                comboPriestBox.Items.Add(item);
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
                
                // Create a custom item class to store both display text and ID
                ComboboxItem item = new ComboboxItem
                {
                    Text = $"{churchName} - {churchLocation}",
                    Value = churchId
                };
                
                comboCerimonyPlaceBox.Items.Add(item);
            }
        }

        // Add this helper class to your form class
        public class ComboboxItem
        {
            public string Text { get; set; }
            public string Value { get; set; }
            
            public override string ToString()
            {
                return Text;
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
            DataTable processData = db.GetProcessById(processId);

            int churchId = db.GetChurchIdForProcess(processId);
            
            if (processData != null && processData.Rows.Count > 0)
            {
                DataRow row = processData.Rows[0];
                
                // Set basic process info
                textProccessBox.Text = row["num_process"].ToString();
                textProccessBox.ReadOnly = true; // Prevent changing process number in edit mode
                
                // Get deceased information
                string id = row["user_id"].ToString();
                textIDNumberBox.Text = id;
                
                // Get personal details - you need to adjust field names based on your database structure
                textFullNameBox.Text = db.GetDeceasedNameByProcessId(processId);
                textSexBox.Text = row["sex"].ToString();
                textMaritalStatusBox.Text = row["marital_status"].ToString();
                textAddressBox.Text = row["residence"].ToString();
                textNationalityBox.Text = row["nationality"].ToString();
                textClientIDBox.Text = row["client_id"].ToString();
                
                // Set birthdate if available
                if (row["birth_date"] != DBNull.Value)
                    textBirthDateBox.Text = Convert.ToDateTime(row["birth_date"]).ToString("dd/MM/yyyy");
                
                // Set client information
                textClientNameBox.Text = row["client_name"].ToString();
                textRelationshipBox.Text = row["degree_kinship"].ToString();
                textClientIDBox.Text = row["client_id"].ToString();
                
                // Set funeral details
                textLocalBox.Text = row["location"].ToString();
                if (row["funeral_date"] != DBNull.Value)
                    textFuneralDateBox.Text = Convert.ToDateTime(row["funeral_date"]).ToString("dd/MM/yyyy");
                
                // Set combobox values - need to select the correct items
                string funeralType = row["funeral_type"].ToString();
                comboFuneralTypeBox.SelectedItem = funeralType;
                
                // Select the correct priest
                string priestBi = row["priest_bi"].ToString();
                SelectItemWithBi(comboPriestBox, priestBi);
                
                // Select church, coffin, and urn (if applicable)
                int coffinId = 0;
                if (funeralType == "Cremation" && row["cremation_coffin_id"] != DBNull.Value)
                    coffinId = Convert.ToInt32(row["cremation_coffin_id"]);
                else if (funeralType == "Burial" && row["burial_coffin_id"] != DBNull.Value)
                    coffinId = Convert.ToInt32(row["burial_coffin_id"]);

                int urnId = (row["urn_id"] != DBNull.Value) ? Convert.ToInt32(row["urn_id"]) : 0;
                
                // Load church - this requires improving your LoadChurches method to store IDs per item
                for (int i = 0; i < comboCerimonyPlaceBox.Items.Count; i++)
                {
                    ComboboxItem item = comboCerimonyPlaceBox.Items[i] as ComboboxItem;
                    if (item != null && item.Value == churchId.ToString())
                    {
                        comboCerimonyPlaceBox.SelectedIndex = i;
                        break;
                    }
                }
                
                // Select coffin from list
                for (int i = 0; i < comboCoffinBox.Items.Count; i++)
                {
                    if (comboCoffinBox.Items[i].ToString().Contains($"ID: {coffinId}"))
                    {
                        comboCoffinBox.SelectedIndex = i;
                        break;
                    }
                }
                
                // Handle urn selection based on funeral type
                if (funeralType == "Cremation" && urnId > 0)
                {
                    for (int i = 0; i < comboUrnBox.Items.Count; i++)
                    {
                        if (comboUrnBox.Items[i].ToString().Contains($"ID: {urnId}"))
                        {
                            comboUrnBox.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Process not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            if (comboPriestBox.SelectedIndex >= 0)
            {
                ComboboxItem selectedItem = comboPriestBox.SelectedItem as ComboboxItem;
                return selectedItem?.Value ?? string.Empty;
            }
            return string.Empty;
        }

        private string GetSelectedChurchId()
        {
            // Retrieve the church ID from the Tag property
            if (comboCerimonyPlaceBox.SelectedIndex >= 0)
            {
                ComboboxItem selectedItem = comboCerimonyPlaceBox.SelectedItem as ComboboxItem;
                return selectedItem?.Value ?? string.Empty;
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

                // Get client and process info
                string clientName = textClientNameBox.Text;
                string clientId = textClientIDBox.Text;
                string relationship = textRelationshipBox.Text;
                string processNumber = textProccessBox.Text;

                if (string.IsNullOrWhiteSpace(clientId))
                {
                    MessageBox.Show("Client ID cannot be empty.", 
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Fix the typo and convert all IDs to integers in one step
                int churchId;
                int urnId = 0; // Default to 0 for burial case
                int coffinId;

                // Get funeral information
                DateTime funeralDate;
                if (!DateTime.TryParse(textFuneralDateBox.Text, out funeralDate))
                {
                    MessageBox.Show("Invalid funeral date format. Please use a valid date format (e.g., DD/MM/YYYY).",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string local = textLocalBox.Text;

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
                
                int procNumberInt;
                if (!int.TryParse(processNumber, out procNumberInt))
                {
                    MessageBox.Show("Invalid process number format. Please enter a valid integer number.", 
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                if (db.ProcessExists(procNumberInt)) {
                    MessageBox.Show("Process with this Number already exists.");
                    return;
                } 

                success = db.AddProcess(processNumber, fullName, bi, sex, local, funeralDate, relationship, clientName, coffinId, urnId, churchId, priestBi, funeralType, nationality, address, maritalStatus, birthDate, clientId, numFunc);
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
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        pictureBox1.Image = new Bitmap(dlg.FileName);
                        pictureBox1.Visible = true;
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
