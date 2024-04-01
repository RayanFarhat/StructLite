<script lang="ts">
  import { onMount } from "svelte";
  import { getAccountData } from "../../core/accountData";
  let btnUrl = "/login";
  let btnTitle = "Login";

  onMount(() => {
    if (getAccountData().token != "") {
      btnUrl = "/profile";
      btnTitle = "Profile";
    } else {
      btnUrl = "/login";
      btnTitle = "Login";
    }
  });
</script>

<br />
<div class="hero min-h-screen bg-base-200">
  <div class="hero-content flex-col lg:flex-row-reverse">
    <!-- first col -->
    <div
      class="mockup-window shadow-primary shadow-2xl bg-base-300 w-2/4 py-10"
    >
      <div class="flex justify-center">
        <img
          class="w-1/2 h-1/2"
          src="/editedIcons/house_plan.png"
          alt="house plan icon"
        />
      </div>
    </div>
    <!-- second col -->
    <div>
      <h1 class="text-5xl font-bold">
        Revit Reinforced Concrete Structural Analysis and Rebar in
        <span
          class="text-transparent bg-clip-text bg-gradient-to-r from-primary to-secondary"
        >
          Minutes</span
        >
      </h1>
      <p class="py-6">
        Finish simple reinforced concrete building project in revit with
        minutes.
      </p>
      <div>
        <a class="p-0 hover:bg-transparentt" href="/contact"
          ><button class="btn btn-outline btn-primary">Contact Us</button></a
        >
        <button class="btn btn-primary">Download</button>
      </div>
    </div>
  </div>
</div>
