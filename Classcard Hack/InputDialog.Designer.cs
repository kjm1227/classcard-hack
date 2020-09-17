namespace Classcard_Hack
{
    partial class InputDialog
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
            this.ScoreInput = new System.Windows.Forms.TextBox();
            this.ApplyBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ScoreInput
            // 
            this.ScoreInput.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ScoreInput.Location = new System.Drawing.Point(12, 12);
            this.ScoreInput.Name = "ScoreInput";
            this.ScoreInput.Size = new System.Drawing.Size(120, 35);
            this.ScoreInput.TabIndex = 0;
            this.ScoreInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ScoreInput_KeyPress);
            // 
            // ApplyBtn
            // 
            this.ApplyBtn.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ApplyBtn.Location = new System.Drawing.Point(142, 12);
            this.ApplyBtn.Name = "ApplyBtn";
            this.ApplyBtn.Size = new System.Drawing.Size(80, 35);
            this.ApplyBtn.TabIndex = 1;
            this.ApplyBtn.Text = "Apply";
            this.ApplyBtn.UseVisualStyleBackColor = true;
            this.ApplyBtn.Click += new System.EventHandler(this.ApplyBtn_Click);
            // 
            // InputDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(234, 61);
            this.Controls.Add(this.ApplyBtn);
            this.Controls.Add(this.ScoreInput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Change Score";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InputDialog_FormClosing);
            this.Load += new System.EventHandler(this.InputDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ScoreInput;
        private System.Windows.Forms.Button ApplyBtn;
    }
}