using funeraria.Entities;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace funeraria.Forms
{
    public partial class InventoryInfoForm : Form
    {
        private int productId;

        public InventoryInfoForm(int productId)
        {
            InitializeComponent();
            this.productId = productId;

            // Set up event handlers
            this.Load += InventoryInfoForm_Load;
            this.PictureBackClick.Click += PictureBackClick_Click;

            // Make all TextBoxes read-only
            MakeTextBoxesReadOnly();
        }

        private void MakeTextBoxesReadOnly()
        {
            // Set all TextBoxes to read-only
            textProductNumberIDBox.ReadOnly = true;
            textStockNumberBox.ReadOnly = true;
            textProdPriceBox.ReadOnly = true;
            textProdColorBox.ReadOnly = true;
            textCoffinWeightBox.ReadOnly = true;
            textProdSizeBox.ReadOnly = true;
            txtProductType.ReadOnly = true;
        }

        private void InventoryInfoForm_Load(object sender, EventArgs e)
        {
            LoadProductData();
        }

        private void PictureBackClick_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadProductData()
        {
            Database db = Database.GetDatabase();
            DataTable productInfo = db.GetProductById(productId);

            if (productInfo != null && productInfo.Rows.Count > 0)
            {
                DataRow row = productInfo.Rows[0];

                // Get product type
                string productType = db.GetProductTypeById(productId);

                // Fill in the TextBoxes with data
                textProductNumberIDBox.Text = productId.ToString();
                textStockNumberBox.Text = row["stock"].ToString();

                if (row["price"] != DBNull.Value)
                {
                    textProdPriceBox.Text = Convert.ToDecimal(row["price"]).ToString("C");
                }

                // Additional details based on product type
                LoadDetailsByType(productType);
            }
            else
            {
                MessageBox.Show("Product information not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void LoadDetailsByType(string productType)
        {
            Database db = Database.GetDatabase();

            switch (productType)
            {
                case "Coffin":
                    DataTable coffinDetails = db.GetCoffinDetailsById(productId);
                    if (coffinDetails != null && coffinDetails.Rows.Count > 0)
                    {

                        label5.Visible = true;
                        label6.Visible = true;
                        label7.Visible = true;

                        textProdColorBox.Visible = true;
                        textCoffinWeightBox.Visible = true;
                        textProdSizeBox.Visible = true;

                        label5.Text = "Color:";
                        label6.Text = "Weight:";
                        label7.Text = "Size:";

                        txtProductType.Text = "Coffin";

                        textProdColorBox.Text = coffinDetails.Rows[0]["color"].ToString();
                        textCoffinWeightBox.Text = coffinDetails.Rows[0]["weight"].ToString();
                        textProdSizeBox.Text = coffinDetails.Rows[0]["size"].ToString();
                    }
                    break;

                case "Urn":
                    DataTable urnDetails = db.GetUrnDetailsById(productId);
                    if (urnDetails != null && urnDetails.Rows.Count > 0)
                    {

                        label5.Visible = true;
                        label6.Visible = false;
                        label7.Visible = false;

                        textProdColorBox.Visible = true;
                        textCoffinWeightBox.Visible = false;
                        textProdSizeBox.Visible = false;

                        txtProductType.Text = "Urn";
                        label5.Text = "Size:";

                        textProdColorBox.Text = urnDetails.Rows[0]["size"].ToString();
                    }
                    break;

                case "Flower":
                    DataTable flowerDetails = db.GetFlowerDetailsById(productId);
                    if (flowerDetails != null && flowerDetails.Rows.Count > 0)
                    {

                        label5.Visible = true;
                        label6.Visible = true;
                        label7.Visible = false;

                        textProdColorBox.Visible = true;
                        textCoffinWeightBox.Visible = true;
                        textProdSizeBox.Visible = false;

                        txtProductType.Text = "Flowers";
                        label5.Text = "Flower Type:";
                        label6.Text = "Color:";

                        textProdColorBox.Text = flowerDetails.Rows[0]["type"].ToString();
                        textCoffinWeightBox.Text = flowerDetails.Rows[0]["color"].ToString();
                    }
                    break;
            }
        }
    }
}
