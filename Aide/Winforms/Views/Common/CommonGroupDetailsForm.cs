using Aibe.Models;
using Aibe.Models.Core;
using Aide.Customs;
using Aide.Logics;
using Aide.Models;
using Aide.Winforms.Helpers;
using Aide.Winforms.Models;
using Extension.Models;
using Extension.String;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AWF = Aide.Winforms.SH;

namespace Aide.Winforms {
  public partial class CommonGroupDetailsForm : Form {
    private AideFilterGroupDetailsModel agdModel { get; set; }
    public int TotalWidth { get; set; }
    public int TotalHeight { get; set; }
    public Dictionary<string, string> Filters { get; set; } = new Dictionary<string, string>();
    //public int PaintNo = 0;

    public CommonGroupDetailsForm(AideFilterGroupDetailsModel model) {
      InitializeComponent();
      initialize(model);
      applyModel(model);
    }

    #region Customized Actions
    //The customized actions can be set outside
    private static Dictionary<string, CustomizedRowActionDelegate> CustomizedRowActions { get; set; } = new Dictionary<string, CustomizedRowActionDelegate>();
    private static Dictionary<string, CustomizedTableActionDelegate> CustomizedTableActions { get; set; } = new Dictionary<string, CustomizedTableActionDelegate>();
    public static void AddCustomizedRowAction(KeyValuePair<string, CustomizedRowActionDelegate> customizedRowAction) {
      CustomizedRowActions.Add(customizedRowAction.Key, customizedRowAction.Value);
    }

    public static void AddCustomizedRowActions(Dictionary<string, CustomizedRowActionDelegate> customizedRowActions) {
      if (customizedRowActions == null)
        return;
      foreach (var customizedRowAction in customizedRowActions)
        CustomizedRowActions.Add(customizedRowAction.Key, customizedRowAction.Value);
    }

    public static void RemoveCustomizedRowAction(string tableAndActionName) {
      if (CustomizedRowActions.ContainsKey(tableAndActionName))
        CustomizedRowActions.Remove(tableAndActionName);
    }

    public static void RemoveCustomizedRowActions(IEnumerable<string> tableAndActionNames) {
      if (tableAndActionNames == null)
        return;
      foreach (string tableAndActionName in tableAndActionNames)
        if (CustomizedRowActions.ContainsKey(tableAndActionName))
          CustomizedRowActions.Remove(tableAndActionName);
    }

    public static void ClearCustomizedRowActions() {
      CustomizedRowActions.Clear();
    }

    public static void AddCustomizedTableAction(KeyValuePair<string, CustomizedTableActionDelegate> customizedTableAction) {
      CustomizedTableActions.Add(customizedTableAction.Key, customizedTableAction.Value);
    }

    public static void AddCustomizedTableActions(Dictionary<string, CustomizedTableActionDelegate> customizedTableActions) {
      if (customizedTableActions == null)
        return;
      foreach (var customizedTableAction in customizedTableActions)
        CustomizedTableActions.Add(customizedTableAction.Key, customizedTableAction.Value);
    }

    public static void RemoveCustomizedTableAction(string tableAndActionName) {
      if (CustomizedTableActions.ContainsKey(tableAndActionName))
        CustomizedTableActions.Remove(tableAndActionName);
    }

    public static void RemoveCustomizedTableActions(IEnumerable<string> tableAndActionNames) {
      if (tableAndActionNames == null)
        return;
      foreach (string tableAndActionName in tableAndActionNames)
        if (CustomizedTableActions.ContainsKey(tableAndActionName))
          CustomizedTableActions.Remove(tableAndActionName);
    }

    public static void ClearCustomizedTableActions() {
      CustomizedTableActions.Clear();
    }

    //Unlike Aiwe, this does not need to have action filter check, because those who have no access to the action, cannot see the button for it
    private void runCustomizedRowActionFor(string actionName, int cid, List<KeyValuePair<string, object>> identifiers) {
      string key = agdModel.TableName + "-" + actionName;
      if (CustomizedRowActions != null && CustomizedRowActions.Any(x => x.Key.EqualsIgnoreCase(key))) {
        var customizedRowAction = CustomizedRowActions.FirstOrDefault(x => x.Key.EqualsIgnoreCase(key));
        customizedRowAction.Value(cid, identifiers);
      }
    }

