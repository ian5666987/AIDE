using Aibe.Models;
using Aide.Models.Accounts;
using System.Collections.Generic;

namespace Aide.Models {
  public class AideDetailsModel : AideBaseTableModel {
    public int Cid { get; private set; }
    public string ActionType { get; private set; } //to distinguish edit from delete....
    public List<int> EmailTriggerResults { get; set; }
    public List<int> HistoryTriggerResults { get; set; }
    public List<object> PreActionTriggerResults { get; set; }
    public List<object> PostActionTriggerResults { get; set; }

    public AideDetailsModel(MetaInfo metaInput, int id, Dictionary<string, string> stringDictionary, string actionType) : 
      base(metaInput, stringDictionary) {
      Cid = id;
      ActionType = actionType;
    }

    public bool IsColumnIncludedInDetails(string columnName) {
      if (Identity.IsMainAdmin()) //if user is in main admin rights, it is always true
        return true; 
      return IsColumnIncluded(Meta.DetailsExclusions, columnName) && !Meta.IsPrefilledColumn(columnName);
    }
  }
}