import {Component, EventEmitter, Output} from '@angular/core';
import {Response} from '@angular/http';
import {ToasterService} from 'angular2-toaster/angular2-toaster';
import {HttpResponse} from '../shared/httpResponse';
import {WidgetWarningService} from './widgetWarningService';
import {WidgetWarning} from './widgetWarning';

@Component({
    selector: 'ow-widget-warning-remove-modal',
    templateUrl: './app/widget_warnings/widget-warning-remove-modal.html'
})
export class WidgetWarningRemoveModalComponent {
    isVisible: boolean = false;
    widgetWarning: WidgetWarning;
    httpResponse: HttpResponse = new HttpResponse();
    @Output() onWidgetWarningRemoved = new EventEmitter<WidgetWarning>();

    constructor(private widgetWarningService: WidgetWarningService, private toasterService: ToasterService) { }

    show(widgetWarning: WidgetWarning) {
        this.widgetWarning = widgetWarning;
        this.isVisible = true;
    }

    hide() {
        this.isVisible = false;
    }

    yes() {
        this.widgetWarningService.removeWidgetWarning(this.widgetWarning.id)
            .subscribe((res: Response) => {
                this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success) {
                    this.toasterService.pop('success', 'Widget warning has been removed!', '');
                    this.onWidgetWarningRemoved.emit(this.widgetWarning);
                }
                if (this.httpResponse.failure) this.toasterService.pop('error', 'Widget warning could not be deleted at this time!', 'Please try again later!');

                this.isVisible = false;
                
            });
    }
}