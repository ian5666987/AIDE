using Aibe.Models;
using Aibe.Models.Core;
using Aide.Logics;
using Aide.Models.Accounts;
using Extension.String;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Aide.Models {
  //To be used to display filter and index
  public class AideBaseFilterIndexModel : AideBaseTableModel {
    public DataTable Table { get; private set; }
    public bool HasTable { get { return Table != null; } }
    public BaseFilterIndexModel BaseModel { get; private set; }

    //Table related add info (combined usage)
    public List<ColumnInfo> ColumnInfos { get; private set; } = new List<ColumnInfo>();
    //Index usage
    public int RowNo { get; private set; }
    public List<DataRow> IndexRows { get; private set; } = new List<DataRow>();

    //Taken directly from Meta
    public List<DropDownInfo> DropDowns { get { return Meta.FilterDropDowns; } }

    //Filter usage, just for nice look (adjustable label portion)!
    public List<DataColumn> FilterColumns { get; private set; } = new List<DataColumn>();
    //public int FilterLabelPortion { get; private set; } //unused in the desktop app

    //Displays
    public NavDataModel NavData { get; set; }

    public AideBaseFilterIndexModel(MetaInfo meta, BaseFilterIndexModel model, Dictionary<string, string> stringDictionary) : 
      base(meta, stringDictionary){
      BaseModel = model;
      Table = model.Data;
      NavData = model.NavData;

      if (Table == null || meta == null)
        return;

      //Handle columns
      List<DataColumn> columns = new List<DataColumn>();
      foreach (DataColumn column in Table.Columns) //the columns are taken from the table, thus group by items have different column names as the group by define
        columns.Add(column);
      var arrangedDataColumns = meta.GetColumnSequenceFor(columns);
      ColumnInfos = arrangedDataColumns.Select(x => meta.CreateColumnInfo(
          x, IsColumnIncludedInIndex(x.ColumnName), IsColumnIncludedInFilter(x.ColumnName),
          IsColumnForcelyIncludedInFilter(x.ColumnName)
        )).ToList();
      FilterColumns = ColumnInfos.Where(x => x.IsFilterIncluded)
        .Select(x => x.Column).ToList();

      //Handle rows
      RowNo = Table.Rows.Count;
      IndexRows.Clear();
      foreach (DataRow row in Table.Rows)
        IndexRows.Add(row);
    }

    public bool IsColumnIncludedInFilter(string columnName, bool isWebApi = false) {
      if (Identity.IsMainAdmin())
        return true;
      return IsColumnIncluded(Meta.FilterExclusions, columnName);
    }

    public bool IsColumnForcelyIncludedInFilter(string columnName) {
      InclusionInfo inInfo = Meta.GetForcedFilterColumn(columnName);
      if (inInfo == null) //not specified, means it is not forced
        return false;
      return inInfo.Roles == null || !inInfo.Roles.Any() || inInfo.Roles.Any(x => UserLogic.IsInRole(Identity.User, x, Identity.Roles));
    }

    public bool IsActionAllowed(string actionName, bool isWebApi = false) {
      if (Identity.IsMainAdmin()) //if user is in main admin rights, it is always true
        return true;
      ActionInfo acInfo = Meta.Actions.FirstOrDefault(x => x.Name.EqualsIgnoreCaseTrim(actionName));
      if (acInfo == null) //such action is not found, then it is definitely false
        return false;
      bool isExplicitlyAllowed = acInfo.Roles.Any(x => UserLogic.IsInRole(Identity.User, x, Identity.Roles));
      return acInfo.Roles == null || acInfo.Roles.Count <= 0 || isExplicitlyAllowed; //if it is explicitly allowed or there isn't role specified, then it is true
    }

    public bool IsTableActionAllowed(string tableActionName, bool isWebApi = false) {
      if (Identity.IsMainAdmin()) //if user is in main admin rights, it is always true
        return true;
      ActionInfo acInfo = Meta.TableActions.FirstOrDefault(x => x.Name.EqualsIgnoreCaseTrim(tableActionName));
      if (acInfo == null) //such action is not found, then it is definitely false
        return false;
      bool isExplicitlyAllowed = acInfo.Roles.Any(x => UserLogic.IsInRole(Identity.User, x, Identity.Roles));
      return acInfo.Roles == null || acInfo.Roles.Count <= 0 || isExplicitlyAllowed; //if it is explicitly allowed or there isn't role specified, then it is true
    }

    public bool IsAllowedToCallAction(string actionName) {
      return IsActionAllowed(actionName);
    }

    public bool IsAllowedToCallTableAction(string tableActionName) {
      return IsTableActionAllowed(tableActionName);
    }

    //--specific for filter
    public string GetDataFromFilterDictionary(string filterName) {
      if (BaseModel == null || BaseModel.StringDictionary == null || BaseModel.StringDictionary.Count <= 0 ||
        !BaseModel.StringDictionary.ContainsKey(filterName))
        return null;
      return BaseModel.StringDictionary[filterName];
    }
  }
}