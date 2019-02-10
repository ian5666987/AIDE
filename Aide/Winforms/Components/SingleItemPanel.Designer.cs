namespace Aide.Winforms.Components {
  partial class SingleItemPanel {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.labelItemTitle = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // labelItemTitle
      // 
      this.labelItemTitle.AutoSize = true;
      this.labelItemTitle.Location = new System.Drawing.Point(4, 7);
      this.labelItemTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.labelItemTitle.MaximumSize = new System.Drawing.Size(240, 0);
      this.labelItemTitle.Name = "labelItemTitle";
      this.labelItemTitle.Size = new System.Drawing.Size(85, 25);
      this.labelItemTitle.TabIndex = 0;
      this.labelItemTitle.Text = "itemTitle";
      // 
      // SingleItemPanel
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.labelItemTitle);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.Name = "SingleItemPanel";
      this.Size = new System.Drawing.Size(624, 41);
      this.Load += new System.EventHandler(this.SingleItemPanel_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label labelItemTitle;
  }
}
