﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI.Reminder.BL
{
    public class Prompt
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime CreatingDate { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        //public DateTime PromptTime { get; set; }


    }
}