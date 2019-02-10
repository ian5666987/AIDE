using Aide.Logics;
using Aide.Models;
using Aide.Models.Accounts;
using Aide.Winforms.Helpers;
using Aide.Winforms.Models;
using Extension.String;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using AWF = Aide.Winforms.SH;

namespace Aide.Winforms {
  public partial class UserIndexForm : Form {
    public int TotalWidth { get; set; }
    public int TotalHeight { get; set; }
    public AideUserFilterIndexModel Model = new AideUserFilterIndexModel(); //the only thing needs to be in the form, the model
    public UserIndexForm() {
      InitializeComponent();
      localization();
      initialization();
    }

    private void localization() {
      Text = Aibe.LCZ.W_User;
      buttonClose.Text = Aibe.LCZ.W_Close;
      buttonCreate.Text = Aibe.LCZ.W_Create;
      buttonFilter.Text = Aibe.LCZ.W_Filter;
      linkLabelFirst.Text = Aibe.LCZ.W_First;
      linkLabelLast.Text = Aibe.LCZ.W_Last;
      linkLabelNext.Text = Aibe.LCZ.W_NextSymbol;
      linkLabelPrev.Text = Aibe.LCZ.W_PreviousSymbol;
      base.Text = Aibe.LCZ.W_Index;
      labelTitle.Text = Aibe.LCZ.W_User;
      labelFilterMessage.Text = string.Empty; //emtifies first
      labelNavDataValue.Text = string.Empty;
      labelAction.Text = string.Concat("(", Aibe.LCZ.W_Index, ")");
    }

    private void initialization() {
      DataGridViewTextBoxColumn dgvNoColumn = new DataGridViewTextBoxColumn() { HeaderText = Aibe.LCZ.W_No, };
      DataGridViewTextBoxColumn dgvUserNameColumn = new DataGridViewTextBoxColumn() { HeaderText = Aibe.LCZ.T_UserNameColumnName, };
      DataGridViewTextBoxColumn dgvFullNameColumn = new DataGridViewTextBoxColumn() { HeaderText = Aibe.LCZ.T_UserFullNameColumnName, };
      DataGridViewTextBoxColumn dgvDisplayNameColumn = new DataGridViewTextBoxColumn() { HeaderText = Aibe.LCZ.T_UserDisplayNameColumnName, };
      DataGridViewTextBoxColumn dgvEmailColumn = new DataGridViewTextBoxColumn() { HeaderText = Aibe.LCZ.T_UserEmailColumnName, };
      DataGridViewTextBoxColumn dgvTeamColumn = new DataGridViewTextBoxColumn() { HeaderText = Aibe.LCZ.T_UserTeamColumnName, };
      DataGridViewTextBoxColumn dgvWorkingRoleColumn = new DataGridViewTextBoxColumn() { HeaderText = Aibe.LCZ.T_UserWorkingRoleColumnName, };
      DataGridViewTextBoxColumn dgvAdminRoleColumn = new DataGridViewTextBoxColumn() { HeaderText = Aibe.LCZ.T_UserAdminRoleColumnName, };
      DataGridViewTextBoxColumn dgvRegistrationDateColumn = new DataGridViewTextBoxColumn() { HeaderText = Aibe.LCZ.T_UserRegistrationDateColumnName, };
      DataGridViewTextBoxColumn dgvLastLoginColumn = new DataGridViewTextBoxColumn() { HeaderText = Aibe.LCZ.T_UserLastLoginColumnName, };
      DataGridViewButtonColumn editColumn = new DataGridViewButtonColumn();
      DataGridViewButtonColumn detailsColumn = new DataGridViewButtonColumn();
      DataGridViewButtonColumn deleteColumn = new DataGridViewButtonColumn();
      dataGridViewTable.Columns.Add(dgvNoColumn);
      dataGridViewTable.Columns.Add(dgvUserNameColumn);
      dataGridViewTable.Columns.Add(dgvFullNameColumn);
      dataGridViewTable.Columns.Add(dgvDisplayNameColumn);
      dataGridViewTable.Columns.Add(dgvEmailColumn);
      dataGridViewTable.Columns.Add(dgvTeamColumn);
      dataGridViewTable.Columns.Add(dgvWorkingRoleColumn);
      dataGridViewTable.Columns.Add(dgvAdminRoleColumn);
      dataGridViewTable.Columns.Add(dgvRegistrationDateColumn);
      dataGridViewTable.Columns.Add(dgvLastLoginColumn);
      dataGridViewTable.Columns.Add(editColumn); //for edit
      dataGridViewTable.Columns.Add(detailsColumn); //for details
      dataGridViewTable.Columns.Add(deleteColumn); //for delete

      refreshTable();

      dataGridViewTable.CellContentClick += DataGridViewTable_CellContentClick; //only initialized once
      dataGridViewTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dataGridViewTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      dataGridViewTable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
      splitContainerContentCore.Panel2Collapsed = true; //always collapse for now, because there isn't table actions
    }

