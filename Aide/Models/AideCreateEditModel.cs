using Aibe.Models;
using Aibe.Models.Core;
using Extension.String;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Aide.Models {
  public class AideCreateEditModel : AideBaseTableModel {
    public string ActionType { get; set; } = Aibe.DH.CreateActionName; //create OR edit, default is create
    public List<DropDownInfo> DropDowns { get { return Meta.CreateEditDropDowns; } }
    public bool HasAttachment { get; set; }
    public bool SaveAttachmentResult { get; set; }
    public bool IsHistoryTriggered { get; set; }
    public List<int> HistoryTriggerResults { get; set; }
    public List<int> EmailTriggerResults { get; set; }
    public List<object> PreActionTriggerResults { get; set; }
    public List<object> PostActionTriggerResults { get; set; }
    public List<KeyValuePair<string, object>> Identifiers { get; set; }

    public AideCreateEditModel(MetaInfo metaInput, string actionType, Dictionary<string, string> stringDictionary, 
      List<KeyValuePair<string, object>> identifiers) : base(metaInput, stringDictionary) {
      ActionType = actionType;
      GroupDetailsOrigin = identifiers != null && identifiers.Count > 0;
      Identifiers = identifiers;
      //Label portion not needed, unlike Aiwe
    }

    public virtual bool IsColumnIdentifier(string columnName) {
      return Identifiers != null && Identifiers.Any(x => x.Key.EqualsIgnoreCase(columnName));
    }

    public virtual object GetIdentifierValueFor(string columnName) {
      return Identifiers.First(x => x.Key.EqualsIgnoreCase(columnName)).Value;
    }

    public virtual bool IsColumnIncludedInCreateEdit(DataColumn column) {
      if (column.Unique || column.ColumnName.EqualsIgnoreCase(Aibe.DH.Cid)) //Cid and unique column always not included in the create and edit
        return false;
      return IsColumnIncluded(Meta.CreateEditExclusions, column.ColumnName) && !Meta.IsPrefilledColumn(column.ColumnName);
    }
  }
}