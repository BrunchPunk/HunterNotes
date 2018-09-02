using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterNotes
{
    public class Material
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Material(string newName, string newDescription)
        {
            Name = newName;
            Description = newDescription;
        }
    }
}
