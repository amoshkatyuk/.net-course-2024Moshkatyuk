﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Domain.Models
{
    public class Employee : Person
    {
        public string Position { get; set; }
        public int Salary { get; set; }
        public string Contract { get; set; }

        public Employee(string name, string surname, string passportData, string position)
            : base(name, surname, passportData)
        {
            Position = position;
        }
    }
}
