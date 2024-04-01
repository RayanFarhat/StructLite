<script lang="ts">
    import { clearAccountData } from "../../core/accountData";
    import { browser } from "$app/environment";
    import { gotoURL } from "../../core/gotoURL";

    export let token: string;

    async function onDelete() {
        if (browser) {
            let baseUrl = window.location.origin;
            const res = await fetch(
                baseUrl + "/api/AccountEdit/DeleteAccount",
                {
                    method: "POST",
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                },
            );
            clearAccountData();
            gotoURL("/login");
        }
    }
</script>

<div class="mx-56 mt-10 border border-primary">
    <div class="collapse bg-base-200">
        <input type="checkbox" />
        <div class="collapse-title text-xl font-medium text-center">
            Delete my account
        </div>
        <div class="collapse-content flex items-center justify-center flex-col">
            <div role="alert">
                <div
                    class="bg-red-500 text-white font-bold rounded-t px-4 py-2"
                >
                    Danger
                </div>
                <div
                    class="border border-t-0 border-red-400 rounded-b bg-red-100 px-4 py-3 text-red-700"
                >
                    <p>
                        Once you Pressed this button below, your account and all
                        your data will be deleted!
                    </p>
                </div>
            </div>
            <button on:click={onDelete} class="btn btn-error text-lg mt-2 w-1/2"
                >Delete my account forever</button
            >
        </div>
    </div>
</div>
