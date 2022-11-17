using TweetSimulation;
namespace TweetSimulationTests
{
    [TestClass]
    public class ToolsTest
    {
        string folderToTest = "TweetsTest";
        [TestMethod]
        public void CanCreateFolder()
        {
            var success = Tools.CreateFolder(folderToTest);

            Assert.IsTrue(success);
        }

        [TestMethod]
        public void  ValidPathReturned()
        {
            var folderPath = Tools.GetPath(folderToTest);

            Assert.IsNotNull(folderPath);
        }

        [TestMethod]
        public void TestUserFile()
        {
            FileInfo user = new FileInfo(Tools.GetPath(folderToTest + General.USERSFILENAME));
            var userFileValid = Tools.CheckFile(user);

            Assert.IsTrue(userFileValid);
        }

        [TestMethod]
        public void TestTweetFile()
        {
            FileInfo tweet = new FileInfo(Tools.GetPath(folderToTest + General.TWEETFILENAME));
            var userFileValid = Tools.CheckFile(tweet);

            Assert.IsTrue(userFileValid);
        }

        [TestMethod]
        public void userGenerationTest()
        {
            FileInfo user = new FileInfo(Tools.GetPath(folderToTest + General.USERSFILENAME));
            var usersGenerated = Tools.GenerateUsers(user);

            Assert.IsTrue(usersGenerated.Count() > 1);
        }
    }
}
