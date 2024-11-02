using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace login_full
{
	internal class UserTarget
	{
		public int TargetStudyDuration { get; internal set; }
		public string NextExamDate { get; internal set; }
		public double target_reading { get; internal set; }
		public double TargetListening { get; internal set; }
		public double TargetWriting { get; internal set; }
		public double TargetSpeaking { get; internal set; }
	}
}
