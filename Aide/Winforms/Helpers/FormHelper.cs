using Aibe.Helpers;
using Aibe.Models.Core;
using Aide.Logics;
using Aide.Models;
using Aide.Winforms.Components;
using Aide.Winforms.Models;
using Extension.Models;
using Extension.String;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Aide.Winforms.Helpers {
  public class FormHelper {
    public static void ProcessCommonIndex(string tableName) {
      BaseErrorModel errorModel = CommonLogic.Index(tableName, 1, null);
      if (errorModel.HasError) {
        MessageBox.Show(errorModel.ToShortString(Aibe.LCZ.W_ErrorModelCodeWord, Aibe.LCZ.W_ErrorModelMessageWord), 
          Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      AideFilterIndexModel model = (AideFilterIndexModel)errorModel.ReturnObject;
      CommonIndexForm form = new CommonIndexForm(model);
      form.Show();
    }

    public static CommonIndexForm GetNewCommonIndexForm(string tableName) {
      BaseErrorModel errorModel = CommonLogic.Index(tableName, 1, null);
      if (errorModel.HasError) {
        MessageBox.Show(errorModel.ToShortString(Aibe.LCZ.W_ErrorModelCodeWord, Aibe.LCZ.W_ErrorModelMessageWord), 
          Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return null;
      }
      AideFilterIndexModel model = (AideFilterIndexModel)errorModel.ReturnObject;
      return new CommonIndexForm(model);
    }

    public static AideFilterIndexModel GetUpdatedCommonIndexModel(string tableName, int page, Dictionary<string, string> filters) {
      BaseErrorModel nonErroneousModel = CommonLogic.Index(tableName, page, filters);
      return (AideFilterIndexModel)nonErroneousModel.ReturnObject;
    }

    public static void ProcessCommonGroupDetails(string tableName, List<KeyValuePair<string, object>> identifiers) {
      BaseErrorModel errorModel = CommonLogic.GroupDetails(tableName, identifiers, 1, null, false);
      if (errorModel.HasError) {
        MessageBox.Show(errorModel.ToShortString(Aibe.LCZ.W_ErrorModelCodeWord, Aibe.LCZ.W_ErrorModelMessageWord),
          Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      AideFilterGroupDetailsModel model = (AideFilterGroupDetailsModel)errorModel.ReturnObject;
      CommonGroupDetailsForm form = new CommonGroupDetailsForm(model);
      form.Show();
    }

    public static CommonGroupDetailsForm GetNewCommonGroupDetailsForm(string tableName, List<KeyValuePair<string, object>> identifiers) {
      BaseErrorModel errorModel = CommonLogic.GroupDetails(tableName, identifiers, 1, null, false);
      if (errorModel.HasError) {
        MessageBox.Show(errorModel.ToShortString(Aibe.LCZ.W_ErrorModelCodeWord, Aibe.LCZ.W_ErrorModelMessageWord),
          Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return null;
      }
      AideFilterGroupDetailsModel model = (AideFilterGroupDetailsModel)errorModel.ReturnObject;
      return new CommonGroupDetailsForm(model);
    }

    public static AideFilterGroupDetailsModel GetUpdatedCommonGroupDetailsModel(string tableName,
      List<KeyValuePair<string, object>> identifiers, int page, Dictionary<string, string> filters, bool isGroupDeletion) {
      BaseErrorModel nonErroneousModel = CommonLogic.GroupDetails(tableName, identifiers, page, filters, isGroupDeletion);
      return (AideFilterGroupDetailsModel)nonErroneousModel.ReturnObject;
    }

    private static List<SingleItemPanelType> foreignProcessableItemTypes = new List<SingleItemPanelType> {
      SingleItemPanelType.Text, SingleItemPanelType.TextField,
      SingleItemPanelType.Number, SingleItemPanelType.DropDown,
    };
    public static void ProcessItemWithItsForeignInfos(string columnName, SingleItemPanel item, AideBaseTableModel model, FlowLayoutPanel panel,
      string pageName = Aide.DH.DefaultPageName, string defaultDateTimeFormat = Aide.DH.DefaultDateTimeFormat) {
      panel.Controls.Add(item);
      if (!foreignProcessableItemTypes.Any(x => x == item.Model.ItemType))
        return; //if item type can be processed, then process it. Otherwise, skips it.
      if (model.Meta.IsForeignInfoColumn(columnName)) { //additional items put in the flow layout here, but all is just read only
        string dateTimeFormat = model.Meta.GetCustomDateTimeFormatFor(columnName, pageName);
        string dataValue = DateTimeHelper.ProcessPossibleDateTimeString(model.GetData(columnName),
          model.Meta.IsDateTimeColumn(columnName), dateTimeFormat ?? defaultDateTimeFormat);
        ForeignInfoColumnInfo info = model.Meta.GetForeignInfoColumn(columnName);
        DataRow row = info.GetForeignData(dataValue);
        if (info.IsFullForeignInfo) { //get everything in the row
          var columns = info.GetForeignFullColumns();
          foreach (DataColumn column in columns) {
            object rowValue = row == null ? null : row[column];
            //Prefix-Foreign table column name-Key column name
            string usedColumnName = Aibe.DH.ForeignInfoPrefix + "-" + column.ColumnName + "-" + columnName;
            item = new SingleItemPanel(SingleItemPanelModel.CreateReadOnlyCommonModel(
              usedColumnName, rowValue == null ? string.Empty : rowValue.ToString(),
              column.ColumnName.ToCamelBrokenString(), isForeignInfo: true, isForeignInfoAssigned: false)); //v1.4.1.0 foreign info always not assigned for full foreign info
            panel.Controls.Add(item);
          }
        } else {
          foreach (var columnNameTrio in info.RefColumnNameTrios) {
            object rowValue = row == null ? null : row[columnNameTrio.Item1];
            bool isAssigned = !string.IsNullOrWhiteSpace(columnNameTrio.Item3); //v1.4.1.0 only assign the foreign info if there is item3
            string usedColumnName = Aibe.DH.ForeignInfoPrefix + "-" + columnNameTrio.Item1 + "-" + columnName + 
              (isAssigned ? ("-" + columnNameTrio.Item3) : string.Empty); //v1.4.1.0 if it is assigned, extend the used column name
            item = new SingleItemPanel(SingleItemPanelModel.CreateReadOnlyCommonModel(
              usedColumnName, rowValue == null ? string.Empty : rowValue.ToString(),
              columnNameTrio.Item2, isForeignInfo: true, isForeignInfoAssigned: isAssigned)); 
            panel.Controls.Add(item);
          }
        }
      }
    }
  }
}
