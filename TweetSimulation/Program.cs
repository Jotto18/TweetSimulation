using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace TweetSimulation
{
    internal class Program
    {
        static bool validInput = false;
        static int userInput = 0;

        static FileInfo usersFile;
        static FileInfo tweetFile;

        static void Main(string[] args)
        {                                   
            Tools.CreateFolder(General.SOURCEFOLDER);

            Console.WriteLine("Welcome to the Tweet Simulation Program." + Environment.NewLine +
                "Please ensure the 'user' and 'tweet' txt files are uploaded to the directory:\n" + Environment.NewLine +
                Tools.GetPath(General.SOURCEFOLDER) + Environment.NewLine);

            GetUserInput();

            usersFile = new FileInfo(Tools.GetPath(General.SOURCEFOLDER + General.USERSFILENAME));
            tweetFile = new FileInfo(Tools.GetPath(General.SOURCEFOLDER + General.TWEETFILENAME));

            if (usersFile.Exists && tweetFile.Exists)
            {
                if (!Tools.CheckFile(usersFile))
                {
                    Console.WriteLine("Users file not valid");
                    Exit();
                }
                if (!Tools.CheckFile(tweetFile))
                {
                    Console.WriteLine("Tweet file not valid");
                    Exit();
                }
            }
            else
            {
                Console.WriteLine("ERROR: Text files not found, please add the txt files to the source folder and try again");
                Exit();
            }

            Tools.LoadContent(tweetFile,usersFile);
        }

        public static void GetUserInput()
        {
            while (!validInput)
            {
                Console.Write("\nPlease press 1 to continue or 2 to exit : ");
                validInput = int.TryParse(Console.ReadLine(), out userInput);

                if (validInput && (userInput < 0 || userInput > 2))
                {
                    validInput = !validInput;
                }
            }

            if (userInput == 2)
            {
                Environment.Exit(0);
            }
        }

        public static void Exit()
        {
            Environment.Exit(0);
            Console.ReadLine();
        }
    }
}