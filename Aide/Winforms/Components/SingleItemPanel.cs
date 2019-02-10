using Aibe.Models.Core;
using Aibe.Models;
using Aide.Models.Controls;
using Aide.Winforms.Extensions;
using Aide.Winforms.Helpers;
using Aide.Winforms.Models;
using AWF = Aide.Winforms.SH;
using Extension.String;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.IO;

namespace Aide.Winforms.Components {
  public partial class SingleItemPanel : UserControl {
    //public SingleItemPanel(string name, string title, SingleItemPanelType type, bool isHidden, bool isReadOnly, object arg) {
    public int TotalWidth { get; private set; }
    public int TotalHeight { get; private set; }
    Point commonLocation = new Point(AWF.BaseControlPositionX, AWF.BaseControlPositionY);
    Size commonSize = new Size(AWF.BaseControlWidth, AWF.BaseControlHeight);
    public SingleItemPanelModel Model { get; private set; }
    private Dictionary<SingleItemPanelType, Action> handlers;    
    public delegate void PanelItemResizedDelegate(SingleItemPanel panel);
    public event PanelItemResizedDelegate PanelItemResized;
    public delegate void ComboBoxSelectedIndexChangedDelegate(ComboBox comboBox);
    public event ComboBoxSelectedIndexChangedDelegate ComboBoxSelectedIndexChanged;
    public delegate void ValueChangedDelegate(string columnName, string columnValue);
    public event ValueChangedDelegate ValueChanged;
    public bool IsEdit { get { return Model != null && Model.ActionType.EqualsIgnoreCase(Aibe.DH.EditActionName); } }
    public bool IsCreate { get { return Model != null && Model.ActionType.EqualsIgnoreCase(Aibe.DH.CreateActionName); } } 
    public SingleItemPanel(SingleItemPanelModel model) {
      InitializeComponent();
      Model = model;
      Name = model.Name;
      Visible = !model.IsHidden;
      labelItemTitle.Width = SH.BaseLabelWidth;
      labelItemTitle.Text = model.DisplayName;
      TotalWidth = AWF.BaseControlPositionX;
      TotalHeight = labelItemTitle.Height;

      initHandlers(); //unfortunately, this has to be called everytime...
      handlers[model.ItemType]();
    }

