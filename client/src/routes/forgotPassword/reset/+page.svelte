<script lang="ts">
    import { browser } from "$app/environment";
    import { page } from "$app/stores";
    import { gotoURL } from "../../../core/gotoURL";

    let SuccessMsgVisable = false;
    let FailMsgVisable = false;
    let FailMsg = "";
    let SuccessMsg = "";
    let passErrors: string[] = [];

    const url = $page.url;
    let token = url.search.split("token=")[1];
    let email = url.searchParams.get("email");

    async function onSubmit(e: any) {
        if (
            (<HTMLInputElement>document.getElementById("resetpassword"))
                .value !==
            (<HTMLInputElement>document.getElementById("resetPasswordConfirm"))
                .value
        ) {
            FailMsgVisable = true;
            FailMsg = "two passwords are not the same!";
            return;
        }
        passErrors = [];
        e.preventDefault(); // Prevent the default form submission behavior
        FailMsgVisable = false;
        let baseUrl;
        if (browser) {
            baseUrl = window.location.origin;
        }
        const formData = new FormData(e.target); // Get the form element
        const reqJson = {
            password: Object.fromEntries(formData.entries()).password,
            email: email,
            token: token,
        };

        const res = await fetch(baseUrl + "/api/AccountEdit/ResetPassword", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(reqJson),
        });
        const resJson = await res.json();
        if (resJson.status == "Error") {
            FailMsg = resJson.message;
            FailMsgVisable = true;
        } else if (resJson.status == "Success") {
            SuccessMsg = resJson.message;
            SuccessMsgVisable = true;
            setTimeout(function () {
                gotoURL("/login");
            }, 3000);
        } else if (resJson.status == 400) {
            if (resJson.errors.Password != undefined) {
                passErrors = resJson.errors.Password as string[];
            }
        }
    }
</script>

<div class="hero min-h-screen bg-base-200">
    <div class="hero-content flex-col lg:flex-row-reverse">
        <div class="card shrink-0 w-full max-w-sm shadow-2xl bg-base-100">
            <form class="card-body" on:submit|preventDefault={onSubmit}>
                <h1 class="text-5xl font-bold">Reset Password</h1>
                <p class="py-6">Please submit your new password</p>
                <div class="form-control">
                    <label for="loginpassword" class="label">
                        <span class="label-text">Password</span>
                    </label>
                    <input
                        name="password"
                        id="resetpassword"
                        type="password"
                        placeholder="password"
                        class="input input-bordered"
                    />
                    <div class="form-control">
                        <label for="loginpassword" class="label">
                            <span class="label-text">Confirm Password</span>
                        </label>
                        <input
                            id="resetPasswordConfirm"
                            type="password"
                            placeholder="password"
                            class="input input-bordered"
                        />
                    </div>
                    {#each passErrors as error}
                        <li class="text-error">{error}</li>
                        <br />
                    {/each}
                    <div class="form-control mt-6">
                        <button type="submit" class="btn btn-primary"
                            >Submit</button
                        >
                    </div>
                </div>
            </form>
        </div>
    </div>
</div>
{#if SuccessMsgVisable == true}
    <div class="toast toast-center">
        <div class="alert alert-success">
            <span>{SuccessMsg}</span>
        </div>
    </div>
{/if}
{#if FailMsgVisable == true}
    <div class="toast toast-center">
        <div class="alert alert-error">
            <span>{FailMsg}</span>
        </div>
    </div>
{/if}
