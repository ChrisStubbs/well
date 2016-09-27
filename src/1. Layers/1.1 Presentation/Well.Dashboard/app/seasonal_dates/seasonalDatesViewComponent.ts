import {Component, OnInit, ViewChild} from '@angular/core';
import {SeasonalDateService} from './seasonalDateService';
import {SeasonalDatesEditModalComponent} from './seasonalDatesEditModalComponent';
import {SeasonalDatesAddModalComponent} from './seasonalDatesAddModalComponent';
import {SeasonalDatesRemoveModalComponent} from './seasonalDatesRemoveModalComponent';
import {SeasonalDate} from './seasonalDate';
import * as lodash from 'lodash';

@Component({
    selector: 'ow-seasonal-date-view',
    templateUrl: './app/seasonal_dates/seasonal-dates-view.html'
})
export class SeasonalDatesViewComponent implements OnInit{
    seasonalDates: SeasonalDate[];

    constructor(private seasonalDateService: SeasonalDateService) { }

    ngOnInit(): void {
        this.loadSeasonalDates();
    }

    @ViewChild(SeasonalDatesEditModalComponent) editModal: SeasonalDatesEditModalComponent;
    @ViewChild(SeasonalDatesAddModalComponent) addModal: SeasonalDatesAddModalComponent;
    @ViewChild(SeasonalDatesRemoveModalComponent) removeModal: SeasonalDatesRemoveModalComponent;

    selectSeason(season: SeasonalDate): void {
        this.editModal.show(season);
    }

    loadSeasonalDates(): void {
        this.seasonalDateService.getSeasonalDates().subscribe(x => { this.seasonalDates = x;
            console.log(this.seasonalDates);
        });
    }

    add() {
        this.addModal.show();
    }

    remove(season: SeasonalDate): void {
        this.removeModal.show(season);
    }

    onRemoved(seasonalDate: SeasonalDate) {
        lodash.remove(this.seasonalDates, seasonalDate);
    }
}