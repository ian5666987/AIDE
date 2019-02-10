using System;

namespace Aide.Models.Accounts {
  public class ApplicationUser {
    public string FullName { get; set; }
    public string DisplayName { get; set; }
    public string WorkingRole { get; set; }
    public string AdminRole { get; set; }
    public string Team { get; set; }
    public DateTime RegistrationDate { get; set; } = new DateTime(1999, 12, 31, 23, 59, 59); //when the user is registered, then it has its registration date
    public DateTime? LastLogin { get; set; } = new DateTime(1999, 12, 31, 23, 59, 59); //To check when was the last login time

    //---Specifics to Aide
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
  }

  public class UserRoleModel {
    public string UserId { get; set; }
    public string RoleId { get; set; }
  }

  public class RoleModel {
    public string Id { get; set; }
    public string Name { get; set; }
  }

  public class TeamModel {
    public int Id { get; set; }
    public string Name { get; set; }
  }
}
