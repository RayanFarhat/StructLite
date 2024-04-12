<script>
    import { chart } from "svelte-apexcharts";

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

    // Create chart data using the generated data points
    const start = 0;
    const end = 10;
    const step = 0.01;
    const data = generateDataPoints(start, end, step);

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
    };
</script>

<div use:chart={options} />
