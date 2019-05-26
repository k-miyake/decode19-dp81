using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace chengefeed_dotnet
{
    public class Tweet
    {
        public string Id { get; set; }
        public string TweetText { get; set; }
        public string Lang { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        [NotMapped]
        public int _ts { get; set; }
    }
}
