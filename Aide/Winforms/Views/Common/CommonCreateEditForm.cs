using Aide.Logics;
using Aibe.Models;
using Aibe.Models.Core;
using Aide.Models.Controls;
using Aide.Models;
using Aide.Winforms.Components;
using Aide.Winforms.Helpers;
using Aide.Winforms.Models;
using AWF = Aide.Winforms.SH;
using Extension.String;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Linq;

namespace Aide.Winforms {
  public partial class CommonCreateEditForm : Form {
    AideCreateEditModel model { get; set; }
    public int TotalWidth { get; set; } = AWF.BaseWindowsWidth; //base width
    public int TotalHeight { get; set; } = AWF.BaseWindowsHeight; //base height
    private List<SingleItemPanel> dropdownPanels = new List<SingleItemPanel>();
    private List<SingleItemPanel> listColumnPanels = new List<SingleItemPanel>();
    private List<SingleItemPanel> allPanels = new List<SingleItemPanel>();

    public CommonCreateEditForm(AideCreateEditModel model) {
      InitializeComponent();
      this.model = model; //yes, duplicate of what is inside of applyModel because localization() will need this too...
      localization();
      applyModel(model);
    }

    private void localization() {
      buttonClose.Text = Aibe.LCZ.W_Close;
      buttonPerformAction.Text = Aibe.LCZ.GetLocalizedDefaultActionName(model.ActionType);
      if(model.GroupDetailsOrigin)
        labelAction.Text = string.Concat("(", buttonPerformAction.Text, ": ", 
          string.Join(", ", model.Identifiers.Select(x => x.Value)), ")");
      else
        labelAction.Text = string.Concat("(", buttonPerformAction.Text, ")");
    }

    //Hidden filled item does not show its foreign info, even if there is any
    private void addHiddenFilledItem(string columnName, object value) {
      SingleItemPanel item = new SingleItemPanel(new SingleItemPanelModel() {
        Name = columnName, DisplayName = model.Meta.GetColumnDisplayName(columnName),
        ItemType = SingleItemPanelType.Display,
        IsHidden = true,
        Arg = value,
      });
      flowLayoutPanelContent.Controls.Add(item);
    }

