﻿namespace WebAPIDotNet.Model
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? InstructorId { get; set; }
        public Instructor? Instructor { get; set; }
    }

}
