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

namespace funeraria.Forms
{
    public partial class ShopForm: Form
    {
        private int productId;
        private string productType;
        private int stockAvailable;
        private decimal price;

        // Constructor for opening the form with a specific product ID
        public ShopForm(int productId)
        {
            InitializeComponent();
            this.productId = productId;

            // Set up event handlers
            this.BuyButtonClick.Click += BuyButtonClick_Click;
            this.PictureBackClick.Click += PictureBackClick_Click;
            this.Load += ShopForm_Load;
        }

        private void ShopForm_Load(object sender, EventArgs e)
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
                productType = db.GetProductTypeById(productId);

                // Get stock information
                stockAvailable = Convert.ToInt32(row["stock"]);

                // Get price if available
                if (row["price"] != DBNull.Value)
                {
                    price = Convert.ToDecimal(row["price"]);
                }

                // Update the UI with product info
                ObjectType.Text = productType;
                ObjectID.Text = productId.ToString();
                ObjectStock.Text = stockAvailable.ToString();

                // Make text boxes read-only as they're for display only
                ObjectType.ReadOnly = true;
                ObjectID.ReadOnly = true;
                ObjectStock.ReadOnly = true;

                // Configure the quantity control
                ShopQuantity.Minimum = 1;
                ShopQuantity.Maximum = stockAvailable;
                ShopQuantity.Value = 1;
            }
            else
            {
                MessageBox.Show("Product information not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void BuyButtonClick_Click(object sender, EventArgs e)
        {
            int quantity = Convert.ToInt32(ShopQuantity.Value);

            if (quantity <= 0 || quantity > stockAvailable)
            {
                MessageBox.Show("Please select a valid quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Call the database method to update the stock
            Database db = Database.GetDatabase();
            bool success = db.PurchaseProduct(productId, quantity);

            if (success)
            {
                MessageBox.Show($"Successfully purchased {quantity} {productType}(s).", "Purchase Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed to complete the purchase.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PictureBackClick_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
