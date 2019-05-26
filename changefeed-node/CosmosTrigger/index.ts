import { AzureFunction, Context } from '@azure/functions';

const cosmosDBTrigger: AzureFunction = async function(
  context: Context,
  documents: Tweet[]
): Promise<void> {
  if (!!documents && documents.length > 0) {
    context.log('Document: ', documents[0].tweetText);
  }
  context.bindings.signalRMessages = [
    {
      target: 'newMessage',
      arguments: [documents]
    }
  ];
};

export default cosmosDBTrigger;

// JSON ペイロード
class Tweet {
  id: string = '';
  tweetText: string = '';
  lang: string = '';
  name: string = '';
  userId: string = '';
}
