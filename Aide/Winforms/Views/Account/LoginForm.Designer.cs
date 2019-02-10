namespace Aide.Winforms {
  partial class LoginForm {
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.textBoxLoginIdentity = new System.Windows.Forms.TextBox();
      this.labelLoginIdentity = new System.Windows.Forms.Label();
      this.labelLoginPassword = new System.Windows.Forms.Label();
      this.buttonLogin = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.textBoxLoginPassword = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // textBoxLoginIdentity
      // 
      this.textBoxLoginIdentity.Location = new System.Drawing.Point(181, 10);
      this.textBoxLoginIdentity.Name = "textBoxLoginIdentity";
      this.textBoxLoginIdentity.Size = new System.Drawing.Size(294, 30);
      this.textBoxLoginIdentity.TabIndex = 0;
      // 
      // labelLoginIdentity
      // 
      this.labelLoginIdentity.AutoSize = true;
      this.labelLoginIdentity.Location = new System.Drawing.Point(12, 13);
      this.labelLoginIdentity.Name = "labelLoginIdentity";
      this.labelLoginIdentity.Size = new System.Drawing.Size(74, 25);
      this.labelLoginIdentity.TabIndex = 2;
      this.labelLoginIdentity.Text = "Identity";
      // 
      // labelLoginPassword
      // 
      this.labelLoginPassword.AutoSize = true;
      this.labelLoginPassword.Location = new System.Drawing.Point(12, 54);
      this.labelLoginPassword.Name = "labelLoginPassword";
      this.labelLoginPassword.Size = new System.Drawing.Size(98, 25);
      this.labelLoginPassword.TabIndex = 3;
      this.labelLoginPassword.Text = "Password";
      // 
      // buttonLogin
      // 
      this.buttonLogin.Location = new System.Drawing.Point(181, 92);
      this.buttonLogin.Name = "buttonLogin";
      this.buttonLogin.Size = new System.Drawing.Size(294, 37);
      this.buttonLogin.TabIndex = 2;
      this.buttonLogin.Text = "Login";
      this.buttonLogin.UseVisualStyleBackColor = true;
      this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Location = new System.Drawing.Point(17, 92);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(140, 37);
      this.buttonCancel.TabIndex = 3;
      this.buttonCancel.Text = "◀ Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // textBoxLoginPassword
      // 
      this.textBoxLoginPassword.Location = new System.Drawing.Point(181, 51);
      this.textBoxLoginPassword.Name = "textBoxLoginPassword";
      this.textBoxLoginPassword.Size = new System.Drawing.Size(294, 30);
      this.textBoxLoginPassword.TabIndex = 1;
      this.textBoxLoginPassword.UseSystemPasswordChar = true;
      // 
      // LoginForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(487, 140);
      this.ControlBox = false;
      this.Controls.Add(this.textBoxLoginPassword);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonLogin);
      this.Controls.Add(this.labelLoginPassword);
      this.Controls.Add(this.labelLoginIdentity);
      this.Controls.Add(this.textBoxLoginIdentity);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.MaximumSize = new System.Drawing.Size(505, 187);
      this.MinimumSize = new System.Drawing.Size(505, 187);
      this.Name = "LoginForm";
      this.Text = "Login";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox textBoxLoginIdentity;
    private System.Windows.Forms.Label labelLoginIdentity;
    private System.Windows.Forms.Label labelLoginPassword;
    private System.Windows.Forms.Button buttonLogin;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.TextBox textBoxLoginPassword;
  }
}