using Aide.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Aide.Helpers {
  public class AideUserHelper {
    public static Dictionary<string, object> GetUserParameters(ApplicationUser user, string prefix) {
      Type type = user.GetType();
      PropertyInfo[] properties = type.GetProperties(); //TODO check how to exclude some parameters by using reflection/attributes
      Dictionary<string, object> result = new Dictionary<string, object>();
      foreach (PropertyInfo property in properties) {
        if (defaultExcludedPropertyNames.Contains(property.Name)) //case insensitive
          continue;
        result.Add(prefix + property.Name, property.GetValue(user, null));
      }
      return result;
    }

    private static List<string> defaultExcludedPropertyNames = new List<string> {
      //"Claims", "Logins", "Roles", //all these are collection
      //"SecurityStamp",
      "PasswordHash", "Id", //all these are not to be displayed or used
    };
  }
}