    private void applyModel(AideCreateEditModel model) {
      this.model = model;
      flowLayoutPanelContent.Controls.Clear();

      Text = Aibe.LCZ.GetLocalizedDefaultActionName(model.ActionType) + " - " + model.TableDisplayName;
      labelTitle.Text = model.TableDisplayName;
      TotalWidth = Math.Max(getInitialTopWidth(), TotalWidth);

      bool isCreate = model.ActionType.EqualsIgnoreCase(Aibe.DH.CreateActionName);
      foreach (DataColumn column in model.Meta.ArrangedDataColumns) {
        string cn = column.ColumnName;
        bool isScriptColumn = model.Meta.IsScriptColumn(cn);
        bool isReadOnly = (!isCreate && model.Meta.IsEditShowOnly(cn)) || isScriptColumn;
        bool isForeignKey = model.Meta.IsForeignInfoColumn(cn);
        string dataType = column.DataType.ToString().Substring(Aibe.DH.SharedPrefixDataType.Length);

        //Cid is specially handled as a hidden input only for Edit
        if (cn.EqualsIgnoreCase(Aibe.DH.Cid) && model.ActionType.EqualsIgnoreCase(Aibe.DH.EditActionName)) {
          addHiddenFilledItem(cn, model.GetData(Aibe.DH.Cid));
          continue;
        }

        //specially entreated items, like prefilled columns
        if (model.GroupDetailsOrigin && model.IsColumnIdentifier(cn)) { 
          addHiddenFilledItem(cn, model.GetIdentifierValueFor(cn));
          continue;
        }

        //excluded column
        if (cn.EqualsIgnoreCase(Aibe.DH.Cid) || !model.IsColumnIncludedInCreateEdit(column)) { 
          PrefilledColumnInfo pcInfo = model.Meta.GetPrefilledColumn(cn); //Check if it is prefilled columns
          if (pcInfo != null && isCreate) //is create and is prefilled column, hides it and fill it, as long as it is create
            addHiddenFilledItem(cn, pcInfo.Value);
          continue;
        }

        //script column type, it is always read only
        if (isScriptColumn) {
          ScTableInfo scTable = model.GetScTable(cn);
          bool isNA = scTable == null || !scTable.IsValid;
          SingleItemPanel item = new SingleItemPanel(new SingleItemPanelModel() {
            Name = cn, DisplayName = model.Meta.GetColumnDisplayName(cn),
            IsReadOnly = true,
            IsNotAvailable = isNA, //to distinguish true display (read only item, such as to be used in Details) from false display (nothing to be obtained) //not sure if it is good, but leave it for now...
            ItemType = isNA ? SingleItemPanelType.Display : SingleItemPanelType.ScTable,
            ActionType = model.ActionType, //need for ScTable and List
            Arg = isNA ? "[" + Aibe.LCZ.W_NA + "]" : null, //no argument needed for real ScTable
            Info = scTable,
          });
          FormHelper.ProcessItemWithItsForeignInfos(cn, item, model, flowLayoutPanelContent, 
            Aibe.DH.CreateEditFilterPageName, Aide.PH.CreateEditFilterDateTimeFormat);
          continue;
        }

        if (dataType.EqualsIgnoreCase(Aibe.DH.StringDataType)) { //String type
          string dataValue = model.GetData(cn);
          if (isCreate && string.IsNullOrWhiteSpace(dataValue)) {
            string prefix = model.Meta.GetPrefix(cn);
            string postfix = model.Meta.GetPostfix(cn);
            //according to http://stackoverflow.com/questions/637308/why-is-adding-null-to-a-string-legal
            //The addition of null using + is legal;
            dataValue = prefix + dataValue + postfix;
          }

          //ordered by precedence
          bool isDropDown = model.Meta.IsCreateEditDropDownColumn(cn);
          bool isTextField = model.Meta.IsTextField(cn);
          bool isPictureColumn = model.Meta.IsPictureColumn(cn);
          bool isNonPictureAttachment = model.Meta.IsNonPictureAttachmentColumn(cn);
          bool isListColumn = model.Meta.IsListColumn(cn);
          SingleItemPanel item;

          if (isDropDown) {
            List<string> dropdowns = model.Meta.GetStaticCreateEditDropDownFor(cn, dataValue, Aibe.DH.DropDownTextDataType);
            if (dropdowns == null || dropdowns.Count <= 0 || isReadOnly) { //because dropdown cannot be read only
              item = new SingleItemPanel(new SingleItemPanelModel {
                Name = cn, DisplayName = model.Meta.GetColumnDisplayName(cn),
                ItemType = SingleItemPanelType.Text,
                IsReadOnly = isReadOnly,
                IsForeignKey = isForeignKey,
                Arg = dataValue,
              });
              item.ValueChanged += Item_ValueChanged;
            } else { //string dropdown case
              item = new SingleItemPanel(new SingleItemPanelModel {
                Name = cn, DisplayName = model.Meta.GetColumnDisplayName(cn),
                ItemType = SingleItemPanelType.DropDown, //Cannot be read-only, does not work
                IsForeignKey = isForeignKey,
                Info = model.Meta.GetCreateEditDropDownColumnInfo(cn),
                Arg = new ComboBoxModel() {
                  Options = dropdowns,
                  OriginalChosenItem = dataValue, DataType = Aibe.DH.DropDownTextDataType
                }
              });
              item.ComboBoxSelectedIndexChanged += Item_ComboBoxSelectedIndexChanged;
            }
            FormHelper.ProcessItemWithItsForeignInfos(cn, item, model, flowLayoutPanelContent,
              Aibe.DH.CreateEditFilterPageName, Aide.PH.CreateEditFilterDateTimeFormat);
            continue;
          }

          if (isTextField) {
            TextFieldColumnInfo tfcInfo = model.Meta.GetTextFieldInfo(cn);
            item = new SingleItemPanel(new SingleItemPanelModel {
              Name = cn, DisplayName = model.Meta.GetColumnDisplayName(cn),
              ItemType = SingleItemPanelType.TextField,
              IsReadOnly = isReadOnly, //can handle this well
              IsForeignKey = isForeignKey,
              Arg = dataValue,
              Info = tfcInfo,
            });
            item.ValueChanged += Item_ValueChanged;
            FormHelper.ProcessItemWithItsForeignInfos(cn, item, model, flowLayoutPanelContent,
              Aibe.DH.CreateEditFilterPageName, Aide.PH.CreateEditFilterDateTimeFormat);
            continue;
          }

          if (isPictureColumn) {
            string picColumnName = cn + Aibe.DH.CreateEditPictureLinkAppendixName;
            string picColumnValue = model.GetData(picColumnName);
            string columnValue = model.GetData(cn, false);

            if (!string.IsNullOrWhiteSpace(columnValue)) { //try to load the image
              string cid = model.GetData(Aibe.DH.Cid);
              string fullRelativePath = columnValue.Contains("/") || columnValue.Contains("\\") ?
                columnValue : //longer relative (folder-contain) path
                model.TableName + "\\" + cid + "\\" + columnValue; //shorter path
              item = new SingleItemPanel(new SingleItemPanelModel {
                Name = cn, //can use the actual column name here, for that will be used to store the picture link
                DisplayName = model.Meta.GetColumnDisplayName(cn),
                ItemType = SingleItemPanelType.Picture,
                IsReadOnly = isReadOnly,
                Info = model.Meta.GetPictureColumnInfo(cn),
                Arg = fullRelativePath,
                TableName = model.Meta.TableName,
                Cid = int.Parse(cid),
              });
              item.PanelItemResized += panelItem_Resized;
            } else { //no image to load
              item = new SingleItemPanel(new SingleItemPanelModel {
                Name = cn,
                DisplayName = model.Meta.GetColumnDisplayName(cn),
                ItemType = SingleItemPanelType.Picture,
                IsReadOnly = isReadOnly,
                Info = model.Meta.GetPictureColumnInfo(cn),
              });
              item.PanelItemResized += panelItem_Resized;
            }
            FormHelper.ProcessItemWithItsForeignInfos(cn, item, model, flowLayoutPanelContent,
              Aibe.DH.CreateEditFilterPageName, Aide.PH.CreateEditFilterDateTimeFormat);
            continue;
          }

          if (isNonPictureAttachment) {
            string attColumnName = cn + Aibe.DH.CreateEditNonPictureAttachmentAppendixName;
            string attColumnValue = model.GetData(attColumnName);
            string columnValue = model.GetData(cn, false);

            if (!string.IsNullOrWhiteSpace(columnValue)) { //try to load the image
              string cid = model.GetData(Aibe.DH.Cid);
              string fullRelativePath = columnValue.Contains("/") || columnValue.Contains("\\") ?
                columnValue : //longer relative (folder-contain) path
                model.TableName + "\\" + cid + "\\" + columnValue; //shorter path
              item = new SingleItemPanel(new SingleItemPanelModel {
                Name = cn, //can use the actual column name here, for that will be used to store the picture link
                DisplayName = model.Meta.GetColumnDisplayName(cn),
                ItemType = SingleItemPanelType.NonPictureAttachment,
                IsReadOnly = isReadOnly,
                Info = model.Meta.GetNonPictureAttachmentColumn(cn),
                Arg = fullRelativePath,
                TableName = model.Meta.TableName,
                Cid = int.Parse(cid),
              });
              item.PanelItemResized += panelItem_Resized;
            } else { //no attachment to load
              item = new SingleItemPanel(new SingleItemPanelModel {
                Name = cn,
                DisplayName = model.Meta.GetColumnDisplayName(cn),
                ItemType = SingleItemPanelType.NonPictureAttachment,
                IsReadOnly = isReadOnly,
                Info = model.Meta.GetNonPictureAttachmentColumn(cn),
              });
              item.PanelItemResized += panelItem_Resized;
            }
            FormHelper.ProcessItemWithItsForeignInfos(cn, item, model, flowLayoutPanelContent,
              Aibe.DH.CreateEditFilterPageName, Aide.PH.CreateEditFilterDateTimeFormat);
            continue;
          }

          if (isListColumn) {
            string lcType = model.Meta.GetListColumnType(cn);
            bool hasStaticTemplate = string.IsNullOrWhiteSpace(dataValue) &&
              model.Meta.ListColumnHasStaticTemplate(cn);
            string usedDataValue = hasStaticTemplate ?
              model.Meta.GetColumnStaticTemplate(cn) : dataValue;
            ListColumnInfo info = model.Meta.GetListColumnInfo(cn);

            item = new SingleItemPanel(new SingleItemPanelModel() {
              Name = cn, DisplayName = model.Meta.GetColumnDisplayName(cn),
              ItemType = SingleItemPanelType.List,
              Info = info,
              ActionType = model.ActionType, //need for ScTable and List
              IsReadOnly = isReadOnly,
              Arg = usedDataValue, //usedDataValue, the original one, is already stored in the Arg, but we may not use it anyway...
            });

          } else { //normal text item
            item = new SingleItemPanel(new SingleItemPanelModel() {
              Name = cn, DisplayName = model.Meta.GetColumnDisplayName(cn),
              ItemType = SingleItemPanelType.Text,
              IsReadOnly = isReadOnly,
              IsForeignKey = isForeignKey,
              Arg = dataValue,
            });
            item.ValueChanged += Item_ValueChanged;
          }

          FormHelper.ProcessItemWithItsForeignInfos(cn, item, model, flowLayoutPanelContent,
            Aibe.DH.CreateEditFilterPageName, Aide.PH.CreateEditFilterDateTimeFormat);
          continue;
        }

        if (Aibe.DH.NumberDataTypes.Contains(dataType)) { //Integer/UInt/Other numbers type
          string dataValue = model.GetData(cn);
          SingleItemPanel item;
          if (model.ActionType.EqualsIgnoreCase(Aibe.DH.CreateActionName) && 
            model.Meta.IsAutoGenerated(cn)) {
            item = new SingleItemPanel(new SingleItemPanelModel() {
              Name = cn, DisplayName = model.Meta.GetColumnDisplayName(cn),
              ItemType = SingleItemPanelType.Number,
              IsHidden = true,
              IsAutoGenerated = true,
              Arg = dataValue,
            });
          } else if (isReadOnly) { //if it is read only, then it must be text
            item = new SingleItemPanel(new SingleItemPanelModel() {
              Name = cn, DisplayName = model.Meta.GetColumnDisplayName(cn),
              ItemType = SingleItemPanelType.Text, //Needs to change to text to get the value from text easily
              IsReadOnly = true,
              IsForeignKey = isForeignKey,
              Arg = dataValue,
            });
            item.ValueChanged += Item_ValueChanged;
          } else { //definitely not read only
            bool isDropDown = model.Meta.IsCreateEditDropDownColumn(cn);
            List<string> dropdowns = model.Meta.GetStaticCreateEditDropDownFor(cn, dataValue);
            if (dropdowns == null || dropdowns.Count <= 0 || !isDropDown) { //normal number
              item = new SingleItemPanel(new SingleItemPanelModel() {
                Name = cn, DisplayName = model.Meta.GetColumnDisplayName(cn),
                ItemType = SingleItemPanelType.Number,
                IsForeignKey = isForeignKey,
                Info = model.Meta.GetNumberLimitColumn(cn),
                Arg = dataValue,
              });
              item.ValueChanged += Item_ValueChanged;
            } else { //number dropdown case
              item = new SingleItemPanel(new SingleItemPanelModel {
                Name = cn, DisplayName = model.Meta.GetColumnDisplayName(cn),
                ItemType = SingleItemPanelType.DropDown, //number dropdown is considered a dropdown
                Info = model.Meta.GetCreateEditDropDownColumnInfo(cn),
                Arg = new ComboBoxModel() {
                  Options = dropdowns,
                  OriginalChosenItem = dataValue, DataType = Aibe.DH.DropDownNumberDataType
                }
              });
              item.ComboBoxSelectedIndexChanged += Item_ComboBoxSelectedIndexChanged;
            }
          }
          FormHelper.ProcessItemWithItsForeignInfos(cn, item, model, flowLayoutPanelContent,
            Aibe.DH.CreateEditFilterPageName, Aide.PH.CreateEditFilterDateTimeFormat);
          continue;
        }

        if (dataType.EqualsIgnoreCase(Aibe.DH.DateTimeDataType)) { //DateTime type
          string dataValueRaw = model.GetData(cn);
          DateTime? dataValueDt = string.IsNullOrWhiteSpace(dataValueRaw) ? null : new DateTime?(DateTime.Parse(dataValueRaw));
          string dateTimeFormat = model.Meta.HasCustomDateTimeFormatFor(cn, Aibe.DH.CreateEditFilterPageName) ?
            model.Meta.GetCustomDateTimeFormatFor(cn, Aibe.DH.CreateEditFilterPageName) :
            Aide.PH.CreateEditFilterDateTimeFormat;
          string dataValue = dataValueDt.HasValue ? 
            dataValueDt.Value.ToString(dateTimeFormat) : string.Empty;
          if (model.Meta.IsTimeStampAppliedFor(cn, model.ActionType)) {
            SingleItemPanel itemDt = new SingleItemPanel(new SingleItemPanelModel() {
              Name = cn, DisplayName = model.Meta.GetColumnDisplayName(cn),
              ItemType = SingleItemPanelType.DateTime,
              IsHidden = true,
              IsTimeStamp = true,
              Arg = dataValueDt,
              DateTimeFormat = dateTimeFormat,
            });
            FormHelper.ProcessItemWithItsForeignInfos(cn, itemDt, model, flowLayoutPanelContent,
              Aibe.DH.CreateEditFilterPageName, Aide.PH.CreateEditFilterDateTimeFormat);
          } else if (isReadOnly) {
            SingleItemPanel item = new SingleItemPanel(new SingleItemPanelModel() {
              Name = cn, DisplayName = model.Meta.GetColumnDisplayName(cn),
              ItemType = SingleItemPanelType.Text, //if it is read only, it cannot be datetime
              IsReadOnly = true, //can handle this well
              Arg = dataValue,
            });
            FormHelper.ProcessItemWithItsForeignInfos(cn, item, model, flowLayoutPanelContent,
              Aibe.DH.CreateEditFilterPageName, Aide.PH.CreateEditFilterDateTimeFormat);
          } else {
            SingleItemPanel item = new SingleItemPanel(new SingleItemPanelModel() {
              Name = cn, DisplayName = model.Meta.GetColumnDisplayName(cn),
              ItemType = SingleItemPanelType.DateTime, //if it is read only, it cannot be datetime
              Arg = dataValueDt,
              DateTimeFormat = dateTimeFormat,
            });
            FormHelper.ProcessItemWithItsForeignInfos(cn, item, model, flowLayoutPanelContent,
              Aibe.DH.CreateEditFilterPageName, Aide.PH.CreateEditFilterDateTimeFormat);
          }
          continue;
        }

        if (dataType.EqualsIgnoreCase(Aibe.DH.BooleanDataType)) { //Boolean type
          string dataValue = model.GetData(column.ColumnName);
          SingleItemPanel item = new SingleItemPanel(new SingleItemPanelModel() {
            Name = cn, DisplayName = model.Meta.GetColumnDisplayName(cn),
            ItemType = SingleItemPanelType.Boolean, //must remain boolean, though it is read only or not...
            IsReadOnly = isReadOnly,
            Arg = Aibe.LCZ.GetLocalizedBooleanOption(dataValue),
          });
          FormHelper.ProcessItemWithItsForeignInfos(cn, item, model, flowLayoutPanelContent,
            Aibe.DH.CreateEditFilterPageName, Aide.PH.CreateEditFilterDateTimeFormat);
          continue;
        }

        //Unknown type
        SingleItemPanel unknownItem = new SingleItemPanel(new SingleItemPanelModel() {
          Name = cn, DisplayName = model.Meta.GetColumnDisplayName(cn),
          ItemType = SingleItemPanelType.Unknown,
          IsReadOnly = true,
          Arg = "[" + Aibe.LCZ.W_Unknown + "]",
        });
        FormHelper.ProcessItemWithItsForeignInfos(cn, unknownItem, model, flowLayoutPanelContent,
          Aibe.DH.CreateEditFilterPageName, Aide.PH.CreateEditFilterDateTimeFormat);
      }

      //To support live dropdown result from the very beginning
      dropdownPanels.Clear();
      listColumnPanels.Clear();
      foreach (Control control in flowLayoutPanelContent.Controls) {
        if (!(control is SingleItemPanel))
          continue;
        SingleItemPanel panel = (SingleItemPanel)control;
        if (panel == null)
          continue;
        allPanels.Add(panel);
        if (panel.Model.ItemType == SingleItemPanelType.DropDown) {
          ComboBox cbItem = panel.GetComboBox();
          if (cbItem == null || cbItem.Tag == null || !(cbItem.Tag is ComboBoxTag))
            continue;
          dropdownPanels.Add(panel);
        } else if (panel.Model.ItemType == SingleItemPanelType.List) {
          DataGridView dgv = panel.GetDgv();
          if (dgv == null)
            continue;
          listColumnPanels.Add(panel);
        }
      }

      foreach (var panel in dropdownPanels) //this may be clashing with ListColumn on retry... need to ensure that this does not happen
        processLiveDropDown(panel.GetComboBox(), false);
    }

