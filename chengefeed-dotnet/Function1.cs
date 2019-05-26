using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace chengefeed_dotnet
{
    public class Function1
    {
        public Function1(AppDbContext context)
        {
            _context = context;
        }

        private readonly AppDbContext _context;

        [FunctionName("ChangeFeedFunction")]
        public async Task Run([CosmosDBTrigger(
            databaseName: "decode19db",
            collectionName: "tweets",
            ConnectionStringSetting = "CosmosConnection",
            LeaseCollectionName = "leasesBi",
            CreateLeaseCollectionIfNotExists = true,
            FeedPollDelay = 500)]JArray input, ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                //log.LogInformation("First document Id " + input[0].Id);
            }

            // Tweet へデシリアライズ
            var documents = input.ToObject<Tweet[]>();

            var entities = documents.Select(x => new Tweet
            {
                Id = x.Id,
                TweetText = x.TweetText,
                Lang = x.Lang,
                Name = x.Name,
                UserId = x.UserId,
                CreatedOn = DateTimeOffset.FromUnixTimeSeconds(x._ts).DateTime
            });

            // SQL DB へ書き込み
            using (var context = _context)
            {
                await context.Tweets.AddRangeAsync(entities);
                await context.SaveChangesAsync();
            }
        }
    }
}
