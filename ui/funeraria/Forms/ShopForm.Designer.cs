namespace funeraria.Forms
{
    partial class ShopForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ShopQuantity = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.ObjectType = new System.Windows.Forms.TextBox();
            this.ObjectStock = new System.Windows.Forms.TextBox();
            this.PictureBackClick = new System.Windows.Forms.PictureBox();
            this.BuyButtonClick = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ObjectID = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ShopQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBackClick)).BeginInit();
            this.SuspendLayout();
            // 
            // ShopQuantity
            // 
            this.ShopQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShopQuantity.Location = new System.Drawing.Point(129, 242);
            this.ShopQuantity.Name = "ShopQuantity";
            this.ShopQuantity.Size = new System.Drawing.Size(86, 30);
            this.ShopQuantity.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 244);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Quantity";
            // 
            // ObjectType
            // 
            this.ObjectType.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ObjectType.Location = new System.Drawing.Point(16, 111);
            this.ObjectType.Name = "ObjectType";
            this.ObjectType.Size = new System.Drawing.Size(281, 34);
            this.ObjectType.TabIndex = 3;
            // 
            // ObjectStock
            // 
            this.ObjectStock.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ObjectStock.Location = new System.Drawing.Point(321, 111);
            this.ObjectStock.Name = "ObjectStock";
            this.ObjectStock.Size = new System.Drawing.Size(111, 34);
            this.ObjectStock.TabIndex = 4;
            // 
            // PictureBackClick
            // 
            this.PictureBackClick.Image = global::funeraria.Properties.Resources.arrow;
            this.PictureBackClick.Location = new System.Drawing.Point(384, 9);
            this.PictureBackClick.Name = "PictureBackClick";
            this.PictureBackClick.Size = new System.Drawing.Size(74, 67);
            this.PictureBackClick.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureBackClick.TabIndex = 5;
            this.PictureBackClick.TabStop = false;
            // 
            // BuyButtonClick
            // 
            this.BuyButtonClick.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.BuyButtonClick.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BuyButtonClick.Location = new System.Drawing.Point(253, 217);
            this.BuyButtonClick.Name = "BuyButtonClick";
            this.BuyButtonClick.Size = new System.Drawing.Size(193, 55);
            this.BuyButtonClick.TabIndex = 6;
            this.BuyButtonClick.Text = "Buy";
            this.BuyButtonClick.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 25);
            this.label2.TabIndex = 7;
            this.label2.Text = "Product Type";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(316, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 25);
            this.label3.TabIndex = 8;
            this.label3.Text = "Stock";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 159);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 25);
            this.label4.TabIndex = 10;
            this.label4.Text = "Product ID";
            // 
            // ObjectID
            // 
            this.ObjectID.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ObjectID.Location = new System.Drawing.Point(16, 187);
            this.ObjectID.Name = "ObjectID";
            this.ObjectID.Size = new System.Drawing.Size(199, 34);
            this.ObjectID.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label5.BackColor = System.Drawing.SystemColors.Control;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(361, 67);
            this.label5.TabIndex = 11;
            this.label5.Text = "Shop Product";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ShopForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(244)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(470, 306);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ObjectID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BuyButtonClick);
            this.Controls.Add(this.PictureBackClick);
            this.Controls.Add(this.ObjectStock);
            this.Controls.Add(this.ObjectType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ShopQuantity);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ShopForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ShopForm";
            ((System.ComponentModel.ISupportInitialize)(this.ShopQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBackClick)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NumericUpDown ShopQuantity;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ObjectType;
        private System.Windows.Forms.TextBox ObjectStock;
        private System.Windows.Forms.PictureBox PictureBackClick;
        private System.Windows.Forms.Button BuyButtonClick;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox ObjectID;
        private System.Windows.Forms.Label label5;
    }
}