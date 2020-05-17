namespace AuthenticationApi.Models
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public UserModel UserDetails { get; set; }
        public bool Success { get; set; }  = false;   
    }
}