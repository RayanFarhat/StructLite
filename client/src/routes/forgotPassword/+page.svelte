<script lang="ts">
    import { browser } from "$app/environment";

    let SuccessMsgVisable = false;
    let FailMsgVisable = false;
    let FailMsg = "";
    let SuccessMsg = "";

    async function onSend(e: any) {
        let baseUrl;
        if (browser) {
            baseUrl = window.location.origin;
        }
        const formData = new FormData(e.target); // Get the form element
        let json = Object.fromEntries(formData.entries());

        const res = await fetch(baseUrl + "/api/AccountEdit/ForgotPassword", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(json),
        });
        const resJson = await res.json();
        console.log(resJson);

        if (resJson.status == "Error") {
            FailMsg = resJson.message;
            FailMsgVisable = true;
        } else if (resJson.status == "Success") {
            SuccessMsg = resJson.message;
            SuccessMsgVisable = true;
        }
    }
</script>

<div class="hero min-h-screen bg-base-200">
    <div class="hero-content flex-col lg:flex-row-reverse">
        <div class="card shrink-0 w-full max-w-sm shadow-2xl bg-base-100">
            <form class="card-body" on:submit|preventDefault={onSend}>
                <h1 class="text-5xl font-bold">Forgot Password</h1>
                <p class="py-6">
                    Enter your email and we'll send you a link to reset your
                    password.
                </p>
                <div class="form-control">
                    <label for="emailForgotPassword" class="label">
                        <span class="label-text">Email</span>
                    </label>
                    <input
                        type="email"
                        name="email"
                        id="emailForgotPassword"
                        placeholder="email"
                        class="input input-bordered"
                        required
                    />
                </div>
                <div class="form-control mt-6">
                    <button type="submit" class="btn btn-primary">Send</button>
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
