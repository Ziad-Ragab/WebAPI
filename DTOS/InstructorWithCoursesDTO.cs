namespace WebAPIDotNet.DTO
{
    public class InstructorWithCoursesDTO
    {
        public int Id { get; set; }

        public string InstructorName { get; set; }
        public List<string> CourseNames { get; set; }
    }
}