    //Unlike Aiwe, this does not need to have action filter check, because those who have no access to the action, cannot see the button for it
    private void runCustomizedTableActionFor(string actionName) {
      string key = agdModel.TableName + "-" + actionName;
      if (CustomizedTableActions != null && CustomizedTableActions.Any(x => x.Key.EqualsIgnoreCase(key))) {
        var customizedTableAction = CustomizedTableActions.FirstOrDefault(x => x.Key.EqualsIgnoreCase(key));
        customizedTableAction.Value(agdModel, Filters);
      }
    }
    #endregion

    private void DataGridViewTable_CellContentClick(object sender, DataGridViewCellEventArgs e) {
      var senderGrid = (DataGridView)sender;

      if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
          e.RowIndex >= 0 && senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewButtonCell) {
        DataGridViewButtonCell dgvCell = (DataGridViewButtonCell)senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
        if (!(dgvCell.Tag is ButtonTag))
          return;
        ButtonTag tag = (ButtonTag)dgvCell.Tag;
        if (tag.UseDefaultAction) {
          var allowedRowActions = Aibe.DH.DefaultNonCreateRowActions.ToList();
          if (!allowedRowActions.Any(x => x.EqualsIgnoreCase(tag.ActionName) || tag.Cid <= 0))
            return;
          if (tag.ActionName.EqualsIgnoreCase(Aibe.DH.EditActionName)) { //Do edit
            rowActionEdit(tag.Cid);
          } else if (tag.ActionName.EqualsIgnoreCase(Aibe.DH.DetailsActionName)) { //Do details
            rowActionDetails(tag.Cid);
          } else if (tag.ActionName.EqualsIgnoreCase(Aibe.DH.DeleteActionName)) { //Do delete
            rowActionDelete(tag.Cid);
          } else if (tag.ActionName.EqualsIgnoreCase(Aibe.DH.DownloadAttachmentsActionName)) { //Do download attachments
            rowActionDownloadAttachments(tag.Cid);
          }
        } else { //Handle not default action here! Perhaps table name is also necessary to determine the action...
          runCustomizedRowActionFor(tag.ActionName, tag.Cid, tag.Identifiers);
        }
      }
    }

    private void initialize(AideFilterGroupDetailsModel model) { //whatever requires initialization only once goes here
      dataGridViewTable.CellContentClick += DataGridViewTable_CellContentClick; //only initialized once
      //Table actions are created first so that it can handle the UI width correctly
      flowLayoutPanelTableActions.Controls.Clear();
      splitContainerContentCore.Panel2Collapsed = !model.Meta.HasTableAction;
      if (model.Meta.HasTableAction)
        foreach (var action in model.Meta.TableActions) {
          LinkLabel linkLabelTableAction = new LinkLabel {
            Text = getTableActionText(action.Name),
            Tag = action.Name,
            AutoSize = true,
            BorderStyle = BorderStyle.FixedSingle,
            Margin = new Padding() { Left = 3, Top = 5, Right = 3, Bottom = 0 }
          };
          linkLabelTableAction.Click += LinkLabelTableAction_Click;
          flowLayoutPanelTableActions.Controls.Add(linkLabelTableAction);
        }
    }

    private string getTableActionText(string actionName) {
      string text = actionName.ToCamelBrokenString();
      if (Aibe.DH.DefaultTableActions.Any(x => x.EqualsIgnoreCase(actionName))) {
        string tableActionName = Aibe.DH.DefaultTableActions.FirstOrDefault(x => x.EqualsIgnoreCase(actionName));
        text = Aibe.LCZ.GetLocalizedDefaultTableActionName(tableActionName);
      }
      return text;
    }

