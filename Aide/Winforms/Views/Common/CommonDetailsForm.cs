using Aibe.Helpers;
using Aibe.Models;
using Aibe.Models.Core;
using Aide.Models;
using Aide.Winforms.Components;
using Aide.Winforms.Helpers;
using Aide.Winforms.Models;
using AWF = Aide.Winforms.SH;
using Extension.String;
using System;
using System.Windows.Forms;
using System.Data;

namespace Aide.Winforms {
  public partial class CommonDetailsForm : Form {
    AideDetailsModel model { get; set; }
    public int TotalWidth { get; set; } = AWF.BaseWindowsWidth; //base width
    public int TotalHeight { get; set; } = AWF.BaseWindowsHeight; //base height
    private int performActionButtonHeight { get; set; }

    public CommonDetailsForm(AideDetailsModel model) {
      InitializeComponent();
      this.model = model;
      localization();
      applyModel(model);
    }

    private void localization() {
      buttonClose.Text = Aibe.LCZ.W_Close;
      buttonPerformAction.Text = Aibe.LCZ.GetLocalizedDefaultActionName(model.ActionType);
      labelAction.Text = string.Concat("(", buttonPerformAction.Text, ")");
    }

    private void applyModel(AideDetailsModel model) {
      this.model = model;
      flowLayoutPanelContent.Controls.Clear();

      Text = Aibe.LCZ.GetLocalizedDefaultActionName(model.ActionType) + " - " + model.TableDisplayName;
      labelTitle.Text = model.TableDisplayName;
      TotalWidth = Math.Max(getInitialTopWidth(), TotalWidth);

      bool isDetails = model.ActionType.EqualsIgnoreCase(Aibe.DH.DetailsActionName);
      if (isDetails) {
        performActionButtonHeight = UiHelper.GetHeightOf(buttonPerformAction);
        splitContainerContent.Panel2Collapsed = true;
      }

      foreach (var data in model.SequencedItems) {
        string cn = data.Key;
        if (!model.IsColumnIncludedInDetails(cn))
          continue;
        SingleItemPanel item;
        if (model.Meta.IsScriptColumn(cn)) {
          ScTableInfo scTable = model.GetScTable(cn);
          bool isNA = scTable == null || !scTable.IsValid;
          item = new SingleItemPanel(new SingleItemPanelModel() {
            Name = cn, DisplayName = model.Meta.GetColumnDisplayName(cn),
            IsReadOnly = true,
            IsNotAvailable = isNA, //to distinguish true display (read only item, such as to be used in Details) from false display (nothing to be obtained) //not sure if it is good, but leave it for now...
            ItemType = isNA ? SingleItemPanelType.Display : SingleItemPanelType.ScTable,
            ActionType = model.ActionType, //need for ScTable and List
            Arg = isNA ? "[" + Aibe.LCZ.W_NA + "]" : null, //no argument needed for real ScTable
            Info = scTable,
          });
        } else if (model.Meta.IsPictureColumn(cn)) {
          PictureColumnInfo pcInfo = model.Meta.GetPictureColumnInfo(cn);
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
              IsReadOnly = true,
              Info = model.Meta.GetPictureColumnInfo(cn),
              Arg = fullRelativePath,
              TableName = model.Meta.TableName,
              Cid = int.Parse(cid),
            });
          } else { //no image to load
            item = new SingleItemPanel(new SingleItemPanelModel {
              Name = cn,
              DisplayName = model.Meta.GetColumnDisplayName(cn),
              ItemType = SingleItemPanelType.Picture,
              IsReadOnly = true,
              Info = model.Meta.GetPictureColumnInfo(cn),
            });
          }
        } else if (model.Meta.IsNonPictureAttachmentColumn(cn)) {
          AttachmentInfo attInfo = model.Meta.GetNonPictureAttachmentColumn(cn);
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
              IsReadOnly = true,
              Info = model.Meta.GetNonPictureAttachmentColumn(cn),
              Arg = fullRelativePath,
              TableName = model.Meta.TableName,
              Cid = int.Parse(cid),
            });
          } else { //no image to load
            item = new SingleItemPanel(new SingleItemPanelModel {
              Name = cn,
              DisplayName = model.Meta.GetColumnDisplayName(cn),
              ItemType = SingleItemPanelType.NonPictureAttachment,
              IsReadOnly = true,
              Info = model.Meta.GetNonPictureAttachmentColumn(cn),
            });
          }
        } else if (model.Meta.IsListColumn(cn)) {
          string dataValue = data.Value ?? string.Empty;
          string lcType = model.Meta.GetListColumnType(cn);
          bool hasStaticTemplate = string.IsNullOrWhiteSpace(dataValue) &&
            model.Meta.ListColumnHasStaticTemplate(cn);
          string usedDataValue = hasStaticTemplate ?
            model.Meta.GetColumnStaticTemplate(cn) : dataValue;
          ListColumnInfo info = model.Meta.GetListColumnInfo(cn);
          item = new SingleItemPanel(new SingleItemPanelModel() {
            Name = cn, DisplayName = model.Meta.GetColumnDisplayName(cn),
            ItemType = SingleItemPanelType.List,
            ActionType = model.ActionType, //need for ScTable and List
            Info = info,
            IsReadOnly = true,
            Arg = usedDataValue, //usedDataValue, the original one, is already stored in the Arg, but we may not use it anyway...
          });
        } else {
          string dateTimeFormat = model.Meta.GetCustomDateTimeFormatFor(cn, Aibe.DH.DetailsPageName);
          string dataValue = DateTimeHelper.ProcessPossibleDateTimeString(model.GetData(cn),
            model.Meta.IsDateTimeColumn(cn), dateTimeFormat ?? Aide.PH.DetailsDateTimeFormat);
          string displayName = model.Meta.GetColumnDisplayName(cn);
          item = new SingleItemPanel(SingleItemPanelModel.CreateReadOnlyCommonModel(
            cn, dataValue, displayName, isForeignInfo: false, isForeignInfoAssigned: false));
        }

        FormHelper.ProcessItemWithItsForeignInfos(cn, item, model, flowLayoutPanelContent, Aibe.DH.DetailsPageName, Aide.PH.DetailsDateTimeFormat);
      }
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
      if (model.ActionType.EqualsIgnoreCase(Aibe.DH.DetailsActionName))
        TotalHeight -= performActionButtonHeight;
    }

    private void buttonClose_Click(object sender, EventArgs e) {
      Close();
    }

    private void buttonPerformAction_Click(object sender, EventArgs e) {
      DialogResult = DialogResult.OK; //do not close... just make the 
    }

    private void CommonDetailsForm_Load(object sender, EventArgs e) {
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
