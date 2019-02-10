using Aide.Logics;
using Extension.Models;
using Extension.String;
using System;
using System.Windows.Forms;

namespace Aide.Winforms {
  public partial class TeamCreateEditFilterForm : Form {
    public string ActionType { get; set; }
    public string OriginalName { get; set; }
    public string TeamText { get; set; }
    public int Id { get; set; }
    public TeamCreateEditFilterForm(string actionType, int id = -1, string originalText = null) {
      InitializeComponent();
      ActionType = actionType;
      Id = id;
      OriginalName = actionType != null && actionType.EqualsIgnoreCase(Aibe.DH.CreateActionName) ? string.Empty : originalText;
      initialization();
      localization();
    }

    private void initialization() {
      MaximizeBox = false;

      textBoxTeamName.Text = OriginalName;
    }

    private void localization() {
      buttonClose.Text = Aibe.LCZ.W_Close;
      buttonPerformAction.Text = Aibe.LCZ.GetLocalizedDefaultActionName(ActionType);
      labelTeamName.Text = Aibe.LCZ.T_TeamNameColumnName;
      labelAction.Text = string.Concat("(", buttonPerformAction.Text, ")");
      Text = Aibe.LCZ.GetLocalizedDefaultActionName(ActionType) + " - " + Aibe.LCZ.W_Team;
      labelTitle.Text = Aibe.LCZ.W_Team;
    }

    private void buttonClose_Click(object sender, EventArgs e) {
      Close();
    }

    private void buttonPerformAction_Click(object sender, EventArgs e) {
      TeamText = textBoxTeamName.Text;
      if (ActionType.EqualsIgnoreCase(Aibe.DH.FilterActionName)) {
        DialogResult = DialogResult.OK;
        return;
      }
      BaseErrorModel errorModel = ActionType.EqualsIgnoreCase(Aibe.DH.CreateActionName) ?
        TeamLogic.Create(TeamText) : TeamLogic.Edit(Id, TeamText);
      if (errorModel.HasError) {
        MessageBox.Show(errorModel.Message, Aibe.LCZ.W_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      DialogResult = DialogResult.OK; //do not close... just make the dialog result
    }
  }
}
