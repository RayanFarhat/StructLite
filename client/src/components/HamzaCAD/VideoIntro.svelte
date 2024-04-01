<script lang="ts">
  import { browser } from "$app/environment";
  import { getAccountData, type AccountData } from "../../core/accountData";
  let userData: AccountData = {
    username: "",
    isPro: false,
    proEndingDate: "",
    token: "",
    expiration: "",
  };
  if (browser) {
    userData = getAccountData();
  }

  let source = "/hamzacad/hamzacad_text.mp4";
</script>

<div class="mt-20 p-6">
  <h1 class="text-5xl font-bold">
    Slab reinforcement Drawing in
    <span
      class="text-transparent bg-clip-text bg-gradient-to-r from-primary to-secondary"
    >
      Seconds</span
    >
  </h1>
  <p class="py-6">
    turn any Rectilinear polygons in autocad to Slab reinforcement plan for
    bars!
  </p>
  <div>
    <!-- <a
      class="p-0 my-20 hover:bg-transparentt"
      href={userData.isPro ? "/hamzacad/download" : "/pricing"}
    >
      <button class="btn btn-primary"
        >{userData.isPro ? "Download Page" : "Subscribe and Download"}</button
      ></a
    > -->
    <!-- * only for beta -->
    <a class="p-0 my-20 hover:bg-transparentt" href="/hamzacad/download">
      <button class="btn btn-primary">Download</button></a
    >
    <a
      class="p-0 my-20 hover:bg-transparentt"
      href="/hamzacad/installInstruction"
    >
      <button class="btn btn-outline btn-primary">How To Install</button></a
    >
  </div>
</div>
<div class="p-10 flex items-center flex-col">
  <iframe
    class="w-full"
    width="560"
    height="400"
    src="https://www.youtube.com/embed/ndVDg9OEWJc?si=7uwZ8t_Xv5_omiNJ"
    title="YouTube video player"
    frameborder="0"
    allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share"
    allowfullscreen
  ></iframe>

  <a
    class="p-0 my-20 hover:bg-transparentt"
    href={userData.isPro ? "/hamzacad/download" : "/pricing"}
  >
    <button class="btn btn-primary"
      >{userData.isPro ? "Download Page" : "Subscribe and Download"}</button
    ></a
  >
</div>
