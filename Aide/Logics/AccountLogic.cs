using Aibe.Helpers;
using Aide.Models.Accounts;
using Extension.Database.SqlServer;
using Extension.Models;
using System;
using System.Collections.Generic;

namespace Aide.Logics {
  public class AccountLogic {
    public static BaseErrorModel Login(string identity, string password, UserAuthenticationType authType, bool isAnonymousAllowed) { //Tested, OK
      BaseErrorModel errorModel = new BaseErrorModel { };
      if (string.IsNullOrWhiteSpace(identity) && isAnonymousAllowed) { //Anonymous user allowed and it is empty
        LogHelper.Access(Aibe.DH.AnonymousRole, Aibe.LCZ.W_LogIn, Aibe.LCZ.W_Successful);
        errorModel.Code = 1; //to indicate successful but is anonymous
        return errorModel;
      }

      string identityLabel = string.Empty;
      switch (authType) {
        case UserAuthenticationType.Email: identityLabel = Aibe.LCZ.T_UserEmailColumnName; break;
        case UserAuthenticationType.Id: identityLabel = Aibe.LCZ.T_UserIdColumnName; break;
        case UserAuthenticationType.Name: identityLabel = Aibe.LCZ.T_UserNameColumnName; break;
      }

      if (string.IsNullOrWhiteSpace(identity)) {
        errorModel.Code = -1;
        errorModel.Message = string.Format(Aibe.LCZ.E_FieldIsRequired, identityLabel);
        return errorModel; //can retry
      }

      switch (authType) {
        case UserAuthenticationType.Email: errorModel = UserLogic.AuthenticateUserByEmail(identity, password); break;
        case UserAuthenticationType.Id: errorModel = UserLogic.AuthenticateUserById(identity, password); break;
        case UserAuthenticationType.Name: errorModel = UserLogic.AuthenticateUserByName(identity, password); break;
      }

      if (errorModel.HasError)
        return errorModel; //can retry
      
      switch (authType) {
        case UserAuthenticationType.Email: Identity.User = UserLogic.GetUserByEmail(identity); break;
        case UserAuthenticationType.Id: Identity.User = UserLogic.GetUserById(identity); break;
        case UserAuthenticationType.Name: Identity.User = UserLogic.GetUserByName(identity); break;
      }

      if (Identity.User != null && !string.IsNullOrWhiteSpace(Identity.User.Id))
        Identity.Roles = UserLogic.GetRoles(Identity.User.Id);

      //Update the last login time
      DateTime now = DateTime.Now;
      int result = SQLServerHandler.Update(Aibe.DH.UserDBConnectionString, Aide.PH.UserTableName, new Dictionary<string, object> {
        { Aibe.DH.UserLastLoginColumnName, now } }, Aibe.DH.UserIdColumnName, Identity.User.Id);
      if (!Identity.IsDeveloper()) //log if not developer
        LogHelper.Access(Identity.User.UserName + " (" + Identity.User.Email + ")", Aibe.LCZ.W_LogIn, result.ToString());

      return errorModel;
    }

    public static BaseErrorModel LogOff() {
      if (Identity.User != null && !Identity.IsDeveloper())
        LogHelper.Access(Identity.User.UserName + " (" + Identity.User.Email + ")", Aibe.LCZ.W_LogOff);
      Identity.User = null;
      return new BaseErrorModel();
    }
  }

  public enum UserAuthenticationType {
    Email,
    Id,
    Name,
  }
}

//BaseErrorModel errorModel = new BaseErrorModel();
//if (string.IsNullOrWhiteSpace(identity)) {
//  errorModel.Code = -1;
//  errorModel.Message = Aibe.LCZ.NFE_UserIdentityMustBeFilled;
//  return errorModel;
//}
//switch (authType) {
//  case UserAuthenticationType.Email: errorModel = UserLogic.AuthenticateUserByEmail(identity, password); break;
//  case UserAuthenticationType.Id: errorModel = UserLogic.AuthenticateUserById(identity, password); break;
//  case UserAuthenticationType.Name: errorModel = UserLogic.AuthenticateUserByName(identity, password); break;
//}
//return errorModel;

