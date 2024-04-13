<script>
    import { chart } from "svelte-apexcharts";
    import { getData } from "../../core/LoadData";
    import { onMount } from "svelte";
    let data = [2, 3];
    $: data3 = data.length;
    onMount(async () => {
        data = (await getData()).shearData;
        options.series = [{ data: data }];
    });
    let options = {
        series: [
            {
                data: data,
            },
        ],
        chart: {
            width: 700,
            height: 550,
            type: "area",
            zoom: {
                enabled: true,
            },
        },
        dataLabels: {
            enabled: false,
        },
        stroke: {
            curve: "stepline",
        },
        title: {
            text: "Shear",
            align: "left",
        },
        grid: {
            row: {
                colors: ["#f3f3f3", "transparent"], // takes an array which will be repeated on columns
                opacity: 0.5,
            },
        },
        xaxis: {
            type: "numeric",
            labels: {
                formatter: function (val) {
                    return val.toFixed(2);
                },
            },
            tickAmount: 10,
        },
        yaxis: {
            type: "numeric",
            labels: {
                formatter: function (val) {
                    return val.toFixed(2);
                },
            },
            tickAmount: 10,
        },
        noData: {
            text: "Loading...",
        },
    };
</script>

<div use:chart={options} />
