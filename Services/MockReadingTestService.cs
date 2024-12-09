//using login_full.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace login_full.Services
//{
//    public class MockReadingTestService : IReadingTestService
//    {
//        private readonly Dictionary<string, ReadingTestDetail> _mockTests;
//        private readonly List<TestHistory> _testHistory;
//        private readonly LocalStorageService _localStorageService;

//        public MockReadingTestService(LocalStorageService localStorageService)
//        {
//            _localStorageService = localStorageService;
//            _testHistory = _localStorageService.GetTestHistory();
//            _mockTests = new Dictionary<string, ReadingTestDetail>
//            {
//                {
//                    "test1", new ReadingTestDetail
//                    {
//                        Id = "test1",
//                        Title = "Alfred Wegener: science, exploration and the theory of continental drift",
//                        Content = @"This is a book about the life and scientific work of Alfred Wegener, whose reputation today rests with his theory of continental displacement, better known as continental drift. The theory maintains that continents were joined together at one time, before breaking apart and drifting to their present positions. This was a highly controversial theory when Wegener proposed it in 1912. It was almost universally rejected by the scientific community during his lifetime...",
//                        TimeLimit = 10,
//                        Questions = new List<ReadingTestQuestion>
//                        {
//                            new ReadingTestQuestion
//                            {
//                                Id = "q1",
//                                Type = QuestionType.YesNoNotGiven,
//                                QuestionText = "Wegener's ideas about continental drift were widely disputed while he was alive.",
//                                Options = new List<string> { "YES", "NO", "NOT GIVEN" },
//                                CorrectAnswer = "YES",
//                                Explanation = "Đáp án đúng là YES vì trong đoạn văn có đề cập: \"It was almost universally rejected by the scientific community during his lifetime\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q2",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "The theory suggests that continents were once ____ before separating.",
//                                CorrectAnswer = "joined",
//                                Explanation = "Đáp án đúng là 'joined' vì trong đoạn văn có đề cập: \"The theory maintains that continents were joined together at one time\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q3",
//                                Type = QuestionType.MultipleChoice,
//                                QuestionText = "When did Wegener first propose his theory?",
//                                Options = new List<string> { "1910", "1911", "1912", "1913" },
//                                CorrectAnswer = "1912",
//                                Explanation = "Đáp án đúng là 1912 vì trong đoạn văn có đề cập: \"when Wegener proposed it in 1912\"",
//                                IsExplanationVisible = false
//                            },
//                        },
//                        Progress = new TestProgress
//                        {
//                            TotalQuestions = 3,
//                            AnsweredQuestions = 0,
//                            RemainingTime = 10*60,
//                            IsCompleted = true
//                        }
//                    }
//                },
//                {
//                    "test2", new ReadingTestDetail
//                    {
//                        Id = "test2",
//                        Title = "The History of the Silk Road",
//                        Content = @"The Silk Road was an ancient network of trade routes that connected the East and West, and was central to cultural interaction between them for centuries. The Silk Road primarily refers to the terrestrial routes connecting East Asia and Southeast Asia with East Africa, West Asia and Southern Europe. The Silk Road derives its name from the lucrative trade in silk carried out along its length, beginning in the Han dynasty of China (207 BCE–220 CE)...",
//                        TimeLimit = 50,
//                        Questions = new List<ReadingTestQuestion>
//                        {
//                            new ReadingTestQuestion
//                            {
//                                Id = "q1",
//                                Type = QuestionType.MultipleChoice,
//                                QuestionText = "What was the main purpose of the Silk Road?",
//                                Options = new List<string> {
//                                    "Cultural exchange only",
//                                    "Trade routes connecting East and West",
//                                    "Silk transportation only",
//                                    "Military routes"
//                                },
//                                CorrectAnswer = "Trade routes connecting East and West",
//                                Explanation = "Đáp án đúng là \"Trade routes connecting East and West\" vì đoạn văn mở đầu nêu rõ đây là \"ancient network of trade routes that connected the East and West\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q2",
//                                Type = QuestionType.TrueFalseNotGiven,
//                                QuestionText = "The Silk Road was exclusively used for trading silk.",
//                                Options = new List<string> { "TRUE", "FALSE", "NOT GIVEN" },
//                                CorrectAnswer = "FALSE",
//                                Explanation = "Đáp án là FALSE vì tuy con đường tơ lụa được đặt tên theo việc buôn bán tơ lụa nhưng không phải chỉ dùng để buôn bán tơ lụa, mà còn là nơi giao lưu văn hóa",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q3",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "The Silk Road got its name from the ____ trade in silk during the Han dynasty.",
//                                CorrectAnswer = "lucrative",
//                                Explanation = "Đáp án là 'lucrative' vì trong đoạn văn có đề cập: \"derives its name from the lucrative trade in silk\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q4",
//                                Type = QuestionType.YesNoNotGiven,
//                                QuestionText = "The Silk Road began operation during the Han dynasty.",
//                                Options = new List<string> { "YES", "NO", "NOT GIVEN" },
//                                CorrectAnswer = "YES",
//                                Explanation = "Đáp án là YES vì trong đoạn văn có đề cập việc buôn bán tơ lụa bắt đầu \"beginning in the Han dynasty\"",
//                                IsExplanationVisible = false
//                            }
//                        },
//                        Progress = new TestProgress
//                        {
//                            TotalQuestions = 4,
//                            AnsweredQuestions = 0,
//                            RemainingTime = 50 * 60,
//                            IsCompleted = false
//                        }
//                    }
//                },
//                {
//                    "test3", new ReadingTestDetail
//                    {
//                        Id = "test3",
//                        Title = "The Benefits of Urban Green Spaces",
//                        Content = @"Green spaces in urban areas provide numerous benefits, including reducing air pollution, improving mental health, and fostering community interaction...",
//                        TimeLimit = 15,
//                        Questions = new List<ReadingTestQuestion>
//                        {
//                            new ReadingTestQuestion
//                            {
//                                Id = "q1",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "Green spaces help to improve ____ by reducing air pollution.",
//                                Options = new List<string> { "air quality", "water quality", "noise levels", "traffic flow" },
//                                CorrectAnswer = "air quality",
//                                Explanation = "Đáp án đúng là air quality vì Green spaces help to improve air quality by reducing air pollution. Trong đoạn văn có đề cập: \"Green spaces help to improve air quality by reducing air pollution.\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q2",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "Urban parks often provide opportunities for ____ activities.",
//                                Options = new List<string> { "cultural", "commercial", "educational", "physical" },
//                                CorrectAnswer = "physical",
//                                Explanation = "Đáp án đúng là physical vì Urban parks often provide opportunities for physical activities. Trong đoạn văn có đề cập: \"Urban parks often provide opportunities for physical activities.\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q3",
//                                Type = QuestionType.TrueFalseNotGiven,
//                                QuestionText = "Green spaces increase property values in nearby areas.",
//                                Options = new List<string> { "TRUE", "FALSE", "NOT GIVEN" },
//                                CorrectAnswer = "TRUE",
//                                Explanation = "Đáp án đúng là TRUE vì Green spaces increase property values in nearby areas. Trong đoạn văn có đề cập: \"Green spaces increase property values in nearby areas.\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q4",
//                                Type = QuestionType.YesNoNotGiven,
//                                QuestionText = "All urban green spaces are publicly owned.",
//                                Options = new List<string> { "YES", "NO", "NOT GIVEN" },
//                                CorrectAnswer = "NO",
//                                Explanation = "Đáp án đúng là NO vì Not all urban green spaces are publicly owned. Trong đoạn văn có đề cập: \"Not all urban green spaces are publicly owned.\"",
//                                IsExplanationVisible = false
//                            }
//                        },
//                        Progress = new TestProgress
//                        {
//                            TotalQuestions = 4,
//                            AnsweredQuestions = 0,
//                            RemainingTime = 2700,
//                            IsCompleted = false
//                        }
//                    }
//                },
//                {
//                    "test4", new ReadingTestDetail
//                    {
//                        Id = "test4",
//                        Title = "The Evolution of Artificial Intelligence",
//                        Content = @"Artificial Intelligence (AI) has evolved significantly since its inception in the 1950s. The field began with simple ____ systems that could perform basic calculations. Today, AI encompasses machine learning, neural networks, and deep learning technologies...",
//                        TimeLimit = 45,
//                        Questions = new List<ReadingTestQuestion>
//                        {
//                            new ReadingTestQuestion
//                            {
//                                Id = "q1",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "AI started with ____ systems in the 1950s.",
//                                Options = new List<string> { "computing", "thinking", "learning", "programming" },
//                                CorrectAnswer = "computing",
//                                Explanation = "Đáp án đúng là computing vì AI started with computing systems in the 1950s. Trong đoạn văn có đề cập: \"AI started with computing systems in the 1950s.\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q2",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "Modern AI uses ____ networks for complex tasks.",
//                                Options = new List<string> { "digital", "neural", "computer", "social" },
//                                CorrectAnswer = "neural",
//                                Explanation = "Đáp án đúng là neural vì Modern AI uses neural networks for complex tasks. Trong đoạn văn có đề cập: \"Modern AI uses neural networks for complex tasks.\"",
//                                IsExplanationVisible = false
//                            }
//                        },
//                        Progress = new TestProgress
//                        {
//                            TotalQuestions = 2,
//                            AnsweredQuestions = 0,
//                            RemainingTime = 2700,
//                            IsCompleted = false
//                        }
//                    }
//                },

