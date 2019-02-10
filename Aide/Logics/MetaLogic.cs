using Aibe.Models.DB;
using Aide.Helpers;
using Aide.Models.Results;
using Extension.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Extension.String;
using Extension.Database.SqlServer;

namespace Aide.Logics {
  public class MetaLogic {
    public static string SettingsFolderPath { get; set; } = string.Empty; //should be put in the beginning
    public static List<string> GetAllConfiguredTables() {
      List<object> items = SQLServerHandler.GetSingleColumn(Aibe.DH.DataDBConnectionString, 
        Aibe.DH.MetaTableName, Aide.DH.TableNameColumnName);
      return items
        .Where(x => x != null)
        .Select(x => x.ToString())
        .OrderBy(x => x.ToLower())
        .ToList();
    }

    public static string GetDescriptionFor(string tableName, string columnName) {
      List<object> items = SQLServerHandler.GetSingleColumnWhere(Aibe.DH.DataDBConnectionString,
        Aibe.DH.MetaTableName, columnName, 
        string.Concat(Aide.DH.TableNameColumnName, "=", tableName.AsSqlStringValue()));
      return items.Count > 0 ? items[0].ToString() : string.Empty;
    }

    public static List<string> GetAllMetaColumns() {
      return SQLServerHandler.GetColumns(Aibe.DH.DataDBConnectionString, Aibe.DH.MetaTableName)
        .Select(x => x.ColumnName)
        .ToList();
    }

    public static MetaResult ApplyUpdates(int id) {
      MetaResult result = new MetaResult();
      DataTable table = SQLServerHandler.GetFullDataTableWhere(Aibe.DH.DataDBConnectionString, Aibe.DH.MetaTableName, string.Concat(Aibe.DH.Cid, "=", id));
      BaseMetaItem meta = BaseMetaItem.ExtractMetaFromDataTable(table);
      if (meta == null) {
        result.ErrorMessage = Aibe.LCZ.NFE_IdNotFound;
        return result;
      }
      AideTableHelper.UpdateMeta(meta);
      result.IsSuccessful = true;
      result.SuccessfulMessage = string.Format(Aibe.LCZ.M_ItemIsUpdated, id);
      return result;
    }

    public static MetaResult CryptoSerialize(int id) {
      MetaResult result = new MetaResult();
      if (string.IsNullOrWhiteSpace(SettingsFolderPath)) {
        result.ErrorMessage = string.Format(Aibe.LCZ.E_FolderPathNotInitialized, Aibe.LCZ.W_Settings);
        return result;
      }

      DataTable table = SQLServerHandler.GetFullDataTableWhere(Aibe.DH.DataDBConnectionString, Aibe.DH.MetaTableName, string.Concat(Aibe.DH.Cid, "=", id));
      BaseMetaItem meta = BaseMetaItem.ExtractMetaFromDataTable(table);
      if (meta == null) {
        result.ErrorMessage = Aibe.LCZ.NFE_IdNotFound;
        return result;
      }
      Cryptography.CryptoSerialize(meta, SettingsFolderPath, meta.TableName);
      result.IsSuccessful = true;
      result.SuccessfulMessage = string.Format(Aibe.LCZ.M_MetaItemIsCryptoSerialized, id);
      return result;
    }

    public static MetaResult ApplyAllUpdates() {
      DataTable table = SQLServerHandler.GetFullDataTable(Aibe.DH.DataDBConnectionString, Aibe.DH.MetaTableName);
      List<BaseMetaItem> metas = BaseMetaItem.ExtractMetasFromDataTable(table);
      foreach (var meta in metas)
        AideTableHelper.UpdateMeta(meta);
      MetaResult result = new MetaResult();
      result.IsSuccessful = true;
      result.SuccessfulMessage = string.Format(Aibe.LCZ.M_MetaItemsAreUpdated, metas.Count);
      return result;
    }

    public static MetaResult CryptoSerializeAll() {
      MetaResult result = new MetaResult();
      if (string.IsNullOrWhiteSpace(SettingsFolderPath)) {
        result.ErrorMessage = string.Format(Aibe.LCZ.E_FolderPathNotInitialized, Aibe.LCZ.W_Settings);
        return result;
      }
      DataTable table = SQLServerHandler.GetFullDataTable(Aibe.DH.DataDBConnectionString, Aibe.DH.MetaTableName);
      List<BaseMetaItem> metas = BaseMetaItem.ExtractMetasFromDataTable(table);
      var fileNames = metas.Select(x => x.TableName).ToList();
      Cryptography.CryptoSerializeAll(metas, SettingsFolderPath, fileNames);
      result.IsSuccessful = true;
      result.SuccessfulMessage = string.Format(Aibe.LCZ.M_CryptoSerializeAllSuccess, metas.Count);
      return result;
    }

