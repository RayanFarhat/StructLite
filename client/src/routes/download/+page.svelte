<script lang="ts">
    import { browser } from "$app/environment";

    async function onDownload(filename: string) {
        let baseUrl;
        if (browser) {
            baseUrl = window.location.origin;
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
            <h2 class="card-title text-white">StructLite Beta 0.0.1</h2>
            <div class="stat">
                <div
                    class="stat-value text-primary flex items-center justify-center"
                >
                    Revit 2024
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
            <h2 class="card-title text-white">StructLite Beta 0.0.1</h2>
            <div class="stat">
                <div
                    class="stat-value text-primary flex items-center justify-center"
                >
                    Revit 2023
                </div>
                <button
                    on:click={() => onDownload("HamzaCAD2023Setup.msi")}
                    class="btn btn-primary mt-8">Download</button
                >
            </div>
        </div>
    </div>
</div>
