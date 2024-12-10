namespace PCRobotApp.Utils;

public class AccessControl {
  private readonly string _usersFile = "users.txt";

  public AccessControl() {
    LoadUsers();
  }

  public string AdminId { get; private set; }
  public HashSet<string> AllowedUsers { get; private set; }

  private void LoadUsers() {
    if (!File.Exists(_usersFile)) File.Create(_usersFile).Close();

    var lines = File.ReadAllLines(_usersFile);
    if (lines.Length > 0 && !string.IsNullOrWhiteSpace(lines[0])) AdminId = lines[0].Trim();

    AllowedUsers = new HashSet<string>();
    for (var i = 1; i < lines.Length; i++)
      if (!string.IsNullOrWhiteSpace(lines[i]))
        AllowedUsers.Add(lines[i].Trim());
  }

  public void SetAdmin(string userId) {
    AdminId = userId;
    SaveUsers();
  }

  public void AddAllowedUser(string userId) {
    if (!AllowedUsers.Contains(userId)) {
      AllowedUsers.Add(userId);
      SaveUsers();
    }
  }

  private void SaveUsers() {
    using (var writer = new StreamWriter(_usersFile, false)) {
      writer.WriteLine(AdminId);
      foreach (var user in AllowedUsers) writer.WriteLine(user);
    }
  }

  public bool IsAdmin(string userId) {
    return AdminId == userId;
  }

  public bool IsAllowedUser(string userId) {
    return IsAdmin(userId) || AllowedUsers.Contains(userId);
  }
}