    public static MetaResult DecryptoSerializeAll() {
      MetaResult result = new MetaResult();
      if (string.IsNullOrWhiteSpace(SettingsFolderPath)) {
        result.ErrorMessage = string.Format(Aibe.LCZ.E_FolderPathNotInitialized, Aibe.LCZ.W_Settings);
        return result;
      }
      int count = AideTableHelper.DecryptMetaItems(SettingsFolderPath);
      result.IsSuccessful = true;
      result.SuccessfulMessage = string.Format(Aibe.LCZ.M_DecryptoSerializeAllSuccess, count, Aibe.LCZ.W_CryptExtension.ToUpper());
      return result;
    }
  }
}

//public ActionResult Success(string msg) {
//  ViewBag.SuccessMessage = msg;
//  return View();
//}

//public ActionResult CryptoSerialize(int id) {
//  MetaItem meta = db.MetaItems.FirstOrDefault(x => x.Cid == id);
//  if (meta == null)
//    return redirectToError(Aibe.LCZ.NFE_IdNotFound);
//  return View(meta);
//}

//public MetaResult Index(int? page) {
//  MetaResult result = new MetaResult();
//  var allOrderedMatches = db.MetaItems
//    .OrderBy(x => x.TableName.ToLower());
//  NavDataModel navDataModel;
//  List<MetaItem> results = ViewHelper.PrepareFilteredModels(page, allOrderedMatches, out navDataModel);
//  result.NavDataModel = navDataModel;
//  result.Items = results;
//  result.IsSuccessful = true;
//  return result;
//}

//public MetaResult Index(MetaFilter filter) {
//  MetaResult result = new MetaResult();
//  var unfiltereds = db.MetaItems
//    .OrderBy(x => x.TableName.ToLower());
//  var filtereds = AideDataFilterHelper.ApplyMetaFilter(unfiltereds, filter);
//  var unordereds = filtereds
//    .OrderBy(x => x.TableName.ToLower());
//  result.Filter = filter;
//  NavDataModel navDataModel;
//  List<MetaItem> results = ViewHelper.PrepareFilteredModels(filter.Page, unordereds, out navDataModel);
//  result.NavDataModel = navDataModel;
//  result.Items = results;
//  return result;
//}

//public ActionResult Create() {
//  return View();
//}

//private RedirectToRouteResult redirectToError(string error) {
//  return RedirectToAction("ErrorLocal", new { error = error });
//}

//public ActionResult ErrorLocal(string error) {
//  ViewBag.Error = error;
//  return View("Error");
//}

//public MetaResult Create(MetaItem model) {
//  if (!ModelState.IsValid)
//    return View(model);

//  db.MetaItems.Add(model);
//  db.SaveChanges();
//  AiweTableHelper.AddMeta(new MetaInfo(model)); //has validity check in the AddMeta method

//  return RedirectToAction("Index");
//}

//public ActionResult Details(string id) {
//  MetaItem meta = db.MetaItems.FirstOrDefault(x => x.TableName == id);
//  if (meta == null)
//    return redirectToError("Id not found");
//  return View(meta);
//}

//public ActionResult Delete(string id) {
//  MetaItem meta = db.MetaItems.FirstOrDefault(x => x.TableName == id);
//  if (meta == null)
//    return redirectToError("Id not found");
//  return View(meta);
//}

//[HttpPost]
//[ActionName("Delete")]
//public ActionResult DeletePost(string id) {
//  MetaItem meta = db.MetaItems.FirstOrDefault(x => x.TableName == id);
//  if (meta == null)
//    return redirectToError("Id not found");
//  db.MetaItems.Remove(meta);
//  db.SaveChanges();
//  AiweTableHelper.DeleteMeta(id);
//  return RedirectToAction("Index");
//}

//public ActionResult Edit(string id) {
//  MetaItem meta = db.MetaItems.FirstOrDefault(x => x.TableName == id);
//  if (meta == null)
//    return redirectToError("Id not found");
//  return View(meta);
//}

