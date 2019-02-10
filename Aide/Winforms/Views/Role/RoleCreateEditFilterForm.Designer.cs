namespace Aide.Winforms {
  partial class RoleCreateEditFilterForm {
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
      this.textBoxRoleName = new System.Windows.Forms.TextBox();
      this.labelRoleName = new System.Windows.Forms.Label();
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
      this.splitContainerMain.Size = new System.Drawing.Size(462, 169);
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
      this.flowLayoutPanelHeader.Size = new System.Drawing.Size(462, 55);
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
      this.splitContainerContent.Size = new System.Drawing.Size(462, 110);
      this.splitContainerContent.SplitterDistance = 53;
      this.splitContainerContent.SplitterWidth = 6;
      this.splitContainerContent.TabIndex = 2;
      // 
      // panelMain
      // 
      this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panelMain.Controls.Add(this.textBoxRoleName);
      this.panelMain.Controls.Add(this.labelRoleName);
      this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelMain.Location = new System.Drawing.Point(0, 0);
      this.panelMain.Name = "panelMain";
      this.panelMain.Size = new System.Drawing.Size(462, 53);
      this.panelMain.TabIndex = 0;
      // 
      // textBoxRoleName
      // 
      this.textBoxRoleName.Location = new System.Drawing.Point(126, 12);
      this.textBoxRoleName.Name = "textBoxRoleName";
      this.textBoxRoleName.Size = new System.Drawing.Size(323, 30);
      this.textBoxRoleName.TabIndex = 1;
      // 
      // labelRoleName
      // 
      this.labelRoleName.AutoSize = true;
      this.labelRoleName.Location = new System.Drawing.Point(11, 15);
      this.labelRoleName.Name = "labelRoleName";
      this.labelRoleName.Size = new System.Drawing.Size(64, 25);
      this.labelRoleName.TabIndex = 0;
      this.labelRoleName.Text = "Name";
      // 
      // buttonPerformAction
      // 
      this.buttonPerformAction.Dock = System.Windows.Forms.DockStyle.Fill;
      this.buttonPerformAction.Location = new System.Drawing.Point(0, 0);
      this.buttonPerformAction.Margin = new System.Windows.Forms.Padding(10, 15, 10, 3);
      this.buttonPerformAction.Name = "buttonPerformAction";
      this.buttonPerformAction.Size = new System.Drawing.Size(462, 51);
      this.buttonPerformAction.TabIndex = 4;
      this.buttonPerformAction.Text = "Perform Action";
      this.buttonPerformAction.UseVisualStyleBackColor = true;
      this.buttonPerformAction.Click += new System.EventHandler(this.buttonPerformAction_Click);
      // 
      // RoleCreateEditFilterForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(462, 169);
      this.ControlBox = false;
      this.Controls.Add(this.splitContainerMain);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.MaximumSize = new System.Drawing.Size(480, 216);
      this.MinimumSize = new System.Drawing.Size(480, 216);
      this.Name = "RoleCreateEditFilterForm";
      this.Text = "Role Create Edit Filter Form";
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
    private System.Windows.Forms.Label labelRoleName;
    private System.Windows.Forms.TextBox textBoxRoleName;
  }
}