using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Media;



namespace CybersecurityAwarenessBotGUI_Part2
{//start of namespace

    public partial class MainWindow : Window
    {//start of class[

        string name = "";
        string favouriteTopic = "";
        string currentTopic = "";
        Random random = new Random();
        string memoryFile = "memory.txt";

        private SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
        private bool voiceEnabled = false;


        Dictionary<string, string[]> cyberResponses =

            new Dictionary<string, string[]>()
            {

               {
                    "phishing",
                    new string[]
                    {

                         " Never click links in suspicious emails. Hover over the link to see the real URL. When in doubt, type the website address manually.",
                         " If an email claims you've won a prize or threatens account closure, that's a major red flag. Contact the company directly using official channels.",
                         " Phishing attacks often mimic login pages. Always check that the URL starts with 'https://' and the site certificate is valid."
                    }

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
            Loaded += MainWindow_Loaded;
            LoadMemoryFromFile();


        }//end of constructor

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Populate quick topic buttons
            foreach (string topic in QuickTopics)
            {
                Button btn = new Button
                {
                    Content = topic.ToUpper(),
                    Margin = new Thickness(0, 4, 0, 0),
                    Style = (Style)FindResource("CyberButton"),
                    Height = 32,
                    FontSize = 12
                };
                btn.Click += (s, ev) =>
                {
                    question_box.Text = $"Tell me about {topic}";
                    send_question(null, null);
                };
                QuickTopicPanel.Children.Add(btn);
            }


            loginborder.Visibility = Visibility.Visible;
            chatborder.Visibility = Visibility.Collapsed;


        }

        private bool hasPlayedGreeting=false;
        public void voiceGreeting()
        {
            if (hasPlayedGreeting) return;
            hasPlayedGreeting = true;

            try
            {
                string audioFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Audio", "HAPPYCODERAWARENESSBOT.wav");
                if (System.IO.File.Exists(audioFile))
                {
                    SoundPlayer player = new SoundPlayer(audioFile);
                    player.Play();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Audio file not found: " + audioFile);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("voiceGreeting error: " + ex.Message);
            }

        }

        //Window Controls 
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }

