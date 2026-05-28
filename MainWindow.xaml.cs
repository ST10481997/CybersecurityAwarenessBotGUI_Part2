using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;



namespace CybersecurityAwarenessBotGUI_Part2
{//start of namespace
    
    public partial class MainWindow : Window
    {//start of class

        string name = "";
        Random random = new Random();
        string memoryFile = "memory.txt";
        string currentTopic = "";

        private static readonly List<string> QuickTopics = new List<string>
        {
            "password", "phishing", "scam", "privacy",
            "malware", "ransomware", "2fa", "encryption",
            "safe browsing", "firewalls", "social engineering",
            "cybersecurity tips", "help"
        };


        Dictionary<string, string[]> cyberResponses =

            new Dictionary<string, string[]>()
            {

               {
                    "phishing",
                    new string[]
                    {
                         "Fraudulent Communication for Data Theft\r\nPhishing involves sending deceptive messages, often via email or text, that appear to come from legitimate sources. The attacker’s goal is to steal sensitive information such as login credentials, financial data, or to install malware on the victim’s device.",
                         "Social Engineering Exploiting Human Trust\r\nPhishing is a form of social engineering that manipulates human psychology. Attackers exploit trust, curiosity, or urgency to trick victims into revealing confidential information, rather than exploiting technical vulnerabilities.",
                         "Impersonation of Legitimate Entities\r\nPhishers disguise themselves as banks, government agencies, online platforms, or even colleagues. By mimicking trusted entities, they convince victims to provide personal information or click on malicious links.",
                         "Multiple Attack Channels\r\nPhishing is not limited to email. It can occur through phone calls (vishing), text messages (smishing), social media, or fake websites. Each channel uses deception to achieve the same goal: obtaining sensitive data. "
                    }
               },

                {
                    "malware",
                    new string[]
                    {
                        "This type of malware encrypts the victim's files and demands a ransom payment to restore access. A notable example is the WannaCry ransomware attack, which affected thousands of computers worldwide.",
                        "Spyware secretly monitors user activity and collects personal information without consent. It can track browsing habits, capture keystrokes, and gather sensitive data like passwords.",
                        "This malware disguises itself as legitimate software to trick users into installing it. Once activated, it can create backdoors for other malicious software. An example is the Zeus Trojan, which targets banking information",
                        "While not always harmful, adware displays unwanted advertisements and can slow down system performance. Some adware can also track user behavior and collect data for targeted advertising."
                    }

                },

                {
                    "cyber threats",
                    new string[]
                    {

                    }
                },

                {
                    "safe browsing",
                    new string[]
                    {

                    }
                },

                {
                    "ransomware",
                    new string[]
                    {

                    }
                },

                {
                    "social engineering",
                    new string[]
                    {

                    }
                },

                {
                    "password safety",
                    new string[]
                    {

                    }
                },

                {
                    "safe browsing",
                    new string[]
                    {

                    }
                },

                {
                    "2fa",
                    new string[]
                    {

                    }
                },
                {
                    "data breaches",
                    new string[]
                    {

                    }
                },
                {
                    "firewalls",
                    new string[]
                    {

                    }
                },

                {
                    "encryption",
                    new string[]
                    {

                    }
                },

                {
                    "password security",
                    new string[]
                    {

                    }
                }
            };

        Dictionary<string, string[]> topicKeyWord = new Dictionary<string, string[]>()
        {
            { "phishing", new string[]{ "fake emails", "suspicious email", "phishing" } },
            { "malware", new string[]{ "virus", "spyware", "adware", "trojan", "malware"} },
            { "cyber threats", new string[]{ ""} },
            { "password safety", new string[]{ ""} },
            { "2fa", new string[]{ ""} },
            { "ransomware", new string[]{ ""} },
            { "safe browsing", new string[]{ ""} },
            { "social enginnering", new string[]{ ""} },
            { "data breaches", new string[]{ ""} },
            { "firewalls", new string[]{ ""} },
            { "encryption", new string[]{ ""} },
            { "cpassword security", new string[]{ ""} },
        };

       
        public MainWindow()
        {//start of constructor

            InitializeComponent();
            
        }//end of constructor

        //Window Controls 
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }

