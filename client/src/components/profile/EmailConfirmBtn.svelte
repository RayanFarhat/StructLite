<script lang="ts">
    import { browser } from "$app/environment";
    export let token: string;

    let isConfirmed = "";

    let SuccessMsgVisable = false;
    let FailMsgVisable = false;
    let SuccessMsg = "";
    let FailMsg = "";

    async function onCheck() {
        let baseUrl;
        if (browser) {
            baseUrl = window.location.origin;
        }

        const res = await fetch(baseUrl + "/api/Account/IsEmailConfirmed", {
            method: "GET",
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        const resJson = await res.json();
        if (resJson.status == "Error") {
            FailMsg = resJson.message;
            FailMsgVisable = true;
        } else if (resJson.status == "Success") {
            isConfirmed = resJson.message;
        }
    }

    async function onSendEmail() {
        let baseUrl;
        if (browser) {
            baseUrl = window.location.origin;
        }

        const res = await fetch(
            baseUrl + "/api/AccountEdit/SendEmailConfirmation",
            {
                method: "POST",
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            },
        );
        const resJson = await res.json();
        if (resJson.status == "Error") {
            FailMsg = resJson.message;
            FailMsgVisable = true;
        } else if (resJson.status == "Success") {
            SuccessMsg = resJson.message;
            SuccessMsgVisable = true;
        }
    }
</script>

{#if isConfirmed == ""}
    <button on:click={onCheck} class="btn btn-outline btn-primary text-lg w-1/2"
        >Check if your email is confirmed</button
    >
{/if}

{#if isConfirmed == "true"}
    <div class="card-actions justify-end">
        <div class="badge badge-outline badge-success">Email is Confirmed</div>
    </div>
{/if}

{#if isConfirmed == "false"}
    <button on:click={onSendEmail} class="btn btn-primary w-1/2"
        >your email is not confirmed, press to send confirmation link to your
        email.</button
    >
{/if}

{#if FailMsgVisable == true}
    <div class="toast toast-center">
        <div class="alert alert-error">
            <span>{FailMsg}</span>
        </div>
    </div>
{/if}
{#if SuccessMsgVisable == true}
    <div class="toast toast-center">
        <div class="alert alert-success">
            <span>{SuccessMsg}</span>
        </div>
    </div>
{/if}
{#if isConfirmed !== "true"}
    <p class="py-6 font-bold text-success">
        Confirming your email is essential because without it, if you forget
        your password and need to reset it, confirmation of your email is
        necessary.
    </p>{/if}
