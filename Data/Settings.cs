using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Data
{
    public class Settings
    {
        public List<string> playerNames { get; set; }
        public string gameType { get; set; }
        public bool canSelect { get; set; } = true;
        public bool canNavigate { get; set; } = true;
        public bool canAdvance { get; set; } = true;
        public bool canNavigatePieces { get; set; } = false;
        public bool canShift { get; set; } = false;
    }
}
