using Aibe.Helpers;
using Aide.Logics;
using Aide.Models.Accounts;
using Extension.Database.SqlServer;
using Extension.String;
using System;
using System.Collections.Generic;
using System.Data;

namespace Aide.Migrations {
  public class Configuration {
    public static void Seed() { //Just to start everything by default
      List<string> roles = new List<string> {
        Aibe.DH.DevRole,
        Aibe.DH.MainAdminRole,
        Aibe.DH.AdminRole,
        Aibe.DH.UserRole,
      };

      foreach (var role in roles) { //if roles do not exist, add the roles
        DataTable table = SQLServerHandler.GetFullDataTableWhere(Aibe.DH.UserDBConnectionString, Aide.PH.RoleTableName,
          string.Concat(Aibe.DH.RoleNameColumnName, "=", role.AsSqlStringValue()));
        if (table != null && table.Rows.Count > 0) //exist
          continue;
        RoleModel rModel = new RoleModel { Id = Guid.NewGuid().ToString(), Name = role };
        SQLServerHandler.InsertObject(Aibe.DH.UserDBConnectionString, Aide.PH.RoleTableName, rModel); //assuming successful
      }

      //Check if the user exist
      bool adminExist = UserLogic.GetUserByEmail(Aide.DH.MainAdminEmail) != null;
      bool devExist = UserLogic.GetUserByEmail(Aide.DH.DevEmail) != null;
      if (adminExist && devExist)
        return;
      DateTime now = DateTime.Now;
      if (!devExist) {
        ApplicationUser ian = new ApplicationUser {
          FullName = Aibe.DH.DevFullName,
          UserName = Aibe.DH.DevName,
          DisplayName = Aibe.DH.DevDisplayName,
          Email = Aide.DH.DevEmail,
          AdminRole = Aibe.DH.DevRole,
          Team = Aide.LCZ.W_HomeTeam,
          RegistrationDate = now,
          LastLogin = now,
        };
        UserLogic.CreateUserWithRoles(ian, Aibe.DH.DevPass);
        UserHelper.CreateUserMap(ian.UserName, Aibe.DH.DevPass);
      }

      if (!adminExist) {
        ApplicationUser admin = new ApplicationUser {
          FullName = Aibe.DH.MainAdminFullName,
          UserName = Aide.DH.MainAdminName,
          DisplayName = Aibe.DH.MainAdminDisplayName,
          Email = Aide.DH.MainAdminEmail,
          AdminRole = Aibe.DH.MainAdminRole,
          Team = Aide.LCZ.W_HomeTeam,
          RegistrationDate = now,
          LastLogin = now,
        };
        UserLogic.CreateUserWithRoles(admin, Aide.DH.MainAdminPass);
        UserHelper.CreateUserMap(admin.UserName, Aide.DH.MainAdminPass);
      }
    }
  }
}
