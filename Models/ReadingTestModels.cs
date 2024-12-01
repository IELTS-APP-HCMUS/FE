﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace login_full.Models
{
    public class ReadingTestDetail
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int TimeLimit { get; set; }
        public List<Question> Questions { get; set; }
        public TestProgress Progress { get; set; }
    }

    public class Question : INotifyPropertyChanged
    {
        private bool _isExplanationVisible;
        private string _userInput;

        public string Id { get; set; }
        public QuestionType Type { get; set; }
        public string QuestionText { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
        public string UserAnswer { get; set; }
        public string UserInput
        {
            get => _userInput;
            set
            {
                _userInput = value;
                OnPropertyChanged(nameof(UserInput));
                UserAnswer = value;
            }
        }
        public bool IsAnswered => !string.IsNullOrEmpty(UserAnswer);
        public bool IsCorrectAnswer => UserAnswer == CorrectAnswer;
        public ObservableCollection<QuestionOptionModel> OptionModels { get; set; }
        public string Explanation { get; set; }

        public bool IsExplanationVisible
        {
            get => _isExplanationVisible;
            set
            {
                if (_isExplanationVisible != value)
                {
                    _isExplanationVisible = value;
                    OnPropertyChanged(nameof(IsExplanationVisible));
                }
            }
        }

        public void InitializeOptionModels()
        {
            if (Type == QuestionType.GapFilling)
            {
                OptionModels = null;
                return;
            }

            OptionModels = new ObservableCollection<QuestionOptionModel>();

            foreach (var option in Options)
            {
                var optionModel = new QuestionOptionModel
                {
                    Text = option,
                    IsSelected = option == UserAnswer,
                    IsCorrect = option == CorrectAnswer,
                    IsWrong = option == UserAnswer && option != CorrectAnswer
                };

                OptionModels.Add(optionModel);
            }
        }

        public void ToggleExplanation()
        {
            IsExplanationVisible = !IsExplanationVisible;
            System.Diagnostics.Debug.WriteLine($"ToggleExplanation called. New value: {IsExplanationVisible}");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }


    public enum QuestionType
    {
        MultipleChoice,
        GapFilling,
        TrueFalseNotGiven,
        YesNoNotGiven
    }

    public class TestProgress
    {
        public int TotalQuestions { get; set; }
        public int AnsweredQuestions { get; set; }
        public int RemainingTime { get; set; }
        public bool IsCompleted { get; set; }
    }
}
