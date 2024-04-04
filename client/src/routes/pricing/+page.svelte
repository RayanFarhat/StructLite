<script lang="ts">
    import { browser } from "$app/environment";
    import { getAccountData, type AccountData } from "../../core/accountData";
    import { gotoURL } from "../../core/gotoURL";

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
    async function onSubmitMonth() {
        if (userData.token != "") {
            let baseUrl;
            if (browser) {
                baseUrl = window.location.origin;
            }
            const endpoint = baseUrl + "/api/Payment/Month";

            const res = await fetch(endpoint, {
                method: "GET",
                headers: {
                    Authorization: `Bearer ${userData.token}`,
                },
            });

            let resJson = await res.json();

            if (resJson.status == 404) {
                console.log("not found");
            } else if (resJson.status == "Success") {
                if (browser) {
                    window.open(resJson.message, "_blank")?.focus();
                    console.log("aaaaaaaaaaa");
                }
            }
        } else {
            gotoURL("/login");
        }
    }
    async function onSubmitYear() {
        if (userData.token != "") {
            let baseUrl;
            if (browser) {
                baseUrl = window.location.origin;
            }
            const endpoint = baseUrl + "/api/Payment/Year";
            const res = await fetch(endpoint, {
                method: "GET",
                headers: {
                    Authorization: `Bearer ${userData.token}`,
                },
            });
            let resJson = await res.json();

            if (resJson.status == 404) {
                console.log("not found");
            } else if (resJson.status == "Success") {
                if (browser) {
                    window.open(resJson.message, "_blank")?.focus();
                }
            }
        } else {
            gotoURL("/login");
        }
    }
</script>

<br />
<br />
<br />
<br />
<div class="hero min-h-screen bg-base-100">
    <div class="hero-content flex-col lg:flex-row">
        <div class="text-center text-3xl border border-primary p-1">
            There's no need to subscribe at the moment as the product is still
            in beta, and you can use it for free.
        </div>

        {#if userData.isPro == true}
            <div class=" pt-16">
                <h1 class="text-4xl">Subscription end in:</h1>
                <h1 class="text-4xl font-bold">{userData.proEndingDate}</h1>
            </div>
        {/if}

        <!-- first col -->
        <div class="card shadow-xl bg-base-200 m-4">
            <div class="card-body items-center text-center">
                <h2 class="card-title text-white">
                    Pro Subscription per Month
                </h2>
                <div class="stat">
                    <div
                        class="stat-value text-primary flex items-center justify-center"
                    >
                        $9.99 <span
                            class="text-white text-base font-normal text-justify"
                            >&nbsp;&nbsp;USD</span
                        >
                    </div>
                </div>
                <svg
                    class="w-6 h-6 text-primary stroke-primary stroke-2"
                    xmlns="http://www.w3.org/2000/svg"
                    viewBox="0 0 24 24"
                    fill="currentColor"
                >
                    <path
                        fill-rule="evenodd"
                        d="M19.916 4.626a.75.75 0 01.208 1.04l-9 13.5a.75.75 0 01-1.154.114l-6-6a.75.75 0 011.06-1.06l5.353 5.353 8.493-12.739a.75.75 0 011.04-.208z"
                        clip-rule="evenodd"
                    />
                </svg>
                <button on:click={onSubmitMonth} class="btn btn-primary"
                    >{userData.isPro
                        ? "Extend by a month"
                        : "Subscribe Now"}</button
                >
            </div>
        </div>
        <!-- second col -->
        <div class="card shadow-xl bg-base-200 m-4 border border-primary">
            <div class="card-body items-center text-center">
                <h2 class="card-title text-white">Pro Subscription per Year</h2>
                <div class="stat">
                    <div
                        class="stat-value text-primary flex items-center justify-center"
                    >
                        $99.99 <span
                            class="text-white text-base font-normal text-justify"
                            >&nbsp;&nbsp;USD</span
                        >
                    </div>
                    <div class="stat-desc">save around 17%</div>
                    <button on:click={onSubmitYear} class="btn btn-primary mt-8"
                        >{userData.isPro
                            ? "Extend by a year"
                            : "Subscribe Now"}</button
                    >
                </div>
            </div>
        </div>
    </div>
</div>
