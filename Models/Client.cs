using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AonFreelancing.Models
{
    public class Client : User
    {
        public string CompanyName { get; set; }

        // a client has many projects one-to-many relationship
        public List<Project>? Projects { get; set; } = new List<Project>(); 

        public override void DisplayProfile()
        {
            Console.WriteLine($"Client display profile, Company: {CompanyName}");
        }
    }
}
