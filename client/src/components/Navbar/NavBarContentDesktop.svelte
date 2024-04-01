<script>
  import { page } from "$app/stores";
  import { onMount } from "svelte";
  import { getAccountData } from "../../core/accountData";
  let btnUrl = "/login";
  let btnTitle = "Login";
  let btnPricingTitle = "Pricing";

  onMount(() => {
    if (getAccountData().token != "") {
      btnUrl = "/profile";
      btnTitle = "Profile";
      if (getAccountData().isPro == true) {
        btnPricingTitle = "Pro Status";
      }
    } else {
      btnUrl = "/login";
      btnTitle = "Login";
    }
  });
</script>

<div class="flex-none hidden lg:block">
  <ul class="menu menu-horizontal">
    <!-- Navbar menu content here -->

    <li>
      <a class="p-0 hover:bg-transparent" href="/pricing">
        <button
          class="btn btn-outline border-0 hover:bg-transparent hover:text-primary"
          >{btnPricingTitle}</button
        ></a
      >
    </li>

    <li>
      <a class=" mr-12 p-0 hover:bg-transparent" href="/hamzacad">
        <button
          class="btn btn-outline border-0 hover:bg-transparent hover:text-primary"
        >
          Hamzacad</button
        ></a
      >
    </li>

    <li>
      <a class="p-0 hover:bg-transparentt" href={btnUrl}>
        <button class="btn btn-outline btn-primary">{btnTitle}</button></a
      >
    </li>

    <li>
      <a
        class="p-0 hover:bg-transparentt"
        href={$page.url.pathname.includes("/hamzacad")
          ? "/pricing"
          : "/contact"}
      >
        <button class="btn btn-primary"
          >{$page.url.pathname.includes("/hamzacad")
            ? "Download"
            : "Let's Talk"}</button
        ></a
      >
    </li>
  </ul>
</div>
