using Extension.Database.SqlServer;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Aide.Winforms {
  public partial class ManageDisplayNameForm : Form {
    public string OriginalText { get; set; }
    public string DisplayNameText { get; set; }
    public string Id { get; set; }
    public ManageDisplayNameForm(string id = null, string originalText = null) {
      InitializeComponent();
      Id = id;
      OriginalText = originalText;
      initialization();
      localization();
    }

    private void initialization() {
      MaximizeBox = false;

      textBoxDisplayName.Text = OriginalText;
    }

    private void localization() {
      buttonClose.Text = Aibe.LCZ.W_Close;
      buttonPerformAction.Text = Aibe.LCZ.W_Change;
      labelDisplayName.Text = Aibe.LCZ.T_UserDisplayNameColumnName;
      labelAction.Text = string.Concat("(", buttonPerformAction.Text, ")");
      Text = Aibe.LCZ.W_Change + " - " + Aibe.LCZ.T_UserDisplayNameColumnName;
      labelTitle.Text = Aibe.LCZ.T_UserDisplayNameColumnName;
    }

    private void buttonClose_Click(object sender, EventArgs e) {
      Close();
    }

    private void buttonPerformAction_Click(object sender, EventArgs e) {
      if (string.IsNullOrWhiteSpace(textBoxDisplayName.Text)) {
        MessageBox.Show(Aibe.LCZ.NFE_InputCannotBeEmpty, Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      DisplayNameText = textBoxDisplayName.Text;
      int result = SQLServerHandler.Update(Aibe.DH.UserDBConnectionString, Aide.PH.UserTableName, new Dictionary<string, object> {
        { Aibe.DH.UserDisplayNameColumnName, DisplayNameText } }, Aibe.DH.UserIdColumnName, Id);
      if (result <= 0) { //fail to update the display name
        MessageBox.Show(Aibe.LCZ.NFE_UserUpdateFailed, Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        Close();
      } else {
        MessageBox.Show(Aibe.LCZ.NFM_ChangeDisplayNameSuccess, Aibe.LCZ.W_Successful, MessageBoxButtons.OK, MessageBoxIcon.Information);
        DialogResult = DialogResult.OK; //do not close... just make the dialog result
      }
    }
  }
}