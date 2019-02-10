namespace Aide.Winforms {
  partial class AdminForm {
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
      if (userIndexForm != null && !userIndexForm.IsDisposed)
        userIndexForm.Dispose();
      if (teamIndexForm != null && !teamIndexForm.IsDisposed)
        teamIndexForm.Dispose();
      if (roleIndexForm != null && !roleIndexForm.IsDisposed)
        roleIndexForm.Dispose();
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.groupBoxUsers = new System.Windows.Forms.GroupBox();
      this.buttonTeams = new System.Windows.Forms.Button();
      this.buttonUsers = new System.Windows.Forms.Button();
      this.groupBoxLogs = new System.Windows.Forms.GroupBox();
      this.buttonActionLog = new System.Windows.Forms.Button();
      this.buttonAccessLog = new System.Windows.Forms.Button();
      this.groupBoxRoles = new System.Windows.Forms.GroupBox();
      this.buttonRoles = new System.Windows.Forms.Button();
      this.groupBoxDeveloperOptions = new System.Windows.Forms.GroupBox();
      this.buttonErrorLog = new System.Windows.Forms.Button();
      this.buttonUserMap = new System.Windows.Forms.Button();
      this.buttonMeta = new System.Windows.Forms.Button();
      this.groupBoxUsers.SuspendLayout();
      this.groupBoxLogs.SuspendLayout();
      this.groupBoxRoles.SuspendLayout();
      this.groupBoxDeveloperOptions.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBoxUsers
      // 
      this.groupBoxUsers.Controls.Add(this.buttonTeams);
      this.groupBoxUsers.Controls.Add(this.buttonUsers);
      this.groupBoxUsers.Location = new System.Drawing.Point(12, 12);
      this.groupBoxUsers.Name = "groupBoxUsers";
      this.groupBoxUsers.Size = new System.Drawing.Size(209, 124);
      this.groupBoxUsers.TabIndex = 0;
      this.groupBoxUsers.TabStop = false;
      this.groupBoxUsers.Text = "Users";
      // 
      // buttonTeams
      // 
      this.buttonTeams.Location = new System.Drawing.Point(6, 75);
      this.buttonTeams.Name = "buttonTeams";
      this.buttonTeams.Size = new System.Drawing.Size(197, 40);
      this.buttonTeams.TabIndex = 1;
      this.buttonTeams.Text = "Teams";
      this.buttonTeams.UseVisualStyleBackColor = true;
      this.buttonTeams.Click += new System.EventHandler(this.buttonTeams_Click);
      // 
      // buttonUsers
      // 
      this.buttonUsers.Location = new System.Drawing.Point(6, 29);
      this.buttonUsers.Name = "buttonUsers";
      this.buttonUsers.Size = new System.Drawing.Size(197, 40);
      this.buttonUsers.TabIndex = 0;
      this.buttonUsers.Text = "Users";
      this.buttonUsers.UseVisualStyleBackColor = true;
      this.buttonUsers.Click += new System.EventHandler(this.buttonUsers_Click);
      // 
      // groupBoxLogs
      // 
      this.groupBoxLogs.Controls.Add(this.buttonActionLog);
      this.groupBoxLogs.Controls.Add(this.buttonAccessLog);
      this.groupBoxLogs.Location = new System.Drawing.Point(12, 142);
      this.groupBoxLogs.Name = "groupBoxLogs";
      this.groupBoxLogs.Size = new System.Drawing.Size(209, 124);
      this.groupBoxLogs.TabIndex = 1;
      this.groupBoxLogs.TabStop = false;
      this.groupBoxLogs.Text = "Logs";
      // 
      // buttonActionLog
      // 
      this.buttonActionLog.Location = new System.Drawing.Point(6, 75);
      this.buttonActionLog.Name = "buttonActionLog";
      this.buttonActionLog.Size = new System.Drawing.Size(197, 40);
      this.buttonActionLog.TabIndex = 1;
      this.buttonActionLog.Text = "Action Log";
      this.buttonActionLog.UseVisualStyleBackColor = true;
      this.buttonActionLog.Click += new System.EventHandler(this.buttonActionLog_Click);
      // 
      // buttonAccessLog
      // 
      this.buttonAccessLog.Location = new System.Drawing.Point(6, 29);
      this.buttonAccessLog.Name = "buttonAccessLog";
      this.buttonAccessLog.Size = new System.Drawing.Size(197, 40);
      this.buttonAccessLog.TabIndex = 0;
      this.buttonAccessLog.Text = "Access Log";
      this.buttonAccessLog.UseVisualStyleBackColor = true;
      this.buttonAccessLog.Click += new System.EventHandler(this.buttonAccessLog_Click);
      // 
      // groupBoxRoles
      // 
      this.groupBoxRoles.Controls.Add(this.buttonRoles);
      this.groupBoxRoles.Location = new System.Drawing.Point(12, 272);
      this.groupBoxRoles.Name = "groupBoxRoles";
      this.groupBoxRoles.Size = new System.Drawing.Size(209, 79);
      this.groupBoxRoles.TabIndex = 2;
      this.groupBoxRoles.TabStop = false;
      this.groupBoxRoles.Text = "Roles";
      // 
      // buttonRoles
      // 
      this.buttonRoles.Location = new System.Drawing.Point(6, 29);
      this.buttonRoles.Name = "buttonRoles";
      this.buttonRoles.Size = new System.Drawing.Size(197, 40);
      this.buttonRoles.TabIndex = 0;
      this.buttonRoles.Text = "Roles";
      this.buttonRoles.UseVisualStyleBackColor = true;
      this.buttonRoles.Click += new System.EventHandler(this.buttonRoles_Click);
      // 
      // groupBoxDeveloperOptions
      // 
      this.groupBoxDeveloperOptions.Controls.Add(this.buttonErrorLog);
      this.groupBoxDeveloperOptions.Controls.Add(this.buttonUserMap);
      this.groupBoxDeveloperOptions.Controls.Add(this.buttonMeta);
      this.groupBoxDeveloperOptions.Location = new System.Drawing.Point(12, 357);
      this.groupBoxDeveloperOptions.Name = "groupBoxDeveloperOptions";
      this.groupBoxDeveloperOptions.Size = new System.Drawing.Size(209, 169);
      this.groupBoxDeveloperOptions.TabIndex = 3;
      this.groupBoxDeveloperOptions.TabStop = false;
      this.groupBoxDeveloperOptions.Text = "Developer Options";
      // 
      // buttonErrorLog
      // 
      this.buttonErrorLog.Location = new System.Drawing.Point(6, 121);
      this.buttonErrorLog.Name = "buttonErrorLog";
      this.buttonErrorLog.Size = new System.Drawing.Size(197, 40);
      this.buttonErrorLog.TabIndex = 2;
      this.buttonErrorLog.Text = "Error Log";
      this.buttonErrorLog.UseVisualStyleBackColor = true;
      this.buttonErrorLog.Click += new System.EventHandler(this.buttonErrorLog_Click);
      // 
      // buttonUserMap
      // 
      this.buttonUserMap.Location = new System.Drawing.Point(6, 75);
      this.buttonUserMap.Name = "buttonUserMap";
      this.buttonUserMap.Size = new System.Drawing.Size(197, 40);
      this.buttonUserMap.TabIndex = 1;
      this.buttonUserMap.Text = "User Map";
      this.buttonUserMap.UseVisualStyleBackColor = true;
      this.buttonUserMap.Click += new System.EventHandler(this.buttonUserMap_Click);
      // 
      // buttonMeta
      // 
      this.buttonMeta.Location = new System.Drawing.Point(6, 29);
      this.buttonMeta.Name = "buttonMeta";
      this.buttonMeta.Size = new System.Drawing.Size(197, 40);
      this.buttonMeta.TabIndex = 0;
      this.buttonMeta.Text = "Meta";
      this.buttonMeta.UseVisualStyleBackColor = true;
      this.buttonMeta.Click += new System.EventHandler(this.buttonMeta_Click);
      // 
      // AdminForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(237, 538);
      this.Controls.Add(this.groupBoxDeveloperOptions);
      this.Controls.Add(this.groupBoxRoles);
      this.Controls.Add(this.groupBoxLogs);
      this.Controls.Add(this.groupBoxUsers);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.Name = "AdminForm";
      this.Text = "Admin";
      this.groupBoxUsers.ResumeLayout(false);
      this.groupBoxLogs.ResumeLayout(false);
      this.groupBoxRoles.ResumeLayout(false);
      this.groupBoxDeveloperOptions.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBoxUsers;
    private System.Windows.Forms.Button buttonUsers;
    private System.Windows.Forms.Button buttonTeams;
    private System.Windows.Forms.GroupBox groupBoxLogs;
    private System.Windows.Forms.Button buttonActionLog;
    private System.Windows.Forms.Button buttonAccessLog;
    private System.Windows.Forms.GroupBox groupBoxRoles;
    private System.Windows.Forms.Button buttonRoles;
    private System.Windows.Forms.GroupBox groupBoxDeveloperOptions;
    private System.Windows.Forms.Button buttonUserMap;
    private System.Windows.Forms.Button buttonMeta;
    private System.Windows.Forms.Button buttonErrorLog;
  }
}