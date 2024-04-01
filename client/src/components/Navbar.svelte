<script>
  import NavBarContentDesktop from "./Navbar/NavBarContentDesktop.svelte";
  import NavBarContentMobile from "./Navbar/NavBarContentMobile.svelte";
</script>

<div class="drawer fixed drawer-end z-[1]">
  <input id="my-drawer-3" type="checkbox" class="drawer-toggle" />
  <div class="drawer-content flex flex-col">
    <!-- Navbar -->
    <div class="w-full navbar bg-base-200 shadow-md shadow-primary">
      <!-- Title -->
      <div class="flex-1 px-2 mx-2 font-extrabold">
        <a href="/"> <img class="w-1/3" src="/StructLiteLogo.png" alt="" /></a>
      </div>

      <!-- for desktop -->
      <NavBarContentDesktop />
      <!-- for mobile -->
      <div class="flex-none lg:hidden">
        <label
          for="my-drawer-3"
          aria-label="open sidebar"
          class="btn btn-square btn-ghost"
        >
          <svg
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
            class="inline-block w-6 h-6 stroke-current"
            ><path
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="2"
              d="M4 6h16M4 12h16M4 18h16"
            ></path></svg
          >
        </label>
      </div>
    </div>
  </div>
  <!-- for mobile -->
  <div class="drawer-side">
    <label for="my-drawer-3" aria-label="close sidebar" class="drawer-overlay"
    ></label>
    <NavBarContentMobile />
  </div>
</div>