    private void Item_ValueChanged(string columnName, string columnValue) {
      processLiveControl(columnName, columnValue);
    }

    private void processLiveControl(string columnName, string columnValue) {
      //If it is a foreign column for dropdown, then we need to set results for other (affected) columns
      if (string.IsNullOrWhiteSpace(columnName) || columnValue == null)
        return;
      bool isForeignColumn = model.Meta.IsForeignInfoColumn(columnName);
      if (!isForeignColumn)
        return;
      ForeignInfoColumnInfo fiColumn = model.Meta.GetForeignInfoColumn(columnName);
      var columns = fiColumn.GetAffectedColumns();
      //DataRow row = fiColumn.GetForeignData(fiColumn.Name, columnValue);
      DataRow row = fiColumn.GetForeignData(columnValue);
      bool nullifyData = row == null;
      foreach (var column in columns) {
        string fiAssignedColumn = fiColumn.GetAssignedColumn(column.ColumnName);
        var panelName = Aibe.DH.ForeignInfoPrefix + "-" + column.ColumnName + "-" + fiColumn.Name + 
          (string.IsNullOrWhiteSpace(fiAssignedColumn) ? string.Empty : ("-" + fiAssignedColumn)); //v1.4.1.0 check if the panel name needs to be extended by 
        SingleItemPanel panel = allPanels.FirstOrDefault(x => x.Name.EqualsIgnoreCase(panelName));
        if (panel == null)
          continue;
        if (nullifyData) {
          panel.SetForeignInfoColumnValue(string.Empty);
          continue;
        }
        object value = row[column.ColumnName];
        panel.SetForeignInfoColumnValue(value);
      }
    }

