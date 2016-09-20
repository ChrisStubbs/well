import {Component, ViewChild} from '@angular/core';
import { Response } from '@angular/http';
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
    isVisible: boolean = false;
    seasonalDate: SeasonalDate = new SeasonalDate();
    httpResponse: HttpResponse = new HttpResponse();

    constructor(private seasonalDateService: SeasonalDateService, private toasterService: ToasterService) { }

    @ViewChild(BranchCheckboxComponent) branch: BranchCheckboxComponent;

    show() {
        this.isVisible = true;
    }

    hide() {
        this.isVisible = false;
    }

    save() {
        this.seasonalDate.branches = this.branch.selectedBranches;

        this.seasonalDateService.saveSeasonalDate(this.seasonalDate)
            .subscribe((res: Response) => {
                this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success) {
                    this.toasterService.pop('success', 'Seasonal date has been saved!', '');
                    this.isVisible = false;
                }
                if (this.httpResponse.failure) {
                    this.toasterService.pop('error', 'Seasonal date could not be saved at this time!', 'Please try again later!');
                    this.isVisible = false;
                }
                if (this.httpResponse.notAcceptable) {
                    this.toasterService.pop('warning', this.httpResponse.message, '');
                }
            });
    }
}