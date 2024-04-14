<script>
    import "../app.css";

    import { onMount } from "svelte";
    import { getData } from "../core/LoadData";

    // render when getData() finish
    let render = null;

    onMount(async () => {
        await getData();
        render = true;
    });
</script>

{#if render}
    <slot />
{/if}
