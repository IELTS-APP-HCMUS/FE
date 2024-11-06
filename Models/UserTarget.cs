using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace login_full.Models
{
    public class UserTarget
    {
        public int TargetStudyDuration { get; set; }
        public string NextExamDate { get; set; }
        public double TargetReading { get; set; }
        public double TargetListening { get; set; }
        public double TargetWriting { get; set; }
        public double TargetSpeaking { get; set; }
    }
}
