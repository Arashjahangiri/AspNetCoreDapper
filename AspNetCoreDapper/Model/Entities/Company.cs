namespace AspNetCoreDapper.Model.Entities
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Country { get; set; } = null!;
        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}
