using funeraria.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace funeraria.Forms
{
    public partial class MainForm : Form
    {
        LoginForm loginForm;
        private DataTable allInventoryData;

        public MainForm(LoginForm loginForm)
        {

            this.loginForm = loginForm;
            InitializeComponent();

            Database db = Database.GetDatabase();
            DataRow userRow = db.GetUserbyId(loginForm.getUserId());
            string username = userRow["username"].ToString();
            Username.Text = "Welcome back, " + username;

            checkedListBox1.Items.Clear();
            checkedListBox1.Items.Add("Coffin");
            checkedListBox1.Items.Add("Urn");
            checkedListBox1.Items.Add("Flowers");

            checkedListBox1.ItemCheck += (s, e) =>
            {
                this.BeginInvoke((MethodInvoker)delegate {
                    ApplyInventoryFilter();
                });
            };

        }

        public void ShowUser()
        {
            Database db = Database.GetDatabase();

            DataRow userInfo = db.GetUserbyId(loginForm.getUserId());
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
                    pictureUser.Image = Image.FromStream(ms);
                }
            }
            else
            {
                pictureUser.Image = global::funeraria.Properties.Resources.user_male;
            }
        }


        //
        // Button Clicks
        //
        private void pictureMainTab_Click(object sender, EventArgs e)
        {
            sectionsTabs.SelectTab(0);
        }
        private void btnProfile_Click(object sender, EventArgs e)
        {
            sectionsTabs.SelectTab(1);
            ShowUser();
        }
        private void btnProcess_Click(object sender, EventArgs e)
        {
            sectionsTabs.SelectTab(2);
            loadProcess();
            PersonalizarGridProcess();
        }
        private void btnDB_Click(object sender, EventArgs e)
        {
            sectionsTabs.SelectTab(3);
        }
        private void btnInventory_Click(object sender, EventArgs e)
        {
            sectionsTabs.SelectTab(4);
            LoadInventory();
            PersonalizarGridInventory();
        }
        private void btnCrematory_Click(object sender, EventArgs e)
        {
            sectionsTabs.SelectTab(5);
            LoadCrematory();
            PersonalizarGridCrematory();
        }
        private void btnPriest_Click(object sender, EventArgs e)
        {
            sectionsTabs.SelectTab(8);
            LoadPriest();
            PersonalizarGridPriest();
        }
        private void btnChurch_Click(object sender, EventArgs e)
        {
            sectionsTabs.SelectTab(7);
            LoadChurch();
            PersonalizarGridChurch();
        }
        private void btnCemetery_Click(object sender, EventArgs e)
        {
            sectionsTabs.SelectTab(6);
            LoadCemetery();
            PersonalizarGridCemetery();
        }
        private void btnFlorist_Click(object sender, EventArgs e)
        {
            sectionsTabs.SelectTab(9);
            LoadFlorist();
            PersonalizarGridFlorist();
        }

        //
        // Load Functions
        //
        public void loadProcess()
        {
            Database db = Database.GetDatabase();
            DataTable dt = db.GetAllProcessList();

            dataGridProcess.Dock = DockStyle.Fill;
            dataGridProcess.RowHeadersVisible = false;
            dataGridProcess.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridProcess.AutoGenerateColumns = false;
            dataGridProcess.Columns.Clear();

            // Info Page
            DataGridViewImageColumn moreInfo = new DataGridViewImageColumn();
            moreInfo.Name = "Info";
            moreInfo.HeaderText = "INFO";
            moreInfo.Image = global::funeraria.Properties.Resources.document_edit;
            moreInfo.ImageLayout = DataGridViewImageCellLayout.Zoom;
            moreInfo.Width = 50;
            dataGridProcess.Columns.Add(moreInfo);

            // Info Page
            DataGridViewImageColumn picture = new DataGridViewImageColumn();
            picture.Name = "picture";
            picture.HeaderText = "picture";
            picture.ImageLayout = DataGridViewImageCellLayout.Zoom;
            picture.Width = 50;
            dataGridProcess.Columns.Add(picture);


            dataGridProcess.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "num_process",
                HeaderText = "PROCESS NUMBER",
                Name = "ProcessNum"
            });

            dataGridProcess.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DeceasedName",
                HeaderText = "NAME"
            });

            dataGridProcess.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FuneralDate",
                HeaderText = "FUNERAL DATE"
            });

            dataGridProcess.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "local",
                HeaderText = "Local"
            });

            dataGridProcess.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Budget",
                HeaderText = "FUNERAL COST (€)"
            });

            dataGridProcess.DataSource = dt;

            foreach (DataGridViewRow row in dataGridProcess.Rows)
            {
                decimal processId = Convert.ToDecimal(row.Cells["ProcessNum"].Value);

                string deceasedName = db.GetDeceasedNameByProcessId(processId);
                DateTime funeralDate = db.GetFuneralDateByProcessId(processId);
                string localDeath = db.GetLocalDeathByProcessId(processId);
                byte[] imageData = db.GetDeceasedImageByProcessId(Convert.ToInt32(processId));
                
                if (imageData != null)
                {
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(imageData))
                    {
                        row.Cells["picture"].Value = Image.FromStream(ms);
                    }
                }
                else
                {
                    row.Cells["picture"].Value = global::funeraria.Properties.Resources.user_male;
                }

                row.Cells["local"].Value = localDeath;
                row.Cells["DeceasedName"].Value = deceasedName;
                row.Cells["FuneralDate"].Value = funeralDate.ToString("dd/MM/yyyy");
            }

            dataGridProcess.CellClick -= dataGridProcess_CellClick;
            dataGridProcess.CellClick += dataGridProcess_CellClick;
        }
        private void LoadCrematory()
        {
            Database db = Database.GetDatabase();
            DataTable dt = db.GetAllCrematoryList();

            gridCrematory.Dock = DockStyle.Fill;
            gridCrematory.RowHeadersVisible = false;
            gridCrematory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridCrematory.AutoGenerateColumns = false;
            gridCrematory.Columns.Clear();


            gridCrematory.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "id",
                HeaderText = "CREMATORY ID",
                Name = "CrematoryId"
            });

            gridCrematory.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "location",
                HeaderText = "LOCATION"
            });

            gridCrematory.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "contact",
                HeaderText = "CONTACT"
            });

            gridCrematory.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "price",
                HeaderText = "PRICE"
            });

            DataGridViewImageColumn editColumn = new DataGridViewImageColumn();
            editColumn.Name = "Edit";
            editColumn.HeaderText = "EDIT";
            editColumn.Image = global::funeraria.Properties.Resources.document_edit;
            editColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            editColumn.Width = 50;
            gridCrematory.Columns.Add(editColumn);

            DataGridViewImageColumn deleteColumn = new DataGridViewImageColumn();
            deleteColumn.Name = "Delete";
            deleteColumn.HeaderText = "DELETE";
            deleteColumn.Image = global::funeraria.Properties.Resources.user_trash_full;
            deleteColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            editColumn.Width = 60;
            gridCrematory.Columns.Add(deleteColumn);

            gridCrematory.DataSource = dt;

            gridCrematory.CellClick -= gridCrematory_CellClick;
            gridCrematory.CellClick += gridCrematory_CellClick;
        }
        private void LoadChurch()
        {
            Database db = Database.GetDatabase();
            DataTable dt = db.GetAllChurcList();

            dataGridChurch.Dock = DockStyle.Fill;
            dataGridChurch.RowHeadersVisible = false;
            dataGridChurch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridChurch.AutoGenerateColumns = false;
            dataGridChurch.Columns.Clear();


            dataGridChurch.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "id",
                HeaderText = "ID",
                Name = "Id"
            });

            dataGridChurch.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "name",
                HeaderText = "NAME"
            });

            dataGridChurch.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "location",
                HeaderText = "LOCATION"
            });

            DataGridViewImageColumn editColumn = new DataGridViewImageColumn();
            editColumn.Name = "EditChurch";
            editColumn.HeaderText = "EDIT";
            editColumn.Image = global::funeraria.Properties.Resources.document_edit;
            editColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            dataGridChurch.Columns.Add(editColumn);

            DataGridViewImageColumn deleteColumn = new DataGridViewImageColumn();
            deleteColumn.Name = "DeleteChurch";
            deleteColumn.HeaderText = "DELETE";
            deleteColumn.Image = global::funeraria.Properties.Resources.user_trash_full;
            deleteColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            dataGridChurch.Columns.Add(deleteColumn);

            dataGridChurch.DataSource = dt;

            dataGridChurch.CellClick -= gridChurch_CellClick;
            dataGridChurch.CellClick += gridChurch_CellClick;
        }
        private void LoadCemetery()
        {
            Database db = Database.GetDatabase();
            DataTable dt = db.GetAllCemeteryList();

            gridCemetery.Dock = DockStyle.Fill;
            gridCemetery.RowHeadersVisible = false;
            gridCemetery.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridCemetery.AutoGenerateColumns = false;
            gridCemetery.Columns.Clear();


            gridCemetery.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "id",
                HeaderText = "ID",
                Name = "CemeteryId"
            });

            gridCemetery.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "location",
                HeaderText = "LOCATION"
            });

            gridCemetery.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "contact",
                HeaderText = "CONTACT"
            });

            gridCemetery.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "price",
                HeaderText = "PRICE"
            });

            DataGridViewImageColumn editColumn = new DataGridViewImageColumn();
            editColumn.Name = "EditCemetery";
            editColumn.HeaderText = "EDIT";
            editColumn.Image = global::funeraria.Properties.Resources.document_edit;
            editColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            editColumn.Width = 50;
            gridCemetery.Columns.Add(editColumn);

            DataGridViewImageColumn deleteColumn = new DataGridViewImageColumn();
            deleteColumn.Name = "DeleteCemetery";
            deleteColumn.HeaderText = "DELETE";
            deleteColumn.Image = global::funeraria.Properties.Resources.user_trash_full;
            deleteColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            editColumn.Width = 60;
            gridCemetery.Columns.Add(deleteColumn);

            gridCemetery.DataSource = dt;

            gridCemetery.CellClick -= gridCemetery_CellClick;
            gridCemetery.CellClick += gridCemetery_CellClick;
        }
        private void LoadPriest()
        {
            Database db = Database.GetDatabase();
            DataTable dt = db.GetAllPriestList();

            gridPriest.Dock = DockStyle.Fill;
            gridPriest.RowHeadersVisible = false;
            gridPriest.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridPriest.AutoGenerateColumns = false;
            gridPriest.Columns.Clear();


            gridPriest.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "title",
                HeaderText = "TITLE",
                Name = "TitlePriest"
            });

            gridPriest.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "name",
                HeaderText = "NAME",
                Name = "NamePriest"
            });

            gridPriest.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "representative_bi",
                HeaderText = "BI",
                Name = "bi"
            });

            gridPriest.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "contact",
                HeaderText = "CONTACT",
                Name = "ContactPriest"
            });

            gridPriest.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "price",
                HeaderText = "PRICE"
            });

            DataGridViewImageColumn editColumn = new DataGridViewImageColumn();
            editColumn.Name = "EditPriest";
            editColumn.HeaderText = "EDIT";
            editColumn.Image = global::funeraria.Properties.Resources.document_edit;
            editColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            editColumn.Width = 50;
            gridPriest.Columns.Add(editColumn);

            DataGridViewImageColumn deleteColumn = new DataGridViewImageColumn();
            deleteColumn.Name = "DeletePriest";
            deleteColumn.HeaderText = "DELETE";
            deleteColumn.Image = global::funeraria.Properties.Resources.user_trash_full;
            deleteColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            editColumn.Width = 60;
            gridPriest.Columns.Add(deleteColumn);

            gridPriest.DataSource = dt;

            foreach (DataGridViewRow row in gridPriest.Rows)
            {
                string bi = Convert.ToString(row.Cells["bi"].Value);

                string priestName = db.GetPriestNameByPriestBi(bi);
                decimal contactPriest = db.GetPriestContactByPriestBi(bi);

                row.Cells["NamePriest"].Value = priestName;
                row.Cells["ContactPriest"].Value = contactPriest;
            }

            gridPriest.CellClick -= gridPriest_CellClick;
            gridPriest.CellClick += gridPriest_CellClick;
        }
        private void LoadFlorist()
        {
            Database db = Database.GetDatabase();
            DataTable dt = db.GetAllFloristList();

            gridFlorist.Dock = DockStyle.Fill;
            gridFlorist.RowHeadersVisible = false;
            gridFlorist.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridFlorist.AutoGenerateColumns = false;
            gridFlorist.Columns.Clear();


            gridFlorist.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "nif",
                HeaderText = "NIF",
                Name = "nif"
            });

            gridFlorist.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "name",
                HeaderText = "NAME",
                Name = "nameFlorist"
            });

            gridFlorist.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "contact",
                HeaderText = "CONTACT",
                Name = "ContactPriest"
            });

            gridFlorist.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "address",
                HeaderText = "ADDRESS"
            });

            DataGridViewImageColumn editColumn = new DataGridViewImageColumn();
            editColumn.Name = "EditFlorist";
            editColumn.HeaderText = "EDIT";
            editColumn.Image = global::funeraria.Properties.Resources.document_edit;
            editColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            editColumn.Width = 50;
            gridFlorist.Columns.Add(editColumn);

            DataGridViewImageColumn deleteColumn = new DataGridViewImageColumn();
            deleteColumn.Name = "DeleteFlorist";
            deleteColumn.HeaderText = "DELETE";
            deleteColumn.Image = global::funeraria.Properties.Resources.user_trash_full;
            deleteColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            editColumn.Width = 60;
            gridFlorist.Columns.Add(deleteColumn);

            gridFlorist.DataSource = dt;

            gridFlorist.CellClick -= gridFlorist_CellClick;
            gridFlorist.CellClick += gridFlorist_CellClick;
        }
        private void LoadInventory()
        {
            Database db = Database.GetDatabase();
            allInventoryData = db.GetAllProductId();

            if (!allInventoryData.Columns.Contains("type"))
                allInventoryData.Columns.Add("type", typeof(string));

            foreach (DataRow row in allInventoryData.Rows)
            {
                decimal productId = Convert.ToDecimal(row["id"]);
                string type = db.GetProductTypeById(productId);
                row["type"] = type;
            }

            ApplyInventoryFilter();
        }


        //
        // DataGridView Cell Clicks
        //
        private void dataGridProcess_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dataGridProcess.Columns[e.ColumnIndex].Name == "Info")
            {
                int id = Convert.ToInt32(dataGridProcess.Rows[e.RowIndex].Cells["ProcessNum"].Value);
                InfoProcessForm infoForm = new InfoProcessForm(id, this.loginForm.getUserId());
                infoForm.FormClosed += (s, args) => loadProcess();
                infoForm.Show();
            }
        }
        private void gridCemetery_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // Ignorar o header

            if (gridCemetery.Columns[e.ColumnIndex].Name == "EditCemetery")
            {
                int id = Convert.ToInt32(gridCemetery.Rows[e.RowIndex].Cells["CemeteryId"].Value);
                CemeteryForm cemeteryForm = new CemeteryForm(id);
                cemeteryForm.Show();
                cemeteryForm.FormClosed += (s, args) => LoadCemetery();
            }
            else if (gridCemetery.Columns[e.ColumnIndex].Name == "DeleteCemetery")
            {
                int id = Convert.ToInt32(gridCemetery.Rows[e.RowIndex].Cells["CemeteryId"].Value);
                var result = MessageBox.Show("Tem a certeza que deseja eliminar este registo?", "Confirmar", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Database.DeleteCemetery(id);
                    LoadCemetery();
                }
            }
        }
        private void gridCrematory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (gridCrematory.Columns[e.ColumnIndex].Name == "Edit")
            {
                int id = Convert.ToInt32(gridCrematory.Rows[e.RowIndex].Cells["CrematoryId"].Value);
                CrematoryForm crematoryForm = new CrematoryForm(id);
                crematoryForm.Show();
                crematoryForm.FormClosed += (s, args) => LoadCrematory();
            }
            else if (gridCrematory.Columns[e.ColumnIndex].Name == "Delete")
            {
                int id = Convert.ToInt32(gridCrematory.Rows[e.RowIndex].Cells["CrematoryId"].Value);
                var result = MessageBox.Show("Tem a certeza que deseja eliminar este registo?", "Confirmar", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Database.DeleteCrematory(id);
                    LoadCrematory();
                }
            }
        }
        private void gridPriest_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // Ignorar o header

            if (gridPriest.Columns[e.ColumnIndex].Name == "EditPriest")
            {
                string bi = Convert.ToString(gridPriest.Rows[e.RowIndex].Cells["bi"].Value);
                PriestForm priestForm = new PriestForm(bi);
                priestForm.Show();
                priestForm.FormClosed += (s, args) => LoadPriest();
            }
            else if (gridPriest.Columns[e.ColumnIndex].Name == "DeletePriest")
            {
                String bi = Convert.ToString(gridPriest.Rows[e.RowIndex].Cells["bi"].Value);
                var result = MessageBox.Show("Tem a certeza que deseja eliminar este registo?", "Confirmar", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Database.DeletePriest(bi);
                    LoadPriest();
                }
            }
        }
        private void gridChurch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // Ignorar o header

            if (dataGridChurch.Columns[e.ColumnIndex].Name == "EditChurch")
            {
                int id = Convert.ToInt32(dataGridChurch.Rows[e.RowIndex].Cells["Id"].Value);
                ChurchForm churchForm = new ChurchForm(id);
                churchForm.Show();
                churchForm.FormClosed += (s, args) => LoadChurch();
            }
            else if (dataGridChurch.Columns[e.ColumnIndex].Name == "DeleteChurch")
            {
                int id = Convert.ToInt32(dataGridChurch.Rows[e.RowIndex].Cells["Id"].Value);
                var result = MessageBox.Show("Tem a certeza que deseja eliminar este registo?", "Confirmar", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Database.DeleteChurch(id);
                    LoadChurch();
                }
            }
        }
        private void gridFlorist_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (gridFlorist.Columns[e.ColumnIndex].Name == "EditFlorist")
            {
                int nif = Convert.ToInt32(gridFlorist.Rows[e.RowIndex].Cells["nif"].Value);
                FloristForm floristForm = new FloristForm(nif);
                floristForm.Show();
                floristForm.FormClosed += (s, args) => LoadFlorist();
            }
            else if (gridFlorist.Columns[e.ColumnIndex].Name == "DeleteFlorist")
            {
                int nif = Convert.ToInt32(gridFlorist.Rows[e.RowIndex].Cells["nif"].Value);
                var result = MessageBox.Show("Tem a certeza que deseja eliminar este registo?", "Confirmar", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Database.DeleteFlorist(nif);
                    LoadFlorist();
                }
            }
        }

        private void gridInventory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // Ignore header clicks
            
            if (gridInventory.Columns[e.ColumnIndex].Name == "shop")
            {
                int productId = Convert.ToInt32(gridInventory.Rows[e.RowIndex].Cells["idProduct"].Value);
                ShopForm shopForm = new ShopForm(productId);
                
                if (shopForm.ShowDialog() == DialogResult.OK)
                {
                    // Reload inventory data after purchase
                    LoadInventory();
                }
            }
            else if (gridInventory.Columns[e.ColumnIndex].Name == "detailsProduct")
            {
                int productId = Convert.ToInt32(gridInventory.Rows[e.RowIndex].Cells["idProduct"].Value);
                
                // Open the inventory info form
                InventoryInfoForm infoForm = new InventoryInfoForm(productId);
                infoForm.ShowDialog();
            }
        }



        //
        // Personalizar Grids
        //
        private void PersonalizarGridCrematory()
        {
            gridCrematory.RowHeadersVisible = false;
            gridCrematory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridCrematory.RowTemplate.Height = 60;
            foreach (DataGridViewRow row in gridCrematory.Rows)
            {
                row.Height = 60;
            }

            gridCrematory.EnableHeadersVisualStyles = false;
            gridCrematory.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            gridCrematory.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            gridCrematory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            gridCrematory.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            gridCrematory.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            gridCrematory.DefaultCellStyle.BackColor = Color.White;
            gridCrematory.DefaultCellStyle.ForeColor = Color.Black;
            gridCrematory.DefaultCellStyle.SelectionBackColor = Color.LightGray;
            gridCrematory.DefaultCellStyle.SelectionForeColor = Color.Black;

            gridCrematory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

            gridCrematory.BorderStyle = BorderStyle.None;
            gridCrematory.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gridCrematory.GridColor = Color.LightGray;
        }
        private void PersonalizarGridProcess()
        {
            dataGridProcess.RowHeadersVisible = false;
            dataGridProcess.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridProcess.RowTemplate.Height = 60;
            foreach (DataGridViewRow row in dataGridProcess.Rows)
            {
                row.Height = 60;
            }

            dataGridProcess.Columns["Info"].Width = 45;

            dataGridProcess.EnableHeadersVisualStyles = false;
            dataGridProcess.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dataGridProcess.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dataGridProcess.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dataGridProcess.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            dataGridProcess.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            dataGridProcess.DefaultCellStyle.BackColor = Color.White;
            dataGridProcess.DefaultCellStyle.ForeColor = Color.Black;
            dataGridProcess.DefaultCellStyle.SelectionBackColor = Color.LightGray;
            dataGridProcess.DefaultCellStyle.SelectionForeColor = Color.Black;

            dataGridProcess.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

            dataGridProcess.BorderStyle = BorderStyle.None;
            dataGridProcess.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridProcess.GridColor = Color.LightGray;
        }
        private void PersonalizarGridPriest()
        {
            gridPriest.RowHeadersVisible = false;
            gridPriest.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridPriest.RowTemplate.Height = 60;
            foreach (DataGridViewRow row in gridPriest.Rows)
            {
                row.Height = 60;
            }

            gridPriest.EnableHeadersVisualStyles = false;
            gridPriest.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            gridPriest.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            gridPriest.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            gridPriest.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            gridPriest.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            gridPriest.DefaultCellStyle.BackColor = Color.White;
            gridPriest.DefaultCellStyle.ForeColor = Color.Black;
            gridPriest.DefaultCellStyle.SelectionBackColor = Color.LightGray;
            gridPriest.DefaultCellStyle.SelectionForeColor = Color.Black;

            gridPriest.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

            gridPriest.BorderStyle = BorderStyle.None;
            gridPriest.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gridPriest.GridColor = Color.LightGray;
        }
        private void PersonalizarGridChurch()
        {
            dataGridChurch.RowHeadersVisible = false;
            dataGridChurch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridChurch.RowTemplate.Height = 60;
            foreach (DataGridViewRow row in dataGridChurch.Rows)
            {
                row.Height = 60;
            }

            dataGridChurch.Columns["EditChurch"].Width = 45;
            dataGridChurch.Columns["Id"].Width = 45;
            dataGridChurch.Columns["DeleteChurch"].Width = 60;

            dataGridChurch.EnableHeadersVisualStyles = false;
            dataGridChurch.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dataGridChurch.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dataGridChurch.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dataGridChurch.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            dataGridChurch.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            dataGridChurch.DefaultCellStyle.BackColor = Color.White;
            dataGridChurch.DefaultCellStyle.ForeColor = Color.Black;
            dataGridChurch.DefaultCellStyle.SelectionBackColor = Color.LightGray;
            dataGridChurch.DefaultCellStyle.SelectionForeColor = Color.Black;

            dataGridChurch.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

            dataGridChurch.BorderStyle = BorderStyle.None;
            dataGridChurch.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridChurch.GridColor = Color.LightGray;
        }
        private void PersonalizarGridCemetery()
        {
            gridCemetery.RowHeadersVisible = false;
            gridCemetery.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridCemetery.RowTemplate.Height = 60;
            foreach (DataGridViewRow row in gridCemetery.Rows)
            {
                row.Height = 60;
            }

            gridCemetery.Columns["EditCemetery"].Width = 45;
            gridCemetery.Columns["CemeteryId"].Width = 45;
            gridCemetery.Columns["DeleteCemetery"].Width = 60;

            gridCemetery.EnableHeadersVisualStyles = false;
            gridCemetery.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            gridCemetery.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            gridCemetery.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            gridCemetery.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            gridCemetery.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            gridCemetery.DefaultCellStyle.BackColor = Color.White;
            gridCemetery.DefaultCellStyle.ForeColor = Color.Black;
            gridCemetery.DefaultCellStyle.SelectionBackColor = Color.LightGray;
            gridCemetery.DefaultCellStyle.SelectionForeColor = Color.Black;

            gridCemetery.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

            gridCemetery.BorderStyle = BorderStyle.None;
            gridCemetery.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gridCemetery.GridColor = Color.LightGray;
        }
        private void PersonalizarGridFlorist()
        {
            gridFlorist.RowHeadersVisible = false;
            gridFlorist.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridFlorist.RowTemplate.Height = 60;
            foreach (DataGridViewRow row in gridFlorist.Rows)
            {
                row.Height = 60;
            }

            gridFlorist.Columns["EditFlorist"].Width = 45;
            gridFlorist.Columns["DeleteFlorist"].Width = 60;

            gridFlorist.EnableHeadersVisualStyles = false;
            gridFlorist.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            gridFlorist.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            gridFlorist.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            gridFlorist.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            gridFlorist.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            gridFlorist.DefaultCellStyle.BackColor = Color.White;
            gridFlorist.DefaultCellStyle.ForeColor = Color.Black;
            gridFlorist.DefaultCellStyle.SelectionBackColor = Color.LightGray;
            gridFlorist.DefaultCellStyle.SelectionForeColor = Color.Black;

            gridFlorist.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

            gridFlorist.BorderStyle = BorderStyle.None;
            gridFlorist.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gridFlorist.GridColor = Color.LightGray;
        }
        private void PersonalizarGridInventory()
        {
            gridInventory.RowHeadersVisible = false;
            gridInventory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridInventory.RowTemplate.Height = 60;
            foreach (DataGridViewRow row in gridInventory.Rows)
            {
                row.Height = 60;
            }

            gridInventory.EnableHeadersVisualStyles = false;
            gridInventory.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            gridInventory.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            gridInventory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            gridInventory.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            gridInventory.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            gridInventory.DefaultCellStyle.BackColor = Color.White;
            gridInventory.DefaultCellStyle.ForeColor = Color.Black;
            gridInventory.DefaultCellStyle.SelectionBackColor = Color.LightGray;
            gridInventory.DefaultCellStyle.SelectionForeColor = Color.Black;

            gridInventory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

            gridInventory.BorderStyle = BorderStyle.None;
            gridInventory.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gridInventory.GridColor = Color.LightGray;
        }

        //
        // Back Buttons
        //
        private void pictureBackChurch_Click(object sender, EventArgs e)
        {
            sectionsTabs.SelectTab(3);
        }
        private void pictureBackCemetery_Click(object sender, EventArgs e)
        {
            sectionsTabs.SelectTab(3);
        }
        private void pictureBack_Click(object sender, EventArgs e)
        {
            sectionsTabs.SelectTab(3);
        }
        private void pictureBackPriest_Click(object sender, EventArgs e)
        {
            sectionsTabs.SelectTab(3);
        }
        private void pictureBackFlorist_Click(object sender, EventArgs e)
        {
            sectionsTabs.SelectTab(3);
        }

        //
        // New Buttons
        //
        private void newCrematory_Click(object sender, EventArgs e)
        {
            CrematoryForm crematoryForm = new CrematoryForm();
            crematoryForm.FormClosed += (s, args) => LoadCrematory();
            crematoryForm.Show();
        }
        private void newChurch_Click(object sender, EventArgs e)
        {
            ChurchForm churchForm = new ChurchForm();
            churchForm.FormClosed += (s, args) => LoadChurch();
            churchForm.Show();
        }
        private void newCemetery_Click(object sender, EventArgs e)
        {
            CemeteryForm cemeteryForm = new CemeteryForm();
            cemeteryForm.FormClosed += (s, args) => LoadCemetery();
            cemeteryForm.Show();
        }
        private void newPriest_Click(object sender, EventArgs e)
        {
            PriestForm priestForm = new PriestForm();
            priestForm.FormClosed += (s, args) => LoadPriest();
            priestForm.Show();
        }
        private void newFlorist_Click(object sender, EventArgs e)
        {
            FloristForm floristForm = new FloristForm();
            floristForm.FormClosed += (s, args) => LoadFlorist();
            floristForm.Show();
        }
        private void addNewProcess_Click(object sender, EventArgs e)
        {
            InfoProcessForm infoProcessForm = new InfoProcessForm(this.loginForm.getUserId());
            infoProcessForm.FormClosed += (s, args) => loadProcess();
            infoProcessForm.Show();
        }


        private void logOutPicture_Click(object sender, EventArgs e)
        {
            this.Close();
            loginForm.clear();
            loginForm.Show();
            loginForm.BringToFront();
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            Database.DeleteAccount(loginForm.getUserId());
            logOutPicture_Click(sender, e);
        }

        private void ApplyInventoryFilter()
        {
            if (allInventoryData == null) return;

            var tiposSelecionados = checkedListBox1.CheckedItems
                .Cast<string>()
                .ToList();

            DataTable filteredData;

            if (tiposSelecionados.Count == 0)
            {
                filteredData = allInventoryData.Copy();
            }
            else
            {
                string filterExpression = string.Join("','", tiposSelecionados);
                DataRow[] filteredRows = allInventoryData.Select($"type IN ('{filterExpression}')");

                filteredData = allInventoryData.Clone();
                foreach (DataRow row in filteredRows)
                    filteredData.ImportRow(row);
            }

            // Atualiza o grid
            gridInventory.DataSource = null;
            gridInventory.Columns.Clear();

            gridInventory.Dock = DockStyle.Fill;
            gridInventory.RowHeadersVisible = false;
            gridInventory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridInventory.AutoGenerateColumns = false;

            gridInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "id",
                HeaderText = "ID",
                Name = "idProduct"
            });

            gridInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "type",
                HeaderText = "TYPE",
                Name = "type"
            });

            gridInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "stock",
                HeaderText = "STOCK",
                Name = "stock"
            });

            DataGridViewImageColumn shopColumn = new DataGridViewImageColumn
            {
                Name = "shop",
                HeaderText = "SHOP",
                Image = global::funeraria.Properties.Resources.produtos,
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                Width = 50
            };
            gridInventory.Columns.Add(shopColumn);

            DataGridViewImageColumn detailsColumn = new DataGridViewImageColumn
            {
                Name = "detailsProduct",
                HeaderText = "DETAILS",
                Image = global::funeraria.Properties.Resources.document7,
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                Width = 60
            };
            gridInventory.Columns.Add(detailsColumn);

            gridInventory.DataSource = filteredData;

            gridInventory.CellClick -= gridInventory_CellClick;
            gridInventory.CellClick += gridInventory_CellClick;
        }

        private void btnProfileEdit_Click(object sender, EventArgs e)
        {
            EditProfileForm editProfileForm = new EditProfileForm(loginForm.getUserId());
            editProfileForm.FormClosed += (s, args) => ShowUser();
            editProfileForm.Show();
        }
    }
}
