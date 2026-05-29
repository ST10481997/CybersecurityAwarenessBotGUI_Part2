using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Speech.Synthesis;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;



namespace CybersecurityAwarenessBotGUI_Part2
{//start of namespace
    
    public partial class MainWindow : Window
    {//start of class[

        string name = "";
        string favouriteTopic = "";
        string currentTopic = "";
        Random random = new Random();
        string memoryFile = "memory.txt";

        //private SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
        //private bool voiceEnabled = false;

        
        Dictionary<string, string[]> cyberResponses =

            new Dictionary<string, string[]>()
            {

               {
                    "phishing",
                    new string[]
                    {

                         " Never click links in suspicious emails. Hover over the link to see the real URL. When in doubt, type the website address manually.",
                         " If an email claims you've won a prize or threatens account closure, that's a major red flag. Contact the company directly using official channels.",
                         " Phishing attacks often mimic login pages. Always check that the URL starts with 'https://' and the site certificate is valid."                    }
              
                },

                {
                    "malware",
                    new string[]
                    {

                         " Keep your operating system and antivirus software updated. Enable automatic updates if possible.",
                         " Don't download software from torrent sites or pop-up ads. Use official app stores or developer websites.",
                         " Be wary of email attachments, even from known senders, if they are unexpected. Malware often spreads via macro-enabled documents."                   }

                },

                {
                    "ransomware",
                    new string[]
                    {

                         " Maintain offline backups. Ransomware can't encrypt data that isn't connected to your computer.",
                         " Do not pay the ransom. It encourages criminals and there's no guarantee you'll get your files back.",
                         " Disable macros in Office files and use application whitelisting to block unknown executables."

                    }
                },

                {
                    "social engineering",
                    new string[]
                    {

                         " Always verify identities through a different channel. If someone calls claiming to be IT support, hang up and call the official number.",
                         " Be suspicious of urgent requests from 'bosses' or 'vendors' asking for gift cards or wire transfers. These are classic scams.",
                         " Never share your password over the phone, even if the caller says they're from 'security'. Real security teams will never ask for it."


                    }
                },

                {
                    "password safety",
                    new string[]
                    {

                        " Use a password manager to generate and store unique 12+ character passwords. Never reuse passwords across sites.",
                        " Enable two-factor authentication (2FA) on all accounts that support it. This adds a critical extra layer of security.",
                        " Avoid using personal info (birthdays, names) in passwords. Instead, use a phrase like 'BlueCoffee$42!Running'."

                    }
                },

                {
                    "safe browsing",
                    new string[]
                    {

                        " Look for the padlock icon in the address bar before entering any personal information on a website.",
                        " Regularly clear your browser cache, cookies, and history to reduce tracking and free up space.",
                        " Use a search engine that doesn't track you (like DuckDuckGo) and enable 'Do Not Track' in your browser settings."

                    }
                },

                {
                    "2fa",
                    new string[]
                    {

                        " Set up 2FA using an authenticator app (Google Authenticator, Authy) rather than SMS – it's more secure against SIM swapping.",
                        " Backup codes are essential. Store them in a safe place (not on your primary device) in case you lose access to your 2FA method.",
                        " Never share your 2FA codes with anyone, even if they claim to be tech support. Real services never ask for your one-time code."

                    }
                },
                {
                    "data breaches",
                    new string[]
                    {

                        " Check if your email has been in a breach using 'haveibeenpwned.com'. Change passwords immediately for affected accounts.",
                        " Use unique passwords for every service – if one site gets breached, other accounts remain safe.",
                        " Enable login alerts and monitor your bank/credit card statements regularly for unauthorized transactions."

                    }
                },
                {
                    "firewalls",
                    new string[]
                    {

                         " A firewall monitors incoming and outgoing traffic. Keep Windows Defender Firewall or your router's firewall enabled at all times.",
                         " For advanced protection, consider a next-gen firewall (NGFW) that includes intrusion prevention and application control.",
                         " On public Wi-Fi, your firewall is still important, but also use a VPN to encrypt all traffic beyond basic filtering."

                    }
                },

                {
                    "encryption",
                    new string[]
                    {

                        " Full-disk encryption (BitLocker on Windows, FileVault on Mac) protects your data if your device is lost or stolen.",
                        " Use end-to-end encrypted messaging apps (Signal, WhatsApp) for sensitive conversations. Email is not encrypted by default.",
                        " For cloud storage, use client-side encryption tools like Cryptomator or VeraCrypt before uploading files."

                    }
                }
            };

        Dictionary<string, string[]> topicKeyWord = new Dictionary<string, string[]>()
        {
            { "phishing", new string[]{ "fake emails", "suspicious email", "phishing" } },
            { "malware", new string[]{ "virus", "spyware", "adware", "trojan", "malware"} },
            { "password", new string[]{ " password", "passphrase", "login", "account security", "strong password" } },
            { "2fa", new string[]{ "2fa", "two factor", "multi factor", "mfa", "authenticator" } },
            { "ransomware", new string[]{ "ransomware", "encrypt files", "pay ransom" } },
            { "safe browsing", new string[]{ " safe browsing", "secure browsing", "https", "safe website" } },
            { "social enginnering", new string[]{ " social engineering", "manipulation", "pretend", "impersonate" } },
            { "data breaches", new string[]{ "  data breach", "leak", "compromised", "hacked database" } },
            { "firewalls", new string[]{ " firewall", "network security", "block traffic" } },
            { "encryption", new string[]{ " encryption", "encrypt", "decrypt", "cipher" } },
        };

        //Quick topics
        private static readonly List<string> QuickTopics = new List<string>
        {
            "password", "phishing", "scam", "privacy",
            "malware", "ransomware", "2fa", "encryption",
            "safe browsing", "firewalls", "social engineering",
            "cybersecurity tips", "help"
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

