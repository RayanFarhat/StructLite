<script lang="ts">
    import { browser } from "$app/environment";

    let SuccessMsgVisable = false;
    let FailMsgVisable = false;
    let FailMsg = "";
    let SuccessMsg = "";

    async function onSubmit(e: any) {
        e.preventDefault(); // Prevent the default form submission behavior
        const formData = new FormData(e.target); // Get the form element
        let json = Object.fromEntries(formData.entries());
        let baseUrl;
        if (browser) {
            baseUrl = window.location.origin;
        }
        const endpoint = baseUrl + "/api/Contact";
        console.log(JSON.stringify(json));
        const res = await fetch(endpoint, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(json),
        });
        const resJson = await res.json();
        console.log(resJson);

        if (resJson.status == "Success") {
            SuccessMsgVisable = true;
            SuccessMsg = resJson.message;
        } else if (resJson.status == "Error") {
            FailMsgVisable = true;
            FailMsg = resJson.message;
        }
    }
</script>

<br />
<br />
<br />
<br />
<br />
<div class="flex items-center justify-center flex-col">
    <h1 class="text-6xl">Contact</h1>
    <h3 class="text-xl text-red-50 p-4">
        Get in touch with the form below or send an email
    </h3>
    <form class="w-full" on:submit|preventDefault={onSubmit}>
        <div class="form-control flex items-center">
            <input
                name="Name"
                type="text"
                placeholder="Name"
                class="input input-bordered input-primary m-2 w-1/2"
                required
            />
        </div>
        <div class="form-control flex items-center">
            <input
                name="Email"
                type="text"
                placeholder="Email"
                class="input input-bordered input-primary m-2 w-1/2"
                required
            />
        </div>
        <div class="form-control flex items-center">
            <input
                name="Company"
                type="text"
                placeholder="Company"
                class="input input-bordered input-primary m-2 w-1/2"
                required
            />
        </div>
        <div class="form-control flex items-center">
            <textarea
                name="Message"
                class="textarea textarea-primary m-2 w-1/2"
                placeholder="Message"
            ></textarea>
        </div>
        <div class="form-control mt-6 flex items-center">
            <button class="btn btn-primary" type="submit">Send Message</button>
        </div>
    </form>
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
