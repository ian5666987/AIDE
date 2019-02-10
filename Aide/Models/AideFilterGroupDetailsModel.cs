using Aibe.Models;
using System.Collections.Generic;

namespace Aide.Models {
  //To be used to display filter and index
  public class AideFilterGroupDetailsModel : AideBaseFilterIndexModel {
    //so that the actual FilterGroupDetailsModel can be recognized, rather than BaseFilterIndexModel
    public FilterGroupDetailsModel GdModel { get { return (FilterGroupDetailsModel)BaseModel; } } 
    public bool IsGroupDeletion { get; private set; }
    public AideFilterGroupDetailsModel (MetaInfo meta, FilterGroupDetailsModel model, 
      bool isGroupDeletion, Dictionary<string, string> stringDictionary) : base(meta, model, stringDictionary) {
      IsGroupDeletion = isGroupDeletion;
    }
  }
}
