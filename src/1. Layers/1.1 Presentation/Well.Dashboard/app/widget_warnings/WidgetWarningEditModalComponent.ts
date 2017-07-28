import {Component, ViewChild, EventEmitter, Output} from '@angular/core';
import {Response} from '@angular/http';
import {WidgetWarning} from './widgetWarning';
import {ToasterService} from 'angular2-toaster/angular2-toaster';
import {BranchCheckboxComponent} from '../shared/branch/branchCheckboxComponent';
import {WidgetWarningService} from './widgetWarningService';
import {HttpResponse} from '../shared/httpResponse';

@Component({
    selector: 'ow-widget-warning-edit-modal',
    templateUrl: './app/widget_warnings/widget-warning-edit-modal.html'
})
export class WidgetWarningEditModalComponent {
    public isVisible: boolean = false;
    public widgetWarning: WidgetWarning;
    public httpResponse: HttpResponse = new HttpResponse();
    public errors: string[];
    @Output() public onWidgetWarningUpdate = new EventEmitter<WidgetWarning>();

    constructor(private widgetWarningService: WidgetWarningService, private toasterService: ToasterService) { }

    @ViewChild(BranchCheckboxComponent) public branch: BranchCheckboxComponent;

    public show(widgetWarning: WidgetWarning) {
        this.clear();
        this.widgetWarning = widgetWarning;
        this.isVisible = true;
    }

    public hide() {
        this.isVisible = false;
        this.clear();
    }

    public clear() {
        this.widgetWarning = new WidgetWarning();
        this.errors = [];
    }

    public  update() {
        this.widgetWarningService.saveWidgetWarning(this.widgetWarning, true)
            .subscribe((res: Response) => {
                this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success) {
                    this.toasterService.pop('success', 'Widget Warning has been updated', '');
                    this.isVisible = false;
                    this.onWidgetWarningUpdate.emit(this.widgetWarning);
                }
                if (this.httpResponse.failure) {
                    this.toasterService.pop(
                        'error',
                        'Widget warning could not be updated at this time',
                        'Please try again later!');
                    this.isVisible = false;
                }
                if (this.httpResponse.notAcceptable) {
                    this.errors = this.httpResponse.errors;
                }
            });
    }
}