    private void refreshTable() {
      dataGridViewTable.Rows.Clear();
      DataTable table = UserLogic.Index(ref Model);
      labelNavDataValue.Text = Aibe.LCZ.W_Page + " " + Model.NavData.CurrentPage + " / " + Model.NavData.MaxPage
        + ", " + Aibe.LCZ.W_Data + " " + Model.NavData.ItemNoInPageFirst + "-" +
        Model.NavData.ItemNoInPageLast + " " + Aibe.LCZ.W_of + " " + Model.NavData.QueryCount + " " + Aibe.LCZ.W_Data;
      labelFilterMessage.Text = Model.Filter.HasFilter() ? string.Concat("[", Model.FilterNo , "]") : string.Empty;
      int count = 0;
      foreach (DataRow row in table.Rows) {
        ++count;
        if (count < Model.NavData.ItemNoInPageFirst)
          continue;
        else if (count > Model.NavData.ItemNoInPageLast)
          break;
        object id = row[Aibe.DH.UserIdColumnName];
        object userName = row[Aibe.DH.UserNameColumnName];
        object fullName = row[Aibe.DH.UserFullNameColumnName];
        object displayName = row[Aibe.DH.UserDisplayNameColumnName];
        object email = row[Aibe.DH.UserEmailColumnName];
        object team = row[Aibe.DH.UserTeamColumnName];
        object workingRole = row[Aibe.DH.UserWorkingRoleColumnName];
        object adminRole = row[Aibe.DH.UserAdminRoleColumnName];
        object registrationDate = row[Aibe.DH.UserRegistrationDateColumnName];
        object lastLogin = row[Aibe.DH.UserLastLoginColumnName];

        bool isThisAdmin = id != null && 
          Identity.User != null && Identity.User.Id == id.ToString() && Identity.User.AdminRole.Equals(Aibe.DH.AdminRole) &&
          adminRole != null && adminRole.Equals(Aibe.DH.AdminRole);
        bool isMainAdmin = adminRole != null && adminRole.ToString().Equals(Aibe.DH.MainAdminRole);

        DataGridViewRow dgvRow = new DataGridViewRow();
        DataGridViewTextBoxCell noCell = new DataGridViewTextBoxCell { Value = count };
        DataGridViewTextBoxCell userNameCell = new DataGridViewTextBoxCell { Value = userName };
        DataGridViewTextBoxCell fullNameCell = new DataGridViewTextBoxCell { Value = fullName };
        DataGridViewTextBoxCell displayNameCell = new DataGridViewTextBoxCell { Value = displayName };
        DataGridViewTextBoxCell emailCell = new DataGridViewTextBoxCell { Value = email };
        DataGridViewTextBoxCell teamCell = new DataGridViewTextBoxCell { Value = team };
        DataGridViewTextBoxCell workingRoleCell = new DataGridViewTextBoxCell { Value = workingRole };
        DataGridViewTextBoxCell adminRoleCell = new DataGridViewTextBoxCell { Value = adminRole };
        DataGridViewTextBoxCell registrationDateCell = new DataGridViewTextBoxCell { Value = registrationDate is DBNull ? registrationDate : 
          ((DateTime)registrationDate).ToString(Aide.DH.DefaultDateTimeFormat) };
        DataGridViewTextBoxCell lastLoginCell = new DataGridViewTextBoxCell { Value = lastLogin is DBNull ? lastLogin :
          ((DateTime)lastLogin).ToString(Aide.DH.DefaultDateTimeFormat) };
        dgvRow.Cells.Add(noCell); //the number cell
        dgvRow.Cells.Add(userNameCell); //the user name cell
        dgvRow.Cells.Add(fullNameCell); //the full name cell
        dgvRow.Cells.Add(displayNameCell); //the display name cell
        dgvRow.Cells.Add(emailCell); //the email cell
        dgvRow.Cells.Add(teamCell); //the team cell
        dgvRow.Cells.Add(workingRoleCell); //the working role cell
        dgvRow.Cells.Add(adminRoleCell); //the admin role cell
        dgvRow.Cells.Add(registrationDateCell); //the registration date cell
        dgvRow.Cells.Add(lastLoginCell); //the last login cell

        if (!isMainAdmin && !isThisAdmin) {
          DataGridViewButtonCell btnEditCell = new DataGridViewButtonCell {
            Value = Aibe.LCZ.W_Edit,
            Tag = new UncommonButtonTag() { ActionName = Aibe.DH.EditActionName, StrId = id.ToString(), Item = row }
          };
          dgvRow.Cells.Add(btnEditCell); //the edit button cell
        } else {
          DataGridViewTextBoxCell btnEditCellDisabled = new DataGridViewTextBoxCell { Value = Aibe.LCZ.W_Edit, };
          dgvRow.Cells.Add(btnEditCellDisabled);
          btnEditCellDisabled.Style.ForeColor = Color.Gray;
        }

        DataGridViewButtonCell btnDetailsCell = new DataGridViewButtonCell {
          Value = Aibe.LCZ.W_Details,
          Tag = new UncommonButtonTag() { ActionName = Aibe.DH.DetailsActionName, StrId = id.ToString(), Item = row }
        };
        dgvRow.Cells.Add(btnDetailsCell); //the details button cell

        if (!isMainAdmin && !isThisAdmin) {
          DataGridViewButtonCell btnDeleteCell = new DataGridViewButtonCell {
            Value = Aibe.LCZ.W_Delete,
            Tag = new UncommonButtonTag() { ActionName = Aibe.DH.DeleteActionName, StrId = id.ToString(), Item = row }
          };
          dgvRow.Cells.Add(btnDeleteCell); //the delete button cell
        } else {
          DataGridViewTextBoxCell btnDeleteCellDisabled = new DataGridViewTextBoxCell { Value = Aibe.LCZ.W_Delete, };
          dgvRow.Cells.Add(btnDeleteCellDisabled);
          btnDeleteCellDisabled.Style.ForeColor = Color.Gray;
        }

        dataGridViewTable.Rows.Add(dgvRow);
      }
      visibility();
    }

