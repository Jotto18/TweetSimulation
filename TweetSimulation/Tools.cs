using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TweetSimulation
{
    public static class Tools
    {
        public static bool CreateFolder(string folderToCreate)
        {
            //Receives a folder name and creates folder on desktop to place files in
            string path = System.IO.Path.Combine(
               Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
               folderToCreate
            );

            if (!System.IO.Directory.Exists(path))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(path);
                    return true;
                }
                catch (IOException ie)
                {
                    Console.WriteLine("IO Error: " + ie.Message);
                    return false;
                }
                catch (Exception e)
                {
                    Console.WriteLine("General Error: " + e.Message);
                    return false;
                }
            }
            return false;
        }

        public static string GetPath(string folderName)
        {
            return System.IO.Path.Combine(
               Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
               folderName);
        }

        public static bool CheckFile(FileInfo fileToCheck)
        {
            var fileValid = true;

            switch (fileToCheck.Name.ToLower())
            {
                case "user.txt":
                    using (StreamReader fileContents = fileToCheck.OpenText())
                    {
                        var currentLine = string.Empty;

                        while ((currentLine = fileContents.ReadLine()) != null)
                        {
                            var splittedLine = currentLine.Split(' ');

                            if (String.IsNullOrEmpty(splittedLine[0]) || splittedLine[1].Trim().ToLower() != "follows" || !FollowersValid(splittedLine))
                            {
                                fileValid = false;
                            }
                        }
                    }
                    break;

                case "tweet.txt":
                    using (StreamReader fileContents = fileToCheck.OpenText())
                    {
                        var currentLine = string.Empty;

                        while ((currentLine = fileContents.ReadLine()) != null)
                        {
                            var userName = currentLine.Substring(0, currentLine.IndexOf('>'));
                            var tweetValue = currentLine.Substring(currentLine.LastIndexOf('>') + 1).TrimStart();

                            if (String.IsNullOrEmpty(userName) || tweetValue.Length > 140)
                            {
                                fileValid = false;
                            }
                        }
                    }
                    break;

                default:
                    fileValid = false;
                    break;
            }

            return fileValid;
        }

        public static bool FollowersValid(string[] LineToCheck)
        {
            var followersValid = true;

            for (int i = 2; i < LineToCheck.Count(); i++)
            {
                if (LineToCheck[i].Length < 2 || string.IsNullOrWhiteSpace(LineToCheck[i]))
                {
                    followersValid = false;
                }
            }

            return followersValid;
        }

        public static List<User> GenerateUsers(FileInfo usersFile)
        {
            List<User> usersToReturn = new List<User>();
            List<string> usersAdded = new List<string>();

            using (StreamReader fileContents = usersFile.OpenText())
            {
                var currentLine = string.Empty;

                while ((currentLine = fileContents.ReadLine()) != null)
                {
                    var splittedLine = currentLine.Split(' ');

                    var userToAdd = new User(splittedLine[0]);

                    if (!usersAdded.Contains(splittedLine[0]))
                    {
                        usersAdded.Add(splittedLine[0]);    
                        usersToReturn.Add((User)userToAdd);
                    }
                }
            }

            return usersToReturn;
        }
        public static List<User> GenerateUsers(FileInfo usersFile, Dictionary<string, List<string>> tweetDictionary)
        {
            List<User> usersToReturn = new List<User>();
            List<string> usersAdded = new List<string>();

            using (StreamReader fileContents = usersFile.OpenText())
            {
                var currentLine = string.Empty;

                while ((currentLine = fileContents.ReadLine()) != null)
                {
                    var splittedLine = currentLine.Split(' ');

                    var userToAdd = new User(splittedLine[0]);

                    for (int i = 2; i < splittedLine.Count(); i++)
                    {
                        userToAdd.AddFollower(new User(splittedLine[i].TrimEnd(',')));

                        usersToReturn.Add(new User(splittedLine[i].TrimEnd(',')));
                        usersAdded.Add(splittedLine[i]);
                    }

                    foreach (KeyValuePair<string, List<string>> entry in tweetDictionary)
                    {
                        if (entry.Key.ToString() == userToAdd.UserName)
                        {
                            userToAdd.Tweets = entry.Value;
                        }
                    }

                    if (!usersAdded.Contains(splittedLine[0]))
                    {
                        usersAdded.Add(splittedLine[0]);
                        usersToReturn.Add((User)userToAdd);
                    }
                    else
                    {
                        var userIndexToRemove = usersAdded.IndexOf(splittedLine[0]);

                        if (userToAdd.Following.Count > usersToReturn[userIndexToRemove].Following.Count)
                        {
                            usersAdded.RemoveAt(userIndexToRemove);
                            usersAdded.Add(splittedLine[0]);

                            usersToReturn.RemoveAt(userIndexToRemove);
                            usersToReturn.Add((User)userToAdd);
                        }
                    }
                }
            }

            return usersToReturn;
        }

        public static Dictionary<string,List<string>> GenerateTweets(FileInfo tweetsFile)
        {
            Dictionary<string, List<string>> tweetsToReturn = new Dictionary<string, List<string>>();

            using (StreamReader fileContents = tweetsFile.OpenText())
            {
                var currentLine = string.Empty;

                while ((currentLine = fileContents.ReadLine()) != null)
                {
                    var userName = currentLine.Substring(0, currentLine.IndexOf('>'));
                    var tweetValue = currentLine.Substring(currentLine.LastIndexOf('>') + 1).TrimStart();

                    if (!tweetsToReturn.ContainsKey(userName))
                    {
                        tweetsToReturn.Add(userName, new List<string>());
                        tweetsToReturn[userName].Add(tweetValue);
                    }
                    else
                    {
                        tweetsToReturn[userName].Add(tweetValue);
                    }
                }
            }

            return tweetsToReturn;
        }

        public static void LoadContent(FileInfo tweetFile,FileInfo usersFile)
        {
            Dictionary<string, List<string>> tweetDictionary = Tools.GenerateTweets(tweetFile);
            List<User> userList = Tools.GenerateUsers(usersFile, tweetDictionary);

            DisplayContent(userList);
        }

        private static void DisplayContent(List<User> userList)
        {
            List<User> SortedList = userList.OrderBy(o => o.UserName).ToList();
            var userContentWritten = new List<string>();
            foreach (User user in SortedList)
            {
                if (!userContentWritten.Contains(user.UserName))
                {
                    user.CreatePost();
                    userContentWritten.Add(user.UserName);
                }
            }
        }
    }
}