    private void processLiveDropDown(ComboBox comboBox, bool applyListColumnLoad) {
      List<LiveDropDownArg> liveDdArgsList = new List<LiveDropDownArg>();
      foreach (SingleItemPanel panel in dropdownPanels) {
        ComboBox cbItem = panel.GetComboBox();
        ComboBoxTag cbItemTag = (ComboBoxTag)cbItem.Tag;
        LiveDropDownArg args = new LiveDropDownArg {
          OriginalValue = cbItemTag.Model.OriginalChosenItem,
          ColumnName = cbItemTag.Info.Name,
          DataType = cbItemTag.Model.DataType,
          SelectedValue = cbItem.SelectedItem?.ToString(),
        };
        liveDdArgsList.Add(args);
      }

      ComboBoxTag tag = (ComboBoxTag)comboBox.Tag;
      //tag.Info.Name is the changed column name

      List<LiveDropDownResult> results = CommonLogic.GetLiveDropDownItems(model.TableName, tag.Info.Name, liveDdArgsList);
      foreach (var result in results) {
        SingleItemPanel panel = dropdownPanels.FirstOrDefault(x => x.Name.EqualsIgnoreCase(result.ColumnName));
        if (panel == null || panel.GetComboBox() == null)
          continue;
        panel.SetComboBox(result);
      }

      //change ListColumn, if not the first time and is affected by the change
      if (applyListColumnLoad) {
        List<ListColumnResult> lcResults = CommonLogic.GetLiveSubcolumns(model.TableName, tag.Info.Name, comboBox.SelectedItem?.ToString());
        foreach (var lcResult in lcResults) {
          SingleItemPanel panel = listColumnPanels.FirstOrDefault(x => x.Name.EqualsIgnoreCase(lcResult.Name));
          if (panel == null || panel.GetDgv() == null)
            continue;
          panel.SetDgv(lcResult);
        }
      }

      processLiveControl(tag.Info.Name, comboBox.SelectedItem?.ToString());
    }

