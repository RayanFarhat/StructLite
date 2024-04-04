<script lang="ts">
  import {
    clearAccountData,
    getAccountData,
    type AccountData,
  } from "../../core/accountData";
  import { browser } from "$app/environment";
  import { gotoURL } from "../../core/gotoURL";
  import EmailConfirmBtn from "../../components/profile/EmailConfirmBtn.svelte";
  import DeleteAccount from "../../components/profile/DeleteAccount.svelte";

  let username: string = "username";
  let isPro: boolean = false;

  let accountdata: AccountData = {
    username: username,
    isPro: isPro,
    proEndingDate: "",
    token: "",
    expiration: "",
  };
  if (browser) {
    accountdata = getAccountData();
  }
  //console.log(accountdata);

  if (accountdata.username != "") {
    username = accountdata.username;
    isPro = accountdata.isPro;
  }

  function onSignout() {
    if (browser) {
      clearAccountData();
      gotoURL("/login");
    }
  }

  function onCopy() {
    navigator.clipboard.writeText(accountdata.token);
  }
</script>

<br />
<div class="flex justify-center mt-10">
  <div class="card bg-base-100 shadow-xl">
    <div class="card-body">
      <h2 class="card-title">
        Hey {username}
        {#if isPro == true}
          <div class="badge badge-secondary">PRO</div>
        {/if}
      </h2>
      {#if isPro == true}
        <p>
          You are a professional user and has the freedom to utilize the
          company's products without any restrictions.
        </p>
      {:else}
        <p>
          You are a not professional user and can't use the company's products.
        </p>
      {/if}

      <div class="card-actions justify-end">
        <div class="badge badge-outline">HamzaCAD</div>
      </div>

      <EmailConfirmBtn token={accountdata.token} />

      <button on:click={onCopy} class="btn btn-primary text-lg"
        >Copy authorization token</button
      >

      <button on:click={onSignout} class="btn btn-outline btn-primary mt-4"
        >Sign out</button
      >
    </div>
  </div>
</div>
<DeleteAccount token={accountdata.token} />
<br />
<br />
<br />
<br />
<br />
<br />
<br />
<br />
<br />
<br />
<br />
<br />
<br />
<br />
