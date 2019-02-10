namespace Aide.Winforms {
  partial class UserFilterForm {
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
      this.splitContainerContent = new System.Windows.Forms.SplitContainer();
      this.panelMain = new System.Windows.Forms.Panel();
      this.linkLabelLastLoginToReset = new System.Windows.Forms.LinkLabel();
      this.linkLabelLastLoginFromReset = new System.Windows.Forms.LinkLabel();
      this.linkLabelRegistrationDateToReset = new System.Windows.Forms.LinkLabel();
      this.linkLabelRegistrationDateFromReset = new System.Windows.Forms.LinkLabel();
      this.dateTimePickerLastLoginTo = new System.Windows.Forms.DateTimePicker();
      this.dateTimePickerLastLoginFrom = new System.Windows.Forms.DateTimePicker();
      this.dateTimePickerRegistrationDateTo = new System.Windows.Forms.DateTimePicker();
      this.labelLastLoginTo = new System.Windows.Forms.Label();
      this.labelLastLoginFrom = new System.Windows.Forms.Label();
      this.labelRegistrationDateTo = new System.Windows.Forms.Label();
      this.labelRegistrationDateFrom = new System.Windows.Forms.Label();
      this.dateTimePickerRegistrationDateFrom = new System.Windows.Forms.DateTimePicker();
      this.textBoxUserName = new System.Windows.Forms.TextBox();
      this.labelUserName = new System.Windows.Forms.Label();
      this.labelAdminRole = new System.Windows.Forms.Label();
      this.comboBoxAdminRole = new System.Windows.Forms.ComboBox();
      this.comboBoxWorkingRole = new System.Windows.Forms.ComboBox();
      this.comboBoxTeam = new System.Windows.Forms.ComboBox();
      this.labelWorkingRole = new System.Windows.Forms.Label();
      this.labelTeam = new System.Windows.Forms.Label();
      this.textBoxEmail = new System.Windows.Forms.TextBox();
      this.labelEmail = new System.Windows.Forms.Label();
      this.textBoxDisplayName = new System.Windows.Forms.TextBox();
      this.labelDisplayName = new System.Windows.Forms.Label();
      this.textBoxFullName = new System.Windows.Forms.TextBox();
      this.labelFullName = new System.Windows.Forms.Label();
      this.buttonPerformAction = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
      this.splitContainerMain.Panel1.SuspendLayout();
      this.splitContainerMain.Panel2.SuspendLayout();
      this.splitContainerMain.SuspendLayout();
      this.flowLayoutPanelHeader.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainerContent)).BeginInit();
      this.splitContainerContent.Panel1.SuspendLayout();
      this.splitContainerContent.Panel2.SuspendLayout();
      this.splitContainerContent.SuspendLayout();
      this.panelMain.SuspendLayout();
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
      this.splitContainerMain.Panel2.Controls.Add(this.splitContainerContent);
      this.splitContainerMain.Size = new System.Drawing.Size(657, 638);
      this.splitContainerMain.SplitterDistance = 55;
      this.splitContainerMain.TabIndex = 5;
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
      this.flowLayoutPanelHeader.Size = new System.Drawing.Size(657, 55);
      this.flowLayoutPanelHeader.TabIndex = 4;
      // 
      // buttonClose
      // 
      this.buttonClose.Location = new System.Drawing.Point(10, 5);
      this.buttonClose.Margin = new System.Windows.Forms.Padding(10, 5, 10, 3);
      this.buttonClose.Name = "buttonClose";
      this.buttonClose.Size = new System.Drawing.Size(99, 42);
      this.buttonClose.TabIndex = 17;
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
      this.labelAction.TabIndex = 7;
      this.labelAction.Text = "labelAction";
      // 
      // splitContainerContent
      // 
      this.splitContainerContent.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainerContent.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
      this.splitContainerContent.IsSplitterFixed = true;
      this.splitContainerContent.Location = new System.Drawing.Point(0, 0);
      this.splitContainerContent.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.splitContainerContent.Name = "splitContainerContent";
      this.splitContainerContent.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitContainerContent.Panel1
      // 
      this.splitContainerContent.Panel1.Controls.Add(this.panelMain);
      // 
      // splitContainerContent.Panel2
      // 
      this.splitContainerContent.Panel2.Controls.Add(this.buttonPerformAction);
      this.splitContainerContent.Size = new System.Drawing.Size(657, 579);
      this.splitContainerContent.SplitterDistance = 504;
      this.splitContainerContent.SplitterWidth = 6;
      this.splitContainerContent.TabIndex = 2;
      // 
      // panelMain
      // 
      this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panelMain.Controls.Add(this.linkLabelLastLoginToReset);
      this.panelMain.Controls.Add(this.linkLabelLastLoginFromReset);
      this.panelMain.Controls.Add(this.linkLabelRegistrationDateToReset);
      this.panelMain.Controls.Add(this.linkLabelRegistrationDateFromReset);
      this.panelMain.Controls.Add(this.dateTimePickerLastLoginTo);
      this.panelMain.Controls.Add(this.dateTimePickerLastLoginFrom);
      this.panelMain.Controls.Add(this.dateTimePickerRegistrationDateTo);
      this.panelMain.Controls.Add(this.labelLastLoginTo);
      this.panelMain.Controls.Add(this.labelLastLoginFrom);
      this.panelMain.Controls.Add(this.labelRegistrationDateTo);
      this.panelMain.Controls.Add(this.labelRegistrationDateFrom);
      this.panelMain.Controls.Add(this.dateTimePickerRegistrationDateFrom);
      this.panelMain.Controls.Add(this.textBoxUserName);
      this.panelMain.Controls.Add(this.labelUserName);
      this.panelMain.Controls.Add(this.labelAdminRole);
      this.panelMain.Controls.Add(this.comboBoxAdminRole);
      this.panelMain.Controls.Add(this.comboBoxWorkingRole);
      this.panelMain.Controls.Add(this.comboBoxTeam);
      this.panelMain.Controls.Add(this.labelWorkingRole);
      this.panelMain.Controls.Add(this.labelTeam);
      this.panelMain.Controls.Add(this.textBoxEmail);
      this.panelMain.Controls.Add(this.labelEmail);
      this.panelMain.Controls.Add(this.textBoxDisplayName);
      this.panelMain.Controls.Add(this.labelDisplayName);
      this.panelMain.Controls.Add(this.textBoxFullName);
      this.panelMain.Controls.Add(this.labelFullName);
      this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelMain.Location = new System.Drawing.Point(0, 0);
      this.panelMain.Name = "panelMain";
      this.panelMain.Size = new System.Drawing.Size(657, 504);
      this.panelMain.TabIndex = 0;
      // 
      // linkLabelLastLoginToReset
      // 
      this.linkLabelLastLoginToReset.AutoSize = true;
      this.linkLabelLastLoginToReset.Location = new System.Drawing.Point(573, 466);
      this.linkLabelLastLoginToReset.Name = "linkLabelLastLoginToReset";
      this.linkLabelLastLoginToReset.Size = new System.Drawing.Size(62, 25);
      this.linkLabelLastLoginToReset.TabIndex = 15;
      this.linkLabelLastLoginToReset.TabStop = true;
      this.linkLabelLastLoginToReset.Text = "Reset";
      this.linkLabelLastLoginToReset.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelLastLoginToReset_LinkClicked);
      // 
      // linkLabelLastLoginFromReset
      // 
      this.linkLabelLastLoginFromReset.AutoSize = true;
      this.linkLabelLastLoginFromReset.Location = new System.Drawing.Point(573, 422);
      this.linkLabelLastLoginFromReset.Name = "linkLabelLastLoginFromReset";
      this.linkLabelLastLoginFromReset.Size = new System.Drawing.Size(62, 25);
      this.linkLabelLastLoginFromReset.TabIndex = 13;
      this.linkLabelLastLoginFromReset.TabStop = true;
      this.linkLabelLastLoginFromReset.Text = "Reset";
      this.linkLabelLastLoginFromReset.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelLastLoginFromReset_LinkClicked);
      // 
      // linkLabelRegistrationDateToReset
      // 
      this.linkLabelRegistrationDateToReset.AutoSize = true;
      this.linkLabelRegistrationDateToReset.Location = new System.Drawing.Point(573, 378);
      this.linkLabelRegistrationDateToReset.Name = "linkLabelRegistrationDateToReset";
      this.linkLabelRegistrationDateToReset.Size = new System.Drawing.Size(62, 25);
      this.linkLabelRegistrationDateToReset.TabIndex = 11;
      this.linkLabelRegistrationDateToReset.TabStop = true;
      this.linkLabelRegistrationDateToReset.Text = "Reset";
      this.linkLabelRegistrationDateToReset.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelRegistrationDateToReset_LinkClicked);
      // 
      // linkLabelRegistrationDateFromReset
      // 
      this.linkLabelRegistrationDateFromReset.AutoSize = true;
      this.linkLabelRegistrationDateFromReset.Location = new System.Drawing.Point(573, 334);
      this.linkLabelRegistrationDateFromReset.Name = "linkLabelRegistrationDateFromReset";
      this.linkLabelRegistrationDateFromReset.Size = new System.Drawing.Size(62, 25);
      this.linkLabelRegistrationDateFromReset.TabIndex = 9;
      this.linkLabelRegistrationDateFromReset.TabStop = true;
      this.linkLabelRegistrationDateFromReset.Text = "Reset";
      this.linkLabelRegistrationDateFromReset.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelRegistrationDateFromReset_LinkClicked);
      // 
      // dateTimePickerLastLoginTo
      // 
      this.dateTimePickerLastLoginTo.CustomFormat = "dd-MMM-yyyy HH:mm:ss";
      this.dateTimePickerLastLoginTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
      this.dateTimePickerLastLoginTo.Location = new System.Drawing.Point(287, 461);
      this.dateTimePickerLastLoginTo.Name = "dateTimePickerLastLoginTo";
      this.dateTimePickerLastLoginTo.Size = new System.Drawing.Size(272, 30);
      this.dateTimePickerLastLoginTo.TabIndex = 14;
      // 
      // dateTimePickerLastLoginFrom
      // 
      this.dateTimePickerLastLoginFrom.CustomFormat = "dd-MMM-yyyy HH:mm:ss";
      this.dateTimePickerLastLoginFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
      this.dateTimePickerLastLoginFrom.Location = new System.Drawing.Point(287, 417);
      this.dateTimePickerLastLoginFrom.Name = "dateTimePickerLastLoginFrom";
      this.dateTimePickerLastLoginFrom.Size = new System.Drawing.Size(272, 30);
      this.dateTimePickerLastLoginFrom.TabIndex = 12;
      // 
      // dateTimePickerRegistrationDateTo
      // 
      this.dateTimePickerRegistrationDateTo.CustomFormat = "dd-MMM-yyyy HH:mm:ss";
      this.dateTimePickerRegistrationDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
      this.dateTimePickerRegistrationDateTo.Location = new System.Drawing.Point(287, 373);
      this.dateTimePickerRegistrationDateTo.Name = "dateTimePickerRegistrationDateTo";
      this.dateTimePickerRegistrationDateTo.Size = new System.Drawing.Size(272, 30);
      this.dateTimePickerRegistrationDateTo.TabIndex = 10;
      // 
      // labelLastLoginTo
      // 
      this.labelLastLoginTo.AutoSize = true;
      this.labelLastLoginTo.Location = new System.Drawing.Point(11, 466);
      this.labelLastLoginTo.Name = "labelLastLoginTo";
      this.labelLastLoginTo.Size = new System.Drawing.Size(145, 25);
      this.labelLastLoginTo.TabIndex = 18;
      this.labelLastLoginTo.Text = "Last Login (To)";
      // 
      // labelLastLoginFrom
      // 
      this.labelLastLoginFrom.AutoSize = true;
      this.labelLastLoginFrom.Location = new System.Drawing.Point(11, 422);
      this.labelLastLoginFrom.Name = "labelLastLoginFrom";
      this.labelLastLoginFrom.Size = new System.Drawing.Size(166, 25);
      this.labelLastLoginFrom.TabIndex = 17;
      this.labelLastLoginFrom.Text = "Last Login (From)";
      // 
      // labelRegistrationDateTo
      // 
      this.labelRegistrationDateTo.AutoSize = true;
      this.labelRegistrationDateTo.Location = new System.Drawing.Point(11, 378);
      this.labelRegistrationDateTo.Name = "labelRegistrationDateTo";
      this.labelRegistrationDateTo.Size = new System.Drawing.Size(203, 25);
      this.labelRegistrationDateTo.TabIndex = 16;
      this.labelRegistrationDateTo.Text = "Registration Date (To)";
      // 
      // labelRegistrationDateFrom
      // 
      this.labelRegistrationDateFrom.AutoSize = true;
      this.labelRegistrationDateFrom.Location = new System.Drawing.Point(11, 334);
      this.labelRegistrationDateFrom.Name = "labelRegistrationDateFrom";
      this.labelRegistrationDateFrom.Size = new System.Drawing.Size(224, 25);
      this.labelRegistrationDateFrom.TabIndex = 15;
      this.labelRegistrationDateFrom.Text = "Registration Date (From)";
      // 
      // dateTimePickerRegistrationDateFrom
      // 
      this.dateTimePickerRegistrationDateFrom.CustomFormat = "dd-MMM-yyyy HH:mm:ss";
      this.dateTimePickerRegistrationDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
      this.dateTimePickerRegistrationDateFrom.Location = new System.Drawing.Point(287, 329);
      this.dateTimePickerRegistrationDateFrom.Name = "dateTimePickerRegistrationDateFrom";
      this.dateTimePickerRegistrationDateFrom.Size = new System.Drawing.Size(272, 30);
      this.dateTimePickerRegistrationDateFrom.TabIndex = 8;
      // 
      // textBoxUserName
      // 
      this.textBoxUserName.Location = new System.Drawing.Point(287, 12);
      this.textBoxUserName.Name = "textBoxUserName";
      this.textBoxUserName.Size = new System.Drawing.Size(348, 30);
      this.textBoxUserName.TabIndex = 1;
      // 
      // labelUserName
      // 
      this.labelUserName.AutoSize = true;
      this.labelUserName.Location = new System.Drawing.Point(11, 15);
      this.labelUserName.Name = "labelUserName";
      this.labelUserName.Size = new System.Drawing.Size(110, 25);
      this.labelUserName.TabIndex = 12;
      this.labelUserName.Text = "User Name";
      // 
      // labelAdminRole
      // 
      this.labelAdminRole.AutoSize = true;
      this.labelAdminRole.Location = new System.Drawing.Point(11, 285);
      this.labelAdminRole.Name = "labelAdminRole";
      this.labelAdminRole.Size = new System.Drawing.Size(112, 25);
      this.labelAdminRole.TabIndex = 11;
      this.labelAdminRole.Text = "Admin Role";
      // 
      // comboBoxAdminRole
      // 
      this.comboBoxAdminRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxAdminRole.FormattingEnabled = true;
      this.comboBoxAdminRole.Location = new System.Drawing.Point(287, 282);
      this.comboBoxAdminRole.Name = "comboBoxAdminRole";
      this.comboBoxAdminRole.Size = new System.Drawing.Size(348, 33);
      this.comboBoxAdminRole.TabIndex = 7;
      // 
      // comboBoxWorkingRole
      // 
      this.comboBoxWorkingRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxWorkingRole.FormattingEnabled = true;
      this.comboBoxWorkingRole.Location = new System.Drawing.Point(287, 235);
      this.comboBoxWorkingRole.Name = "comboBoxWorkingRole";
      this.comboBoxWorkingRole.Size = new System.Drawing.Size(348, 33);
      this.comboBoxWorkingRole.TabIndex = 6;
      // 
      // comboBoxTeam
      // 
      this.comboBoxTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxTeam.FormattingEnabled = true;
      this.comboBoxTeam.Location = new System.Drawing.Point(287, 188);
      this.comboBoxTeam.Name = "comboBoxTeam";
      this.comboBoxTeam.Size = new System.Drawing.Size(348, 33);
      this.comboBoxTeam.TabIndex = 5;
      // 
      // labelWorkingRole
      // 
      this.labelWorkingRole.AutoSize = true;
      this.labelWorkingRole.Location = new System.Drawing.Point(11, 238);
      this.labelWorkingRole.Name = "labelWorkingRole";
      this.labelWorkingRole.Size = new System.Drawing.Size(129, 25);
      this.labelWorkingRole.TabIndex = 7;
      this.labelWorkingRole.Text = "Working Role";
      // 
      // labelTeam
      // 
      this.labelTeam.AutoSize = true;
      this.labelTeam.Location = new System.Drawing.Point(11, 191);
      this.labelTeam.Name = "labelTeam";
      this.labelTeam.Size = new System.Drawing.Size(63, 25);
      this.labelTeam.TabIndex = 6;
      this.labelTeam.Text = "Team";
      // 
      // textBoxEmail
      // 
      this.textBoxEmail.Location = new System.Drawing.Point(287, 144);
      this.textBoxEmail.Name = "textBoxEmail";
      this.textBoxEmail.Size = new System.Drawing.Size(348, 30);
      this.textBoxEmail.TabIndex = 4;
      // 
      // labelEmail
      // 
      this.labelEmail.AutoSize = true;
      this.labelEmail.Location = new System.Drawing.Point(11, 147);
      this.labelEmail.Name = "labelEmail";
      this.labelEmail.Size = new System.Drawing.Size(60, 25);
      this.labelEmail.TabIndex = 4;
      this.labelEmail.Text = "Email";
      // 
      // textBoxDisplayName
      // 
      this.textBoxDisplayName.Location = new System.Drawing.Point(287, 100);
      this.textBoxDisplayName.Name = "textBoxDisplayName";
      this.textBoxDisplayName.Size = new System.Drawing.Size(348, 30);
      this.textBoxDisplayName.TabIndex = 3;
      // 
      // labelDisplayName
      // 
      this.labelDisplayName.AutoSize = true;
      this.labelDisplayName.Location = new System.Drawing.Point(11, 103);
      this.labelDisplayName.Name = "labelDisplayName";
      this.labelDisplayName.Size = new System.Drawing.Size(133, 25);
      this.labelDisplayName.TabIndex = 2;
      this.labelDisplayName.Text = "Display Name";
      // 
      // textBoxFullName
      // 
      this.textBoxFullName.Location = new System.Drawing.Point(287, 56);
      this.textBoxFullName.Name = "textBoxFullName";
      this.textBoxFullName.Size = new System.Drawing.Size(348, 30);
      this.textBoxFullName.TabIndex = 2;
      // 
      // labelFullName
      // 
      this.labelFullName.AutoSize = true;
      this.labelFullName.Location = new System.Drawing.Point(11, 56);
      this.labelFullName.Name = "labelFullName";
      this.labelFullName.Size = new System.Drawing.Size(100, 25);
      this.labelFullName.TabIndex = 0;
      this.labelFullName.Text = "Full Name";
      // 
      // buttonPerformAction
      // 
      this.buttonPerformAction.Dock = System.Windows.Forms.DockStyle.Fill;
      this.buttonPerformAction.Location = new System.Drawing.Point(0, 0);
      this.buttonPerformAction.Margin = new System.Windows.Forms.Padding(10, 15, 10, 3);
      this.buttonPerformAction.Name = "buttonPerformAction";
      this.buttonPerformAction.Size = new System.Drawing.Size(657, 69);
      this.buttonPerformAction.TabIndex = 16;
      this.buttonPerformAction.Text = "Perform Action";
      this.buttonPerformAction.UseVisualStyleBackColor = true;
      this.buttonPerformAction.Click += new System.EventHandler(this.buttonPerformAction_Click);
      // 
      // UserFilterForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(657, 638);
      this.ControlBox = false;
      this.Controls.Add(this.splitContainerMain);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.MaximumSize = new System.Drawing.Size(675, 685);
      this.MinimumSize = new System.Drawing.Size(675, 685);
      this.Name = "UserFilterForm";
      this.Text = "User Filter Form";
      this.splitContainerMain.Panel1.ResumeLayout(false);
      this.splitContainerMain.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
      this.splitContainerMain.ResumeLayout(false);
      this.flowLayoutPanelHeader.ResumeLayout(false);
      this.flowLayoutPanelHeader.PerformLayout();
      this.splitContainerContent.Panel1.ResumeLayout(false);
      this.splitContainerContent.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainerContent)).EndInit();
      this.splitContainerContent.ResumeLayout(false);
      this.panelMain.ResumeLayout(false);
      this.panelMain.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.SplitContainer splitContainerMain;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelHeader;
    private System.Windows.Forms.Button buttonClose;
    private System.Windows.Forms.Label labelTitle;
    private System.Windows.Forms.SplitContainer splitContainerContent;
    private System.Windows.Forms.Button buttonPerformAction;
    private System.Windows.Forms.Label labelAction;
    private System.Windows.Forms.Panel panelMain;
    private System.Windows.Forms.Label labelFullName;
    private System.Windows.Forms.TextBox textBoxFullName;
    private System.Windows.Forms.Label labelDisplayName;
    private System.Windows.Forms.TextBox textBoxDisplayName;
    private System.Windows.Forms.TextBox textBoxEmail;
    private System.Windows.Forms.Label labelEmail;
    private System.Windows.Forms.Label labelTeam;
    private System.Windows.Forms.ComboBox comboBoxTeam;
    private System.Windows.Forms.Label labelWorkingRole;
    private System.Windows.Forms.ComboBox comboBoxWorkingRole;
    private System.Windows.Forms.ComboBox comboBoxAdminRole;
    private System.Windows.Forms.Label labelAdminRole;
    private System.Windows.Forms.Label labelUserName;
    private System.Windows.Forms.TextBox textBoxUserName;
    private System.Windows.Forms.DateTimePicker dateTimePickerRegistrationDateFrom;
    private System.Windows.Forms.Label labelRegistrationDateTo;
    private System.Windows.Forms.Label labelRegistrationDateFrom;
    private System.Windows.Forms.Label labelLastLoginTo;
    private System.Windows.Forms.Label labelLastLoginFrom;
    private System.Windows.Forms.DateTimePicker dateTimePickerRegistrationDateTo;
    private System.Windows.Forms.DateTimePicker dateTimePickerLastLoginTo;
    private System.Windows.Forms.DateTimePicker dateTimePickerLastLoginFrom;
    private System.Windows.Forms.LinkLabel linkLabelRegistrationDateToReset;
    private System.Windows.Forms.LinkLabel linkLabelRegistrationDateFromReset;
    private System.Windows.Forms.LinkLabel linkLabelLastLoginFromReset;
    private System.Windows.Forms.LinkLabel linkLabelLastLoginToReset;
  }
}