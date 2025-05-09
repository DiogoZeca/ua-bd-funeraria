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
        }

        private void InventoryInfoForm_Load(object sender, EventArgs e)
        {
            LoadProductData();
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
                ProductType.Text = productType;
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
            
            // Hide all specific fields by default
            textProdColorBox.Visible = false;
            label5.Visible = false;  // Color label
            textCoffinWeightBox.Visible = false;
            label6.Visible = false;  // Weight label
            textFlowerTypeBox.Visible = false;
            label7.Visible = false;  // Flower Type label
            
            switch (productType)
            {
                case "Coffin":
                    DataTable coffinDetails = db.GetCoffinDetailsById(productId);
                    if (coffinDetails != null && coffinDetails.Rows.Count > 0)
                    {
                        // Show and fill coffin-specific details
                        textProdColorBox.Visible = true;
                        label5.Visible = true;
                        textCoffinWeightBox.Visible = true;
                        label6.Visible = true;
                        
                        textProdColorBox.Text = coffinDetails.Rows[0]["color"].ToString();
                        textCoffinWeightBox.Text = coffinDetails.Rows[0]["size"].ToString();
                    }
                    break;
                    
                case "Urn":
                    DataTable urnDetails = db.GetUrnDetailsById(productId);
                    if (urnDetails != null && urnDetails.Rows.Count > 0)
                    {
                        // Show and fill urn-specific details - Urns only have size
                        textCoffinWeightBox.Visible = true;
                        label6.Text = "Urn Size";
                        label6.Visible = true;
                        
                        textCoffinWeightBox.Text = urnDetails.Rows[0]["size"].ToString();
                    }
                    break;
                    
                case "Flowers":
                    DataTable flowerDetails = db.GetFlowerDetailsById(productId);
                    if (flowerDetails != null && flowerDetails.Rows.Count > 0)
                    {
                        // Show and fill flower-specific details
                        textProdColorBox.Visible = true;
                        label5.Visible = true;
                        textFlowerTypeBox.Visible = true;
                        label7.Visible = true;
                        
                        textProdColorBox.Text = flowerDetails.Rows[0]["color"].ToString();
                        textFlowerTypeBox.Text = flowerDetails.Rows[0]["type"].ToString();
                    }
                    break;
            }
        }
    }
}