    private void localization() {
      buttonClose.Text = Aibe.LCZ.W_Close;
      buttonCreate.Text = agdModel.IsGroupDeletion ? Aibe.LCZ.W_DeleteGroup : Aibe.LCZ.W_Create;
      buttonFilter.Text = Aibe.LCZ.W_Filter;
      linkLabelFirst.Text = Aibe.LCZ.W_First;
      linkLabelLast.Text = Aibe.LCZ.W_Last;
      linkLabelNext.Text = Aibe.LCZ.W_NextSymbol;
      linkLabelPrev.Text = Aibe.LCZ.W_PreviousSymbol;
      base.Text = agdModel.IsGroupDeletion ? Aibe.LCZ.W_DeleteGroup : Aibe.LCZ.W_GroupDetails;
      labelAction.Text = string.Concat("(", agdModel.IsGroupDeletion ? Aibe.LCZ.W_DeleteGroup : Aibe.LCZ.W_GroupDetails, ": ", 
        string.Join(", ", agdModel.GdModel.Identifiers.Select(x => x.Value).ToArray()), ")");
    }

    private void visibility() {
      linkLabelNext100.Visible = agdModel.NavData.MaxPage > 100;
      linkLabelPrev100.Visible = agdModel.NavData.MaxPage > 100;
      linkLabelNext10.Visible = agdModel.NavData.MaxPage > 10;
      linkLabelPrev10.Visible = agdModel.NavData.MaxPage > 10;
    }

