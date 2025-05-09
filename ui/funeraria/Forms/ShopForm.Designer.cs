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
            this.ObjectName = new System.Windows.Forms.TextBox();
            this.ObjectID = new System.Windows.Forms.TextBox();
            this.PictureBackClick = new System.Windows.Forms.PictureBox();
            this.BuyButtonClick = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ShopQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBackClick)).BeginInit();
            this.SuspendLayout();
            // 
            // ShopQuantity
            // 
            this.ShopQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShopQuantity.Location = new System.Drawing.Point(153, 135);
            this.ShopQuantity.Name = "ShopQuantity";
            this.ShopQuantity.Size = new System.Drawing.Size(86, 30);
            this.ShopQuantity.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(42, 137);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Quantity";
            // 
            // ObjectName
            // 
            this.ObjectName.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ObjectName.Location = new System.Drawing.Point(47, 37);
            this.ObjectName.Name = "ObjectName";
            this.ObjectName.Size = new System.Drawing.Size(295, 34);
            this.ObjectName.TabIndex = 3;
            // 
            // ObjectID
            // 
            this.ObjectID.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ObjectID.Location = new System.Drawing.Point(377, 37);
            this.ObjectID.Name = "ObjectID";
            this.ObjectID.Size = new System.Drawing.Size(111, 34);
            this.ObjectID.TabIndex = 4;
            // 
            // PictureBackClick
            // 
            this.PictureBackClick.Image = global::funeraria.Properties.Resources.arrow;
            this.PictureBackClick.Location = new System.Drawing.Point(587, 24);
            this.PictureBackClick.Name = "PictureBackClick";
            this.PictureBackClick.Size = new System.Drawing.Size(62, 57);
            this.PictureBackClick.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureBackClick.TabIndex = 5;
            this.PictureBackClick.TabStop = false;
            // 
            // BuyButtonClick
            // 
            this.BuyButtonClick.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.BuyButtonClick.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BuyButtonClick.Location = new System.Drawing.Point(406, 168);
            this.BuyButtonClick.Name = "BuyButtonClick";
            this.BuyButtonClick.Size = new System.Drawing.Size(193, 55);
            this.BuyButtonClick.TabIndex = 6;
            this.BuyButtonClick.Text = "Buy";
            this.BuyButtonClick.UseVisualStyleBackColor = false;
            // 
            // ShopForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(244)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(702, 278);
            this.Controls.Add(this.BuyButtonClick);
            this.Controls.Add(this.PictureBackClick);
            this.Controls.Add(this.ObjectID);
            this.Controls.Add(this.ObjectName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ShopQuantity);
            this.Name = "ShopForm";
            this.Text = "ShopForm";
            ((System.ComponentModel.ISupportInitialize)(this.ShopQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBackClick)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NumericUpDown ShopQuantity;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ObjectName;
        private System.Windows.Forms.TextBox ObjectID;
        private System.Windows.Forms.PictureBox PictureBackClick;
        private System.Windows.Forms.Button BuyButtonClick;
    }
}