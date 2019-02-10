using System.Collections.Generic;

namespace Aide.Models.Controls {
  public class ComboBoxModel {
    public List<string> Options { get; set; }
    public string OriginalChosenItem { get; set; } //actually, original data value
    public string DataType { get; set; }
  }
}
