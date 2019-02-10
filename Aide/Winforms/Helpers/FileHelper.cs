using System.IO;
using System.Windows.Forms;

namespace Aide.Winforms.Helpers {
  public class FileHelper {
    public static string GetSettingsFolderPath() {
      return Application.StartupPath + "\\" + Aibe.DH.DefaultSettingFolderName;
    }

    public static string GetImageFolderPath() {
      return Application.StartupPath + "\\" + Aibe.DH.DefaultImageFolderName;
    }

    public static string GetAttachmentFolderPath() {
      return Application.StartupPath + "\\" + Aibe.DH.DefaultAttachmentFolderName;
    }

    public static string GetDownloadFolderPath() {
      return Application.StartupPath + "\\" + Aibe.DH.DefaultDownloadFolderName;
    }

    public static string GetImagePath(string fullRelativePath) {
      return Path.Combine(GetImageFolderPath(), fullRelativePath);
    }

    public static string GetAttachmentPath(string fullRelativePath) {
      return Path.Combine(GetAttachmentFolderPath(), fullRelativePath);
    }

    public static string GetDownloadPath(string fullRelativePath) {
      return Path.Combine(GetDownloadFolderPath(), fullRelativePath);
    }
  }
}