using Microsoft.AspNetCore.Identity;

namespace Rich.WebHook.EntityFramework.Model;

public class UserInfo : Entity
{
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public string PassWord { get; set; }
    

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; }
}