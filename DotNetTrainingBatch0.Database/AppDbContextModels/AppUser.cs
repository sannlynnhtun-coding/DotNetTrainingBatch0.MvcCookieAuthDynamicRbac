namespace DotNetTrainingBatch0.Database.AppDbContextModels;

public class AppUser
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public int RoleId { get; set; }
}
