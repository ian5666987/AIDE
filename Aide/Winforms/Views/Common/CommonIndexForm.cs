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
  public partial class CommonIndexForm : Form {
    private AideFilterIndexModel fiModel { get; set; }
    public int TotalWidth { get; set; }
    public int TotalHeight { get; set; }
    public Dictionary<string, string> Filters { get; set; } = new Dictionary<string, string>();
    //public int PaintNo = 0;
    //private StringBuilder timingString = new StringBuilder();
    //private DateTime lastCapturedTime = DateTime.Now;

    public CommonIndexForm(AideFilterIndexModel model) {
      InitializeComponent();
      initialize(model);
      applyModel(model);
      //showTimeCapture();
    }

    //private void timeCapture(string message) {
    //  TimeSpan span = DateTime.Now - lastCapturedTime;
    //  timingString.AppendLine(message + ": " + span.TotalSeconds.ToString());
    //  lastCapturedTime = DateTime.Now;
    //}

    //private void showTimeCapture() {
    //  MessageBox.Show(timingString.ToString());
    //}

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
      string key = fiModel.TableName + "-" + actionName;
      if (CustomizedRowActions != null && CustomizedRowActions.Any(x => x.Key.EqualsIgnoreCase(key))) {
        var customizedRowAction = CustomizedRowActions.FirstOrDefault(x => x.Key.EqualsIgnoreCase(key));
        customizedRowAction.Value(cid, identifiers);
      }
    }

    //Unlike Aiwe, this does not need to have action filter check, because those who have no access to the action, cannot see the button for it
    private void runCustomizedTableActionFor(string actionName) {
      string key = fiModel.TableName + "-" + actionName;
      if (CustomizedTableActions != null && CustomizedTableActions.Any(x => x.Key.EqualsIgnoreCase(key))) {
        var customizedTableAction = CustomizedTableActions.FirstOrDefault(x => x.Key.EqualsIgnoreCase(key));
        customizedTableAction.Value(fiModel, Filters);
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
          var allowedRowActions = Aibe.DH.DefaultNonCreateRowActions.Union(Aibe.DH.DefaultGroupByNonCreateRowActions).ToList();
          if (!allowedRowActions.Any(x => x.EqualsIgnoreCase(tag.ActionName)))
            return;
          if (tag.Cid <= 0 && Aibe.DH.DefaultNonCreateRowActions.Any(x => x.EqualsIgnoreCase(tag.ActionName)))
            return;
          if ((tag.Identifiers == null || tag.Identifiers.Count <= 0) && Aibe.DH.DefaultGroupByNonCreateRowActions.Any(
            x => x.EqualsIgnoreCase(tag.ActionName)))
            return;
          if (fiModel.Meta.IsGroupTable) {
            if (tag.ActionName.EqualsIgnoreCase(Aibe.DH.EditGroupActionName)) { //Do edit group
              groupByRowActionEditGroup(tag.Identifiers);
            } else if (tag.ActionName.EqualsIgnoreCase(Aibe.DH.DeleteGroupActionName)) { //Do delete group
              groupByRowActionDeleteGroup(tag.Identifiers);
            } else if (tag.ActionName.EqualsIgnoreCase(Aibe.DH.GroupDetailsActionName)) { //Do details group
              groupByRowActionGroupDetails(tag.Identifiers);
            }
          } else {
            if (tag.ActionName.EqualsIgnoreCase(Aibe.DH.EditActionName)) { //Do edit
              rowActionEdit(tag.Cid);
            } else if (tag.ActionName.EqualsIgnoreCase(Aibe.DH.DetailsActionName)) { //Do details
              rowActionDetails(tag.Cid);
            } else if (tag.ActionName.EqualsIgnoreCase(Aibe.DH.DeleteActionName)) { //Do delete
              rowActionDelete(tag.Cid);
            } else if (tag.ActionName.EqualsIgnoreCase(Aibe.DH.DownloadAttachmentsActionName)) { //Do download attachments
              rowActionDownloadAttachments(tag.Cid);
            }
          } 
        } else { //Handle not default action here! Perhaps table name is also necessary to determine the action...
          runCustomizedRowActionFor(tag.ActionName, tag.Cid, tag.Identifiers);
        }
      }
    }

    private void initialize(AideFilterIndexModel model) { //whatever requires initialization only once goes here
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
      buttonCreate.Text = fiModel.Meta.IsGroupTable ? Aibe.LCZ.W_CreateGroup : Aibe.LCZ.W_Create;
      buttonFilter.Text = Aibe.LCZ.W_Filter;
      linkLabelFirst.Text = Aibe.LCZ.W_First;
      linkLabelLast.Text = Aibe.LCZ.W_Last;
      linkLabelNext.Text = Aibe.LCZ.W_NextSymbol;
      linkLabelPrev.Text = Aibe.LCZ.W_PreviousSymbol;
      base.Text = Aibe.LCZ.W_Index;
      labelAction.Text = string.Concat("(", Aibe.LCZ.W_Index, ")");
    }

    private void visibility() {
      linkLabelNext100.Visible = fiModel.NavData.MaxPage > 100;
      linkLabelPrev100.Visible = fiModel.NavData.MaxPage > 100;
      linkLabelNext10.Visible = fiModel.NavData.MaxPage > 10;
      linkLabelPrev10.Visible = fiModel.NavData.MaxPage > 10;
    }

    private void applyModel(AideFilterIndexModel model) {
      fiModel = model;
      dataGridViewTable.Rows.Clear();
      dataGridViewTable.Columns.Clear();
      localization();
      visibility();

      Text += " - " + model.TableDisplayName;
      labelTitle.Text = model.TableDisplayName;

      buttonFilter.Visible = !model.Meta.FilterIsDisabled;
      labelFilterMessage.Visible = !model.Meta.FilterIsDisabled;
      labelFilterMessage.Text = model.FiModel.FilterNo > 0 ? "[" + model.FiModel.FilterNo.ToString() + "]" : string.Empty;

      labelNavDataValue.Text = Aibe.LCZ.W_Page + " " + model.NavData.CurrentPage + " / " + model.NavData.MaxPage
        + ", " + Aibe.LCZ.W_Data + " " + model.NavData.ItemNoInPageFirst + "-" + 
        model.NavData.ItemNoInPageLast + " " + Aibe.LCZ.W_of + " " + model.NavData.QueryCount + " " + Aibe.LCZ.W_Data;

      bool hasData = model.HasTable && model.RowNo > 0;
      buttonCreate.Visible = model.Meta.HasAnyCreateAction;
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

      if (model.Meta.HasIndexNonCreateAction) {
        int index = 1;
        foreach (var action in model.Meta.IndexNonCreateActions) {
          if (!model.IsAllowedToCallAction(action.Name))
            continue;
          bool isDefaultAction = model.Meta.IsDefaultAction(action.Name);
          bool isRowAction = Aibe.DH.DefaultRowActions.Any(x => x.EqualsIgnoreCase(action.Name));
          if (isDefaultAction && isRowAction && model.Meta.IsGroupTable)
            continue; //cannot process default row action when meta is actually a group table here
          DataGridViewColumn dgvColumn = new DataGridViewButtonColumn() {
            HeaderText = string.Empty,
          };
          dgvColumn.ReadOnly = true;
          dataGridViewTable.Columns.Add(dgvColumn);
          ++index;
        }
      }

      int rowIndex = 0; //v1.4.1.0
      foreach (DataRow row in model.IndexRows) {
        int id = 0; //record id
        List<KeyValuePair<string, object>> identifiers = new List<KeyValuePair<string, object>>();
        if (model.Meta.IsGroupTable) {
          foreach (var identifierColumn in model.FiModel.IdentifierColumns) {
            //object identifier = row[identifierColumn];
            DataRow identifierRow = model.IdentifierIndexRows[rowIndex]; //v1.4.1.0 to distinguish true identifiers for group table from what are displayed in the index view
            object identifier = identifierRow[identifierColumn];
            identifiers.Add(new KeyValuePair<string, object>(identifierColumn, identifier));
          }
        } else {
          object cid = row[Aibe.DH.Cid];
          if (cid != null)
            id = (int)cid;
        }
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
          } else if (columnInfo.IsIndexShowImage) { //cannot be group by, because Id is not available in that
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

        if (model.Meta.HasIndexNonCreateAction) {
          var actions = model.Meta.IndexNonCreateActions.ToArray();
          foreach (var action in actions) {
            if (!model.IsAllowedToCallAction(action.Name))
              continue;
            bool isDefaultAction = model.Meta.IsDefaultAction(action.Name);
            bool isRowAction = Aibe.DH.DefaultRowActions.Any(x => x.EqualsIgnoreCase(action.Name));
            if (isDefaultAction && isRowAction && model.Meta.IsGroupTable)
              continue; //cannot process default row action when meta is actually a group table here
            if (isDefaultAction) { //using default actions
              if (id != 0 || identifiers.Count > 0) {
                DataGridViewCell cell = new DataGridViewButtonCell() {
                  Value = Aibe.LCZ.GetLocalizedDefaultActionName(action.Name).ToCamelBrokenString(),
                  Tag = new ButtonTag() { UseDefaultAction = true, ActionName = action.Name, Cid = id, Identifiers = identifiers },
                };
                dgvRow.Cells.Add(cell);
              }
            } else {
              DataGridViewCell cell = new DataGridViewButtonCell() {
                Value = action.Name.ToCamelBrokenString(),
                Tag = new ButtonTag() { UseDefaultAction = false, ActionName = action.Name, Cid = id, Identifiers = identifiers },
              };
              dgvRow.Cells.Add(cell);
            }
          }
        }
        dataGridViewTable.Rows.Add(dgvRow);
        ++rowIndex; //v1.4.1.0
      }
    }

    private void LinkLabelTableAction_Click(object sender, EventArgs e) {
      if (!fiModel.Meta.HasTableAction || !(sender is LinkLabel))
        return;
      LinkLabel linkLabel = (LinkLabel)sender;
      if (linkLabel.Tag == null || string.IsNullOrWhiteSpace(linkLabel.Tag.ToString()))
        return;
      string tableActionName = linkLabel.Tag.ToString();
      if (!fiModel.IsAllowedToCallTableAction(tableActionName))
        return;
      if (!fiModel.Meta.IsDefaultTableAction(tableActionName)) {
        runCustomizedTableActionFor(tableActionName);
        return;
      }
      //using default table actions
      bool result;
      if (tableActionName.EqualsIgnoreCase(Aibe.DH.ExportToCSVTableActionName)) {
        var model = CommonLogic.GetFilterIndexModel(fiModel.TableName, fiModel.NavData.CurrentPage,
          fiModel.BaseModel.StringDictionary, loadAllData: false);
        result = CommonLogic.ExportToCsv(model, fiModel.TableName, FileHelper.GetDownloadFolderPath());
        string format = result ? Aibe.LCZ.M_TableActionSuccess : Aibe.LCZ.E_TableActionUnsuccessful;
        string title = result ? Aibe.LCZ.W_Successful : Aibe.LCZ.W_Error;
        MessageBoxIcon icon = result ? MessageBoxIcon.Information : MessageBoxIcon.Warning;
        MessageBox.Show(string.Format(format, getTableActionText(tableActionName)), title, MessageBoxButtons.OK, icon);
      } else if (tableActionName.EqualsIgnoreCase(Aibe.DH.ExportAllToCSVTableActionName)) {
        var model = CommonLogic.GetFilterIndexModel(fiModel.TableName, fiModel.NavData.CurrentPage,
          fiModel.BaseModel.StringDictionary, loadAllData: true);
        result = CommonLogic.ExportAllToCsv(model, fiModel.TableName, FileHelper.GetDownloadFolderPath());
        string format = result ? Aibe.LCZ.M_TableActionSuccess : Aibe.LCZ.E_TableActionUnsuccessful;
        string title = result ? Aibe.LCZ.W_Successful : Aibe.LCZ.W_Error;
        MessageBoxIcon icon = result ? MessageBoxIcon.Information : MessageBoxIcon.Warning;
        MessageBox.Show(string.Format(format, getTableActionText(tableActionName)), title, MessageBoxButtons.OK, icon);
      }
      //Implement default table actions here!
    }

    private void disposeFormAndApplyUpdate(Form form, AideFilterIndexModel model) {
      if (form != null) {
        applyModelWithUiUpdate(model);
        form.Dispose();
      }
    }

    private void applyModelWithUiUpdate(AideFilterIndexModel model) {
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
        if (!string.IsNullOrWhiteSpace(fiModel.FiModel.FilterMsg))
          toolTip.SetToolTip(labelFilterMessage, fiModel.FiModel.FilterMsg);
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
    private void CommonIndexForm_Load(object sender, System.EventArgs e) {
      if (!hasToolTipBeenSet) {
        toolTip = new ToolTip();

        toolTip.InitialDelay = 2000;
        toolTip.ReshowDelay = 1000;
        toolTip.ShowAlways = false;

        if (!string.IsNullOrWhiteSpace(fiModel.FiModel.FilterMsg))
          toolTip.SetToolTip(labelFilterMessage, fiModel.FiModel.FilterMsg);

        hasToolTipBeenSet = true;
      }

      uiFinalTouch();
    }

    private string createRowActionErrorModel(AideBaseTableModel model, string localizedAction) {
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
    private void rowActionCommonCreateEdit(bool isCreate, int cid) {
      BaseErrorModel errorModel = isCreate ? 
        CommonLogic.Create(fiModel.Meta.TableName, identifiers: null) :
        CommonLogic.Edit(fiModel.Meta.TableName, identifiers: null, id: cid); //create does not have table to be put
      if (errorModel.HasError) {
        MessageBox.Show(errorModel.Message, Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      AideCreateEditModel model = (AideCreateEditModel)errorModel.ReturnObject;

      CommonCreateEditForm form = new CommonCreateEditForm(model);
      DialogResult result = DialogResult.Retry;

      while (result == DialogResult.Retry) {
        result = form.ShowDialog();
        if (result != DialogResult.OK)
          return;
        AideRequestModel request = form.CreateRequestModel();
        if (!isCreate && cid > 0)
          request.Cid = cid;
        errorModel = isCreate ? //it is non-erroneous, otherwise it cannot come here in the first place
          CommonLogic.Create(model.TableName, identifiers: null, model: request) : //this is an attempt to create a new item based on the form collection gathered
          CommonLogic.Edit(model.TableName, identifiers: null, model: request); //null identifiers here to indicate "don't put values on the identifiers, even there is any"
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
      disposeFormAndApplyUpdate(form, FormHelper.GetUpdatedCommonIndexModel(model.TableName,
        fiModel.NavData.CurrentPage, Filters));
    }

    private void rowActionCreate() {
      rowActionCommonCreateEdit(isCreate: true, cid: -1);
    }

    private void rowActionEdit(int cid) {
      rowActionCommonCreateEdit(isCreate: false, cid: cid);
    }

    private void rowActionDetails(int cid) {
      BaseErrorModel errorModel = CommonLogic.Details(fiModel.Meta.TableName, cid);
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

    private BaseErrorModel commonGroupByRowActionCreateEditGroup(bool isCreate, List<KeyValuePair<string, object>> identifiers) {
      BaseErrorModel errorModel = isCreate ? 
        CommonLogic.CreateGroup(fiModel.Meta.TableName, fiModel.Meta.GroupByColumns) :
        CommonLogic.EditGroup(fiModel.Meta.TableName, identifiers);
      if (errorModel.HasError) {
        MessageBox.Show(errorModel.Message, Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return null; //indicates true return, not to progress
      }
      AideCreateEditGroupModel model = (AideCreateEditGroupModel)errorModel.ReturnObject;

      CommonCreateEditGroupForm form = new CommonCreateEditGroupForm(model);
      DialogResult result = DialogResult.Retry;

      while (result == DialogResult.Retry) {
        if (!model.Meta.IsGroupByFullyAutomatic()) { //only if it is not fully automatic, it needs to fill anything in the form
          result = form.ShowDialog();
          if (result != DialogResult.OK) {
            form.Dispose();
            form = null;
            return null;
          }
        }
        AideRequestModel request = form.CreateRequestModel();
        errorModel = isCreate ? 
          CommonLogic.CreateGroup(model.TableName, model.IdentifierColumns, request) :
          CommonLogic.EditGroup(model.TableName, identifiers, request);
        if (errorModel.HasError) { //error due to access problem, thus return, not even try to retry
          MessageBox.Show(errorModel.ToShortString(Aibe.LCZ.W_ErrorModelCodeWord, Aibe.LCZ.W_ErrorModelMessageWord),
            Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
          form.Dispose();
          form = null;
          return null;
        }
        if (errorModel.ReturnObject is AideCreateEditGroupModel) {
          model = (AideCreateEditGroupModel)errorModel.ReturnObject;
          if (!model.IsSuccessful) { //unsuccessful... but not due to access problem, thus may retry
            string errorMessage = createRowActionErrorModel(model, isCreate ? Aibe.LCZ.W_CreateGroup : Aibe.LCZ.W_EditGroup);
            result = MessageBox.Show(errorMessage, Aibe.LCZ.W_Error, MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
          }
        }
      }

      form.Dispose();
      form = null;

      return errorModel;
    }

    private void groupByRowActionCreateGroup() {
      BaseErrorModel errorModel = commonGroupByRowActionCreateEditGroup(isCreate: true, identifiers: null);
      if (errorModel == null)
        return;
      //the correct item does not return AideCreateEditGroupModel but list of pairs as identifiers
      List<KeyValuePair<string, object>> identifiers = (List<KeyValuePair<string, object>>)errorModel.ReturnObject;
      commonGroupByDetails(identifiers, isGroupDeletion: false); //following the normal procedure of GroupByDetails
    }

    private void groupByRowActionEditGroup(List<KeyValuePair<string, object>> identifiers) {
      BaseErrorModel errorModel = commonGroupByRowActionCreateEditGroup(isCreate: false, identifiers: identifiers);
      if (errorModel == null)
        return;
      List<KeyValuePair<string, object>> unreplacedIdentifierResults = (List<KeyValuePair<string, object>>)errorModel.ReturnObject; //the return object is not right for the automatic items here...
      int result = fiModel.Meta.ApplyEditGroup(identifiers, unreplacedIdentifierResults);
      if (result < 0)
        return;
      AideFilterIndexModel afiModel = FormHelper.GetUpdatedCommonIndexModel(fiModel.Meta.TableName, fiModel.NavData.CurrentPage, Filters);
      applyModelWithUiUpdate(afiModel);
    }

    private void commonGroupByDetails(List<KeyValuePair<string, object>> identifiers, bool isGroupDeletion) {
      BaseErrorModel errorModel = CommonLogic.GroupDetails(fiModel.Meta.TableName, identifiers, isGroupDeletion: isGroupDeletion);
      if (errorModel.HasError) {
        MessageBox.Show(errorModel.Message, Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      AideFilterGroupDetailsModel model = (AideFilterGroupDetailsModel)errorModel.ReturnObject;
      CommonGroupDetailsForm form = new CommonGroupDetailsForm(model);
      form.ShowDialog();

      //This should work based on
      //https://stackoverflow.com/questions/3861602/how-to-check-if-a-windows-form-is-already-open-and-close-it-if-it-is      
      disposeFormAndApplyUpdate(form, FormHelper.GetUpdatedCommonIndexModel(model.TableName,
        fiModel.NavData.CurrentPage, Filters));
    }

    private void groupByRowActionDeleteGroup(List<KeyValuePair<string, object>> identifiers) {
      commonGroupByDetails(identifiers, isGroupDeletion: true);
    }

    private void groupByRowActionGroupDetails(List<KeyValuePair<string, object>> identifiers) {
      commonGroupByDetails(identifiers, isGroupDeletion: false);
    }

    private void rowActionDelete(int cid) {
      BaseErrorModel errorModel = CommonLogic.Delete(fiModel.Meta.TableName, cid);
      if (errorModel.HasError) {
        MessageBox.Show(errorModel.Message, Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      AideDetailsModel model = (AideDetailsModel)errorModel.ReturnObject;
      CommonDetailsForm form = new CommonDetailsForm(model);
      DialogResult result = form.ShowDialog();
      if (result != DialogResult.OK)
        return;
      CommonLogic.DeletePost(fiModel.Meta.TableName, cid); //no need to check, currently it is always successful
      disposeFormAndApplyUpdate(form, FormHelper.GetUpdatedCommonIndexModel(model.TableName,
        fiModel.NavData.CurrentPage, Filters)); //no need to check this error, because it must be successful if someone can open a CommonIndex in the first place
    }

    private void buttonCreate_Click(object sender, EventArgs e) {
      if (!fiModel.Meta.IsGroupTable && fiModel.Meta.IsDefaultAction(Aibe.DH.CreateActionName))
        rowActionCreate();
      else if (fiModel.Meta.IsGroupTable && fiModel.Meta.IsDefaultAction(Aibe.DH.CreateGroupActionName))
        groupByRowActionCreateGroup();
      else {
        List<KeyValuePair<string, object>> identifiers = fiModel.Meta.GroupByColumns
          .Select(x => new KeyValuePair<string, object>(x, null)).ToList();
        runCustomizedRowActionFor(Aibe.DH.CreateActionName, -1, identifiers); //-1 indicates non-availability
      }
    }

    private void buttonFilter_Click(object sender, EventArgs e) {
      CommonFilterForm form = new CommonFilterForm(fiModel);
      DialogResult result = form.ShowDialog();
      if (result != DialogResult.OK)
        return;
      AideRequestModel request = form.CreateRequestModel();
      if (request != null && request.FormCollection != null)
        Filters = request.FormCollection;
      disposeFormAndApplyUpdate(form, FormHelper.GetUpdatedCommonIndexModel(fiModel.TableName,
        fiModel.NavData.CurrentPage, Filters));
    }

    #region Navigation
    private void linkLabelFirst_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {      
      applyModelWithUiUpdate(FormHelper.GetUpdatedCommonIndexModel(fiModel.TableName, 1, Filters));
    }

    private void linkLabelPrev100_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {      
      applyModelWithUiUpdate(FormHelper.GetUpdatedCommonIndexModel(fiModel.TableName, fiModel.NavData.Prev100Page, Filters));
    }

    private void linkLabelPrev10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {     
      applyModelWithUiUpdate(FormHelper.GetUpdatedCommonIndexModel(fiModel.TableName, fiModel.NavData.Prev10Page, Filters));
    }

    private void linkLabelPrev_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {     
      applyModelWithUiUpdate(FormHelper.GetUpdatedCommonIndexModel(fiModel.TableName, fiModel.NavData.PrevPage, Filters));
    }

    private void linkLabelNext_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {      
      applyModelWithUiUpdate(FormHelper.GetUpdatedCommonIndexModel(fiModel.TableName, fiModel.NavData.NextPage, Filters));
    }

    private void linkLabelNext10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {      
      applyModelWithUiUpdate(FormHelper.GetUpdatedCommonIndexModel(fiModel.TableName, fiModel.NavData.Next10Page, Filters));
    }

    private void linkLabelNext100_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {      
      applyModelWithUiUpdate(FormHelper.GetUpdatedCommonIndexModel(fiModel.TableName, fiModel.NavData.Next100Page, Filters));
    }

    private void linkLabelLast_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {      
      applyModelWithUiUpdate(FormHelper.GetUpdatedCommonIndexModel(fiModel.TableName, fiModel.NavData.MaxPage, Filters));
    }
    #endregion
  }
}

//<span><i>(Data [@Model.ItemNoInPageFirst] - [@Model.ItemNoInPageLast] of <b>@Model.QueryCount</b> Data)</i></span>

//Value = "[" + Aibe.LCZ.W_NA + "]",

//dgvRow.Cells.Add(new DataGridViewTextBoxCell() {
//  ReadOnly = true,
//  Value = val,
//});

//Width = dataGridViewTable.Width + dataGridViewTable.Margin.Left + dataGridViewTable.Margin.Right + Padding.Right + Padding.Left + Margin.Left + Margin.Right;

//In the website, this is supposed to become customized controller's action
//<a href="../../../../../../@Model.TableName/@action.Name/@id">@action.Name.ToCamelBrokenString()</a>

//CommonCreateEditForm form = new CommonCreateEditForm(model);
//DialogResult result = DialogResult.Retry;

//while (result == DialogResult.Retry) {
//  result = form.ShowDialog();
//  if (result != DialogResult.OK)
//    return;
//  AideRequestModel request = form.CreateRequestModel();
//  model = CommonLogic.Create(model.TableName, request); //this is an attempt to create a new item based on the form collection gathered
//  if (!model.IsSuccessful) { //unsuccessful...
//    string errorMessage = createRowActionErrorModel(model, Aibe.LCZ.W_Create);
//    result = MessageBox.Show(errorMessage, Aibe.LCZ.W_Error, MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
//  }
//}

////This should work based on
////https://stackoverflow.com/questions/3861602/how-to-check-if-a-windows-form-is-already-open-and-close-it-if-it-is
//closeFormAndApplyUpdate(form, CommonLogic.Index(model.TableName, fiModel.NavData.CurrentPage, Filters));

//CommonCreateEditForm form = new CommonCreateEditForm(model);
//DialogResult result = DialogResult.Retry;

//while (result == DialogResult.Retry) {
//  result = form.ShowDialog();
//  if (result != DialogResult.OK)
//    return;
//  AideRequestModel request = form.CreateRequestModel();
//  request.Cid = cid; //add cid in the request model
//  model = CommonLogic.Edit(model.TableName, request); //this is an attempt to create a new item based on the form collection gathered
//  if (!model.IsSuccessful) { //unsuccessful...
//    string errorMessage = createRowActionErrorModel(model, Aibe.LCZ.W_Edit);
//    result = MessageBox.Show(errorMessage, Aibe.LCZ.W_Error, MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
//  }
//}

//if (form != null) {
//  applyModel(CommonLogic.Index(model.TableName, fiModel.NavData.CurrentPage, Filters)); //actually, can open current page too...
//  uiFinalTouch();
//  form.Close();
//}

//DataPropertyName = "CommonAction" + index,
//Name = "CommonAction" + index,

//DataGridViewRow headerRow = new DataGridViewRow();

//public CommonIndexForm(AideFilterIndexModel model, Dictionary<string, CustomizedRowActionDelegate> customizedRowActions) : this(model) {
//  if (customizedRowActions != null)
//    CustomizedRowActions = customizedRowActions;
//}

//public CommonIndexForm(AideFilterIndexModel model, Dictionary<string, CustomizedTableActionDelegate> customizedTableActions) : this(model) {
//  if (customizedTableActions != null)
//    CustomizedTableActions = customizedTableActions;
//}

//public CommonIndexForm(AideFilterIndexModel model, Dictionary<string, CustomizedRowActionDelegate> customizedRowActions, Dictionary<string, CustomizedTableActionDelegate> customizedTableActions) : this(model, customizedRowActions) {
//  if (customizedTableActions != null)
//    CustomizedTableActions = customizedTableActions;
//}

//  CommonLogic.EditGroup(fiModel.Meta.TableName, identifiers);
//if (errorModel.HasError) {
//  MessageBox.Show(errorModel.Message, Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
//  return;
//}
//AideCreateEditGroupModel model = (AideCreateEditGroupModel)errorModel.ReturnObject;

//CommonCreateEditGroupForm form = new CommonCreateEditGroupForm(model);
//DialogResult result = DialogResult.Retry;

//while (result == DialogResult.Retry) {
//  if (!model.Meta.IsGroupByFullyAutomatic()) { //only if it is not fully automatic, it needs to fill anything in the form
//    result = form.ShowDialog();
//    if (result != DialogResult.OK) {
//      form.Dispose();
//      form = null;
//      return;
//    }
//  }
//  AideRequestModel request = form.CreateRequestModel();
//  errorModel = CommonLogic.EditGroup(model.TableName, identifiers, request);
//  if (errorModel.HasError) { //error due to access problem, thus return, not even try to retry
//    MessageBox.Show(errorModel.ToShortString(Aibe.LCZ.W_ErrorModelCodeWord, Aibe.LCZ.W_ErrorModelMessageWord),
//      Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
//    form.Dispose();
//    form = null;
//    return;
//  }
//  if (errorModel.ReturnObject is AideCreateEditGroupModel) {
//    model = (AideCreateEditGroupModel)errorModel.ReturnObject;
//    if (!model.IsSuccessful) { //unsuccessful... but not due to access problem, thus may retry
//      string errorMessage = createRowActionErrorModel(model, Aibe.LCZ.W_EditGroup);
//      result = MessageBox.Show(errorMessage, Aibe.LCZ.W_Error, MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
//    }
//  }
//}

//form.Dispose();
//form = null;