    //This should return the value of whatever is added as its control
    private Control controlItem; //this might be multiple, like picture case. But still... the real value has to be returned from one among the many... TODO check if it is true
    //now, based on single info, Model.ItemType, we can know what kind of controlItem we would expect to have
    //Then, based on the controlItem we will be having, we can form the proper value for the SingleItemPanel, except for... picture!
    //Actually, picture has only one value to be returned, the path to take the picture from...
    //That is the actual item to be used as control item, then it is from the outside, based on the path to take the picture from that we must retrieve the picture and name it accordingly
    public string GetValue() {
      StringBuilder sb = new StringBuilder();
      if (Model.IsForeignInfo && !Model.IsForeignInfoAssigned) //foreign info does not show anything (v1.4.1.0 except when it is assigned)
        return string.Empty;
      switch (Model.ItemType) {
        case SingleItemPanelType.Boolean:
          if (Model.IsReadOnly) {
            TextBox tb = (TextBox)controlItem;
            if (string.IsNullOrWhiteSpace(tb.Text))
              return string.Empty; //to represent null choice
            return Aibe.LCZ.IsBooleanTrue(tb.Text) ? true.ToString() : false.ToString();
          } else {
            ComboBox cb = (ComboBox)controlItem;
            if (cb.SelectedIndex < 0 || cb.SelectedItem == null || string.IsNullOrWhiteSpace(cb.SelectedItem.ToString()))
              return string.Empty;
            return Aibe.LCZ.IsBooleanTrue(cb.SelectedItem.ToString()) ? true.ToString() : false.ToString();
          } //the controlItem is quite special for this case
        case SingleItemPanelType.DateTime:
          if (Model.IsTimeStamp && !Model.IsFilter) //time stamp is not filled with anything... except if it is a filter
            return string.Empty;
          DateTime dateTimeVal = ((DateTimePicker)controlItem).Value;
          return dateTimeVal == DateTimePicker.MinimumDateTime ? string.Empty : //minimum value will be interpreted as empty
            dateTimeVal.ToString(Aibe.DH.DefaultDateTimeFormat); //TODO this may give little trouble as compared to the website counterpart which uses two values instead of one to form a 
        case SingleItemPanelType.DropDown:
          return ((ComboBox)controlItem).SelectedItem?.ToString();
        case SingleItemPanelType.List: //Basically, forms the string based on the current items
          if (Model.Info == null || !(Model.Info is ListColumnInfo) || dgv == null || dgv.Rows == null || dgv.Rows.Count <= 0)
            return string.Empty;
          ListColumnInfo lcInfo = (ListColumnInfo)Model.Info;
          char[] lcTypes = lcInfo.ListType.ToCharArray();
          int rowNo = 0;
          int index = 0;
          int totalRow = dgv.Rows.Count;
          foreach (DataGridViewRow row in dgv.Rows) { //foreach row, get each cell and then if the type is
            if ((IsCreate || IsEdit) && rowNo == totalRow - 1) //DGV which allows addition cannot count the last row
              break;
            if (rowNo > 0)
              sb.Append(";");
            index = 0;
            for (int i = 0; i < row.Cells.Count; ++i) {
              if (i > 0)
                sb.Append("|");
              DataGridViewCell cell = row.Cells[i];
              char c = lcTypes.Length > index ? lcTypes[index] : 'L'; //L is default subItemType
              string val = cell.Value == null ? string.Empty : cell.Value.ToString();
              sb.Append(val);
              ++index;
              if (c != 'O' && c != 'C')
                continue;
              DataGridViewComboBoxCell cbCell = (DataGridViewComboBoxCell)cell; //for option, increase more
              sb.Append("|");
              bool firstTime = true;
              foreach (object item in cbCell.Items) {
                if (item == null || string.IsNullOrWhiteSpace(item.ToString()))
                  continue;
                if (!firstTime)
                  sb.Append(",");
                sb.Append(item.ToString());
                firstTime = false;
              }
              ++index;
            }
            ++rowNo;
          }          
          return sb.ToString();
        case SingleItemPanelType.Number:
          if (Model.IsAutoGenerated && !Model.IsFilter) //auto generated, like timestamp, always returns empty string, except when it is filter
            return string.Empty;
          decimal decVal = ((DecimalAwareNumericUpDown)controlItem).Value;
          if (decVal == decimal.MinValue || decVal == decimal.MaxValue) //max number is interpreted as empty
            return string.Empty;
          return ((DecimalAwareNumericUpDown)controlItem).Value.ToString();
        case SingleItemPanelType.Picture: //yes, the controlItem for the picture is actually a picture link, which is a hidden label
        case SingleItemPanelType.NonPictureAttachment: //it is the same for the non-picture attachment
          string path = ((Label)controlItem).Text; //We only get the file name here, the full attachment source path is returned by GetAttachmentSourcePath()
          return string.IsNullOrWhiteSpace(path) ? string.Empty : Path.GetFileName(path); 
        case SingleItemPanelType.ScTable:
          return string.Empty; //should not return anything, but must be handled from outside correctly
        case SingleItemPanelType.Text:
          return ((TextBox)controlItem).Text;
        case SingleItemPanelType.TextField:
          return ((RichTextBox)controlItem).Text;
        case SingleItemPanelType.Display: //There are two types of display, IsNotAvailable and the reverse... as of now, just return as it is
          return ((Label)controlItem).Text;
        case SingleItemPanelType.Unknown:
        default: return string.Empty;
      }
    }

    public string GetAttachmentSourcePath() {
      if (Model.ItemType != SingleItemPanelType.Picture && Model.ItemType != SingleItemPanelType.NonPictureAttachment)
        return string.Empty;
      return ((Label)controlItem).Text; //yes, the controlItem for the picture is actually a picture link, which is a hidden label
    }

    private void initHandlers() {
      handlers = new Dictionary<SingleItemPanelType, Action> {
        {SingleItemPanelType.Text, new Action(handleText) },
        {SingleItemPanelType.DropDown, new Action(handleDropDown) },
        {SingleItemPanelType.TextField, new Action(handleTextField) },
        {SingleItemPanelType.DateTime, new Action(handleDateTime) },
        {SingleItemPanelType.Boolean, new Action(handleBoolean) },
        {SingleItemPanelType.List, new Action(handleList) },
        {SingleItemPanelType.Number, new Action(handleNumber) },
        {SingleItemPanelType.Picture, new Action(handlePicture) },
        {SingleItemPanelType.NonPictureAttachment, new Action(handleNonPictureAttachment) },
        {SingleItemPanelType.ScTable, new Action(handleScTable) },
        {SingleItemPanelType.Display, new Action(handleDisplay) },
        {SingleItemPanelType.Unknown, new Action(handleDisplay) },
      };
    }

