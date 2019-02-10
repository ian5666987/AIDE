using Aibe.Helpers;
using Aibe.Models;
using Aide.ActionFilters;
using Aide.Helpers;
using Aide.Models;
using Aide.Models.Accounts;
using Extension.Database.SqlServer;
using Extension.Models;
using Extension.String;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Aide.Logics {
  public class CommonLogic {
    public static BaseErrorModel Index(string commonDataTableName, int? page = 1, Dictionary<string, string> collections = null) {
      BaseErrorModel errorModel = CommonActionFilter.OnActionExecuting(commonDataTableName, Aide.DH.CommonLogicName, Aibe.DH.IndexActionName, Aide.DH.GetRequest, 
        id: null, identifiers: null, collections: collections); //Can actually be post, but doesn't matter for index
      if (errorModel.HasError)
        return errorModel;
      errorModel.ReturnObject = GetFilterIndexModel(commonDataTableName, page, collections, loadAllData: false);
      //BaseErrorModel errorModel = new BaseErrorModel { ReturnObject = getFilterIndexModel(commonDataTableName, page, collections, loadAllData: false) };
      return errorModel;
    }

    public static AideFilterIndexModel GetFilterIndexModel(string commonDataTableName, int? page = 1, Dictionary<string, string> collections = null, bool loadAllData = false) {
      MetaInfo meta = AideTableHelper.GetMeta(commonDataTableName);      
      FilterIndexModel model = new FilterIndexModel(meta, page, collections);
      QueryHelper.HandleUserRelatedScripting(model.QueryScript, Aide.PH.UserTableName, Identity.User?.UserName, 
        Identity.IsMainAdmin(),
        Identity.User != null && !string.IsNullOrWhiteSpace(Identity.User.UserName), meta.UserRelatedFilters);
      model.CompleteModelAndData(meta.IsGroupTable, loadAllData); //FilterIndex depends on IsGroupTable, but GroupDetails is always false
      return new AideFilterIndexModel(meta, model, model.StringDictionary);
    }

    public static BaseErrorModel Create(string commonDataTableName, List<KeyValuePair<string, object>> identifiers) {
      BaseErrorModel errorModel = CommonActionFilter.OnActionExecuting(commonDataTableName, Aide.DH.CommonLogicName, Aibe.DH.CreateActionName, Aide.DH.GetRequest, 
        id: null, identifiers: identifiers, collections: null);
      if (errorModel.HasError)
        return errorModel;
      errorModel.ReturnObject = new AideCreateEditModel(AideTableHelper.GetMeta(commonDataTableName), Aibe.DH.CreateActionName, null, identifiers);
      return errorModel;
    }

    //There is additional attachments here... actually for Aide, we can combine the collections and attachments into AideRequestModel too
    public static BaseErrorModel Create(string commonDataTableName, List<KeyValuePair<string, object>> identifiers, AideRequestModel model) {
      BaseErrorModel errorModel = CommonActionFilter.OnActionExecuting(commonDataTableName, Aide.DH.CommonLogicName, Aibe.DH.CreateActionName, Aide.DH.PostRequest, 
        id: null, identifiers: identifiers, collections: model?.FormCollection);
      if (errorModel.HasError)
        return errorModel;

      CheckerHelper checker = new CheckerHelper();
      MetaInfo meta = AideTableHelper.GetMeta(commonDataTableName);
      DateTime now = DateTime.Now;

      List<string> checkExclusions = new List<string> { Aibe.DH.TableNameParameterName }; //different per Action, because of additional item in the ModelState

      //Check model state's validity
      Dictionary<string, string> errorDict = checker.CheckModelValidity(Aide.PH.TableModelClassPrefix, meta.TableSource, meta.ArrangedDataColumns,
        model.FormCollection, model.FormCollection.Keys.ToList(), meta, checkExclusions, Identity.IsDeveloper(), now, Aibe.DH.CreateActionName, strongCheck: Aide.PH.UseStrongCheck,
        isTagChecked: Aide.PH.isTagChecked);
      if (errorDict != null && errorDict.Count > 0) { //purposely not an error, just return the object
        errorModel.ReturnObject = new AideCreateEditModel(meta, Aibe.DH.CreateActionName, model.FormCollection, identifiers) { ErrorDict = errorDict };
        return errorModel;
      }

      //Only if model state is correct that we could get valid key infos safely
      var completeKeyInfo = KeyInfoHelper.GetCompleteKeyInfo(meta.TableSource, model.FormCollection, model.FormCollection.Keys, meta.ArrangedDataColumns, filterStyle: false, meta: meta, actionType: Aibe.DH.CreateActionName);
      if (completeKeyInfo == null || completeKeyInfo.ValidKeys == null || !completeKeyInfo.ValidKeys.Any()) { //purposely not an error, just return the object
        errorModel.ReturnObject = new AideCreateEditModel(meta, Aibe.DH.CreateActionName, model.FormCollection, identifiers) {
          ErrorMessage = string.Format(Aibe.LCZ.E_InvalidOrEmptyParameter, commonDataTableName)
        };
        return errorModel;
      }

      var userPars = AideUserHelper.GetUserParameters(Identity.User, Aibe.DH.ParameterUserPrefix);
      List<object> preActionProcedureResults = meta.HandlePreActionProcedures(Aibe.DH.CreateActionName, -1, null, userPars); //pre action does not have cid or row

      BaseScriptModel scriptModel = LogicHelper.CreateInsertScriptModel(meta.TableSource, completeKeyInfo, model.FormCollection, now, meta);
      object generatedId = SQLServerHandler.ExecuteScalar(Aibe.DH.DataDBConnectionString, scriptModel.Script, scriptModel.Pars);
      int cid = int.Parse(generatedId.ToString());
      bool saveAttachmentResult = AideFileHelper.SaveAttachments(model.Attachments, 
        model.AttachmentBaseFolderPath + "\\" + commonDataTableName + "\\" + cid);

      //Email and history
      List<int> emailTriggerResults = meta.HandleEmailEvents(Aibe.DH.CreateActionName, cid, null, userPars); //create has no originalRow
      List<int> historyTriggerResults = meta.HandleHistoryEvents(Aibe.DH.CreateActionName, cid, null); //create has no originalRow
      List<object> postActionProcedureResults = meta.HandlePostActionProcedures(Aibe.DH.CreateActionName, cid, null, userPars); //post action has no row BUT has cid

      errorModel.ReturnObject = new AideCreateEditModel(meta, Aibe.DH.CreateActionName, model.FormCollection, identifiers) { //purposely not an error, just return the object
        IsSuccessful = true, SaveAttachmentResult = saveAttachmentResult, HasAttachment = model.Attachments != null && model.Attachments.Count > 0,
        HistoryTriggerResults = historyTriggerResults, EmailTriggerResults = emailTriggerResults,
        PreActionTriggerResults = preActionProcedureResults, PostActionTriggerResults = postActionProcedureResults,
      }; //the only part where it returns true for IsSuccessful
      return errorModel;
    }

    public static BaseErrorModel Edit(string commonDataTableName, List<KeyValuePair<string, object>> identifiers, int id) {
      BaseErrorModel errorModel = CommonActionFilter.OnActionExecuting(commonDataTableName, Aide.DH.CommonLogicName, Aibe.DH.EditActionName, Aide.DH.GetRequest, 
        id: id, identifiers: identifiers, collections: null); //get doesn't have collections
      if (errorModel.HasError)
        return errorModel;

      MetaInfo meta = AideTableHelper.GetMeta(commonDataTableName);
      Dictionary<string, object> objectDictionary = LogicHelper.FillDetailsFromTableToObjectDictionary(meta.TableSource, id);
      errorModel.ReturnObject = new AideCreateEditModel(meta, Aibe.DH.EditActionName, LogicHelper.ObjectDictionaryToStringDictionary(objectDictionary), identifiers);
      return errorModel;
    }

    //Post
    public static BaseErrorModel Edit(string commonDataTableName, List<KeyValuePair<string, object>> identifiers, AideRequestModel model) {
      BaseErrorModel errorModel = CommonActionFilter.OnActionExecuting(commonDataTableName, Aide.DH.CommonLogicName, Aibe.DH.EditActionName, Aide.DH.PostRequest, 
        id: model.Cid, identifiers: identifiers, collections: model.FormCollection); //get doesn't have collections
      if (errorModel.HasError)
        return errorModel;

      CheckerHelper checker = new CheckerHelper();
      MetaInfo meta = AideTableHelper.GetMeta(commonDataTableName);
      DateTime now = DateTime.Now;

      List<string> checkExclusions = new List<string> { Aibe.DH.TableNameParameterName }; //different per Action, because of additional item in the ModelState

      //Check model state's validity
      Dictionary<string, string> errorDict = checker.CheckModelValidity(Aide.PH.TableModelClassPrefix, meta.TableSource, meta.ArrangedDataColumns,
        model.FormCollection, model.FormCollection.Keys.ToList(), meta, checkExclusions, Identity.IsDeveloper(), now, Aibe.DH.EditActionName, strongCheck: Aide.PH.UseStrongCheck,
        isTagChecked: Aide.PH.isTagChecked);
      if (errorDict != null && errorDict.Count > 0) { //purposely not an error, just return the object
        errorModel.ReturnObject = new AideCreateEditModel(meta, Aibe.DH.EditActionName, model.FormCollection, identifiers) { ErrorDict = errorDict };
        return errorModel;
      }
        
      var filteredKeys = model.FormCollection.Keys.Where(x => !x.EqualsIgnoreCase(Aibe.DH.Cid)); //everything filled but the Cid
      var completeKeyInfo = KeyInfoHelper.GetCompleteKeyInfo(meta.TableSource, model.FormCollection, filteredKeys, meta.ArrangedDataColumns, filterStyle: false, meta: meta, actionType: Aibe.DH.EditActionName);
      if (completeKeyInfo == null || completeKeyInfo.ValidKeys == null || !completeKeyInfo.ValidKeys.Any()) { //purposely not an error, just return the object
        errorModel.ReturnObject = new AideCreateEditModel(meta, Aibe.DH.EditActionName, model.FormCollection, identifiers) {
          ErrorMessage = string.Format(Aibe.LCZ.E_InvalidOrEmptyParameter, commonDataTableName)
        };
        return errorModel;
      }

      var userPars = AideUserHelper.GetUserParameters(Identity.User, Aibe.DH.ParameterUserPrefix);
      DataRow originalRow = meta.GetFullRowSource(model.Cid);
      List<object> preActionProcedureResults = meta.HandlePreActionProcedures(Aibe.DH.EditActionName, model.Cid, originalRow, userPars);

      BaseScriptModel scriptModel = LogicHelper.CreateUpdateScriptModel(meta.TableSource, model.Cid, completeKeyInfo, model.FormCollection, now);
      SQLServerHandler.ExecuteScript(Aibe.DH.DataDBConnectionString, scriptModel.Script, scriptModel.Pars);
      bool saveAttachmentResult = AideFileHelper.SaveAttachments(model.Attachments,
        model.AttachmentBaseFolderPath + "\\" + commonDataTableName + "\\" + model.Cid);
      
      List<int> emailTriggerResults = meta.HandleEmailEvents(Aibe.DH.EditActionName, model.Cid, originalRow, userPars); //Email      
      List<int> historyTriggerResults = meta.HandleHistoryEvents(Aibe.DH.EditActionName, model.Cid, originalRow); //History
      List<object> postActionProcedureResults = meta.HandlePostActionProcedures(Aibe.DH.EditActionName, model.Cid, originalRow, userPars);

      errorModel.ReturnObject = new AideCreateEditModel(meta, Aibe.DH.EditActionName, model.FormCollection, identifiers) { //purposely not an error, just return the object
        IsSuccessful = true, SaveAttachmentResult = saveAttachmentResult, HasAttachment = model.Attachments != null && model.Attachments.Count > 0,
        IsHistoryTriggered = meta.HasValidHistoryTable, HistoryTriggerResults = historyTriggerResults, EmailTriggerResults = emailTriggerResults,
        PreActionTriggerResults = preActionProcedureResults, PostActionTriggerResults = postActionProcedureResults,
      }; //the only part where it returns true for IsSuccessful
      return errorModel;
    }

    public static BaseErrorModel Delete(string commonDataTableName, int id) {
      BaseErrorModel errorModel = CommonActionFilter.OnActionExecuting(commonDataTableName, Aide.DH.CommonLogicName, Aibe.DH.DeleteActionName, Aide.DH.GetRequest, 
        id: id, identifiers: null, collections: null); //delete doesn't have collection
      if (errorModel.HasError)
        return errorModel;
      MetaInfo meta = AideTableHelper.GetMeta(commonDataTableName);
      Dictionary<string, object> objectDictionary = LogicHelper.FillDetailsFromTableToObjectDictionary(meta.TableSource, id);
      errorModel.ReturnObject = new AideDetailsModel(meta, id, LogicHelper.ObjectDictionaryToStringDictionary(objectDictionary),
        Aibe.DH.DeleteActionName);
      return errorModel;
    }

    public static BaseErrorModel DeletePost(string commonDataTableName, int id) { //Where all common tables deletes are returned and can be deleted
      BaseErrorModel errorModel = CommonActionFilter.OnActionExecuting(commonDataTableName, Aide.DH.CommonLogicName, Aibe.DH.DeleteActionName, Aide.DH.PostRequest, 
        id: id, identifiers: null, collections: null); //delete doesn't have collection
      if (errorModel.HasError)
        return errorModel;
      MetaInfo meta = AideTableHelper.GetMeta(commonDataTableName);

      var userPars = AideUserHelper.GetUserParameters(Identity.User, Aibe.DH.ParameterUserPrefix);
      DataRow originalRow = meta.GetFullRowSource(id);
      List<object> preActionProcedureResults = meta.HandlePreActionProcedures(Aibe.DH.DeleteActionName, id, originalRow, userPars);

      List<int> emailTriggerResults = meta.HandleEmailEvents(Aibe.DH.DeleteActionName, id, null, userPars); //Email and history events must be handled before deletion
      List<int> historyTriggerResults = meta.HandleHistoryEvents(Aibe.DH.EditActionName, id, null);
      LogicHelper.DeleteItem(meta.TableSource, id); //Currently do not return any error

      List<object> postActionProcedureResults = meta.HandlePostActionProcedures(Aibe.DH.DeleteActionName, -1, null, userPars); //delete has neither cid nor row

      errorModel.ReturnObject = new AideDetailsModel(meta, id, null, Aibe.DH.DeleteActionName) { IsSuccessful = true,
        HistoryTriggerResults = historyTriggerResults, EmailTriggerResults = emailTriggerResults,
        PreActionTriggerResults = preActionProcedureResults, PostActionTriggerResults = postActionProcedureResults,
      };
      return errorModel;
    }

    public static BaseErrorModel Details(string commonDataTableName, int id) {
      BaseErrorModel errorModel = CommonActionFilter.OnActionExecuting(commonDataTableName, Aide.DH.CommonLogicName, Aibe.DH.DetailsActionName, Aide.DH.GetRequest, 
        id: id, identifiers: null, collections: null); //details doesn't have collection
      if (errorModel.HasError)
        return errorModel;
      MetaInfo meta = AideTableHelper.GetMeta(commonDataTableName);
      Dictionary<string, object> objectDictionary = LogicHelper.FillDetailsFromTableToObjectDictionary(meta.TableSource, id);
      errorModel.ReturnObject = new AideDetailsModel(meta, id, LogicHelper.ObjectDictionaryToStringDictionary(objectDictionary),
        Aibe.DH.DetailsActionName);
      return errorModel;
    }

    public static BaseErrorModel CreateGroup(string commonDataTableName, List<string> identifierColumns) {
      BaseErrorModel errorModel = CommonActionFilter.OnActionExecuting(commonDataTableName, Aide.DH.CommonLogicName, Aibe.DH.CreateGroupActionName, Aide.DH.GetRequest,
        id: null, identifiers: null, collections: null); //TODO not sure if CreateGroup here should or shouldn't have CreateGroup
      if (errorModel.HasError)
        return errorModel;
      errorModel.ReturnObject = new AideCreateEditGroupModel(AideTableHelper.GetMeta(commonDataTableName),
        Aibe.DH.CreateGroupActionName, null, identifierColumns);
      return errorModel;
    }

    private static BaseErrorModel commonCreateEditGroupPost(string commonDataTableName, string actionName, 
      List<string> identifierColumns, List<KeyValuePair<string, object>> identifiers, AideRequestModel model) {

      BaseErrorModel errorModel = CommonActionFilter.OnActionExecuting(commonDataTableName, Aide.DH.CommonLogicName, 
        actionName, Aide.DH.GetRequest,
        id: null, identifiers: identifiers, collections: model?.FormCollection);
      if (errorModel.HasError)
        return errorModel;

      CheckerHelper checker = new CheckerHelper();
      MetaInfo meta = AideTableHelper.GetMeta(commonDataTableName);
      DateTime now = DateTime.Now;

      List<string> checkExclusions = new List<string> { Aibe.DH.TableNameParameterName }; //different per Action, because of additional item in the ModelState

      //Check model state's validity
      Dictionary<string, string> errorDict = checker.CheckModelValidity(Aide.PH.TableModelClassPrefix, meta.TableSource, meta.ArrangedDataColumns,
        model.FormCollection, model.FormCollection.Keys.ToList(), meta, checkExclusions, Identity.IsDeveloper(), now, 
        actionName, strongCheck: Aide.PH.UseStrongCheck, isTagChecked: Aide.PH.isTagChecked);
      if (errorDict != null && errorDict.Count > 0) { //purposely not an error, just return the object
        errorModel.ReturnObject = new AideCreateEditGroupModel(meta, actionName, model.FormCollection, identifierColumns) { ErrorDict = errorDict };
        return errorModel;
      }

      //Only if model state is correct that we could get valid key infos safely
      var completeKeyInfo = KeyInfoHelper.GetCompleteKeyInfo(meta.TableSource, model.FormCollection, model.FormCollection.Keys, meta.ArrangedDataColumns, 
        filterStyle: false, meta: meta, actionType: actionName);
      if (completeKeyInfo == null || completeKeyInfo.ValidKeys == null || !completeKeyInfo.ValidKeys.Any()) { //purposely not an error, just return the object
        errorModel.ReturnObject = new AideCreateEditGroupModel(meta, actionName, model.FormCollection, identifierColumns) {
          ErrorMessage = string.Format(Aibe.LCZ.E_InvalidOrEmptyParameter, commonDataTableName)
        };
        return errorModel;
      }

      List<KeyValuePair<string, object>> pairs = LogicHelper.GetAllAutoGeneratedPairs(meta.TableSource, true, completeKeyInfo, model.FormCollection, now, meta);
      errorModel.ReturnObject = pairs;

      return errorModel;
    }

    public static BaseErrorModel CreateGroup(string commonDataTableName, List<string> identifierColumns, AideRequestModel model) {
      return commonCreateEditGroupPost(commonDataTableName, Aibe.DH.CreateGroupActionName, identifierColumns, null, model);
    }

    public static BaseErrorModel EditGroup(string commonDataTableName, List<KeyValuePair<string, object>> identifiers) {
      BaseErrorModel errorModel = CommonActionFilter.OnActionExecuting(commonDataTableName, Aide.DH.CommonLogicName, Aibe.DH.EditGroupActionName, Aide.DH.GetRequest,
        id: null, identifiers: identifiers, collections: null);
      if (errorModel.HasError)
        return errorModel;
      Dictionary<string, string> stringDict = new Dictionary<string, string>();
      foreach (var identifier in identifiers)
        stringDict.Add(identifier.Key, identifier.Value.ToString());
      AideCreateEditGroupModel model = new AideCreateEditGroupModel(AideTableHelper.GetMeta(commonDataTableName),
        Aibe.DH.EditGroupActionName, stringDict, identifiers.Select(x => x.Key).ToList());
      errorModel.ReturnObject = model;
      return errorModel;
    }

    public static BaseErrorModel EditGroup(string commonDataTableName, List<KeyValuePair<string, object>> identifiers, AideRequestModel model) {
      List<string> identifierColumns = identifiers.Select(x => x.Key).ToList();
      return commonCreateEditGroupPost(commonDataTableName, Aibe.DH.CreateGroupActionName, identifierColumns, identifiers, model);
    }

    public static AideFilterGroupDetailsModel GetFilterGroupDetailsModel(string commonDataTableName, List<KeyValuePair<string, object>> identifiers,
      int? page = 1, Dictionary<string, string> collections = null, bool loadAllData = false, bool isGroupDeletion = false) {
      MetaInfo meta = AideTableHelper.GetMeta(commonDataTableName);
      FilterGroupDetailsModel model = new FilterGroupDetailsModel(meta, page, identifiers, collections);
      QueryHelper.HandleUserRelatedScripting(model.QueryScript, Aide.PH.UserTableName, Identity.User?.UserName,
        Identity.IsMainAdmin(),
        Identity.User != null && !string.IsNullOrWhiteSpace(Identity.User.UserName), meta.UserRelatedFilters);
      model.CompleteModelAndData(isGrouping: false, loadAllData: loadAllData);
      return new AideFilterGroupDetailsModel(meta, model, isGroupDeletion, model.StringDictionary);
    }

    public static BaseErrorModel GroupDetails(string commonDataTableName, List<KeyValuePair<string, object>> identifiers, int? page = 1,
      Dictionary<string, string> collections = null, bool isGroupDeletion = false) {
      BaseErrorModel errorModel = CommonActionFilter.OnActionExecuting(commonDataTableName, Aide.DH.CommonLogicName, Aibe.DH.GroupDetailsActionName, Aide.DH.GetRequest,
        id: null, identifiers: identifiers, collections: collections); //Can actually be post, but doesn't matter for index
      if (errorModel.HasError)
        return errorModel;
      errorModel.ReturnObject = GetFilterGroupDetailsModel(commonDataTableName, identifiers, page, collections,
        loadAllData: false, isGroupDeletion: isGroupDeletion);
      return errorModel;
    }

    public static BaseErrorModel DeleteGroupPost(string commonDataTableName, List<KeyValuePair<string, object>> identifiers) { //Where all common tables deletes are returned and can be deleted
      BaseErrorModel errorModel = CommonActionFilter.OnActionExecuting(commonDataTableName, Aide.DH.CommonLogicName, Aibe.DH.DeleteGroupActionName, Aide.DH.PostRequest,
        id: null, identifiers: identifiers, collections: null); //delete doesn't have collection
      if (errorModel.HasError)
        return errorModel;
      MetaInfo meta = AideTableHelper.GetMeta(commonDataTableName);
      LogicHelper.DeleteGroup(meta.TableSource, identifiers);
      return errorModel; //at this point it should have no error
    }

    #region table actions
    private static void commonExportToCsv(string tableName, string folderPath, string fileDownloadName, AideBaseFilterIndexModel model) {
      List<string> excludedColumns = model.GetExcludedColumnsInCsv(model.Meta.RawDataColumnNames);
      string csvString = model.BaseModel.GenerateCSVString(excludedColumns, Aide.PH.CsvDateTimeFormat);
      string fullRelativePath = tableName + "\\" + fileDownloadName;
      string path = System.IO.Path.Combine(folderPath, fullRelativePath);
      System.IO.Directory.CreateDirectory(folderPath + "\\" + tableName);
      System.IO.File.WriteAllText(path, csvString);
    }

    public static bool ExportToCsv(AideBaseFilterIndexModel model, string tableName, string folderPath) {
      try {
        commonExportToCsv(tableName, folderPath, tableName + ".csv", model);
        //commonExportToCsv(tableName, folderPath, tableName + ".csv", getFilterIndexModel(tableName, page, collections, loadAllData: false));
        return true;
      } catch (Exception ex) {
        string exStr = ex.ToString();
        LogHelper.Error(Identity.User?.UserName, null, Aide.DH.Aide, Aide.DH.CommonLogicName,
          tableName, Aibe.DH.ExportToCSVTableActionName, folderPath, exStr);
        return false;
      }
    }

    public static bool ExportAllToCsv(AideBaseFilterIndexModel model, string tableName, string folderPath) {
      try {
        commonExportToCsv(tableName, folderPath, tableName + "_All.csv", model);
        //commonExportToCsv(tableName, folderPath, tableName + "_All.csv", getFilterIndexModel(tableName, page, collections, loadAllData: true));
        return true;
      } catch (Exception ex) {
        string exStr = ex.ToString();
        LogHelper.Error(Identity.User?.UserName, null, Aide.DH.Aide, Aide.DH.CommonLogicName,
          tableName, Aibe.DH.ExportAllToCSVTableActionName, folderPath, exStr);
        return false;
      }
    }
    #endregion

    #region live items
    public static List<LiveDropDownResult> GetLiveDropDownItems(string commonDataTableName, string changedColumnName, List<LiveDropDownArg> args) {
      MetaInfo meta = AideTableHelper.GetMeta(commonDataTableName);
      List<LiveDropDownResult> results = meta.GetLiveDropDownResults(changedColumnName, args);
      return results;
    }

    public static List<ListColumnResult> GetLiveSubcolumns(string commonDataTableName, string changedColumnName, string changedColumnValue) {
      MetaInfo meta = AideTableHelper.GetMeta(commonDataTableName);
      List<ListColumnResult> results = meta.GetLiveListColumnResults(changedColumnName, changedColumnValue);
      return results;
    }
    #endregion
  }
}

