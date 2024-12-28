using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace login_full.Models
{
    public class VocabularyItem : INotifyPropertyChanged
    {
        private int _index;
        private string _word;
        private string _status;
        private string _wordType;
        private string _meaning;
        private string _example;
        private string _note;

        public int Index
        {
            get => _index;
            set
            {
                _index = value;
                OnPropertyChanged();
            }
        }

        public string Word
        {
            get => _word;
            set
            {
                _word = value;
                OnPropertyChanged();
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(StatusColor));
            }
        }
        [JsonIgnore]
        public SolidColorBrush StatusColor
        {
            get => Status == "Đã học" ?
                new SolidColorBrush(Colors.Green) :
                new SolidColorBrush(Colors.Red);
        }
        [JsonPropertyName("type")]
        public string WordType
        {
            get => _wordType;
            set
            {
                _wordType = value;
                OnPropertyChanged();
            }
        }

        public string Meaning
        {
            get => _meaning;
            set
            {
                _meaning = value;
                OnPropertyChanged();
            }
        }

        public string Example
        {
            get => _example;
            set
            {
                _example = value;
                OnPropertyChanged();
            }
        }

        public string Note
        {
            get => _note;
            set
            {
                _note = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
