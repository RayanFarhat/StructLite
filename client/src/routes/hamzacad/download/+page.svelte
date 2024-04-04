<script lang="ts">
    import { browser } from "$app/environment";
    import {
        getAccountData,
        type AccountData,
    } from "../../../core/accountData";
    import { gotoURL } from "../../../core/gotoURL";

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

    async function onDownload(filename: string) {
        let baseUrl;
        if (browser) {
            baseUrl = window.location.origin;
        }
        const endpoint =
            baseUrl + "/api/HamzaCAD/downloadfile?fileName=" + filename;

        const res = await fetch(endpoint, {
            method: "GET",
            headers: {
                Authorization: `Bearer ${userData.token}`,
            },
        });

        if (res.status == 200) {
            const data = await res.blob();
            const filename = res.headers
                .get("Content-Disposition")!
                .split(";")[1]
                .split("=")[1];
            var url = window.URL.createObjectURL(data),
                anchor = document.createElement("a");
            anchor.href = url;
            anchor.download = filename ? filename : "error_with_name.txt";
            document.body.appendChild(anchor);
            anchor.click();
            // CLEAN UP
            anchor.remove();
            window.URL.revokeObjectURL(url);
        } else {
            gotoURL("/pricing");
        }
    }
</script>

<br />
<br />
<br />
<br />
<div class="grid grid-flow-row lg:grid-cols-3">
    <div class="card shadow-xl bg-base-200 m-4 border border-primary">
        <div class="card-body items-center text-center">
            <h2 class="card-title text-white">HamzaCAD Beta 0.0.1</h2>
            <div class="stat">
                <div
                    class="stat-value text-primary flex items-center justify-center"
                >
                    Autocad 2024
                </div>
                <button
                    on:click={() => onDownload("HamzaCAD2024Setup.msi")}
                    class="btn btn-primary mt-8">Download</button
                >
            </div>
        </div>
    </div>
    <div class="card shadow-xl bg-base-200 m-4 border border-primary">
        <div class="card-body items-center text-center">
            <h2 class="card-title text-white">HamzaCAD Beta 0.0.1</h2>
            <div class="stat">
                <div
                    class="stat-value text-primary flex items-center justify-center"
                >
                    Autocad 2023
                </div>
                <button
                    on:click={() => onDownload("HamzaCAD2023Setup.msi")}
                    class="btn btn-primary mt-8">Download</button
                >
            </div>
        </div>
    </div>
</div>
