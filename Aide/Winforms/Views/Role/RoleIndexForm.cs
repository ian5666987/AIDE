﻿using Aibe.Models;
using Aide.Logics;
using Aide.Winforms.Helpers;
using Aide.Winforms.Models;
using Extension.String;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using AWF = Aide.Winforms.SH;

namespace Aide.Winforms {
  public partial class RoleIndexForm : Form {
    public int TotalWidth { get; set; }
    public int TotalHeight { get; set; }
    public string FilterText { get; set; }
    public bool HasFilter { get { return !string.IsNullOrWhiteSpace(FilterText); } }
    public NavDataModel NavData; //for filtering and paging correctly
    public RoleIndexForm() {
      InitializeComponent();
      localization();
      initDgv();
    }

    private void localization() {
      Text = Aibe.LCZ.W_Role;
      buttonClose.Text = Aibe.LCZ.W_Close;
      buttonCreate.Text = Aibe.LCZ.W_Create;
      buttonFilter.Text = Aibe.LCZ.W_Filter;
      linkLabelFirst.Text = Aibe.LCZ.W_First;
      linkLabelLast.Text = Aibe.LCZ.W_Last;
      linkLabelNext.Text = Aibe.LCZ.W_NextSymbol;
      linkLabelPrev.Text = Aibe.LCZ.W_PreviousSymbol;
      base.Text = Aibe.LCZ.W_Index;
      labelTitle.Text = Aibe.LCZ.W_Role;
      labelFilterMessage.Text = string.Empty; //emtifies first
      labelNavDataValue.Text = string.Empty;
      labelAction.Text = string.Concat("(", Aibe.LCZ.W_Index, ")");
    }

    private void initDgv() {
      DataGridViewTextBoxColumn dgvNoColumn = new DataGridViewTextBoxColumn() { HeaderText = Aibe.LCZ.W_No, };
      DataGridViewTextBoxColumn dgvRoleColumn = new DataGridViewTextBoxColumn() { HeaderText = Aibe.LCZ.T_RoleNameColumnName, };
      DataGridViewButtonColumn editColumn = new DataGridViewButtonColumn();
      DataGridViewButtonColumn detailsColumn = new DataGridViewButtonColumn();
      DataGridViewButtonColumn deleteColumn = new DataGridViewButtonColumn();
      dataGridViewTable.Columns.Add(dgvNoColumn);
      dataGridViewTable.Columns.Add(dgvRoleColumn);
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
      DataTable table = RoleLogic.Index(FilterText, ref NavData);
      labelNavDataValue.Text = Aibe.LCZ.W_Page + " " + NavData.CurrentPage + " / " + NavData.MaxPage
        + ", " + Aibe.LCZ.W_Data + " " + NavData.ItemNoInPageFirst + "-" +
        NavData.ItemNoInPageLast + " " + Aibe.LCZ.W_of + " " + NavData.QueryCount + " " + Aibe.LCZ.W_Data;
      labelFilterMessage.Text = HasFilter ? "[1]" : string.Empty;      
      int count = 0;
      foreach (DataRow row in table.Rows) {
        ++count;
        if (count < NavData.ItemNoInPageFirst)
          continue;
        else if (count > NavData.ItemNoInPageLast)
          break;
        object role = row[Aibe.DH.RoleNameColumnName];
        string roleId = (string)row[Aibe.DH.RoleIdColumnName];
        DataGridViewRow dgvRow = new DataGridViewRow();
        DataGridViewTextBoxCell noCell = new DataGridViewTextBoxCell { Value = count };
        DataGridViewTextBoxCell roleCell = new DataGridViewTextBoxCell { Value = role };
        DataGridViewButtonCell btnEditCell = new DataGridViewButtonCell {
          Value = Aibe.LCZ.W_Edit,
          Tag = new UncommonButtonTag() { ActionName = Aibe.DH.EditActionName, StrId = roleId, Item = role }
        };
        DataGridViewButtonCell btnDetailsCell = new DataGridViewButtonCell {
          Value = Aibe.LCZ.W_Details,
          Tag = new UncommonButtonTag() { ActionName = Aibe.DH.DetailsActionName, StrId = roleId, Item = role }
        };
        DataGridViewButtonCell btnDeleteCell = new DataGridViewButtonCell {
          Value = Aibe.LCZ.W_Delete,
          Tag = new UncommonButtonTag() { ActionName = Aibe.DH.DeleteActionName, StrId = roleId, Item = role }
        };
        dgvRow.Cells.Add(noCell); //the number cell
        dgvRow.Cells.Add(roleCell); //the role cell
        dgvRow.Cells.Add(btnEditCell); //the edit button cell
        dgvRow.Cells.Add(btnDetailsCell); //the details button cell
        dgvRow.Cells.Add(btnDeleteCell); //the delete button cell
        dataGridViewTable.Rows.Add(dgvRow);
      }
      visibility();
    }

