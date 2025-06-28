using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIDotNet.DTOS;
using WebAPIDotNet.Model;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly ITIContext context;

    public EmployeeController(ITIContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {

        var empFromDB = context.Employees
            .Include(e => e.Department)
            .Select(e => new EmployeeDTO
            {
                Name = e.Name,
                Age = e.Age,
                DepartmentId = e.DepartmentId
            })
            .ToList();
        return Ok(empFromDB);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var emp = context.Employees
            .Where(e => e.Id == id)
            .Select(e => new EmployeeDTO
            {
                Name = e.Name,
                Age = e.Age,
                DepartmentId = e.DepartmentId
            })
            .FirstOrDefault();
        if (emp == null)
            return NotFound();

        return Ok(emp);
    }

    [HttpGet("name/{name}")]
    public IActionResult GetByName(string name)
    {
        var dept = context.Employees
            .Where(e => e.Name.ToLower() == name.ToLower())
            .Select(e => new EmployeeDTO
            {
                Name = e.Name,
                Age = e.Age,
                DepartmentId = e.DepartmentId
            })
            .FirstOrDefault();
        if (dept == null)
            return NotFound();

        return Ok(dept);
    }


    [HttpPost]
    public IActionResult Add(EmployeeDTO dto)
    {
        var emp = new Employee
        {
            Name = dto.Name,
            Age = dto.Age,
            DepartmentId = dto.DepartmentId
        };
        context.Employees
            .Add(emp);
        context.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = emp.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public IActionResult Put(int id, EmployeeDTO newEmp)
    {
        var oldEmp = context.Employees
            .Where(e => e.Id == id)
            .FirstOrDefault();

        if (oldEmp == null)
            return NotFound();

        oldEmp.Name = newEmp.Name;
        oldEmp.Age = newEmp.Age;
        oldEmp.DepartmentId = newEmp.DepartmentId;

        context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var emp = context.Employees.Find(id);
        if (emp == null)
            return NotFound();

        context.Employees.Remove(emp);
        context.SaveChanges();
        return NoContent();
    }
}
