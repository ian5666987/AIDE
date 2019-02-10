using Aibe.Models.Core;
using Aide.Winforms.Helpers;
using Aide.Winforms.Models;
using Extension.String;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace Aide.Winforms.Extensions {
  public static class ListColumnInfoExtension {
    //Called to create HTML for the list column
    public static DataGridView GetView(this ListColumnInfo info, string dataValue, bool isReadOnly = false) {
      //Initialization
      DataGridView dgv = new DataGridView() {
        ReadOnly = isReadOnly,
        AllowUserToAddRows = false,
        AllowUserToDeleteRows = false,
        AllowUserToOrderColumns = false,
        Tag = info,
      };
      dgv.CellContentClick += Dgv_CellContentClick;
      info.UpdateView(dgv, dataValue, isReadOnly);
      return dgv;
    }

    public static void UpdateView(this ListColumnInfo info, DataGridView dgv, string dataValue, bool isReadOnly = false) {
      //Initialization
      List<ListColumnItem> listColumnItems = new List<ListColumnItem>();
      if (!string.IsNullOrWhiteSpace(dataValue))
        listColumnItems = dataValue.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
          ?.Select(x => new ListColumnItem(x.Trim(), info.ListType, info.Widths)).ToList();
      dgv.Rows.Clear();
      dgv.Columns.Clear();

      //Create headers
      List<string> usedHeaders = new List<string>();
      string header = string.Empty;
      int width = ListColumnInfo.DefaultWidth;
      info.ResetHeaderCount();
      foreach (char c in info.ListType) { //for every item known, one header
        header = info.GetNextHeader();
        usedHeaders.Add(header);
        DataGridViewColumn dgvColumn = null;
        if (c == 'L' || c == 'V')
          dgvColumn = new DataGridViewTextBoxColumn() {
            HeaderText = header,
            SortMode = DataGridViewColumnSortMode.NotSortable, //ListColumn should not be sortable
          };
        else if (c == 'O' || c == 'C')
          dgvColumn = new DataGridViewComboBoxColumn() {
            HeaderText = header,
            SortMode = DataGridViewColumnSortMode.NotSortable, //ListColumn should not be sortable
          };
        if (dgvColumn != null) {
          dgvColumn.ReadOnly = isReadOnly;
          dgv.Columns.Add(dgvColumn);
        }
      }
      if (!isReadOnly) { //two columns, one for delete, another for copy
        dgv.Columns.Add(new DataGridViewTextBoxColumn() {
          Name = Aide.DH.ListColumnAddDeleteButtonColumnName,
          HeaderText = string.Empty,
          SortMode = DataGridViewColumnSortMode.NotSortable, //ListColumn should not be sortable
        });
        dgv.Columns.Add(new DataGridViewTextBoxColumn() {
          Name = Aide.DH.ListColumnCopyButtonColumnName,
          HeaderText = string.Empty,
          SortMode = DataGridViewColumnSortMode.NotSortable, //ListColumn should not be sortable
        });
      }

      //Create items
      int count = 1;
      foreach (var item in listColumnItems) {
        DataGridViewRow dgvRow = UiHelper.CreateCommonDgvRow();

        for (int i = 0; i < item.SubItems.Count; ++i) {
          ListColumnSubItem subItem = item.SubItems[i];
          int columnNo = i + 1;
          DataGridViewCell cell = isReadOnly ? new DataGridViewTextBoxCell() { Value = subItem.Value } : //The simplest of all, just print it
            createContentCell(subItem); //If not read only, do individual printing of the cell
          dgvRow.Cells.Add(cell);
          if (subItem.SubItemType == 'L')
            cell.ReadOnly = true; //can only be set AFTER the cell is attached to a row
        }

        if (!isReadOnly) {
          DataGridViewCell bdCell = createDeleteCell(info);
          dgvRow.Cells.Add(bdCell);
          DataGridViewCell bcCell = createCopyCell(info);
          dgvRow.Cells.Add(bcCell);
        }

        dgv.Rows.Add(dgvRow);
        count++;
      }

      if (!isReadOnly) {
        DataGridViewRow dgvAddRow = UiHelper.CreateCommonDgvRow();

        //All subcolumns are valueless TextBoxCells
        for (int i = 0; i < info.ListType.Length; ++i)
          dgvAddRow.Cells.Add(new DataGridViewTextBoxCell());

        DataGridViewCell baCell = createAddCell(info);
        DataGridViewTextBoxCell emptyCell = new DataGridViewTextBoxCell() {};
        dgvAddRow.Cells.Add(baCell);
        dgvAddRow.Cells.Add(emptyCell);
        emptyCell.ReadOnly = true; //can only be assigned after the cell is attached to a row
        dgv.Rows.Add(dgvAddRow);
      }
    }

    private static DataGridViewCell createAddCell(ListColumnInfo info) {
      DataGridViewCell baCell = new DataGridViewButtonCell() { //create one delete cell per row
        Value = Aibe.LCZ.W_Add,
        Tag = new DgvButtonTag { LcInfo = info, ActionName = Aibe.DH.AddActionName },
      };
      return baCell;
    }

    private static DataGridViewCell createDeleteCell(ListColumnInfo info) {
      DataGridViewCell bdCell = new DataGridViewButtonCell() { //create one delete cell per row
        Value = Aibe.LCZ.W_Delete,
        Tag = new DgvButtonTag { LcInfo = info, ActionName = Aibe.DH.DeleteActionName },
      };
      return bdCell;
    }

    private static DataGridViewCell createCopyCell(ListColumnInfo info) {
      DataGridViewCell bcCell = new DataGridViewButtonCell() { //create one delete cell per row
        Value = Aibe.LCZ.W_Copy,
        Tag = new DgvButtonTag { LcInfo = info, ActionName = Aibe.DH.CopyActionName },
      };
      return bcCell;
    }

    private static DataGridViewCell createContentCell(ListColumnSubItem subItem) {
      DataGridViewCell cell = null;
      switch (subItem.SubItemType) {
        case 'L': //the simplest case, just print it
          cell = new DataGridViewTextBoxCell() { Value = subItem.Value }; //label cannot be changed
          break;
        case 'V': //Value type, print it...
          cell = new DataGridViewTextBoxCell() { Value = subItem.Value, };
          break;
        case 'O':
          cell = new DataGridViewComboBoxCell();
          ((DataGridViewComboBoxCell)cell).Items.Add(string.Empty);
          if (subItem.HasOptions) {
            ((DataGridViewComboBoxCell)cell).Items.AddRange(subItem.Options.ToArray());
            if (subItem.Value != null && !string.IsNullOrWhiteSpace(subItem.Value) && subItem.HasOptionOfItsValue)
              cell.Value = subItem.Value;
          } else {
            if (subItem.Value != null && !string.IsNullOrWhiteSpace(subItem.Value)) {
              ((DataGridViewComboBoxCell)cell).Items.Add(subItem.Value);
              cell.Value = subItem.Value;
            }
          }
          break;
        case 'C':
          cell = new DataGridViewComboBoxCell();
          ((DataGridViewComboBoxCell)cell).Items.AddRange(Aibe.LCZ.GetLocalizedLcBooleanOptions().ToArray()); //without to array, the result would be funny
          if (subItem.Value != null && !string.IsNullOrWhiteSpace(subItem.Value) && Aibe.LCZ.IsLocalizedLcBooleanOption(subItem.Value))
            cell.Value = subItem.Value;
          break;
        default: break;
      }
      return cell;

    }

    private static void Dgv_CellContentClick(object sender, DataGridViewCellEventArgs e) {
      DataGridView dgv = (DataGridView)sender;
      if (dgv.Columns[e.ColumnIndex].Name != Aide.DH.ListColumnAddDeleteButtonColumnName &&
        dgv.Columns[e.ColumnIndex].Name != Aide.DH.ListColumnCopyButtonColumnName)
        return;
      DataGridViewCell cell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
      if (cell.Tag == null || !(cell.Tag is DgvButtonTag))
        return;
      DgvButtonTag tag = (DgvButtonTag)cell.Tag;
      if (tag.LcInfo == null) //cannot proceed without the right info
        return;
      ListColumnInfo info = tag.LcInfo;
      bool isAdd = tag.ActionName.EqualsIgnoreCase(Aibe.DH.AddActionName);
      bool isCopy = tag.ActionName.EqualsIgnoreCase(Aibe.DH.CopyActionName);
      if (isAdd || isCopy) {
        DataGridViewRow addRow = isAdd ? dgv.Rows[dgv.Rows.Count - 1] : dgv.Rows[e.RowIndex];
        DataGridViewRow newRow = UiHelper.CreateCommonDgvRow(); //needs info of what type of item is here
        for (int i = 0; i < info.ListType.Length; ++i) {
          char c = info.ListType[i];
          ListColumnSubItem subItem = new ListColumnSubItem(addRow.Cells[i].Value?.ToString(), c, -1); //apply the default width
          DataGridViewCell newCell = createContentCell(subItem);
          if (isAdd) //only nullify the cell value after use if the action is add
            addRow.Cells[i].Value = null;
          if (newCell != null) {
            newRow.Cells.Add(newCell);
            newCell.ReadOnly = c == 'L';
          }
        }
        newRow.Cells.Add(createDeleteCell(info));
        newRow.Cells.Add(createCopyCell(info));
        dgv.Rows.Insert(dgv.Rows.Count - 1, newRow);
      } else if (tag.ActionName.EqualsIgnoreCase(Aibe.DH.DeleteActionName)) {
        dgv.Rows.RemoveAt(e.RowIndex); //Delete is very straightforward
      }
    }

    public static string GetDetailsView(this ListColumnInfo info, string dataValue) {
      //Checking
      var listColumnItems = dataValue.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
        ?.Select(x => new ListColumnItem(x.Trim(), info.ListType, info.Widths)).ToList();

      if (listColumnItems.Count <= 0)
        return null;

      //Initialization
      StringBuilder sb = new StringBuilder();
      sb.Append("<table style=\"border-collapse:separate;border-spacing:10px 5px;border:1px solid black\">");

      //Create headers
      List<string> usedHeaders = new List<string>();
      string header = string.Empty;
      info.ResetHeaderCount();
      sb.Append("<tr>");
      foreach (char c in info.ListType) { //for every item known, one header
        sb.Append("<td><b>");
        header = info.GetNextHeader();
        usedHeaders.Add(header);
        sb.Append(header);
        sb.Append("</b></td>");
      }
      sb.Append("</tr>");

      //Create items
      foreach (var item in listColumnItems) {
        sb.Append("<tr>");
        for (int i = 0; i < item.SubItems.Count; ++i)
          sb.Append("<td>" + item.SubItems[i].Value + "</td>");
        sb.Append("</tr>");
      }

      //Ending
      sb.Append("</table>");
      return sb.ToString();
    }
  }
}

