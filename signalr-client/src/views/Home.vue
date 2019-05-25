<template>
  <div class="home">
    <ul>
      <li
        v-for="tweet in tweets"
        v-bind:key="tweet.id"
      >
        {{ tweet.tweetText }}
      </li>
    </ul>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
  JsonHubProtocol
} from "@aspnet/signalr";
import { constants } from "crypto";

class Tweet {
  id: string = "";
  tweetText: string = "";
  lang: string = "";
  name: string = "";
  userId: string = "";
  _ts: number = 0;
}

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
        this.tweets.push(item);
        console.log(item);
      });
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
    connection.on("newMessage", (tweets: any) => this.displayTweet(tweets));

    // 切断
    connection.onclose(() => console.log("disconnected"));
  }
});
</script>
