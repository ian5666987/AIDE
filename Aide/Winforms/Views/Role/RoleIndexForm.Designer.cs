namespace Aide.Winforms {
  partial class RoleIndexForm {
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
      this.buttonCreate = new System.Windows.Forms.Button();
      this.buttonFilter = new System.Windows.Forms.Button();
      this.labelFilterMessage = new System.Windows.Forms.Label();
      this.splitContainerContent = new System.Windows.Forms.SplitContainer();
      this.splitContainerContentCore = new System.Windows.Forms.SplitContainer();
      this.dataGridViewTable = new System.Windows.Forms.DataGridView();
      this.flowLayoutPanelTableActions = new System.Windows.Forms.FlowLayoutPanel();
      this.flowLayoutPanelFooter = new System.Windows.Forms.FlowLayoutPanel();
      this.linkLabelFirst = new System.Windows.Forms.LinkLabel();
      this.linkLabelPrev100 = new System.Windows.Forms.LinkLabel();
      this.linkLabelPrev10 = new System.Windows.Forms.LinkLabel();
      this.linkLabelPrev = new System.Windows.Forms.LinkLabel();
      this.linkLabelNext = new System.Windows.Forms.LinkLabel();
      this.linkLabelNext10 = new System.Windows.Forms.LinkLabel();
      this.linkLabelNext100 = new System.Windows.Forms.LinkLabel();
      this.linkLabelLast = new System.Windows.Forms.LinkLabel();
      this.labelNavDataValue = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
      this.splitContainerMain.Panel1.SuspendLayout();
      this.splitContainerMain.Panel2.SuspendLayout();
      this.splitContainerMain.SuspendLayout();
      this.flowLayoutPanelHeader.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainerContent)).BeginInit();
      this.splitContainerContent.Panel1.SuspendLayout();
      this.splitContainerContent.Panel2.SuspendLayout();
      this.splitContainerContent.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainerContentCore)).BeginInit();
      this.splitContainerContentCore.Panel1.SuspendLayout();
      this.splitContainerContentCore.Panel2.SuspendLayout();
      this.splitContainerContentCore.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTable)).BeginInit();
      this.flowLayoutPanelFooter.SuspendLayout();
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
      this.splitContainerMain.Size = new System.Drawing.Size(782, 262);
      this.splitContainerMain.SplitterDistance = 54;
      this.splitContainerMain.TabIndex = 0;
      // 
      // flowLayoutPanelHeader
      // 
      this.flowLayoutPanelHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.flowLayoutPanelHeader.Controls.Add(this.buttonClose);
      this.flowLayoutPanelHeader.Controls.Add(this.labelTitle);
      this.flowLayoutPanelHeader.Controls.Add(this.labelAction);
      this.flowLayoutPanelHeader.Controls.Add(this.buttonCreate);
      this.flowLayoutPanelHeader.Controls.Add(this.buttonFilter);
      this.flowLayoutPanelHeader.Controls.Add(this.labelFilterMessage);
      this.flowLayoutPanelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flowLayoutPanelHeader.Location = new System.Drawing.Point(0, 0);
      this.flowLayoutPanelHeader.Name = "flowLayoutPanelHeader";
      this.flowLayoutPanelHeader.Size = new System.Drawing.Size(782, 54);
      this.flowLayoutPanelHeader.TabIndex = 3;
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
      // buttonCreate
      // 
      this.buttonCreate.Location = new System.Drawing.Point(359, 5);
      this.buttonCreate.Margin = new System.Windows.Forms.Padding(10, 5, 10, 3);
      this.buttonCreate.Name = "buttonCreate";
      this.buttonCreate.Size = new System.Drawing.Size(106, 42);
      this.buttonCreate.TabIndex = 4;
      this.buttonCreate.Text = "Create";
      this.buttonCreate.UseVisualStyleBackColor = true;
      this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
      // 
      // buttonFilter
      // 
      this.buttonFilter.Location = new System.Drawing.Point(485, 5);
      this.buttonFilter.Margin = new System.Windows.Forms.Padding(10, 5, 10, 3);
      this.buttonFilter.Name = "buttonFilter";
      this.buttonFilter.Size = new System.Drawing.Size(100, 42);
      this.buttonFilter.TabIndex = 4;
      this.buttonFilter.Text = "Filter";
      this.buttonFilter.UseVisualStyleBackColor = true;
      this.buttonFilter.Click += new System.EventHandler(this.buttonFilter_Click);
      // 
      // labelFilterMessage
      // 
      this.labelFilterMessage.AutoSize = true;
      this.labelFilterMessage.ForeColor = System.Drawing.Color.DarkGreen;
      this.labelFilterMessage.Location = new System.Drawing.Point(598, 17);
      this.labelFilterMessage.Margin = new System.Windows.Forms.Padding(3, 17, 3, 0);
      this.labelFilterMessage.Name = "labelFilterMessage";
      this.labelFilterMessage.Size = new System.Drawing.Size(128, 25);
      this.labelFilterMessage.TabIndex = 5;
      this.labelFilterMessage.Text = "filterMessage";
      // 
      // splitContainerContent
      // 
      this.splitContainerContent.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainerContent.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
      this.splitContainerContent.IsSplitterFixed = true;
      this.splitContainerContent.Location = new System.Drawing.Point(0, 0);
      this.splitContainerContent.Name = "splitContainerContent";
      this.splitContainerContent.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitContainerContent.Panel1
      // 
      this.splitContainerContent.Panel1.Controls.Add(this.splitContainerContentCore);
      // 
      // splitContainerContent.Panel2
      // 
      this.splitContainerContent.Panel2.Controls.Add(this.flowLayoutPanelFooter);
      this.splitContainerContent.Size = new System.Drawing.Size(782, 204);
      this.splitContainerContent.SplitterDistance = 164;
      this.splitContainerContent.TabIndex = 0;
      // 
      // splitContainerContentCore
      // 
      this.splitContainerContentCore.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainerContentCore.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
      this.splitContainerContentCore.IsSplitterFixed = true;
      this.splitContainerContentCore.Location = new System.Drawing.Point(0, 0);
      this.splitContainerContentCore.Name = "splitContainerContentCore";
      this.splitContainerContentCore.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitContainerContentCore.Panel1
      // 
      this.splitContainerContentCore.Panel1.Controls.Add(this.dataGridViewTable);
      // 
      // splitContainerContentCore.Panel2
      // 
      this.splitContainerContentCore.Panel2.Controls.Add(this.flowLayoutPanelTableActions);
      this.splitContainerContentCore.Size = new System.Drawing.Size(782, 164);
      this.splitContainerContentCore.SplitterDistance = 124;
      this.splitContainerContentCore.TabIndex = 1;
      // 
      // dataGridViewTable
      // 
      this.dataGridViewTable.AllowUserToAddRows = false;
      this.dataGridViewTable.AllowUserToDeleteRows = false;
      this.dataGridViewTable.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dataGridViewTable.Location = new System.Drawing.Point(0, 0);
      this.dataGridViewTable.Name = "dataGridViewTable";
      this.dataGridViewTable.ReadOnly = true;
      this.dataGridViewTable.RowTemplate.Height = 24;
      this.dataGridViewTable.Size = new System.Drawing.Size(782, 124);
      this.dataGridViewTable.TabIndex = 0;
      // 
      // flowLayoutPanelTableActions
      // 
      this.flowLayoutPanelTableActions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.flowLayoutPanelTableActions.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flowLayoutPanelTableActions.Location = new System.Drawing.Point(0, 0);
      this.flowLayoutPanelTableActions.Name = "flowLayoutPanelTableActions";
      this.flowLayoutPanelTableActions.Size = new System.Drawing.Size(782, 36);
      this.flowLayoutPanelTableActions.TabIndex = 0;
      // 
      // flowLayoutPanelFooter
      // 
      this.flowLayoutPanelFooter.AutoSize = true;
      this.flowLayoutPanelFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.flowLayoutPanelFooter.Controls.Add(this.linkLabelFirst);
      this.flowLayoutPanelFooter.Controls.Add(this.linkLabelPrev100);
      this.flowLayoutPanelFooter.Controls.Add(this.linkLabelPrev10);
      this.flowLayoutPanelFooter.Controls.Add(this.linkLabelPrev);
      this.flowLayoutPanelFooter.Controls.Add(this.linkLabelNext);
      this.flowLayoutPanelFooter.Controls.Add(this.linkLabelNext10);
      this.flowLayoutPanelFooter.Controls.Add(this.linkLabelNext100);
      this.flowLayoutPanelFooter.Controls.Add(this.linkLabelLast);
      this.flowLayoutPanelFooter.Controls.Add(this.labelNavDataValue);
      this.flowLayoutPanelFooter.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flowLayoutPanelFooter.Location = new System.Drawing.Point(0, 0);
      this.flowLayoutPanelFooter.Name = "flowLayoutPanelFooter";
      this.flowLayoutPanelFooter.Size = new System.Drawing.Size(782, 36);
      this.flowLayoutPanelFooter.TabIndex = 0;
      // 
      // linkLabelFirst
      // 
      this.linkLabelFirst.AutoSize = true;
      this.linkLabelFirst.Location = new System.Drawing.Point(3, 5);
      this.linkLabelFirst.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
      this.linkLabelFirst.Name = "linkLabelFirst";
      this.linkLabelFirst.Size = new System.Drawing.Size(49, 25);
      this.linkLabelFirst.TabIndex = 13;
      this.linkLabelFirst.TabStop = true;
      this.linkLabelFirst.Text = "First";
      this.linkLabelFirst.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelFirst_LinkClicked);
      // 
      // linkLabelPrev100
      // 
      this.linkLabelPrev100.AutoSize = true;
      this.linkLabelPrev100.Location = new System.Drawing.Point(58, 5);
      this.linkLabelPrev100.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
      this.linkLabelPrev100.Name = "linkLabelPrev100";
      this.linkLabelPrev100.Size = new System.Drawing.Size(48, 25);
      this.linkLabelPrev100.TabIndex = 14;
      this.linkLabelPrev100.TabStop = true;
      this.linkLabelPrev100.Text = "<<<";
      this.linkLabelPrev100.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelPrev100_LinkClicked);
      // 
      // linkLabelPrev10
      // 
      this.linkLabelPrev10.AutoSize = true;
      this.linkLabelPrev10.Location = new System.Drawing.Point(112, 5);
      this.linkLabelPrev10.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
      this.linkLabelPrev10.Name = "linkLabelPrev10";
      this.linkLabelPrev10.Size = new System.Drawing.Size(36, 25);
      this.linkLabelPrev10.TabIndex = 15;
      this.linkLabelPrev10.TabStop = true;
      this.linkLabelPrev10.Text = "<<";
      this.linkLabelPrev10.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelPrev10_LinkClicked);
      // 
      // linkLabelPrev
      // 
      this.linkLabelPrev.AutoSize = true;
      this.linkLabelPrev.Location = new System.Drawing.Point(154, 5);
      this.linkLabelPrev.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
      this.linkLabelPrev.Name = "linkLabelPrev";
      this.linkLabelPrev.Size = new System.Drawing.Size(24, 25);
      this.linkLabelPrev.TabIndex = 16;
      this.linkLabelPrev.TabStop = true;
      this.linkLabelPrev.Text = "<";
      this.linkLabelPrev.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelPrev_LinkClicked);
      // 
      // linkLabelNext
      // 
      this.linkLabelNext.AutoSize = true;
      this.linkLabelNext.Location = new System.Drawing.Point(184, 5);
      this.linkLabelNext.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
      this.linkLabelNext.Name = "linkLabelNext";
      this.linkLabelNext.Size = new System.Drawing.Size(24, 25);
      this.linkLabelNext.TabIndex = 17;
      this.linkLabelNext.TabStop = true;
      this.linkLabelNext.Text = ">";
      this.linkLabelNext.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelNext_LinkClicked);
      // 
      // linkLabelNext10
      // 
      this.linkLabelNext10.AutoSize = true;
      this.linkLabelNext10.Location = new System.Drawing.Point(214, 5);
      this.linkLabelNext10.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
      this.linkLabelNext10.Name = "linkLabelNext10";
      this.linkLabelNext10.Size = new System.Drawing.Size(36, 25);
      this.linkLabelNext10.TabIndex = 18;
      this.linkLabelNext10.TabStop = true;
      this.linkLabelNext10.Text = ">>";
      this.linkLabelNext10.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelNext10_LinkClicked);
      // 
      // linkLabelNext100
      // 
      this.linkLabelNext100.AutoSize = true;
      this.linkLabelNext100.Location = new System.Drawing.Point(256, 5);
      this.linkLabelNext100.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
      this.linkLabelNext100.Name = "linkLabelNext100";
      this.linkLabelNext100.Size = new System.Drawing.Size(48, 25);
      this.linkLabelNext100.TabIndex = 19;
      this.linkLabelNext100.TabStop = true;
      this.linkLabelNext100.Text = ">>>";
      this.linkLabelNext100.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelNext100_LinkClicked);
      // 
      // linkLabelLast
      // 
      this.linkLabelLast.AutoSize = true;
      this.linkLabelLast.Location = new System.Drawing.Point(310, 5);
      this.linkLabelLast.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
      this.linkLabelLast.Name = "linkLabelLast";
      this.linkLabelLast.Size = new System.Drawing.Size(49, 25);
      this.linkLabelLast.TabIndex = 20;
      this.linkLabelLast.TabStop = true;
      this.linkLabelLast.Text = "Last";
      this.linkLabelLast.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelLast_LinkClicked);
      // 
      // labelNavDataValue
      // 
      this.labelNavDataValue.AutoSize = true;
      this.labelNavDataValue.ForeColor = System.Drawing.SystemColors.ControlText;
      this.labelNavDataValue.Location = new System.Drawing.Point(365, 5);
      this.labelNavDataValue.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
      this.labelNavDataValue.Name = "labelNavDataValue";
      this.labelNavDataValue.Size = new System.Drawing.Size(95, 25);
      this.labelNavDataValue.TabIndex = 12;
      this.labelNavDataValue.Text = "navValue";
      // 
      // RoleIndexForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(782, 262);
      this.ControlBox = false;
      this.Controls.Add(this.splitContainerMain);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Margin = new System.Windows.Forms.Padding(4);
      this.MinimumSize = new System.Drawing.Size(800, 280);
      this.Name = "RoleIndexForm";
      this.Text = "Role Index Form";
      this.Load += new System.EventHandler(this.RoleIndexForm_Load);
      this.splitContainerMain.Panel1.ResumeLayout(false);
      this.splitContainerMain.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
      this.splitContainerMain.ResumeLayout(false);
      this.flowLayoutPanelHeader.ResumeLayout(false);
      this.flowLayoutPanelHeader.PerformLayout();
      this.splitContainerContent.Panel1.ResumeLayout(false);
      this.splitContainerContent.Panel2.ResumeLayout(false);
      this.splitContainerContent.Panel2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainerContent)).EndInit();
      this.splitContainerContent.ResumeLayout(false);
      this.splitContainerContentCore.Panel1.ResumeLayout(false);
      this.splitContainerContentCore.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainerContentCore)).EndInit();
      this.splitContainerContentCore.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTable)).EndInit();
      this.flowLayoutPanelFooter.ResumeLayout(false);
      this.flowLayoutPanelFooter.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.SplitContainer splitContainerMain;
    private System.Windows.Forms.Label labelTitle;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelHeader;
    private System.Windows.Forms.Button buttonClose;
    private System.Windows.Forms.Button buttonFilter;
    private System.Windows.Forms.Label labelFilterMessage;
    private System.Windows.Forms.SplitContainer splitContainerContent;
    private System.Windows.Forms.Button buttonCreate;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelFooter;
    private System.Windows.Forms.Label labelNavDataValue;
    private System.Windows.Forms.DataGridView dataGridViewTable;
    private System.Windows.Forms.Label labelAction;
    private System.Windows.Forms.LinkLabel linkLabelFirst;
    private System.Windows.Forms.LinkLabel linkLabelPrev100;
    private System.Windows.Forms.LinkLabel linkLabelPrev10;
    private System.Windows.Forms.LinkLabel linkLabelPrev;
    private System.Windows.Forms.LinkLabel linkLabelNext;
    private System.Windows.Forms.LinkLabel linkLabelNext10;
    private System.Windows.Forms.LinkLabel linkLabelNext100;
    private System.Windows.Forms.LinkLabel linkLabelLast;
    private System.Windows.Forms.SplitContainer splitContainerContentCore;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelTableActions;
  }
}