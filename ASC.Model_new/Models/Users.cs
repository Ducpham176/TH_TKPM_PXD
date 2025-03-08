using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASC.Model.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Password { get; set; }
    }
}
