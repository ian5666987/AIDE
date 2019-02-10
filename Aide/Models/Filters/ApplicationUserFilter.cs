using Aibe.Models.Filters;
using System;
using System.Text;

namespace Aide.Models.Filters {
  public class ApplicationUserFilter {
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string Team { get; set; }
    public string WorkingRole { get; set; }
    public string AdminRole { get; set; }
    public DateTime? RegistrationDateFrom { get; set; } = null;
    public DateTime? RegistrationDateTo { get; set; } = null;
    public DateTime? LastLoginFrom { get; set; } = null;
    public DateTime? LastLoginTo { get; set; } = null;

    public int CreateMessage(out string msg) {
      StringBuilder sb = new StringBuilder();
      msg = string.Empty;
      int no = 0;
      if (!string.IsNullOrWhiteSpace(UserName)) {
        sb.AppendLine(string.Concat(Aibe.LCZ.T_UserNameColumnName, ": ", UserName));
        ++no;
      }
      if (!string.IsNullOrWhiteSpace(FullName)) { 
        sb.AppendLine(string.Concat(Aibe.LCZ.T_UserFullNameColumnName, ": ", FullName));
        ++no;
      }
      if (!string.IsNullOrWhiteSpace(DisplayName)) { 
        sb.AppendLine(string.Concat(Aibe.LCZ.T_UserDisplayNameColumnName, ": ", DisplayName));
        ++no;
      }
      if (!string.IsNullOrWhiteSpace(Email)) { 
        sb.AppendLine(string.Concat(Aibe.LCZ.T_UserEmailColumnName, ": ", Email));
        ++no;
      }
      if (!string.IsNullOrWhiteSpace(Team)) { 
        sb.AppendLine(string.Concat(Aibe.LCZ.T_UserTeamColumnName, ": ", Team));
        ++no;
      }
      if (!string.IsNullOrWhiteSpace(WorkingRole)) { 
        sb.AppendLine(string.Concat(Aibe.LCZ.T_UserWorkingRoleColumnName, ": ", WorkingRole));
        ++no;
      }
      if (!string.IsNullOrWhiteSpace(AdminRole)) { 
        sb.AppendLine(string.Concat(Aibe.LCZ.T_UserAdminRoleColumnName, ": ", AdminRole));
        ++no;
      }
      if (RegistrationDateFrom != null) { 
        sb.AppendLine(string.Concat(Aibe.LCZ.T_UserRegistrationDateColumnName, " (", Aibe.LCZ.W_From, ")", ": ", RegistrationDateFrom.Value.ToString(Aide.DH.DefaultDateTimeFormat)));
        ++no;
      }
      if (RegistrationDateTo != null) { 
        sb.AppendLine(string.Concat(Aibe.LCZ.T_UserRegistrationDateColumnName, " (", Aibe.LCZ.W_To, ")", ": ", RegistrationDateTo.Value.ToString(Aide.DH.DefaultDateTimeFormat)));
        ++no;
      }
      if (LastLoginFrom != null) { 
        sb.AppendLine(string.Concat(Aibe.LCZ.T_UserLastLoginColumnName, " (", Aibe.LCZ.W_From, ")", ": ", LastLoginFrom.Value.ToString(Aide.DH.DefaultDateTimeFormat)));
        ++no;
      }
      if (LastLoginTo != null) { 
        sb.AppendLine(string.Concat(Aibe.LCZ.T_UserLastLoginColumnName, " (", Aibe.LCZ.W_To, ")", ": ", LastLoginTo.Value.ToString(Aide.DH.DefaultDateTimeFormat)));
        ++no;
      }
      msg = sb.ToString();
      return no;
    }

    public bool HasFilter() {
      return !string.IsNullOrWhiteSpace(UserName) || !string.IsNullOrWhiteSpace(FullName) || !string.IsNullOrWhiteSpace(DisplayName) ||
        !string.IsNullOrWhiteSpace(Email) || !string.IsNullOrWhiteSpace(Team) || !string.IsNullOrWhiteSpace(WorkingRole) || !string.IsNullOrWhiteSpace(AdminRole) ||
        RegistrationDateFrom != null || RegistrationDateTo != null || LastLoginFrom != null || LastLoginTo != null;
    }
  }
}
