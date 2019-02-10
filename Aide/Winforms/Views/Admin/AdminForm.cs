using Aide.Winforms.Helpers;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Aide.Winforms {
  public partial class AdminForm : Form {
    Size adminSize = new Size(255, 320);
    Size devSize = new Size(255, 585);
    public AdminForm(bool asDeveloper) {
      InitializeComponent();
      localization();
      groupBoxRoles.Visible = asDeveloper;
      groupBoxDeveloperOptions.Visible = asDeveloper;
      Size usedSize = asDeveloper ? devSize : adminSize;
      Size = usedSize;
      MaximumSize = usedSize;
      MinimumSize = usedSize;
      MaximizeBox = false;
      ShowIcon = false;      
    }

    UserIndexForm userIndexForm;
    private void buttonUsers_Click(object sender, EventArgs e) {
      if (userIndexForm == null || userIndexForm.IsDisposed)
        userIndexForm = new UserIndexForm();
      userIndexForm.Show();
    }

    TeamIndexForm teamIndexForm;
    private void buttonTeams_Click(object sender, EventArgs e) {
      if (teamIndexForm == null || teamIndexForm.IsDisposed)
        teamIndexForm = new TeamIndexForm();
      teamIndexForm.Show();
    }

    private void buttonAccessLog_Click(object sender, EventArgs e) {
      FormHelper.ProcessCommonIndex(Aibe.DH.AccessLogTableName);
    }

    private void buttonActionLog_Click(object sender, EventArgs e) {
      FormHelper.ProcessCommonIndex(Aibe.DH.ActionLogTableName);
    }

    RoleIndexForm roleIndexForm;
    private void buttonRoles_Click(object sender, EventArgs e) {
      if (roleIndexForm == null || roleIndexForm.IsDisposed)
        roleIndexForm = new RoleIndexForm();
      roleIndexForm.Show();
    }

    private void buttonMeta_Click(object sender, EventArgs e) {
      FormHelper.ProcessCommonIndex(Aibe.DH.MetaTableName);
    }

    private void buttonUserMap_Click(object sender, EventArgs e) {
      FormHelper.ProcessCommonIndex(Aibe.DH.UserMapTableName);
    }

    private void buttonErrorLog_Click(object sender, EventArgs e) {
      FormHelper.ProcessCommonIndex(Aibe.DH.ErrorLogTableName);
    }

    private void localization() {
      Text = Aibe.LCZ.W_Admin;
      groupBoxUsers.Text = Aibe.LCZ.W_Users;
      groupBoxRoles.Text = Aibe.LCZ.W_Roles;
      groupBoxLogs.Text = Aibe.LCZ.W_Logs;
      groupBoxDeveloperOptions.Text = Aibe.LCZ.W_DeveloperOptions;
      buttonUsers.Text = Aibe.LCZ.W_Users;
      buttonTeams.Text = Aibe.LCZ.W_Teams;
      buttonAccessLog.Text = Aibe.LCZ.W_AccessLog;
      buttonActionLog.Text = Aibe.LCZ.W_ActionLog;
      buttonRoles.Text = Aibe.LCZ.W_Roles;
      buttonMeta.Text = Aibe.LCZ.W_Meta;
      buttonUserMap.Text = Aibe.LCZ.W_UserMap;
      buttonErrorLog.Text = Aibe.LCZ.W_ErrorLog;
    }
  }
}
