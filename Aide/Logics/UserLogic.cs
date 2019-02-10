using Aibe.Helpers;
using Aibe.Models;
using Aide.Models;
using Aide.Models.Accounts;
using Extension.Cryptography;
using Extension.Database.SqlServer;
using Extension.Extractor;
using Extension.Models;
using Extension.String;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Aide.Logics {
  public class UserLogic {
    //Base functions
    public static ApplicationUser GetUserById(string id) {
      DataTable table = SQLServerHandler.GetFullDataTableWhere(Aibe.DH.UserDBConnectionString, Aide.PH.UserTableName,
        string.Concat(Aibe.DH.UserIdColumnName, "=", id.AsSqlStringValue()));
      if (table == null || table.Rows.Count <= 0)
        return null;
      ApplicationUser user = BaseExtractor.Extract<ApplicationUser>(table);
      return user;
    }

    public static ApplicationUser GetUserByName(string userName) {
      DataTable table = SQLServerHandler.GetFullDataTableWhere(Aibe.DH.UserDBConnectionString, Aide.PH.UserTableName,
        string.Concat(Aibe.DH.UserNameColumnName, "=", userName.AsSqlStringValue()));
      if (table == null || table.Rows.Count <= 0)
        return null;
      ApplicationUser user = BaseExtractor.Extract<ApplicationUser>(table);
      return user;
    }

    public static ApplicationUser GetUserByEmail(string email) {
      DataTable table = SQLServerHandler.GetFullDataTableWhere(Aibe.DH.UserDBConnectionString, Aide.PH.UserTableName,
        string.Concat(Aibe.DH.UserEmailColumnName, "=", email.AsSqlStringValue()));
      if (table == null || table.Rows.Count <= 0)
        return null;
      ApplicationUser user = BaseExtractor.Extract<ApplicationUser>(table);
      return user;
    }

    public static BaseErrorModel CreateUserWithRoles(ApplicationUser user, string password) {
      BaseErrorModel errorModel = CreateUser(user, password);
      if (errorModel.HasError)
        return errorModel;

      if (!string.IsNullOrWhiteSpace(user.WorkingRole)) { //if this is not empty, then add the user to this role
        errorModel = AddToRole(user.Id, user.WorkingRole);
        if (errorModel.HasError)
          return errorModel;
      }

      if (!string.IsNullOrWhiteSpace(user.AdminRole))
        errorModel = AddToRole(user.Id, user.AdminRole);

      return errorModel;
    }

    public static BaseErrorModel CreateUser(ApplicationUser user, string password) {
      BaseErrorModel errorModel = new BaseErrorModel { };
      Guid guid = Guid.NewGuid();
      string id = guid.ToString();
      user.Id = id;
      user.RegistrationDate = DateTime.Now;
      string passwordHash = string.IsNullOrEmpty(password) ? null : Cryptography.Encrypt(password + id); //password + id => Encrypt
      user.PasswordHash = passwordHash;
      object result = SQLServerHandler.InsertObject<ApplicationUser>(Aibe.DH.UserDBConnectionString,
        Aide.PH.UserTableName, user, excludedPropertyNames: null); //can be empty but successful, do not need to check for now
      return errorModel;
    }

    public static BaseErrorModel EditUserAndItsRoles(ApplicationUser user) {
      BaseErrorModel errorModel = new BaseErrorModel { };

      DataTable table = SQLServerHandler.GetFullDataTableWhere(Aibe.DH.UserDBConnectionString, Aide.PH.UserTableName,
        string.Concat(Aibe.DH.UserIdColumnName, "=", user.Id.AsSqlStringValue()));
      ApplicationUser oldDataUser = BaseExtractor.Extract<ApplicationUser>(table);
      user.RegistrationDate = oldDataUser.RegistrationDate; //the registration date should not be changed upon edit
      user.LastLogin = oldDataUser.LastLogin; //the last login should also not be changed upon edit

      bool bothWorkingRolesAreEmpty = string.IsNullOrEmpty(user.WorkingRole) && string.IsNullOrEmpty(oldDataUser.WorkingRole);
      bool bothAdminRolesAreEmpty = string.IsNullOrEmpty(user.AdminRole) && string.IsNullOrEmpty(oldDataUser.AdminRole);
      if(!bothWorkingRolesAreEmpty && user.WorkingRole != oldDataUser.WorkingRole) { //changes happen on working role
        bool hasOldRole = !string.IsNullOrEmpty(oldDataUser.WorkingRole);
        if (hasOldRole) {
          errorModel = RemoveFromRole(oldDataUser.Id, oldDataUser.WorkingRole);
          if (errorModel.HasError)
            return errorModel;
        }
        bool hasNewRole = !string.IsNullOrEmpty(user.WorkingRole);
        if (hasNewRole) {
          errorModel = AddToRole(user.Id, user.WorkingRole);
          if (errorModel.HasError)
            return errorModel;
        }
      }

      if(!bothAdminRolesAreEmpty && user.AdminRole != oldDataUser.AdminRole) { //changes happen on admin role
        bool hasOldRole = !string.IsNullOrWhiteSpace(oldDataUser.AdminRole);
        if (hasOldRole) {
          errorModel = RemoveFromRole(oldDataUser.Id, oldDataUser.AdminRole);
          if (errorModel.HasError)
            return errorModel;
        }
        bool hasNewRole = !string.IsNullOrEmpty(user.AdminRole);
        if (hasNewRole) {
          errorModel = AddToRole(user.Id, user.AdminRole);
          if (errorModel.HasError)
            return errorModel;
        }
      }

      //Must be the last one
      SQLServerHandler.UpdateObject(Aibe.DH.UserDBConnectionString, Aide.PH.UserTableName, user, Aibe.DH.UserIdColumnName); //TODO currently the result is unchecked

      return errorModel;
    }

    public static BaseErrorModel DeleteUserAndItsRoles(string id) {
      BaseErrorModel errorModel = new BaseErrorModel { };

      DataTable table = SQLServerHandler.GetFullDataTableWhere(Aibe.DH.UserDBConnectionString, Aide.PH.UserTableName,
        string.Concat(Aibe.DH.UserIdColumnName, "=", id.AsSqlStringValue()));
      ApplicationUser oldDataUser = BaseExtractor.Extract<ApplicationUser>(table);

      bool hasWorkingRole = !string.IsNullOrWhiteSpace(oldDataUser.WorkingRole);
      bool hasAdminRole = !string.IsNullOrWhiteSpace(oldDataUser.AdminRole);

      if (hasWorkingRole) {
        errorModel = RemoveFromRole(id, oldDataUser.WorkingRole);
        if (errorModel.HasError)
          return errorModel;
      }

      if (hasAdminRole) {
        errorModel = RemoveFromRole(id, oldDataUser.AdminRole);
        if (errorModel.HasError)
          return errorModel;
      }

      //Must be the last one
      int val = SQLServerHandler.DeleteFromTableWhere(Aibe.DH.UserDBConnectionString, Aide.PH.UserTableName, string.Concat(
        Aibe.DH.UserIdColumnName, "=", id)); //actually, does not matter if val = 0, so leave it alone

      return errorModel;
    }

    private static BaseErrorModel authenticateUser(ApplicationUser user, string password) {
      BaseErrorModel errorModel = new BaseErrorModel { };
      if (user == null) {//cannot be found
        errorModel.Code = -1;
        errorModel.Message = Aibe.LCZ.NFE_UserNotFound;
        return errorModel;
      }
      string passwordHash = string.IsNullOrEmpty(password) ? null : Cryptography.Encrypt(password + user.Id);
      bool bothEmpty = string.IsNullOrEmpty(password) && string.IsNullOrEmpty(user.PasswordHash);
      if (user.PasswordHash != passwordHash) {
        errorModel.Code = -2;
        errorModel.Message = Aibe.LCZ.NFE_UserCannotBeAuthenticated;
      }
      return errorModel;
    }

    public static BaseErrorModel AuthenticateUserByName(string userName, string password) {
      ApplicationUser user = GetUserByName(userName);
      return authenticateUser(user, password);
    }

    public static BaseErrorModel AuthenticateUserById(string id, string password) {
      ApplicationUser user = GetUserById(id);
      return authenticateUser(user, password);
    }

    public static BaseErrorModel AuthenticateUserByEmail(string email, string password) {
      ApplicationUser user = GetUserByEmail(email);
      return authenticateUser(user, password);
    }

    public static List<RoleModel> GetRoleModels() {
      DataTable table = SQLServerHandler.GetFullDataTable(Aibe.DH.UserDBConnectionString, Aide.PH.RoleTableName);
      return BaseExtractor.ExtractList<RoleModel>(table);
    }

    public static BaseErrorModel AddToRole(string userId, string role) {
      BaseErrorModel errorModel = new BaseErrorModel { };
      ApplicationUser user = GetUserById(userId);
      if (user == null) {
        errorModel.Code = -1;
        errorModel.Message = Aibe.LCZ.NFE_UserNotFound;
        return errorModel;
      }
      List<RoleModel> roles = GetRoleModels();
      RoleModel roleModel = roles.FirstOrDefault(x => x.Name == role);
      if (roleModel == null) {
        errorModel.Code = -2;
        errorModel.Message = Aibe.LCZ.NFE_RoleNotFound;
        return errorModel;
      }
      UserRoleModel urModel = new UserRoleModel {
        UserId = user.Id,
        RoleId = roleModel.Id,
      };
      object result = SQLServerHandler.InsertObject(Aibe.DH.UserDBConnectionString,
        Aide.PH.UserRoleTableName, urModel);
      if (result != null) {
        errorModel.Code = -3;
        errorModel.Message = Aibe.LCZ.NFE_RoleAdditionFailed;
      }
      return errorModel;
    }

    public static BaseErrorModel ChangePassword(string userId, string oldPassword, string newPassword) {
      BaseErrorModel errorModel = new BaseErrorModel { };
      ApplicationUser user = GetUserById(userId);
      if (user == null) {
        errorModel.Code = -1;
        errorModel.Message = Aibe.LCZ.NFE_UserNotFound;
        return errorModel;
      }
      string oldPasswordHash = string.IsNullOrEmpty(oldPassword) ? null : Cryptography.Encrypt(oldPassword + userId);
      bool bothEmpty = string.IsNullOrEmpty(oldPassword) && string.IsNullOrEmpty(user.PasswordHash);
      if (!bothEmpty && oldPasswordHash != user.PasswordHash) { //if not both empty, the password in DB and the old password must match to be allowed to pass
        errorModel.Code = -2;
        errorModel.Message = Aibe.LCZ.NFE_PasswordDoesNotMatch;
        return errorModel;
      }
      string newPasswordHash = string.IsNullOrEmpty(newPassword) ? null : Cryptography.Encrypt(newPassword + userId);
      user.PasswordHash = newPasswordHash;
      SQLServerHandler.UpdateObject(Aibe.DH.UserDBConnectionString, Aide.PH.UserTableName, user, Aibe.DH.UserIdColumnName);
      return errorModel;
    }

    public static BaseErrorModel SetPassword(string userId, string password) {
      BaseErrorModel errorModel = new BaseErrorModel { };
      ApplicationUser user = GetUserById(userId);
      if (user == null) {
        errorModel.Code = -1;
        errorModel.Message = Aibe.LCZ.NFE_UserNotFound;
        return errorModel;
      }
      string passwordHash = string.IsNullOrEmpty(password) ? null : Cryptography.Encrypt(password + userId);
      user.PasswordHash = passwordHash;
      SQLServerHandler.UpdateObject(Aibe.DH.UserDBConnectionString, Aide.PH.UserTableName, user, Aibe.DH.UserIdColumnName);
      return errorModel;
    }

    public static BaseErrorModel RemoveFromRole(string userId, string role) {
      BaseErrorModel errorModel = new BaseErrorModel { };
      ApplicationUser user = GetUserById(userId);
      if (user == null) {
        errorModel.Code = -1;
        errorModel.Message = Aibe.LCZ.NFE_UserNotFound;
        return errorModel;
      }
      DataTable table = SQLServerHandler.GetFullDataTableWhere(Aibe.DH.UserDBConnectionString, Aide.PH.RoleTableName, 
        string.Concat(Aibe.DH.RoleNameColumnName, "=", role.AsSqlStringValue()));
      RoleModel roleModel = BaseExtractor.Extract<RoleModel>(table);
      int result = SQLServerHandler.DeleteFromTableWhere(Aibe.DH.UserDBConnectionString, Aide.PH.UserRoleTableName,
        string.Concat(Aibe.DH.UserRoleRoleColumnName, "=", roleModel.Id.AsSqlStringValue(), " AND ", Aibe.DH.UserRoleUserColumnName, "=", userId.AsSqlStringValue()));
      if (result <= 0) {
        errorModel.Code = -2;
        errorModel.Message = Aibe.LCZ.NFE_RoleRemovalFailed;
      }
      return errorModel;
    }

    private bool HasPassword(string userId) {
      ApplicationUser user = GetUserById(userId);
      if (user != null)
        return !string.IsNullOrEmpty(user.PasswordHash);
      return false;
    }
    
    //Aiwe parallel functions
    public const int ItemsPerPage = 20;
    public static DataTable Index(ref AideUserFilterIndexModel model) { //update model and return the data table
      model.FilterNo = model.Filter.CreateMessage(out model.FilterText);
      string baseWhereClause = string.Concat("([", Aibe.DH.UserAdminRoleColumnName, "] != ", Aibe.DH.DevRole.AsSqlStringValue(), 
        " OR [", Aibe.DH.UserAdminRoleColumnName, "] IS NULL)"); //to filter out the developer role
      DataTable table = model.Filter.HasFilter() ?
        SQLServerHandler.GetFullDataTableFilterBy(Aibe.DH.UserDBConnectionString, Aide.PH.UserTableName, model.Filter, useNull: false,
        addWhereClause: baseWhereClause, orderByClause: Aibe.DH.UserNameColumnName) :
        SQLServerHandler.GetFullDataTableWhere(Aibe.DH.UserDBConnectionString, Aide.PH.UserTableName, baseWhereClause, Aibe.DH.UserNameColumnName);
      if (model.NavData == null) //the first time
        model.NavData = new NavDataModel(1, ItemsPerPage, table.Rows.Count);
      else {
        model.NavData.UpdateModel(model.NavData.CurrentPage, ItemsPerPage, table.Rows.Count);
      }
      return table;
    }

    public static BaseErrorModel Check(ApplicationUser user) {
      StringBuilder sb = new StringBuilder();
      if (string.IsNullOrWhiteSpace(user.UserName))
        sb.AppendLine(string.Format(Aibe.LCZ.E_FieldIsRequired, Aibe.LCZ.T_UserNameColumnName));
      if (string.IsNullOrWhiteSpace(user.FullName))
        sb.AppendLine(string.Format(Aibe.LCZ.E_FieldIsRequired, Aibe.LCZ.T_UserFullNameColumnName));
      if (string.IsNullOrWhiteSpace(user.DisplayName))
        sb.AppendLine(string.Format(Aibe.LCZ.E_FieldIsRequired, Aibe.LCZ.T_UserDisplayNameColumnName));
      if (string.IsNullOrWhiteSpace(user.Email))
        sb.AppendLine(string.Format(Aibe.LCZ.E_FieldIsRequired, Aibe.LCZ.T_UserEmailColumnName));
      if (string.IsNullOrWhiteSpace(user.Team))
        sb.AppendLine(string.Format(Aibe.LCZ.E_FieldIsRequired, Aibe.LCZ.T_UserTeamColumnName));

      string wRole = user.WorkingRole;
      string aRole = user.AdminRole;
      if (string.IsNullOrWhiteSpace(wRole) && string.IsNullOrWhiteSpace(aRole))
        sb.AppendLine(string.Format(Aibe.LCZ.E_FieldsCannotBeBothEmpty, Aibe.LCZ.T_UserWorkingRoleColumnName, Aibe.LCZ.T_UserAdminRoleColumnName));

      if (!string.IsNullOrWhiteSpace(user.UserName)) {
        int userNameCount = SQLServerHandler.GetCountWhere(Aibe.DH.UserDBConnectionString, Aide.PH.UserTableName,
          string.Concat(Aibe.DH.UserNameColumnName, "=", user.UserName.AsSqlStringValue(), " AND ",
          Aibe.DH.UserIdColumnName, "!=", user.Id.AsSqlStringValue())); //same user name but on different id
        if (userNameCount > 0)
          sb.AppendLine(string.Format(Aibe.LCZ.E_AlreadyExist, Aibe.LCZ.W_UserName));
      }
      if (!string.IsNullOrWhiteSpace(user.Email)) {
        int emailCount = SQLServerHandler.GetCountWhere(Aibe.DH.UserDBConnectionString, Aide.PH.UserTableName,
          string.Concat(Aibe.DH.UserEmailColumnName, "=", user.Email.AsSqlStringValue(), " AND ",
          Aibe.DH.UserIdColumnName, "!=", user.Id.AsSqlStringValue())); //same email but on different id
        if (emailCount > 0)
          sb.AppendLine(string.Format(Aibe.LCZ.E_AlreadyExist, Aibe.LCZ.W_Email));
      }

      string errorMessage = sb.ToString();
      bool hasError = !string.IsNullOrWhiteSpace(errorMessage);
      return new BaseErrorModel { Code = hasError ? -1 : 0, Message = errorMessage };
    }

    public static BaseErrorModel Create(ApplicationUser user) {
      BaseErrorModel errorModel = Check(user);
      if (errorModel.HasError)
        return errorModel;
      errorModel = CreateUserWithRoles(user, null);
      if (errorModel.HasError)
        return errorModel;
      UserHelper.CreateUserMap(user.UserName, null);
      return new BaseErrorModel();
    }

    public static BaseErrorModel Edit(ApplicationUser user, string originalUserName) {
      BaseErrorModel errorModel = Check(user);
      if (errorModel.HasError)
        return errorModel;
      errorModel = EditUserAndItsRoles(user); //there is no change on the user map here, just leave it be
      if (errorModel.HasError)
        return errorModel;
      bool bothUserNamesAreEmpty = string.IsNullOrWhiteSpace(originalUserName) && string.IsNullOrWhiteSpace(user.UserName);
      if (!bothUserNamesAreEmpty && originalUserName != user.UserName) //changes of the user name happen
        UserHelper.EditUserMapName(originalUserName, user.UserName);
      return errorModel;
    }

    public static BaseErrorModel Delete(string id, string userName) {
      BaseErrorModel errorModel = DeleteUserAndItsRoles(id);
      if (errorModel.HasError)
        return errorModel;
      if (!string.IsNullOrWhiteSpace(userName))
        UserHelper.DeleteUserMap(userName);
      return errorModel;
    }

    //To check its roles
    public static List<string> GetRoles(string userId) {
      List<string> roleIds = SQLServerHandler.GetSingleColumnWhere(Aibe.DH.UserDBConnectionString, Aide.PH.UserRoleTableName,
        Aibe.DH.UserRoleRoleColumnName, string.Concat(Aibe.DH.UserRoleUserColumnName, "=", userId.AsSqlStringValue()))
        .Select(x => x.ToString()).ToList();
      DataTable table = SQLServerHandler.GetFullDataTable(Aibe.DH.UserDBConnectionString, Aide.PH.RoleTableName);
      Dictionary<string, string> roleMaps = new Dictionary<string, string>();
      foreach(DataRow row in table.Rows) {
        string id = (string)row[Aibe.DH.RoleIdColumnName];
        string role = (string)row[Aibe.DH.RoleNameColumnName];
        roleMaps.Add(id, role);
      }
      List<string> roles = new List<string>();
      foreach(var roleId in roleIds)
        if (roleMaps.ContainsKey(roleId))
          roles.Add(roleMaps[roleId]);
      return roles;
    }

    public static bool IsInRole(string userId, string role, List<string> givenRoles) {
      if (string.IsNullOrWhiteSpace(userId))
        return false;
      if (givenRoles != null)
        return givenRoles.Any(x => x.Equals(role));
      var obtainedRoles = GetRoles(userId);
      return obtainedRoles.Any(x => x.Equals(role));
    }

    public static bool IsInRole(ApplicationUser user, string role, List<string> givenRoles) {
      if (user == null)
        return false;
      return IsInRole(user.Id, role, givenRoles);
    }
  }
}
