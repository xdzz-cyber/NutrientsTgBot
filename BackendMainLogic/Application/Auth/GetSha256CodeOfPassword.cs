using System.Security.Cryptography;
using System.Text;
using Application.Interfaces;

namespace Application.Auth;

public class GetSha256CodeOfPassword : IGetHashCodeOfString
{
    public string GetHashCodeOfString(string s)
    {
        using (var sha256Hash = SHA256.Create())  
        {  
            // ComputeHash - returns byte array  
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(s));  
  
            // Convert byte array to a string   
            var builder = new StringBuilder();  
            foreach (var t in bytes)
            {
                builder.Append(t.ToString("x2"));
            }  
            return builder.ToString();  
        }
    }
}