    private void applyModel(AideFilterGroupDetailsModel model) {
      agdModel = model;
      dataGridViewTable.Rows.Clear();
      dataGridViewTable.Columns.Clear();
      localization();
      visibility();

      Text += " - " + model.TableDisplayName;
      labelTitle.Text = model.TableDisplayName;

      buttonFilter.Visible = !model.Meta.FilterIsDisabled;
      labelFilterMessage.Visible = !model.Meta.FilterIsDisabled;
      labelFilterMessage.Text = model.BaseModel.FilterNo > 0 ? "[" + model.BaseModel.FilterNo.ToString() + "]" : string.Empty;

      labelNavDataValue.Text = Aibe.LCZ.W_Page + " " + model.NavData.CurrentPage + " / " + model.NavData.MaxPage
        + ", " + Aibe.LCZ.W_Data + " " + model.NavData.ItemNoInPageFirst + "-" + 
        model.NavData.ItemNoInPageLast + " " + Aibe.LCZ.W_of + " " + model.NavData.QueryCount + " " + Aibe.LCZ.W_Data;

      bool hasData = model.HasTable && model.RowNo > 0;
      buttonCreate.Visible = model.Meta.HasCreateAction;
      flowLayoutPanelFooter.Enabled = hasData;

      TotalWidth = getInitialWidth();
      TotalHeight = MinimumSize.Height; //apply minimum height first
      //dataGridViewTable.DataSource = model.Table; //this basic show... works! Left for note.

      if (!hasData) //Do nothing if there is nothing to show
        return;

      List<int> excludedColumnNo = new List<int>();
      List<ColumnInfo> includedColumns = model.ColumnInfos.Where(x => x.IsIndexIncluded).ToList();
      bool isCidForced = !includedColumns.Any(x => x.Name.EqualsIgnoreCase(Aibe.DH.Cid));

      foreach (ColumnInfo column in includedColumns) {
        DataGridViewColumn dgvColumn;
        if (column.IsPictureColumn) {
          dgvColumn = new DataGridViewImageColumn() {
            DataPropertyName = column.Name,
            HeaderText = column.DisplayName,
            Name = column.Name,
          };            
        } else
          dgvColumn = new DataGridViewTextBoxColumn() {
            DataPropertyName = column.Name,
            HeaderText = column.DisplayName,
            Name = column.Name,
          };
        dgvColumn.ReadOnly = true;
        dataGridViewTable.Columns.Add(dgvColumn);
      }

      if (model.Meta.HasGroupDetailsNonCreateAction && !model.IsGroupDeletion) { //group deletion doesn't have any action button per row
        int index = 1;
        foreach (var action in model.Meta.GroupDetailsNonCreateActions) {
          if (!model.IsAllowedToCallAction(action.Name))
            continue;
          DataGridViewColumn dgvColumn = new DataGridViewButtonColumn() {
            HeaderText = string.Empty,
          };
          dgvColumn.ReadOnly = true;
          dataGridViewTable.Columns.Add(dgvColumn);
          ++index;
        }
      }

      foreach (DataRow row in model.IndexRows) {
        int id = 0; //record id
        object cid = row[Aibe.DH.Cid];
        if (cid != null)
          id = (int)cid;
        DataGridViewRow dgvRow = UiHelper.CreateCommonDgvRow();

        foreach (ColumnInfo columnInfo in includedColumns) {
          object val = row[columnInfo.Name];
          if (!model.IsColumnIncludedInIndex(columnInfo.Name))
            continue;
          DataGridViewCell cell = null;
          if (columnInfo.IsSciptColumn) {
            cell = new DataGridViewTextBoxCell() {
              Value = "[" + Aibe.LCZ.W_See + " " + Aibe.LCZ.W_Details + "]",
            };
          } else if (columnInfo.IsIndexShowImage) {
            if (val is DBNull || val == null || string.IsNullOrWhiteSpace(val.ToString())) {
              cell = new DataGridViewImageCell();
            } else if (val.ToString().Contains("/") || val.ToString().Contains("\\")) {
              PictureColumnInfo pcInfo = model.Meta.GetPictureColumnInfo(columnInfo.Name);
              cell = new DataGridViewImageCell() {
                Value = UiHelper.GetIndexImage(val.ToString(), pcInfo),
              };
            } else {
              string fullRelativePath = model.TableName + "\\" + id + "\\" + val.ToString();
              PictureColumnInfo pcInfo = model.Meta.GetPictureColumnInfo(columnInfo.Name);
              cell = new DataGridViewImageCell() {
                Value = UiHelper.GetIndexImage(fullRelativePath, pcInfo),
              };
            }
          } else if (columnInfo.IsListColumn) { //choose not to show this in index, because I am unable to
            cell = new DataGridViewTextBoxCell() {
              Value = "[" + Aibe.LCZ.W_See + " " + Aibe.LCZ.W_Details + "]",
            };
          } else if (columnInfo.IsDateTime) {
            string usedVal = val?.ToString();
            if (!string.IsNullOrWhiteSpace(usedVal)) {
              DateTime dateTime = DateTime.Parse(usedVal); //cannot fail
              usedVal = dateTime.ToString(columnInfo.HasCustomDateTimeFormat ? 
                columnInfo.CustomDateTimeFormat : Aide.PH.IndexDateTimeFormat);
            }
            cell = new DataGridViewTextBoxCell() {
              Value = usedVal,
            };
          } else {
            if (columnInfo.Colorings == null) {
              cell = new DataGridViewTextBoxCell() {
                Value = val,
              };
            } else if (columnInfo.Colorings.Any(x => x.ColumnName.EqualsIgnoreCase(columnInfo.Name))) {
              ColoringInfo colorItem = columnInfo.Colorings.FirstOrDefault(x => x.ColumnName.EqualsIgnoreCase(columnInfo.Name));
              cell = new DataGridViewTextBoxCell() {
                Value = val,
              };
              object usedVal = colorItem.ConditionColumnIsDifferentColumn ? row[colorItem.ConditionColumnName] : val;
              if (colorItem.CheckConditionMet(columnInfo.DataType, usedVal, id)) //TODO might be able to be changed to identifiers... but
                cell.Style.BackColor = Color.FromName(colorItem.Color);
            } else {
              string usedVal = val?.ToString();
              if (!string.IsNullOrWhiteSpace(usedVal)) {
                if (usedVal.EqualsIgnoreCase(true.ToString()))
                  usedVal = Aibe.LCZ.W_BoTrue;
                if (usedVal.EqualsIgnoreCase(false.ToString()))
                  usedVal = Aibe.LCZ.W_BoFalse;
              }
              cell = new DataGridViewTextBoxCell() {
                Value = usedVal,
              };
            } //if else block for coloring ends
          } //if else block type columns end
          dgvRow.Cells.Add(cell);
        } //for each ends

        if (model.Meta.HasGroupDetailsNonCreateAction && !model.IsGroupDeletion) {
          foreach (var action in model.Meta.GroupDetailsNonCreateActions) {
            if (!model.IsAllowedToCallAction(action.Name))
              continue;
            if (model.Meta.IsDefaultAction(action.Name)) { //using default actions
              if (id != 0) {
                DataGridViewCell cell = new DataGridViewButtonCell() {
                  Value = Aibe.LCZ.GetLocalizedDefaultActionName(action.Name).ToCamelBrokenString(),
                  Tag = new ButtonTag() { UseDefaultAction = true, ActionName = action.Name, Cid = id },
                };
                dgvRow.Cells.Add(cell);
              }
            } else {
              DataGridViewCell cell = new DataGridViewButtonCell() {
                Value = action.Name.ToCamelBrokenString(),
                Tag = new ButtonTag() { UseDefaultAction = false, ActionName = action.Name, Cid = id },
              };
              dgvRow.Cells.Add(cell);
            }
          }
        }
        dataGridViewTable.Rows.Add(dgvRow);
      }

      //if (model.IsGroupDeletion) //v1.4.1.0 although it is group deletion, the dataGrid should still be enabled
      //  dataGridViewTable.Enabled = false; //otherwise the wide-table cannot be horizontally scrolled to be seen
    }

