import { Component, OnInit}  from '@angular/core';
import 'rxjs/Rx';   // Load all features
import {WidgetService} from './widgetService';
import {RefreshService} from '../shared/refreshService';

@Component({
    selector: 'ow-widget-graph',
    templateUrl: './app/home/widgetGraph.html'
})
export class WidgetGraphComponent implements OnInit {

    refreshSubscription: any;

    constructor(private widgetService: WidgetService,
        private refreshService: RefreshService) {
    }

    ngOnInit() {
        this.refreshSubscription = this.refreshService.dataRefreshed$.subscribe(r => this.getWidgets());
        this.getWidgets();
    }

    ngOnDestroy() {
        this.refreshSubscription.unsubscribe();
    }

    getWidgets() {

    }

    public barChartOptions: any = {
        scaleShowVerticalLines: false,
        responsive: true
    };
    public barChartLabels: string[] = ['Exceptions', 'Assigned', 'Outstanding', 'Notifications'];
    public barChartType: string = 'bar';
    public barChartLegend: boolean = false;
    red: string = "#f2dede";
    redLine: string = "#a94442";
    blue: string = "#428bca";


    public barChartColors: Array<any> = [
        { // Exceptions
            backgroundColor: [this.blue, this.blue, this.redLine, this.blue],
        }
    ];

    public barChartData: any[] = [
        {
            data: [5, 3, 10, 0]
        }
    ];
}