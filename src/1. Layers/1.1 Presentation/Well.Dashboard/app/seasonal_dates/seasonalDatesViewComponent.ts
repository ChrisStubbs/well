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
export class SeasonalDatesViewComponent implements OnInit {
    public seasonalDates: SeasonalDate[];

    constructor(private seasonalDateService: SeasonalDateService) { }

    public ngOnInit(): void {
        this.loadSeasonalDates();
    }

    @ViewChild(SeasonalDatesEditModalComponent) public editModal: SeasonalDatesEditModalComponent;
    @ViewChild(SeasonalDatesAddModalComponent) public addModal: SeasonalDatesAddModalComponent;
    @ViewChild(SeasonalDatesRemoveModalComponent) public removeModal: SeasonalDatesRemoveModalComponent;

    public selectSeason(season: SeasonalDate): void {
        this.editModal.show(season);
    }

    public loadSeasonalDates(): void {
        this.seasonalDateService.getSeasonalDates().subscribe(x => this.seasonalDates = x);
    }

    public add() {
        this.addModal.show();
    }

    public remove(season: SeasonalDate): void {
        this.removeModal.show(season);
    }

    public onRemoved(seasonalDate: SeasonalDate) {
        lodash.remove(this.seasonalDates, seasonalDate);
    }
}