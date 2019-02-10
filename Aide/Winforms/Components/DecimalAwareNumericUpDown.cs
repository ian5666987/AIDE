using System.Drawing;
using System.Windows.Forms;

namespace Aide.Winforms.Components {
  public class DecimalAwareNumericUpDown : NumericUpDown { //can be extended as wanted
    protected override void UpdateEditText() {
      if (Value == decimal.MinValue || Value == decimal.MaxValue)
        Text = string.Empty;
      else 
        Text = Value.ToString("0." + new string('#', DecimalPlaces));
    }
  }
}