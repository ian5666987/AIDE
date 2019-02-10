using Aibe.Models;
using System.Collections.Generic;
using System.Data;

namespace Aide.Models {
  //To be used to display filter and index
  public class AideFilterIndexModel : AideBaseFilterIndexModel {
    //so that the actual FilterIndexModel can be recognized, rather than BaseFilterIndexModel
    public FilterIndexModel FiModel { get { return (FilterIndexModel)BaseModel; } }
    public List<DataRow> IdentifierIndexRows { get; private set; } = new List<DataRow>(); //v1.4.1.0 used to make rows for identifiers purpose

    public AideFilterIndexModel (MetaInfo meta, FilterIndexModel model, 
      Dictionary<string, string> stringDictionary) : base(meta, model, stringDictionary) {

      //v1.4.1.0 to handle rows for identifiers
      IdentifierIndexRows.Clear();
      foreach (DataRow row in model.ForeignIdentifiersData.Rows)
        IdentifierIndexRows.Add(row);      
    }
  }
}
