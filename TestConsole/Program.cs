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
            Person person1 = new Person()
            {
                Name = "sda",
                Age = 10,
                Sex = 'M'
            };
            Person person2 = new Person()
            {
                Name = "sda",
                Age = 10,
                Sex = 'M'
            };
            Console.WriteLine(person2.Equals(person1));
            Console.Read();
        }
    }

    internal class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public char Sex { get; set; }

        public override string ToString()
        {
            return $"{this.Name}:{this.Age}:{this.Sex}";
        }
    }
}