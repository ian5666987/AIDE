using Aibe.Helpers;
using Aibe.Models;
using Aibe.Models.Core;
using Aide.Extensions;
using Aide.Helpers;
using Aide.Logics;
using Aide.Models.Accounts;
using Extension.Models;
using Extension.String;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aide.ActionFilters {
  public class CommonActionFilter {
    public static BaseErrorModel OnActionExecuting(string tableName, string logicName, string actionName, string method, object id, 
      List<KeyValuePair<string, object>> identifiers, Dictionary<string, string> collections) {
      BaseErrorModel errorModel = new BaseErrorModel();
      ApplicationUser user = Identity.User;

      string userName = string.IsNullOrWhiteSpace(user?.UserName) ? string.Empty : user.UserName;

      if (!Identity.IsDeveloper() && //the developer does not need log
        !Aibe.DH.NonRecordedActions.Any(x => x.EqualsIgnoreCase(actionName)) && //the action must not be excluded for record
        method != null &&
        method.EqualsIgnoreCase(Aide.DH.PostRequest) && //only post is recorded
          (string.IsNullOrWhiteSpace(tableName) ||
          !Aibe.DH.CoreTableNames.Contains(tableName))) { //the table name is either null or it is not excluded

        StringBuilder sb = new StringBuilder();

        if (actionName.EqualsIgnoreCase(Aibe.DH.DeleteActionName))
          sb.AppendLine("Id: " + id?.ToString());
        if (identifiers != null && identifiers.Count > 0)
          foreach(var identifier in identifiers)
            sb.AppendLine("Identifier [" + identifier.Key + "]: " + identifier.Value?.ToString());        
        if (collections != null &&
          (actionName.EqualsIgnoreCase(Aibe.DH.CreateActionName) ||
          actionName.EqualsIgnoreCase(Aibe.DH.EditActionName))) {
          if (collections != null) {
            int index = 0;
            foreach (var key in collections.Keys) {
              if (index > 0)
                sb.Append(Environment.NewLine);
              sb.Append(string.Concat(key, ": ", collections[key]));
              ++index;
            }
          }
        }

        LogHelper.Action(userName, Aide.DH.Aide, logicName, tableName, actionName, sb.ToString());
      }
      
      if (string.IsNullOrWhiteSpace(tableName)) {
        errorModel.Code = -2;
        errorModel.Message = Aibe.LCZ.NFM_TableDescriptionNotFound + Environment.NewLine + Aibe.LCZ.NFM_TableDescriptionNotFoundMessage;
        return errorModel;
      }

      MetaInfo meta = AideTableHelper.GetMeta(tableName);

      if (meta == null) {
        errorModel.Code = -3;
        errorModel.Message = Aibe.LCZ.NFM_TableDescriptionNotFound + Environment.NewLine + Aibe.LCZ.NFM_TableDescriptionNotFoundMessage;
        return errorModel;
      }

      //Only developer is allowed to do anything on MetaItem page
      if (tableName.EqualsIgnoreCase(Aibe.DH.MetaTableName) && !Identity.IsDeveloper()) {
        errorModel.Code = -3;
        errorModel.Message = Aibe.LCZ.NFM_InsufficientAccessRight + Environment.NewLine + Aibe.LCZ.NFM_InsufficientAccessRightPageMessage;
        return errorModel;
      }

      if (Identity.IsMainAdmin()) //main admins are immune to exclusion
        return errorModel;

      //Access checking (checked)
      if (meta.AccessExclusions != null) { //there is access exclusion and user role is supposed to be excluded
        if (meta.AccessExclusions.Contains(Aibe.DH.AnonymousRole) && user == null) { //unauthorized is excluded here and user is not authenticated
          errorModel.Code = -1;
          errorModel.Message = Aibe.LCZ.NFM_InsufficientAccessRight + Environment.NewLine + Aibe.LCZ.NFM_InsufficientAccessRightPageMessage;
          return errorModel;
        }
        //TODO this User.IsInRole is case insensitive, unfortunately
        if (meta.AccessExclusions.Any(x => UserLogic.IsInRole(Identity.User, x, Identity.Roles))) { //user is in any of the exclusion authorization
          errorModel.Code = -2;
          errorModel.Message = Aibe.LCZ.NFM_InsufficientAccessRight + Environment.NewLine + Aibe.LCZ.NFM_InsufficientAccessRightPageMessage;
          return errorModel;
        }
      }

      //Action and table action checking
      if (Aide.DH.OnlyAccessCheckingActions.Any(x => x.EqualsIgnoreCase(actionName))) //Index action only needs as far as access checking
        return errorModel;

      //Action checking (checked)
      if (meta.Actions != null && meta.Actions.Any(x => x.Name.EqualsIgnoreCase(actionName))) {
        ActionInfo action = meta.Actions.FirstOrDefault(x => x.Name.EqualsIgnoreCase(actionName));
        if (action.IsAllowed(user)) //if user is found or is not defined
          return errorModel; //then returns
        errorModel.Code = -1;
        errorModel.Message = Aibe.LCZ.NFM_InsufficientAccessRight + Environment.NewLine + Aibe.LCZ.NFM_InsufficientAccessRightActionMessage; //action is registered but role is not found
        return errorModel;
      } //below onwards means actionNotFound

      //Table action checking (checked)
      if (meta.TableActions == null || meta.TableActions.Count <= 0) { //table action is null and action is not found
        errorModel.Code = -1;
        errorModel.Message = Aibe.LCZ.NFM_ActionNotFound + Environment.NewLine + Aibe.LCZ.NFM_ActionNotFoundMessage; //action is registered but role is not found
        return errorModel;
      }

      if (!meta.TableActions.Any(x => x.Name.EqualsIgnoreCase(actionName))) { //action not found in the table list either
        errorModel.Code = -2;
        errorModel.Message = Aibe.LCZ.NFM_ActionNotFound + Environment.NewLine + Aibe.LCZ.NFM_ActionNotFoundMessage; //action is registered but role is not found
        return errorModel;
      } //below means found in the table action

      ActionInfo tableActionInfo = meta.TableActions.FirstOrDefault(x => x.Name.EqualsIgnoreCase(actionName));
      if (!tableActionInfo.IsAllowed(user)) {
        errorModel.Code = -2;
        errorModel.Message = Aibe.LCZ.NFM_InsufficientAccessRight + Environment.NewLine + Aibe.LCZ.NFM_InsufficientAccessRightActionMessage; //action is registered but role is not found
      }

      return errorModel;
    }
  }
}
