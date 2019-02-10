using Aide.Logics;
using Extension.Models;
using Extension.String;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Aide.Winforms {
  public partial class RoleDetailsForm : Form {
    public string ActionType { get; set; }
    public string OriginalName { get; set; }
    public string Id { get; set; }
    bool isDetails = false;
    Size fullSize = new Size(480, 216);
    Size detailsSize = new Size(480, 160);
    public RoleDetailsForm(string actionType, string id, string originalText) {
      InitializeComponent();
      ActionType = actionType;
      Id = id;
      OriginalName = originalText;
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
      //Can only be the delete, since details will hide the perform action button
      BaseErrorModel errorModel = RoleLogic.Delete(Id);
      if (errorModel.HasError) {
        MessageBox.Show(errorModel.Message, Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      DialogResult = DialogResult.OK; //do not close... just make the dialog result
    }
  }
}
