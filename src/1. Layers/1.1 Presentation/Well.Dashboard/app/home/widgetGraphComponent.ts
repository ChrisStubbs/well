import { Component, EventEmitter, Output, ViewChild }  from '@angular/core';
import { BaseChartComponent } from 'ng2-charts/ng2-charts';

@Component({
    selector: 'ow-widget-graph',
    templateUrl: './app/home/widgetGraph.html'
})
export class WidgetGraphComponent {
    private red: string = "#a94442";
    private blue: string = "#428bca";

    @Output() barClicked = new EventEmitter<string>();
    @ViewChild(BaseChartComponent) myChart: BaseChartComponent;

    public barChartOptions: any = {
        scaleShowVerticalLines: false,
        responsive: true
    };
    public barChartLabels: string[] = [];
    public barChartData: any[] = [{data: []}];
    public barChartColors: Array<any> = [{ backgroundColor: [] }];
    public updateDate: Date;

    init(labels: string[], data: any[], warningLevels: boolean[], updateDate: Date) :void {
        this.barChartLabels = labels;
        this.barChartData[0].data = data;
        var colors: string[] = warningLevels.map(showWarning => { return showWarning ? this.red : this.blue });
        this.barChartColors[0].backgroundColor = colors;
        this.updateDate = updateDate;
    };

    public chartClicked(e: any): void {
        if (e.active.length > 0) {
            let index: number = e.active[0]._index;
            let name: string = this.barChartLabels[index];
            console.log(name);
            this.barClicked.emit(name);
        }
    }
}