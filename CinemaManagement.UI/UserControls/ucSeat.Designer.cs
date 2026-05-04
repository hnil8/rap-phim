namespace CinemaManagement.UI.UserControls
{
    partial class ucSeat
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

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.btnSeat = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            // 
            // btnSeat
            // 
            this.btnSeat.BorderRadius = 8;
            this.btnSeat.BorderThickness = 1;
            this.btnSeat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSeat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSeat.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSeat.Location = new System.Drawing.Point(0, 0);
            this.btnSeat.Name = "btnSeat";
            this.btnSeat.Size = new System.Drawing.Size(50, 50);
            this.btnSeat.TabIndex = 0;
            this.btnSeat.Text = "A1";
            // 
            // ucSeat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnSeat);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "ucSeat";
            this.Size = new System.Drawing.Size(50, 50);
            this.ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Button btnSeat;
    }
}