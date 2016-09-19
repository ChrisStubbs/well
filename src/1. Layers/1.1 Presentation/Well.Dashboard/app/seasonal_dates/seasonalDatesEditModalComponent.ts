import {Component} from '@angular/core';
import {SeasonalDate} from './seasonalDate';

@Component({
    selector: 'ow-seasonal-edit-modal',
    templateUrl: './app/seasonal_dates/seasonal-dates-edit-modal.html'
})
export class SeasonalDatesEditModalComponent {
    isVisible: boolean = false;
    seasonalDate: SeasonalDate;

    show(seasonalDate: SeasonalDate) {
        this.seasonalDate = seasonalDate;
        this.isVisible = true;
    }

    hide() {
        this.isVisible = false;
    }
}