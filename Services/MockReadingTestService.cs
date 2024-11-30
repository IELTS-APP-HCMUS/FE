using login_full.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace login_full.Services
{
    public class MockReadingTestService : IReadingTestService
    {
        private readonly Dictionary<string, ReadingTestDetail> _mockTests;

        public MockReadingTestService()
        {
            _mockTests = new Dictionary<string, ReadingTestDetail>
            {
                {
                    "test1", new ReadingTestDetail
                    {
                        Id = "test1",
                        Title = "Alfred Wegener: science, exploration and the theory of continental drift",
                        Content = @"This is a book about the life and scientific work of Alfred Wegener, whose reputation today rests with his theory of continental displacement, better known as continental drift...",
                        TimeLimit = 10,
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                Id = "q1",
                                Type = QuestionType.YesNoNotGiven,
                                QuestionText = "Wegener's ideas about continental drift were widely disputed while he was alive.",
                                Options = new List<string> { "YES", "NO", "NOT GIVEN" },
                                CorrectAnswer = "YES",
                                Explanation = "Đáp án đúng là YES vì Wegener's ideas about continental drift were widely disputed while he was alive. Trong đoạn văn có đề cập: \"Wegener's ideas about continental drift were widely disputed while he was alive.\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q2",
                                Type = QuestionType.YesNoNotGiven,
                                QuestionText = "Wegener was a geologist by profession.",
                                Options = new List<string> { "YES", "NO", "NOT GIVEN" },
                                CorrectAnswer = "NOT GIVEN",
                                Explanation = "Đáp án đúng là NOT GIVEN vì Wegener was not a geologist by profession. Trong đoạn văn không có đề cập: \"Wegener was a geologist by profession.\"",
                                IsExplanationVisible = false
                            }
                        },
                        Progress = new TestProgress
                        {
                            TotalQuestions = 2,
                            AnsweredQuestions = 0,
                            RemainingTime = 10*60,
                            IsCompleted = true
                        }
                    }
                },
                {
                    "test2", new ReadingTestDetail
                    {
                        Id = "test2",
                        Title = "The History of the Silk Road",
                        Content = @"The Silk Road, an ancient trade route, connected the East and the West, facilitating not only trade but also cultural exchanges...",
                        TimeLimit = 50,
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                Id = "q1",
                                Type = QuestionType.MultipleChoice,
                                QuestionText = "Which countries were connected by the Silk Road?",
                                Options = new List<string> { "China and India", "China and Europe", "India and Africa", "Europe and Africa" },
                                CorrectAnswer = "China and Europe",
                                Explanation = "Đáp án đúng là China and Europe vì The Silk Road connected the East and the West, facilitating not only trade but also cultural exchanges. Trong đoạn văn có đề cập: \"The Silk Road, an ancient trade route, connected the East and the West, facilitating not only trade but also cultural exchanges...\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q2",
                                Type = QuestionType.MultipleChoice,
                                QuestionText = "What was the primary trade item on the Silk Road?",
                                Options = new List<string> { "Spices", "Gold", "Silk", "Tea" },
                                CorrectAnswer = "Silk",
                                Explanation = "Đáp án đúng là Silk vì The Silk Road was primarily used for trading goods. Trong đoạn văn có đề cập: \"The Silk Road was primarily used for trading goods.\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q3",
                                Type = QuestionType.TrueFalseNotGiven,
                                QuestionText = "The Silk Road was only used for trading goods.",
                                Options = new List<string> { "TRUE", "FALSE", "NOT GIVEN" },
                                CorrectAnswer = "FALSE",
                                Explanation = "Đáp án đúng là FALSE vì The Silk Road was not only used for trading goods. Trong đoạn văn có đề cập: \"The Silk Road was primarily used for trading goods.\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q4",
                                Type = QuestionType.TrueFalseNotGiven,
                                QuestionText = "The Silk Road was a single, continuous route.",
                                Options = new List<string> { "TRUE", "FALSE", "NOT GIVEN" },
                                CorrectAnswer = "FALSE",
                                Explanation = "Đáp án đúng là FALSE vì The Silk Road was not a single, continuous route. Trong đoạn văn có đề cập: \"The Silk Road was primarily used for trading goods.\"",
                                IsExplanationVisible = false
                            }
                        },
                        Progress = new TestProgress
                        {
                            TotalQuestions = 4,
                            AnsweredQuestions = 0,
                            RemainingTime = 50 * 6  ,
                            IsCompleted = false
                        }
                    }
                },
                {
                    "test3", new ReadingTestDetail
                    {
                        Id = "test3",
                        Title = "The Benefits of Urban Green Spaces",
                        Content = @"Green spaces in urban areas provide numerous benefits, including reducing air pollution, improving mental health, and fostering community interaction...",
                        TimeLimit = 15,
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                Id = "q1",
                                Type = QuestionType.GapFilling,
                                QuestionText = "Green spaces help to improve ____ by reducing air pollution.",
                                Options = new List<string> { "air quality", "water quality", "noise levels", "traffic flow" },
                                CorrectAnswer = "air quality",
                                Explanation = "Đáp án đúng là air quality vì Green spaces help to improve air quality by reducing air pollution. Trong đoạn văn có đề cập: \"Green spaces help to improve air quality by reducing air pollution.\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q2",
                                Type = QuestionType.GapFilling,
                                QuestionText = "Urban parks often provide opportunities for ____ activities.",
                                Options = new List<string> { "cultural", "commercial", "educational", "physical" },
                                CorrectAnswer = "physical",
                                Explanation = "Đáp án đúng là physical vì Urban parks often provide opportunities for physical activities. Trong đoạn văn có đề cập: \"Urban parks often provide opportunities for physical activities.\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q3",
                                Type = QuestionType.TrueFalseNotGiven,
                                QuestionText = "Green spaces increase property values in nearby areas.",
                                Options = new List<string> { "TRUE", "FALSE", "NOT GIVEN" },
                                CorrectAnswer = "TRUE",
                                Explanation = "Đáp án đúng là TRUE vì Green spaces increase property values in nearby areas. Trong đoạn văn có đề cập: \"Green spaces increase property values in nearby areas.\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q4",
                                Type = QuestionType.YesNoNotGiven,
                                QuestionText = "All urban green spaces are publicly owned.",
                                Options = new List<string> { "YES", "NO", "NOT GIVEN" },
                                CorrectAnswer = "NO",
                                Explanation = "Đáp án đúng là NO vì Not all urban green spaces are publicly owned. Trong đoạn văn có đề cập: \"Not all urban green spaces are publicly owned.\"",
                                IsExplanationVisible = false
                            }
                        },
                        Progress = new TestProgress
                        {
                            TotalQuestions = 4,
                            AnsweredQuestions = 0,
                            RemainingTime = 2700,
                            IsCompleted = false
                        }
                    }
                },
                {
                    "test4", new ReadingTestDetail
                    {
                        Id = "test4",
                        Title = "The Evolution of Artificial Intelligence",
                        Content = @"Artificial Intelligence (AI) has evolved significantly since its inception in the 1950s. The field began with simple ____ systems that could perform basic calculations. Today, AI encompasses machine learning, neural networks, and deep learning technologies...",
                        TimeLimit = 45,
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                Id = "q1",
                                Type = QuestionType.GapFilling,
                                QuestionText = "AI started with ____ systems in the 1950s.",
                                Options = new List<string> { "computing", "thinking", "learning", "programming" },
                                CorrectAnswer = "computing",
                                Explanation = "Đáp án đúng là computing vì AI started with computing systems in the 1950s. Trong đoạn văn có đề cập: \"AI started with computing systems in the 1950s.\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q2",
                                Type = QuestionType.GapFilling,
                                QuestionText = "Modern AI uses ____ networks for complex tasks.",
                                Options = new List<string> { "digital", "neural", "computer", "social" },
                                CorrectAnswer = "neural",
                                Explanation = "Đáp án đúng là neural vì Modern AI uses neural networks for complex tasks. Trong đoạn văn có đề cập: \"Modern AI uses neural networks for complex tasks.\"",
                                IsExplanationVisible = false
                            }
                        },
                        Progress = new TestProgress
                        {
                            TotalQuestions = 2,
                            AnsweredQuestions = 0,
                            RemainingTime = 2700,
                            IsCompleted = false
                        }
                    }
                },
                
                {
                    "test6", new ReadingTestDetail
                    {
                        Id = "test6",
                        Title = "Renewable Energy Sources",
                        Content = @"Renewable energy sources are becoming increasingly important in the fight against climate change. Solar ____ converts sunlight directly into electricity, while wind ____ harness the power of moving air. These technologies are becoming more efficient and cost-effective...",
                        TimeLimit = 10,
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                Id = "q1",
                                Type = QuestionType.GapFilling,
                                QuestionText = "Solar ____ converts sunlight to electricity.",
                                Options = new List<string> { "panels", "cells", "batteries", "systems" },
                                CorrectAnswer = "panels",
                                Explanation = "Đáp án đúng là panels vì Solar panels convert sunlight to electricity. Trong đoạn văn có đề cập: \"Solar panels convert sunlight to electricity.\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q2",
                                Type = QuestionType.GapFilling,
                                QuestionText = "Wind ____ generate power from moving air.",
                                Options = new List<string> { "mills", "turbines", "engines", "generators" },
                                CorrectAnswer = "turbines",
                                Explanation = "Đáp án đúng là turbines vì Wind turbines generate power from moving air. Trong đoạn văn có đề cập: \"Wind turbines generate power from moving air.\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q3",
                                Type = QuestionType.GapFilling,
                                QuestionText = "These technologies are becoming more ____.",
                                Options = new List<string> { "expensive", "complex", "efficient", "popular" },
                                CorrectAnswer = "efficient",
                                Explanation = "Đáp án đúng là efficient vì These technologies are becoming more efficient. Trong đoạn văn có đề cập: \"These technologies are becoming more efficient.\"",
                                IsExplanationVisible = false
                            }
                        },
                        Progress = new TestProgress
                        {
                            TotalQuestions = 3,
                            AnsweredQuestions = 0,
                            RemainingTime = 10*60,
                            IsCompleted = false
                        }
                    }
                },
                                {
                    "test7", new ReadingTestDetail
                    {
                        Id = "test6",
                        Title = "Renewable Energy Sources",
                        Content = @"Renewable energy sources are becoming increasingly important in the fight against climate change. Solar ____ converts sunlight directly into electricity, while wind ____ harness the power of moving air. These technologies are becoming more efficient and cost-effective...",
                        TimeLimit = 10,
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                Id = "q1",
                                Type = QuestionType.GapFilling,
                                QuestionText = "Solar ____ converts sunlight to electricity.",
                                Options = new List<string> { "panels", "cells", "batteries", "systems" },
                                CorrectAnswer = "panels",
                                Explanation = "Đáp án đúng là panels vì Solar panels convert sunlight to electricity. Trong đoạn văn có đề cập: \"Solar panels convert sunlight to electricity.\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q2",
                                Type = QuestionType.GapFilling,
                                QuestionText = "Wind ____ generate power from moving air.",
                                Options = new List<string> { "mills", "turbines", "engines", "generators" },
                                CorrectAnswer = "turbines",
                                Explanation = "Đáp án đúng là turbines vì Wind turbines generate power from moving air. Trong đoạn văn có đề cập: \"Wind turbines generate power from moving air.\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q3",
                                Type = QuestionType.GapFilling,
                                QuestionText = "These technologies are becoming more ____.",
                                Options = new List<string> { "expensive", "complex", "efficient", "popular" },
                                CorrectAnswer = "efficient",
                                Explanation = "Đáp án đúng là efficient vì These technologies are becoming more efficient. Trong đoạn văn có đề cập: \"These technologies are becoming more efficient.\"",
                                IsExplanationVisible = false
                            }
                        },
                        Progress = new TestProgress
                        {
                            TotalQuestions = 3,
                            AnsweredQuestions = 0,
                            RemainingTime = 10*60,
                            IsCompleted = false
                        }
                    }
                },
                                {
                    "test8", new ReadingTestDetail
                    {
                        Id = "test8",
                        Title = "Renewable Energy Sources",
                        Content = @"Renewable energy sources are becoming increasingly important in the fight against climate change. Solar ____ converts sunlight directly into electricity, while wind ____ harness the power of moving air. These technologies are becoming more efficient and cost-effective...",
                        TimeLimit = 5,
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                Id = "q1",
                                Type = QuestionType.GapFilling,
                                QuestionText = "Solar ____ converts sunlight to electricity.",
                                Options = new List<string> { "panels", "cells", "batteries", "systems" },
                                CorrectAnswer = "panels",
                                Explanation = "Đáp án đúng là panels vì Solar panels convert sunlight to electricity. Trong đoạn văn có đề cập: \"Solar panels convert sunlight to electricity.\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q2",
                                Type = QuestionType.GapFilling,
                                QuestionText = "Wind ____ generate power from moving air.",
                                Options = new List<string> { "mills", "turbines", "engines", "generators" },
                                CorrectAnswer = "turbines",
                                Explanation = "Đáp án đúng là turbines vì Wind turbines generate power from moving air. Trong đoạn văn có đề cập: \"Wind turbines generate power from moving air.\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q3",
                                Type = QuestionType.GapFilling,
                                QuestionText = "These technologies are becoming more ____.",
                                Options = new List<string> { "expensive", "complex", "efficient", "popular" },
                                CorrectAnswer = "efficient",
                                Explanation = "Đáp án đúng là efficient vì These technologies are becoming more efficient. Trong đoạn văn có đề cập: \"These technologies are becoming more efficient.\"",
                                IsExplanationVisible = false
                            }
                        },
                        Progress = new TestProgress
                        {
                            TotalQuestions = 3,
                            AnsweredQuestions = 0,
                            RemainingTime = 5 * 60,
                            IsCompleted = false
                        }
                    }
                },
                                {
                    "test9", new ReadingTestDetail
                    {
                        Id = "test9",
                        Title = "Renewable Energy Sources",
                        Content = @"Renewable energy sources are becoming increasingly important in the fight against climate change. Solar ____ converts sunlight directly into electricity, while wind ____ harness the power of moving air. These technologies are becoming more efficient and cost-effective...",
                        TimeLimit = 50,
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                Id = "q1",
                                Type = QuestionType.GapFilling,
                                QuestionText = "Solar ____ converts sunlight to electricity.",
                                Options = new List<string> { "panels", "cells", "batteries", "systems" },
                                CorrectAnswer = "panels",
                                Explanation = "Đáp án đúng là panels vì Solar panels convert sunlight to electricity. Trong đoạn văn có đề cập: \"Solar panels convert sunlight to electricity.\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q2",
                                Type = QuestionType.GapFilling,
                                QuestionText = "Wind ____ generate power from moving air.",
                                Options = new List<string> { "mills", "turbines", "engines", "generators" },
                                CorrectAnswer = "turbines",
                                Explanation = "Đáp án đúng là turbines vì Wind turbines generate power from moving air. Trong đoạn văn có đề cập: \"Wind turbines generate power from moving air.\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q3",
                                Type = QuestionType.GapFilling,
                                QuestionText = "These technologies are becoming more ____.",
                                Options = new List<string> { "expensive", "complex", "efficient", "popular" },
                                CorrectAnswer = "efficient",
                                Explanation = "Đáp án đúng là efficient vì These technologies are becoming more efficient. Trong đoạn văn có đề cập: \"These technologies are becoming more efficient.\"",
                                IsExplanationVisible = false
                            }
                        },
                        Progress = new TestProgress
                        {
                            TotalQuestions = 3,
                            AnsweredQuestions = 0,
                            RemainingTime = 50 * 60,
                            IsCompleted = false
                        }
                    }
                },
                                {
                    "test10", new ReadingTestDetail
                    {
                        Id = "test10",
                        Title = "Renewable Energy Sources",
                        Content = @"Renewable energy sources are becoming increasingly important in the fight against climate change. Solar ____ converts sunlight directly into electricity, while wind ____ harness the power of moving air. These technologies are becoming more efficient and cost-effective...",
                        TimeLimit = 50,
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                Id = "q1",
                                Type = QuestionType.GapFilling,
                                QuestionText = "Solar ____ converts sunlight to electricity.",
                                Options = new List<string> { "panels", "cells", "batteries", "systems" },
                                CorrectAnswer = "panels",
                                Explanation = "Đáp án đúng là panels vì Solar panels convert sunlight to electricity. Trong đoạn văn có đề cập: \"Solar panels convert sunlight to electricity.\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q2",
                                Type = QuestionType.GapFilling,
                                QuestionText = "Wind ____ generate power from moving air.",
                                Options = new List<string> { "mills", "turbines", "engines", "generators" },
                                CorrectAnswer = "turbines",
                                Explanation = "Đáp án đúng là turbines vì Wind turbines generate power from moving air. Trong đoạn văn có đề cập: \"Wind turbines generate power from moving air.\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q3",
                                Type = QuestionType.GapFilling,
                                QuestionText = "These technologies are becoming more ____.",
                                Options = new List<string> { "expensive", "complex", "efficient", "popular" },
                                CorrectAnswer = "efficient",
                                Explanation = "Đáp án đúng là efficient vì These technologies are becoming more efficient. Trong đoạn văn có đề cập: \"These technologies are becoming more efficient.\"",
                                IsExplanationVisible = false
                            }
                        },
                        Progress = new TestProgress
                        {
                            TotalQuestions = 3,
                            AnsweredQuestions = 0,
                            RemainingTime = 3000,
                            IsCompleted = false
                        }
                    }
                },
                             
                                {
                    "test11", new ReadingTestDetail
                    {
                        Id = "test11",
                        Title = "Renewable Energy Sources",
                        Content = @"Renewable energy sources are becoming increasingly important in the fight against climate change. Solar ____ converts sunlight directly into electricity, while wind ____ harness the power of moving air. These technologies are becoming more efficient and cost-effective...",
                        TimeLimit = 20,
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                Id = "q1",
                                Type = QuestionType.GapFilling,
                                QuestionText = "Solar ____ converts sunlight to electricity.",
                                Options = new List<string> { "panels", "cells", "batteries", "systems" },
                                CorrectAnswer = "panels",
                                Explanation = "Đáp án đúng là panels vì Solar panels convert sunlight to electricity. Trong đoạn văn có đề cập: \"Solar panels convert sunlight to electricity.\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q2",
                                Type = QuestionType.GapFilling,
                                QuestionText = "Wind ____ generate power from moving air.",
                                Options = new List<string> { "mills", "turbines", "engines", "generators" },
                                CorrectAnswer = "turbines",
                                Explanation = "Đáp án đúng là turbines vì Wind turbines generate power from moving air. Trong đoạn văn có đề cập: \"Wind turbines generate power from moving air.\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q3",
                                Type = QuestionType.GapFilling,
                                QuestionText = "These technologies are becoming more ____.",
                                Options = new List<string> { "expensive", "complex", "efficient", "popular" },
                                CorrectAnswer = "efficient",
                                Explanation = "Đáp án đúng là efficient vì These technologies are becoming more efficient. Trong đoạn văn có đề cập: \"These technologies are becoming more efficient.\"",
                                IsExplanationVisible = false
                            }
                        },
                        Progress = new TestProgress
                        {
                            TotalQuestions = 3,
                            AnsweredQuestions = 0,
                            RemainingTime = 20 * 60 ,
                            IsCompleted = false
                        }
                    }
                },
                                {
                    "test12", new ReadingTestDetail
                    {
                        Id = "test12",
                        Title = "Renewable Energy Sources",
                        Content = @"Renewable energy sources are becoming increasingly important in the fight against climate change. Solar ____ converts sunlight directly into electricity, while wind ____ harness the power of moving air. These technologies are becoming more efficient and cost-effective...",
                        TimeLimit = 50,
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                Id = "q1",
                                Type = QuestionType.GapFilling,
                                QuestionText = "Solar ____ converts sunlight to electricity.",
                                Options = new List<string> { "panels", "cells", "batteries", "systems" },
                                CorrectAnswer = "panels",
                                Explanation = "Đáp án đúng là panels vì Solar panels convert sunlight to electricity. Trong đoạn văn có đề cập: \"Solar panels convert sunlight to electricity.\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q2",
                                Type = QuestionType.GapFilling,
                                QuestionText = "Wind ____ generate power from moving air.",
                                Options = new List<string> { "mills", "turbines", "engines", "generators" },
                                CorrectAnswer = "turbines",
                                Explanation = "Đáp án đúng là turbines vì Wind turbines generate power from moving air. Trong đoạn văn có đề cập: \"Wind turbines generate power from moving air.\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q3",
                                Type = QuestionType.GapFilling,
                                QuestionText = "These technologies are becoming more ____.",
                                Options = new List<string> { "expensive", "complex", "efficient", "popular" },
                                CorrectAnswer = "efficient",
                                Explanation = "Đáp án đúng là efficient vì These technologies are becoming more efficient. Trong đoạn văn có đề cập: \"These technologies are becoming more efficient.\"",
                                IsExplanationVisible = false
                            }
                        },
                        Progress = new TestProgress
                        {
                            TotalQuestions = 3,
                            AnsweredQuestions = 0,
                            RemainingTime = 50 * 60,
                            IsCompleted = false
                        }
                    }
                },
                                 {
                    "test13", new ReadingTestDetail
                    {
                        Id = "test13",
                        Title = "Renewable Energy Sources",
                        Content = @"Renewable energy sources are becoming increasingly important in the fight against climate change. Solar ____ converts sunlight directly into electricity, while wind ____ harness the power of moving air. These technologies are becoming more efficient and cost-effective...",
                        TimeLimit = 50,
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                Id = "q1",
                                Type = QuestionType.GapFilling,
                                QuestionText = "Solar ____ converts sunlight to electricity.",
                                Options = new List<string> { "panels", "cells", "batteries", "systems" },
                                CorrectAnswer = "panels",
                                Explanation = "Đáp án đúng là panels vì Solar panels convert sunlight to electricity. Trong đoạn văn có đề cập: \"Solar panels convert sunlight to electricity.\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q2",
                                Type = QuestionType.GapFilling,
                                QuestionText = "Wind ____ generate power from moving air.",
                                Options = new List<string> { "mills", "turbines", "engines", "generators" },
                                CorrectAnswer = "turbines",
                                Explanation = "Đáp án đúng là turbines vì Wind turbines generate power from moving air. Trong đoạn văn có đề cập: \"Wind turbines generate power from moving air.\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q3",
                                Type = QuestionType.GapFilling,
                                QuestionText = "These technologies are becoming more ____.",
                                Options = new List<string> { "expensive", "complex", "efficient", "popular" },
                                CorrectAnswer = "efficient",
                                Explanation = "Đáp án đúng là efficient vì These technologies are becoming more efficient. Trong đoạn văn có đề cập: \"These technologies are becoming more efficient.\"",
                                IsExplanationVisible = false
                            }
                        },
                        Progress = new TestProgress
                        {
                            TotalQuestions = 3,
                            AnsweredQuestions = 0,
                            RemainingTime = 50 * 60,
                            IsCompleted = false
                        }
                    }
                },
                                  {
                    "test14", new ReadingTestDetail
                    {
                        Id = "test14",
                        Title = "Renewable Energy Sources",
                        Content = @"Renewable energy sources are becoming increasingly important in the fight against climate change. Solar ____ converts sunlight directly into electricity, while wind ____ harness the power of moving air. These technologies are becoming more efficient and cost-effective...",
                        TimeLimit = 50,
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                Id = "q1",
                                Type = QuestionType.GapFilling,
                                QuestionText = "Solar ____ converts sunlight to electricity.",
                                Options = new List<string> { "panels", "cells", "batteries", "systems" },
                                CorrectAnswer = "panels",
                                Explanation = "Đáp án đúng là panels vì Solar panels convert sunlight to electricity. Trong đoạn văn có đề cập: \"Solar panels convert sunlight to electricity.\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q2",
                                Type = QuestionType.GapFilling,
                                QuestionText = "Wind ____ generate power from moving air.",
                                Options = new List<string> { "mills", "turbines", "engines", "generators" },
                                CorrectAnswer = "turbines",
                                Explanation = "Đáp án đúng là turbines vì Wind turbines generate power from moving air. Trong đoạn văn có đề cập: \"Wind turbines generate power from moving air.\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q3",
                                Type = QuestionType.GapFilling,
                                QuestionText = "These technologies are becoming more ____.",
                                Options = new List<string> { "expensive", "complex", "efficient", "popular" },
                                CorrectAnswer = "efficient",
                                Explanation = "Đáp án đúng là efficient vì These technologies are becoming more efficient. Trong đoạn văn có đề cập: \"These technologies are becoming more efficient.\"",
                                IsExplanationVisible = false
                            }
                        },
                        Progress = new TestProgress
                        {
                            TotalQuestions = 3,
                            AnsweredQuestions = 0,
                            RemainingTime = 50 * 60,
                            IsCompleted = false
                        }
                    }
                },
                                   {
                    "test15", new ReadingTestDetail
                    {
                        Id = "test16",
                        Title = "Renewable Energy Sources",
                        Content = @"Renewable energy sources are becoming increasingly important in the fight against climate change. Solar ____ converts sunlight directly into electricity, while wind ____ harness the power of moving air. These technologies are becoming more efficient and cost-effective...",
                        TimeLimit = 50,
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                Id = "q1",
                                Type = QuestionType.GapFilling,
                                QuestionText = "Solar ____ converts sunlight to electricity.",
                                Options = new List<string> { "panels", "cells", "batteries", "systems" },
                                CorrectAnswer = "panels",
                                Explanation = "Đáp án đúng là panels vì Solar panels convert sunlight to electricity. Trong đoạn văn có đề cập: \"Solar panels convert sunlight to electricity.\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q2",
                                Type = QuestionType.GapFilling,
                                QuestionText = "Wind ____ generate power from moving air.",
                                Options = new List<string> { "mills", "turbines", "engines", "generators" },
                                CorrectAnswer = "turbines",
                                Explanation = "Đáp án đúng là turbines vì Wind turbines generate power from moving air. Trong đoạn văn có đề cập: \"Wind turbines generate power from moving air.\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q3",
                                Type = QuestionType.GapFilling,
                                QuestionText = "These technologies are becoming more ____.",
                                Options = new List<string> { "expensive", "complex", "efficient", "popular" },
                                CorrectAnswer = "efficient",
                                Explanation = "Đáp án đúng là efficient vì These technologies are becoming more efficient. Trong đoạn văn có đề cập: \"These technologies are becoming more efficient.\"",
                                IsExplanationVisible = false
                            }
                        },
                        Progress = new TestProgress
                        {
                            TotalQuestions = 3,
                            AnsweredQuestions = 0,
                            RemainingTime = 50 * 60,
                            IsCompleted = false
                        }
                    }
                },
                                    {
                    "test16", new ReadingTestDetail
                    {
                        Id = "test16",
                        Title = "Renewable Energy Sources",
                        Content = @"Renewable energy sources are becoming increasingly important in the fight against climate change. Solar ____ converts sunlight directly into electricity, while wind ____ harness the power of moving air. These technologies are becoming more efficient and cost-effective...",
                        TimeLimit = 50,
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                Id = "q1",
                                Type = QuestionType.GapFilling,
                                QuestionText = "Solar ____ converts sunlight to electricity.",
                                Options = new List<string> { "panels", "cells", "batteries", "systems" },
                                CorrectAnswer = "panels",
                                Explanation = "Đáp án đúng là panels vì Solar panels convert sunlight to electricity. Trong đoạn văn có đề cập: \"Solar panels convert sunlight to electricity.\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q2",
                                Type = QuestionType.GapFilling,
                                QuestionText = "Wind ____ generate power from moving air.",
                                Options = new List<string> { "mills", "turbines", "engines", "generators" },
                                CorrectAnswer = "turbines",
                                Explanation = "Đáp án đúng là turbines vì Wind turbines generate power from moving air. Trong đoạn văn có đề cập: \"Wind turbines generate power from moving air.\"",
                                IsExplanationVisible = false
                            },
                            new Question
                            {
                                Id = "q3",
                                Type = QuestionType.GapFilling,
                                QuestionText = "These technologies are becoming more ____.",
                                Options = new List<string> { "expensive", "complex", "efficient", "popular" },
                                CorrectAnswer = "efficient",
                                Explanation = "Đáp án đúng là efficient vì These technologies are becoming more efficient. Trong đoạn văn có đề cập: \"These technologies are becoming more efficient.\"",
                                IsExplanationVisible = false
                            }
                        },
                        Progress = new TestProgress
                        {
                            TotalQuestions = 3,
                            AnsweredQuestions = 0,
                            RemainingTime = 50 * 60,
                            IsCompleted = false
                        }
                    }
                }


            };
        }


        public async Task<ReadingTestDetail> GetTestDetailAsync(string testId)
        {
            // Giả lập delay của network
            await Task.Delay(500);
            return _mockTests.GetValueOrDefault(testId) ??
                throw new Exception("Test not found");
        }
        // tuog tu
        public async Task SaveAnswerAsync(string testId, string questionId, string answer)
        {
            await Task.Delay(100); // Giả lập delay
            if (_mockTests.TryGetValue(testId, out var test))
            {
                var question = test.Questions.FirstOrDefault(q => q.Id == questionId);
                if (question != null)
                {
                    question.UserAnswer = answer;
                }
            }
        }

        public async Task<bool> UpdateTestCompletionStatus(string testId, bool isCompleted)
        {
            await Task.Delay(100);
            if (_mockTests.ContainsKey(testId))
            {
                _mockTests[testId].Progress.IsCompleted = isCompleted;
                return true;
            }
            return false;
        }


        // đã update vs testID bên cs
        public async Task<bool> SubmitTestAsync(string testId)
        {
            if (_mockTests.ContainsKey(testId))
            {
                var test = _mockTests[testId];
                test.Progress.IsCompleted = true;
                test.Progress.AnsweredQuestions = test.Questions.Count(q =>
                    q.UserAnswer == q.CorrectAnswer);

                // Thông báo cho UI cập nhật trạng thái
                await UpdateTestCompletionStatus(testId, true);
                return true;
            }
            return false;
        }


    }
}

// await Task.Delay(500); // Giả lập delay