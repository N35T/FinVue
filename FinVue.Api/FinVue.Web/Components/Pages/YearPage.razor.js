
const renderYearPageProfitChart = (dataOptions) => {
    const months = ["Jan", "Feb", "Mär", "Mai", "Apr", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dez"];
    const options = {
        legend: {display: true},
        scales: {
            yAxes: [{
                ticks: {
                    callback: function(value, index, values) {
                        return numberToCurrency(value, true);
                    }
                }
            }]
        },
        tooltips: {
            callbacks: {
                label: function(tooltipItem, data) {
                    return data.datasets[tooltipItem.datasetIndex].label + ': ' + numberToCurrency(tooltipItem.yLabel);
                }
            }
        }
    }
    const data = {
        labels: months,
        datasets: [{
            data: dataOptions.outcome,
            borderColor: "#ac4d4d",
            fill: false,
            label: "Ausgaben"
        },{
            data: dataOptions.income,
            borderColor: "#81a975",
            fill: false,
            label: "Einkommen"
        },{
            data: dataOptions.profit,
            borderColor: "#ede3e390",
            fill: false,
            label: "Profit pro Monat"
        },{
            data: dataOptions.cumProfit,
            borderColor: "#33aad6",
            fill: false,
            label: "Kumulierter Profit"
        }]
    };

    new Chart("profit-chart", {
        type: "line",
        data: data,
        options: options
    });
}

const renderCategorySumsChart = (dataOptions) => {
    const data = {
        labels: dataOptions.sumByCategory.map(e => e.CategoryName),
        datasets: [{
            label: 'Kategorien',
            data: dataOptions.sumByCategory.map(e => e.TotalSum / 100),
            backgroundColor: dataOptions.sumByCategory.map(e => e.CategoryColor),
            hoverOffset: 4
        }]
    };
    const options = {
        tooltips: {
            callbacks: {
                label: function(tooltipItem, data) {
                    return data['labels'][tooltipItem['index']] + ': ' + numberToCurrency(data['datasets'][0]['data'][tooltipItem['index']]);
                }
            }
        }
    };

    new Chart("category-sums-chart", {
        type: "pie",
        data: data,
        options: options
    });
};

function numberToCurrency(num, noComma = false) {
    return Intl.NumberFormat('de-DE',{currency:"EUR", style:"currency", maximumFractionDigits: noComma ? 0 : 2}).format(num);
}

const renderYearPage = () => {
    renderYearPageProfitChart({
        income: plotData.income,
        outcome: plotData.outcome,
        profit: plotData.profit,
        cumProfit: plotData.cumProfit
    });

    renderCategorySumsChart({
        sumByCategory: plotData.sumByCategory,
    });
}

export function onLoad() {
}

export function onUpdate() {
    renderYearPage();
}

export function onDispose() {
}
