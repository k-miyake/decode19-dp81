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

export default Vue.extend({
  name: "home",
  data() {
    return {
      baseUrl: "http://localhost:7071/api"
    };
  },

  methods: {
    displayTweet(tweets: Object[]): void {
      console.log(tweets[0]);
    }
  },

  created: async function(): Promise<void> {
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
    connection.on("newMessage", tweets => this.displayTweet(tweets));

    // 切断
    connection.onclose(() => console.log("disconnected"));
  }
});
</script>
