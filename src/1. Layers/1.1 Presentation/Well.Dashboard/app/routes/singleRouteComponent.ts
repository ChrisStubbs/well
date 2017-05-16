import { Component, ViewChild } from '@angular/core';
import { RoutesService } from './routesService'
import { SingleRoute } from './singleRoute';
import { ActivatedRoute } from '@angular/router';
import { AppDefaults } from '../shared/defaults/defaults';
import { JobType, JobService, JobStatus } from '../job/job';
import * as _ from 'lodash';
import { DataTable } from 'primeng/primeng';
import { AssignModel, AssignModalResult } from '../shared/assignModel';
import { Branch } from '../shared/branch/branch';
import { SecurityService } from '../shared/security/securityService';
import { GlobalSettingsService } from '../shared/globalSettings';
import { IObservableAlive } from '../shared/IObservableAlive';
import 'rxjs/add/operator/mergeMap';
import { SingleRouteItem } from './singleRoute';
import { SplitButtonComponent } from '../shared/splitButtonComponent';
import { ActionModal } from '../shared/action/actionModal';
import { ActionModalModel } from '../shared/action/actionModalModel';

@Component({
    selector: 'ow-route',
    templateUrl: './app/routes/singleRouteComponent.html',
    providers: [RoutesService, JobService],
    styles: ['.groupRow { display: table-row} ' +
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
    public lastRefresh = Date.now();
    private isReadOnlyUser: boolean = false;
    private selectedItems: SingleRouteItem[] = [];
    private actions: string[] = ['Credit']; /*['Close', 'Credit', 'Re-plan'];*/

    public ids: Array<number> = [1, 2, 3, 4, 5, 6];

    @ViewChild('dt') public grid: DataTable;
    @ViewChild(SplitButtonComponent) private splitButtonComponent: SplitButtonComponent;
    @ViewChild(ActionModal) private actionModal: ActionModal;
    constructor(
        private routeService: RoutesService,
        private route: ActivatedRoute,
        private jobService: JobService,
        private securityService: SecurityService,
        private globalSettingsService: GlobalSettingsService) { }

    public ngOnInit()
    {
        this.route.params
            .flatMap(data =>
            {
                this.routeId = data.id;

                return this.routeService.getSingleRoute(this.routeId);
            })
            .takeWhile(() => this.isAlive)
            .subscribe((data: SingleRoute) =>
            {
                this.singleRoute = data;
                this.allSingleRouteItems = this.singleRoute.items;
                this.singleRouteItems = _.filter(this.allSingleRouteItems);
                this.lastRefresh = Date.now();
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

        this.singleRouteItems = _.filter(this.allSingleRouteItems, { jobStatusDescription: value });
        this.clearFilter();
    }

    public ngOnDestroy(): void
    {
        this.isAlive = false;
    }

    public getAssignModel(route: SingleRouteItem, level: string): AssignModel
    {
        const branch = { id: this.singleRoute.branchId } as Branch;
        let jobs: number[];

        /*this has an error and i can't solve it*/
        /*
        whenever the group is expanded and i click on the group assignee link what it's pass down to the other component
        it's not the group object but the object of the first element in the group
        */
        if (level == 'group')
        {
            jobs = _.chain(this.allSingleRouteItems)
                .filter((value: SingleRouteItem) => value.stop == route.stop)
                .map('jobId')
                .values();
        }
        else
        {
            jobs = [route.jobId] as number[];
        }

        return new AssignModel(
            route.stopAssignee,
            branch,
            jobs,
            this.isReadOnlyUser,
            _.extend(route, { level: level }));
    }

    public onAssigned(event: AssignModalResult): void
    {
        const userName = _.isNil(event.newUser) ? undefined : event.newUser.name;

        if (event.source.level == 'group')
        {
            const stops = _.filter(this.singleRouteItems, (value: SingleRouteItem) => value.stop == event.source.stop);

            _.map(stops, (value: SingleRouteItem) =>
            {
                value.assignee = userName;
                value.stopAssignee = userName;
            });
        }
        else
        {
            const job = _.filter(this.singleRouteItems,
                (value: SingleRouteItem) => value.jobId == event.source.jobId)[0];

            job.assignee = userName;
        }
    }

    public clearFilter(): void
    {
        this.podFilter = undefined;
        this.grid.filters = {};
        this.grid.filter(undefined, undefined, undefined);
        this.selectedItems = [];
        this.splitButtonComponent.reset();
    }

    public totalPerGroup(perCol: string, stop: string): number
    {
        return _.chain(this.singleRouteItems)
            .filter((current: SingleRouteItem) => current.stop == stop)
            .map(perCol)
            .sum()
            .value();
    }

    public onOptionClicked(event: string)
    {
        const model = new ActionModalModel();
        model.action = event;
        model.jobIds = _.uniq(_.map(this.selectedItems, 'jobId'));
        model.items = this.selectedItems;
        this.actionModal.show(model);
    }
}