//switch (c) {
//  case 'L': newCell = new DataGridViewTextBoxCell() { Value = addRow.Cells[i].Value, ReadOnly = true }; break; //label cannot be changed
//  case 'V': newCell = new DataGridViewTextBoxCell() { Value = addRow.Cells[i].Value }; break;
//  case 'O':
//    newCell = new DataGridViewComboBoxCell();
//    if()
//    break;
//  case 'C':
//    break;
//  default:
//    break;
//};

//Ending
//sb.Append("<tr>");
//string usedHeader = usedHeaders[i];
//char subItemType = info.ListType[i];
//int columnNo = i + 1;
//DataGridViewCell dgvAddCell = new DataGridViewTextBoxCell();
//sb.Append("<td>");
//sb.Append("<input");
//insertCommonHTMLAttributes(sb, info.Name, count, subItemType, columnNo, true);
//sb.Append(" type=\"text\" size=\"" + info.Widths[i] + "\" value=\"\"");
//sb.Append(" placeholder=\"");
//switch (subItemType) {
//  case 'L': //L an V share the same placeholder
//  case 'V': //sb.Append(usedHeader); break;
//  case 'O': //sb.Append(usedHeader + " Or " + usedHeader + " | Option 1, Option 2, ..., Option N"); break;
//  case 'C': //sb.Append("Yes or No"); break; //very special for the check
//  default: break;
//}
//sb.Append("\""); //end of placeholder
//sb.Append("/>"); //end of input
//sb.Append("</td>");
//dgvAddRow.Cells.Add(dgvAddCell);
//  //Add button
//  //sb.Append("<td>");
//  //sb.Append("<button");
//  //sb.Append(" class=\"common-subcolumn-button\"");
//  //sb.Append(" commonbuttontype=\"add\"");
//  //sb.Append(" id=\"common-subcolumn-button-add-" + info.Name + "\"");
//  //sb.Append(" commoncolumnname=\"" + info.Name + "\"");
//  //sb.Append(">Add</button>");
//  //sb.Append("</td>");
//sb.Append("</tr>");
//sb.Append("</table>");
//return sb.ToString();
//dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; //now handled separately
//dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

