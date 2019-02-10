using System.Collections.Generic;

namespace Aide {
  public class DHObscured { //Change "DHObscured" to "DH" to make the project works
    //User related global items
    public const string DevEmail = "fakedevemail@fakedomain.com"; //change this with your actual email address
    public const string MainAdminName = "admin";
    public const string MainAdminPass = "fakepassword"; //change this with your actual default password
    public const string MainAdminEmail = "fakeadminemail@fakedomain.com"; //change this with your actual email address
    public const string AdminAuthorizedRoles = Aibe.DH.AdminRole + "," + Aibe.DH.MainAdminRole + "," + Aibe.DH.DevRole;
    //public readonly static List<string> WorkingRoles = new List<string> { "User" }; Not used anywhere
    public readonly static List<string> DeveloperNames = new List<string> { Aibe.DH.DevName };

    //Constants
    public const string ListColumnAddDeleteButtonColumnName = "ListColumnAddDeleteActionColumn";
    public const string ListColumnCopyButtonColumnName = "ListColumnCopyActionColumn";
    public const string LiveDropDownTag = "live-dd";
    public const string TableNameColumnName = "TableName";

    //Aide specifics
    public const string Aide = "Aide";
    public const string CommonLogicName = "CommonLogic";
    public const string GetRequest = "GET"; //to distinguish Get from Post requests, following web-site standard and AIWE method. The other requests may not be needed
    public const string PostRequest = "POST"; //to distinguish Get from Post requests, following web-site standard and AIWE method. The other requests may not be needed

    //Special items
    public readonly static List<string> OnlyAccessCheckingActions = new List<string> {
      Aibe.DH.IndexActionName,      
    };

    //Formats
    public const string DefaultDateTimeFormat = "dd-MMM-yyyy HH:mm:ss";
    public const string DefaultPageName = Aibe.DH.IndexPageName;

    //Table
    public const string DefaultTableModelClassPrefix = "Aide.Models.DB.";
  }
}