//                {
//                    "test6", new ReadingTestDetail
//                    {
//                        Id = "test6",
//                        Title = "Renewable Energy Sources",
//                        Content = @"Renewable energy sources are becoming increasingly important in the fight against climate change. Solar ____ converts sunlight directly into electricity, while wind ____ harness the power of moving air. These technologies are becoming more efficient and cost-effective...",
//                        TimeLimit = 10,
//                        Questions = new List<ReadingTestQuestion>
//                        {
//                            new ReadingTestQuestion
//                            {
//                                Id = "q1",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "Solar ____ converts sunlight to electricity.",
//                                Options = new List<string> { "panels", "cells", "batteries", "systems" },
//                                CorrectAnswer = "panels",
//                                Explanation = "Đáp án đúng là panels vì Solar panels convert sunlight to electricity. Trong đoạn văn có đề cập: \"Solar panels convert sunlight to electricity.\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q2",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "Wind ____ generate power from moving air.",
//                                Options = new List<string> { "mills", "turbines", "engines", "generators" },
//                                CorrectAnswer = "turbines",
//                                Explanation = "Đáp án đúng là turbines vì Wind turbines generate power from moving air. Trong đoạn văn có đề cập: \"Wind turbines generate power from moving air.\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q3",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "These technologies are becoming more ____.",
//                                Options = new List<string> { "expensive", "complex", "efficient", "popular" },
//                                CorrectAnswer = "efficient",
//                                Explanation = "Đáp án đúng là efficient vì These technologies are becoming more efficient. Trong đoạn văn có đề cập: \"These technologies are becoming more efficient.\"",
//                                IsExplanationVisible = false
//                            }
//                        },
//                        Progress = new TestProgress
//                        {
//                            TotalQuestions = 3,
//                            AnsweredQuestions = 0,
//                            RemainingTime = 10*60,
//                            IsCompleted = false
//                        }
//                    }
//                },
//                                {
//                    "test7", new ReadingTestDetail
//                    {
//                        Id = "test6",
//                        Title = "Renewable Energy Sources",
//                        Content = @"Renewable energy sources are becoming increasingly important in the fight against climate change. Solar ____ converts sunlight directly into electricity, while wind ____ harness the power of moving air. These technologies are becoming more efficient and cost-effective...",
//                        TimeLimit = 10,
//                        Questions = new List<ReadingTestQuestion>
//                        {
//                            new ReadingTestQuestion
//                            {
//                                Id = "q1",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "Solar ____ converts sunlight to electricity.",
//                                Options = new List<string> { "panels", "cells", "batteries", "systems" },
//                                CorrectAnswer = "panels",
//                                Explanation = "Đáp án đúng là panels vì Solar panels convert sunlight to electricity. Trong đoạn văn có đề cập: \"Solar panels convert sunlight to electricity.\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q2",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "Wind ____ generate power from moving air.",
//                                Options = new List<string> { "mills", "turbines", "engines", "generators" },
//                                CorrectAnswer = "turbines",
//                                Explanation = "Đáp án đúng là turbines vì Wind turbines generate power from moving air. Trong đoạn văn có đề cập: \"Wind turbines generate power from moving air.\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q3",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "These technologies are becoming more ____.",
//                                Options = new List<string> { "expensive", "complex", "efficient", "popular" },
//                                CorrectAnswer = "efficient",
//                                Explanation = "Đáp án đúng là efficient vì These technologies are becoming more efficient. Trong đoạn văn có đề cập: \"These technologies are becoming more efficient.\"",
//                                IsExplanationVisible = false
//                            }
//                        },
//                        Progress = new TestProgress
//                        {
//                            TotalQuestions = 3,
//                            AnsweredQuestions = 0,
//                            RemainingTime = 10*60,
//                            IsCompleted = false
//                        }
//                    }
//                },
//                                {
//                    "test8", new ReadingTestDetail
//                    {
//                        Id = "test8",
//                        Title = "Renewable Energy Sources",
//                        Content = @"Renewable energy sources are becoming increasingly important in the fight against climate change. Solar ____ converts sunlight directly into electricity, while wind ____ harness the power of moving air. These technologies are becoming more efficient and cost-effective...",
//                        TimeLimit = 5,
//                        Questions = new List<ReadingTestQuestion>
//                        {
//                            new ReadingTestQuestion
//                            {
//                                Id = "q1",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "Solar ____ converts sunlight to electricity.",
//                                Options = new List<string> { "panels", "cells", "batteries", "systems" },
//                                CorrectAnswer = "panels",
//                                Explanation = "Đáp án đúng là panels vì Solar panels convert sunlight to electricity. Trong đoạn văn có đề cập: \"Solar panels convert sunlight to electricity.\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q2",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "Wind ____ generate power from moving air.",
//                                Options = new List<string> { "mills", "turbines", "engines", "generators" },
//                                CorrectAnswer = "turbines",
//                                Explanation = "Đáp án đúng là turbines vì Wind turbines generate power from moving air. Trong đoạn văn có đề cập: \"Wind turbines generate power from moving air.\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q3",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "These technologies are becoming more ____.",
//                                Options = new List<string> { "expensive", "complex", "efficient", "popular" },
//                                CorrectAnswer = "efficient",
//                                Explanation = "Đáp án đúng là efficient vì These technologies are becoming more efficient. Trong đoạn văn có đề cập: \"These technologies are becoming more efficient.\"",
//                                IsExplanationVisible = false
//                            }
//                        },
//                        Progress = new TestProgress
//                        {
//                            TotalQuestions = 3,
//                            AnsweredQuestions = 0,
//                            RemainingTime = 5 * 60,
//                            IsCompleted = false
//                        }
//                    }
//                },
//                                {
//                    "test9", new ReadingTestDetail
//                    {
//                        Id = "test9",
//                        Title = "Renewable Energy Sources",
//                        Content = @"Renewable energy sources are becoming increasingly important in the fight against climate change. Solar ____ converts sunlight directly into electricity, while wind ____ harness the power of moving air. These technologies are becoming more efficient and cost-effective...",
//                        TimeLimit = 50,
//                        Questions = new List<ReadingTestQuestion>
//                        {
//                            new ReadingTestQuestion
//                            {
//                                Id = "q1",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "Solar ____ converts sunlight to electricity.",
//                                Options = new List<string> { "panels", "cells", "batteries", "systems" },
//                                CorrectAnswer = "panels",
//                                Explanation = "Đáp án đúng là panels vì Solar panels convert sunlight to electricity. Trong đoạn văn có đề cập: \"Solar panels convert sunlight to electricity.\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q2",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "Wind ____ generate power from moving air.",
//                                Options = new List<string> { "mills", "turbines", "engines", "generators" },
//                                CorrectAnswer = "turbines",
//                                Explanation = "Đáp án đúng là turbines vì Wind turbines generate power from moving air. Trong đoạn văn có đề cập: \"Wind turbines generate power from moving air.\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q3",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "These technologies are becoming more ____.",
//                                Options = new List<string> { "expensive", "complex", "efficient", "popular" },
//                                CorrectAnswer = "efficient",
//                                Explanation = "Đáp án đúng là efficient vì These technologies are becoming more efficient. Trong đoạn văn có đề cập: \"These technologies are becoming more efficient.\"",
//                                IsExplanationVisible = false
//                            }
//                        },
//                        Progress = new TestProgress
//                        {
//                            TotalQuestions = 3,
//                            AnsweredQuestions = 0,
//                            RemainingTime = 50 * 60,
//                            IsCompleted = false
//                        }
//                    }
//                },
//                                {
//                    "test10", new ReadingTestDetail
//                    {
//                        Id = "test10",
//                        Title = "Renewable Energy Sources",
//                        Content = @"Renewable energy sources are becoming increasingly important in the fight against climate change. Solar ____ converts sunlight directly into electricity, while wind ____ harness the power of moving air. These technologies are becoming more efficient and cost-effective...",
//                        TimeLimit = 50,
//                        Questions = new List<ReadingTestQuestion>
//                        {
//                            new ReadingTestQuestion
//                            {
//                                Id = "q1",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "Solar ____ converts sunlight to electricity.",
//                                Options = new List<string> { "panels", "cells", "batteries", "systems" },
//                                CorrectAnswer = "panels",
//                                Explanation = "Đáp án đúng là panels vì Solar panels convert sunlight to electricity. Trong đoạn văn có đề cập: \"Solar panels convert sunlight to electricity.\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q2",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "Wind ____ generate power from moving air.",
//                                Options = new List<string> { "mills", "turbines", "engines", "generators" },
//                                CorrectAnswer = "turbines",
//                                Explanation = "Đáp án đúng là turbines vì Wind turbines generate power from moving air. Trong đoạn văn có đề cập: \"Wind turbines generate power from moving air.\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q3",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "These technologies are becoming more ____.",
//                                Options = new List<string> { "expensive", "complex", "efficient", "popular" },
//                                CorrectAnswer = "efficient",
//                                Explanation = "Đáp án đúng là efficient vì These technologies are becoming more efficient. Trong đoạn văn có đề cập: \"These technologies are becoming more efficient.\"",
//                                IsExplanationVisible = false
//                            }
//                        },
//                        Progress = new TestProgress
//                        {
//                            TotalQuestions = 3,
//                            AnsweredQuestions = 0,
//                            RemainingTime = 3000,
//                            IsCompleted = false
//                        }
//                    }
//                },

