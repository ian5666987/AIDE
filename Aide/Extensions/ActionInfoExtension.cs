using Aibe.Models.Core;
using Aide.Models.Accounts;
using Extension.String;
using System.Linq;

namespace Aide.Extensions {
  public static class ActionInfoExtension {    
    public static bool IsAllowed(this ActionInfo actInfo, ApplicationUser user, bool isWebApi = false) {
      if (actInfo.Roles == null || actInfo.Roles.Count <= 0)
        return true;
      bool allowed = actInfo.Roles.Any(x => x.EqualsIgnoreCase(user.WorkingRole) || x.EqualsIgnoreCase(user.AdminRole));
      if (isWebApi) {
        bool appliedToMobile = actInfo.Roles.Any(x => x.EqualsIgnoreCase(Aibe.DH.MobileAppRole));
        return allowed || appliedToMobile; //the relationship here is singularly OR
      }
      return allowed;
    }
  }
}
