using Aibe.Models;
using Aibe.Models.Core;
using Aide.Logics;
using Aide.Models.Accounts;
using Extension.String;
using System.Collections.Generic;
using System.Linq;

namespace Aide.Models {
  public class AideBaseTableModel : BaseTableModel {
    public Dictionary<string, string> ErrorDict { get; set; }
    public string ErrorMessage { get; set; }
    public bool IsSuccessful { get; set; }
    public AideBaseTableModel(MetaInfo meta, Dictionary<string, string> stringDictionary) : base(meta, stringDictionary) { }

    protected bool IsColumnIncluded(List<ExclusionInfo> exclusions, string columnName) {
      if (exclusions == null || exclusions.Count <= 0) //if there is no column exclusion, then the column is definitely included
        return true;
      ExclusionInfo exInfo = exclusions.FirstOrDefault(x => x.Name.EqualsIgnoreCaseTrim(columnName));
      if (exInfo == null) //non specified column exclusion is allowed
        return true;
      bool isExplicitlyExcluded = exInfo.Roles == null || !exInfo.Roles.Any() || exInfo.Roles.Any(x => UserLogic.IsInRole(Identity.User, x, Identity.Roles));
      return !isExplicitlyExcluded; //if user is not explicitly excluded, then he is definitely included.
    }

    public virtual bool IsColumnIncludedInIndex(string columnName) {
      return IsColumnIncluded(Meta.ColumnExclusions, columnName);
    }

    public virtual bool IsColumnIncludedInCsv(string columnName) {
      return IsColumnIncluded(Meta.CsvExclusions, columnName);
    }

    public virtual List<string> GetExcludedColumnsInCsv(List<string> columnNames) {
      return columnNames
        .Where(x => !IsColumnIncludedInCsv(x))
        .ToList();
    }
  }
}