//[HttpPost]
//[ValidateInput(false)]
//public ActionResult Edit(MetaItem model) {
//  MetaItem meta = db.MetaItems.FirstOrDefault(x => x.TableName == model.TableName);
//  if (!ModelState.IsValid)
//    return View(model);

//  if (meta == null)
//    return redirectToError("Id not found");

//  //meta.TableName = model.TableName; //Not needed because it is the key
//  meta.DisplayName = model.DisplayName;
//  meta.ItemsPerPage = model.ItemsPerPage;
//  meta.OrderBy = model.OrderBy;
//  meta.ActionList = model.ActionList;
//  meta.DefaultActionList = model.DefaultActionList;
//  meta.TableActionList = model.TableActionList;
//  meta.DefaultTableActionList = model.DefaultTableActionList;
//  meta.TextFieldColumns = model.TextFieldColumns;
//  meta.PictureColumns = model.PictureColumns;
//  meta.IndexShownPictureColumns = model.IndexShownPictureColumns;
//  meta.RequiredColumns = model.RequiredColumns;
//  meta.NumberLimitColumns = model.NumberLimitColumns;
//  meta.RegexCheckedColumns = model.RegexCheckedColumns;
//  meta.RegexCheckedColumnExamples = model.RegexCheckedColumnExamples;
//  meta.UserRelatedFilters = model.UserRelatedFilters;
//  meta.DisableFilter = model.DisableFilter;
//  meta.ColumnExclusionList = model.ColumnExclusionList;
//  meta.FilterExclusionList = model.FilterExclusionList;
//  meta.DetailsExclusionList = model.DetailsExclusionList;
//  meta.CreateEditExclusionList = model.CreateEditExclusionList;
//  meta.AccessExclusionList = model.AccessExclusionList;
//  meta.ColoringList = model.ColoringList;
//  meta.FilterDropDownLists = model.FilterDropDownLists;
//  meta.CreateEditDropDownLists = model.CreateEditDropDownLists;
//  meta.PrefixesOfColumns = model.PrefixesOfColumns;
//  meta.PostfixesOfColumns = model.PostfixesOfColumns;
//  meta.ListColumns = model.ListColumns;
//  meta.TimeStampColumns = model.TimeStampColumns;
//  meta.HistoryTable = model.HistoryTable;
//  meta.HistoryTrigger = model.HistoryTrigger;
//  meta.AutoGeneratedColumns = model.AutoGeneratedColumns;
//  meta.ColumnSequence = model.ColumnSequence;
//  meta.ColumnAliases = model.ColumnAliases;
//  meta.EditShowOnlyColumns = model.EditShowOnlyColumns;
//  meta.ScriptConstructorColumns = model.ScriptConstructorColumns;
//  meta.ScriptColumns = model.ScriptColumns;

//  db.MetaItems.AddOrUpdate(meta);
//  db.SaveChanges();

//  AiweTableHelper.UpdateMeta(meta);

//  return RedirectToAction("Index");
//}

//public ActionResult CryptoSerialize(string id) {
//  MetaItem meta = db.MetaItems.FirstOrDefault(x => x.TableName == id);
//  if (meta == null)
//    return redirectToError("Id not found");
//  return View(meta);
//}

//[HttpPost]
//[ActionName("CryptoSerialize")]
//public ActionResult CryptoSerializePost(string id) {
//  MetaItem meta = db.MetaItems.FirstOrDefault(x => x.TableName == id);
//  if (meta == null)
//    return redirectToError("Id not found");
//  string folderPath = Server.MapPath("~/Settings");
//  Cryptography.CryptoSerialize(meta, folderPath, id);
//  return RedirectToAction("Index");
//}

//public ActionResult CryptoSerializeAll() {
//  var all = db.MetaItems.ToList();
//  var fileNames = all.Select(x => x.TableName).ToList();
//  string folderPath = Server.MapPath("~/Settings");
//  Cryptography.CryptoSerializeAll(all, folderPath, fileNames);
//  return RedirectToAction("Success", new { msg = "You have successfully crypto-serialize all (" + all.Count + ") meta table entries!" });
//}

//public ActionResult Success(string msg) {
//  ViewBag.SuccessMessage = msg;
//  return View();
//}

