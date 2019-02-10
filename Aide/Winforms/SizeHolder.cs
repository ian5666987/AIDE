using System.Drawing;

namespace Aide.Winforms {
  public class SH { //can be extended as wanted
    //Base sizes
    public const int MaxTextFieldRowSize = 20;
    public const int TextLengthPerRowInDetails = 40;
    public const int BaseControlWidth = 320;
    public const int BaseControlHeight = 30;
    public const int BaseDgvHeight = 60;
    public const int BaseControlHeightIncrement = 24;
    public const int BaseControlAdditionalHeight = 10;
    public const int BaseLabelWidth = 240; //good but not enough for some
    public const int BaseControlPositionX = 250;
    public const int BaseControlPositionY = 5;
    public const int BaseWindowsWidth = 70;
    public const int BaseWindowsHeight = 220;
    public const int BaseIndexWindowsWidth = 100;
    public const int BaseIndexWindowsHeight = 200;
    public const int BaseDgvAddHeight = 60;
    public const int BaseDgvAddWidth = 43; //so that leftmost column (choice column?) can be shown
    public const int MinDgvControlHeight = 30;
    public const int MaxDgvControlHeight = 500;
    public const int MaxDgvControlWidth = 1000;
    public const int MaxDgvColumnWidth = 220; //this is a good limit for DGV column so that it won't be too wide
    public readonly static Size RemoveImageButtonSize = new Size(100, 40);
    public readonly static Size BrowseImageButtonSize = new Size(100, 40);
    public readonly static Size CommonIndexWindowsMaxSize = new Size(1440, 720); //actually 1550, 830 is ok, but it is too close to the page limit
    public readonly static Size CommonActionWindowsMaxSize = new Size(1440, 720); //actually 1550, 830 is ok, but it is too close to the page limit
    public readonly static Size AttachmentLabelSize = new Size(220, 40);
  }
}