namespace Rich.WebHook.Model.Users;

public class RegisterInput
{
    public string UserName { get; set; }
    public string PassWord { get; set; }
    public string PassWordTwo { get; set; }
    public string Email { get; set; }
}