    private void handleText() {
      TextBox tb = new TextBox() {
        Text = Model.Arg?.ToString(),
        Location = commonLocation,
        Size = commonSize,
        ReadOnly = Model.IsReadOnly,        
      };
      if (Model.IsForeignKey)
        tb.TextChanged += Tb_TextChanged;
      addItemWithSizeAdjustment(tb);
    }

    private void Tb_TextChanged(object sender, EventArgs e) {
      TextBox textBox = sender as TextBox;
      string value = textBox.Text;      
      ValueChanged?.Invoke(Model.Name, value);
    }

    #region DropDown
    private ComboBox comboBox;
    private void handleDropDown() {
      DropDownInfo ddInfo = Model.Info != null && Model.Info is DropDownInfo ? (DropDownInfo)Model.Info : null;
      ComboBoxModel cbModel = Model.Arg != null && Model.Arg is ComboBoxModel ? (ComboBoxModel)Model.Arg : null;
      if (ddInfo == null || cbModel == null) //cannot handle such case
        return;
      comboBox = new ComboBox() {
        Location = commonLocation,
        Size = commonSize, //DO NOT have read only... must translate the read only into text... outside        
        DropDownStyle = ComboBoxStyle.DropDownList,
        Tag = new ComboBoxTag { Name = Aide.DH.LiveDropDownTag, Model = cbModel, Info = ddInfo },
      };

      comboBox.Items.Add(string.Empty);
      if (cbModel.Options != null && cbModel.Options.Any())
        comboBox.Items.AddRange(cbModel.Options.ToArray());
      if (!string.IsNullOrWhiteSpace(cbModel.OriginalChosenItem) && comboBox.Items.Contains(cbModel.OriginalChosenItem))
        comboBox.SelectedIndex = comboBox.Items.IndexOf(cbModel.OriginalChosenItem);
      else
        comboBox.SelectedIndex = 0; //because there will be always empty option the first one

      comboBox.SelectedIndexChanged += Cb_SelectedIndexChanged;

      addItemWithSizeAdjustment(comboBox);
    }

    //When this is changed, it may affect a lot of items, including other dropdowns and listcolumns
    private void Cb_SelectedIndexChanged(object sender, EventArgs e) {
      if (!(sender is ComboBox))
        return;
      ComboBox cb = (ComboBox)sender;
      if (cb.Tag == null || !(cb.Tag is ComboBoxTag))
        return;
      ComboBoxSelectedIndexChanged?.Invoke(cb);
    }

    public ComboBox GetComboBox() {
      return comboBox;
    }

    public void SetComboBox(LiveDropDownResult result) { //equivalent to BuildDropDownString() in the AIWE
      if (comboBox == null || comboBox.Tag == null || !(comboBox.Tag is ComboBoxTag))
        return;
      ComboBoxTag tag = (ComboBoxTag)comboBox.Tag;
      comboBox.SelectedIndexChanged -= Cb_SelectedIndexChanged; //to prevent infinite calling when this thing itself is being changed
      string prevValue = result.Arg == null || 
        string.IsNullOrWhiteSpace(result.Arg.Value.ToString()) ?
        string.Empty : result.Arg.Value.ToString();
      comboBox.Items.Clear();
      comboBox.Items.Add(string.Empty);
      if (!string.IsNullOrWhiteSpace(result.ArgOriginalValue) && !result.Values.Contains(result.ArgOriginalValue))
        result.Values.Insert(0, result.ArgOriginalValue);
      if (result.Values != null && result.Values.Any())
        comboBox.Items.AddRange(result.Values.ToArray());
      if (!string.IsNullOrWhiteSpace(prevValue) && comboBox.Items.Contains(prevValue))
        comboBox.SelectedIndex = comboBox.Items.IndexOf(prevValue);
      else
        comboBox.SelectedIndex = 0; //because there will be always empty option the first one
      comboBox.SelectedIndexChanged += Cb_SelectedIndexChanged; //returns the selected index event handler at the very last step
    }
    #endregion

    #region foreign info related
    public void SetForeignInfoColumnValue(object value) {
      controlItem.Text = value?.ToString();
    }
    #endregion

