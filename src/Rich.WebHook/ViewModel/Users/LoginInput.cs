namespace Rich.WebHook.Model.Users;

public class LoginInput
{
    public LoginInput(string userName, string passWord)
    {
        UserName = userName;
        PassWord = passWord;
    }

    public string UserName { get; set; }
    public string PassWord { get; set; }
}