using Aibe.Helpers;
using Aide.Logics;
using Aide.Models.Accounts;
using Extension.Models;
using System;
using System.Windows.Forms;

namespace Aide.Winforms {
  public partial class ManageChangePasswordForm : Form {
    public string OldPasswordText { get; set; }
    public string NewPasswordText { get; set; }
    public string ConfirmPasswordText { get; set; }
    public string Id { get; set; }
    public ManageChangePasswordForm(string id = null) {
      InitializeComponent();
      Id = id;
      initialization();
      localization();
    }

    private void initialization() {
      MaximizeBox = false;
    }

    private void localization() {
      buttonClose.Text = Aibe.LCZ.W_Close;
      buttonPerformAction.Text = Aibe.LCZ.W_Change;
      labelOldPassword.Text = Aibe.LCZ.W_OldPassword;
      labelNewPassword.Text = Aibe.LCZ.W_NewPassword;
      labelConfirmPassword.Text = Aibe.LCZ.W_ConfirmPassword;
      labelAction.Text = string.Concat("(", buttonPerformAction.Text, ")");
      Text = Aibe.LCZ.W_Change + " - " + Aibe.LCZ.W_Password;
      labelTitle.Text = Aibe.LCZ.W_Password;
    }

    private void buttonClose_Click(object sender, EventArgs e) {
      Close();
    }

    private void buttonPerformAction_Click(object sender, EventArgs e) {
      OldPasswordText = textBoxOldPassword.Text;
      BaseErrorModel errorModel = UserLogic.AuthenticateUserById(Id, OldPasswordText);
      if (errorModel.HasError) { //fail to authenticate the old password
        MessageBox.Show(errorModel.Message, Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      } //means password match whatever stored in the database
      NewPasswordText = textBoxNewPassword.Text;
      ConfirmPasswordText = textBoxConfrimPassword.Text;
      if (NewPasswordText != ConfirmPasswordText) {
        MessageBox.Show(Aibe.LCZ.NFE_PasswordDoesNotMatch, Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      BaseErrorModel model = UserLogic.ChangePassword(Id, OldPasswordText, NewPasswordText);
      if (model.HasError) { //fail to change the password
        MessageBox.Show(model.Message, Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        Close();
      } else {
        ApplicationUser user = UserLogic.GetUserById(Id);
        UserHelper.SetUserMapPassword(user.UserName, NewPasswordText); //cannot be checked even if there is an error, since it returns void
        MessageBox.Show(Aibe.LCZ.NFM_ChangePasswordSuccess, Aibe.LCZ.W_Successful, MessageBoxButtons.OK, MessageBoxIcon.Information);
        DialogResult = DialogResult.OK; //do not close... just make the dialog result
      }
    }
  }
}