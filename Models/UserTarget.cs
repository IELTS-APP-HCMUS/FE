using System.ComponentModel;

namespace login_full.Models
{
    public class UserTarget : INotifyPropertyChanged
	{
        private int _targetStudyDuration;
        private string _nextExamDate;
		private double _targetReading;
		private double _targetListening;
		private double _targetWriting;
		private double _targetSpeaking;

		// constructor
		public UserTarget()
		{
			_targetStudyDuration = 0;
			_nextExamDate = "- / - / -";
			_targetReading = -1;
			_targetListening = -1;
			_targetWriting = -1;
			_targetSpeaking = -1;
		}

		public int TargetStudyDuration
		{
			get => _targetStudyDuration;
			set
			{
				if (_targetStudyDuration != value)
				{
					_targetStudyDuration = value;
					OnPropertyChanged(nameof(TargetStudyDuration));
				}
			}
		}
		public string NextExamDate
		{
			get => _nextExamDate;
			set
			{
				if (_nextExamDate != value)
				{
					_nextExamDate = value;
					OnPropertyChanged(nameof(NextExamDate));
				}
			}
		}
		public double TargetReading
		{
			get => _targetReading;
			set
			{
				if (_targetReading != value)
				{
					_targetReading = value;
					OnPropertyChanged(nameof(TargetReading));
				}
			}
		}
		public double TargetListening
		{
			get => _targetListening;
			set
			{
				if (_targetListening != value)
				{
					_targetListening = value;
					OnPropertyChanged(nameof(TargetListening));
				}
			}
		}
		public double TargetWriting
		{
			get => _targetWriting;
			set
			{
				if (_targetWriting != value)
				{
					_targetWriting = value;
					OnPropertyChanged(nameof(TargetWriting));
				}
			}
		}
		public double TargetSpeaking
		{
			get => _targetSpeaking;
			set
			{
				if (_targetSpeaking != value)
				{
					_targetSpeaking = value;
					OnPropertyChanged(nameof(TargetSpeaking));
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
