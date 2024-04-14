<script>
    import { chart } from "svelte-apexcharts";
    import { DataStore } from "../../core/LoadData";
    import { get } from "svelte/store";
    let data = get(DataStore).YShearData;

    let options = {
        series: [
            {
                data: data,
            },
        ],
        chart: {
            width: 500,
            height: 400,
            type: "area",
            zoom: {
                enabled: true,
            },
        },
        dataLabels: {
            enabled: false,
        },
        stroke: {
            curve: "straight",
        },
        title: {
            text: "Shear on Y",
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
