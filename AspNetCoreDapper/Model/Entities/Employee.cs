namespace AspNetCoreDapper.Model.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Age { get; set; }
        public string Position { get; set; } = null!;
        public int CompanyId { get; set; }
    }
}
