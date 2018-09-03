using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterNotes
{
    public class Skill
    {
        public string Name { get; set; }
        public int MaxPoints { get; set; }
        public string Description { get; set; }

        public Skill(string newName, int newMaxPoints, string newDescription)
        {
            Name = newName;
            MaxPoints = newMaxPoints;
            Description = newDescription;
        }
    }
}
