import { Component, OnInit, ViewChild } from '@angular/core';
import { WidgetWarning } from './widgetWarning';

import { WidgetWarningService } from './widgetWarningService';
import { WidgetWarningAddModalComponent} from './widgetWarningAddModalComponent';
import { WidgetWarningRemoveModalComponent} from './widgetWarningRemoveModalComponent';
import { WidgetWarningEditModalComponent} from './widgetWarningEditModalComponent';

import * as lodash from 'lodash';
 
@Component({
    selector: 'ow-widget-warnings-view',
    templateUrl: './app/widget_warnings/widget-warnings-view.html',
    providers: [ WidgetWarningService ]
})

export class WidgetWarningsViewComponent implements OnInit {
    public widgetWarnings: WidgetWarning[];

    constructor(private widgetWarningService: WidgetWarningService) {}

    @ViewChild(WidgetWarningAddModalComponent) public addModal: WidgetWarningAddModalComponent;
    @ViewChild(WidgetWarningRemoveModalComponent) public removeModal: WidgetWarningRemoveModalComponent;
    @ViewChild(WidgetWarningEditModalComponent) public editModal: WidgetWarningEditModalComponent;

    public ngOnInit(): void {
        this.loadWidgetWarnings();
    }

    public selectWarning(widgetWarning: WidgetWarning): void {
        this.editModal.show(widgetWarning);
    }

    public loadWidgetWarnings(): void {
        this.widgetWarningService.getWidgetWarnings().subscribe(x => this.widgetWarnings = x);
    }

    public add() {
        this.addModal.show();
    }

    public remove(widgetWarning: WidgetWarning): void {
        this.removeModal.show(widgetWarning);
    }

    public onWidgetWarningRemoved(widgetWarning: WidgetWarning) {
        lodash.remove(this.widgetWarnings, widgetWarning);
    }
}