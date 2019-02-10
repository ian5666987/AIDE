using Aibe.Models;
using System.Collections.Generic;

namespace Aide.Models {
  public class AideCreateEditGroupModel : AideBaseTableModel {
    public string ActionType { get; set; } = Aibe.DH.CreateGroupActionName;
    public List<string> IdentifierColumns { get; private set; }
    public List<KeyValuePair<string, object>> IdentifierResults { get; private set; } = new List<KeyValuePair<string, object>>();

    public AideCreateEditGroupModel(MetaInfo metaInput, string actionType, Dictionary<string, string> stringDictionary,
      List<string> identifierColumns) : base(metaInput, stringDictionary) {
      ActionType = actionType;
      IdentifierColumns = identifierColumns;
    }
  }
}