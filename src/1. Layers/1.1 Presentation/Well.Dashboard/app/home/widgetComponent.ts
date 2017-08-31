import {Component}                      from '@angular/core';
import {GlobalSettingsService}          from '../shared/globalSettings';
import 'rxjs/Rx';   // Load all features
import {RefreshService}                 from '../shared/refreshService';
import { IObservableAlive }             from '../shared/IObservableAlive';

@Component({
    templateUrl: './app/home/widgets.html'
})

export class WidgetComponent implements IObservableAlive
{
    public isAlive: boolean = true;

    constructor(
        public globalSettingsService: GlobalSettingsService,
        private refreshService: RefreshService) {
    }

    public ngOnInit()
    {
        this.refreshService.dataRefreshed$
            .takeWhile(() => this.isAlive)
            .subscribe(r => this.getWidgets());
        this.getWidgets();
    }

    public ngOnDestroy()
    {
        this.isAlive = false;
    }

    public getWidgets() {
        // this.widgetService.getWidgets()
        //     .takeWhile(() => this.isAlive)
        //     .subscribe(widgets =>
        //     {
        //         // const graphWidgets = lodash.filter(widgets, w => w.showOnGraph === true);
        //         // this.widgets = widgets;
        //     });
    }
}