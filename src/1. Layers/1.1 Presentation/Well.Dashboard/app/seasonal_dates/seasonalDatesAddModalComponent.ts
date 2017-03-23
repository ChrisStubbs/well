import {Component, ViewChild, EventEmitter, Output} from '@angular/core';
import {Response} from '@angular/http';
import {SeasonalDate} from './seasonalDate';
import {ToasterService} from 'angular2-toaster/angular2-toaster';
import {BranchCheckboxComponent} from '../shared/branch/branchCheckboxComponent';
import {SeasonalDateService} from './seasonalDateService';
import {HttpResponse} from '../shared/httpResponse';

@Component({
    selector: 'ow-seasonal-add-modal',
    templateUrl: './app/seasonal_dates/seasonal-dates-add-modal.html'
})
export class SeasonalDatesAddModalComponent {
    public isVisible: boolean = false;
    public seasonalDate: SeasonalDate = new SeasonalDate();
    public httpResponse: HttpResponse = new HttpResponse();
    public errors: string[];
    @Output() public onSave = new EventEmitter<SeasonalDate>();

    constructor(private seasonalDateService: SeasonalDateService, private toasterService: ToasterService) { }

    @ViewChild(BranchCheckboxComponent) public branch: BranchCheckboxComponent;

    public show() {
        this.isVisible = true;
        this.errors = [];
    }

    public hide() {
        this.isVisible = false;
        this.clear();
    }

    public clear() {
        this.seasonalDate = new SeasonalDate();
    }

    public save() {
        this.seasonalDate.branches = this.branch.selectedBranches;

        this.seasonalDateService.saveSeasonalDate(this.seasonalDate)
            .subscribe((res: Response) => {
                this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success) {
                    this.toasterService.pop('success', 'Seasonal date has been saved', '');
                    this.isVisible = false;
                    this.clear();
                    this.onSave.emit(this.seasonalDate);
                }
                if (this.httpResponse.failure) {
                    this.toasterService.pop(
                        'error',
                        'Seasonal date could not be saved at this time',
                        'Please try again later!');
                    this.isVisible = false;
                }
                if (this.httpResponse.notAcceptable) {
                    this.errors = this.httpResponse.errors;
                }
            });
    }
}