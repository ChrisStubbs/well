import { Component, ViewChild }             from '@angular/core';
import { RoutesService }                    from './routesService'
import { SingleRoute }                      from './singleRoute';
import { ActivatedRoute }                   from '@angular/router';
import { AppDefaults }                      from '../shared/defaults/defaults';
import { JobType, JobService, JobStatus}    from '../job/job';
import * as _                               from 'lodash';
import { DataTable }                        from 'primeng/primeng';
import {AssignModel}                        from '../shared/assignModel';
import {Branch}                             from '../shared/branch/branch';
import {SecurityService}                    from '../shared/security/securityService';
import {GlobalSettingsService}              from '../shared/globalSettings';
import {IObservableAlive}                   from '../shared/IObservableAlive';
import 'rxjs/add/operator/mergeMap';
import {SingleRouteItem}                    from './singleRoute';

@Component({
    selector: 'ow-route',
    templateUrl: './app/routes/singleRouteComponent.html',
    providers: [RoutesService, JobService],
    styles: [ '.groupRow { display: table-row} ' +
    '.groupRow div { display: table-cell; padding-right: 9px; padding-left: 9px} ' +
    '#modal a { color: #428bca; text-decoration: none} ' +
    '.group1{ width: 19%} ' +
    '.group2{ width: 27%} ' +
    '.group3{ width: 7%; text-align: right} ' +
    '.group4{ width: 7%; text-align: right} ' +
    '.group5{ width: 7%; text-align: right} ' +
    '.group6{ width: 9%} ' +
    '.group7{ width: 12%} ' +
    '.group8{ width: 10%} ' +
    '.group9{ width: 6%}']
})
export class SingleRouteComponent implements IObservableAlive
{
    public singleRoute: SingleRoute;
    public singleRouteItems: Array<SingleRouteItem>;
    private allSingleRouteItems: Array<SingleRouteItem>;
    public isAlive: boolean = true;
    private routeId: number;
    public rowCount = AppDefaults.Paginator.rowCount();
    public jobTypes: Array<JobType>;
    public jobStatus: JobStatus[];
    public podFilter: boolean;
    private isReadOnlyUser: boolean = false;

    @ViewChild('dt') public grid: DataTable;
    constructor(
        private routeService: RoutesService,
        private route: ActivatedRoute,
        private jobService: JobService,
        private securityService: SecurityService,
        private globalSettingsService: GlobalSettingsService) {}

    public ngOnInit()
    {
        this.route.params
            .flatMap(data =>
            {
                this.routeId = data.id;

                return this.routeService.getSingleRoute(this.routeId);
            })
            .takeWhile(() => this.isAlive)
            .subscribe((data: SingleRoute) => {
                this.singleRoute = data;
                this.allSingleRouteItems = this.singleRoute.items;
                this.singleRouteItems = _.filter(this.allSingleRouteItems, { jobStatusDescription: 'Exception'});
            });

        this.jobService.JobTypes()
            .takeWhile(() => this.isAlive)
            .subscribe(types =>
            {
                const emptyType = new JobType();

                this.jobTypes = types;
                emptyType.description = 'All';
                emptyType.id = undefined;
                this.jobTypes.unshift(emptyType);
            });

        this.jobService.JobStatus()
            .takeWhile(() => this.isAlive)
            .subscribe(status =>
            {
                const emptyState = new JobStatus();

                this.jobStatus = status;
                emptyState.description = 'All';
                emptyState.id = undefined;
                this.jobStatus.unshift(emptyState);
            });

        this.isReadOnlyUser = this.securityService
            .hasPermission(this.globalSettingsService.globalSettings.permissions, this.securityService.readOnly);
    }

    public filter(value: string): void
    {
        if (value == 'All')
        {
            this.singleRouteItems = this.allSingleRouteItems;
            return;
        }

        this.singleRouteItems = _.filter(this.allSingleRouteItems, { jobStatusDescription: value});
        this.clearFilter();
    }

    public ngOnDestroy()
    {
        this.isAlive = false;
    }

    public getAssignModel(route: SingleRouteItem): AssignModel
    {
        const branch = { id: this.singleRoute.branchId } as Branch;
        const jobs =  _.chain(this.allSingleRouteItems)
            .filter((value: SingleRouteItem) => value.stop == route.stop)
            .map('jobId')
            .values();

        return new AssignModel(route.assignee, branch, jobs, this.isReadOnlyUser);
    }

    public onAssigned($event) {
        // this.getRoutes();
    }

    public clearFilter()
    {
        this.podFilter = undefined;
        this.grid.filters = {};
        this.grid.filter(undefined, undefined, undefined);
    }

    public totalPerGroup(perCol: string, stop: string): number
    {
        return _.chain(this.singleRouteItems)
            .filter((current: SingleRouteItem) => current.stop == stop)
            .map(perCol)
            .sum()
            .value();
    }
}