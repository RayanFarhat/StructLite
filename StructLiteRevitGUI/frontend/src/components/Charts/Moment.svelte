<script>
    import { chart } from "svelte-apexcharts";
    import { getData } from "../../core/LoadData";
    import { onMount } from "svelte";

    onMount(async () => {
        const res = await fetch(`/tutorial/api/album`);
        photos = await res.json();
    });
    function Shear(x) {
        return Math.sin(x);
    }

    function getLengthArr(L) {
        return Math.sin(x) * Math.cos(x);
    }

    function generateDataPoints(start, end, step) {
        const data = [];
        for (let x = start; x <= end; x += step) {
            data.push([x, Shear(x)]);
        }
        return data;
    }

    (async () => {
        console.log(await getData());
    })();

    // Create chart data using the generated data points
    const start = 0;
    const end = 10;
    const step = 0.01;
    let data = generateDataPoints(start, end, step);
    data = [
        {
            x: 0,
            y: 0,
        },
        {
            x: 2,
            y: 2,
        },
        {
            x: 3,
            y: 3,
        },
        {
            x: 4,
            y: 4,
        },
    ];
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
