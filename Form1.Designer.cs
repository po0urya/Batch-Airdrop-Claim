namespace BatchAirdropClaim
{
    partial class Form1
    {

        private System.ComponentModel.IContainer components = null;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod


        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.btnCheckAndTransferTokens = new System.Windows.Forms.Button();
            this.btnExecuteAirdrop = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(776, 350);
            this.dataGridView1.TabIndex = 0;
            // 
            // btnLoadFile
            // 
            this.btnLoadFile.Location = new System.Drawing.Point(12, 368);
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.Size = new System.Drawing.Size(100, 23);
            this.btnLoadFile.TabIndex = 1;
            this.btnLoadFile.Text = "Load File";
            this.btnLoadFile.UseVisualStyleBackColor = true;
            this.btnLoadFile.Click += new System.EventHandler(this.btnLoadFile_Click);
            // 
            // btnCheckAndTransferTokens
            // 
            this.btnCheckAndTransferTokens.Location = new System.Drawing.Point(118, 368);
            this.btnCheckAndTransferTokens.Name = "btnCheckAndTransferTokens";
            this.btnCheckAndTransferTokens.Size = new System.Drawing.Size(150, 23);
            this.btnCheckAndTransferTokens.TabIndex = 2;
            this.btnCheckAndTransferTokens.Text = "Check and Transfer Tokens";
            this.btnCheckAndTransferTokens.UseVisualStyleBackColor = true;
            this.btnCheckAndTransferTokens.Click += new System.EventHandler(this.btnCheckAndTransferTokens_Click);
            // 
            // btnExecuteAirdrop
            // 
            this.btnExecuteAirdrop.Location = new System.Drawing.Point(274, 368);
            this.btnExecuteAirdrop.Name = "btnExecuteAirdrop";
            this.btnExecuteAirdrop.Size = new System.Drawing.Size(100, 23);
            this.btnExecuteAirdrop.TabIndex = 3;
            this.btnExecuteAirdrop.Text = "Execute Airdrop";
            this.btnExecuteAirdrop.UseVisualStyleBackColor = true;
            this.btnExecuteAirdrop.Click += new System.EventHandler(this.btnExecuteAirdrop_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnExecuteAirdrop);
            this.Controls.Add(this.btnCheckAndTransferTokens);
            this.Controls.Add(this.btnLoadFile);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form1";
            this.Text = "Batch Airdrop Claim";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnLoadFile;
        private System.Windows.Forms.Button btnCheckAndTransferTokens;
        private System.Windows.Forms.Button btnExecuteAirdrop;
    }
}

