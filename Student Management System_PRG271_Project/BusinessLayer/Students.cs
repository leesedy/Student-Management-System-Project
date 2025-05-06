using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Management_System.BusinessLayer
{
    internal class Students
    {
        //fields 
        private int studentID;
        private string name;
        private string surname;
        private int age;
        private string course;

        public Students() { }

        //encapsulation
        public Students(int studentID, string name, string surname, int age, string course)
        {
            this.StudentID = studentID;
            this.Name = name;
            this.Surname = surname;
            this.Age = age;
            this.Course = course;
        }

        //encapsulation
        public int StudentID { get => studentID; set => studentID = value; }
        public string Name { get => name; set => name = value; }
        public string Surname { get => surname; set => surname = value; }
        public int Age { get => age; set => age = value; }
        public string Course { get => course; set => course = value; }
    }
}