    private void Item_ComboBoxSelectedIndexChanged(ComboBox comboBox) {
      processLiveDropDown(comboBox, true);
    }

    public AideRequestModel CreateRequestModel() {
      Dictionary<string, string> formCollection = new Dictionary<string, string>();
      Dictionary<string, string> attachments = new Dictionary<string, string>();
      foreach (Control control in flowLayoutPanelContent.Controls) {
        if (!(control is SingleItemPanel))
          continue;
        SingleItemPanel item = (SingleItemPanel)control;
        string key = item.Name; //without key the item is simply undefined, not meant for FormCollection
        if (string.IsNullOrWhiteSpace(key) ||
          item.Model.ItemType == SingleItemPanelType.ScTable || //ScTable is also excluded, because it is always read only
          (item.Model.IsForeignInfo && !item.Model.IsForeignInfoAssigned) || //do not include foreign info for request model (v1.4.1.0 if the foreign info is not assigned)
          item.Model.IsNotAvailable) //Not available items are also not meant for FormCollection
          continue;
        //item.Model.IsTimeStamp || //No need to exclude TimeStamp value, this will be handled later on by the engine
        //item.Model.IsAutoGenerated || //No need to exclude AutoGenerated value, like TimeStamp they will be empty and will be filled later on by the engine
        string value = item.GetValue();
        if (item.Model.IsForeignInfo && item.Model.IsForeignInfoAssigned) { //v1.4.1.0 to accommodate the foreign info assigned
          formCollection.Add(item.Model.ForeignInfoAssignedColumnName, value);
        } else //v1.4.1.0 if not assigned foreign info, just proceed as usual
          formCollection.Add(key, value);
        if (item.Model.ItemType == SingleItemPanelType.Picture ||
          item.Model.ItemType == SingleItemPanelType.NonPictureAttachment) {
          string attachment = item.GetAttachmentSourcePath();
          attachments.Add(key, attachment);
        }
      }
      AideRequestModel model = new AideRequestModel() {
        FormCollection = formCollection,
        Attachments = attachments,
        AttachmentBaseFolderPath = FileHelper.GetAttachmentFolderPath(),
      };
      return model;
    }

