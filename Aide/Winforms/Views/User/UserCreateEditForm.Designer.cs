namespace Aide.Winforms {
  partial class UserCreateEditForm {
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
      this.splitContainerMain.Size = new System.Drawing.Size(542, 463);
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
      this.flowLayoutPanelHeader.Size = new System.Drawing.Size(542, 55);
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
      this.splitContainerContent.Size = new System.Drawing.Size(542, 404);
      this.splitContainerContent.SplitterDistance = 333;
      this.splitContainerContent.SplitterWidth = 6;
      this.splitContainerContent.TabIndex = 2;
      // 
      // panelMain
      // 
      this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
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
      this.panelMain.Size = new System.Drawing.Size(542, 333);
      this.panelMain.TabIndex = 0;
      // 
      // textBoxUserName
      // 
      this.textBoxUserName.Location = new System.Drawing.Point(181, 12);
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
      this.labelAdminRole.Location = new System.Drawing.Point(11, 291);
      this.labelAdminRole.Name = "labelAdminRole";
      this.labelAdminRole.Size = new System.Drawing.Size(112, 25);
      this.labelAdminRole.TabIndex = 11;
      this.labelAdminRole.Text = "Admin Role";
      // 
      // comboBoxAdminRole
      // 
      this.comboBoxAdminRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxAdminRole.FormattingEnabled = true;
      this.comboBoxAdminRole.Location = new System.Drawing.Point(181, 288);
      this.comboBoxAdminRole.Name = "comboBoxAdminRole";
      this.comboBoxAdminRole.Size = new System.Drawing.Size(348, 33);
      this.comboBoxAdminRole.TabIndex = 7;
      // 
      // comboBoxWorkingRole
      // 
      this.comboBoxWorkingRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxWorkingRole.FormattingEnabled = true;
      this.comboBoxWorkingRole.Location = new System.Drawing.Point(181, 240);
      this.comboBoxWorkingRole.Name = "comboBoxWorkingRole";
      this.comboBoxWorkingRole.Size = new System.Drawing.Size(348, 33);
      this.comboBoxWorkingRole.TabIndex = 6;
      // 
      // comboBoxTeam
      // 
      this.comboBoxTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxTeam.FormattingEnabled = true;
      this.comboBoxTeam.Location = new System.Drawing.Point(181, 192);
      this.comboBoxTeam.Name = "comboBoxTeam";
      this.comboBoxTeam.Size = new System.Drawing.Size(348, 33);
      this.comboBoxTeam.TabIndex = 5;
      // 
      // labelWorkingRole
      // 
      this.labelWorkingRole.AutoSize = true;
      this.labelWorkingRole.Location = new System.Drawing.Point(11, 243);
      this.labelWorkingRole.Name = "labelWorkingRole";
      this.labelWorkingRole.Size = new System.Drawing.Size(129, 25);
      this.labelWorkingRole.TabIndex = 7;
      this.labelWorkingRole.Text = "Working Role";
      // 
      // labelTeam
      // 
      this.labelTeam.AutoSize = true;
      this.labelTeam.Location = new System.Drawing.Point(11, 195);
      this.labelTeam.Name = "labelTeam";
      this.labelTeam.Size = new System.Drawing.Size(63, 25);
      this.labelTeam.TabIndex = 6;
      this.labelTeam.Text = "Team";
      // 
      // textBoxEmail
      // 
      this.textBoxEmail.Location = new System.Drawing.Point(181, 147);
      this.textBoxEmail.Name = "textBoxEmail";
      this.textBoxEmail.Size = new System.Drawing.Size(348, 30);
      this.textBoxEmail.TabIndex = 4;
      // 
      // labelEmail
      // 
      this.labelEmail.AutoSize = true;
      this.labelEmail.Location = new System.Drawing.Point(11, 150);
      this.labelEmail.Name = "labelEmail";
      this.labelEmail.Size = new System.Drawing.Size(60, 25);
      this.labelEmail.TabIndex = 4;
      this.labelEmail.Text = "Email";
      // 
      // textBoxDisplayName
      // 
      this.textBoxDisplayName.Location = new System.Drawing.Point(181, 102);
      this.textBoxDisplayName.Name = "textBoxDisplayName";
      this.textBoxDisplayName.Size = new System.Drawing.Size(348, 30);
      this.textBoxDisplayName.TabIndex = 3;
      // 
      // labelDisplayName
      // 
      this.labelDisplayName.AutoSize = true;
      this.labelDisplayName.Location = new System.Drawing.Point(11, 105);
      this.labelDisplayName.Name = "labelDisplayName";
      this.labelDisplayName.Size = new System.Drawing.Size(133, 25);
      this.labelDisplayName.TabIndex = 2;
      this.labelDisplayName.Text = "Display Name";
      // 
      // textBoxFullName
      // 
      this.textBoxFullName.Location = new System.Drawing.Point(181, 57);
      this.textBoxFullName.Name = "textBoxFullName";
      this.textBoxFullName.Size = new System.Drawing.Size(348, 30);
      this.textBoxFullName.TabIndex = 2;
      // 
      // labelFullName
      // 
      this.labelFullName.AutoSize = true;
      this.labelFullName.Location = new System.Drawing.Point(11, 60);
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
      this.buttonPerformAction.Size = new System.Drawing.Size(542, 65);
      this.buttonPerformAction.TabIndex = 4;
      this.buttonPerformAction.Text = "Perform Action";
      this.buttonPerformAction.UseVisualStyleBackColor = true;
      this.buttonPerformAction.Click += new System.EventHandler(this.buttonPerformAction_Click);
      // 
      // UserCreateEditForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(542, 463);
      this.ControlBox = false;
      this.Controls.Add(this.splitContainerMain);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.MaximumSize = new System.Drawing.Size(560, 510);
      this.MinimumSize = new System.Drawing.Size(560, 510);
      this.Name = "UserCreateEditForm";
      this.Text = "User Create Edit Form";
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
  }
}