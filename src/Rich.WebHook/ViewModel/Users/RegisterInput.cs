namespace Rich.WebHook.Model.Users;

public class RegisterInput
{
    public RegisterInput(string userName, string passWord, string passWordTwo, string email)
    {
        UserName = userName;
        PassWord = passWord;
        PassWordTwo = passWordTwo;
        Email = email;
    }

    public string UserName { get; set; }
    public string PassWord { get; set; }
    public string PassWordTwo { get; set; }
    public string Email { get; set; }
}