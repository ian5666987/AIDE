using Aide.Logics;
using Aide.Models.Accounts;
using Extension.Models;
using System;
using System.Windows.Forms;

namespace Aide.Winforms {
  public partial class LoginForm : Form {
    public string Identity { get; private set; }
    public string Password { get; private set; }
    public UserAuthenticationType AuthenticationType { get; private set; }
    public string IdentityLabel { get; private set; }
    public ApplicationUser User { get; private set; }
    public bool IsAnonymousAllowed { get; private set; }
    public LoginForm(UserAuthenticationType authenticationType, bool isAnonymousAllowed) {
      InitializeComponent();
      AuthenticationType = authenticationType;
      IsAnonymousAllowed = isAnonymousAllowed;
      Text = Aibe.LCZ.W_LogIn;
      switch (authenticationType) {
        case UserAuthenticationType.Email: IdentityLabel = Aibe.LCZ.T_UserEmailColumnName; break;
        case UserAuthenticationType.Id: IdentityLabel = Aibe.LCZ.T_UserIdColumnName; break;
        case UserAuthenticationType.Name: IdentityLabel = Aibe.LCZ.T_UserNameColumnName; break;
      }
      labelLoginIdentity.Text = IdentityLabel;
      labelLoginPassword.Text = Aibe.LCZ.W_Password;
      buttonLogin.Text = Aibe.LCZ.W_LogIn;
      buttonCancel.Text = "\u25C0 " + Aibe.LCZ.W_Cancel;
    }

    private void buttonLogin_Click(object sender, EventArgs e) {
      BaseErrorModel errorModel = AccountLogic.Login(textBoxLoginIdentity.Text, textBoxLoginPassword.Text, AuthenticationType, IsAnonymousAllowed);
      if (errorModel.Code == 1) { //anonymous, when allowed
        DialogResult = DialogResult.Cancel;
        return;
      }
      if (errorModel.HasError) {
        MessageBox.Show(errorModel.Message, Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return; //can retry
      }
      DialogResult = DialogResult.OK;
    }

    private void buttonCancel_Click(object sender, EventArgs e) {
      DialogResult = DialogResult.Cancel;
    }
  }
}
