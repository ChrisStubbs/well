import { Component, OnDestroy, OnInit, ViewChild }  from '@angular/core';
import { RoutesService }                            from './routesService'
import { SingleRoute }                              from './singleRoute';
import { ActivatedRoute }                           from '@angular/router';
import { AppDefaults }                              from '../shared/defaults/defaults';
import { JobType, JobService, JobStatus}            from '../job/job';
import * as _                                       from 'lodash';
import { DataTable }                                from 'primeng/primeng';
import 'rxjs/add/operator/mergeMap';

@Component({
    selector: 'ow-route',
    templateUrl: './app/routes/singleRouteComponent.html',
    providers: [RoutesService, JobService]
})
export class SingleRouteComponent implements OnDestroy, OnInit
{
    public singleRoute: Array<SingleRoute>;
    private allSingleRoute: Array<SingleRoute>;
    private alive: boolean = true;
    private routeId: number;
    public rowCount = AppDefaults.Paginator.rowCount();
    public pageLinks = AppDefaults.Paginator.pageLinks();
    public rowsPerPageOptions = AppDefaults.Paginator.rowsPerPageOptions();
    public allStops: Array<string>;
    public jobTypes: Array<JobType>;
    public jobStatus: JobStatus[];
    public podFilter: boolean;

    @ViewChild('dt') public grid: DataTable;
    constructor(
        private routeService: RoutesService,
        private route: ActivatedRoute,
        private jobService: JobService) {}

    public ngOnInit()
    {
        this.route.params
            .flatMap(data =>
            {
                this.routeId = data.id;

                return this.routeService.getSingleRoute(this.routeId)
            })
            .takeWhile(() => this.alive)
            .subscribe(data =>
            {
                this.allSingleRoute = data;
                this.singleRoute = _.filter(this.allSingleRoute, {status: 'Exception'});
            });

        this.jobService.JobTypes()
            .takeWhile(() => this.alive)
            .subscribe(types =>
            {
                const emptyType = new JobType();

                this.jobTypes = types;
                emptyType.description = 'All';
                emptyType.id = undefined;
                this.jobTypes.unshift(emptyType)
            });

        this.jobService.JobStatus()
            .takeWhile(() => this.alive)
            .subscribe(status =>
            {
                const emptyState = new JobStatus();

                this.jobStatus = status;
                emptyState.description = 'All';
                emptyState.id = undefined;
                this.jobStatus.unshift(emptyState)
            });
    }

    public filter(value: string): void
    {
        if (value == 'All')
        {
            this.singleRoute = this.allSingleRoute;
            return;
        }

        this.singleRoute = _.filter(this.allSingleRoute, {status: value});
        this.clearFilter();
    }

    public ngOnDestroy()
    {
        this.alive = false;
    }

    public stops(): Array<string>
    {
        if (_.isNil(this.allStops) && !_.isNil(this.singleRoute))
        {
            this.allStops = _.chain(this.singleRoute)
                .uniqBy((current: SingleRoute) => current.stop)
                .map('stop')
                .value();

            this.allStops.unshift('All');
        }

        return this.allStops;
    }

    public clearFilter()
    {
        this.podFilter = undefined;
        this.grid.filters = {};
        this.grid.filter(undefined, undefined, undefined);
    }
}