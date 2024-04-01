<script>
  import "../app.css";
  import Navbar from "../components/Navbar.svelte";
  import Footer from "../components/Footer.svelte";
  import {
    setAccountData,
    getAccountData,
    clearAccountData,
  } from "../core/accountData";
  import { browser } from "$app/environment";

  async function updateAccountData() {
    let userData = {
      username: "",
      isPro: false,
      proEndingDate: "",
      token: "",
      expiration: "",
    };
    let baseUrl;
    if (browser) {
      userData = getAccountData();
      baseUrl = window.location.origin;
    }

    if (userData?.token != "") {
      const res = await fetch(baseUrl + "/api/Account", {
        method: "GET",
        headers: {
          Authorization: `Bearer ${userData.token}`,
        },
      });
      const accountdata = await res.json();
      if (res.status == 401) {
        clearAccountData();
      } else {
        try {
          if (browser) {
            setAccountData({
              username: accountdata.username,
              isPro: accountdata.isPro,
              proEndingDate: accountdata.proEndingDate,
              token: userData.token,
              expiration: userData.expiration,
            });
          }
        } catch (error) {
          console.log("somthing bad happen, maybe the token date has expired");
        }
      }
    }
  }

  updateAccountData();
</script>

<Navbar />
<slot />
<Footer />