//public ActionResult DecryptoSerializeAll() {
//  string folderPath = Server.MapPath("~/Settings");
//  int count = AiweTableHelper.DecryptMetaItems(folderPath);
//  AiweTableHelper.PrepareMetas();
//  return RedirectToAction("Success", new { msg = "You have successfully decrypto-serialize all (" + count + ") meta table ASTRIOCFILE(s)!" });
//}

//using Aide.Models.DB;
//using Aibe.Models.Filters;
//using Extension.String;
//using System.Linq;

//namespace Aide.Helpers {
//  public class AideDataFilterHelper {
//    public static IQueryable<MetaItem> ApplyMetaFilter(IQueryable<MetaItem> unfiltered, MetaFilter filter) {
//      IQueryable<MetaItem> filtered = unfiltered;

//      if (!string.IsNullOrWhiteSpace(filter.TableName))
//        filtered = filtered
//          .Where(x => x.TableName != null &&
//            x.TableName.ToLower().Contains(filter.TableName.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.DisplayName))
//        filtered = filtered
//          .Where(x => x.DisplayName != null &&
//            x.DisplayName.ToLower().Contains(filter.DisplayName.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.OrderBy))
//        filtered = filtered
//          .Where(x => x.OrderBy != null &&
//            x.OrderBy.ToLower().Contains(filter.OrderBy.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.ActionList))
//        filtered = filtered
//          .Where(x => x.ActionList != null &&
//            x.ActionList.ToLower().Contains(filter.ActionList.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.DefaultActionList))
//        filtered = filtered
//          .Where(x => x.DefaultActionList != null &&
//            x.DefaultActionList.ToLower().Contains(filter.DefaultActionList.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.TableActionList))
//        filtered = filtered
//          .Where(x => x.TableActionList != null &&
//            x.TableActionList.ToLower().Contains(filter.TableActionList.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.DefaultTableActionList))
//        filtered = filtered
//          .Where(x => x.DefaultTableActionList != null &&
//            x.DefaultTableActionList.ToLower().Contains(filter.DefaultTableActionList.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.TextFieldColumns))
//        filtered = filtered
//          .Where(x => x.TextFieldColumns != null &&
//            x.TextFieldColumns.ToLower().Contains(filter.TextFieldColumns.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.PictureColumns))
//        filtered = filtered
//          .Where(x => x.PictureColumns != null &&
//            x.PictureColumns.ToLower().Contains(filter.PictureColumns.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.IndexShownPictureColumns))
//        filtered = filtered
//          .Where(x => x.IndexShownPictureColumns != null &&
//            x.IndexShownPictureColumns.ToLower().Contains(filter.IndexShownPictureColumns.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.RequiredColumns))
//        filtered = filtered
//          .Where(x => x.RequiredColumns != null &&
//            x.RequiredColumns.ToLower().Contains(filter.RequiredColumns.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.NumberLimitColumns))
//        filtered = filtered
//          .Where(x => x.NumberLimitColumns != null &&
//            x.NumberLimitColumns.ToLower().Contains(filter.NumberLimitColumns.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.RegexCheckedColumns))
//        filtered = filtered
//          .Where(x => x.RegexCheckedColumns != null &&
//            x.RegexCheckedColumns.ToLower().Contains(filter.RegexCheckedColumns.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.RegexCheckedColumnExamples))
//        filtered = filtered
//          .Where(x => x.RegexCheckedColumnExamples != null &&
//            x.RegexCheckedColumnExamples.ToLower().Contains(filter.RegexCheckedColumnExamples.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.UserRelatedFilters))
//        filtered = filtered
//          .Where(x => x.UserRelatedFilters != null &&
//            x.UserRelatedFilters.ToLower().Contains(filter.UserRelatedFilters.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.ColumnExclusionList))
//        filtered = filtered
//          .Where(x => x.ColumnExclusionList != null &&
//            x.ColumnExclusionList.ToLower().Contains(filter.ColumnExclusionList.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.FilterExclusionList))
//        filtered = filtered
//          .Where(x => x.FilterExclusionList != null &&
//            x.FilterExclusionList.ToLower().Contains(filter.FilterExclusionList.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.DetailsExclusionList))
//        filtered = filtered
//          .Where(x => x.DetailsExclusionList != null &&
//            x.DetailsExclusionList.ToLower().Contains(filter.DetailsExclusionList.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.CreateEditExclusionList))
//        filtered = filtered
//          .Where(x => x.CreateEditExclusionList != null &&
//            x.CreateEditExclusionList.ToLower().Contains(filter.CreateEditExclusionList.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.AccessExclusionList))
//        filtered = filtered
//          .Where(x => x.AccessExclusionList != null &&
//            x.AccessExclusionList.ToLower().Contains(filter.AccessExclusionList.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.ColoringList))
//        filtered = filtered
//          .Where(x => x.ColoringList != null &&
//            x.ColoringList.ToLower().Contains(filter.ColoringList.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.FilterDropDownLists))
//        filtered = filtered
//          .Where(x => x.FilterDropDownLists != null &&
//            x.FilterDropDownLists.ToLower().Contains(filter.FilterDropDownLists.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.CreateEditDropDownLists))
//        filtered = filtered
//          .Where(x => x.CreateEditDropDownLists != null &&
//            x.CreateEditDropDownLists.ToLower().Contains(filter.CreateEditDropDownLists.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.PrefixesOfColumns))
//        filtered = filtered
//          .Where(x => x.PrefixesOfColumns != null &&
//            x.PrefixesOfColumns.ToLower().Contains(filter.PrefixesOfColumns.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.PostfixesOfColumns))
//        filtered = filtered
//          .Where(x => x.PostfixesOfColumns != null &&
//            x.PostfixesOfColumns.ToLower().Contains(filter.PostfixesOfColumns.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.ListColumns))
//        filtered = filtered
//          .Where(x => x.ListColumns != null &&
//            x.ListColumns.ToLower().Contains(filter.ListColumns.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.TimeStampColumns))
//        filtered = filtered
//          .Where(x => x.TimeStampColumns != null &&
//            x.TimeStampColumns.ToLower().Contains(filter.TimeStampColumns.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.HistoryTable))
//        filtered = filtered
//          .Where(x => x.HistoryTable != null &&
//            x.HistoryTable.ToLower().Contains(filter.HistoryTable.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.HistoryTrigger))
//        filtered = filtered
//          .Where(x => x.HistoryTrigger != null &&
//            x.HistoryTrigger.ToLower().Contains(filter.HistoryTrigger.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.AutoGeneratedColumns))
//        filtered = filtered
//          .Where(x => x.AutoGeneratedColumns != null &&
//            x.AutoGeneratedColumns.ToLower().Contains(filter.AutoGeneratedColumns.ToLower()));

