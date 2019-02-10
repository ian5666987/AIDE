using Aide.Logics;
using Aide.Models.Accounts;
using Extension.Database.SqlServer;
using Extension.Models;
using Extension.String;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Aide.Winforms {
  public partial class UserCreateEditForm : Form {
    public string ActionType { get; set; }
    public DataRow OriginalData { get; set; }
    public ApplicationUser User { get; set; }
    private bool isCreate { get; set; }
    public string Id { get; set; }
    public string OriginalUserName { get; set; }
    public UserCreateEditForm(string actionType, string id = null, DataRow originalData = null) {
      InitializeComponent();
      ActionType = actionType;
      Id = id;
      OriginalData = originalData;
      initialization();
      localization();
    }

    private void initialization() {
      MaximizeBox = false;
      isCreate = ActionType.EqualsIgnoreCase(Aibe.DH.CreateActionName);

      List<string> teams = SQLServerHandler.GetSingleColumn(Aibe.DH.UserDBConnectionString, Aide.PH.TeamTableName, Aibe.DH.TeamNameColumnName)
        .Select(x => x?.ToString()).ToList();

      var adminRoles = Aibe.DH.AdminRoles.Select(x => x.AsSqlStringValue()).ToList();
      string baseWhereClause = string.Concat("([", Aibe.DH.RoleNameColumnName, "] != ", string.Join(string.Concat(" AND [", Aibe.DH.RoleNameColumnName, "] != "), adminRoles), ")");

      List<string> workingRoles = SQLServerHandler.GetSingleColumnWhere(Aibe.DH.UserDBConnectionString, Aide.PH.RoleTableName, Aibe.DH.RoleNameColumnName,
        baseWhereClause)
        .Select(x => x?.ToString()).ToList();

      comboBoxTeam.Items.Clear();
      comboBoxTeam.Items.Add(string.Empty);
      comboBoxTeam.Items.AddRange(teams.ToArray());
      comboBoxTeam.SelectedIndex = 0;
      comboBoxWorkingRole.Items.Clear();
      comboBoxWorkingRole.Items.Add(string.Empty);
      comboBoxWorkingRole.Items.AddRange(workingRoles.ToArray());
      comboBoxWorkingRole.SelectedIndex = 0;
      comboBoxAdminRole.Items.Clear();
      comboBoxAdminRole.Items.Add(string.Empty);
      comboBoxAdminRole.Items.Add(Aibe.DH.AdminRole); //the only admin role can be chosen here
      comboBoxAdminRole.SelectedIndex = 0;

      if (isCreate) //put original data only if this is an edit
        return;

      object userName = OriginalData[Aibe.DH.UserNameColumnName];
      object fullName = OriginalData[Aibe.DH.UserFullNameColumnName];
      object displayName = OriginalData[Aibe.DH.UserDisplayNameColumnName];
      object email = OriginalData[Aibe.DH.UserEmailColumnName];
      object team = OriginalData[Aibe.DH.UserTeamColumnName];
      object workingRole = OriginalData[Aibe.DH.UserWorkingRoleColumnName];
      object adminRole = OriginalData[Aibe.DH.UserAdminRoleColumnName];

      textBoxUserName.Text = userName is DBNull ? null : (string)userName;
      textBoxFullName.Text = fullName is DBNull ? null : (string)fullName;
      textBoxDisplayName.Text = displayName is DBNull ? null : (string)displayName;
      textBoxEmail.Text = email is DBNull ? null : (string)email;

      int index = -1;
      if (!(team is DBNull) && !string.IsNullOrWhiteSpace(team.ToString())) {
        index = comboBoxTeam.FindStringExact(team.ToString());
        if (index > 0)
          comboBoxTeam.SelectedIndex = index;
      }
      if (!(workingRole is DBNull) && !string.IsNullOrWhiteSpace(workingRole.ToString())) {
        index = comboBoxWorkingRole.FindStringExact(workingRole.ToString());
        if (index > 0)
          comboBoxWorkingRole.SelectedIndex = index;
      }
      if (!(adminRole is DBNull) && !string.IsNullOrWhiteSpace(adminRole.ToString())) {
        index = comboBoxAdminRole.FindStringExact(adminRole.ToString());
        if (index > 0)
          comboBoxAdminRole.SelectedIndex = index;
      }

      OriginalUserName = userName is DBNull ? string.Empty : (string)userName;
    }

    private void localization() {
      buttonClose.Text = Aibe.LCZ.W_Close;
      buttonPerformAction.Text = Aibe.LCZ.GetLocalizedDefaultActionName(ActionType);
      labelAction.Text = string.Concat("(", buttonPerformAction.Text, ")");
      Text = Aibe.LCZ.GetLocalizedDefaultActionName(ActionType) + " - " + Aibe.LCZ.W_User;
      labelTitle.Text = Aibe.LCZ.W_Team;

      labelUserName.Text = Aibe.LCZ.T_UserNameColumnName;
      labelFullName.Text = Aibe.LCZ.T_UserFullNameColumnName;
      labelDisplayName.Text = Aibe.LCZ.T_UserDisplayNameColumnName;
      labelEmail.Text = Aibe.LCZ.T_UserEmailColumnName;
      labelTeam.Text = Aibe.LCZ.T_UserTeamColumnName;
      labelWorkingRole.Text = Aibe.LCZ.T_UserWorkingRoleColumnName;
      labelAdminRole.Text = Aibe.LCZ.T_UserAdminRoleColumnName;
    }

    private void buttonClose_Click(object sender, EventArgs e) {
      Close();
    }

    private void buttonPerformAction_Click(object sender, EventArgs e) {
      User = new ApplicationUser {
        UserName = textBoxUserName.Text,
        FullName = textBoxFullName.Text,
        DisplayName = textBoxDisplayName.Text,
        Email = textBoxEmail.Text,
        Team = comboBoxTeam.SelectedItem.ToString(),
        WorkingRole = comboBoxWorkingRole.SelectedIndex >= 0 ? comboBoxWorkingRole.SelectedItem.ToString() : string.Empty,
        AdminRole = comboBoxAdminRole.SelectedIndex >= 0 ? comboBoxAdminRole.SelectedItem.ToString() : string.Empty,
      };
      BaseErrorModel errorModel;
      if (isCreate) {
        errorModel = UserLogic.Create(User);
      } else {
        User.Id = Id;
        errorModel = UserLogic.Edit(User, OriginalUserName);
      }
      if (errorModel.HasError) {
        MessageBox.Show(errorModel.Message, Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      DialogResult = DialogResult.OK; //do not close... just make the dialog result
    }
  }
}
