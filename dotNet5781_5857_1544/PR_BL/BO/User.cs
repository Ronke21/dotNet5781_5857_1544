namespace BO
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; } 
        public Authorization Authorization { get; set; }
        public bool Active { get; set; } //
    }
}
