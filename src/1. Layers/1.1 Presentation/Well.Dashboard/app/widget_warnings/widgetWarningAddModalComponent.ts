import { Component, ViewChild, EventEmitter, Output } from '@angular/core'
import { Response} from '@angular/http';
import { ToasterService} from 'angular2-toaster/angular2-toaster';
import { BranchCheckboxComponent} from '../shared/branch/branchCheckboxComponent';
import { HttpResponse} from '../shared/httpResponse';
import { WidgetWarning } from './widgetWarning';
import { WidgetWarningService } from './widgetWarningService';

@Component({
    selector: 'ow-widget-warning-add-modal',
    templateUrl: './app/widget_warnings/widget-warning-add-modal.html'
})
export class WidgetWarningAddModalComponent {
    public isVisible: boolean = false;
    public httpResponse: HttpResponse = new HttpResponse();
    public errors: string[];
    public widgetWarning: WidgetWarning = new WidgetWarning();
    @Output() public onWidgetWarningSave = new EventEmitter<WidgetWarning>();

    constructor(private widgetWarningService: WidgetWarningService, private toasterService: ToasterService) {}

    @ViewChild(BranchCheckboxComponent) public branch: BranchCheckboxComponent;

    public show() {
        this.clear();
        this.isVisible = true;
        this.widgetWarning.type = 'Widget Type';
        //this.widgetWarning.warningLevel = 'Level';
        //this.widgetWarning.widgetName = 'WidgetName';
    }

    public hide() {
        this.isVisible = false;
        this.clear();
    }

    public clear() {
        this.widgetWarning = new WidgetWarning();
        this.errors = [];
    }

    public save() {
        this.widgetWarning.branches = this.branch.selectedBranches;

        this.widgetWarningService.saveWidgetWarning(this.widgetWarning, false)
            .subscribe((res: Response) => {
                this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success) {
                    this.toasterService.pop('success', 'Widget warning level has been saved!', '');
                    this.isVisible = false;
                    this.clear();
                    this.onWidgetWarningSave.emit(this.widgetWarning);
                }
                if (this.httpResponse.failure) {
                    this.toasterService.pop('error',
                        'Widget warning could not be saved at this time!',
                        'Please try again later!');
                    this.isVisible = false;
                }
                if (this.httpResponse.notAcceptable) {
                    this.errors = this.httpResponse.errors;
                }
            });
    }

    public setSelectedType(type) {
        this.widgetWarning.type = type;
    }
}