    private void actionFilter() {
      RoleCreateEditFilterForm form = new RoleCreateEditFilterForm(Aibe.DH.FilterActionName, null, FilterText);
      if (DialogResult.OK == form.ShowDialog()) {
        FilterText = form.RoleText;
        refreshTableWithUiUpdate();
      }
      form.Dispose();
      form = null;
    }

    private void actionCreate() {
      RoleCreateEditFilterForm form = new RoleCreateEditFilterForm(Aibe.DH.CreateActionName);
      if (DialogResult.OK == form.ShowDialog())
        refreshTableWithUiUpdate();
      form.Dispose();
      form = null;
    }

    private void actionEdit(string id, string roleName) {
      RoleCreateEditFilterForm form = new RoleCreateEditFilterForm(Aibe.DH.EditActionName, id, roleName);
      if (DialogResult.OK == form.ShowDialog())
        refreshTableWithUiUpdate();
      form.Dispose();
      form = null;
    }
    private void actionDelete(string id, string roleName) {
      RoleDetailsForm form = new RoleDetailsForm(Aibe.DH.DeleteActionName, id, roleName);
      if (DialogResult.OK == form.ShowDialog())
        refreshTableWithUiUpdate();
      form.Dispose();
      form = null;
    }

    private void actionDetails(string id, string roleName) {
      RoleDetailsForm form = new RoleDetailsForm(Aibe.DH.DetailsActionName, id, roleName);
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
          actionEdit(tag.StrId, tag.Item.ToString());
        } else if (tag.ActionName.EqualsIgnoreCase(Aibe.DH.DetailsActionName)) { //Do details
          actionDetails(tag.StrId, tag.Item.ToString());
        } else if (tag.ActionName.EqualsIgnoreCase(Aibe.DH.DeleteActionName)) { //Do delete
          actionDelete(tag.StrId, tag.Item.ToString());
        }
      }
    }

    private void visibility() {
      linkLabelNext100.Visible = NavData.MaxPage > 100;
      linkLabelPrev100.Visible = NavData.MaxPage > 100;
      linkLabelNext10.Visible = NavData.MaxPage > 10;
      linkLabelPrev10.Visible = NavData.MaxPage > 10;
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
      if (hasToolTipBeenSet && HasFilter)
        toolTip.SetToolTip(labelFilterMessage, FilterText);
      Invalidate();
      FunctionHelper.LockWindowUpdate(IntPtr.Zero);
    }

    bool hasToolTipBeenSet = false;
    ToolTip toolTip;
    private void RoleIndexForm_Load(object sender, System.EventArgs e) {
      if (!hasToolTipBeenSet) {
        toolTip = new ToolTip();

        toolTip.InitialDelay = 2000;
        toolTip.ReshowDelay = 1000;
        toolTip.ShowAlways = false;

        if (HasFilter)
          toolTip.SetToolTip(labelFilterMessage, FilterText);

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
      NavData.GoToFirstPage();
      refreshTableWithUiUpdate();
    }

    private void linkLabelPrev100_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      NavData.GoToPrev100Page();
      refreshTableWithUiUpdate();
    }

    private void linkLabelPrev10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      NavData.GoToPrev10Page();
      refreshTableWithUiUpdate();
    }

    private void linkLabelPrev_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      NavData.GoToPrevPage();
      refreshTableWithUiUpdate();
    }

    private void linkLabelNext_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      NavData.GoToNextPage();
      refreshTableWithUiUpdate();
    }

    private void linkLabelNext10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      NavData.GoToNext10Page();
      refreshTableWithUiUpdate();
    }

    private void linkLabelNext100_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      NavData.GoToNext100Page();
      refreshTableWithUiUpdate();
    }

    private void linkLabelLast_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      NavData.GoToLastPage();
      refreshTableWithUiUpdate();
    }
    #endregion
  }
}

//var adminRoles = Aibe.DH.AdminRoles.Select(x => x.AsSqlStringValue()).ToList();
//string baseWhereClause = string.Concat("([", Aibe.DH.RoleNameColumnName, "] != ", string.Join(string.Concat(" AND [", Aibe.DH.RoleNameColumnName, "] != "), adminRoles), ")");
//string filterWhereClause = HasFilter ? string.Concat(" AND ([",
//  Aibe.DH.RoleNameColumnName, "] LIKE ", ("%" + FilterText + "%").AsSqlStringValue(), ")") : string.Empty;
//DataTable table = SQLServerHandler.GetFullDataTableWhere(Aibe.DH.UserDBConnectionString, Aide.PH.RoleTableName, 
//  baseWhereClause + filterWhereClause, Aibe.DH.RoleNameColumnName);
//if (NavData == null) //the first time
//  NavData = new NavDataModel(1, itemsPerPage, table.Rows.Count);
//else {
//  NavData.UpdateModel(NavData.CurrentPage, itemsPerPage, table.Rows.Count);
//}
