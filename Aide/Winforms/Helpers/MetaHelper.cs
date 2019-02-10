using Aide.Customs;
using Aide.Logics;
using Aide.Models;
using Aide.Models.Results;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Aide.Winforms.Helpers {
  public class MetaHelper {
    public static Dictionary<string, CustomizedRowActionDelegate> CustomizedRowActions = new Dictionary<string, CustomizedRowActionDelegate>();
    public static Dictionary<string, CustomizedTableActionDelegate> CustomizedTableActions = new Dictionary<string, CustomizedTableActionDelegate>();

    public static void Init() {
      CustomizedRowActions = new Dictionary<string, CustomizedRowActionDelegate> {
        { Aibe.DH.MetaTableName + "-" + Aibe.LCZ.I_MetaItemApplyUpdatesActionName, applyUpdates },
        { Aibe.DH.MetaTableName + "-" + Aibe.LCZ.I_MetaItemCryptoSerializeActionName, cryptoSerialize },
      };
      CustomizedTableActions = new Dictionary<string, CustomizedTableActionDelegate> {
        { Aibe.DH.MetaTableName + "-" + Aibe.LCZ.I_MetaItemApplyAllUpdatesActionName, applyAllUpdates },
        { Aibe.DH.MetaTableName + "-" + Aibe.LCZ.I_MetaItemCryptoSerializeAllActionName, cryptoSerializeAll },
        { Aibe.DH.MetaTableName + "-" + Aibe.LCZ.I_MetaItemDecryptoSerializeAllActionName, decryptoSerializeAll },
      };
    }

    private static void handleMetaResult(MetaResult result) {
      if (result.IsSuccessful)
        MessageBox.Show(result.SuccessfulMessage, Aibe.LCZ.W_Successful, MessageBoxButtons.OK, MessageBoxIcon.Information);
      else
        MessageBox.Show(result.ErrorMessage, Aibe.LCZ.W_Failed, MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    private static void applyUpdates(int id, List<KeyValuePair<string, object>> identifiers) {
      handleMetaResult(MetaLogic.ApplyUpdates(id));
    }

    private static void cryptoSerialize(int id, List<KeyValuePair<string, object>> identifiers) {
      handleMetaResult(MetaLogic.CryptoSerialize(id));
    }

    private static void applyAllUpdates(AideBaseFilterIndexModel fiModel, Dictionary<string, string> filters) {
      handleMetaResult(MetaLogic.ApplyAllUpdates());
    }

    private static void cryptoSerializeAll(AideBaseFilterIndexModel fiModel, Dictionary<string, string> filters) {
      handleMetaResult(MetaLogic.CryptoSerializeAll());
    }

    private static void decryptoSerializeAll(AideBaseFilterIndexModel fiModel, Dictionary<string, string> filters) {
      handleMetaResult(MetaLogic.DecryptoSerializeAll());
    }
  }
}