//private ActionResult commonExportToCSV(string tableName, string fileDownloadName, AideFilterIndexModel model) {
//  List<string> excludedColumns = model.GetExcludedColumnsInCsv(model.Meta.RawDataColumnNames);
//  string csvString = model.FiModel.GenerateCSVString(true, true, excludedColumns, Aide.PH.CsvDateTimeFormat);
//  //byte[] buff = Encoding.ASCII.GetBytes(csvString);
//  //string mimeType = "text/csv";
//  //return File(buff, mimeType, fileDownloadName);
//}

//private ActionResult exportToCSV(string commonDataTableName, int? commonDataFilterPage, FormCollection collections) {
//  try {
//    return commonExportToCSV(commonDataTableName, commonDataTableName + ".csv",
//      getFilterIndexModel(commonDataTableName, commonDataFilterPage, collections, loadAllData: false));
//  } catch (Exception ex) {
//    string exStr = ex.ToString();
//    LogHelper.Error(User.Identity.Name, null, Aiwe.DH.Mvc, Aiwe.DH.MvcCommonControllerName,
//      commonDataTableName, "ExportToCSV", null, exStr);
//    return View(Aiwe.DH.ErrorViewName);
//  }
//}

//private ActionResult exportAllToCSV(string commonDataTableName, int? commonDataFilterPage, FormCollection collections) {
//  try {
//    return commonExportToCSV(commonDataTableName, commonDataTableName + "_All.csv",
//      getFilterIndexModel(commonDataTableName, commonDataFilterPage, collections, loadAllData: true));
//  } catch (Exception ex) {
//    string exStr = ex.ToString();
//    LogHelper.Error(User.Identity.Name, null, Aiwe.DH.Mvc, Aiwe.DH.MvcCommonControllerName,
//      commonDataTableName, "ExportAllToCSV", null, exStr);
//    return View(Aiwe.DH.ErrorViewName);
//  }
//}

