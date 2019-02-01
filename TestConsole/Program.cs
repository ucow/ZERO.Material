using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Student s = new Student();
            Console.WriteLine(s.ToString());
            Console.Read();
        }
    }

    internal class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public char Sex { get; set; }

        public Person()
        {
            Name = "sdsad";
            Age = 12;
            Sex = 'M';
        }

        public override string ToString()
        {
            return $"{this.Name}:{this.Age}:{this.Sex}";
        }
    }

    internal class Student : Person
    {
        public string Id { get; set; }

        public Student()
        {
            Id = "1111";
        }
    }
}