    private void LinkLabelTableAction_Click(object sender, EventArgs e) {
      if (!agdModel.Meta.HasTableAction || !(sender is LinkLabel))
        return;
      LinkLabel linkLabel = (LinkLabel)sender;
      if (linkLabel.Tag == null || string.IsNullOrWhiteSpace(linkLabel.Tag.ToString()))
        return;
      string tableActionName = linkLabel.Tag.ToString();
      if (!agdModel.IsAllowedToCallTableAction(tableActionName))
        return;
      if (!agdModel.Meta.IsDefaultTableAction(tableActionName)) {
        runCustomizedTableActionFor(tableActionName);
        return;
      }

      //using default table actions
      bool result;
      if (tableActionName.EqualsIgnoreCase(Aibe.DH.ExportToCSVTableActionName)) {
        var model = CommonLogic.GetFilterGroupDetailsModel(agdModel.TableName, agdModel.GdModel.Identifiers, agdModel.NavData.CurrentPage,
          agdModel.BaseModel.StringDictionary, loadAllData: false, isGroupDeletion: agdModel.IsGroupDeletion);          
        result = CommonLogic.ExportToCsv(model, agdModel.TableName, FileHelper.GetDownloadFolderPath());
        string format = result ? Aibe.LCZ.M_TableActionSuccess : Aibe.LCZ.E_TableActionUnsuccessful;
        string title = result ? Aibe.LCZ.W_Successful : Aibe.LCZ.W_Error;
        MessageBoxIcon icon = result ? MessageBoxIcon.Information : MessageBoxIcon.Warning;
        MessageBox.Show(string.Format(format, getTableActionText(tableActionName)), title, MessageBoxButtons.OK, icon);
      } else if (tableActionName.EqualsIgnoreCase(Aibe.DH.ExportAllToCSVTableActionName)) {
        var model = CommonLogic.GetFilterGroupDetailsModel(agdModel.TableName, agdModel.GdModel.Identifiers, agdModel.NavData.CurrentPage,
          agdModel.BaseModel.StringDictionary, loadAllData: true, isGroupDeletion: agdModel.IsGroupDeletion);
        result = CommonLogic.ExportAllToCsv(model, agdModel.TableName, FileHelper.GetDownloadFolderPath());
        string format = result ? Aibe.LCZ.M_TableActionSuccess : Aibe.LCZ.E_TableActionUnsuccessful;
        string title = result ? Aibe.LCZ.W_Successful : Aibe.LCZ.W_Error;
        MessageBoxIcon icon = result ? MessageBoxIcon.Information : MessageBoxIcon.Warning;
        MessageBox.Show(string.Format(format, getTableActionText(tableActionName)), title, MessageBoxButtons.OK, icon);
      }
      //Implement default table actions here!
    }

    private void disposeFormAndApplyUpdate(Form form, AideFilterGroupDetailsModel model) {
      if (form != null) {
        applyModelWithUiUpdate(model);
        form.Dispose();
      }
    }

