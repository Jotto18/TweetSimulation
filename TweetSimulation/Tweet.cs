using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetSimulation
{
    public class Tweet
    {
        private string _tweetText;
        private readonly string _tweetedBy;

        public Tweet(string tweetText, string tweetedBy)
        {
            _tweetText = tweetText;
            _tweetedBy = tweetedBy;
        }

        public string TweetText 
        { 
            get
            {
                return _tweetText;
            }
            set
            {
                _tweetText = value;
            } 
        }

        public string TweetedBy
        {
            get
            {
                return _tweetedBy;
            }
        }
    }
}