    private int getInitialTopWidth() {
      return labelAction.Width + labelTitle.Width + buttonClose.Width + AWF.BaseWindowsWidth;
    }

    private void updateSizeNeeded() {
      TotalWidth = Math.Max(getInitialTopWidth(), TotalWidth);
      TotalHeight = AWF.BaseWindowsHeight;
      foreach (Control control in flowLayoutPanelContent.Controls) {
        if (!control.Visible || !(control is SingleItemPanel))
          continue;
        SingleItemPanel item = (SingleItemPanel)control;
        TotalWidth = UiHelper.AdjustWindowsWidth(item.TotalWidth, TotalWidth);
        TotalHeight += UiHelper.GetHeightOf(item);
      }
    }

    private void panelItem_Resized(SingleItemPanel sender) {
      updateSizeNeeded();
      Size = UiHelper.GetAppliedWindowsSize(Math.Max(TotalWidth, Size.Width), Math.Max(TotalHeight, Size.Height), AWF.CommonActionWindowsMaxSize); //Apply the maximum width and height among whatever currently implemented with the need
    }

    private void buttonClose_Click(object sender, EventArgs e) {
      Close();
    }

    private void buttonPerformAction_Click(object sender, EventArgs e) {
      DialogResult = DialogResult.OK; //do not close... just make the 
    }

    private void CommonCreateEditForm_Load(object sender, EventArgs e) {
      TotalWidth = AWF.BaseWindowsWidth;
      updateSizeNeeded();
      Size = UiHelper.GetAppliedWindowsSize(TotalWidth, TotalHeight, AWF.CommonActionWindowsMaxSize);
    }

    private void CommonCreateEditForm_Shown(object sender, EventArgs e) {
      TotalWidth = AWF.BaseWindowsWidth;
      updateSizeNeeded();
      Size = UiHelper.GetAppliedWindowsSize(TotalWidth, TotalHeight, AWF.CommonActionWindowsMaxSize);
    }
  }
}