    private void applyModelWithUiUpdate(AideFilterGroupDetailsModel model) {
      applyModel(model);
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
      if (hasToolTipBeenSet)
        if (!string.IsNullOrWhiteSpace(agdModel.BaseModel.FilterMsg))
          toolTip.SetToolTip(labelFilterMessage, agdModel.BaseModel.FilterMsg);
      Invalidate();
      FunctionHelper.LockWindowUpdate(IntPtr.Zero);
    }

    private int getFlPanelsItemsWidth(FlowLayoutPanel flPanel) {
      int width = 0;
      foreach (Control control in flPanel.Controls)
        width += UiHelper.GetWidthOf(control);
      return width;
    }

    private int getInitialWidth() {
      int footerWidth = getFlPanelsItemsWidth(flowLayoutPanelFooter);
      int tableActionsWidth = getFlPanelsItemsWidth(flowLayoutPanelTableActions);
      int headerWidth = getFlPanelsItemsWidth(flowLayoutPanelHeader);
      return Math.Max(Math.Max(footerWidth, headerWidth), tableActionsWidth) + AWF.BaseIndexWindowsWidth; //apply minimum width first
    }

    private void buttonClose_Click(object sender, System.EventArgs e) {
      Close();
    }

    bool hasToolTipBeenSet = false;
    ToolTip toolTip;
    private void CommonGroupDetailsForm_Load(object sender, System.EventArgs e) {
      if (!hasToolTipBeenSet) {
        toolTip = new ToolTip();

        toolTip.InitialDelay = 2000;
        toolTip.ReshowDelay = 1000;
        toolTip.ShowAlways = false;

        if (!string.IsNullOrWhiteSpace(agdModel.BaseModel.FilterMsg))
          toolTip.SetToolTip(labelFilterMessage, agdModel.BaseModel.FilterMsg);

        hasToolTipBeenSet = true;
      }

      uiFinalTouch();
    }

    private string createRowActionErrorModel(AideCreateEditModel model, string localizedAction) {
      StringBuilder sb = new StringBuilder(Aibe.LCZ.W_Action);
      sb.Append(" [");
      sb.Append(localizedAction);
      sb.Append("] ");
      sb.Append(Aibe.LCZ.W_in);
      sb.Append(" ");
      sb.Append(Aibe.LCZ.W_Table);
      sb.Append(" [");
      sb.Append(model.TableDisplayName);
      sb.Append("] ");
      sb.Append(Aibe.LCZ.W_Failed);
      sb.AppendLine();
      if (!string.IsNullOrWhiteSpace(model.ErrorMessage)) {
        sb.Append(Aibe.LCZ.W_ErrorMessage);
        sb.Append(": ");
        sb.Append(model.ErrorMessage);
        sb.AppendLine();
      }
      if (model.ErrorDict != null && model.ErrorDict.Count > 0) {
        sb.Append(Aibe.LCZ.W_ErrorList);
        sb.Append(":");
        sb.AppendLine();
        foreach (var errorItem in model.ErrorDict)
          sb.AppendLine(" - " + Aibe.LCZ.W_in + " [" + errorItem.Key + "]: " + errorItem.Value);
      }
      return sb.ToString();
    }

