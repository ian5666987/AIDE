﻿using Aide.Winforms.Components;
using System;

namespace Aide.Winforms.Models {
  public class SingleItemPanelModel {
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public SingleItemPanelType ItemType { get; set; }
    public bool IsHidden { get; set; }
    public bool IsReadOnly { get; set; }
    public object Info { get; set; }
    public object Arg { get; set; }
    public bool IsNotAvailable { get; set; } //only for Display
    public bool IsAutoGenerated { get; set; } //only for Number
    public bool IsTimeStamp { get; set; } //only for DateTime
    public string TableName { get; set; } //only used for edit picture
    public string ActionType { get; set; } //only used for ScTable, to distinguish create from edit, and also to distinguish editable from read only for List
    public bool IsFilter { get; set; } //to distinguish if a model is used for filter or for something else
    public int Cid { get; set; } //only used in edit
    public int? PreferredRowSize { get; set; } //only used to force TextField row in details and delete
    public string DateTimeFormat { get; set; }
    public bool IsForeignInfo { get; set; } //only to indicate foreign info
    public bool IsForeignKey { get; set; } //to indicate if this column is a foreign key
    public bool IsForeignInfoAssigned { get; set; } //since v1.4.1.0 to indicate if the foreign info value will be transferred
    public string ForeignInfoAssignedColumnName { get; private set; } //since v1.4.1.0, to directly indicate the foreign info assign column, if any, for simplicity

    public static SingleItemPanelModel CreateReadOnlyCommonModel(string columnName, string dataValue, string displayName, bool isForeignInfo, bool isForeignInfoAssigned) {
      int dataLength = dataValue == null ? 0 : dataValue.Length;
      int preferredSize = dataLength > SH.TextLengthPerRowInDetails ? (int)((double)dataLength / SH.TextLengthPerRowInDetails + 1) : 1;
      preferredSize = Math.Min(SH.MaxTextFieldRowSize, preferredSize);
      return new SingleItemPanelModel() {
        Name = columnName, DisplayName = displayName,
        ItemType = preferredSize > 1 ? SingleItemPanelType.TextField : SingleItemPanelType.Text, //Not Display, simply to distinguish it from the actual label
        PreferredRowSize = preferredSize,
        IsReadOnly = true,
        IsForeignInfo = isForeignInfo,
        IsForeignInfoAssigned = isForeignInfoAssigned,
        ForeignInfoAssignedColumnName = isForeignInfoAssigned ? columnName.Split('-')[3] : null, //v1.4.1.0 columnName.Split('-')[3] is not the best but the simplest way to handle the assigned column
        Arg = dataValue,
      };
    }
  }
}
