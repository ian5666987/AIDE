using Aide.Logics;
using System.Collections.Generic;
using System.Linq;

namespace Aide.Models.Accounts {
  public class Identity {
    public static ApplicationUser User { get; set; } //To be used and kept when logged-in in the Aide System
    public static List<string> Roles { get; set; } //should be started with null if not available

    public static bool IsDeveloper() {
      if (User == null || string.IsNullOrWhiteSpace(User.Id))
        return false;
      if (Roles != null)
        return Roles.Any(x => x.Equals(Aibe.DH.DevRole));
      var roles = UserLogic.GetRoles(User.Id);
      return roles.Any(x => x.Equals(Aibe.DH.DevRole));
    }

    public static bool IsMainAdmin() {
      if (User == null || string.IsNullOrWhiteSpace(User.Id))
        return false;
      if (Roles != null)
        return Roles.Any(x => Aibe.DH.MainAdminRoles.Any(y => y.Equals(x)));
      var roles = UserLogic.GetRoles(User.Id);
      return roles.Any(x => Aibe.DH.MainAdminRoles.Any(y => y.Equals(x)));
    }

    public static bool IsAdmin() {
      if (User == null || string.IsNullOrWhiteSpace(User.Id))
        return false;
      if (Roles != null)
        return Roles.Any(x => Aibe.DH.AdminRoles.Any(y => y.Equals(x)));
      var roles = UserLogic.GetRoles(User.Id);
      return roles.Any(x => Aibe.DH.AdminRoles.Any(y => y.Equals(x)));
    }
  }
}