    //Each row action is implemented individually here, if they are non-default
    private void rowActionCommonCreateEdit(AideCreateEditModel model, bool isCreate, int? cid = null) {
      CommonCreateEditForm form = new CommonCreateEditForm(model);
      DialogResult result = DialogResult.Retry;

      while (result == DialogResult.Retry) {
        result = form.ShowDialog();
        if (result != DialogResult.OK)
          return;
        AideRequestModel request = form.CreateRequestModel();
        if (!isCreate && cid != null && cid.HasValue && cid.Value > 0)
          request.Cid = cid.Value;
        BaseErrorModel errorModel = isCreate ? //it is non-erroneous, otherwise it cannot come here in the first place
          CommonLogic.Create(model.TableName, identifiers: agdModel.GdModel.Identifiers, model: request) : //this is an attempt to create a new item based on the form collection gathered
          CommonLogic.Edit(model.TableName, identifiers: agdModel.GdModel.Identifiers, model: request);
        if (errorModel.HasError) { //error due to access problem, thus return, not even try to retry
          MessageBox.Show(errorModel.ToShortString(Aibe.LCZ.W_ErrorModelCodeWord, Aibe.LCZ.W_ErrorModelMessageWord), 
            Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
          form.Dispose();
          form = null;
          return;
        }
        model = (AideCreateEditModel)errorModel.ReturnObject;
        if (!model.IsSuccessful) { //unsuccessful... but not due to access problem, thus may retry
          string errorMessage = createRowActionErrorModel(model, isCreate ? Aibe.LCZ.W_Create : Aibe.LCZ.W_Edit);
          result = MessageBox.Show(errorMessage, Aibe.LCZ.W_Error, MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
        }
      }

      //This should work based on
      //https://stackoverflow.com/questions/3861602/how-to-check-if-a-windows-form-is-already-open-and-close-it-if-it-is      
      disposeFormAndApplyUpdate(form, FormHelper.GetUpdatedCommonGroupDetailsModel(model.TableName, 
        agdModel.GdModel.Identifiers, agdModel.NavData.CurrentPage, Filters, isGroupDeletion: agdModel.IsGroupDeletion));
    }

    private void rowActionCreate() {
      BaseErrorModel errorModel = CommonLogic.Create(agdModel.Meta.TableName, identifiers: agdModel.GdModel.Identifiers); //create does not have table to be put
      if (errorModel.HasError) {
        MessageBox.Show(errorModel.Message, Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      AideCreateEditModel model = (AideCreateEditModel)errorModel.ReturnObject; 
      rowActionCommonCreateEdit(model, true);
    }

    private void rowActionEdit(int cid) {
      BaseErrorModel errorModel = CommonLogic.Edit(agdModel.Meta.TableName, identifiers: agdModel.GdModel.Identifiers, id: cid);
      if (errorModel.HasError) {
        MessageBox.Show(errorModel.Message, Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      AideCreateEditModel model = (AideCreateEditModel)errorModel.ReturnObject;
      rowActionCommonCreateEdit(model, false, cid);
    }

    private void rowActionDetails(int cid) {
      BaseErrorModel errorModel = CommonLogic.Details(agdModel.Meta.TableName, cid);
      if (errorModel.HasError) {
        MessageBox.Show(errorModel.Message, Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      AideDetailsModel model = (AideDetailsModel)errorModel.ReturnObject;
      CommonDetailsForm form = new CommonDetailsForm(model);
      form.Show(); //There is no need to show dialog in details...
    }

    private void rowActionDownloadAttachments(int cid) {
      MessageBox.Show(string.Format(Aibe.LCZ.M_DefaultActionNotImplemented, Aibe.LCZ.W_DownloadAttachments, "Aide.WinForms"));
    }

    private void rowActionDelete(int cid) {
      BaseErrorModel errorModel = CommonLogic.Delete(agdModel.Meta.TableName, cid);
      if (errorModel.HasError) {
        MessageBox.Show(errorModel.Message, Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      AideDetailsModel model = (AideDetailsModel)errorModel.ReturnObject;
      CommonDetailsForm form = new CommonDetailsForm(model);
      DialogResult result = form.ShowDialog();
      if (result != DialogResult.OK)
        return;
      CommonLogic.DeletePost(agdModel.Meta.TableName, cid); //no need to check, currently it is always successful
      disposeFormAndApplyUpdate(form, FormHelper.GetUpdatedCommonGroupDetailsModel( //no need to check this error, because it must be successful if someone can open a CommonIndex in the first place
        model.TableName, agdModel.GdModel.Identifiers, agdModel.NavData.CurrentPage, Filters, 
        isGroupDeletion: agdModel.IsGroupDeletion)); 
    }

    private void rowActionDeleteGroup() {
      DialogResult result = MessageBox.Show(Aibe.LCZ.NFM_DeleteGroupConfirmation, Aibe.LCZ.W_DeleteGroup, 
        MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
      if (result != DialogResult.OK)
        return;
      BaseErrorModel errorModel = CommonLogic.DeleteGroupPost(agdModel.Meta.TableName, agdModel.GdModel.Identifiers);
      if (errorModel.HasError) {
        MessageBox.Show(errorModel.Message, Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      Close();
    }

    private void buttonCreate_Click(object sender, EventArgs e) {
      if (agdModel.IsGroupDeletion) {
        if (agdModel.Meta.IsDefaultAction(Aibe.DH.DeleteGroupActionName))
          rowActionDeleteGroup();
        else
          runCustomizedRowActionFor(Aibe.DH.DeleteGroupActionName, -1, agdModel.GdModel.Identifiers);
      } else {
        if (agdModel.Meta.IsDefaultAction(Aibe.DH.CreateActionName))
          rowActionCreate();
        else
          runCustomizedRowActionFor(Aibe.DH.CreateActionName, -1, null);
      }
    }

    private void buttonFilter_Click(object sender, EventArgs e) {
      CommonFilterForm form = new CommonFilterForm(agdModel);
      DialogResult result = form.ShowDialog();
      if (result != DialogResult.OK)
        return;
      AideRequestModel request = form.CreateRequestModel();
      if (request != null && request.FormCollection != null)
        Filters = request.FormCollection;
      disposeFormAndApplyUpdate(form, FormHelper.GetUpdatedCommonGroupDetailsModel(agdModel.TableName, 
        agdModel.GdModel.Identifiers, agdModel.NavData.CurrentPage, Filters,
        isGroupDeletion: agdModel.IsGroupDeletion));
    }

    #region Navigation
    private void linkLabelFirst_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {      
      applyModelWithUiUpdate(FormHelper.GetUpdatedCommonGroupDetailsModel(agdModel.TableName, agdModel.GdModel.Identifiers, 
        1, Filters, isGroupDeletion: agdModel.IsGroupDeletion));
    }

    private void linkLabelPrev100_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {      
      applyModelWithUiUpdate(FormHelper.GetUpdatedCommonGroupDetailsModel(agdModel.TableName, agdModel.GdModel.Identifiers, 
        agdModel.NavData.Prev100Page, Filters, isGroupDeletion: agdModel.IsGroupDeletion));
    }

    private void linkLabelPrev10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {     
      applyModelWithUiUpdate(FormHelper.GetUpdatedCommonGroupDetailsModel(agdModel.TableName, agdModel.GdModel.Identifiers, 
        agdModel.NavData.Prev10Page, Filters, isGroupDeletion: agdModel.IsGroupDeletion));
    }

    private void linkLabelPrev_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {     
      applyModelWithUiUpdate(FormHelper.GetUpdatedCommonGroupDetailsModel(agdModel.TableName, agdModel.GdModel.Identifiers, 
        agdModel.NavData.PrevPage, Filters, isGroupDeletion: agdModel.IsGroupDeletion));
    }

    private void linkLabelNext_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {      
      applyModelWithUiUpdate(FormHelper.GetUpdatedCommonGroupDetailsModel(agdModel.TableName, agdModel.GdModel.Identifiers, 
        agdModel.NavData.NextPage, Filters, isGroupDeletion: agdModel.IsGroupDeletion));
    }

    private void linkLabelNext10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {      
      applyModelWithUiUpdate(FormHelper.GetUpdatedCommonGroupDetailsModel(agdModel.TableName, agdModel.GdModel.Identifiers, 
        agdModel.NavData.Next10Page, Filters, isGroupDeletion: agdModel.IsGroupDeletion));
    }

    private void linkLabelNext100_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {      
      applyModelWithUiUpdate(FormHelper.GetUpdatedCommonGroupDetailsModel(agdModel.TableName, agdModel.GdModel.Identifiers, 
        agdModel.NavData.Next100Page, Filters, isGroupDeletion: agdModel.IsGroupDeletion));
    }

    private void linkLabelLast_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {      
      applyModelWithUiUpdate(FormHelper.GetUpdatedCommonGroupDetailsModel(agdModel.TableName, agdModel.GdModel.Identifiers, 
        agdModel.NavData.MaxPage, Filters, isGroupDeletion: agdModel.IsGroupDeletion));
    }
    #endregion
  }
}
