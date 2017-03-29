import {Component, EventEmitter, Output} from '@angular/core';
import {Response} from '@angular/http';
import {ToasterService} from 'angular2-toaster/angular2-toaster';
import {HttpResponse} from '../shared/httpResponse';
import {SeasonalDateService} from './seasonalDateService';
import {SeasonalDate} from './seasonalDate';

@Component({
    selector: 'ow-seasonal-remove-modal',
    templateUrl: './app/seasonal_dates/seasonal-dates-remove-modal.html'
})
export class SeasonalDatesRemoveModalComponent {
    public isVisible: boolean = false;
    public seasonalDate: SeasonalDate;
    public httpResponse: HttpResponse = new HttpResponse();
    @Output() public onRemoved = new EventEmitter<SeasonalDate>();

    constructor(private seasonalDateService: SeasonalDateService, private toasterService: ToasterService) { }

    public show(seasonalDate: SeasonalDate) {
        this.seasonalDate = seasonalDate;
        this.isVisible = true;
    }

    public hide() {
        this.isVisible = false;
    }

    public yes() {
        this.seasonalDateService.removeSeasonalDate(this.seasonalDate.id)
            .subscribe((res: Response) => {
                this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success) {
                    this.toasterService.pop('success', 'Seasonal date has been removed', '');
                }
                if (this.httpResponse.failure) {
                    this.toasterService.pop(
                        'error',
                        'Seasonal date could not be deleted at this time',
                        'Please try again later!');
                }
                this.isVisible = false;

                this.onRemoved.emit(this.seasonalDate);
            });
    }
}