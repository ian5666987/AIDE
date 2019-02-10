using Aibe.Helpers;
using Aibe.Models;
using Aibe.Models.DB;
using Extension.Cryptography;
using Extension.Database.SqlServer;
using Extension.String;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Aide.Helpers {
  public class AideTableHelper {
    private static List<IMetaItem> metaList = new List<IMetaItem>();
    private static List<MetaInfo> metaInfoList = new List<MetaInfo>();
    public static bool IsRelease { get; set; }
    private static List<IMetaItem> getMetaItems() {
      if (IsRelease)
        return metaList;
      DataTable table = SQLServerHandler.GetFullDataTable(Aibe.DH.DataDBConnectionString, Aibe.DH.MetaTableName);
      return BaseMetaItem.ExtractMetasFromDataTable(table)
        .Select(x => (IMetaItem)x).ToList();
    }

    public static IEnumerable<MetaInfo> GetMetas() {
      return metaInfoList;
    }

    public static void PrepareMetas() {
      metaInfoList = getMetaItems().Select(x => new MetaInfo(x)).Where(x => x.IsValid).ToList();
    }

    public static void AddMeta(MetaInfo meta) {
      if (meta.IsValid)
        metaInfoList.Add(meta);
    }

    public static void DeleteMeta(string tableName) {
      MetaInfo removedItem = GetMeta(tableName);
      if (removedItem != null)
        metaInfoList.Remove(removedItem);
    }

    public static MetaInfo GetMeta(string tableName) {
      MetaInfo meta = GetMetas()
        .FirstOrDefault(x => x.TableName.EqualsIgnoreCase(tableName));
      return meta;
    }

    public static void UpdateMeta(IMetaItem metaItem) {
      MetaInfo editedItem = GetMeta(metaItem.TableName);
      editedItem.AssignParameters(metaItem);
    }

    public static int DecryptMetaItems(string folderPath) {
      int count = 0;
      try {
        List<BaseMetaItem> metaItems = Cryptography.DecryptoSerializeAll<BaseMetaItem>(folderPath);
        count = metaItems.Count;

        if (IsRelease) {
          metaList.Clear();
          metaList.AddRange(metaItems);
        } else {
          List<BaseMetaItem> currentMetaItems = getMetaItems().Select(x => (BaseMetaItem)x).ToList();
          foreach (BaseMetaItem metaItem in metaItems) {
            if (currentMetaItems.Any(x => x.Cid == metaItem.Cid)) { //old item
              string sqlString = metaItem.BuildSqlUpdateString();
              SQLServerHandler.ExecuteScript(Aibe.DH.DataDBConnectionString, sqlString);
            } else { //new item
              string sqlString = metaItem.BuildSqlInsertString();
              SQLServerHandler.ExecuteScalar(Aibe.DH.DataDBConnectionString, sqlString);
            }
          }
        }

        PrepareMetas();

      } catch (Exception ex) {
        LogHelper.Error(null, null, null, null, "Meta", "Decrypt", null, ex.ToString());
        throw ex;
      }
      return count;
    }
  }
}