//                                {
//                    "test11", new ReadingTestDetail
//                    {
//                        Id = "test11",
//                        Title = "Renewable Energy Sources",
//                        Content = @"Renewable energy sources are becoming increasingly important in the fight against climate change. Solar ____ converts sunlight directly into electricity, while wind ____ harness the power of moving air. These technologies are becoming more efficient and cost-effective...",
//                        TimeLimit = 20,
//                        Questions = new List<ReadingTestQuestion>
//                        {
//                            new ReadingTestQuestion
//                            {
//                                Id = "q1",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "Solar ____ converts sunlight to electricity.",
//                                Options = new List<string> { "panels", "cells", "batteries", "systems" },
//                                CorrectAnswer = "panels",
//                                Explanation = "Đáp án đúng là panels vì Solar panels convert sunlight to electricity. Trong đoạn văn có đề cập: \"Solar panels convert sunlight to electricity.\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q2",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "Wind ____ generate power from moving air.",
//                                Options = new List<string> { "mills", "turbines", "engines", "generators" },
//                                CorrectAnswer = "turbines",
//                                Explanation = "Đáp án đúng là turbines vì Wind turbines generate power from moving air. Trong đoạn văn có đề cập: \"Wind turbines generate power from moving air.\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q3",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "These technologies are becoming more ____.",
//                                Options = new List<string> { "expensive", "complex", "efficient", "popular" },
//                                CorrectAnswer = "efficient",
//                                Explanation = "Đáp án đúng là efficient vì These technologies are becoming more efficient. Trong đoạn văn có đề cập: \"These technologies are becoming more efficient.\"",
//                                IsExplanationVisible = false
//                            }
//                        },
//                        Progress = new TestProgress
//                        {
//                            TotalQuestions = 3,
//                            AnsweredQuestions = 0,
//                            RemainingTime = 20 * 60 ,
//                            IsCompleted = false
//                        }
//                    }
//                },
//                                {
//                    "test12", new ReadingTestDetail
//                    {
//                        Id = "test12",
//                        Title = "Renewable Energy Sources",
//                        Content = @"Renewable energy sources are becoming increasingly important in the fight against climate change. Solar ____ converts sunlight directly into electricity, while wind ____ harness the power of moving air. These technologies are becoming more efficient and cost-effective...",
//                        TimeLimit = 50,
//                        Questions = new List<ReadingTestQuestion>
//                        {
//                            new ReadingTestQuestion
//                            {
//                                Id = "q1",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "Solar ____ converts sunlight to electricity.",
//                                Options = new List<string> { "panels", "cells", "batteries", "systems" },
//                                CorrectAnswer = "panels",
//                                Explanation = "Đáp án đúng là panels vì Solar panels convert sunlight to electricity. Trong đoạn văn có đề cập: \"Solar panels convert sunlight to electricity.\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q2",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "Wind ____ generate power from moving air.",
//                                Options = new List<string> { "mills", "turbines", "engines", "generators" },
//                                CorrectAnswer = "turbines",
//                                Explanation = "Đáp án đúng là turbines vì Wind turbines generate power from moving air. Trong đoạn văn có đề cập: \"Wind turbines generate power from moving air.\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q3",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "These technologies are becoming more ____.",
//                                Options = new List<string> { "expensive", "complex", "efficient", "popular" },
//                                CorrectAnswer = "efficient",
//                                Explanation = "Đáp án đúng là efficient vì These technologies are becoming more efficient. Trong đoạn văn có đề cập: \"These technologies are becoming more efficient.\"",
//                                IsExplanationVisible = false
//                            }
//                        },
//                        Progress = new TestProgress
//                        {
//                            TotalQuestions = 3,
//                            AnsweredQuestions = 0,
//                            RemainingTime = 50 * 60,
//                            IsCompleted = false
//                        }
//                    }
//                },
//                                 {
//                    "test13", new ReadingTestDetail
//                    {
//                        Id = "test13",
//                        Title = "Renewable Energy Sources",
//                        Content = @"Renewable energy sources are becoming increasingly important in the fight against climate change. Solar ____ converts sunlight directly into electricity, while wind ____ harness the power of moving air. These technologies are becoming more efficient and cost-effective...",
//                        TimeLimit = 50,
//                        Questions = new List<ReadingTestQuestion>
//                        {
//                            new ReadingTestQuestion
//                            {
//                                Id = "q1",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "Solar ____ converts sunlight to electricity.",
//                                Options = new List<string> { "panels", "cells", "batteries", "systems" },
//                                CorrectAnswer = "panels",
//                                Explanation = "Đáp án đúng là panels vì Solar panels convert sunlight to electricity. Trong đoạn văn có đề cập: \"Solar panels convert sunlight to electricity.\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q2",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "Wind ____ generate power from moving air.",
//                                Options = new List<string> { "mills", "turbines", "engines", "generators" },
//                                CorrectAnswer = "turbines",
//                                Explanation = "Đáp án đúng là turbines vì Wind turbines generate power from moving air. Trong đoạn văn có đề cập: \"Wind turbines generate power from moving air.\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q3",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "These technologies are becoming more ____.",
//                                Options = new List<string> { "expensive", "complex", "efficient", "popular" },
//                                CorrectAnswer = "efficient",
//                                Explanation = "Đáp án đúng là efficient vì These technologies are becoming more efficient. Trong đoạn văn có đề cập: \"These technologies are becoming more efficient.\"",
//                                IsExplanationVisible = false
//                            }
//                        },
//                        Progress = new TestProgress
//                        {
//                            TotalQuestions = 3,
//                            AnsweredQuestions = 0,
//                            RemainingTime = 50 * 60,
//                            IsCompleted = false
//                        }
//                    }
//                },
//                                  {
//                    "test14", new ReadingTestDetail
//                    {
//                        Id = "test14",
//                        Title = "Renewable Energy Sources",
//                        Content = @"Renewable energy sources are becoming increasingly important in the fight against climate change. Solar ____ converts sunlight directly into electricity, while wind ____ harness the power of moving air. These technologies are becoming more efficient and cost-effective...",
//                        TimeLimit = 50,
//                        Questions = new List<ReadingTestQuestion>
//                        {
//                            new ReadingTestQuestion
//                            {
//                                Id = "q1",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "Solar ____ converts sunlight to electricity.",
//                                Options = new List<string> { "panels", "cells", "batteries", "systems" },
//                                CorrectAnswer = "panels",
//                                Explanation = "Đáp án đúng là panels vì Solar panels convert sunlight to electricity. Trong đoạn văn có đề cập: \"Solar panels convert sunlight to electricity.\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q2",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "Wind ____ generate power from moving air.",
//                                Options = new List<string> { "mills", "turbines", "engines", "generators" },
//                                CorrectAnswer = "turbines",
//                                Explanation = "Đáp án đúng là turbines vì Wind turbines generate power from moving air. Trong đoạn văn có đề cập: \"Wind turbines generate power from moving air.\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q3",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "These technologies are becoming more ____.",
//                                Options = new List<string> { "expensive", "complex", "efficient", "popular" },
//                                CorrectAnswer = "efficient",
//                                Explanation = "Đáp án đúng là efficient vì These technologies are becoming more efficient. Trong đoạn văn có đề cập: \"These technologies are becoming more efficient.\"",
//                                IsExplanationVisible = false
//                            }
//                        },
//                        Progress = new TestProgress
//                        {
//                            TotalQuestions = 3,
//                            AnsweredQuestions = 0,
//                            RemainingTime = 50 * 60,
//                            IsCompleted = false
//                        }
//                    }
//                },
//                                   {
//                    "test15", new ReadingTestDetail
//                    {
//                        Id = "test16",
//                        Title = "Renewable Energy Sources",
//                        Content = @"Renewable energy sources are becoming increasingly important in the fight against climate change. Solar ____ converts sunlight directly into electricity, while wind ____ harness the power of moving air. These technologies are becoming more efficient and cost-effective...",
//                        TimeLimit = 50,
//                        Questions = new List<ReadingTestQuestion>
//                        {
//                            new ReadingTestQuestion
//                            {
//                                Id = "q1",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "Solar ____ converts sunlight to electricity.",
//                                Options = new List<string> { "panels", "cells", "batteries", "systems" },
//                                CorrectAnswer = "panels",
//                                Explanation = "Đáp án đúng là panels vì Solar panels convert sunlight to electricity. Trong đoạn văn có đề cập: \"Solar panels convert sunlight to electricity.\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q2",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "Wind ____ generate power from moving air.",
//                                Options = new List<string> { "mills", "turbines", "engines", "generators" },
//                                CorrectAnswer = "turbines",
//                                Explanation = "Đáp án đúng là turbines vì Wind turbines generate power from moving air. Trong đoạn văn có đề cập: \"Wind turbines generate power from moving air.\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q3",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "These technologies are becoming more ____.",
//                                Options = new List<string> { "expensive", "complex", "efficient", "popular" },
//                                CorrectAnswer = "efficient",
//                                Explanation = "Đáp án đúng là efficient vì These technologies are becoming more efficient. Trong đoạn văn có đề cập: \"These technologies are becoming more efficient.\"",
//                                IsExplanationVisible = false
//                            }
//                        },
//                        Progress = new TestProgress
//                        {
//                            TotalQuestions = 3,
//                            AnsweredQuestions = 0,
//                            RemainingTime = 50 * 60,
//                            IsCompleted = false
//                        }
//                    }
//                },
//                                    {
//                    "test16", new ReadingTestDetail
//                    {
//                        Id = "test16",
//                        Title = "Renewable Energy Sources",
//                        Content = @"Renewable energy sources are becoming increasingly important in the fight against climate change. Solar ____ converts sunlight directly into electricity, while wind ____ harness the power of moving air. These technologies are becoming more efficient and cost-effective...",
//                        TimeLimit = 50,
//                        Questions = new List<ReadingTestQuestion>
//                        {
//                            new ReadingTestQuestion
//                            {
//                                Id = "q1",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "Solar ____ converts sunlight to electricity.",
//                                Options = new List<string> { "panels", "cells", "batteries", "systems" },
//                                CorrectAnswer = "panels",
//                                Explanation = "Đáp án đúng là panels vì Solar panels convert sunlight to electricity. Trong đoạn văn có đề cập: \"Solar panels convert sunlight to electricity.\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q2",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "Wind ____ generate power from moving air.",
//                                Options = new List<string> { "mills", "turbines", "engines", "generators" },
//                                CorrectAnswer = "turbines",
//                                Explanation = "Đáp án đúng là turbines vì Wind turbines generate power from moving air. Trong đoạn văn có đề cập: \"Wind turbines generate power from moving air.\"",
//                                IsExplanationVisible = false
//                            },
//                            new ReadingTestQuestion
//                            {
//                                Id = "q3",
//                                Type = QuestionType.GapFilling,
//                                QuestionText = "These technologies are becoming more ____.",
//                                Options = new List<string> { "expensive", "complex", "efficient", "popular" },
//                                CorrectAnswer = "efficient",
//                                Explanation = "Đáp án đúng là efficient vì These technologies are becoming more efficient. Trong đoạn văn có đề cập: \"These technologies are becoming more efficient.\"",
//                                IsExplanationVisible = false
//                            }
//                        },
//                        Progress = new TestProgress
//                        {
//                            TotalQuestions = 3,
//                            AnsweredQuestions = 0,
//                            RemainingTime = 50 * 60,
//                            IsCompleted = false
//                        }
//                    }
//                }


