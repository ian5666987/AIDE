using Aibe.Helpers;
using Aide.Logics;
using Extension.Database.SqlServer;
using Extension.Models;
using Extension.String;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Aide.Winforms {
  public partial class UserDetailsForm : Form {
    public string ActionType { get; set; }
    public DataRow OriginalData { get; set; }
    public string UserName { get; set; }
    public string Id { get; set; }
    bool isDetails = false;
    Size fullSize = new Size(560, 575);
    Size detailsSize = new Size(560, 519);

    public UserDetailsForm(string actionType, string id = null, DataRow originalData = null) {
      InitializeComponent();
      ActionType = actionType;
      Id = id;
      OriginalData = originalData;
      initialization();
      localization();
    }

    private void initialization() {
      MaximizeBox = false;
      isDetails = ActionType.EqualsIgnoreCase(Aibe.DH.DetailsActionName);
      splitContainerContent.Panel2Collapsed = isDetails; //if it is details, then just collapse this panel
      Size usedSize = isDetails ? detailsSize : fullSize;
      Size = usedSize;
      MinimumSize = usedSize;
      MaximumSize = usedSize;

      object userName = OriginalData[Aibe.DH.UserNameColumnName];
      object fullName = OriginalData[Aibe.DH.UserFullNameColumnName];
      object displayName = OriginalData[Aibe.DH.UserDisplayNameColumnName];
      object email = OriginalData[Aibe.DH.UserEmailColumnName];
      object team = OriginalData[Aibe.DH.UserTeamColumnName];
      object workingRole = OriginalData[Aibe.DH.UserWorkingRoleColumnName];
      object adminRole = OriginalData[Aibe.DH.UserAdminRoleColumnName];
      object registrationDate = OriginalData[Aibe.DH.UserRegistrationDateColumnName];
      object lastLogin = OriginalData[Aibe.DH.UserLastLoginColumnName];

      textBoxUserName.Text = userName is DBNull ? null : (string)userName;
      textBoxFullName.Text = fullName is DBNull ? null : (string)fullName;
      textBoxDisplayName.Text = displayName is DBNull ? null : (string)displayName;
      textBoxEmail.Text = email is DBNull ? null : (string)email;
      textBoxTeam.Text = team is DBNull ? null : (string)team;
      textBoxWorkingRole.Text = workingRole is DBNull ? null : (string)workingRole;
      textBoxAdminRole.Text = adminRole is DBNull ? null : (string)adminRole;
      textBoxRegistrationDate.Text = registrationDate is DBNull ? null : ((DateTime)registrationDate).ToString(Aide.DH.DefaultDateTimeFormat);
      textBoxLastLogin.Text = lastLogin is DBNull ? null : ((DateTime)lastLogin).ToString(Aide.DH.DefaultDateTimeFormat);

      UserName = userName is DBNull ? string.Empty : (string)userName;
    }

    private void localization() {
      buttonClose.Text = Aibe.LCZ.W_Close;
      buttonPerformAction.Text = Aibe.LCZ.GetLocalizedDefaultActionName(ActionType);
      labelAction.Text = string.Concat("(", buttonPerformAction.Text, ")");
      Text = Aibe.LCZ.GetLocalizedDefaultActionName(ActionType) + " - " + Aibe.LCZ.W_Team;
      labelTitle.Text = Aibe.LCZ.W_Team;

      labelUserName.Text = Aibe.LCZ.T_UserNameColumnName;
      labelFullName.Text = Aibe.LCZ.T_UserFullNameColumnName;
      labelDisplayName.Text = Aibe.LCZ.T_UserDisplayNameColumnName;
      labelEmail.Text = Aibe.LCZ.T_UserEmailColumnName;
      labelTeam.Text = Aibe.LCZ.T_UserTeamColumnName;
      labelWorkingRole.Text = Aibe.LCZ.T_UserWorkingRoleColumnName;
      labelAdminRole.Text = Aibe.LCZ.T_UserAdminRoleColumnName;
      labelRegistrationDate.Text = Aibe.LCZ.T_UserRegistrationDateColumnName;
      labelLastLogin.Text = Aibe.LCZ.T_UserLastLoginColumnName;
    }

    private void buttonClose_Click(object sender, EventArgs e) {
      Close();
    }

    private void buttonPerformAction_Click(object sender, EventArgs e) {
      //Can only be the delete
      BaseErrorModel errorModel = UserLogic.Delete(Id, UserName);
      if (errorModel.HasError) {
        MessageBox.Show(errorModel.Message, Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        DialogResult = DialogResult.No;
        return;
      }
      DialogResult = DialogResult.OK; //do not close... just make the dialog result
    }
  }
}