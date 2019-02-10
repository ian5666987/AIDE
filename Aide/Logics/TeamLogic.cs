using Aibe.Models;
using Aide.Models.Accounts;
using Extension.Database.SqlServer;
using Extension.Models;
using Extension.String;
using System.Data;

namespace Aide.Logics {
  public class TeamLogic {
    public const int ItemsPerPage = 20;
    public static DataTable Index(string filterText, ref NavDataModel navData) { //get DataTable and update the navData
      DataTable table = string.IsNullOrWhiteSpace(filterText) ?
        SQLServerHandler.GetFullDataTable(Aibe.DH.UserDBConnectionString, Aide.PH.TeamTableName, Aibe.DH.TeamNameColumnName) :
        SQLServerHandler.GetFullDataTableWhere(Aibe.DH.UserDBConnectionString, Aide.PH.TeamTableName, string.Concat(
          Aibe.DH.TeamNameColumnName, " LIKE ", ("%" + filterText + "%").AsSqlStringValue()), Aibe.DH.TeamNameColumnName);        
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
      TeamModel teamModel = new TeamModel { Name = text };
      int count = SQLServerHandler.GetCount(Aibe.DH.UserDBConnectionString, Aide.PH.TeamTableName); //check if there is an item in the table in the first place
      teamModel.Id = count <= 0 ? 1 : ((int)SQLServerHandler.GetAggregatedValue(Aibe.DH.UserDBConnectionString, Aide.PH.TeamTableName, Aibe.DH.TeamIdColumnName, "MAX") + 1);
      SQLServerHandler.InsertObject(Aibe.DH.UserDBConnectionString, Aide.PH.TeamTableName, teamModel);
      return new BaseErrorModel();
    }

    public static BaseErrorModel Edit(int id, string text) {
      if (string.IsNullOrWhiteSpace(text))
        return new BaseErrorModel { Code = -1, Message = Aibe.LCZ.NFE_InputCannotBeEmpty };
      TeamModel teamModel = new TeamModel { Name = text };
      teamModel.Id = id;
      SQLServerHandler.UpdateObject(Aibe.DH.UserDBConnectionString, Aide.PH.TeamTableName, teamModel, Aibe.DH.TeamIdColumnName);
      return new BaseErrorModel();
    }

    public static BaseErrorModel Delete(int id) { //always su
      if (id <= 0)
        return new BaseErrorModel { Code = -1, Message = Aibe.LCZ.NFE_IdNotFound };
      SQLServerHandler.DeleteFromTableWhere(Aibe.DH.UserDBConnectionString, Aide.PH.TeamTableName, string.Concat(
        Aibe.DH.TeamIdColumnName, "=", id));
      return new BaseErrorModel();
    }
  }
}