        private void minimise_button(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void close_buttonclick(object sender, RoutedEventArgs e) => Application.Current.Shutdown();


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
        private void EnterButton_Click(object sender, RoutedEventArgs e) => login();

        public void login()
        {
            string username = NameBox.Text.Trim().ToUpper();
            SidebarUserName.Text = username.ToUpper();

            if (string.IsNullOrEmpty(username))
            {
                ShowError("Name cannot be empty, please add a valid name");
                return;
            }
            if (username.Length < 2)
            {
                ShowError("Name must be at least 2 characters long.");
                return;
            }
            if (!Regex.IsMatch(username, @"^[a-zA-Z]+$"))
            {
                ShowError("Name must only contain letters (A-Z), spaces or hyphens");
                return;
            }


            name = username;
            ValidationMsg.Visibility = Visibility.Collapsed;


            loginborder.Visibility = Visibility.Collapsed;
            chatborder.Visibility = Visibility.Visible;
            SidebarUserName.Text = name;

            chats_box.AppendText($"HAPPYCODER: Welcome {name}! \n");
            chats_box.AppendText($" I'm your cybersecurity assistant. Ask me about passwords, scams, privacy, phishing, and more.\n");
            chats_box.AppendText($" You can also say 'I'm interested in [topic]' and I'll remember it. \n\n");

            //play the intro sound
            voiceGreeting();
            question_box.Focus();

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
            string message = question_box.Text.Trim();
            if (string.IsNullOrEmpty(message))
            {
                chats_box.AppendText("HAPPYCODER: Please enter a message.\n");
                return;
            }

            // Show user message correctly
            chats_box.AppendText($"{name}: {message}\n");
            question_box.Clear();

            // Capture "I'm interested in ..."
            if (message.ToLower().Contains("interested in"))
            {
                SaveToFile(message);
                return;  // let SaveToFile also show the bot's response
            }

            // Get bot response
            string botResponse = chatbotResponse(message);
            chats_box.AppendText($"HAPPYCODER: {botResponse}\n\n");

            // Voice output
            if (voiceEnabled)
            {
                try { speechSynthesizer.SpeakAsync(botResponse); }
                catch { }
            }

            chats_scroll.ScrollToEnd();

        }


        public string chatbotResponse(string message)
        {

            if(message.Trim().Contains("help"))
            {
                return "You can ask me about any of these topics:\n\n" +
               "  • password / password safety / password security\n" +
               "  • phishing / phishing tips\n" +
               "  • scam / scams\n" +
               "  • privacy\n" +
               "  • safe browsing\n" +
               "  • malware\n" +
               "  • ransomware\n" +
               "  • social engineering\n" +
               "  • two-factor authentication / 2fa\n" +
               "  • data breaches\n" +
               "  • firewalls\n" +
               "  • encryption\n" +
               "  • cyber threats\n" +
               "  • cybersecurity tips\n\n" +
               "Just type the topic you're interested in!";
            }

            if (message.Trim().ToLower().Contains("purpose") ||
                message.Trim() == "options" ||
                message.Trim() == "what can you do")

            {
                return "I can provide information on various cybersecurity topics, " +
                "answer your questions, and help you stay safe online. " +
                "Just ask me anything related to cybersecurity!";

            }

            if(message.Trim().Contains("hello") ||
                message.Trim().ToLower() == "hi" ||
                message.Trim().ToLower() == "hey")
            {
                return "Hello! How can I assist you with cybersecurity today?";
            }

            if(message.Trim().Contains("how are you?") ||
                message.Trim() == "how are you doing?")
            {
                return "I am functioning perfectly! Ready to help you stay safe online. " +
                        "\nHow can I assist you today?";
            }
            if(message.Trim().Contains("what is your name"))
            {
                return "My name is HAPPYCODER, your cybersecurity awareness bot!" +
                        "\nHow can I assist you today?";
            }
            

            string sentiment = DetectSentiment(message);
            UpdateSentimentUI(sentiment);

            bool moreInfor = isFollowUp(message);

            string topic = DetectTopic(message);


            //continue previous topic if user asks for more
            if (string.IsNullOrEmpty(topic) && moreInfor && !string.IsNullOrEmpty(currentTopic))
            {
                topic = currentTopic;
            }

            if (!string.IsNullOrEmpty(topic))
            {
                currentTopic = topic;
                return BuildResponses(topic, sentiment, moreInfor);
            }

            //sentiment without mentioned topic
            if (!string.IsNullOrEmpty(sentiment))
            {
                return $"{GetSentimentSupport(sentiment)}, Tell me which cybersecurity topic you'd like to learn about (e.g., passwords, phishing, privacy).";
            }

            return " I'm specialized in cybersecurity. Could you ask about passwords, phishing, scams, privacy, malware, or 2FA?";
        }

        public string DetectTopic(string message)
        {

            message = message.ToLower();

            // 1. Check keyword mapping
            foreach (var topic in topicKeyWord)
            {
                if (topic.Value.Any(word => message.Contains(word)))
                    return topic.Key;
            }

            // 2. Check direct topic names
            foreach (var topic in cyberResponses)
            {
                if (message.Contains(topic.Key.ToLower()))
                    return topic.Key;
            }

            return "";
        }


        private string BuildResponses(string topic, string sentiment,
                                     bool moreInfor)
        {
            //if topic is  not found in the response dictionary, returns general tip
            if (!cyberResponses.ContainsKey(topic))
            {
                return $"Let me give you a general cybersecurity tip: always keep your software updated and use strong, unique passwords.";
            }

            //random index
            string[] foundResponse = cyberResponses[topic];
            string chosenResponse = foundResponse[random.Next(foundResponse.Length)];

            //if user has a favourite topic
            string personalisation = "";
            if (!string.IsNullOrEmpty(favouriteTopic) && favouriteTopic.Equals(topic, StringComparison.OrdinalIgnoreCase))
            {
                personalisation = $"(As someone interested in {favouriteTopic}, here's a key point) ";
            }

            //provide a different tip from the same topic 

            if (moreInfor && foundResponse.Length > 1)
            {
                string alternative = foundResponse[random.Next(foundResponse.Length)];
                while (alternative == chosenResponse && foundResponse.Length > 1)
                    alternative = foundResponse[random.Next(foundResponse.Length)];
                chosenResponse = alternative;

            }

            //if a user is worried, frustrated, or scared the GetSentimentSupport method returns an empathetic sentence
            string supportPrefix = GetSentimentSupport(sentiment);
            if (!string.IsNullOrEmpty(supportPrefix))
                return $"{supportPrefix} {personalisation}{chosenResponse}";
            else
                return $"{personalisation}{chosenResponse}";

        }


        public string GetSentimentSupport(string sentiment)
        {

            if (sentiment == "worried")
            {
                return $"{name}, it's completely normal to feel worried about online threats. Let me share a practical step to help you feel more secure:";
            }

            if (sentiment == "frustrated")
            {
                return $"{name}, I understand this can be frustrating. Let's break it down simply:";
            }
            if (sentiment == "scared")
            {
                return $"{name}, I know that you are scared right now. Let's try and break this down and understaand what is happening:";
            }
            return "";
        }

        private void UpdateSentimentUI(string sentiment)
        {
            switch (sentiment)
            {
                case "worried":
                    MoodDot.Fill = new SolidColorBrush(Colors.Gold);
                    MoodLabel.Text = "Worried";
                    break;
                case "frustrated":
                    MoodDot.Fill = new SolidColorBrush(Colors.OrangeRed);
                    MoodLabel.Text = "Frustrated";
                    break;
                case "scared":
                    MoodDot.Fill = new SolidColorBrush(Colors.Red);
                    MoodLabel.Text = "Scared";
                    break;
                default:
                    MoodDot.Fill = new SolidColorBrush(Colors.SpringGreen);
                    MoodLabel.Text = "Neutral";
                    break;

            }
        }


        //sentiment detection
        public string DetectSentiment(string message)
        {
            message = message.ToLower();

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

            if (message.Contains("fear") ||
                message.Contains("unsure") ||
                message.Contains("panic") ||
                message.Contains("scared") ||
                message.Contains("mistake"))
            {
                return "scared";
            }
            return "";
        }

        public bool isFollowUp(string message)
        {
            return message.Contains("explain more") ||
                   message.Contains("more details") ||
                   message.Contains("i don't understand") ||
                   message.Contains("tell me more") ||
                   message.Contains("another tip") ||
                   message.Contains("give me another") ||
                   message.Contains("elaborate");
        }

        public void SaveToFile(string message)
        {
            if (message.ToLower().Contains("interested in"))
            {
                int idx = message.ToLower().IndexOf("interested in") + "interested in".Length;
                favouriteTopic = message.Substring(idx).Trim();
                File.WriteAllText(memoryFile, favouriteTopic);
                chats_box.AppendText($"HAPPYCODER: Great! I'll remember that you're interested in '{favouriteTopic}'.\n");
            }
        }

        private void LoadMemoryFromFile()
        {
            if (File.Exists(memoryFile))
            {
                try
                {
                    favouriteTopic = File.ReadAllText(memoryFile).Trim();
                }
                catch { }
            }
        }


        private void clear_chat(object sender, RoutedEventArgs e)
        {
            chats_box.Clear();
            chats_box.AppendText(" [Chats cleared] ");

        }

        private void memory_recap(object sender, RoutedEventArgs e)
        {

            chats_box.AppendText("**MEMORY RECAP**\n");
            chats_box.AppendText($"User: {name}\n");
            if (!string.IsNullOrEmpty(favouriteTopic))
            {
                chats_box.AppendText($"Favorite topic: {favouriteTopic}\n");
            }
            else
                chats_box.AppendText($"Favorite topic: not set yet (say 'I'm interested in privacy')\n");
            chats_box.AppendText($"Last discussed topic: {currentTopic}\n\n");

        }

        private void voice_toggle(object sender, RoutedEventArgs e)
        {

            voiceEnabled = !voiceEnabled;
            Button btn = sender as Button;
            if (btn != null)
            {
                btn.Content = voiceEnabled ? "Voice ON" : "Voice OFF";
            }
            if (voiceEnabled)
            {
                try
                {
                    speechSynthesizer.SpeakAsync("Voice mode activated");
                }
                catch { }
            }

        }

        private void close_button(object sender, RoutedEventArgs e) => Application.Current.Shutdown();





    }//end of class
}//end of namespace

