using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG.Models
{
    public class Character
    {
        //int level { get; set; }
        public string name { get; set; } = string.Empty;
        //string job { get; set; }
        public float power { get; set; }
        public float defense { get; set; }
        public float hp { get; set; }
        public float speed { get; set; }
        //int gold { get; set; }

        public Character()
        {
            name = "캐릭터";
            power = 1f;
            defense = 1f;
            hp = 1f;
            speed = 1f;
        }
    }
}
