using Aibe.Helpers;
using Aibe.Models;
using Aide.Winforms.Helpers;
using Extension.String;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Aide.Winforms.Extensions {
  public static class ScTableInfoExtension {
    //Called to create HTML for the list column
    public static DataGridView GetView(this ScTableInfo scTable, bool isCreate) {
      //Initialization
      DataGridView dgv = new DataGridView() {
        ReadOnly = true, //always read only
        AllowUserToAddRows = false,
        AllowUserToDeleteRows = false,
      };
      List<DataColumn> scColumns = scTable.GetAvailableDataColumns();

      foreach (var scColumn in scColumns) {
        DataGridViewColumn dgvColumn;
        if (scTable.ScInfo.IsPictureLinkColumn(scColumn.ColumnName)) {
          dgvColumn = new DataGridViewImageColumn() {
            HeaderText = scColumn.ColumnName.ToCamelBrokenString(),
            ReadOnly = true, //ScTable always read only              
          };
        } else {
          dgvColumn = new DataGridViewTextBoxColumn() {
            HeaderText = scColumn.ColumnName.ToCamelBrokenString(),
            ReadOnly = true, //ScTable always read only              
          };
        }
        dgv.Columns.Add(dgvColumn);
      }
      dgv.Invalidate();

      if (!scTable.HasRow || isCreate)
        return dgv;

      foreach (var scRow in scTable.Rows) {
        int scCid = scTable.HasCidColumn ? (int)scRow[Aibe.DH.Cid] : -1;
        DataGridViewRow dgvRow = UiHelper.CreateCommonDgvRow();
        foreach (var scColumn in scTable.Columns) {
          DataGridViewCell cell = null;
          string scColumnName = scColumn.ColumnName;
          object scData = scRow[scColumnName];
          if (scData != null && !string.IsNullOrWhiteSpace(scData.ToString()) && scTable.ScInfo.IsPictureLinkColumn(scColumnName)) {
            string scLink = scData.ToString();
            int inputWidth = scTable.ScInfo.GetPictureWidthFor(scColumnName);
            if (scLink.Contains("/") || scLink.Contains("\\") || !scTable.HasCidColumn || //longer relative path
              string.IsNullOrWhiteSpace(scTable.ScInfo.RefTableName)) {
              cell = new DataGridViewImageCell() {
                Value = UiHelper.GetImage(scLink, false, false, inputWidth, 0),
              };
            } else { //the RefTableName must exists if it is to be used
              string fullRelativePath = scTable.ScInfo.RefTableName + "\\" + scCid + "\\" + scLink;
              cell = new DataGridViewImageCell() {
                Value = UiHelper.GetImage(fullRelativePath, false, false, inputWidth, 0),
              };
            }
          } else { //not picture link column            
            cell = new DataGridViewTextBoxCell() {
              Value = DateTimeHelper.ProcessPossibleDateTimeString(scData, scTable.IsDateTimeColumn(scColumnName), Aide.PH.ScTableDateTimeFormat),
            };
          }
          dgvRow.Cells.Add(cell);
        }
        dgv.Rows.Add(dgvRow);
      }
      return dgv;
    }
  }
}

//string imgPath = UiHelper.GetImagePath(scLink);
//Image img = new Bitmap(imgPath);
//< img src = "~/Images/@scLink" width = "@scTable.ScInfo.GetPictureWidthFor(scColumnName)" title = "@scData" />
//string imgPath = UiHelper.GetImagePath(fullRelativePath);
//Image img = new Bitmap(imgPath);
//< img src = "~/Images/@scTable.ScInfo.RefTableName/@scCid/@scLink" width = "@scTable.ScInfo.GetPictureWidthFor(scColumnName)" title = "@scData" />
//@scData
