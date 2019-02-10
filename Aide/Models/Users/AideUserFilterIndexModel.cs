using Aibe.Models;
using Aide.Models.Filters;

namespace Aide.Models {
  public class AideUserFilterIndexModel {
    public ApplicationUserFilter Filter { get; set; } = new ApplicationUserFilter();
    public string FilterText; //cannot be a property because it is set as "out"
    public int FilterNo { get; set; }
    public NavDataModel NavData { get; set; } //for filtering and paging correctly
  }
}
