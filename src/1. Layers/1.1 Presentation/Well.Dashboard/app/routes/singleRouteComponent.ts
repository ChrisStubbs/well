import { Component, OnDestroy, OnInit }     from '@angular/core';
import { RoutesService }                    from './routesService'
import { SingleRoute }                      from './singleRoute';
import { ActivatedRoute }                   from '@angular/router';
import { AppDefaults }                      from '../shared/defaults/defaults';
import { JobType, JobService, JobStatus}    from '../job/Job';
import * as _                               from 'lodash';
import 'rxjs/add/operator/mergeMap';

@Component({
    selector: 'ow-route',
    templateUrl: './app/routes/SingleRouteComponent.html',
    providers: [RoutesService, JobService]
})
export class SingleRouteComponent implements OnDestroy, OnInit
{
    public singleRoute: Array<SingleRoute>;
    private alive: boolean = true;
    private routeId: number;
    public rowCount = AppDefaults.Paginator.rowCount();
    public pageLinks= AppDefaults.Paginator.pageLinks();
    public rowsPerPageOptions = AppDefaults.Paginator.rowsPerPageOptions();
    public allStops: Array<string>;
    public jobTypes: Array<JobType>;
    public jobStatus: JobStatus[];

    constructor(
        private routeService: RoutesService,
        private route: ActivatedRoute,
        private jobService: JobService) {}

    public ngOnInit()
    {
        this.route.params
            .flatMap(data =>
            {
                this.routeId = data['routeId'];

                return this.routeService.getSingleRoute(this.routeId)
            })
            .takeWhile(() => this.alive)
            .subscribe(data => this.singleRoute = data);

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

    public ngOnDestroy()
    {
        this.alive = false;
    }

    public stops(): Array<string>
    {
        if (_.isNil(this.allStops))
        {
            this.allStops = _.uniqBy(this.singleRoute, (current: SingleRoute) => current.stop);
            this.allStops.unshift('');
        }

        return this.allStops;
    }
}