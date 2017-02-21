import { Component, EventEmitter, Output, ViewChild }  from '@angular/core';
import { BaseChartComponent } from 'ng2-charts/ng2-charts';

@Component({
    selector: 'ow-widget-graph',
    templateUrl: './app/home/widgetGraph.html'
})
export class WidgetGraphComponent {
    private red: string = '#a94442';
    private blue: string = '#428bca';

    @ViewChild(BaseChartComponent) public myChart: BaseChartComponent;

    public barChartOptions: any = {
        scaleShowVerticalLines: false,
        responsive: true,
        scales: {yAxes: [{ticks: {beginAtZero: true}}]},
        tooltips: {
            custom: (e) => {
                this.chartStyle = (e.body && e.body.length > 0) ? 'chart-hover' : 'chart';
            }
        }
    };

    public barChartLabels: string[] = [];
    public barChartData: any[] = [{data: []}];
    public barChartColors: Array<any> = [{ backgroundColor: [] }];
    public updateDate: Date;
    public chartStyle: string = 'chart';

    public init(labels: string[], data: any[], warningLevels: boolean[], updateDate: Date): void {
        const colors: string[] = warningLevels.map(showWarning => { return showWarning ? this.red : this.blue });

        this.barChartLabels = labels;
        this.barChartData[0].data = data;
        this.barChartColors[0].backgroundColor = colors;
        this.updateDate = updateDate;
    };
}