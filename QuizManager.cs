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
        public string Topic { get; set; }
    }



    public class QuizManager
    {
        private List<QuizQuestion> _allQuestions; //All questions
        private List<QuizQuestion> _questions; // chooses 10 random quiz questions
        private int _currentIndex = 0;
        private int _score = 0;
        private Random _random;

        //constructor for questions
        public QuizManager()
        {
            _allQuestions = new List<QuizQuestion>();
            _questions = new List<QuizQuestion>();
            _random = new Random();

            InitializeQuestions();
            InitializeRandomQuiz();
        }
        //Initializes a new quiz with 10 random questions from the pool
        public void InitializeRandomQuiz()
        {
            // Shuffle all questions and select 10 random ones
            _questions = _allQuestions
                .OrderBy(x => _random.Next())
                .Take(10)
                .ToList();

            _currentIndex = 0;
            _score = 0;
        }

        //returns current question
        public QuizQuestion GetCurrentQuestion()
        {
            if (_currentIndex < _questions.Count)
            {
                return _questions[_currentIndex];
            }
            return null;
        }

        //check against CorrectAnswer,
        // increment _score if correct, increment _currentIndex,
        // return true if correct, otherwise false
        public bool SubmitAnswer(string answer)
        {
            if (_currentIndex >= _questions.Count)
                return false;

            QuizQuestion currentQuestion = _questions[_currentIndex];
            bool isCorrect = currentQuestion.CorrectAnswer.Equals(answer, StringComparison.OrdinalIgnoreCase);

            if (isCorrect)
            {
                _score++;
            }

            _currentIndex++;
            return isCorrect;
        }

        // GetFeedback(bool correct): return Explanation string
        public string GetFeedback()
        {
            if (_currentIndex - 1 >= 0 && _currentIndex - 1 < _questions.Count)
            {
                return _questions[_currentIndex - 1].Explanation;
            }
            return string.Empty;
        }


        // checks if quiz is finished
        public bool IsFinished()
        {
            return _currentIndex >= _questions.Count;
        }
        //returns score out of total
        public string GetFinalScore() 
        {
            return $"{_score}/{_questions.Count}";
        }

        //returns final message based on the user's performance
        public string GetFinalMessage()
        {
            if (_questions.Count == 0)
                return "No questions available.";

            double percentage = (_score / (double)_questions.Count) * 100;

            if (percentage >= 90)
                return "Excellent! You're a Cybersecurity expert!";
            else if (percentage >= 80)
                return "Great job! You have strong Cybersecurity awareness!";
            else if (percentage >= 70)
                return "Good effort! You understand the basics. Keep learning!";
            else if (percentage >= 60)
                return "You're on the right track. Review cybersecurity best practices.";
            else
                return "Keep learning! CyberSecurity awareness is important.";
        }
        // Reset quiz to start
        public void ResetQuiz()
        {
            InitializeRandomQuiz();
        }

        //get total number of questions
        public int GetTotalQuestions()
        {
            return _questions.Count;
        }

        //get current question number
        public int GetCurrentQuestionNumber()
        {
            return _currentIndex + 1;
        }
        
        //returns users current score
        public int GetCurrentScore()
        {
            return _score;
        }
        //get score percentage
        public double GetScorePercentage()
        {
            if (_questions.Count == 0)
                return 0;
            return (_score / (double)_questions.Count) * 100;
        }

        private void InitializeQuestions()
        {
            // Phishing - Multiple Choice questions
            _allQuestions.AddRange(new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Topic = "Phishing",
                    Question = "What should you do if you receive an email asking for your password?",
                    Options = new List<string> { "Reply with your password", "Delete the email", "Report the email as phishing", "Ignore it" },
                    CorrectAnswer = "C",
                    Explanation = "Correct! Reporting phishing emails helps prevent scams. Legitimate companies never ask for passwords via email.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Topic = "Phishing",
                    Question = "Which of these is a red flag for a phishing email?",
                    Options = new List<string> { "Sender's email address looks slightly different from the official one", "Email asks you to verify your account urgently", "Email contains spelling and grammar errors", "All of the above" },
                    CorrectAnswer = "D",
                    Explanation = "Correct! All these are common phishing tactics. Always scrutinize emails carefully before clicking links or providing information.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Topic = "Phishing",
                    Question = "What is the safest way to verify if an email from your bank is legitimate?",
                    Options = new List<string> { "Click the link in the email", "Call the phone number provided in the email", "Call your bank using the number on your bank card", "Reply to the email asking for confirmation" },
                    CorrectAnswer = "C",
                    Explanation = "Correct! Always verify by contacting the organization directly using a known contact number. Never use information provided in suspicious emails.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Topic = "Phishing",
                    Question = "Phishing emails often create a sense of urgency. What is the best response?",
                    Options = new List<string> { "Act immediately as instructed", "Take time to verify the email's authenticity", "Share it with colleagues to get their opinion", "Forward it to the sender asking for clarification" },
                    CorrectAnswer = "B",
                    Explanation = "Correct! Scammers use urgency to bypass your critical thinking. Always take time to verify suspicious emails before taking action.",
                    IsTrueFalse = false
                }
            });

            // Password Safety - True/False and Multiple Choice questions
            _allQuestions.AddRange(new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Topic = "Password Safety",
                    Question = "It is safe to use the same password for multiple accounts as long as it is strong.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "False",
                    Explanation = "Correct! Even strong passwords should be unique for each account. If one is compromised, all accounts are at risk.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Topic = "Password Safety",
                    Question = "A strong password should include:",
                    Options = new List<string> { "Only uppercase letters", "A mix of uppercase, lowercase, numbers, and special characters", "Your name or birthdate for easy remembering", "At least 4 characters" },
                    CorrectAnswer = "B",
                    Explanation = "Correct! Strong passwords require diversity and length. Avoid personal information and short passwords as they are easily guessed.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Topic = "Password Safety",
                    Question = "You should write down your passwords on a sticky note and keep it under your keyboard.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "False",
                    Explanation = "Correct! Writing passwords down makes them vulnerable to theft. Use a password manager to securely store your passwords.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Topic = "Password Safety",
                    Question = "How often should you change your passwords?",
                    Options = new List<string> { "Every day", "Every month", "Only when you suspect compromise", "Only when you receive a notification" },
                    CorrectAnswer = "C",
                    Explanation = "Correct! Regular password changes are less important now than regularly monitoring for breaches. However, change immediately if you suspect compromise.",
                    IsTrueFalse = false
                }
            });

            // Safe Browsing - Multiple Choice questions
            _allQuestions.AddRange(new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Topic = "Safe Browsing (HTTPS, Public Wi-Fi)",
                    Question = "What does the 'S' in HTTPS stand for?",
                    Options = new List<string> { "Secure", "Standard", "System", "Server" },
                    CorrectAnswer = "A",
                    Explanation = "Correct! HTTPS means the connection is encrypted and secure. Always look for HTTPS when entering sensitive information online.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Topic = "Safe Browsing (HTTPS, Public Wi-Fi)",
                    Question = "Is it safe to access your bank account on public Wi-Fi?",
                    Options = new List<string> { "Yes, if you have antivirus software", "No, public Wi-Fi can be intercepted", "Yes, if the website uses HTTPS", "Only if you use incognito mode" },
                    CorrectAnswer = "B",
                    Explanation = "Correct! Public Wi-Fi is inherently unsafe. Use a VPN if you must access sensitive accounts on public networks.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Topic = "Safe Browsing (HTTPS, Public Wi-Fi)",
                    Question = "What is a VPN and why should you use one on public Wi-Fi?",
                    Options = new List<string> { "Virtual Private Network; it encrypts your data and hides your activity", "Very Private Network; it blocks all ads", "Virtual Protection Network; it deletes cookies", "Video Personal Network; it improves streaming" },
                    CorrectAnswer = "A",
                    Explanation = "Correct! A VPN encrypts your internet traffic, protecting you from interceptors on public Wi-Fi networks.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Topic = "Safe Browsing (HTTPS, Public Wi-Fi)",
                    Question = "Before entering your password on a website, you should check for:",
                    Options = new List<string> { "A padlock icon and HTTPS in the URL", "A green background", "The website's logo", "The number of visitors" },
                    CorrectAnswer = "A",
                    Explanation = "Correct! The padlock icon and HTTPS indicate an encrypted, secure connection where your password will be protected.",
                    IsTrueFalse = false
                }
            });

            // Social Engineering - True/False questions
            _allQuestions.AddRange(new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Topic = "Social Engineering",
                    Question = "Social engineering is a method of hacking that manipulates people into revealing confidential information.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "True",
                    Explanation = "Correct! Social engineering exploits human psychology rather than technical vulnerabilities to gain access to information.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Topic = "Social Engineering",
                    Question = "A person claiming to be from IT support asks you to verify your password over the phone. You should do this.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "False",
                    Explanation = "Correct! Legitimate IT support will never ask for your password over the phone. This is a classic social engineering tactic.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Topic = "Social Engineering",
                    Question = "Pretexting is a social engineering tactic where someone creates a false scenario to gain trust.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "True",
                    Explanation = "Correct! Pretexting involves fabricating scenarios (e.g., posing as a contractor or urgent situation) to manipulate someone.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Topic = "Social Engineering",
                    Question = "It is safe to discuss sensitive company information with a friendly stranger in a coffee shop.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "False",
                    Explanation = "Correct! Social engineers often pose as friendly individuals to gather information. Always be cautious about discussing sensitive topics in public.",
                    IsTrueFalse = true
                }
            });

            // Two-Factor Authentication - Multiple Choice questions
            _allQuestions.AddRange(new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Topic = "Two-Factor Authentication",
                    Question = "What is two-factor authentication (2FA)?",
                    Options = new List<string> { "Using two different passwords", "A security process requiring two types of verification", "Having two email accounts", "Logging in twice a day" },
                    CorrectAnswer = "B",
                    Explanation = "Correct! 2FA requires two different types of verification, making your account much harder to compromise even if your password is stolen.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Topic = "Two-Factor Authentication",
                    Question = "Which of these is NOT a valid 2FA method?",
                    Options = new List<string> { "SMS code to your phone", "Fingerprint scan", "Security questions about your childhood", "Authentication app like Google Authenticator" },
                    CorrectAnswer = "C",
                    Explanation = "Correct! Security questions alone are not 2FA. True 2FA uses two independent verification methods like something you know and something you have.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Topic = "Two-Factor Authentication",
                    Question = "What should you do if someone asks for your 2FA code?",
                    Options = new List<string> { "Share it only with people you trust", "Never share it; it's personal to your account", "Share it with IT support if requested", "Share it to help friends access their accounts" },
                    CorrectAnswer = "B",
                    Explanation = "Correct! Never share your 2FA code with anyone, not even support staff. The code is meant to verify only your identity.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Topic = "Two-Factor Authentication",
                    Question = "Why is 2FA considered more secure than just a password?",
                    Options = new List<string> { "Because it requires more time to set up", "Because it prevents all cyber attacks", "Because even if your password is compromised, attackers need the second factor", "Because it changes your password automatically" },
                    CorrectAnswer = "C",
                    Explanation = "Correct! 2FA adds an extra layer of security. Even if someone steals your password, they cannot access your account without the second verification method.",
                    IsTrueFalse = false
                }
            });

            // Malware and Ransomware - True/False questions
            _allQuestions.AddRange(new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Topic = "Malware and Ransomware",
                    Question = "Ransomware is malicious software that encrypts your files and demands payment to restore access.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "True",
                    Explanation = "Correct! Ransomware locks your data and threatens deletion unless you pay. Prevention and backups are your best defense.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Topic = "Malware and Ransomware",
                    Question = "You can safely download software from any website as long as it has a good design.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "False",
                    Explanation = "Correct! Professional-looking websites can still distribute malware. Only download from official sources or trusted vendors.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Topic = "Malware and Ransomware",
                    Question = "Opening an email attachment from an unknown sender is generally safe.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "False",
                    Explanation = "Correct! Malware is often distributed through email attachments. Never open attachments from unknown or suspicious senders.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Topic = "Malware and Ransomware",
                    Question = "Keeping your operating system and software up-to-date helps protect against malware.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "True",
                    Explanation = "Correct! Updates patch security vulnerabilities that malware exploits. Always install security updates promptly.",
                    IsTrueFalse = true
                }
            });

            // Privacy Settings - Multiple Choice questions
            _allQuestions.AddRange(new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Topic = "Privacy Settings",
                    Question = "What should you do with your social media profile to protect your privacy?",
                    Options = new List<string> { "Share all personal information publicly", "Review and restrict privacy settings", "Use your real address as your username", "Accept all friend requests" },
                    CorrectAnswer = "B",
                    Explanation = "Correct! Regularly review your privacy settings to control who sees your personal information. Limit visibility to trusted contacts.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Topic = "Privacy Settings",
                    Question = "It is safe to share your location in real-time on social media.",
                    Options = new List<string> { "Yes, everyone does it", "Yes, if your profile is private", "No, it reveals when you are away from home", "Only on weekends" },
                    CorrectAnswer = "C",
                    Explanation = "Correct! Sharing your location tells people you are not at home, making you a target for theft. Disable location sharing when possible.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Topic = "Privacy Settings",
                    Question = "What personal information should you avoid posting on social media?",
                    Options = new List<string> { "Photos of your cat", "Your full birthdate", "Your favorite book", "Your hobbies" },
                    CorrectAnswer = "B",
                    Explanation = "Correct! Full birthdates can be used for identity theft or to answer security questions. Avoid sharing sensitive personal data.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Topic = "Privacy Settings",
                    Question = "How often should you review the privacy settings of your online accounts?",
                    Options = new List<string> { "Once a year", "Never, they don't change", "Quarterly or after platform updates", "Only when you change your password" },
                    CorrectAnswer = "C",
                    Explanation = "Correct! Review privacy settings regularly as platforms often update their policies and settings. Stay informed about who can access your data.",
                    IsTrueFalse = false
                }
            });

            // Data Backup - Multiple Choice questions
            _allQuestions.AddRange(new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Topic = "Data Backup",
                    Question = "What is the best reason to regularly backup your data?",
                    Options = new List<string> { "To free up storage space", "To protect against data loss from hardware failure, malware, or accidents", "To make your computer faster", "To increase your internet speed" },
                    CorrectAnswer = "B",
                    Explanation = "Correct! Regular backups protect your important data from various threats including hardware failure, ransomware, and accidental deletion.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Topic = "Data Backup",
                    Question = "Which backup strategy is most secure?",
                    Options = new List<string> { "Storing all backups on the same computer", "The 3-2-1 rule: 3 copies of data, 2 different media types, 1 offsite", "Backing up only once a year", "Emailing your files to yourself" },
                    CorrectAnswer = "B",
                    Explanation = "Correct! The 3-2-1 backup strategy ensures redundancy and protection: multiple copies, different storage types, and at least one offsite backup.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Topic = "Data Backup",
                    Question = "You should test your backups periodically to ensure they work.",
                    Options = new List<string> { "Yes, always test them regularly", "No, testing is unnecessary", "Only if you suspect data loss", "Only once when you set them up" },
                    CorrectAnswer = "A",
                    Explanation = "Correct! Regularly test your backups to confirm they can be restored. A backup is only useful if it actually works when needed.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Topic = "Data Backup",
                    Question = "Where should you store offline backup copies?",
                    Options = new List<string> { "Next to your computer for easy access", "At a separate secure location (off-site)", "In the cloud only", "In your email attachments" },
                    CorrectAnswer = "B",
                    Explanation = "Correct! Store offline backups at a separate location to protect against physical threats like theft, fire, or flooding affecting your workspace.",
                    IsTrueFalse = false
                }
            });

            // General Cybersecurity - Mixed questions
            _allQuestions.AddRange(new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Topic = "General Cybersecurity",
                    Question = "What should you do if you suspect your computer has been infected with malware?",
                    Options = new List<string> { "Continue using it normally", "Disconnect from the internet and contact IT support", "Delete your important files", "Share it with someone to help" },
                    CorrectAnswer = "B",
                    Explanation = "Correct! Immediately disconnect from the internet to prevent the malware from spreading and contact IT support for assistance in removing it.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Topic = "General Cybersecurity",
                    Question = "Is it safe to use the same username and password across multiple devices?",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "False",
                    Explanation = "Correct! Using the same credentials across devices increases risk. If one device is compromised, all accounts are vulnerable. Use unique credentials per device.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Topic = "General Cybersecurity",
                    Question = "What is encryption?",
                    Options = new List<string> { "A way to delete data permanently", "The process of converting information into code to prevent unauthorized access", "A type of antivirus software", "A backup method for important files" },
                    CorrectAnswer = "B",
                    Explanation = "Correct! Encryption converts data into unreadable code without the correct decryption key. It protects sensitive information from unauthorized viewing.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Topic = "General Cybersecurity",
                    Question = "Unknown USB devices connected to your computer are completely safe to use.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "False",
                    Explanation = "Correct! Never insert unknown USB devices as they may contain malware. Always verify the source and owner before using any USB device.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Topic = "General Cybersecurity",
                    Question = "What is a firewall?",
                    Options = new List<string> { "A physical barrier around your computer", "A software or hardware system that monitors and controls network traffic", "An antivirus program", "A type of password manager" },
                    CorrectAnswer = "B",
                    Explanation = "Correct! A firewall acts as a security barrier between your device and the internet, controlling incoming and outgoing network traffic.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Topic = "General Cybersecurity",
                    Question = "Should you share your security credentials with colleagues to help them access shared resources?",
                    Options = new List<string> { "Yes, it helps with efficiency", "No, never share credentials; use proper access controls", "Only for administrative passwords", "Only with people you have known for years" },
                    CorrectAnswer = "B",
                    Explanation = "Correct! Never share your credentials. Instead, use proper access management systems and request that IT grant colleagues appropriate permissions.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Topic = "General Cybersecurity",
                    Question = "Antivirus software provides 100% protection against all cyber threats.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "False",
                    Explanation = "Correct! While antivirus software is important, it's not foolproof. Use it with other security practices like caution with emails and regular updates for defense-in-depth.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Topic = "General Cybersecurity",
                    Question = "What is the best practice for your home network security?",
                    Options = new List<string> { "Leave your Wi-Fi open for guests", "Use a strong password and enable WPA3 encryption", "Change your router password to something simple", "Disable your router's firewall for better speed" },
                    CorrectAnswer = "B",
                    Explanation = "Correct! Secure your home network with a strong password and modern encryption (WPA3 if available). This prevents unauthorized access to your devices.",
                    IsTrueFalse = false
                }
            });
        }

    }
}
