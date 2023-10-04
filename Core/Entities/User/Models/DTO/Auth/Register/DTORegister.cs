namespace Core.Entities.User.Models.DTO.Auth.Register
{
    public partial class DTORegister
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Password { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