    private void handleTextField() {
      RichTextBox rtb = new RichTextBox() {
        Location = commonLocation,
        Size = new Size(AWF.BaseControlWidth, AWF.BaseControlHeight + 2 * AWF.BaseControlHeightIncrement),
        ReadOnly = Model.IsReadOnly, //this is OK
      };
      TextFieldColumnInfo tfcInfo = Model.Info != null && Model.Info is TextFieldColumnInfo ? (TextFieldColumnInfo)Model.Info : null;
      if (tfcInfo != null) //adjust the height
        rtb.Height = AWF.BaseControlHeight + (Math.Max(0, tfcInfo.RowSize - 1)) * AWF.BaseControlHeightIncrement;
      else if(Model.PreferredRowSize != null && Model.PreferredRowSize.HasValue)
        rtb.Height = AWF.BaseControlHeight + (Math.Max(0, Model.PreferredRowSize.Value - 1)) * AWF.BaseControlHeightIncrement;
      if (Model.Arg != null)
        rtb.Text = Model.Arg.ToString();

      if (Model.IsForeignKey)
        rtb.TextChanged += Rtb_TextChanged;
      addItemWithSizeAdjustment(rtb);
    }

    private void Rtb_TextChanged(object sender, EventArgs e) {
      RichTextBox rtb = sender as RichTextBox;
      string value = rtb.Text;      
      ValueChanged?.Invoke(Model.Name, value);
    }

    private void handleDateTime() { //only called for filter, create, and edit
      DateTimePicker dtp = new DateTimePicker() {
        Location = commonLocation,
        Size = commonSize,
        Enabled = !Model.IsReadOnly, //this is OK. DateTime do not have ReadOnly property anyway...
        Format = DateTimePickerFormat.Custom,
        CustomFormat = Model.DateTimeFormat, //Use the given DateTimeFormat
        Value = DateTimePicker.MinimumDateTime,
      };
      if (Model.Arg != null && Model.Arg is DateTime? && ((DateTime?)Model.Arg).Value != null)
        dtp.Value = ((DateTime?)Model.Arg).Value;
      addItemWithSizeAdjustment(dtp);
    }

    //Very special handler because of the difficulties...
    private void handleBoolean() {
      Control ctrl;
      if (Model.IsReadOnly) {
        ctrl = new TextBox() {
          Location = commonLocation,
          Size = commonSize,
          Text = Model.Arg?.ToString(),
          ReadOnly = true
        };
      } else {
        ctrl = new ComboBox() {
          Location = commonLocation,
          Size = commonSize,
          DropDownStyle = ComboBoxStyle.DropDownList,
        };
        ComboBox cb = (ComboBox)ctrl;
        cb.Items.Clear();
        cb.Items.AddRange(Aibe.LCZ.GetLocalizedBooleanOptions().ToArray());
        if (Model.Arg != null) {
          string localStr = Aibe.LCZ.GetLocalizedBooleanOption(Model.Arg.ToString());
          int index = cb.FindStringExact(Model.Arg.ToString());
          if (index >= 0 && index < cb.Items.Count)
            cb.SelectedIndex = index;
        }
      }
      addItemWithSizeAdjustment(ctrl);
    }

    private DataGridView dgv; //the only dgv here...
    private void handleList() {
      ListColumnInfo lcInfo = Model.Info != null && Model.Info is ListColumnInfo ? (ListColumnInfo)Model.Info : null;
      if (Model.Info == null)
        return;
      dgv = lcInfo.GetView(Model.Arg?.ToString(), Model.IsReadOnly);
      dgv.RowsAdded += Dgv_RowsAdded;
      dgv.RowsRemoved += Dgv_RowsRemoved;
      dgv.CellValueChanged += Dgv_CellValueChanged;
      addItem(dgv);
    }

    private void changeSizeForDgv() {
      TotalWidth = AWF.BaseControlPositionX;
      TotalHeight = labelItemTitle.Height;
      allocateDgvWithSizeAdjustment();
      applyChangeSize();
    }

    public DataGridView GetDgv() {
      return dgv;
    }

    public void SetDgv (ListColumnResult result) { //equivalent to GetHTML in the AIWE
      if (dgv == null || dgv.Tag == null || (!(dgv.Tag is ListColumnInfo)))
        return;
      ListColumnInfo lcInfo = (ListColumnInfo)dgv.Tag;
      lcInfo.UpdateView(dgv, result.DataValue, Model.IsReadOnly);
    }

    private void Dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
      changeSizeForDgv();
    }

