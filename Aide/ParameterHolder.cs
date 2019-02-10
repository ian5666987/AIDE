namespace Aide {
  public class PH { //can be extended as wanted
    //Formats
    public static string IndexDateTimeFormat = Aide.DH.DefaultDateTimeFormat;
    public static string CreateEditFilterDateTimeFormat = Aide.DH.DefaultDateTimeFormat;
    public static string DetailsDateTimeFormat = Aide.DH.DefaultDateTimeFormat;
    public static string ScTableDateTimeFormat = Aide.DH.DefaultDateTimeFormat;
    public static string CsvDateTimeFormat = Aide.DH.DefaultDateTimeFormat;

    //Table
    public static string TableModelClassPrefix = Aide.DH.DefaultTableModelClassPrefix;
    public static string UserTableName = "Users";
    public static string RoleTableName = "Roles";
    public static string TeamTableName = "Teams";
    public static string UserRoleTableName = "UserRoles";

    //Apps
    public static bool UseStrongCheck = true; //by default, use strong check unless stated otherwise
    public static bool isTagChecked = false; //by default, no need to check the tag

    //Attachment path
    public static string AttachmentImageIconFileName = "attachment.png";

  }
}