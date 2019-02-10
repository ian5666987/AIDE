using Aide.Logics;
using Aide.Winforms.Helpers;

namespace Aide.Winforms {
  public class Initializer { //can be extended as wanted
    public static void Init() {
      MetaLogic.SettingsFolderPath = FileHelper.GetSettingsFolderPath();
      MetaHelper.Init(); //must be done after localization and before add customized actions
      CommonIndexForm.AddCustomizedRowActions(MetaHelper.CustomizedRowActions);
      CommonIndexForm.AddCustomizedTableActions(MetaHelper.CustomizedTableActions);
    }
  }
}