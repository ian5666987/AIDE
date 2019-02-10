namespace Aide.Winforms {
  partial class ManageIndexForm {
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
      this.splitContainerMain = new System.Windows.Forms.SplitContainer();
      this.flowLayoutPanelHeader = new System.Windows.Forms.FlowLayoutPanel();
      this.buttonClose = new System.Windows.Forms.Button();
      this.labelTitle = new System.Windows.Forms.Label();
      this.labelAction = new System.Windows.Forms.Label();
      this.groupBoxAccountManagement = new System.Windows.Forms.GroupBox();
      this.labelFullName = new System.Windows.Forms.Label();
      this.flowLayoutPanelDisplayName = new System.Windows.Forms.FlowLayoutPanel();
      this.labelDisplayNameValue = new System.Windows.Forms.Label();
      this.linkLabelDisplayNameChange = new System.Windows.Forms.LinkLabel();
      this.labelDisplayName = new System.Windows.Forms.Label();
      this.labelLastLoginValue = new System.Windows.Forms.Label();
      this.labelPassword = new System.Windows.Forms.Label();
      this.flowLayoutPanelPassword = new System.Windows.Forms.FlowLayoutPanel();
      this.labelPasswordValue = new System.Windows.Forms.Label();
      this.linkLabelPasswordChange = new System.Windows.Forms.LinkLabel();
      this.labelUserName = new System.Windows.Forms.Label();
      this.labelRegistrationDateValue = new System.Windows.Forms.Label();
      this.labelWorkingRole = new System.Windows.Forms.Label();
      this.labelTeamValue = new System.Windows.Forms.Label();
      this.labelAdminRole = new System.Windows.Forms.Label();
      this.labelAdminRoleValue = new System.Windows.Forms.Label();
      this.labelTeam = new System.Windows.Forms.Label();
      this.labelWorkingRoleValue = new System.Windows.Forms.Label();
      this.labelRegistrationDate = new System.Windows.Forms.Label();
      this.labelUserNameValue = new System.Windows.Forms.Label();
      this.labelLastLogin = new System.Windows.Forms.Label();
      this.labelFullNameValue = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
      this.splitContainerMain.Panel1.SuspendLayout();
      this.splitContainerMain.Panel2.SuspendLayout();
      this.splitContainerMain.SuspendLayout();
      this.flowLayoutPanelHeader.SuspendLayout();
      this.groupBoxAccountManagement.SuspendLayout();
      this.flowLayoutPanelDisplayName.SuspendLayout();
      this.flowLayoutPanelPassword.SuspendLayout();
      this.SuspendLayout();
      // 
      // splitContainerMain
      // 
      this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainerMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
      this.splitContainerMain.IsSplitterFixed = true;
      this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
      this.splitContainerMain.Name = "splitContainerMain";
      this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitContainerMain.Panel1
      // 
      this.splitContainerMain.Panel1.Controls.Add(this.flowLayoutPanelHeader);
      // 
      // splitContainerMain.Panel2
      // 
      this.splitContainerMain.Panel2.Controls.Add(this.groupBoxAccountManagement);
      this.splitContainerMain.Size = new System.Drawing.Size(657, 473);
      this.splitContainerMain.SplitterDistance = 54;
      this.splitContainerMain.TabIndex = 11;
      // 
      // flowLayoutPanelHeader
      // 
      this.flowLayoutPanelHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.flowLayoutPanelHeader.Controls.Add(this.buttonClose);
      this.flowLayoutPanelHeader.Controls.Add(this.labelTitle);
      this.flowLayoutPanelHeader.Controls.Add(this.labelAction);
      this.flowLayoutPanelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flowLayoutPanelHeader.Location = new System.Drawing.Point(0, 0);
      this.flowLayoutPanelHeader.Name = "flowLayoutPanelHeader";
      this.flowLayoutPanelHeader.Size = new System.Drawing.Size(657, 54);
      this.flowLayoutPanelHeader.TabIndex = 4;
      // 
      // buttonClose
      // 
      this.buttonClose.Location = new System.Drawing.Point(10, 5);
      this.buttonClose.Margin = new System.Windows.Forms.Padding(10, 5, 10, 3);
      this.buttonClose.Name = "buttonClose";
      this.buttonClose.Size = new System.Drawing.Size(99, 42);
      this.buttonClose.TabIndex = 3;
      this.buttonClose.Text = "Close";
      this.buttonClose.UseVisualStyleBackColor = true;
      this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
      // 
      // labelTitle
      // 
      this.labelTitle.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.labelTitle.AutoSize = true;
      this.labelTitle.Font = new System.Drawing.Font("MS Reference Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelTitle.ForeColor = System.Drawing.SystemColors.HotTrack;
      this.labelTitle.Location = new System.Drawing.Point(124, 7);
      this.labelTitle.Margin = new System.Windows.Forms.Padding(5, 5, 0, 0);
      this.labelTitle.Name = "labelTitle";
      this.labelTitle.Size = new System.Drawing.Size(88, 40);
      this.labelTitle.TabIndex = 2;
      this.labelTitle.Text = "Title";
      // 
      // labelAction
      // 
      this.labelAction.AutoSize = true;
      this.labelAction.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelAction.ForeColor = System.Drawing.SystemColors.Highlight;
      this.labelAction.Location = new System.Drawing.Point(212, 17);
      this.labelAction.Margin = new System.Windows.Forms.Padding(0, 17, 5, 0);
      this.labelAction.Name = "labelAction";
      this.labelAction.Size = new System.Drawing.Size(132, 29);
      this.labelAction.TabIndex = 6;
      this.labelAction.Text = "labelAction";
      // 
      // groupBoxAccountManagement
      // 
      this.groupBoxAccountManagement.Controls.Add(this.labelFullName);
      this.groupBoxAccountManagement.Controls.Add(this.flowLayoutPanelDisplayName);
      this.groupBoxAccountManagement.Controls.Add(this.labelDisplayName);
      this.groupBoxAccountManagement.Controls.Add(this.labelLastLoginValue);
      this.groupBoxAccountManagement.Controls.Add(this.labelPassword);
      this.groupBoxAccountManagement.Controls.Add(this.flowLayoutPanelPassword);
      this.groupBoxAccountManagement.Controls.Add(this.labelUserName);
      this.groupBoxAccountManagement.Controls.Add(this.labelRegistrationDateValue);
      this.groupBoxAccountManagement.Controls.Add(this.labelWorkingRole);
      this.groupBoxAccountManagement.Controls.Add(this.labelTeamValue);
      this.groupBoxAccountManagement.Controls.Add(this.labelAdminRole);
      this.groupBoxAccountManagement.Controls.Add(this.labelAdminRoleValue);
      this.groupBoxAccountManagement.Controls.Add(this.labelTeam);
      this.groupBoxAccountManagement.Controls.Add(this.labelWorkingRoleValue);
      this.groupBoxAccountManagement.Controls.Add(this.labelRegistrationDate);
      this.groupBoxAccountManagement.Controls.Add(this.labelUserNameValue);
      this.groupBoxAccountManagement.Controls.Add(this.labelLastLogin);
      this.groupBoxAccountManagement.Controls.Add(this.labelFullNameValue);
      this.groupBoxAccountManagement.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBoxAccountManagement.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBoxAccountManagement.ForeColor = System.Drawing.Color.DarkGreen;
      this.groupBoxAccountManagement.Location = new System.Drawing.Point(0, 0);
      this.groupBoxAccountManagement.Name = "groupBoxAccountManagement";
      this.groupBoxAccountManagement.Size = new System.Drawing.Size(657, 415);
      this.groupBoxAccountManagement.TabIndex = 20;
      this.groupBoxAccountManagement.TabStop = false;
      this.groupBoxAccountManagement.Text = "Account Management";
      // 
      // labelFullName
      // 
      this.labelFullName.AutoSize = true;
      this.labelFullName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelFullName.ForeColor = System.Drawing.SystemColors.ControlText;
      this.labelFullName.Location = new System.Drawing.Point(8, 33);
      this.labelFullName.Name = "labelFullName";
      this.labelFullName.Size = new System.Drawing.Size(109, 25);
      this.labelFullName.TabIndex = 0;
      this.labelFullName.Text = "Full Name";
      // 
      // flowLayoutPanelDisplayName
      // 
      this.flowLayoutPanelDisplayName.Controls.Add(this.labelDisplayNameValue);
      this.flowLayoutPanelDisplayName.Controls.Add(this.linkLabelDisplayNameChange);
      this.flowLayoutPanelDisplayName.Location = new System.Drawing.Point(206, 117);
      this.flowLayoutPanelDisplayName.Name = "flowLayoutPanelDisplayName";
      this.flowLayoutPanelDisplayName.Size = new System.Drawing.Size(1364, 30);
      this.flowLayoutPanelDisplayName.TabIndex = 18;
      // 
      // labelDisplayNameValue
      // 
      this.labelDisplayNameValue.AutoSize = true;
      this.labelDisplayNameValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelDisplayNameValue.ForeColor = System.Drawing.SystemColors.ControlText;
      this.labelDisplayNameValue.Location = new System.Drawing.Point(3, 0);
      this.labelDisplayNameValue.Name = "labelDisplayNameValue";
      this.labelDisplayNameValue.Size = new System.Drawing.Size(189, 25);
      this.labelDisplayNameValue.TabIndex = 10;
      this.labelDisplayNameValue.Text = "Display Name Value";
      // 
      // linkLabelDisplayNameChange
      // 
      this.linkLabelDisplayNameChange.AutoSize = true;
      this.linkLabelDisplayNameChange.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.linkLabelDisplayNameChange.Location = new System.Drawing.Point(198, 0);
      this.linkLabelDisplayNameChange.Name = "linkLabelDisplayNameChange";
      this.linkLabelDisplayNameChange.Size = new System.Drawing.Size(82, 25);
      this.linkLabelDisplayNameChange.TabIndex = 11;
      this.linkLabelDisplayNameChange.TabStop = true;
      this.linkLabelDisplayNameChange.Text = "Change";
      this.linkLabelDisplayNameChange.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelDisplayNameChange_LinkClicked);
      // 
      // labelDisplayName
      // 
      this.labelDisplayName.AutoSize = true;
      this.labelDisplayName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelDisplayName.ForeColor = System.Drawing.SystemColors.ControlText;
      this.labelDisplayName.Location = new System.Drawing.Point(8, 117);
      this.labelDisplayName.Name = "labelDisplayName";
      this.labelDisplayName.Size = new System.Drawing.Size(145, 25);
      this.labelDisplayName.TabIndex = 1;
      this.labelDisplayName.Text = "Display Name";
      // 
      // labelLastLoginValue
      // 
      this.labelLastLoginValue.AutoSize = true;
      this.labelLastLoginValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelLastLoginValue.ForeColor = System.Drawing.SystemColors.ControlText;
      this.labelLastLoginValue.Location = new System.Drawing.Point(209, 377);
      this.labelLastLoginValue.Name = "labelLastLoginValue";
      this.labelLastLoginValue.Size = new System.Drawing.Size(158, 25);
      this.labelLastLoginValue.TabIndex = 17;
      this.labelLastLoginValue.Text = "Last Login Value";
      // 
      // labelPassword
      // 
      this.labelPassword.AutoSize = true;
      this.labelPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelPassword.ForeColor = System.Drawing.SystemColors.ControlText;
      this.labelPassword.Location = new System.Drawing.Point(8, 160);
      this.labelPassword.Name = "labelPassword";
      this.labelPassword.Size = new System.Drawing.Size(106, 25);
      this.labelPassword.TabIndex = 2;
      this.labelPassword.Text = "Password";
      // 
      // flowLayoutPanelPassword
      // 
      this.flowLayoutPanelPassword.Controls.Add(this.labelPasswordValue);
      this.flowLayoutPanelPassword.Controls.Add(this.linkLabelPasswordChange);
      this.flowLayoutPanelPassword.Location = new System.Drawing.Point(206, 160);
      this.flowLayoutPanelPassword.Name = "flowLayoutPanelPassword";
      this.flowLayoutPanelPassword.Size = new System.Drawing.Size(1364, 30);
      this.flowLayoutPanelPassword.TabIndex = 19;
      // 
      // labelPasswordValue
      // 
      this.labelPasswordValue.AutoSize = true;
      this.labelPasswordValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelPasswordValue.ForeColor = System.Drawing.SystemColors.ControlText;
      this.labelPasswordValue.Location = new System.Drawing.Point(3, 0);
      this.labelPasswordValue.Name = "labelPasswordValue";
      this.labelPasswordValue.Size = new System.Drawing.Size(60, 25);
      this.labelPasswordValue.TabIndex = 10;
      this.labelPasswordValue.Text = "******";
      // 
      // linkLabelPasswordChange
      // 
      this.linkLabelPasswordChange.AutoSize = true;
      this.linkLabelPasswordChange.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.linkLabelPasswordChange.Location = new System.Drawing.Point(69, 0);
      this.linkLabelPasswordChange.Name = "linkLabelPasswordChange";
      this.linkLabelPasswordChange.Size = new System.Drawing.Size(82, 25);
      this.linkLabelPasswordChange.TabIndex = 11;
      this.linkLabelPasswordChange.TabStop = true;
      this.linkLabelPasswordChange.Text = "Change";
      this.linkLabelPasswordChange.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelPasswordChange_LinkClicked);
      // 
      // labelUserName
      // 
      this.labelUserName.AutoSize = true;
      this.labelUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelUserName.ForeColor = System.Drawing.SystemColors.ControlText;
      this.labelUserName.Location = new System.Drawing.Point(8, 74);
      this.labelUserName.Name = "labelUserName";
      this.labelUserName.Size = new System.Drawing.Size(119, 25);
      this.labelUserName.TabIndex = 3;
      this.labelUserName.Text = "User Name";
      // 
      // labelRegistrationDateValue
      // 
      this.labelRegistrationDateValue.AutoSize = true;
      this.labelRegistrationDateValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelRegistrationDateValue.ForeColor = System.Drawing.SystemColors.ControlText;
      this.labelRegistrationDateValue.Location = new System.Drawing.Point(209, 334);
      this.labelRegistrationDateValue.Name = "labelRegistrationDateValue";
      this.labelRegistrationDateValue.Size = new System.Drawing.Size(216, 25);
      this.labelRegistrationDateValue.TabIndex = 16;
      this.labelRegistrationDateValue.Text = "Registration Date Value";
      // 
      // labelWorkingRole
      // 
      this.labelWorkingRole.AutoSize = true;
      this.labelWorkingRole.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelWorkingRole.ForeColor = System.Drawing.SystemColors.ControlText;
      this.labelWorkingRole.Location = new System.Drawing.Point(8, 205);
      this.labelWorkingRole.Name = "labelWorkingRole";
      this.labelWorkingRole.Size = new System.Drawing.Size(141, 25);
      this.labelWorkingRole.TabIndex = 4;
      this.labelWorkingRole.Text = "Working Role";
      // 
      // labelTeamValue
      // 
      this.labelTeamValue.AutoSize = true;
      this.labelTeamValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelTeamValue.ForeColor = System.Drawing.SystemColors.ControlText;
      this.labelTeamValue.Location = new System.Drawing.Point(209, 291);
      this.labelTeamValue.Name = "labelTeamValue";
      this.labelTeamValue.Size = new System.Drawing.Size(119, 25);
      this.labelTeamValue.TabIndex = 15;
      this.labelTeamValue.Text = "Team Value";
      // 
      // labelAdminRole
      // 
      this.labelAdminRole.AutoSize = true;
      this.labelAdminRole.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelAdminRole.ForeColor = System.Drawing.SystemColors.ControlText;
      this.labelAdminRole.Location = new System.Drawing.Point(8, 248);
      this.labelAdminRole.Name = "labelAdminRole";
      this.labelAdminRole.Size = new System.Drawing.Size(122, 25);
      this.labelAdminRole.TabIndex = 5;
      this.labelAdminRole.Text = "Admin Role";
      // 
      // labelAdminRoleValue
      // 
      this.labelAdminRoleValue.AutoSize = true;
      this.labelAdminRoleValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelAdminRoleValue.ForeColor = System.Drawing.SystemColors.ControlText;
      this.labelAdminRoleValue.Location = new System.Drawing.Point(209, 248);
      this.labelAdminRoleValue.Name = "labelAdminRoleValue";
      this.labelAdminRoleValue.Size = new System.Drawing.Size(168, 25);
      this.labelAdminRoleValue.TabIndex = 14;
      this.labelAdminRoleValue.Text = "Admin Role Value";
      // 
      // labelTeam
      // 
      this.labelTeam.AutoSize = true;
      this.labelTeam.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelTeam.ForeColor = System.Drawing.SystemColors.ControlText;
      this.labelTeam.Location = new System.Drawing.Point(8, 291);
      this.labelTeam.Name = "labelTeam";
      this.labelTeam.Size = new System.Drawing.Size(67, 25);
      this.labelTeam.TabIndex = 6;
      this.labelTeam.Text = "Team";
      // 
      // labelWorkingRoleValue
      // 
      this.labelWorkingRoleValue.AutoSize = true;
      this.labelWorkingRoleValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelWorkingRoleValue.ForeColor = System.Drawing.SystemColors.ControlText;
      this.labelWorkingRoleValue.Location = new System.Drawing.Point(209, 205);
      this.labelWorkingRoleValue.Name = "labelWorkingRoleValue";
      this.labelWorkingRoleValue.Size = new System.Drawing.Size(185, 25);
      this.labelWorkingRoleValue.TabIndex = 13;
      this.labelWorkingRoleValue.Text = "Working Role Value";
      // 
      // labelRegistrationDate
      // 
      this.labelRegistrationDate.AutoSize = true;
      this.labelRegistrationDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelRegistrationDate.ForeColor = System.Drawing.SystemColors.ControlText;
      this.labelRegistrationDate.Location = new System.Drawing.Point(8, 334);
      this.labelRegistrationDate.Name = "labelRegistrationDate";
      this.labelRegistrationDate.Size = new System.Drawing.Size(177, 25);
      this.labelRegistrationDate.TabIndex = 7;
      this.labelRegistrationDate.Text = "Registration Date";
      // 
      // labelUserNameValue
      // 
      this.labelUserNameValue.AutoSize = true;
      this.labelUserNameValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelUserNameValue.ForeColor = System.Drawing.SystemColors.ControlText;
      this.labelUserNameValue.Location = new System.Drawing.Point(209, 74);
      this.labelUserNameValue.Name = "labelUserNameValue";
      this.labelUserNameValue.Size = new System.Drawing.Size(166, 25);
      this.labelUserNameValue.TabIndex = 12;
      this.labelUserNameValue.Text = "User Name Value";
      // 
      // labelLastLogin
      // 
      this.labelLastLogin.AutoSize = true;
      this.labelLastLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelLastLogin.ForeColor = System.Drawing.SystemColors.ControlText;
      this.labelLastLogin.Location = new System.Drawing.Point(8, 377);
      this.labelLastLogin.Name = "labelLastLogin";
      this.labelLastLogin.Size = new System.Drawing.Size(112, 25);
      this.labelLastLogin.TabIndex = 8;
      this.labelLastLogin.Text = "Last Login";
      // 
      // labelFullNameValue
      // 
      this.labelFullNameValue.AutoSize = true;
      this.labelFullNameValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelFullNameValue.ForeColor = System.Drawing.SystemColors.ControlText;
      this.labelFullNameValue.Location = new System.Drawing.Point(209, 33);
      this.labelFullNameValue.Name = "labelFullNameValue";
      this.labelFullNameValue.Size = new System.Drawing.Size(156, 25);
      this.labelFullNameValue.TabIndex = 9;
      this.labelFullNameValue.Text = "Full Name Value";
      // 
      // ManageIndexForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(657, 473);
      this.ControlBox = false;
      this.Controls.Add(this.splitContainerMain);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.MaximumSize = new System.Drawing.Size(1600, 520);
      this.MinimumSize = new System.Drawing.Size(675, 520);
      this.Name = "ManageIndexForm";
      this.Text = "Manage Index Form";
      this.splitContainerMain.Panel1.ResumeLayout(false);
      this.splitContainerMain.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
      this.splitContainerMain.ResumeLayout(false);
      this.flowLayoutPanelHeader.ResumeLayout(false);
      this.flowLayoutPanelHeader.PerformLayout();
      this.groupBoxAccountManagement.ResumeLayout(false);
      this.groupBoxAccountManagement.PerformLayout();
      this.flowLayoutPanelDisplayName.ResumeLayout(false);
      this.flowLayoutPanelDisplayName.PerformLayout();
      this.flowLayoutPanelPassword.ResumeLayout(false);
      this.flowLayoutPanelPassword.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.SplitContainer splitContainerMain;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelHeader;
    private System.Windows.Forms.Button buttonClose;
    private System.Windows.Forms.Label labelTitle;
    private System.Windows.Forms.Label labelAction;
    private System.Windows.Forms.Label labelFullName;
    private System.Windows.Forms.Label labelDisplayName;
    private System.Windows.Forms.Label labelPassword;
    private System.Windows.Forms.Label labelLastLogin;
    private System.Windows.Forms.Label labelRegistrationDate;
    private System.Windows.Forms.Label labelTeam;
    private System.Windows.Forms.Label labelAdminRole;
    private System.Windows.Forms.Label labelWorkingRole;
    private System.Windows.Forms.Label labelUserName;
    private System.Windows.Forms.Label labelFullNameValue;
    private System.Windows.Forms.Label labelLastLoginValue;
    private System.Windows.Forms.Label labelRegistrationDateValue;
    private System.Windows.Forms.Label labelTeamValue;
    private System.Windows.Forms.Label labelAdminRoleValue;
    private System.Windows.Forms.Label labelWorkingRoleValue;
    private System.Windows.Forms.Label labelUserNameValue;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelDisplayName;
    private System.Windows.Forms.Label labelDisplayNameValue;
    private System.Windows.Forms.LinkLabel linkLabelDisplayNameChange;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelPassword;
    private System.Windows.Forms.Label labelPasswordValue;
    private System.Windows.Forms.LinkLabel linkLabelPasswordChange;
    private System.Windows.Forms.GroupBox groupBoxAccountManagement;
  }
}