//public static List<LiveDropDownResult> GetLiveDropDownItems(string tableName, string changedColumnName, string[] originalColumnValues,
//  string[] liveddColumnNames, string[] liveddDataTypes, string[] liveddItems) {
//foreach (var result in results)
//  result.ViewString = result.BuildDropdownString();
//foreach (var result in results)
//  result.ViewString = result.UsedListColumnInfo.GetHTML(result.DataValue);
//return Json(results, JsonRequestBehavior.AllowGet);

////Not used in Desktop app, only in website which uses javascript
//public static ListColumnResult GetSubcolumnItems(string tableName, string columnName, string dataValue, string lcType, int deleteNo, string addString) {
//  MetaInfo meta = AideTableHelper.GetMeta(tableName);
//  ListColumnResult result = new ListColumnResult(null, dataValue); //no need to have columnName here, only dataValue is needed
//  if (!result.AddOrDeleteDataValue(deleteNo, meta, columnName, addString, lcType))
//    return result; //if not successful, no need to take time and built HTML string      
//  ListColumnInfo info = meta.GetListColumnInfo(columnName); //Need to get the info from the columnName
//  //result.ViewString = info.GetHTML(result.DataValue); //if successful, do not forget to recreate the HTML string before return
//  return result; //Json(result, JsonRequestBehavior.AllowGet);
//}

////Not used in Desktop app, only in website which uses javascript
//public static ListColumnResult UpdateSubcolumnItemsDescription(string tableName, string columnName, int rowNo, int columnNo, string dataValue, string inputValue, string lcType) {
//  ListColumnResult result = new ListColumnResult(null, dataValue);
//  MetaInfo meta = AideTableHelper.GetMeta(tableName);
//  ListColumnInfo info = meta.GetListColumnInfo(columnName);
//  result.UpdateDataValue(info, inputValue, rowNo, columnNo, lcType); //the result does not matter here, just return it anyway      
//  return result; //Return the result, successful or not
//}