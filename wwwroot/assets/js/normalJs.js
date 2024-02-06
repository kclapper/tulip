//set data to local storage
function setData(point, level, badge) {
	localStorage.setItem('level', level);
	localStorage.setItem('point', point);
	localStorage.setItem('badge', badge);
}

showStoredData();

function showStoredData() {
	$("#aLevel").text(localStorage.getItem('level'));
	$("#aBadge").text(localStorage.getItem('badge'));
	$("#aPoint").text(localStorage.getItem('point'));
}


var queryString = window.location.search;// Returns the query string from the current URL

var params = new URLSearchParams(queryString);

// Get the value of the first query parameter
var firstValue = params.get(params.keys().next().value);


function showStats(element) {
	$(element).click(function () {
		showStoredData();
	})
}



function returnProcess(process) {
	
	var output = "";
	switch (process) {
	
		case "FI":
			output = "FI - Accounts Payable";
			showStats($("#FI"));
			break;
		case "SD":
			output = "Sales and Distribution";
			showStats($("#SD"));
			break;
		case "PP":
			output = "Production Planning";
			showStats($("#PP"));
			break;
		case "FI_AR":
			output = "FI - Accounts Receivable";
			showStats($("#FI_AR"));
			break;
		case "MM":
			output = "Material Management";
			showStats($("#MM"));
			break;
		
		default:
			break;
			
	}

	return output;
}


function plotGraph(points, steps) {
  var options = {
    series: [{
      name: 'Steps',
      data: points
    }],
    annotations: {
      points: [{
        x: 'Bananas',
        seriesIndex: 0,
        label: {
          borderColor: '#775DD0',
          offsetY: 0,
          style: {
            color: '#fff',
            background: '#775DD0',
          },
          text: 'Bananas are good',
        }
      }]
    },
    chart: {
      height:250,
      type: 'bar',
    },
    plotOptions: {
      bar: {
        borderRadius: 10,
        columnWidth: '50%',
      }
    },
    dataLabels: {
      enabled: false
    },
    stroke: {
      width: 2
    },

    grid: {
      row: {
        colors: ['#fff', '#f2f2f2']
      }
    },
    xaxis: {
      labels: {
        rotate: -45
      },
      categories: steps,
      tickPlacement: 'on'
    },
    yaxis: {
      title: {
        text: 'Points',
      },
    },
    fill: {
      type: 'gradient',
      gradient: {
        shade: 'light',
        type: "horizontal",
        shadeIntensity: 0.25,
        gradientToColors: undefined,
        inverseColors: true,
        opacityFrom: 0.85,
        opacityTo: 0.85,
        stops: [50, 0, 100]
      },
    }
  };


  var chart = new ApexCharts(document.querySelector("#charts"), options);
  chart.render();
}