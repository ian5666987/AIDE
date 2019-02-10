using Aide.Logics;
using Extension.Models;
using Extension.String;
using System;
using System.Windows.Forms;

namespace Aide.Winforms {
  public partial class RoleCreateEditFilterForm : Form {
    public string ActionType { get; set; }
    public string OriginalName { get; set; }
    public string RoleText { get; set; }
    public string Id { get; set; }
    public RoleCreateEditFilterForm(string actionType, string id = null, string originalText = null) {
      InitializeComponent();
      ActionType = actionType;
      Id = id;
      OriginalName = actionType != null && actionType.EqualsIgnoreCase(Aibe.DH.CreateActionName) ? string.Empty : originalText;
      initialization();
      localization();
    }

    private void initialization() {
      MaximizeBox = false;

      textBoxRoleName.Text = OriginalName;
    }

    private void localization() {
      buttonClose.Text = Aibe.LCZ.W_Close;
      buttonPerformAction.Text = Aibe.LCZ.GetLocalizedDefaultActionName(ActionType);
      labelRoleName.Text = Aibe.LCZ.T_RoleNameColumnName;
      labelAction.Text = string.Concat("(", buttonPerformAction.Text, ")");
      Text = Aibe.LCZ.GetLocalizedDefaultActionName(ActionType) + " - " + Aibe.LCZ.W_Role;
      labelTitle.Text = Aibe.LCZ.W_Role;
    }

    private void buttonClose_Click(object sender, EventArgs e) {
      Close();
    }

    private void buttonPerformAction_Click(object sender, EventArgs e) {
      RoleText = textBoxRoleName.Text;
      if (ActionType.EqualsIgnoreCase(Aibe.DH.FilterActionName)) {
        DialogResult = DialogResult.OK;
        return;
      }
      BaseErrorModel errorModel = ActionType.EqualsIgnoreCase(Aibe.DH.CreateActionName) ?
        RoleLogic.Create(RoleText) : RoleLogic.Edit(Id, RoleText);
      if (errorModel.HasError) {
        MessageBox.Show(errorModel.Message, Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      DialogResult = DialogResult.OK; //do not close... just make the dialog result
    }
  }
}
