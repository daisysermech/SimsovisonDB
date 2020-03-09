using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SimsovisionDataBase.Models
{
    public class User:IdentityUser
    {
        public DateTime DateofBirth { get; set; }
    }
}
