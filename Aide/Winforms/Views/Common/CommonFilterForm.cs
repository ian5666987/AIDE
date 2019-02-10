using Aide.Models.Controls;
using Aide.Models;
using Aide.Winforms.Components;
using Aide.Winforms.Helpers;
using Aide.Winforms.Models;
using AWF = Aide.Winforms.SH;
using Extension.String;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Aide.Winforms {
  public partial class CommonFilterForm : Form {
    AideBaseFilterIndexModel model { get; set; }
    public int TotalWidth { get; set; } = AWF.BaseWindowsWidth; //base width
    public int TotalHeight { get; set; } = AWF.BaseWindowsHeight; //base height
    private List<SingleItemPanel> dropdownPanels = new List<SingleItemPanel>();
    private List<SingleItemPanel> listColumnPanels = new List<SingleItemPanel>();

    public CommonFilterForm(AideBaseFilterIndexModel model) {
      InitializeComponent();
      this.model = model;
      localization();
      applyModel(model);
    }

    private void localization() {
      buttonClose.Text = Aibe.LCZ.W_Close;
      buttonPerformAction.Text = Aibe.LCZ.W_Apply;
      labelAction.Text = string.Concat("(", Aibe.LCZ.W_Filter, ")");
    }

    private void applyModel(AideBaseFilterIndexModel model) {
      this.model = model;
      flowLayoutPanelContent.Controls.Clear();

      Text = Aibe.LCZ.W_Filter + " - " + model.TableDisplayName;
      labelTitle.Text = model.TableDisplayName;
      TotalWidth = Math.Max(getInitialTopWidth(), TotalWidth);

      foreach(var column in model.FilterColumns) {
        string cn = column.ColumnName;        
        string dataType = column.DataType.ToString().Substring(Aibe.DH.SharedPrefixDataType.Length);
        if (dataType.EqualsIgnoreCase(Aibe.DH.StringDataType)) {
          SingleItemPanel item;
          string dataValue = model.GetDataFromFilterDictionary(cn);
          if (!model.Meta.IsFilterDropDownColumn(cn)) {
            item = new SingleItemPanel(new SingleItemPanelModel {
              Name = cn, DisplayName = model.Meta.GetColumnDisplayName(cn),
              ItemType = SingleItemPanelType.Text,
              IsFilter = true, //must be set whenever model uses filter (to get the right get value)
              Arg = dataValue,
            });
          } else {
            List<string> dropdowns = model.Meta.GetStaticFilterDropDownFor(cn, Aibe.DH.DropDownTextDataType);
            if (dropdowns == null || dropdowns.Count <= 0) {
              item = new SingleItemPanel(new SingleItemPanelModel {
                Name = cn, DisplayName = model.Meta.GetColumnDisplayName(cn),
                ItemType = SingleItemPanelType.Text,
                IsFilter = true, //must be set whenever model uses filter (to get the right get value)
                Arg = dataValue,
              });
            } else {
              item = new SingleItemPanel(new SingleItemPanelModel {
                Name = cn, DisplayName = model.Meta.GetColumnDisplayName(cn),
                ItemType = SingleItemPanelType.DropDown, //Cannot be read-only, does not work
                Info = model.Meta.GetFilterDropDownColumnInfo(cn),
                IsFilter = true, //must be set whenever model uses filter (to get the right get value)
                Arg = new ComboBoxModel() {
                  Options = dropdowns,
                  OriginalChosenItem = dataValue, DataType = Aibe.DH.DropDownTextDataType
                }
              });
            }
          }
          flowLayoutPanelContent.Controls.Add(item);
        } else if (Aibe.DH.NumberDataTypes.Contains(dataType)) {
          string dataNameFrom = string.Concat(cn, Aibe.DH.BaseFilterAppendixName, dataType, Aibe.DH.FromName);
          string dataValueFrom = model.GetDataFromFilterDictionary(dataNameFrom);
          string dataNameTo = string.Concat(cn, Aibe.DH.BaseFilterAppendixName, dataType, Aibe.DH.ToName);
          string dataValueTo = model.GetDataFromFilterDictionary(dataNameTo);
          SingleItemPanel itemFrom = new SingleItemPanel(new SingleItemPanelModel() {
            Name = dataNameFrom, DisplayName = model.Meta.GetColumnDisplayName(cn) + " (" + Aibe.LCZ.W_From + ")",
            ItemType = SingleItemPanelType.Number,
            Info = model.Meta.GetNumberLimitColumn(cn),
            IsFilter = true, //must be set whenever model uses filter (to get the right get value)
            Arg = dataValueFrom,
          });
          SingleItemPanel itemTo = new SingleItemPanel(new SingleItemPanelModel() {
            Name = dataNameTo, DisplayName = model.Meta.GetColumnDisplayName(cn) + " (" + Aibe.LCZ.W_To + ")",
            ItemType = SingleItemPanelType.Number,
            Info = model.Meta.GetNumberLimitColumn(cn),
            IsFilter = true, //must be set whenever model uses filter (to get the right get value)
            Arg = dataValueTo,
          });
          flowLayoutPanelContent.Controls.Add(itemFrom);
          flowLayoutPanelContent.Controls.Add(itemTo);
        } else if (dataType.EqualsIgnoreCase(Aibe.DH.DateTimeDataType)) {
          string dataNameFrom = cn + Aibe.DH.FilterDateAppendixFrontName + Aibe.DH.FromName;
          string dataValueRawFrom = model.GetDataFromFilterDictionary(dataNameFrom);
          DateTime? dataValueDtFrom = string.IsNullOrWhiteSpace(dataValueRawFrom) ? null : new DateTime?(DateTime.Parse(dataValueRawFrom));

          string dataNameTo = cn + Aibe.DH.FilterDateAppendixFrontName + Aibe.DH.ToName;
          string dataValueRawTo = model.GetDataFromFilterDictionary(dataNameTo);
          DateTime? dataValueDtTo = string.IsNullOrWhiteSpace(dataValueRawTo) ? null : new DateTime?(DateTime.Parse(dataValueRawTo));

          string dateTimeFormat = model.Meta.HasCustomDateTimeFormatFor(cn, Aibe.DH.CreateEditFilterPageName) ?
            model.Meta.GetCustomDateTimeFormatFor(cn, Aibe.DH.CreateEditFilterPageName) :
            Aide.PH.CreateEditFilterDateTimeFormat;

          SingleItemPanel itemFrom = new SingleItemPanel(new SingleItemPanelModel() {
            Name = dataNameFrom, DisplayName = model.Meta.GetColumnDisplayName(cn) + " (" + Aibe.LCZ.W_From + ")",
            ItemType = SingleItemPanelType.DateTime, //if it is read only, it cannot be datetime
            IsFilter = true, //must be set whenever model uses filter (to get the right get value)
            Arg = dataValueDtFrom,
            DateTimeFormat = dateTimeFormat,
          });

          SingleItemPanel itemTo = new SingleItemPanel(new SingleItemPanelModel() {
            Name = dataNameTo, DisplayName = model.Meta.GetColumnDisplayName(cn) + " (" + Aibe.LCZ.W_To + ")",
            ItemType = SingleItemPanelType.DateTime, //if it is read only, it cannot be datetime
            IsFilter = true, //must be set whenever model uses filter (to get the right get value)
            Arg = dataValueDtTo,
            DateTimeFormat = dateTimeFormat,
          });

          flowLayoutPanelContent.Controls.Add(itemFrom);
          flowLayoutPanelContent.Controls.Add(itemTo);
        } else if (dataType.EqualsIgnoreCase(Aibe.DH.BooleanDataType)) {
          string dataName = string.Concat(cn, Aibe.DH.BaseFilterAppendixName, dataType);
          string dataValue = model.GetDataFromFilterDictionary(dataName);
          SingleItemPanel item = new SingleItemPanel(new SingleItemPanelModel() {
            Name = cn, DisplayName = model.Meta.GetColumnDisplayName(cn),
            ItemType = SingleItemPanelType.Boolean, //must remain boolean, though it is read only or not...
            IsFilter = true, //must be set whenever model uses filter (to get the right get value)
            Arg = Aibe.LCZ.GetLocalizedBooleanOption(dataValue),
          });
          flowLayoutPanelContent.Controls.Add(item);
        } //Other than these are all unknown
      }
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

    private void buttonClose_Click(object sender, EventArgs e) {
      Close();
    }

    private void buttonPerformAction_Click(object sender, EventArgs e) {
      DialogResult = DialogResult.OK; //do not close... just make the 
    }

    private void CommonFilterForm_Load(object sender, EventArgs e) {
      TotalWidth = AWF.BaseWindowsWidth;
      updateSizeNeeded();
      Size = UiHelper.GetAppliedWindowsSize(TotalWidth, TotalHeight, AWF.CommonActionWindowsMaxSize);
    }
  }
}

//string dataValueFrom = dataValueDtFrom.HasValue ? dataValueDtFrom.Value.ToString(Aibe.DH.DefaultDateFormat) : string.Empty;
//string dataTimeNameFrom = cn + Aibe.DH.FilterTimeAppendixFrontName + Aibe.DH.FromName;
//string dataTimeValueFrom = dataValueDtFrom.HasValue ?
//dataValueDtFrom.Value.ToString(Aibe.DH.DefaultTimeFormatWithoutSecond) : null;
//string dataValueTo = dataValueDtTo.HasValue ? dataValueDtTo.Value.ToString(Aibe.DH.DefaultDateFormat) : string.Empty;
//string dataTimeNameTo = cn + Aibe.DH.FilterTimeAppendixFrontName + Aibe.DH.ToName;
//string dataTimeValueTo = dataValueDtTo.HasValue ?
//dataValueDtTo.Value.ToString(Aibe.DH.DefaultTimeFormatWithoutSecond) : null;
