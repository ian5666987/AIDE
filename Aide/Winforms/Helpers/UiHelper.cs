using Aibe.Models.Core;
using AWF = Aide.Winforms.SH;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Aide.Winforms.Helpers {
  public class UiHelper {
    public static void AdjustDgv(DataGridView dgv) {
      //dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; //this is causing the datagridview to run very slowly!
      bool adjustRow = false;
      dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize; //this is acceptable, because there can't be too many columns
      for (int i = 0; i < dgv.Columns.Count; ++i) {
        //dgv.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; //this is causing the datagridview to run very slowly!
        dgv.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells; //this is acceptable, because there can't be too many columns
        int colw = dgv.Columns[i].Width;
        dgv.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        dgv.Columns[i].Width = Math.Min(colw, AWF.MaxDgvColumnWidth);
        if (dgv.Columns[i] is DataGridViewImageColumn)
          adjustRow = true;
      }
      if (adjustRow)
        dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells; //this is also slowing down the performance! But unfortunately necessary for Dgv with ImageCell
    }

    //Using fixed DgvControlHeight is a lot faster than auto-adjustment
    public static DataGridViewRow CreateCommonDgvRow() { //To start DGV row with common height + some other propertis (if necessary)
      return new DataGridViewRow { Height = SH.MinDgvControlHeight };
    }

    public static int GetWidthOf(Control item) {
      return item.Width + item.Margin.Left + item.Margin.Right;
    }

    public static int GetHeightOf(Control item) {
      return item.Height + item.Margin.Top + item.Margin.Bottom;
    }

    public static int AdjustWindowsWidth(int itemTotalWidth, int previousTotalWidth) {
      return Math.Max(itemTotalWidth + AWF.BaseWindowsWidth, previousTotalWidth);
    }

    public static Size GetAppliedWindowsSize(int totalWidth, int totalHeight, Size maxSize) {
      return new Size(Math.Min(maxSize.Width, totalWidth), Math.Min(maxSize.Height, totalHeight));
    }

    public static Size GetAppliedIndexWindowsSize(int totalWidth, int totalHeight, Size maxSize) {
      return new Size(Math.Min(maxSize.Width, totalWidth), Math.Min(maxSize.Height, totalHeight));
    }

    public static int GetDgvRowsHeight(DataGridView dgv) {
      int height = 0;
      //Previously attempting to have fixed row height, but doesn't work well because picture column in index has different row height!
      foreach (DataGridViewRow row in dgv.Rows)
        height += row.Height;
      return height + dgv.ColumnHeadersHeight;
    }

    public static int GetDgvColumnsWidth(DataGridView dgv) {
      int width = 0;
      foreach (DataGridViewColumn column in dgv.Columns)
        width += column.Width;
      return width;
    }

    public static Image GetIndexImage(string fullRelativePath, PictureColumnInfo pcInfo) {
      string imgPath = FileHelper.GetAttachmentPath(fullRelativePath);
      Image img = new Bitmap(imgPath);
      if (pcInfo.IndexIsStretched) {
        return ResizeImage(img, pcInfo.IndexWidth, pcInfo.IndexHeight);
      } else if (pcInfo.IndexHeightComesFirst) {
        double scale = (double)pcInfo.IndexHeight / img.Height; //the displayed one is how many times the original
        int width = (int)(scale * img.Width);
        return ResizeImage(img, width, pcInfo.IndexHeight);
      } else {
        double scale = (double)pcInfo.IndexWidth / img.Width; //the displayed one is how many times the original
        int height = (int)(scale * img.Height);
        return ResizeImage(img, pcInfo.IndexWidth, height);
      }
    }

    public static Image GetImage(string fullRelativePath, bool isStretched, bool heightComesFirst, int inputWidth, int inputHeight) {
      string attPath = FileHelper.GetAttachmentPath(fullRelativePath);
      Image img = new Bitmap(attPath);
      if (isStretched) {
        return ResizeImage(img, inputWidth, inputHeight);
      } else if (heightComesFirst) {
        double scale = (double)inputHeight / img.Height; //the displayed one is how many times the original
        int width = (int)(scale * img.Width);
        return ResizeImage(img, width, inputHeight);
      } else {
        double scale = (double)inputWidth / img.Width; //the displayed one is how many times the original
        int height = (int)(scale * img.Height);
        return ResizeImage(img, inputWidth, height);
      }
    }

    public static Image GetImage(string fullRelativePath, PictureColumnInfo pcInfo) {
      string imgPath = FileHelper.GetAttachmentPath(fullRelativePath);
      Image img = new Bitmap(imgPath);
      if (pcInfo.IsStretched) {
        return ResizeImage(img, pcInfo.Width, pcInfo.Height);
      } else if (pcInfo.HeightComesFirst) {
        double scale = (double)pcInfo.Height / img.Height; //the displayed one is how many times the original
        int width = (int)(scale * img.Width);
        return ResizeImage(img, width, pcInfo.Height);
      } else {
        double scale = (double)pcInfo.Width / img.Width; //the displayed one is how many times the original
        int height = (int)(scale * img.Height);
        return ResizeImage(img, pcInfo.Width, height);
      }
    }

    public static Image GetImageByFullPath(string fullPath, PictureColumnInfo pcInfo) {
      Image img = new Bitmap(fullPath);
      if (pcInfo.IsStretched) {
        return ResizeImage(img, pcInfo.Width, pcInfo.Height);
      } else if (pcInfo.HeightComesFirst) {
        double scale = (double)pcInfo.Height / img.Height; //the displayed one is how many times the original
        int width = (int)(scale * img.Width);
        return ResizeImage(img, width, pcInfo.Height);
      } else {
        double scale = (double)pcInfo.Width / img.Width; //the displayed one is how many times the original
        int height = (int)(scale * img.Height);
        return ResizeImage(img, pcInfo.Width, height);
      }
    }

    //Taken from https://stackoverflow.com/questions/1922040/resize-an-image-c-sharp
    /// <summary>
    /// Resize the image to the specified width and height.
    /// </summary>
    /// <param name="image">The image to resize.</param>
    /// <param name="width">The width to resize to.</param>
    /// <param name="height">The height to resize to.</param>
    /// <returns>The resized image.</returns>
    public static Bitmap ResizeImage(Image image, int width, int height) {
      var destRect = new Rectangle(0, 0, width, height);
      var destImage = new Bitmap(width, height);

      destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

      using (var graphics = Graphics.FromImage(destImage)) {
        graphics.CompositingMode = CompositingMode.SourceCopy;
        graphics.CompositingQuality = CompositingQuality.HighQuality;
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        using (var wrapMode = new ImageAttributes()) {
          wrapMode.SetWrapMode(WrapMode.TileFlipXY);
          graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
        }
      }

      return destImage;
    }
  }
}

//Auto adjustment is no longer used, because to items will be placed straight down
//int originalTotalHeight = totalHeight - AWF.BaseWindowsHeight;
//int originalTotalWidth = totalWidth - AWF.BaseWindowsWidth;
//int multiplier = 1;
//while (totalWidth < maxSize.Width) {
//  if (totalHeight <= maxSize.Height)
//    break;
//  int testTotalWidth = originalTotalWidth * (multiplier + 1) + AWF.BaseWindowsWidth;
//  if (testTotalWidth > maxSize.Width)
//    break;
//  totalWidth = testTotalWidth;
//  totalHeight = (int)((double)originalTotalHeight / (multiplier + 1)) + AWF.BaseWindowsHeight;
//  multiplier++;
//}
//return new Size(totalWidth, totalHeight);
