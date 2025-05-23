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

            // Hide save button when viewing existing process
            SaveButtonProcess.Visible = false;

            // Make all fields read-only for view mode
            SetAllFieldsReadOnly(true);
            buttonChangeIcon.Visible = false;

            // Funeral Type ComboBox event handler
            comboFuneralTypeBox.SelectedIndexChanged += ComboFuneralTypeBox_SelectedIndexChanged;

            // Make sure the trash icon click event is properly hooked up
            if (PictureTrashClick != null)
                PictureTrashClick.Click -= PictureTrashClick_Click;
                PictureTrashClick.Click += PictureTrashClick_Click;
        }

        private void SetAllFieldsReadOnly(bool readOnly)
        {
            // Set all textboxes to read-only
            textFuncNameBox.ReadOnly = readOnly;
            textProccessBox.ReadOnly = readOnly;
            textIDNumberBox.ReadOnly = readOnly;
            textFullNameBox.ReadOnly = readOnly;
            textSexBox.ReadOnly = readOnly;
            textMaritalStatusBox.ReadOnly = readOnly;
            textAddressBox.ReadOnly = readOnly;
            textNationalityBox.ReadOnly = readOnly;
            textBirthDateBox.ReadOnly = readOnly;
            textLocalBox.ReadOnly = readOnly;
            textFuneralDateBox.ReadOnly = readOnly;
            textGraveNumberBox.ReadOnly = readOnly;
            textClientNameBox.ReadOnly = readOnly;
            textRelationshipBox.ReadOnly = readOnly;
            textClientIDBox.ReadOnly = readOnly;

            // Disable comboboxes
            comboFuneralTypeBox.Enabled = !readOnly;
            comboPriestBox.Enabled = !readOnly;
            comboCerimonyPlaceBox.Enabled = !readOnly;
            comboUrnBox.Enabled = !readOnly;
            comboCoffinBox.Enabled = !readOnly;
            
            // Cemetery and crematory combo boxes
            comboCemeteryBox.Enabled = !readOnly;
            comboCrematoryBox.Enabled = !readOnly;
            
            // Florist and flower combo boxes (should always be accessible)
            comboFloristBox.Enabled = !readOnly;
            comboFlowerBox.Enabled = !readOnly;
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
                this.ActiveControl = null;
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

            // Load cemetery
            LoadCemeteries();

            // Load crematories
            LoadCrematories();

            // Load flowers
            LoadFlowers();

            // Load florists
            LoadFlorists();
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

        private void LoadCemeteries()
        {
            Database db = Database.GetDatabase();
            comboCemeteryBox.Items.Clear();

            DataTable cemeteries = db.GetAllCemeteryList();
            foreach (DataRow row in cemeteries.Rows)
            {
                string cemeteryId = row["id"].ToString();
                string cemeteryLocation = row["location"].ToString();
                string cemeteryContact = row["contact"].ToString();
                decimal cemeteryPrice = Convert.ToDecimal(row["price"]);

                // Create a custom item class to store both display text and ID
                ComboboxItem item = new ComboboxItem
                {
                    Text = $"{cemeteryLocation} - ${cemeteryPrice} (ID: {cemeteryId})",
                    Value = cemeteryId
                };

                comboCemeteryBox.Items.Add(item);
            }
        }

        private void LoadCrematories()
        {
            Database db = Database.GetDatabase();
            comboCrematoryBox.Items.Clear();

            DataTable crematories = db.GetAllCrematoryList();
            foreach (DataRow row in crematories.Rows)
            {
                string crematoryId = row["id"].ToString();
                string crematoryLocation = row["location"].ToString();
                string crematoryContact = row["contact"].ToString();
                decimal crematoryPrice = Convert.ToDecimal(row["price"]);

                // Create a custom item class to store both display text and ID
                ComboboxItem item = new ComboboxItem
                {
                    Text = $"{crematoryLocation} - ${crematoryPrice} (ID: {crematoryId})",
                    Value = crematoryId
                };

                comboCrematoryBox.Items.Add(item);
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

                int userId = Convert.ToInt32(row["user_id"]);
                textFuncNameBox.Text = db.GetUserbyId(userId)["name"].ToString();

                if (row["deceased_bi"] != DBNull.Value)
                {
                    if (row.Table.Columns.Contains("picture") && row["picture"] != DBNull.Value)
                    {
                        byte[] img = (byte[])row["picture"];
                        using (MemoryStream ms = new MemoryStream(img))
                        {
                            PicDeceased.Image = Image.FromStream(ms);
                        }
                    }
                    else
                    {
                        PicDeceased.Image = global::funeraria.Properties.Resources.user_male;
                    }
                }

                string funeralType = row["funeral_type"].ToString();
                comboFuneralTypeBox.SelectedItem = funeralType;

                // Select cemetery if it's a burial
                 if (funeralType == "Burial" && row["cemetery_id"] != DBNull.Value)
                {
                    int cemeteryId = Convert.ToInt32(row["cemetery_id"]);
                    for (int i = 0; i < comboCemeteryBox.Items.Count; i++)
                    {
                        ComboboxItem item = comboCemeteryBox.Items[i] as ComboboxItem;
                        if (item != null && item.Value == cemeteryId.ToString())
                        {
                            comboCemeteryBox.SelectedIndex = i;
                            break;
                        }
                    }

                    if (row["num_grave"] != DBNull.Value)
                        textGraveNumberBox.Text = row["num_grave"].ToString();

                    comboCrematoryBox.Enabled = false;
                    comboCrematoryBox.SelectedIndex = -1;
                    comboCrematoryBox.Text = "Not applicable for burial";

                    comboUrnBox.Enabled = false;
                    comboUrnBox.SelectedIndex = -1;
                    comboUrnBox.Text = "Not applicable for burial";
                }
                else if (funeralType == "Cremation" && row["crematory_id"] != DBNull.Value)
                {
                    int crematoryId = Convert.ToInt32(row["crematory_id"]);
                    for (int i = 0; i < comboCrematoryBox.Items.Count; i++)
                    {
                        ComboboxItem item = comboCrematoryBox.Items[i] as ComboboxItem;
                        if (item != null && item.Value == crematoryId.ToString())
                        {
                            comboCrematoryBox.SelectedIndex = i;
                            break;
                        }
                    }

                    textGraveNumberBox.Enabled = false;
                    textGraveNumberBox.Text = "Not applicable for cremation";

                    comboCemeteryBox.Enabled = false;
                    comboCemeteryBox.SelectedIndex = -1;
                    comboCemeteryBox.Text = "Not applicable for cremation";
                }

                ComboFuneralTypeBox_SelectedIndexChanged(comboFuneralTypeBox, EventArgs.Empty);


                textProccessBox.Text = row["num_process"].ToString();

                string id = row["user_id"].ToString();
                textIDNumberBox.Text = id;

                textFullNameBox.Text = db.GetDeceasedNameByProcessId(processId);
                textSexBox.Text = row["sex"].ToString();
                textMaritalStatusBox.Text = row["marital_status"].ToString();
                textAddressBox.Text = row["residence"].ToString();
                textNationalityBox.Text = row["nationality"].ToString();

                if (row["birth_date"] != DBNull.Value)
                    textBirthDateBox.Text = Convert.ToDateTime(row["birth_date"]).ToString("dd/MM/yyyy");

                textClientNameBox.Text = row["client_name"].ToString();
                textRelationshipBox.Text = row["degree_kinship"].ToString();
                textClientIDBox.Text = row["client_id"].ToString();

                textLocalBox.Text = row["location"].ToString();
                if (row["funeral_date"] != DBNull.Value)
                    textFuneralDateBox.Text = Convert.ToDateTime(row["funeral_date"]).ToString("dd/MM/yyyy");

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

                // Load church - properly compare the Value property of ComboboxItem
                for (int i = 0; i < comboCerimonyPlaceBox.Items.Count; i++)
                {
                    if (comboCerimonyPlaceBox.Items[i] is ComboboxItem item && item.Value == churchId.ToString())
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

        private string GetSelectedCemeteryId()
        {
            if (comboCemeteryBox.SelectedIndex >= 0)
            {
                ComboboxItem selectedItem = comboCemeteryBox.SelectedItem as ComboboxItem;
                return selectedItem?.Value ?? string.Empty;
            }
            return string.Empty;
        }

        private string GetSelectedCrematoryId()
        {
            if (comboCrematoryBox.SelectedIndex >= 0)
            {
                ComboboxItem selectedItem = comboCrematoryBox.SelectedItem as ComboboxItem;
                return selectedItem?.Value ?? string.Empty;
            }
            return string.Empty;
        }




        private void LoadFlorists()
        {
            Database db = Database.GetDatabase();
            comboFloristBox.Items.Clear();  // Make sure we're using the right combobox

            DataTable florists = db.GetAllFloristList();
            foreach (DataRow row in florists.Rows)
            {
                string floristNif = row["nif"].ToString();
                string floristName = row["name"].ToString();
                string floristAddress = row["address"].ToString();
                string floristContact = row["contact"].ToString();

                // Create ComboboxItem to properly store the NIF
                ComboboxItem item = new ComboboxItem
                {
                    Text = $"{floristName} - {floristAddress} (NIF: {floristNif})",
                    Value = floristNif
                };

                comboFloristBox.Items.Add(item);
            }
        }




        private void LoadFlowers()
        {
            Database db = Database.GetDatabase();
            comboFlowerBox.Items.Clear();

            DataTable products = db.GetAllProductId();
            foreach (DataRow row in products.Rows)
            {
                decimal productId = Convert.ToDecimal(row["id"]);
                string type = db.GetProductTypeById(productId);
                
                if (type == "Flower")
                {
                    DataTable flowerDetails = db.GetFlowerDetailsById((int)productId);
                    if (flowerDetails != null && flowerDetails.Rows.Count > 0)
                    {
                        string flowerType = flowerDetails.Rows[0]["type"].ToString();
                        string flowerColor = flowerDetails.Rows[0]["color"].ToString();
                        
                        ComboboxItem item = new ComboboxItem
                        {
                            Text = $"{flowerType} - {flowerColor} (ID: {productId})",
                            Value = productId.ToString()
                        };
                        
                        comboFlowerBox.Items.Add(item);
                    }
                }
            }
        }






        private void ComboFuneralTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get whether the form is in read-only mode
            bool isReadOnly = !SaveButtonProcess.Visible;

            // First set up controls that are always the same regardless of funeral type
            comboFloristBox.Enabled = !isReadOnly;
            comboFlowerBox.Enabled = !isReadOnly;

            // Check which funeral type is selected
            if (comboFuneralTypeBox.SelectedItem?.ToString() == "Burial")
            {
                // Configure for Burial
                comboCemeteryBox.Enabled = !isReadOnly;
                textGraveNumberBox.Enabled = !isReadOnly;
                
                // Disable cremation-specific controls
                comboUrnBox.Enabled = false;
                comboUrnBox.SelectedIndex = -1;
                comboUrnBox.Text = isReadOnly ? "" : "Not applicable for burial";
                
                comboCrematoryBox.Enabled = false;
                comboCrematoryBox.SelectedIndex = -1;
                comboCrematoryBox.Text = isReadOnly ? "" : "Not applicable for burial";
            }
            else if (comboFuneralTypeBox.SelectedItem?.ToString() == "Cremation")
            {
                // Configure for Cremation
                comboUrnBox.Enabled = !isReadOnly;
                comboCrematoryBox.Enabled = !isReadOnly;
                
                // Disable burial-specific controls
                comboCemeteryBox.Enabled = false;
                comboCemeteryBox.SelectedIndex = -1;
                comboCemeteryBox.Text = isReadOnly ? "" : "Not applicable for cremation";
                
                textGraveNumberBox.Enabled = false;
                textGraveNumberBox.Text = isReadOnly ? "" : "Not applicable for cremation";
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
                byte[] img = new byte[0];
                MemoryStream ms = new MemoryStream();

                // Get selected values from combo boxes
                string funeralType = GetSelectedFuneralType();
                string priestBi = GetSelectedPriestBi();

                // Get client and process info
                string clientName = textClientNameBox.Text;
                string clientId = textClientIDBox.Text;
                string relationship = textRelationshipBox.Text;
                string processNumber = textProccessBox.Text;
                int numGrave = 0;

                int crematoryId = 0; // Default to 0 for burial case
                if (funeralType == "Cremation")
                {
                    string crematoryIdStr = GetSelectedCrematoryId();
                    if (!int.TryParse(crematoryIdStr, out crematoryId))
                    {
                        MessageBox.Show("Invalid crematory selection for cremation.");
                        return;
                    }
                }

                int cemeteryId = 0; // Default to 0 for burial case
                if (funeralType == "Burial")
                {
                    numGrave = textGraveNumberBox.Text != "" ? Convert.ToInt32(textGraveNumberBox.Text) : 0;
                    string cemeteryIdStr = GetSelectedCemeteryId();
                    if (!int.TryParse(cemeteryIdStr, out cemeteryId))
                    {
                        MessageBox.Show("Invalid cemetery selection for burial.");
                        return;
                    }
                }




                if (string.IsNullOrWhiteSpace(clientId))
                {
                    MessageBox.Show("Client ID cannot be empty.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (PicDeceased.Image != null)
                {
                    PicDeceased.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    img = ms.ToArray();
                }

                // Fix the typo and convert all IDs to integers in one st   ep
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

                success = db.AddProcess(processNumber, fullName, bi, sex, local, funeralDate, relationship, clientName, coffinId, urnId, churchId, priestBi, funeralType, nationality, address, maritalStatus, birthDate, clientId, numFunc, img, numGrave, crematoryId, cemeteryId);

                MessageBox.Show(success ? "Process added successfully." : "Failed to add Process.");

                db.UpdateProcessBudget();

                if (success)
                {
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
                        PicDeceased.Image = new Bitmap(dlg.FileName);
                        PicDeceased.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void PictureTrashClick_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Tem a certeza que deseja eliminar este processo?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                Database db = Database.GetDatabase();
                bool deleted = db.DeleteProcess(id);
                if (deleted)
                {
                    MessageBox.Show("Processo eliminado com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Não foi possível eliminar o processo.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
