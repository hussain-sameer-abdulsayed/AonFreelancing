﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AonFreelancing.Models
{
    public class Freelancer : User
    {

        public string Skills { get; set; }

        public List<Project> Projects { get; set; }
        
    }


}
