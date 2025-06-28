using System.ComponentModel.DataAnnotations;
using WebAPIDotNet.Model;

namespace WebAPIDotNet.DTOS
{
    public class EmployeeDTO
    {
        [Required]
        public string Name { get; set; }

        [Range(18,65)]
        public int Age { get; set; }
        
        [Required]
        public int DepartmentId { get; set; }

    }
}
