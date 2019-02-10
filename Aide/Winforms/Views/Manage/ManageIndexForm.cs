using Aide.Logics;
using Aide.Models.Accounts;
using System;
using System.Windows.Forms;

namespace Aide.Winforms {
  public partial class ManageIndexForm : Form {
    public string Id { get; set; }
    public ApplicationUser User { get; set; }
    public bool HasPassword { get; set; }
    public event EventHandler DisplayNameChanged;
    public ManageIndexForm() {
      InitializeComponent();
      Id = Identity.User.Id;
      localization();
      initialization();
    }

    protected virtual void OnDisplayNameChanged (EventArgs e) {
      if (DisplayNameChanged != null)
        DisplayNameChanged(this, e);
    }

    private void initialization() {
      MaximizeBox = false;
      refreshTable();
    }

    private void localization() {
      Text = Aibe.LCZ.W_Account;
      buttonClose.Text = Aibe.LCZ.W_Close;
      labelTitle.Text = Aibe.LCZ.W_Account;
      labelAction.Text = string.Concat("(", Aibe.LCZ.W_Index, ")");
      groupBoxAccountManagement.Text = Aibe.LCZ.NFM_ChangeYourAccountSettings;

      labelFullName.Text = Aibe.LCZ.T_UserFullNameColumnName;
      labelUserName.Text = Aibe.LCZ.T_UserNameColumnName;
      labelDisplayName.Text = Aibe.LCZ.T_UserDisplayNameColumnName;
      labelPassword.Text = Aibe.LCZ.W_Password;
      labelWorkingRole.Text = Aibe.LCZ.T_UserWorkingRoleColumnName;
      labelAdminRole.Text = Aibe.LCZ.T_UserAdminRoleColumnName;
      labelTeam.Text = Aibe.LCZ.T_UserTeamColumnName;
      labelRegistrationDate.Text = Aibe.LCZ.T_UserRegistrationDateColumnName;
      labelLastLogin.Text = Aibe.LCZ.T_UserLastLoginColumnName;
    }

    private void refreshTable() {
      User = UserLogic.GetUserById(Id);
      if (User == null) {
        MessageBox.Show(Aibe.LCZ.NFE_UserNotFound, Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        Close();
      }

      HasPassword = !string.IsNullOrEmpty(User.PasswordHash);
      labelFullNameValue.Text = User.FullName;
      labelUserNameValue.Text = User.UserName;
      labelDisplayNameValue.Text = User.DisplayName;
      labelPasswordValue.Text = HasPassword ? "******" : string.Empty;
      labelWorkingRoleValue.Text = User.WorkingRole;
      labelAdminRoleValue.Text = User.AdminRole;
      labelTeamValue.Text = User.Team;
      labelRegistrationDateValue.Text = User.RegistrationDate.ToString(Aide.DH.DefaultDateTimeFormat);
      labelLastLoginValue.Text = User.LastLogin == null ? string.Empty : User.LastLogin.Value.ToString(Aide.DH.DefaultDateTimeFormat);
      linkLabelPasswordChange.Text = HasPassword ? Aibe.LCZ.W_Change : Aibe.LCZ.W_SetPassword;
      linkLabelDisplayNameChange.Text = Aibe.LCZ.W_Change;
    }

    private void buttonClose_Click(object sender, EventArgs e) {
      Close();
    }

    private void linkLabelDisplayNameChange_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      ManageDisplayNameForm form = new ManageDisplayNameForm(Id, User.DisplayName);
      if (DialogResult.OK == form.ShowDialog()) {
        refreshTable();
        Identity.User.DisplayName = User.DisplayName;
        OnDisplayNameChanged(new EventArgs());
      }
      form.Dispose();
      form = null;
    }

    private void linkLabelPasswordChange_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      if (HasPassword) { //change password
        ManageChangePasswordForm form = new ManageChangePasswordForm(Id);
        if (DialogResult.OK == form.ShowDialog())
          refreshTable();
        form.Dispose();
        form = null;
      } else { //new password
        ManageSetPasswordForm form = new ManageSetPasswordForm(Id);
        if (DialogResult.OK == form.ShowDialog())
          refreshTable();
        form.Dispose();
        form = null;
      }
    }
  }
}
