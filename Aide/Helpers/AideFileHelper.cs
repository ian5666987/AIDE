using System.Collections.Generic;
using System.IO;

namespace Aide.Helpers {
  public class AideFileHelper {
    public static bool SaveAttachments(Dictionary<string, string> attachments, string folderPath) {
      if (attachments == null || attachments.Count <= 0)
        return false;
      try {
        foreach (var attachment in attachments) {
          if (string.IsNullOrWhiteSpace(attachment.Value))
            continue;
          string fileName = Path.GetFileName(attachment.Value);
          var path = Path.Combine(folderPath, fileName);
          Directory.CreateDirectory(folderPath);
          if (attachment.Value != path)
            File.Copy(attachment.Value, path, true); //TODO may need to put try-catch block just in case
        }
        return true;
      } catch {
        return false;
      }
    }
  }
}
