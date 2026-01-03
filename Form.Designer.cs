namespace TestConnection_Honda
{
    partial class Form1
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
            this.btnTunerPro = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnTunerPro
            // 
            this.btnTunerPro.Location = new System.Drawing.Point(48, 23);
            this.btnTunerPro.Name = "btnTunerPro";
            this.btnTunerPro.Size = new System.Drawing.Size(136, 41);
            this.btnTunerPro.TabIndex = 0;
            this.btnTunerPro.Text = "Tuner Pro RT";
            this.btnTunerPro.UseVisualStyleBackColor = true;
            this.btnTunerPro.Click += new System.EventHandler(this.btnTunerPro_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 190);
            this.Controls.Add(this.btnTunerPro);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnTunerPro;
    }
}

