using System.Collections.Generic;

namespace Aide.Winforms.Models {
  public class ButtonTag {
    public bool UseDefaultAction { get; set; }
    public string ActionName { get; set; }
    public int Cid { get; set; }
    public List<KeyValuePair<string, object>> Identifiers { get; set; } = new List<KeyValuePair<string, object>>();
  }
}
