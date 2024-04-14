<script>
    import ZShear from "../components/Charts/ZShear.svelte";
    import YMoment from "../components/Charts/YMoment.svelte";
    import ZMoment from "../components/Charts/ZMoment.svelte";
    import YShear from "../components/Charts/YShear.svelte";
    import XDeflection from "../components/Charts/XDeflection.svelte";
    import ZDeflection from "../components/Charts/ZDeflection.svelte";
    import YDeflection from "../components/Charts/YDeflection.svelte";
</script>

<div class="flex w-full">
    <div class="grid flex-grow card rounded-box place-items-cente">
        <h1 class="text-center font-bold">Z Axis</h1>
        <ZShear />
        <YMoment />
        <ZDeflection />
    </div>
    <div class="divider divider-primary divider-horizontal"></div>

    <div class="grid flex-grow card rounded-box place-items-center">
        <h1 class="text-center font-bold">Y Axis</h1>

        <YShear />
        <ZMoment />
        <YDeflection />
    </div>
</div>
<XDeflection />