    private void actionFilter() {
      UserFilterForm form = new UserFilterForm(Aibe.DH.FilterActionName, Model.Filter);
      if (DialogResult.OK == form.ShowDialog()) {
        Model.Filter = form.Filter;
        refreshTableWithUiUpdate();
      }
      form.Dispose();
      form = null;
    }

    private void actionCreate() {
      UserCreateEditForm form = new UserCreateEditForm(Aibe.DH.CreateActionName);
      if (DialogResult.OK == form.ShowDialog())
        refreshTableWithUiUpdate();
      form.Dispose();
      form = null;
    }

    private void actionEdit(string id, DataRow item) {
      UserCreateEditForm form = new UserCreateEditForm(Aibe.DH.EditActionName, id, item);
      if (DialogResult.OK == form.ShowDialog())
        refreshTableWithUiUpdate();
      form.Dispose();
      form = null;
    }

    private void actionDelete(string id, DataRow item) {
      UserDetailsForm form = new UserDetailsForm(Aibe.DH.DeleteActionName, id, item);
      if (DialogResult.OK == form.ShowDialog())
        refreshTableWithUiUpdate();
      form.Dispose();
      form = null;
    }

    private void actionDetails(string id, DataRow item) {
      UserDetailsForm form = new UserDetailsForm(Aibe.DH.DetailsActionName, id, item);
      form.ShowDialog();
      form.Dispose();
      form = null;
    }

