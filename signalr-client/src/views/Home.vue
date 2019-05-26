<template>
  <div class="home">
    <el-container>
      <el-header>
        <span class="title">リアルタイムツイートモニター </span>
        <small>"de:code" "decode" "#decode19" で検索したツイート</small>
      </el-header>
      <el-main>
        <div class="block">
          <el-timeline>
            <el-timeline-item
              v-for="tweet in reversedTweets"
              :key="tweet.id"
              :timestamp="tweet.timeStamp"
              placement="top"
            >
              <el-card>
                <h4>{{ tweet.name }}</h4>
                <p>{{ tweet.tweetText }}</p>
              </el-card>
            </el-timeline-item>
          </el-timeline>
        </div>
      </el-main>
    </el-container>
  </div>
</template>

<style scoped>
.home {
  text-align: left;
}
.title {
  font-size: 1.5rem;
  font-weight: bold;
}
</style>


<script lang="ts">
import Vue from "vue";
import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
  JsonHubProtocol
} from "@aspnet/signalr";
import { constants } from "crypto";
import { DateType } from "element-ui/types/calendar";
import { treeNode } from "element-ui/types/table";

// API ペイロード
class Tweet {
  id: string = "";
  tweetText: string = "";
  lang: string = "";
  name: string = "";
  userId: string = "";
  _ts: number = 0;
  timeStamp: string = "";
}

// ビューモデル
export default Vue.extend({
  name: "home",
  data() {
    return {
      baseUrl: process.env.VUE_APP_HOST as string,
      tweets: [] as Tweet[]
    };
  },

  methods: {
    displayTweet(messages: Tweet[]): void {
      messages.forEach((item: Tweet) => {
        item.timeStamp = new Date(item._ts * 1000).toLocaleTimeString();
        this.tweets.push(item);
        console.log(item);
      });
    }
  },

  computed: {
    reversedTweets(): Tweet[] {
      return this.tweets.reverse();
    }
  },

  created: async function(): Promise<void> {
    console.log("VUE_APP_HOST: ", process.env.VUE_APP_HOST);
    // SignalRとコネクションを作成
    const connection = new HubConnectionBuilder()
      .withUrl(this.baseUrl)
      .configureLogging(LogLevel.Information)
      .build();
    console.log("connecting...");

    // SignalR Serviceへの接続
    connection
      .start()
      .then(() => console.log("connected!"))
      .catch(console.error);

    // SignalRからの呼び出し
    connection.on("newMessage", (tweets: Tweet[]) => this.displayTweet(tweets));

    // 切断
    connection.onclose(() => console.log("disconnected"));
  }
});
</script>
