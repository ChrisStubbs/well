import {Component, ViewChild}   from '@angular/core';
import { ActivatedRoute }       from '@angular/router';
import { IObservableAlive }     from '../shared/IObservableAlive';
import { JobService, JobType }  from '../job/job'
import { StopService }          from './stopService';
import {Stop, StopFilter}       from './stop';
import * as _                   from 'lodash';
import {DataTable}              from 'primeng/components/datatable/datatable';

@Component({
    selector: 'ow-stop',
    templateUrl: './app/stops/stopComponent.html',
    providers: [JobService, StopService],
    styles: [ '.groupRow { display: table-row} ' +
        ' .groupRow div { display: table-cell} ' +
        '.group1{ width: 2%} ' +
        '.group2{ width: 12%} ' +
        '.group3{ width: 48%} ' +
        '.group4{ width: 6%; text-align: right; padding-right: 9px} ' +
        '.group5{ width: 6%; text-align: right; padding-right: 9px} ' +
        '.group6{ width: 6%; text-align: right; padding-right: 9px} ' +
        '.group7{ width: 6%; text-align: right; padding-right: 9px} ' +
        '.group8{ width: 15%}']
})
export class StopComponent implements IObservableAlive
{
    public isAlive: boolean = true;
    public jobTypes: Array<JobType>;
    public tobaccoBags: Array<[string, string]>;
    public stops: Array<Stop>;
    private stopId: number;

    public filters: StopFilter;

    @ViewChild('dt') public grid: DataTable;

    constructor(
        private jobService: JobService,
        private stopService: StopService,
        private route: ActivatedRoute) {}

    public ngOnInit(): void
    {
        this.route.params
            .flatMap(data =>
            {
                this.stopId = data.id;

                return this.stopService.getStop(this.stopId)
            })
            .takeWhile(() => this.isAlive)
            .subscribe(data => {
                this.stops = data;

                this.tobaccoBags = _.chain(this.stops)
                    .map((value: Stop) => [value.barCodeFilter, value.tobacco])
                    .uniqWith((one: [string, string], another: [string, string]) =>
                        one[0] == another[0] && one[1] == another[1])
                    .value();
            });

        this.jobService.JobTypes()
            .takeWhile(() => this.isAlive)
            .subscribe(types =>
            {
                this.jobTypes = types;
            });

        this.filters = new StopFilter();
    }

    public totalPerGroup(perCol: string, job: string): number
    {
        return _.chain(this.stops)
            .filter((current: Stop) => current.job == job)
            .map(perCol)
            .sum()
            .value();
    }

    public ngOnDestroy(): void
    {
        this.isAlive = false;
    }

    public clearFilter()
    {
        this.filters = new StopFilter();
        this.grid.filters = {};
        this.grid.filter(undefined, undefined, undefined);
    }
}