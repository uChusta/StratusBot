using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StratusBot
{
    public class QuizQuestion
    {
        public string Question { get; set; }
        public List<string> Options { get; set; } // A, B, C, D for multiple choice
        public string CorrectAnswer { get; set; } // e.g. 'C' or 'True'
        public string Explanation { get; set; } // shown after answering
        public bool IsTrueFalse { get; set; }
    }



    public class QuizManager
    {
        private List<QuizQuestion> _questions;
        private int _currentIndex = 0;
        private int _score = 0;

        //constructor for questions
        public QuizManager()
        {
            _questions = new List<QuizQuestion>();

        }
        private void GetCurrentQuestion()
        {

        }
        private void SubmitAnswer(string answer)
        {

        }
        private bool GetFeedback(bool correct)
        {

        }
        private void IsFinished()
        {

        }
        private void GetFinalScore() 
        {
        
        }
        private void GetFinalMessage()
        {

        }
        private void ResetQuiz()
        {

        }
    }
}
