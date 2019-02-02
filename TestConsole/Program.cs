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
            ZERO_MaterialEntities context = new ZERO_MaterialEntities();
            foreach (Apply_Info applyInfo in context.Apply_Info)
            {
                Console.WriteLine(applyInfo.Apply_Id);
            }

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