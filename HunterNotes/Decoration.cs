using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterNotes
{
    public class Decoration
    {
        public string Name { get; set; }
        public string Skill { get; set; }
        public int Size { get; set; }
        public int Owned { get; set; }

        public Decoration(string newName, string newSkill, int newSize, int newOwned)
        {
            Name = newName;
            Skill = newSkill;
            Size = newSize;
            Owned = newOwned;
        }
    }
}
