using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetSimulation
{
    internal interface IUser
    {
        public string UserName { get; set; }
        public List<User> Following { get; }
        public List<string> Tweets { get; set; }

        public void AddFollower(User userToAdd);
        public void AddTweet(string tweetToAdd);

        public void CreatePost();
    }
}
