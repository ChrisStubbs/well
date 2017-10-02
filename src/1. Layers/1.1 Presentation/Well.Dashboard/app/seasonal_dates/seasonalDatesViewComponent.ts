import { IFilter } from './../shared/gridHelpers/IFilter';
import { VOID_VALUE } from '@angular/animations/browser/src/render/transition_animation_engine';
import { IObservableAlive } from './../shared/IObservableAlive';
import {Component, OnInit, ViewChild} from '@angular/core';
import {SeasonalDateService} from './seasonalDateService';
import {SeasonalDatesEditModalComponent} from './seasonalDatesEditModalComponent';
import {SeasonalDatesAddModalComponent} from './seasonalDatesAddModalComponent';
import {SeasonalDatesRemoveModalComponent} from './seasonalDatesRemoveModalComponent';
import {SeasonalDate} from './seasonalDate';
import * as _ from 'lodash';

@Component({
    selector: 'ow-seasonal-date-view', 
    templateUrl: './app/seasonal_dates/seasonal-dates-view.html'
})
export class SeasonalDatesViewComponent implements IObservableAlive {
    public seasonalDates: SeasonalDate[];
    public isAlive: boolean = true;

    @ViewChild(SeasonalDatesEditModalComponent) public editModal: SeasonalDatesEditModalComponent;
    @ViewChild(SeasonalDatesAddModalComponent) public addModal: SeasonalDatesAddModalComponent;
    @ViewChild(SeasonalDatesRemoveModalComponent) public removeModal: SeasonalDatesRemoveModalComponent;

    private years: Array<number>;

    public constructor(private seasonalDateService: SeasonalDateService) { }

    public ngOnInit(): void 
    {
        this.loadSeasonalDates();
    }

    public ngOnDestroy(): void
    {
        this.isAlive = false;
    }

    public selectSeason(season: SeasonalDate): void {
        this.editModal.show(season);
    }

    public loadSeasonalDates(): void {
        this.seasonalDateService.getSeasonalDates()
        .takeWhile(() => this.isAlive)
        .subscribe(x => 
            {
                this.seasonalDates = x;
                this.years = this.fillYears();
            });
    }

    private fillYears(): Array<number>
    {
        const result = [];

        this.seasonalDates.forEach((item: SeasonalDate) => 
        {
            result.push(new Date(item.fromDate).getFullYear());
        });

        return _.uniq(result);
    }

    public seasonalDatesForYear(year: number)
    {
        return _.sortBy(
                this.seasonalDates.filter((item: SeasonalDate) => new Date(item.fromDate).getFullYear() == year),
                (item: SeasonalDate) => item.fromDate);
    }

    public add() {
        this.addModal.show();
    }

    public remove(season: SeasonalDate): void {
        this.removeModal.show(season);
    }

    public onRemoved(seasonalDate: SeasonalDate) {
        _.remove(this.seasonalDates, seasonalDate);
    }
}