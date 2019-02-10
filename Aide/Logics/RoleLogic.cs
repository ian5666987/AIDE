using Aibe.Models;
using Aide.Models.Accounts;
using Extension.Database.SqlServer;
using Extension.Models;
using Extension.String;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Aide.Logics {
  public class RoleLogic {
    public const int ItemsPerPage = 20;
    public static DataTable Index(string filterText, ref NavDataModel navData) {
      var adminRoles = Aibe.DH.AdminRoles.Select(x => x.AsSqlStringValue()).ToList();
      string baseWhereClause = string.Concat("([", Aibe.DH.RoleNameColumnName, "] != ", string.Join(string.Concat(" AND [", Aibe.DH.RoleNameColumnName, "] != "), adminRoles), ")");
      string filterWhereClause = string.IsNullOrWhiteSpace(filterText) ? string.Empty :
        string.Concat(" AND ([", Aibe.DH.RoleNameColumnName, "] LIKE ", ("%" + filterText + "%").AsSqlStringValue(), ")");
      DataTable table = SQLServerHandler.GetFullDataTableWhere(Aibe.DH.UserDBConnectionString, Aide.PH.RoleTableName,
        baseWhereClause + filterWhereClause, Aibe.DH.RoleNameColumnName);
      if (navData == null) //the first time
        navData = new NavDataModel(1, ItemsPerPage, table.Rows.Count);
      else {
        navData.UpdateModel(navData.CurrentPage, ItemsPerPage, table.Rows.Count);
      }
      return table;
    }

    public static BaseErrorModel Create(string text) {
      if (string.IsNullOrWhiteSpace(text))
        return new BaseErrorModel { Code = -1, Message = Aibe.LCZ.NFE_InputCannotBeEmpty };
      RoleModel roleModel = new RoleModel { Name = text };
      roleModel.Id = Guid.NewGuid().ToString();
      SQLServerHandler.InsertObject(Aibe.DH.UserDBConnectionString, Aide.PH.RoleTableName, roleModel);
      return new BaseErrorModel();
    }

    public static BaseErrorModel Edit(string id, string text) {
      if (string.IsNullOrWhiteSpace(text))
        return new BaseErrorModel { Code = -1, Message = Aibe.LCZ.NFE_InputCannotBeEmpty };
      RoleModel roleModel = new RoleModel { Name = text };
      roleModel.Id = id;
      SQLServerHandler.UpdateObject(Aibe.DH.UserDBConnectionString, Aide.PH.RoleTableName, roleModel, Aibe.DH.RoleIdColumnName);
      return new BaseErrorModel();
    }

    public static BaseErrorModel Delete(string id) {
      if (string.IsNullOrWhiteSpace(id))
        return new BaseErrorModel { Code = -1, Message = Aibe.LCZ.NFE_IdNotFound };
      SQLServerHandler.DeleteFromTableWhere(Aibe.DH.UserDBConnectionString, Aide.PH.RoleTableName, string.Concat(
        Aibe.DH.RoleIdColumnName, "=", id.AsSqlStringValue()));
      return new BaseErrorModel();
    }
    
    public static List<string> Roles() { //Probably Aide specific
      List<string> roles = new List<string>();
      List<object> results = SQLServerHandler.GetSingleColumn(Aibe.DH.UserDBConnectionString, Aide.PH.RoleTableName, Aibe.DH.RoleNameColumnName);
      if (results == null || results.Count <= 0)
        return roles;
      return results.Select(x => x.ToString()).ToList();
    }
  }
}
