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
    widgetWarnings: WidgetWarning[];

    constructor(private widgetWarningService: WidgetWarningService) {}

    @ViewChild(WidgetWarningAddModalComponent) addModal: WidgetWarningAddModalComponent;
    @ViewChild(WidgetWarningRemoveModalComponent) removeModal: WidgetWarningRemoveModalComponent;
    @ViewChild(WidgetWarningEditModalComponent) editModal: WidgetWarningEditModalComponent;

    ngOnInit(): void {
        this.loadWidgetWarnings();
    }

    selectWarning(widgetWarning: WidgetWarning): void {
        this.editModal.show(widgetWarning);
    }

    loadWidgetWarnings(): void {
        this.widgetWarningService.getWidgetWarnings().subscribe(x => this.widgetWarnings = x);
    }

    add() {
        this.addModal.show();
    }

    remove(widgetWarning: WidgetWarning): void {
        this.removeModal.show(widgetWarning);
    }

    onWidgetWarningRemoved(widgetWarning: WidgetWarning) {
        lodash.remove(this.widgetWarnings, widgetWarning);
    }
}