using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using ZERO.Material.Model;

namespace TestConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ZERO_MaterialEntities2 context = new ZERO_MaterialEntities2();
            foreach (var action in context.Material_Action)
            {
                string url = action.Action_Url;
                if (url != null)
                {
                    action.Action_Url = url.Replace("\\", "/");
                    context.Material_Action.Attach(action);
                    context.Entry(action).State = EntityState.Modified;
                }
 ;
            }

            context.SaveChanges();
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