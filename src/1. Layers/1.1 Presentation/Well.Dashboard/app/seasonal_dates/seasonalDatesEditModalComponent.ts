import {Component, ViewChild, EventEmitter, Output} from '@angular/core';
import {Response} from '@angular/http';
import {SeasonalDate} from './seasonalDate';
import {ToasterService} from 'angular2-toaster/angular2-toaster';
import {BranchCheckboxComponent} from '../shared/branch/branchCheckboxComponent';
import {SeasonalDateService} from './seasonalDateService';
import {HttpResponse} from '../shared/models/httpResponse';

@Component({
    selector: 'ow-seasonal-edit-modal',
    templateUrl: './app/seasonal_dates/seasonal-dates-edit-modal.html'
})
export class SeasonalDatesEditModalComponent {
    public isVisible: boolean = false;
    public seasonalDate: SeasonalDate;
    public httpResponse: HttpResponse = new HttpResponse();
    public errors: string[];
    @Output() public onUpdate = new EventEmitter<SeasonalDate>();

    constructor(private seasonalDateService: SeasonalDateService, private toasterService: ToasterService) { }

    @ViewChild(BranchCheckboxComponent) public branch: BranchCheckboxComponent;
    
    public show(seasonalDate: SeasonalDate) {
        this.clear();
        this.seasonalDate = seasonalDate;
        this.isVisible = true;
    }

    public hide() {
        this.isVisible = false;
        this.clear();
    }

    public clear() {
        this.seasonalDate = new SeasonalDate();
        this.errors = [];
    }

    public update() {
        this.seasonalDateService.saveSeasonalDate(this.seasonalDate)
            .subscribe((res: Response) => {
                this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success) {
                    this.toasterService.pop('success', 'Seasonal date has been updated', '');
                    this.isVisible = false;
                    this.onUpdate.emit(this.seasonalDate);
                }
                if (this.httpResponse.failure) {
                    this.toasterService.pop(
                        'error',
                        'Seasonal date could not be updated at this time',
                        'Please try again later!');
                    this.isVisible = false;
                }
                if (this.httpResponse.notAcceptable) {
                    this.errors = this.httpResponse.errors;
                }
            });
    }
}