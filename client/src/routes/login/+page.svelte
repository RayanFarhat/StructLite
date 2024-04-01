<script lang="ts">
  import { browser } from "$app/environment";
  import { setAccountData } from "../../core/accountData";
  import { gotoURL } from "../../core/gotoURL";

  let emailErrors: string[] = [];
  let passErrors: string[] = [];
  let error401: string[] = [];

  async function onSubmit(e: any) {
    emailErrors = [];
    passErrors = [];
    error401 = [];
    e.preventDefault(); // Prevent the default form submission behavior

    const formData = new FormData(e.target); // Get the form element
    let json = Object.fromEntries(formData.entries());
    let baseUrl;
    if (browser) {
      baseUrl = window.location.origin;
    }
    const endpoint = baseUrl + "/api/Account/login";
    console.log(JSON.stringify(json));

    const res = await fetch(endpoint, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(json),
    });
    let data = await res.json();
    if (data.status == 401) {
      error401 = ["email or password are worng!"];
    } else if (data.status == 400) {
      if (data.errors.Email != undefined) {
        emailErrors = data.errors.Email as string[];
      }
      if (data.errors.Password != undefined) {
        passErrors = data.errors.Password as string[];
      }
    } else if (data.status == 200) {
      console.log(data);

      const res = await fetch(baseUrl + "/api/Account", {
        method: "GET",
        headers: {
          Authorization: `Bearer ${data.token}`,
        },
      });
      const accountdata = await res.json();
      try {
        if (browser) {
          setAccountData({
            username: accountdata.username,
            isPro: accountdata.isPro,
            proEndingDate: accountdata.proEndingDate,
            token: data.token,
            expiration: data.expiration,
          });
          gotoURL("/profile");
        }
      } catch (error) {
        console.log("somthing bad happen, maybe the token date has expired");
      }
    }
  }
</script>

<div class="hero min-h-screen pt-9 bg-base-200">
  <div class="hero-content flex-col lg:flex-row-reverse">
    <div class="text-center lg:text-left">
      <h1 class="text-5xl font-bold">Login now!</h1>
      <p class="py-6">
        To Access our products, You must sign in first. So we can identify you.
      </p>
      <div class="flex justify-center items-center bg-base-200">
        <p>Don't have an account? Sign up here</p>
        <a href="/register">
          <button class="btn btn-outline btn-primary ml-3">Sign Up</button>
        </a>
      </div>
    </div>
    <div class="card flex-shrink-0 w-full max-w-sm shadow-2xl bg-base-100">
      <form class="card-body" on:submit|preventDefault={onSubmit}>
        <div class="form-control">
          <label for="loginemail" class="label">
            <span class="label-text">Email</span>
          </label>
          <input
            name="email"
            id="loginemail"
            placeholder="email"
            class="input input-bordered"
          />
          {#each emailErrors as error}
            <li class="text-error">{error}</li>
            <br />
          {/each}
        </div>
        <div class="form-control">
          <label for="loginpassword" class="label">
            <span class="label-text">Password</span>
          </label>
          <input
            name="password"
            id="loginpassword"
            type="password"
            placeholder="password"
            class="input input-bordered"
          />
          {#each passErrors as error}
            <li class="text-error">{error}</li>
            <br />
          {/each}
          <!-- svelte-ignore a11y-label-has-associated-control -->
          <label class="label">
            <!-- must be /forgotpassword -->
            <a href="/forgotPassword" class="label-text-alt link link-hover"
              >Forgot password?</a
            >
          </label>
          {#each error401 as error}
            <br />
            <li class="text-error">{error}</li>
          {/each}
        </div>
        <div class="form-control mt-6">
          <button class="btn btn-primary" type="submit">Login</button>
        </div>
      </form>
    </div>
  </div>
</div>
