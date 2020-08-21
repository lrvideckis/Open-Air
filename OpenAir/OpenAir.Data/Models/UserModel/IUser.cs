namespace OpenAir.Data.Models
{
    public interface IUser
    {
        public string EmailId { get; set; }
        public string Name { get; set; }
        public string Pwd { get; set; }
        public string Aircraft { get; set; }
        public bool IsAdmin { get; set; }
    }
}
