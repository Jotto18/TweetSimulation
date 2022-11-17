using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetSimulation
{
    public class User : IUser
    {
        private string _userName;
        private List<User> _following = new List<User>();
        private List<string> _tweets = new List<string>();

        public User(string userName)
        {
            _userName = userName;
        }

        public string UserName 
        {
            get
            {
                return _userName;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _userName = value;
                }
                else
                {
                    throw new ArgumentException("Name Cannot be Null or Empty");
                }
            }
        }

        public List<User> Following 
        { 
            get
            {
                return _following;
            }
        }

        public List<string> Tweets
        {
            get
            {
                return _tweets;
            }
            set
            {
                _tweets = value;
            }
        }

        public void AddFollower(User userToAdd)
        {
            _following.Add(userToAdd);
        }

        public void AddTweet(string tweetToAdd)
        {
            _tweets.Add(tweetToAdd);
        }

        public void CreatePost()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(_userName);
            foreach (var tweet in _tweets)
            {
                sb.AppendLine("\t@"+_userName+": " + tweet);
            }
            Console.WriteLine(sb);
        }
    }
}
