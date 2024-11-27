using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public class Question
    {
        public string Id { get; set; }
        public QuestionType Type { get; set; }
        public string QuestionText { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
        public string UserAnswer { get; set; }
        public bool IsAnswered => !string.IsNullOrEmpty(UserAnswer);
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