//            };
//        }


//        public async Task<ReadingTestDetail> GetTestDetailAsync(string testId)
//        {
//            // Giả lập delay của network
//            await Task.Delay(500);
//            return _mockTests.GetValueOrDefault(testId) ??
//                throw new Exception("Test not found");
//        }
//        // tuog tu
//        public async Task SaveAnswerAsync(string testId, string questionId, string answer)
//        {
//            await Task.Delay(100); // Giả lập delay
//            if (_mockTests.TryGetValue(testId, out var test))
//            {
//                var question = test.Questions.FirstOrDefault(q => q.Id == questionId);
//                if (question != null)
//                {
//                    question.UserAnswer = answer;
//                }
//            }
//        }

//        public async Task<bool> UpdateTestCompletionStatus(string testId, bool isCompleted)
//        {
//            await Task.Delay(100);
//            if (_mockTests.ContainsKey(testId))
//            {
//                _mockTests[testId].Progress.IsCompleted = isCompleted;
//                return true;
//            }
//            return false;
//        }


//        // đã update vs testID bên cs
//        public async Task<bool> SubmitTestAsync(string testId)
//        {
//            try
//            {
//                if (_mockTests.ContainsKey(testId))
//                {
//                    var test = _mockTests[testId];
//                    test.Progress.IsCompleted = true;