//      if (filter.ItemsPerPageFrom.HasValue || filter.ItemsPerPageTo.HasValue) {
//        int from = filter.ItemsPerPageFrom.HasValue ? filter.ItemsPerPageFrom.Value : 0;
//        int to = filter.ItemsPerPageTo.HasValue ? filter.ItemsPerPageTo.Value : int.MaxValue;
//        filtered = filtered
//          .Where(x => x.ItemsPerPage >= from && x.ItemsPerPage <= to);
//      }

//      if (!string.IsNullOrWhiteSpace(filter.DisableFilter)) {
//        if (filter.DisableFilter.EqualsIgnoreCase("true")) {
//          filtered = filtered.Where(x => x.DisableFilter.Value);
//        } else {
//          filtered = filtered.Where(x => !x.DisableFilter.Value);
//        }
//      }

//      if (!string.IsNullOrWhiteSpace(filter.ColumnSequence))
//        filtered = filtered
//          .Where(x => x.ColumnSequence != null &&
//            x.ColumnSequence.ToLower().Contains(filter.ColumnSequence.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.ColumnAliases))
//        filtered = filtered
//          .Where(x => x.ColumnAliases != null &&
//            x.ColumnAliases.ToLower().Contains(filter.ColumnAliases.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.EditShowOnlyColumns))
//        filtered = filtered
//          .Where(x => x.EditShowOnlyColumns != null &&
//            x.EditShowOnlyColumns.ToLower().Contains(filter.EditShowOnlyColumns.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.ScriptConstructorColumns))
//        filtered = filtered
//          .Where(x => x.ScriptConstructorColumns != null &&
//            x.ScriptConstructorColumns.ToLower().Contains(filter.ScriptConstructorColumns.ToLower()));

//      if (!string.IsNullOrWhiteSpace(filter.ScriptColumns))
//        filtered = filtered
//          .Where(x => x.ScriptColumns != null &&
//            x.ScriptColumns.ToLower().Contains(filter.ScriptColumns.ToLower()));

//      return filtered;
//    }
//  }
//}