//sb.Append("<tr>");
//sb.Append("<td>");
//sb.Append(subItem.Value);
//sb.Append("<input");
//sb.Append(" readonly=\"readonly\"");
//sb.Append(" style=\"background-color:#" + readOnlyBackgroundColor + "\"");
//sb.Append(" type=\"text\" size=\"" + subItem.Width + "\" value=\"");
//sb.Append(subItem.Value);
//sb.Append("\" />");
//sb.Append(subItem.Value);
//sb.Append("<input");
//insertCommonHTMLAttributes(sb, info.Name, count, subItem.SubItemType, columnNo, false);
//sb.Append(" type=\"text\" size=\"" + subItem.Width + "\" value=\"");
//sb.Append(subItem.Value);
//sb.Append("\" />");
//sb.Append("<select");
//insertCommonHTMLAttributes(sb, info.Name, count, subItem.SubItemType, columnNo, false);
//sb.Append(">");
//if (subItem.HasOptions) {
//  if (string.IsNullOrWhiteSpace(subItem.Value)) {
//    sb.Append("<option selected=\"selected\"></option>");
//  } else {
//    sb.Append("<option value=\"\"></option>");
//  }
//  foreach (var option in subItem.Options) {
//    sb.Append("<option value=\"");
//    sb.Append(option);
//    if (!string.IsNullOrWhiteSpace(subItem.Value) && option == subItem.Value)
//      sb.Append("\" selected=\"selected");
//    sb.Append("\">");
//    sb.Append(option);
//    sb.Append("</option>\n");
//  }
//}
//sb.Append("</select>");
//sb.Append("<select");
//insertCommonHTMLAttributes(sb, info.Name, count, subItem.SubItemType, columnNo, false);
//sb.Append(">");
//string def = subItem.Value?.ToLower()?.Trim();
//string selectedStr = " selected=\"selected\"";
//string addStr = string.IsNullOrWhiteSpace(def) ? selectedStr : string.Empty;
//sb.Append(string.Concat("<option value=\"\"", addStr, "></option>"));
//addStr = def == "yes" ? selectedStr : string.Empty;
//sb.Append(string.Concat("<option value=\"Yes\"", addStr, ">Yes</option>"));
//addStr = def == "no" ? selectedStr : string.Empty;
//sb.Append(string.Concat("<option value=\"No\"", addStr, ">No</option>"));
//sb.Append("</select>");
//sb.Append("</td>");
//sb.Append("<td>");
//sb.Append("<button");
//sb.Append(" class=\"common-subcolumn-button\"");
//sb.Append(" commonbuttontype=\"delete\"");
//sb.Append(" id=\"common-subcolumn-button-delete-" + info.Name + "-" + count + "\"");
//sb.Append(" commondeleteno=\"" + count + "\"");
//sb.Append(" commoncolumnname=\"" + info.Name + "\"");
//sb.Append(">Delete</button>");
//sb.Append("</td>");
//sb.Append("</tr>");