//                    // Tính toán kết quả
//                    var correctAnswers = test.Questions.Count(q => q.UserAnswer == q.CorrectAnswer);
//                    var wrongAnswers = test.Questions.Count(q => !string.IsNullOrEmpty(q.UserAnswer) && q.UserAnswer != q.CorrectAnswer);
//                    var skippedAnswers = test.Questions.Count(q => string.IsNullOrEmpty(q.UserAnswer));

//                    // Tạo đối tượng lịch sử mới
//                    var testHistory = new TestHistory
//                    {
//                        TestId = testId,
//                        Title = test.Title,
//                        SubmitTime = DateTime.Now,
//                        Duration = TimeSpan.FromSeconds(test.TimeLimit * 60 - test.Progress.RemainingTime),
//                        TotalQuestions = test.Questions.Count,
//                        CorrectAnswers = correctAnswers,
//                        WrongAnswers = wrongAnswers,
//                        SkippedAnswers = skippedAnswers
//                    };

//                    // Thêm vào list (đã được khởi tạo trong constructor)
//                    _testHistory.Add(testHistory);
//                    _localStorageService.SaveTestHistory(_testHistory);


//                    // Cập nhật Progress
//                    test.Progress.AnsweredQuestions = correctAnswers;
//                    await UpdateTestCompletionStatus(testId, true);
//                    return true;
//                }
//                return false;
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error in SubmitTestAsync: {ex.Message}");
//                return false;
//            }
//        }

//        public async Task<List<TestHistory>> GetTestHistoryAsync()
//        {
//            try
//            {
//                await Task.Delay(100); // Simulate network delay
//                return _localStorageService.GetTestHistory().OrderByDescending(h => h.SubmitTime).ToList();
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error in GetTestHistoryAsync: {ex.Message}");
//                return new List<TestHistory>();
//            }
//        }

//    }
//}

//// await Task.Delay(500); // Giả lập delay