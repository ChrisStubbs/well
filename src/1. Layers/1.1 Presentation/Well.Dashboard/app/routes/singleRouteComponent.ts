import { Component, ViewChild } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { RoutesService } from './routesService'
import { SingleRoute } from './singleRoute';
import { ActivatedRoute } from '@angular/router';
import { AppDefaults } from '../shared/defaults/defaults';
import * as _ from 'lodash';
import { DataTable } from 'primeng/primeng';
import { AssignModel, AssignModalResult } from '../shared/assignModel';
import { Branch } from '../shared/branch/branch';
import { SecurityService } from '../shared/security/securityService';
import { GlobalSettingsService } from '../shared/globalSettings';
import { IObservableAlive } from '../shared/IObservableAlive';
import { SingleRouteItem } from './singleRoute';
import { SplitButtonComponent } from '../shared/splitButtonComponent';
import { ActionModal } from '../shared/action/actionModal';
import { LookupService, LookupsEnum, ILookupValue } from '../shared/services/services';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/mergeMap';

@Component({
    selector: 'ow-route',
    templateUrl: './app/routes/singleRouteComponent.html',
    providers: [RoutesService, LookupService, CurrencyPipe],
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
    public isAlive: boolean = true;
    public rowCount = AppDefaults.Paginator.rowCount();
    public jobTypes: Array<ILookupValue>;
    public jobStatus: ILookupValue[];
    public podFilter: boolean;
    public lastRefresh = Date.now();

    @ViewChild('dt') public grid: DataTable;
    @ViewChild(SplitButtonComponent) private splitButtonComponent: SplitButtonComponent;
    @ViewChild(ActionModal) private actionModal: ActionModal;

    private routeId: number;
    private isReadOnlyUser: boolean = false;
    private actions: string[] = ['Credit']; /*['Close', 'Credit', 'Re-plan'];*/
    private allSingleRouteItems: Array<SingleRouteItem>;

    constructor(
        private lookupService: LookupService,
        private routeService: RoutesService,
        private route: ActivatedRoute,
        private securityService: SecurityService,
        private globalSettingsService: GlobalSettingsService,
        private currencyPipe: CurrencyPipe) { }

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

        Observable.forkJoin(
            this.lookupService.get(LookupsEnum.JobType),
            this.lookupService.get(LookupsEnum.JobStatus)
        )
            .takeWhile(() => this.isAlive)
            .subscribe(res =>
            {
                this.jobTypes = res[0];
                this.jobStatus = res[1];
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
        _.map(this.singleRouteItems, current => current.isSelected = false);
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

    public selectStops(select: boolean, stop?: string): void
    {
        let filterToApply = function (item: SingleRouteItem): boolean { return true; };

        if (!_.isNull(stop))
        {
            //if it is not null it means the user click on a group
            filterToApply = function (item: SingleRouteItem): boolean { return item.stop == stop; };
        }

        _.chain(this.singleRouteItems)
            .filter(filterToApply)
            .map(current => current.isSelected = select)
            .value();
    }

    public selectedItems(): Array<SingleRouteItem>
    {
        return _.filter(this.singleRouteItems, current => current.isSelected);
    }

    public allChildrenSelected(stop?: string): boolean
    {
        let filterToApply = function (item: SingleRouteItem): boolean { return true; };

        if (!_.isNull(stop))
        {
            //if it is not null it means the user click on a group
            filterToApply = function (item: SingleRouteItem): boolean { return item.stop == stop; };
        }

        return _.every(
            _.filter(this.singleRouteItems, filterToApply),
            current => current.isSelected);
    }

    public getActionSummaryData(): string
    {
        //TODO: Hook up the userThresholdValue
        const totalCreditValue = this.currencyPipe.transform(_.sumBy(this.selectedItems(), x => x.credit), 'GBP', true);
        const usersThreshold = this.currencyPipe.transform(1000.00, 'GBP', true);
        const summary = `The total to be actioned for the selection is ${totalCreditValue}. 
                        The maximum you are allowed to credit is ${usersThreshold}. 
                        Any items over your threshold will be sent for approval`;
        return summary;
    }
}