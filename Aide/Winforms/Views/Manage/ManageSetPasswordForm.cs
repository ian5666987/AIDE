using Aibe.Helpers;
using Aide.Logics;
using Aide.Models.Accounts;
using Extension.Models;
using System;
using System.Windows.Forms;

namespace Aide.Winforms {
  public partial class ManageSetPasswordForm : Form {
    public string PasswordText { get; set; }
    public string ConfirmPasswordText { get; set; }
    public string Id { get; set; }
    public ManageSetPasswordForm(string id = null) {
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
      buttonPerformAction.Text = Aibe.LCZ.GetLocalizedDefaultActionName(Aibe.DH.CreateActionName);
      labelPassword.Text = Aibe.LCZ.W_Password;
      labelConfirmPassword.Text = Aibe.LCZ.W_ConfirmPassword;
      labelAction.Text = string.Concat("(", buttonPerformAction.Text, ")");
      Text = Aibe.LCZ.GetLocalizedDefaultActionName(Aibe.DH.CreateActionName) + " - " + Aibe.LCZ.W_SetPassword;
      labelTitle.Text = Aibe.LCZ.W_Password;
    }

    private void buttonClose_Click(object sender, EventArgs e) {
      Close();
    }

    private void buttonPerformAction_Click(object sender, EventArgs e) {
      if (string.IsNullOrWhiteSpace(textBoxPassword.Text) || string.IsNullOrWhiteSpace(textBoxConfrimPassword.Text)) {
        MessageBox.Show(Aibe.LCZ.NFE_InputCannotBeEmpty, Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      PasswordText = textBoxPassword.Text;
      ConfirmPasswordText = textBoxConfrimPassword.Text;
      if (PasswordText != ConfirmPasswordText) {
        MessageBox.Show(Aibe.LCZ.NFE_PasswordDoesNotMatch, Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      BaseErrorModel model = UserLogic.SetPassword(Id, PasswordText);
      if (model.HasError) { //fail to set the password
        MessageBox.Show(model.Message, Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        Close();
      } else {
        ApplicationUser user = UserLogic.GetUserById(Id);
        UserHelper.SetUserMapPassword(user.UserName, PasswordText); //cannot be checked even if there is an error, since it returns void
        MessageBox.Show(Aibe.LCZ.NFM_SetPasswordSuccess, Aibe.LCZ.W_Successful, MessageBoxButtons.OK, MessageBoxIcon.Information);
        DialogResult = DialogResult.OK; //do not close... just make the dialog result
      }
    }
  }
}