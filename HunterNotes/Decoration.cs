using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterNotes
{
    public class Decoration
    {
        string Name { get; set; }
        string Skill { get; set; }
        int Size { get; set; }
        int Owned { get; set; }

        public Decoration(string newName, string newSkill, int newSize, int newOwned)
        {
            Name = newName;
            Skill = newSkill;
            Size = newSize;
            Owned = newOwned;
        }
    }
}
