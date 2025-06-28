namespace WebAPIDotNet.Model
{
    public class Department
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Location { get; set; }
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

    }
}
