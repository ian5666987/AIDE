using System.Collections.Generic;
using System.Text;

namespace Aide.Models {
  public class AideRequestModel {
    public Dictionary<string, string> FormCollection { get; set; } = new Dictionary<string, string>();
    public Dictionary<string, string> Attachments { get; set; } = new Dictionary<string, string>();
    public string AttachmentBaseFolderPath { get; set; }
    public int Cid { get; set; } = -1; //by default it is not available
  }
}