//if (isReadOnly) {
//  item = new SingleItemPanel(new SingleItemPanelModel() {
//    Name = cn, DisplayName = model.Meta.GetColumnDisplayName(cn),
//    ItemType = SingleItemPanelType.Boolean,
//    IsReadOnly = true,
//    Arg = dataValue,
//  });
//  //<div class="form-group">
//  //  <label class="control-label @ViewBag.LabelClasses">@Model.Meta.GetColumnDisplayName(column.ColumnName)</label>
//  //  <div class="@ViewBag.EditorClass">          
//  //    <input type="text" name="@column.ColumnName" class="@controlClasses" value="@dataValue" readonly="readonly" />
//  //  </div>
//  //</div>
//} else {
//  item = new SingleItemPanel(new SingleItemPanelModel() {
//    Name = cn, DisplayName = model.Meta.GetColumnDisplayName(cn),
//    ItemType = SingleItemPanelType.Boolean,
//    Arg = dataValue,
//  });
//}          //could probably be in checkbox form
//List<SelectListItem> dropdownItems = new List<SelectListItem> {
//  new SelectListItem { Text = null, Value = null },
//  new SelectListItem { Text = "True", Value = "true" },
//  new SelectListItem { Text = "False", Value = "false" },
//};

//SelectListItem item = dropdownItems.FirstOrDefault(x => x.Text == dataValue);
//if (item != null) {
//  item.Selected = true;
//}

//<div class="form-group">
//  <label class="control-label @ViewBag.LabelClasses">@Model.Meta.GetColumnDisplayName(column.ColumnName)</label>
//  <div class="@ViewBag.EditorClass">          
//    @Html.DropDownList(column.ColumnName, dropdownItems, new { @class = controlClasses })
//    @Html.ValidationMessage(column.ColumnName, new { @class = "text-danger" })
//  </div>
//</div>

//string dataValueDate = dataValueDt.HasValue ? dataValueDt.Value.ToString(Aibe.DH.DefaultDateFormat) : string.Empty;
//string dataTimeName = cn + Aibe.DH.CreateEditTimeAppendixName;
//string dataTimeValue = model.GetTime(dataTimeName, dataValueDt);
//<input type="hidden" name="@column.ColumnName" class="@controlClasses" value="@dataValue" />
//<input type="hidden" name="@dataTimeName" class="@controlClasses" value="@dataTimeValue" />
//<div class="form-group">
//  <label class="control-label @ViewBag.LabelClasses">@Model.Meta.GetColumnDisplayName(column.ColumnName)</label>
//  <div class="@ViewBag.EditorClass">
//    <div class="row form-inline" style="margin-left:0px;">
//      <input type="date" name="@column.ColumnName" class="@controlClasses" value="@dataValue" @readOnlyAttrIfReadOnly/>
//      <input type="time" step="1" name="@dataTimeName" class="@controlClasses" value="@dataTimeValue" @readOnlyAttrIfReadOnly/>
//    </div>
//    @Html.ValidationMessage(column.ColumnName, new { @class = "text-danger" })
//    @Html.ValidationMessage(dataTimeName, new { @class = "text-danger" })
//  </div>
//</div>          

//<input type="hidden" name="@column.ColumnName" class="@controlClasses" value="@dataValue" />
//<input type="text" name="@column.ColumnName" class="@controlClasses" value="@dataValue" readonly="readonly" />
//<input type="number" name="@column.ColumnName" class="@controlClasses" value="@dataValue" @readOnlyAttrIfReadOnly/>
//<div id="live-dd-@column.ColumnName" class="live-dd" commondatatype="number">
//  @{
//    List<SelectListItem> dropdownItems = new List<SelectListItem> {
//      new SelectListItem { Text = null, Value = null },
//    };
//    dropdownItems.AddRange(dropdowns.Select(x => new SelectListItem { Text = x, Value = x }).ToArray());
//    SelectListItem item = dropdownItems.FirstOrDefault(x => x.Text == dataValue);
//    if (item != null) {
//      item.Selected = true;
//    }
//  }
//  @Html.DropDownList(column.ColumnName, dropdownItems, new { @class = controlClasses + " common-column-dropdown",
//    id = "common-column-dropdown-" + column.ColumnName })
//</div>
//<input type="text" class="common-column-dropdown-original" id="common-column-dropdown-original-@column.ColumnName" value="@dataValue" hidden="hidden" />
//<input type="number" name="@column.ColumnName" class="@controlClasses" value="@dataValue" @readOnlyAttrIfReadOnly/>

//normal string
//<input type="text" name="@column.ColumnName" class="@controlClasses" value="@dataValue" @readOnlyAttrIfReadOnly/>

//<img src="" style="width:100px" id="picturedisplay-@column.ColumnName" hidden="hidden"/>
//<button class="common-remove-picture" type="button" id="pictureremove-@column.ColumnName" hidden="hidden">remove</button>

//<img src="../../../Images/@columnValue" style="width:100px" id="picturedisplay-@column.ColumnName" />
//<img src="../../../Images/@Model.TableName/@cid/@columnValue" style="width:100px" id="picturedisplay-@column.ColumnName" />
//<button class="common-remove-picture" type="button" id="pictureremove-@column.ColumnName" @hiddenAttrIfReadOnly>remove</button>

