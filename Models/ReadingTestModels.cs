using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;


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

    public class Question : ObservableObject
    {
        private bool _isExplanationVisible;
        
        public string Id { get; set; }
        public QuestionType Type { get; set; }
        public string QuestionText { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
        public string UserAnswer { get; set; }
        public bool IsAnswered => !string.IsNullOrEmpty(UserAnswer);
        public ObservableCollection<QuestionOptionModel> OptionModels { get; set; }
        public string Explanation { get; set; }

        public bool IsExplanationVisible
        {
            get => _isExplanationVisible;
            set => SetProperty(ref _isExplanationVisible, value);
        }

        public void InitializeOptionModels()
        {
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
