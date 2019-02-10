using Aide.Models.Filters;
using Extension.Database.SqlServer;
using Extension.String;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Aide.Winforms {
  public partial class UserFilterForm : Form {
    public string ActionType { get; set; }
    public ApplicationUserFilter Filter { get; set; }
    public UserFilterForm(string actionType, ApplicationUserFilter filter) {
      InitializeComponent();
      ActionType = actionType;
      Filter = filter;
      initialization();
      localization();
    }

    private void initialization() {
      MaximizeBox = false;
      dateTimePickerRegistrationDateFrom.Value = DateTimePicker.MinimumDateTime;
      dateTimePickerRegistrationDateTo.Value = DateTimePicker.MinimumDateTime;
      dateTimePickerLastLoginFrom.Value = DateTimePicker.MinimumDateTime;
      dateTimePickerLastLoginTo.Value = DateTimePicker.MinimumDateTime;

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
      comboBoxAdminRole.Items.Add(Aibe.DH.AdminRole);
      comboBoxAdminRole.Items.Add(Aibe.DH.MainAdminRole);
      comboBoxAdminRole.SelectedIndex = 0;

      textBoxUserName.Text = Filter.UserName;
      textBoxFullName.Text = Filter.FullName;
      textBoxDisplayName.Text = Filter.DisplayName;
      textBoxEmail.Text = Filter.Email;

      int index = -1;
      if (!string.IsNullOrWhiteSpace(Filter.Team)) {
        index = comboBoxTeam.FindStringExact(Filter.Team);
        if (index > 0)
          comboBoxTeam.SelectedIndex = index;
      }
      if (!string.IsNullOrWhiteSpace(Filter.WorkingRole)) {
        index = comboBoxWorkingRole.FindStringExact(Filter.WorkingRole);
        if (index > 0)
          comboBoxWorkingRole.SelectedIndex = index;
      }
      if (!string.IsNullOrWhiteSpace(Filter.AdminRole)) {
        index = comboBoxAdminRole.FindStringExact(Filter.AdminRole);
        if (index > 0)
          comboBoxAdminRole.SelectedIndex = index;
      }

      if (Filter.RegistrationDateFrom != null)
        dateTimePickerRegistrationDateFrom.Value = Filter.RegistrationDateFrom.Value;
      if (Filter.RegistrationDateTo != null)
        dateTimePickerRegistrationDateTo.Value = Filter.RegistrationDateTo.Value;
      if (Filter.LastLoginFrom != null)
        dateTimePickerLastLoginFrom.Value = Filter.LastLoginFrom.Value;
      if (Filter.LastLoginTo != null)
        dateTimePickerLastLoginTo.Value = Filter.LastLoginTo.Value;
    }

    private void localization() {
      buttonClose.Text = Aibe.LCZ.W_Close;
      buttonPerformAction.Text = Aibe.LCZ.GetLocalizedDefaultActionName(ActionType);
      labelAction.Text = string.Concat("(", buttonPerformAction.Text, ")");
      Text = Aibe.LCZ.GetLocalizedDefaultActionName(ActionType) + " - " + Aibe.LCZ.W_User;
      labelTitle.Text = Aibe.LCZ.W_User;

      labelUserName.Text = Aibe.LCZ.T_UserNameColumnName;
      labelFullName.Text = Aibe.LCZ.T_UserFullNameColumnName;
      labelDisplayName.Text = Aibe.LCZ.T_UserDisplayNameColumnName;
      labelEmail.Text = Aibe.LCZ.T_UserEmailColumnName;
      labelTeam.Text = Aibe.LCZ.T_UserTeamColumnName;
      labelWorkingRole.Text = Aibe.LCZ.T_UserWorkingRoleColumnName;
      labelAdminRole.Text = Aibe.LCZ.T_UserAdminRoleColumnName;
      labelRegistrationDateFrom.Text = Aibe.LCZ.T_UserRegistrationDateColumnName + " (" + Aibe.LCZ.W_From + ")";
      labelRegistrationDateTo.Text = Aibe.LCZ.T_UserRegistrationDateColumnName + " (" + Aibe.LCZ.W_To + ")";
      labelLastLoginFrom.Text = Aibe.LCZ.T_UserLastLoginColumnName + " (" + Aibe.LCZ.W_From + ")";
      labelLastLoginTo.Text = Aibe.LCZ.T_UserLastLoginColumnName + " (" + Aibe.LCZ.W_To + ")";
    }

    private void buttonClose_Click(object sender, EventArgs e) {
      Close();
    }

    private void buttonPerformAction_Click(object sender, EventArgs e) {
      string userName = string.IsNullOrWhiteSpace(textBoxUserName.Text) ? null : textBoxUserName.Text;
      string fullName = string.IsNullOrWhiteSpace(textBoxFullName.Text) ? null : textBoxFullName.Text;
      string displayName = string.IsNullOrWhiteSpace(textBoxDisplayName.Text) ? null : textBoxDisplayName.Text;
      string email = string.IsNullOrWhiteSpace(textBoxEmail.Text) ? null : textBoxEmail.Text;
      string team = string.IsNullOrWhiteSpace(comboBoxTeam.SelectedItem?.ToString()) ? null : comboBoxTeam.SelectedItem?.ToString();
      string workingRole = string.IsNullOrWhiteSpace(comboBoxWorkingRole.SelectedItem?.ToString()) ? null : comboBoxWorkingRole.SelectedItem?.ToString();
      string adminRole = string.IsNullOrWhiteSpace(comboBoxAdminRole.SelectedItem?.ToString()) ? null : comboBoxAdminRole.SelectedItem?.ToString();
      Filter = new ApplicationUserFilter {
        UserName = userName,
        FullName = fullName,
        DisplayName = displayName,
        Email = email,
        Team = team,
        WorkingRole = workingRole,
        AdminRole = adminRole,
        RegistrationDateFrom = null,
        RegistrationDateTo = null,
        LastLoginFrom = null,
        LastLoginTo = null,
      };
      if (dateTimePickerRegistrationDateFrom.Value != DateTimePicker.MinimumDateTime)
        Filter.RegistrationDateFrom = dateTimePickerRegistrationDateFrom.Value;
      if (dateTimePickerRegistrationDateTo.Value != DateTimePicker.MinimumDateTime)
        Filter.RegistrationDateTo = dateTimePickerRegistrationDateTo.Value;
      if (dateTimePickerLastLoginFrom.Value != DateTimePicker.MinimumDateTime)
        Filter.LastLoginFrom = dateTimePickerLastLoginFrom.Value;
      if (dateTimePickerLastLoginTo.Value != DateTimePicker.MinimumDateTime)
        Filter.LastLoginTo = dateTimePickerLastLoginTo.Value;
      DialogResult = DialogResult.OK; //do not close... just make the dialog result
    }

    private void linkLabelRegistrationDateFromReset_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      dateTimePickerRegistrationDateFrom.Value = DateTimePicker.MinimumDateTime;
    }

    private void linkLabelRegistrationDateToReset_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      dateTimePickerRegistrationDateTo.Value = DateTimePicker.MinimumDateTime;
    }

    private void linkLabelLastLoginFromReset_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      dateTimePickerLastLoginFrom.Value = DateTimePicker.MinimumDateTime;
    }

    private void linkLabelLastLoginToReset_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      dateTimePickerLastLoginTo.Value = DateTimePicker.MinimumDateTime;
    }
  }
}
