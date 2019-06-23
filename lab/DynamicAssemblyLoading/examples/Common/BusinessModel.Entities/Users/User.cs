namespace SmokeTests.BusinessModel.Entities
{
    public class User : Entity
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
    }
}