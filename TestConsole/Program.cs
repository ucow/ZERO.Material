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
            List<Person> persons = new List<Person>();
            for (int i = 0; i < 10; i++)
            {
                Person person = new Person()
                {
                    Name = "sda" + i.ToString(),
                    Age = i * 10,
                    Sex = i % 2 == 0 ? 'N' : 'M'
                };
                persons.Add(person);
            };

            var p = from a in persons where a.Age > 50 select a.Name + a.Sex;

            foreach (var item in persons)
            {
                System.Console.WriteLine(item.ToString());
            }
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