        private void minimise_button(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void close_buttonclick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        //Login 
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {//start of  method
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }
        private void CloseBtn_Click(object sender, RoutedEventArgs e) =>
            Application.Current.Shutdown();

        private void NameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) login();
        }
        private void EnterButton_Click(object sender, RoutedEventArgs e) =>login();
        
        public void login()
        {
            string username = NameBox.Text.Trim().ToUpper();

            if(string.IsNullOrEmpty(username))
            {
                ShowError("Name cannot be empty, please add a valid name");
                return;               
            }
            if(username.Length<2)
            {
                ShowError("Name must be at least 2 characters long.");
                return;
            }
            if(!Regex.IsMatch(username, @"^[a-zA-Z]+$"))
            {
                ShowError("Name must only contain letters (A-Z), spaces or hyphens");
                return;
            }
                       

            name = username;
            ValidationMsg.Visibility = Visibility.Collapsed;

            logo_grid.Visibility = Visibility.Hidden;
            robotface_panel.Visibility = Visibility.Hidden;
            loginform_panel.Visibility = Visibility.Hidden;

            chat_grid.Visibility = Visibility.Visible;

            chats_box.AppendText($"HAPPYCODER: Welcome to AI assistance. \nHow may I assist you today Mr\\\\Mrs: {name}!\n\n");

            
        }

        private void ShowError(string msg)
        {//start of show error
            ValidationMsg.Text = msg;
            ValidationMsg.Visibility = Visibility.Visible;
            NameBox.Focus();
        }//end of show error

        //send a message/question
        private void send_question(object sender, RoutedEventArgs e)
        {
            string message = chats_box.Text.Trim().ToLower();
            if(string.IsNullOrEmpty(message))
            {
                chats_box.AppendText($"HAPPYCODER: Sorry i didn't understand that\n");
                question_box.Clear();
                return;
            }

            chats_box.AppendText($"{name}: {chats_box}\n");
            if(message.Contains("interested in"))
            {
                SaveToFile(message);
                
            }else if(message.Contains("favourite topic"))
            {
                if(File.Exists(memoryFile))
                {
                    string savedTopic = File.ReadAllText(memoryFile);
                    chats_box.AppendText($"HAPPYCODER: Your favourite topic is: {savedTopic}\n");
                }
                else
                {
                    chats_box.AppendText($"I don't know what your favourite topic is yet");
                }
                question_box.Clear() ;
                return;
                
            }

            string botResponse = chatbotResponse(message);
            chats_box.AppendText($"HAPPYCODER: {botResponse} \n\n");

            question_box.Clear();

        }


        public string chatbotResponse(string message)
        {
            string sentiment = DetectSentiment(message);
            bool moreInfor = isFollowUp(message);
            string topic = DetectTopic(message);

            if (string.IsNullOrEmpty(topic) && moreInfor && !string.IsNullOrEmpty(currentTopic))
            {
                topic = currentTopic;
            }

            if (!string.IsNullOrEmpty(topic))
            {
                currentTopic = topic;
                return BuildResponses(topic, sentiment, moreInfor);
            }

            if (!string.IsNullOrEmpty(sentiment))
            {
                return $"{GetSentimentSupport(sentiment)}, Tell me which cybersecurity topic is bothering you, such as phishing, malware, or scams, and  I will assit step by step";
            }

            return "I am build to respond to cybersecurity related questions\n";
        }

        public string DetectTopic(string message)
        {
            foreach (var topic in topicKeyWord)
            {
                if (topic.Value.Any(word => message.Contains(word)))
                {
                    return topic.Key;
                }
            }
            foreach (var topic in cyberResponses)
            {
                if (message.Contains(topic.Key))
                {
                    return topic.Key;
                }
            }
            return "";
        }

        public string BuildResponses(string topic, string sentiment, bool moreInfor)
        {
            string[] foundResponce = cyberResponses[topic];
            int index = random.Next(foundResponce.Length);
            string responce = foundResponce[index];
            string support = GetSentimentSupport(sentiment);

            return responce;
        }

        public string GetSentimentSupport(string sentiment)
        {
            if (sentiment == "worried")
            {
                return $"Hey {name}, it's completely understandable to feel that way. Cybersecurity threats can seem overwhelming, but few careful habit can protect you.";
            }

            if (sentiment == "frustrated")
            {
                return $"Hey {name}, I know this can feel frustrating. let's slow down and focus on one practical step at a time";
            }
            return "";
        }

        public string DetectSentiment(string message)
        {
            if (message.Contains("worried") ||
                message.Contains("anxious") ||
                message.Contains("nervous") ||
                message.Contains("unsure") ||
                message.Contains("afraid"))
            {
                return "worried";
            }

            if (message.Contains("frustrated") ||
                message.Contains("annoyed") ||
                message.Contains("angry") ||
                message.Contains("confused") ||
                message.Contains("stuck"))
            {
                return "frustrated";
            }
            return "";
        }

        public bool isFollowUp(string message)
        {
            return message.Contains("explain more") ||
                message.Contains("more details") ||
                message.Contains("i did not understand");
        }

        public void SaveToFile(string message)
        {
            if (message.Contains("interested    q in"))
            {
                string topic = message.Replace("i am interested in", "").Trim();
                File.WriteAllText(memoryFile, topic);

                chats_box.AppendText($"Chatbot: I will remember that your favorite topic is {topic}\n");
            }
        }

        private void clear_chat(object sender, RoutedEventArgs e)
        {
            
        }

        private void memory_recap(object sender, RoutedEventArgs e)
        {

        }

        private void voice_toggle(object sender, RoutedEventArgs e)
        {

        }

        private void close_button(object sender, RoutedEventArgs e) => Application.Current.Shutdown();


        

        
    }//end of class
}//end of namespace