//sb.Append("<tr>");
//sb.Append("<td><b>");
//sb.Append(header);
//sb.Append("</b></td>");
//  sb.Append("<td><b>Actions</b></td>"); //last header would be action, if there is any
//sb.Append("</tr>");

//private static void insertCommonHTMLAttributes(StringBuilder sb, string columnName, int rowNo, char subItemType, int columnNo, bool isAdd) {
//  sb.Append(isAdd ? " class=\"common-subcolumn-input-add\"" : " class=\"common-subcolumn-input\"");
//  sb.Append(" commoncolumnname=\"" + columnName + "\"");
//  sb.Append(" commonrowno=\"" + rowNo + "\"");
//  sb.Append(" commonsubitemtype=\"" + subItemType + "\"");
//  sb.Append(" commoncolumnno=\"" + columnNo + "\"");
//  sb.Append(" commoninputisadd=\"" + isAdd + "\"");
//  sb.Append(" id=\"common-subcolumn-input-" + columnName + "-" + subItemType + "-" + rowNo + "-" + columnNo + "-" + isAdd + "\"");
//}

//string readOnlyBackgroundColor = "ececec";
//StringBuilder sb = new StringBuilder();
//sb.Append("<table style=\"border-collapse:separate;border-spacing:10px 5px;border:1px solid black\">");
