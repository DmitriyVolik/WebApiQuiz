using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WebApiQuiz.Models.Options;

public class AuthOptions
{
   
    public const string ISSUER = "MyAuthServer"; // издатель токена
    
    public const string AUDIENCE = "MyAuthClient"; // потребитель токена
    
    const string KEY = "qgtdG67!*";   // ключ для шифрации
    
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => 
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    
}