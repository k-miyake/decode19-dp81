<template>
  <div class="home">
    <img
      alt="Vue logo"
      src="../assets/logo.png"
    >
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

export default Vue.extend({
  name: "home",
  data() {
    return {
      baseUrl: process.env.VUE_APP_HOST as string
    };
  },

  methods: {
    displayTweet(tweets: any): void {
      console.log(tweets[0]);
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