//<textarea class="@controlClasses"
//          rows="@Model.Meta.GetTextFieldRowSize(column.ColumnName)"                                                
//          name="@column.ColumnName" @readOnlyAttrIfReadOnly>@dataValue</textarea> //must be written this way and cannot be auto adjusted. Otherwise the dataValue will be put in the centre of the area (not in the beginning)

//<input type="text" name="@column.ColumnName" class="@controlClasses" value="@dataValue" @readOnlyAttrIfReadOnly/>
//<input type="text" name="@column.ColumnName" class="@controlClasses" value="@dataValue" readonly="readonly" />

//<div id="live-dd-@column.ColumnName" class="live-dd" commondatatype="string">
//@{ 
//List<SelectListItem> dropdownItems = new List<SelectListItem> {
//  new SelectListItem { Text = null, Value = null },
//};
//dropdownItems.AddRange(dropdowns.Select(x => new SelectListItem { Text = x, Value = x }).ToArray());
//SelectListItem item = dropdownItems.FirstOrDefault(x => x.Text == dataValue);
//if (item != null && !string.IsNullOrWhiteSpace(dataValue)) {
//  item.Selected = true;
//}
//  }
//  @Html.DropDownList(column.ColumnName, dropdownItems, new { @class = controlClasses + " common-column-dropdown",
//    id = "common-column-dropdown-" + column.ColumnName })
// </div>
//<input type="text" class="common-column-dropdown-original" id="common-column-dropdown-original-@column.ColumnName" value="@dataValue" hidden="hidden" />

//Excluded columns
//if (cn.EqualsIgnoreCase("Cid"))
//  continue;
//if (dataType.EqualsIgnoreCase(Aibe.DH.DateTimeDataType)) { //not too sure why do we still need this for Aide.Winforms, could probably be removed
//  string dataValueRaw = model.GetData(cn);
//  DateTime? dataValueDt = string.IsNullOrWhiteSpace(dataValueRaw) ? null : new DateTime?(DateTime.Parse(dataValueRaw));
//  SingleItemPanel itemDt = new SingleItemPanel(new SingleItemPanelModel() {
//    Name = cn, DisplayName = model.Meta.GetColumnDisplayName(cn),
//    ItemType = SingleItemPanelType.DateTime,
//    IsHidden = true,
//    Arg = dataValueDt,              
//  });
//  flowLayoutPanelContent.Controls.Add(itemDt);
//  //continue;
//}

//This hidden item is no longer needed as the datetime value will be stored... above...
//string dataValue = model.GetData(cn);
//SingleItemPanel item = new SingleItemPanel(new SingleItemPanelModel() {
//  Name = cn, DisplayName = model.Meta.GetColumnDisplayName(cn),
//  ItemType = SingleItemPanelType.Display,
//  IsHidden = true,
//  Arg = dataValue,
//});
//flowLayoutPanelContent.Controls.Add(item);

//SingleItemPanel valuedHiddenItem = new SingleItemPanel(new SingleItemPanelModel() {
//  Name = cn, DisplayName = model.Meta.GetColumnDisplayName(cn),
//  ItemType = SingleItemPanelType.Display, //no longer needed, but the used value for the list needs to actually be stored...
//  IsHidden = true,
//  Arg = usedDataValue,
//});
//flowLayoutPanelContent.Controls.Add(valuedHiddenItem);

//if (isReadOnly) {
//  item = new SingleItemPanel(new SingleItemPanelModel() {
//    Name = cn + Aibe.DH.ListColumnAppendixName, DisplayName = model.Meta.GetColumnDisplayName(cn),
//    ItemType = SingleItemPanelType.List,
//    Info = info,
//    IsReadOnly = true,
//    Arg = usedDataValue, //usedDataValue, the original one, is already stored in the Arg, but we may not use it anyway...
//  });
//  item.PanelItemResized += panelItem_Resized;
//  //<input type="hidden" name="@column.ColumnName" id="ro-content-@column.ColumnName" value="@usedDataValue" />
//  //<div>
//  //  @Html.Raw(info.GetHTML(usedDataValue, true))
//  //</div>
//} else {
//  item = new SingleItemPanel(new SingleItemPanelModel() {
//    Name = cn + Aibe.DH.ListColumnAppendixName, DisplayName = model.Meta.GetColumnDisplayName(cn),
//    ItemType = SingleItemPanelType.List,
//    Info = info,
//    Arg = usedDataValue,
//  });
//  item.PanelItemResized += panelItem_Resized;
//  //<div id="common-subcolumn-datavalue-@column.ColumnName">
//  //  <input type="hidden" name="@column.ColumnName" id="common-subcolumn-content-@column.ColumnName" value="@usedDataValue" />
//  //</div>                  
//  //<span hidden id="common-subcolumn-span-@column.ColumnName">@lcType</span>
//  //<span hidden id="common-subcolumn-portion-@column.ColumnName">@Model.CreateEditLabelPortion</span>
//  //<div id="common-subcolumn-div-@column.ColumnName">
//  //  @Html.Raw(info.GetHTML(usedDataValue))
//  //</div>
//}
