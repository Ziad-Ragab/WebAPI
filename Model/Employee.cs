using System.ComponentModel.DataAnnotations;
using WebAPIDotNet.Model;

public class Employee
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Range(18, 60)]
    public int Age { get; set; }
    public int DepartmentId { get; set; }
    public Department Department { get; set; }
}
