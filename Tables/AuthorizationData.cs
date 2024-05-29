namespace Kursach.Tables
{
    public class AuthorizationData
    {
        public AuthorizationData() { }
        public AuthorizationData(int id, string name, string pass)
        {
            UserName = name; Pass = pass; UserId = id;
        }
        public int UserId { get; set; }

        public string? UserName { get; set; }

        public string? Pass { get; set; }
    }
}