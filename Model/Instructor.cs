﻿namespace WebAPIDotNet.Model
{
    public class Instructor
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Course> Courses { get; set; }
    }

}