    private void Dgv_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e) {
      changeSizeForDgv();
    }

    private void Dgv_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e) {
      changeSizeForDgv();
    }

    private void handleNumber() {
      DecimalAwareNumericUpDown numUd = new DecimalAwareNumericUpDown() {
        Location = commonLocation,
        Size = commonSize,
        Enabled = !Model.IsReadOnly,
        Minimum = decimal.MinValue, //so that it won't be set as default value 0 to 100
        Maximum = decimal.MaxValue,
        Value = decimal.MinValue, //so that it will be invisible at first
        DecimalPlaces = 8,
        ThousandsSeparator = true,
        //ReadOnly = Model.IsReadOnly, //CANNOT use this because the spinner will still work!
      };
      NumberLimitColumnInfo numInfo = Model.Info != null && Model.Info is NumberLimitColumnInfo ? (NumberLimitColumnInfo)Model.Info : null;
      if (numInfo != null) {
        numUd.Minimum = (decimal)numInfo.Min;
        numUd.Maximum = (decimal)numInfo.Max;
      }
      if (Model.Arg != null) {
        decimal val;
        bool result = decimal.TryParse(Model.Arg.ToString(), out val);
        if (result)
          numUd.Value = val;
      }

      if (Model.IsForeignKey)
        numUd.ValueChanged += NumUd_ValueChanged;
      addItemWithSizeAdjustment(numUd);
    }

    private void NumUd_ValueChanged(object sender, EventArgs e) {
      DecimalAwareNumericUpDown numUd = sender as DecimalAwareNumericUpDown;
      string value = numUd.Value.ToString();
      ValueChanged?.Invoke(Model.Name, value);
    }

    #region picture and attachments
    private Label attachmentLink; //this is actually the most important item which will be used to return the picture value...
    private PictureBox pictureBox;
    private Label attachmentText;
    private Button removeButton;
    private Button browseButton;
    private List<string> attachmentFormats = new List<string>();
    private void handlePicture() {
      pictureBox = new PictureBox() {
        Name = "PictureBoxItem", //to be searched and removed
        Location = commonLocation,
        Enabled = !Model.IsReadOnly, //This is OK... PictureBox does not have ReadOnly property
        BorderStyle = BorderStyle.FixedSingle,
        SizeMode = PictureBoxSizeMode.Zoom,
      };

      attachmentLink = new Label() { //hidden, but will be used to return the value stored when picture is provided
        Visible = false,
      };
      addItem(attachmentLink);

      string imgPath = Model.Arg != null && Model.Arg is string ? Model.Arg.ToString() : null;
      if (imgPath != null) {
        PictureColumnInfo pcInfo = Model.Info != null && Model.Info is PictureColumnInfo ? (PictureColumnInfo)Model.Info : null;
        if (pcInfo != null) {
          //imgPath is the path stored in the value (original one), such as when editing
          string fullRelativePath = imgPath.Contains("/") || imgPath.Contains("\\") ? imgPath :
            Model.TableName + "\\" + Model.Cid + "\\" + imgPath;
          Image img = UiHelper.GetImage(fullRelativePath, pcInfo);
          if (pcInfo.IsStretched)
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
          pictureBox.Image = img;
          pictureBox.Size = new Size(img.Width, img.Height);
          attachmentLink.Text = FileHelper.GetAttachmentPath(fullRelativePath); 
          //This is stored with full path (always) //TODO probably won't work if there isn't image or path is invalid
        }
      } else
        pictureBox.Size = new Size(10, 10); //very small size when there is nothing to show      

      int oldTotalWidth = TotalWidth; //store the old Total width before anything else is added
      sizeAdjustment(pictureBox); //picture box is unique, it uses size adjustment but not addItem as if to register it as the main item...
      Controls.Add(pictureBox); //instead, the hidden pictureLink label will be registered thus used addItem
      //addItemWithSizeAdjustment(pictureBox);

      if (!Model.IsReadOnly) { //means there is also browse button, whenever is it not read only
        browseButton = new Button() {
          Name = "PictureBoxBrowseButton",
          Text = Aibe.LCZ.W_Browse,
          Location = new Point(commonLocation.X, commonLocation.Y + UiHelper.GetHeightOf(pictureBox)),
          Size = AWF.BrowseImageButtonSize,
        };
        browseButton.Click += ButtonBrowse_Click;
        TotalHeight += UiHelper.GetHeightOf(browseButton);
        TotalWidth = oldTotalWidth + //take the old total width, and add whichever is bigger, button or the picture
          Math.Max(UiHelper.GetWidthOf(browseButton), UiHelper.GetWidthOf(pictureBox)); //re-add the total width with whichever bigger
        Controls.Add(browseButton); //This item is added, but not really registered as the real "Control"

        removeButton = new Button() {
          Name = "PictureBoxRemoveButton",
          Text = Aibe.LCZ.W_Remove,
          Size = AWF.RemoveImageButtonSize,
          Visible = imgPath != null, //if there is image path, then this remove button is visible. Otherwise invisible
        };
        removeButton.Location = new Point(commonLocation.X + UiHelper.GetWidthOf(pictureBox),
          (int)(commonLocation.Y + 0.5 * (UiHelper.GetHeightOf(pictureBox) - UiHelper.GetHeightOf(removeButton))));
        int minTotalWidth = TotalWidth; //take the current total width
        removeButton.Click += ButtonRemove_Click;
        TotalWidth = Math.Max(oldTotalWidth + UiHelper.GetWidthOf(pictureBox) + UiHelper.GetWidthOf(removeButton), 
          minTotalWidth); //either oldTotalWidth + picture width + this button width or minTotal width, whichever is bigger
        Controls.Add(removeButton); //This item is added, but not really registered as the real "Control"
      }
    }

    private void handleNonPictureAttachment() {
      attachmentFormats.Clear();
      pictureBox = new PictureBox() {
        Name = "PictureBoxItem", //to be searched and removed
        Location = commonLocation,
        Enabled = !Model.IsReadOnly,
        BorderStyle = BorderStyle.FixedSingle,
        SizeMode = PictureBoxSizeMode.Zoom,
        //TODO get special attachment image
      };

      attachmentLink = new Label() { //hidden, but will be used to return the value stored when picture is provided
        Visible = false,
      };
      addItem(attachmentLink);

      string attPath = Model.Arg != null && Model.Arg is string ? Model.Arg.ToString() : null;
      if (attPath != null) {
        AttachmentInfo attInfo = Model.Info != null && Model.Info is AttachmentInfo ? (AttachmentInfo)Model.Info : null;
        attachmentFormats = attInfo.Formats ?? new List<string>();
        if (attInfo != null) {
          //imgPath is the path stored in the value (original one), such as when editing
          string fullRelativePath = attPath.Contains("/") || attPath.Contains("\\") ? attPath :
            Model.TableName + "\\" + Model.Cid + "\\" + attPath;
          string fullAttPath = FileHelper.GetAttachmentPath(fullRelativePath);
          bool exists = File.Exists(fullAttPath);
          if (exists) {
            Image img = new Bitmap(FileHelper.GetImagePath(Aide.PH.AttachmentImageIconFileName));
            pictureBox.Image = img;
            pictureBox.Size = new Size(img.Width, img.Height);
            attachmentLink.Text = FileHelper.GetAttachmentPath(fullRelativePath);
          } else {
            pictureBox.Size = new Size(10, 10); //very small size when there is nothing to show
          }
        }
      } else
        pictureBox.Size = new Size(10, 10); //very small size when there is nothing to show      

      int oldTotalWidth = TotalWidth; //store the old Total width before anything else is added
      sizeAdjustment(pictureBox); //picture box is unique, it uses size adjustment but not addItem as if to register it as the main item...
      Controls.Add(pictureBox); //instead, the hidden pictureLink label will be registered thus used addItem
      //addItemWithSizeAdjustment(pictureBox);

      if (!Model.IsReadOnly) { //means there is also browse button, whenever is it not read only
        browseButton = new Button() {
          Name = "PictureBoxBrowseButton",
          Text = Aibe.LCZ.W_Browse,
          Location = new Point(commonLocation.X, commonLocation.Y + UiHelper.GetHeightOf(pictureBox)),
          Size = AWF.BrowseImageButtonSize,
        };
        browseButton.Click += ButtonBrowseAttachment_Click;
        attachmentText = new Label() {
          Name = "AttachmentTextLabel",
          Text = string.IsNullOrWhiteSpace(attPath) ? "[" + Aibe.LCZ.W_NoAttachment + "]" : attPath,
          AutoSize = false,
          Location = new Point(commonLocation.X + browseButton.Width, commonLocation.Y + UiHelper.GetHeightOf(pictureBox)),
          MaximumSize = AWF.AttachmentLabelSize,
          Size = AWF.AttachmentLabelSize,
        };
        TotalHeight += UiHelper.GetHeightOf(browseButton);
        TotalWidth = oldTotalWidth + attachmentText.Width + //take the old total width, and add whichever is bigger, button or the picture
          Math.Max(UiHelper.GetWidthOf(browseButton), UiHelper.GetWidthOf(pictureBox)); //re-add the total width with whichever bigger
        Controls.Add(browseButton); //This item is added, but not really registered as the real "Control"
        Controls.Add(attachmentText);

        removeButton = new Button() {
          Name = "PictureBoxRemoveButton",
          Text = Aibe.LCZ.W_Remove,
          Size = AWF.RemoveImageButtonSize,
          Visible = attPath != null, //if there is image path, then this remove button is visible. Otherwise invisible
        };
        removeButton.Location = new Point(commonLocation.X + UiHelper.GetWidthOf(pictureBox),
          (int)(commonLocation.Y + 0.5 * (UiHelper.GetHeightOf(pictureBox) - UiHelper.GetHeightOf(removeButton))));
        int minTotalWidth = TotalWidth; //take the current total width
        removeButton.Click += ButtonRemove_Click;
        TotalWidth = Math.Max(oldTotalWidth + UiHelper.GetWidthOf(pictureBox) + UiHelper.GetWidthOf(removeButton),
          minTotalWidth); //either oldTotalWidth + picture width + this button width or minTotal width, whichever is bigger
        Controls.Add(removeButton); //This item is added, but not really registered as the real "Control"
      } else {
        attachmentText = new Label() {
          Name = "AttachmentTextLabel",
          Text = string.IsNullOrWhiteSpace(attPath) ? "[" + Aibe.LCZ.W_NoAttachment + "]" : attPath,
          AutoSize = false,
          Location = new Point(commonLocation.X + pictureBox.Width, commonLocation.Y),
          MaximumSize = AWF.AttachmentLabelSize,
          Size = AWF.AttachmentLabelSize,
        };
        int minTotalWidth = TotalWidth; //take the current total width
        TotalWidth = Math.Max(oldTotalWidth + UiHelper.GetWidthOf(pictureBox) + UiHelper.GetWidthOf(attachmentText),
          minTotalWidth); //either oldTotalWidth + picture width + this button width or minTotal width, whichever is bigger
        Controls.Add(attachmentText);
      }
    }

    private void repositioning() {
      int removeButtonWidth = removeButton == null ? 0 : UiHelper.GetWidthOf(removeButton);
      int imgWidth = UiHelper.GetWidthOf(pictureBox);
      int imgHeight = UiHelper.GetHeightOf(pictureBox);
      int browseButtonWidth = browseButton == null ? 0 : UiHelper.GetWidthOf(browseButton);
      int attachmentTextWidth = attachmentText == null ? 0 : UiHelper.GetWidthOf(attachmentText);
      int browseButtonHeight = browseButton == null ? 0 : UiHelper.GetHeightOf(browseButton);
      TotalWidth = AWF.BaseControlPositionX;
      TotalWidth += Math.Max(removeButtonWidth + imgWidth, browseButtonWidth + attachmentTextWidth);
      TotalHeight = Math.Max(imgHeight + browseButtonHeight, labelItemTitle.Height);
      if (browseButton != null) { //repositioning
        browseButton.Location = new Point(commonLocation.X, commonLocation.Y + UiHelper.GetHeightOf(pictureBox));
        if (attachmentText != null) //repositioning
          attachmentText.Location = new Point(commonLocation.X + browseButton.Width, commonLocation.Y + UiHelper.GetHeightOf(pictureBox));
      }
      if (removeButton != null) //repositioning
        removeButton.Location = new Point(commonLocation.X + UiHelper.GetWidthOf(pictureBox),
          (int)(commonLocation.Y + 0.5 * (UiHelper.GetHeightOf(pictureBox) - UiHelper.GetHeightOf(removeButton))));
      applyChangeSize();
    }

    private void ButtonRemove_Click(object sender, EventArgs e) {
      pictureBox.Image = null;
      pictureBox.Size = new Size(10, 10); //very small size when there is nothing to show
      attachmentLink.Text = string.Empty; //likewise, when the image is gone, the picture link also removes its image link...
      if (attachmentText != null)
        attachmentText.Text = "[" + Aibe.LCZ.W_NoAttachment + "]";
      removeButton.Hide();
      repositioning();
    }

    private void ButtonBrowseAttachment_Click(object sender, EventArgs e) {
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.Multiselect = false;
      ofd.InitialDirectory = Application.StartupPath;
      string formats = "*" + (attachmentFormats != null && attachmentFormats.Count > 0 ? string.Join(";*", attachmentFormats) : ".*");
      ofd.Filter = "Files|" + formats;
      DialogResult result = ofd.ShowDialog();
      if (result != DialogResult.OK)
        return;
      Image img = new Bitmap(FileHelper.GetImagePath(Aide.PH.AttachmentImageIconFileName));
      pictureBox.Image = img;
      pictureBox.Size = new Size(img.Width, img.Height);
      attachmentLink.Text = ofd.FileName; //again, the link is simply saved by the picture link... hidden but it is the real deal
      if (attachmentText != null)
        attachmentText.Text = ofd.FileName;
      repositioning();
      if (removeButton != null)
        removeButton.Show();
    }

    private void ButtonBrowse_Click(object sender, EventArgs e) {
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.Multiselect = false;
      ofd.InitialDirectory = Application.StartupPath;
      ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;*.tiff;*.bmp";
      DialogResult result = ofd.ShowDialog();
      if (result != DialogResult.OK)
        return;
      Image img = UiHelper.GetImageByFullPath(ofd.FileName, (PictureColumnInfo)Model.Info);
      attachmentLink.Text = ofd.FileName; //again, the link is simply saved by the picture link... hidden but it is the real deal
      pictureBox.Size = new Size(img.Width, img.Height);
      pictureBox.Image = img;
      repositioning();
      if (removeButton != null)
        removeButton.Show();
    }
    #endregion

    private void handleScTable() {
      ScTableInfo scTable = Model.Info != null && Model.Info is ScTableInfo ? (ScTableInfo)Model.Info : null;
      if (scTable == null)
        return;
      dgv = scTable.GetView(Model.ActionType.EqualsIgnoreCase(Aibe.DH.CreateActionName));
      addItem(dgv);
    }

    private void handleDisplay() {
      Label label = new Label() {
        Text = Model.Arg?.ToString(),
        Location = commonLocation,
        Size = commonSize,
        //Enabled = !Model.IsReadOnly, //Not needed as Display is always read only... Left alone for info
      };
      addItemWithSizeAdjustment(label);
    }

    //All registered control is added here...
    private void addItem(Control item) { 
      controlItem = item;
      Controls.Add(item);
    }

    private void addItemWithSizeAdjustment(Control item) {
      sizeAdjustment(item);
      addItem(item);
    }

    private void allocateDgv() {
      dgv.Location = commonLocation;
      UiHelper.AdjustDgv(dgv);
      int height = UiHelper.GetDgvRowsHeight(dgv);
      int width = UiHelper.GetDgvColumnsWidth(dgv);
      dgv.Width = Math.Min(Math.Max(AWF.BaseControlWidth, width + AWF.BaseDgvAddWidth), AWF.MaxDgvControlWidth);
      dgv.Height = Math.Min(Math.Max(AWF.BaseControlHeight, height + AWF.BaseDgvAddHeight), AWF.MaxDgvControlHeight);
    }

    private void allocateDgvWithSizeAdjustment() {
      allocateDgv();
      sizeAdjustment(dgv);
    }

    private void sizeAdjustment(Control item) {
      TotalWidth += UiHelper.GetWidthOf(item);
      TotalHeight = Math.Max(Math.Max(UiHelper.GetHeightOf(item) + AWF.BaseControlAdditionalHeight, TotalHeight), labelItemTitle.Height);
    }

    private void applyChangeSize() {
      Size = new Size(TotalWidth, TotalHeight);
      PanelItemResized?.Invoke(this);
    }

    bool firstTime = true; //without flag, the TotalWidth may keep getting larger everytime retry attempt is made...
    private void SingleItemPanel_Load(object sender, EventArgs e) {
      if (dgv != null && firstTime)
        allocateDgvWithSizeAdjustment();
      Size = new Size(TotalWidth, TotalHeight);
      firstTime = false;
    }
  }

  public enum SingleItemPanelType {
    Display, //for showing other message, for example, [Table-referenced column is not available]
    Text, //for normal string
    DropDown, //for dropdown
    TextField, //for textfield
    DateTime, //for date time
    Boolean, //for boolean
    List, //for list column
    Number, //for number, may be auto generated
    Picture, //for picture
    NonPictureAttachment, //for non-picture attachments
    ScTable, //for script columns
    Unknown,
  }
}

//CheckBox chb = new CheckBox() {
//  Location = commonLocation,
//  Enabled = !Model.IsReadOnly, //This is OK. CheckBox does not have ReadOnly property.
//};

//chb.Checked = Model.Arg != null && Model.Arg.ToString().EqualsIgnoreCase(Aibe.DH.True);