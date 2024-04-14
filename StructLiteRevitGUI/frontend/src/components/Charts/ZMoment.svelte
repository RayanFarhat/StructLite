<script>
    import { chart } from "svelte-apexcharts";
    import { DataStore } from "../../core/LoadData";
    import { get } from "svelte/store";
    let data = get(DataStore).ZMomentData;

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
        colors: ["#F443aa", "#E91E63", "#9C27B0"],
        title: {
            text: "Moment on Z",
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
