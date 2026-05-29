using System;
using System.Collections.Generic;
using System.Windows.Input;

public class KeywordResponder
{
    private Dictionary<string, List<string>> _responses;
    private Dictionary<string, List<string>> _greeting;
    private Random _random = new Random();

    public KeywordResponder()
    {
        _greeting = new Dictionary<string, List<string>>
        {
            { "hello", new List<string> 
            { "Hi there!", "Hello!", "Greetings!" } },

            { "hi", new List<string> 
            { "Hi there!", "Hello!", "Greetings!" } },

            { "what's up", new List<string>
            { "Not much, how about you?", "Just here, what's new?" } },

            { "bye", new List<string> 
            { "Goodbye!", "See you later!", "Take care!" } },


        };
        _responses = new Dictionary<string, List<string>>
        {
            { "what is your name", new List<string>
            { "I'm StratusBot, your cybersecurity awareness chatbot!" } },

            { "who are you", new List<string>
            { "I'm StratusBot, your cybersecurity awareness chatbot!" } },

            {"how are you", new List<string>
            {"I'm doing well, thank you!", "I'm fine, how about you?", "All good here!",
             "I'm just a program, but I'm here to help you learn about cybersecurity! How can I assist you today?" } },

            {"phishing", new List<string>
            {"Phishing is a cyber attack where attackers impersonate legitimate entities to trick individuals into revealing sensitive information, such as passwords or credit card numbers." +
             "It often occurs through email, social media, or other communication channels."}},
            
            {"what is phishing", new List<string>
            {"Phishing is a cyber attack where attackers impersonate legitimate entities to trick individuals into revealing sensitive information, such as passwords or credit card numbers." +
             "It often occurs through email, social media, or other communication channels."}},

            {"socialengineering", new List<string>
            {"Social engineering is a manipulation technique that exploits human psychology to gain unauthorized access to information or systems." +
             "It often involves tricking individuals into divulging confidential information or performing actions that compromise security."}},

            { "what is social engineering", new List<string>
            {"Social engineering is a manipulation technique that exploits human psychology to gain unauthorized access to information or systems." +
             "It often involves tricking individuals into divulging confidential information or performing actions that compromise security."}},

            {"firewall", new List<string>
            {"A firewall is a network security device that monitors and filters incoming and outgoing network traffic based on an organization's previously established security policies." +
             "It acts as a barrier between a trusted internal network and untrusted external networks, such as the internet."}},

            { "what is a firewall", new List<string>
            {"A firewall is a network security device that monitors and filters incoming and outgoing network traffic based on an organization's previously established security policies." +
             "It acts as a barrier between a trusted internal network and untrusted external networks, such as the internet."}},

            {"cybersecurity", new List<string>
            {"Cybersecurity is the practice of protecting computers, servers, mobile devices, electronic systems, networks, and data from malicious attacks." +
             "It involves implementing measures to prevent unauthorized access, data breaches, and other cyber threats."}},
            
            {"what is cybersecurity", new List<string>
            {"Cybersecurity is the practice of protecting computers, servers, mobile devices, electronic systems, networks, and data from malicious attacks." +
             "It involves implementing measures to prevent unauthorized access, data breaches, and other cyber threats."}},

            {"malware", new List<string>
            {"Malware, short for malicious software, is any software intentionally designed to cause damage to a computer, server, client, or computer network." +
             "It can take various forms, including viruses, worms, trojans, ransomware, spyware, adware, and more."}},
                
            {"what is malware", new List<string>
            {"Malware, short for malicious software, is any software intentionally designed to cause damage to a computer, server, client, or computer network." +
             "It can take various forms, including viruses, worms, trojans, ransomware, spyware, adware, and more."}},

            {"ddos", new List<string>
            {"A DDoS (Distributed Denial of Service) attack is a cyber attack where multiple compromised systems are used to flood a target system, such as a website or server, with excessive traffic." +
             "The goal is to overwhelm the target and make it unavailable to legitimate users."}},

            {"what is ddos", new List<string>
            {"A DDoS (Distributed Denial of Service) attack is a cyber attack where multiple compromised systems are used to flood a target system, such as a website or server, with excessive traffic." +
             "The goal is to overwhelm the target and make it unavailable to legitimate users."}},

            {"what is a ddos attack", new List<string>
            {"A DDoS (Distributed Denial of Service) attack is a cyber attack where multiple compromised systems are used to flood a target system, such as a website or server, with excessive traffic." +
             "The goal is to overwhelm the target and make it unavailable to legitimate users."}},

            {"ransomware", new List<string>
            {"Ransomware is a type of malware that encrypts a victim's files or locks them out of their system, demanding a ransom payment to restore access." +
             "It can cause significant damage to individuals and organizations if not properly mitigated."}},

            {"what is ransomware", new List<string>
            {"Ransomware is a type of malware that encrypts a victim's files or locks them out of their system, demanding a ransom payment to restore access." +
             "It can cause significant damage to individuals and organizations if not properly mitigated."}},

            {"botnet", new List<string>
            {"A botnet is a network of compromised computers or devices that are controlled by an attacker, often without the owners' knowledge." +
             "Botnets can be used to launch distributed denial-of-service (DDoS) attacks, send spam emails, or distribute malware."}},

            {"what is a botnet", new List<string>
            {"A botnet is a network of compromised computers or devices that are controlled by an attacker, often without the owners' knowledge." +
             "Botnets can be used to launch distributed denial-of-service (DDoS) attacks, send spam emails, or distribute malware."}},

            {"spoofing", new List<string>
            {"Spoofing is a cyber attack where an attacker disguises themselves as a trusted entity to gain unauthorized access to information or systems." +
            "It can involve falsifying email addresses, IP addresses, or other identifiers to deceive the target."}},

            {"what is spoofing", new List<string>
            {"Spoofing is a cyber attack where an attacker disguises themselves as a trusted entity to gain unauthorized access to information or systems." +
             "It can involve falsifying email addresses, IP addresses, or other identifiers to deceive the target."}},

            {"virus", new List<string>
            {"A computer virus is a type of malware that attaches itself to a legitimate program or file and spreads from one computer to another, causing harm to the infected systems." +
             "It can corrupt or delete data, disrupt system performance, and spread to other devices."}},

            {"what is a virus", new List<string>
            {"A computer virus is a type of malware that attaches itself to a legitimate program or file and spreads from one computer to another, causing harm to the infected systems." +
             "It can corrupt or delete data, disrupt system performance, and spread to other devices."}},

            {"Scam", new List<string>
            {"A scam is a fraudulent scheme designed to trick people into providing personal information or money."+
             "Common types include phishing emails, fake online stores, and misleading investment opportunities."}},

            {"what is a scam", new List<string>
            {"A scam is a fraudulent scheme designed to trick people into providing personal information or money."+
             "Common types include phishing emails, fake online stores, and misleading investment opportunities."}},
                
            {"generaltips", new List<string>
            {   "If you think you've been hacked, report it to the IT Security team immediately.",
                "AI is now being used to create 'Deepfake' audio and video. Verify identity via a second channel.",
                "Enable multi-factor authentication (MFA) whenever possible.",
                "Be cautious of suspicious emails and links.",
                "Clean desk policy: never leave passwords on sticky notes.",
                "Regularly back up important data to an external drive or cloud storage.",
                "Be cautious when using public Wi-Fi; consider using a VPN for added security.",
                "Educate yourself about common cyber threats and how to recognize them.",
                "Use strong, unique passwords for each of your accounts and consider using a password manager.",
                "Keep your software and devices up to date with the latest security patches.",
                "Shred physical documents that contain sensitive information."}},


            {"phishingtips", new List<string>
            {   "Never click on links from unknown senders; they often lead to credential-harvesting sites.",
                "Did you expect this attachment? If not, do not open it—even if it's from a 'friend'.",
                "If an email creates a sense of extreme urgency, it's likely a trap. Attackers use this sense of urgency to get you to make rash decisions. Take a breath and verify.",
                "Never share credentials. Official organizations will never ask for your password via email.",
                "Always verify the sender's email address and look for signs of spoofing, such as misspellings or unusual domains.",
                "Be cautious of unsolicited emails or messages asking for personal information.",
                "Always verify the sender's identity and avoid clicking on suspicious links.",
                "Phishing isn't just email. 'Smishing' (SMS phishing) and 'Vishing' (voice phishing) are on the rise.",
                "Report suspicious emails using the 'Report Phishing' button in your mail client.",
                "If an offer in an email seems too good to be true, it almost certainly is."}},

            {"passwordtips", new List<string>
            {   "Use a passphrase or a password manager to create and store strong passwords.",
                "Avoid using easily guessable information like birthdays or common words.",
                "Never reuse the same password across multiple sites.",
                "Biometric login (fingerprint or face ID) is a secure and convenient alternative to typing passwords.",
                "Enable MFA (Multi-Factor Authentication) for an extra layer of security, especially for sensitive accounts.",
                "Change your password immediately if you suspect a breach."}},

            {"device", new List<string>
            {   "Always keep your operating system and apps updated to patch security vulnerabilities.",
                "Lock your computer (Win + L) every time you walk away from your desk.",
                "Only download software from official sources like the Windows Store, Apple App Store or Google Play Store.",
                "Restart your computer regularly; many security updates require a reboot to finish.",
                "Never plug in 'found' USB drives. They are a classic way to spread malware.",
                "Antivirus software is essential, but it can't catch everything. Stay vigilant.",
                "Disable 'Auto-run' for USB drives to prevent malicious code from executing automatically.",
                "Be careful with browser extensions; choose carefully as they can sometimes see everything you type.",
                "Back up your important files to an offline or cloud-based storage system.",
                "Encryption protects your data if your device is stolen. Ensure BitLocker or FileVault is on.",
                "Check your privacy settings on social media to limit what strangers can see."}},

            {"software", new List<string>
            {   "Always keep your operating system and apps updated to patch security vulnerabilities.",
                "Lock your computer (Win + L) every time you walk away from your desk.",
                "Only download software from official sources like the Windows Store, Apple App Store or Google Play Store.",
                "Restart your computer regularly; many security updates require a reboot to finish.",
                "Never plug in 'found' USB drives. They are a classic way to spread malware.",
                "Antivirus software is essential, but it can't catch everything. Stay vigilant.",
                "Disable 'Auto-run' for USB drives to prevent malicious code from executing automatically.",
                "Be careful with browser extensions; choose carefully as they can sometimes see everything you type.",
                "Back up your important files to an offline or cloud-based storage system.",
                "Keep your work and personal devices separate whenever possible.",
                "If you see a 'Your connection is not private' warning, do not proceed to the website.",
                "IoT devices (smart bulbs, cameras) can be entry points. Put them on a separate guest network.",
                "Webcams should be covered when not in use for an extra layer of privacy.",
                "Always log out of sensitive accounts (like banking) when you're finished.",
                "Use an encrypted messaging app for sensitive business discussions."}},

            {"how to keep my devices safe", new List<string>
            {   "Always keep your operating system and apps updated to patch security vulnerabilities.",
                "Lock your computer (Win + L) every time you walk away from your desk.",
                "Only download software from official sources like the Windows Store, Apple App Store or Google Play Store.",
                "Restart your computer regularly; many security updates require a reboot to finish.",
                "Never plug in 'found' USB drives. They are a classic way to spread malware.",
                "Antivirus software is essential, but it can't catch everything. Stay vigilant.",
                "Disable 'Auto-run' for USB drives to prevent malicious code from executing automatically.",
                "Be careful with browser extensions; choose carefully as they can sometimes see everything you type.",
                "Back up your important files to an offline or cloud-based storage system.",
                "Keep your work and personal devices separate whenever possible.",
                "If you see a 'Your connection is not private' warning, do not proceed to the website.",
                "IoT devices (smart bulbs, cameras) can be entry points. Put them on a separate guest network.",
                "Webcams should be covered when not in use for an extra layer of privacy.",
                "Always log out of sensitive accounts (like banking) when you're finished.",
                "Use an encrypted messaging app for sensitive business discussions."}},

            {"how to keep my device safe", new List<string>
            {   "Always keep your operating system and apps updated to patch security vulnerabilities.",
                "Lock your computer (Win + L) every time you walk away from your desk.",
                "Only download software from official sources like the Windows Store, Apple App Store or Google Play Store.",
                "Restart your computer regularly; many security updates require a reboot to finish.",
                "Never plug in 'found' USB drives. They are a classic way to spread malware.",
                "Antivirus software is essential, but it can't catch everything. Stay vigilant.",
                "Disable 'Auto-run' for USB drives to prevent malicious code from executing automatically.",
                "Be careful with browser extensions; choose carefully as they can sometimes see everything you type.",
                "Back up your important files to an offline or cloud-based storage system.",
                "Keep your work and personal devices separate whenever possible.",
                "If you see a 'Your connection is not private' warning, do not proceed to the website.",
                "IoT devices (smart bulbs, cameras) can be entry points. Put them on a separate guest network.",
                "Webcams should be covered when not in use for an extra layer of privacy.",
                "Always log out of sensitive accounts (like banking) when you're finished.",
                "Use an encrypted messaging app for sensitive business discussions."}},

            {"how to protect my devices", new List<string>
            {   "Always keep your operating system and apps updated to patch security vulnerabilities.",
                "Lock your computer (Win + L) every time you walk away from your desk.",
                "Only download software from official sources like the Windows Store, Apple App Store or Google Play Store.",
                "Restart your computer regularly; many security updates require a reboot to finish.",
                "Never plug in 'found' USB drives. They are a classic way to spread malware.",
                "Antivirus software is essential, but it can't catch everything. Stay vigilant.",
                "Disable 'Auto-run' for USB drives to prevent malicious code from executing automatically.",
                "Be careful with browser extensions; choose carefully as they can sometimes see everything you type.",
                "Back up your important files to an offline or cloud-based storage system.",
                "Keep your work and personal devices separate whenever possible.",
                "If you see a 'Your connection is not private' warning, do not proceed to the website.",
                "IoT devices (smart bulbs, cameras) can be entry points. Put them on a separate guest network.",
                "Webcams should be covered when not in use for an extra layer of privacy.",
                "Always log out of sensitive accounts (like banking) when you're finished.",
                "Use an encrypted messaging app for sensitive business discussions."}},

            {"how to protect my device", new List<string>
            {   "Always keep your operating system and apps updated to patch security vulnerabilities.",
                "Lock your computer (Win + L) every time you walk away from your desk.",
                "Only download software from official sources like the Windows Store, Apple App Store or Google Play Store.",
                "Restart your computer regularly; many security updates require a reboot to finish.",
                "Never plug in 'found' USB drives. They are a classic way to spread malware.",
                "Antivirus software is essential, but it can't catch everything. Stay vigilant.",
                "Disable 'Auto-run' for USB drives to prevent malicious code from executing automatically.",
                "Be careful with browser extensions; choose carefully as they can sometimes see everything you type.",
                "Back up your important files to an offline or cloud-based storage system.",
                "Keep your work and personal devices separate whenever possible.",
                "If you see a 'Your connection is not private' warning, do not proceed to the website.",
                "IoT devices (smart bulbs, cameras) can be entry points. Put them on a separate guest network.",
                "Webcams should be covered when not in use for an extra layer of privacy.",
                "Always log out of sensitive accounts (like banking) when you're finished.",
                "Use an encrypted messaging app for sensitive business discussions."}},

            {"how to stay safe online", new List<string>
            {   "Public Wi-Fi is insecure. Use a VPN if you must connect in coffee shops or airports.",
                "Your home router should have a unique, strong password—not the default 'admin'.",
                "Avoid working on sensitive documents in public places where people can 'shoulder surf'.",
                "Keep your work and personal devices separate whenever possible.",
                "If you see a 'Your connection is not private' warning, do not proceed to the website.",
                "IoT devices (smart bulbs, cameras) can be entry points. Put them on a separate guest network.",
                "Webcams should be covered when not in use for an extra layer of privacy.",
                "Always log out of sensitive accounts (like banking) when you're finished.",
                "Use an encrypted messaging app for sensitive business discussions."}},

            {"online", new List<string>
            {   "Public Wi-Fi is insecure. Use a VPN if you must connect in coffee shops or airports.",
                "Your home router should have a unique, strong password—not the default 'admin'.",
                "Avoid working on sensitive documents in public places where people can 'shoulder surf'.",
                "Keep your work and personal devices separate whenever possible.",
                "If you see a 'Your connection is not private' warning, do not proceed to the website.",
                "IoT devices (smart bulbs, cameras) can be entry points. Put them on a separate guest network.",
                "Webcams should be covered when not in use for an extra layer of privacy.",
                "Always log out of sensitive accounts (like banking) when you're finished.",
                "Use an encrypted messaging app for sensitive business discussions."}},

            {"browswer", new List<string>
            {   "Public Wi-Fi is insecure. Use a VPN if you must connect in coffee shops or airports.",
                "Your home router should have a unique, strong password—not the default 'admin'.",
                "Avoid working on sensitive documents in public places where people can 'shoulder surf'.",
                "Keep your work and personal devices separate whenever possible.",
                "If you see a 'Your connection is not private' warning, do not proceed to the website.",
                "IoT devices (smart bulbs, cameras) can be entry points. Put them on a separate guest network.",
                "Webcams should be covered when not in use for an extra layer of privacy.",
                "Always log out of sensitive accounts (like banking) when you're finished.",
                "Use an encrypted messaging app for sensitive business discussions."}},

            {"browsing", new List<string>
            {   "Public Wi-Fi is insecure. Use a VPN if you must connect in coffee shops or airports.",
                "Your home router should have a unique, strong password—not the default 'admin'.",
                "Avoid working on sensitive documents in public places where people can 'shoulder surf'.",
                "Keep your work and personal devices separate whenever possible.",
                "If you see a 'Your connection is not private' warning, do not proceed to the website.",
                "IoT devices (smart bulbs, cameras) can be entry points. Put them on a separate guest network.",
                "Webcams should be covered when not in use for an extra layer of privacy.",
                "Always log out of sensitive accounts (like banking) when you're finished.",
                "Use an encrypted messaging app for sensitive business discussions."}},

            {"internet", new List<string>
            {   "Public Wi-Fi is insecure. Use a VPN if you must connect in coffee shops or airports.",
                "Your home router should have a unique, strong password—not the default 'admin'.",
                "Avoid working on sensitive documents in public places where people can 'shoulder surf'.",
                "Keep your work and personal devices separate whenever possible.",
                "If you see a 'Your connection is not private' warning, do not proceed to the website.",
                "IoT devices (smart bulbs, cameras) can be entry points. Put them on a separate guest network.",
                "Webcams should be covered when not in use for an extra layer of privacy.",
                "Always log out of sensitive accounts (like banking) when you're finished.",
                "Use an encrypted messaging app for sensitive business discussions."}},
        };
    }

    public string GetResponse(string input)
    {
        foreach (var kvp in _responses)
        {
            if (input.Contains(kvp.Key))
            {
                return kvp.Value[_random.Next(kvp.Value.Count)];
            }
        }
        foreach (var kvp in _greeting)
        {
            if (input.Contains(kvp.Key))
            {
                return kvp.Value[_random.Next(kvp.Value.Count)];
            }
        }
        return "I'm sorry, I don't understand.";
    }
    //Getallkeywords: return a string listing all the keywords that the chatbot can respond to what can I ask
    public string GetAllKeywords()
    {
        return string.Join(", ", _responses.Keys);
    }
}