    private void DataGridViewTable_CellContentClick(object sender, DataGridViewCellEventArgs e) {
      var senderGrid = (DataGridView)sender;
      if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
          e.RowIndex >= 0 && senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewButtonCell) {
        DataGridViewButtonCell dgvCell = (DataGridViewButtonCell)senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
        if (!(dgvCell.Tag is UncommonButtonTag))
          return;
        UncommonButtonTag tag = (UncommonButtonTag)dgvCell.Tag;
        if (tag.ActionName.EqualsIgnoreCase(Aibe.DH.EditActionName)) { //Do edit
          actionEdit(tag.StrId, (DataRow)tag.Item);
        } else if (tag.ActionName.EqualsIgnoreCase(Aibe.DH.DetailsActionName)) { //Do details
          actionDetails(tag.StrId, (DataRow)tag.Item);
        } else if (tag.ActionName.EqualsIgnoreCase(Aibe.DH.DeleteActionName)) { //Do delete
          actionDelete(tag.StrId, (DataRow)tag.Item);
        }
      }
    }

    private void visibility() {
      linkLabelNext100.Visible = Model.NavData.MaxPage > 100;
      linkLabelPrev100.Visible = Model.NavData.MaxPage > 100;
      linkLabelNext10.Visible = Model.NavData.MaxPage > 10;
      linkLabelPrev10.Visible = Model.NavData.MaxPage > 10;
    }

    private void refreshTableWithUiUpdate() {
      refreshTable();
      uiFinalTouch();
    }

    private void uiFinalTouch() {
      FunctionHelper.LockWindowUpdate(this.Handle);
      UiHelper.AdjustDgv(dataGridViewTable);
      TotalHeight = Math.Max(UiHelper.GetDgvRowsHeight(dataGridViewTable) + AWF.BaseIndexWindowsHeight,
        Size.Height);
      TotalWidth = Math.Max(Math.Max(TotalWidth,
        UiHelper.GetDgvColumnsWidth(dataGridViewTable) + AWF.BaseIndexWindowsWidth),
        Size.Width);
      Size = UiHelper.GetAppliedIndexWindowsSize(TotalWidth, TotalHeight, AWF.CommonIndexWindowsMaxSize);
      if (hasToolTipBeenSet && Model.Filter.HasFilter())
        toolTip.SetToolTip(labelFilterMessage, Model.FilterText);
      Invalidate();
      FunctionHelper.LockWindowUpdate(IntPtr.Zero);
    }

    bool hasToolTipBeenSet = false;
    ToolTip toolTip;
    private void UserIndexForm_Load(object sender, System.EventArgs e) {
      if (!hasToolTipBeenSet) {
        toolTip = new ToolTip();

        toolTip.InitialDelay = 2000;
        toolTip.ReshowDelay = 1000;
        toolTip.ShowAlways = false;

        if (Model.Filter.HasFilter())
          toolTip.SetToolTip(labelFilterMessage, Model.FilterText);

        hasToolTipBeenSet = true;
      }

      uiFinalTouch();
    }

    private void buttonCreate_Click(object sender, EventArgs e) {
      actionCreate();
    }

    private void buttonClose_Click(object sender, System.EventArgs e) {
      Close();
    }

    private void buttonFilter_Click(object sender, EventArgs e) {
      actionFilter();
    }

    #region Navigation
    private void linkLabelFirst_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      Model.NavData.GoToFirstPage();
      refreshTableWithUiUpdate();
    }

    private void linkLabelPrev100_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      Model.NavData.GoToPrev100Page();
      refreshTableWithUiUpdate();
    }

    private void linkLabelPrev10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      Model.NavData.GoToPrev10Page();
      refreshTableWithUiUpdate();
    }

    private void linkLabelPrev_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      Model.NavData.GoToPrevPage();
      refreshTableWithUiUpdate();
    }

    private void linkLabelNext_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      Model.NavData.GoToNextPage();
      refreshTableWithUiUpdate();
    }

    private void linkLabelNext10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      Model.NavData.GoToNext10Page();
      refreshTableWithUiUpdate();
    }

    private void linkLabelNext100_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      Model.NavData.GoToNext100Page();
      refreshTableWithUiUpdate();
    }

    private void linkLabelLast_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      Model.NavData.GoToLastPage();
      refreshTableWithUiUpdate();
    }
    #endregion
  }
}

