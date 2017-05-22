import { Component, ViewChild }      from '@angular/core';
import { ActivatedRoute }                           from '@angular/router';
import { IObservableAlive }                         from '../shared/IObservableAlive';
import { StopService }                              from './stopService';
import { Stop, StopItem, StopFilter }               from './stop';
import * as _                                       from 'lodash';
import { DataTable }                                from 'primeng/components/datatable/datatable';
import { AssignModel, AssignModalResult }           from '../shared/assignModel';
import { Branch }                                   from '../shared/branch/branch';
import { SecurityService }                          from '../shared/security/securityService';
import { GlobalSettingsService }                    from '../shared/globalSettings';
import { LookupService, LookupsEnum, ILookupValue}  from '../shared/services/services';

@Component({
    selector: 'ow-stop',
    templateUrl: './app/stops/stopComponent.html',
    providers: [LookupService, StopService],
    styles: ['.groupRow { display: flex } ' +
        '.group1{ width: 2% } ' +
        '.group2{ width: 12% } ' +
        '.group3{ width: 42% } ' +
        '.group4{ width: 6%; text-align: right } ' +
        '.group5{ width: 6%; text-align: right } ' +
        '.group6{ width: 6%; text-align: right } ' +
        '.group7{ width: 6%; text-align: right } ' +
        '.group8{ width: 16% } ' +
        '.group9{ width: 38px; text-align: right; padding-right: 2px }']
})
export class StopComponent implements IObservableAlive
{
    public isAlive: boolean = true;
    public jobTypes: Array<ILookupValue>;
    public tobaccoBags: Array<[string, string]>;
    public stop: Stop = new Stop();
    public stopsItems: Array<StopItem>;
    public filters: StopFilter;
    public lastRefresh = Date.now();

    @ViewChild('dt') public grid: DataTable;
    @ViewChild('btt') public btt;

    private actions: string[] = ['Close', 'Credit', 'Re-plan'];
    private stopId: number;
    private isReadOnlyUser: boolean = false;
    private isActionMode: boolean = false;

    constructor(
        private lookupService: LookupService,
        private stopService: StopService,
        private route: ActivatedRoute,
        private securityService: SecurityService,
        private globalSettingsService: GlobalSettingsService) { }

    public ngOnInit(): void
    {
        this.route.params
            .flatMap(data =>
            {
                this.stopId = data.id;

                return this.stopService.getStop(this.stopId);
            })
            .takeWhile(() => this.isAlive)
            .subscribe((data: Stop) =>
            {
                this.stop = data;
                this.stopsItems = this.stop.items;
                this.lastRefresh = Date.now();
                this.tobaccoBags = _.chain(this.stopsItems)
                    .map((value: StopItem) => [value.barCodeFilter, value.tobacco])
                    .uniqWith((one: [string, string], another: [string, string]) =>
                    one[0] == another[0] && one[1] == another[1])
                    .value();
                this.expandGroups();
            });

        this.lookupService.get(LookupsEnum.JobType)
            .takeWhile(() => this.isAlive)
            .subscribe((value: Array<ILookupValue>) => this.jobTypes = value);

        this.filters = new StopFilter();
        this.isReadOnlyUser = this.securityService
            .hasPermission(this.globalSettingsService.globalSettings.permissions, this.securityService.readOnly);
    }

    private expandGroups(): void
    {
        this.grid.expandedRowsGroups = [];
        _.chain(this.stopsItems)
            .groupBy(this.grid.groupField)
            .map(current => this.grid.expandedRowsGroups.push(current[0].jobId))
            .value();
    }

    public totalPerGroup(perCol: string, jobId: number): number
    {
        return _.chain(this.stopsItems)
            .filter((current: StopItem) => current.jobId == jobId)
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

    public getAssignModel(): AssignModel
    {
        const branch = { id: this.stop.branchId } as Branch;
        const jobIds = _.uniq(_.map(this.stop.items, 'jobId'));
        return new AssignModel(this.stop.assignedTo, branch, jobIds, this.isReadOnlyUser, undefined);
    }

    public onAssigned(event: AssignModalResult)
    {
        this.stop.assignedTo = event.newUser.name;
    }

    public selectJobs(select: boolean, jobId?: number): void
    {
        let filterToApply = function(item: StopItem): boolean { return true; };

        if (!_.isNull(jobId))
        {
            //if it is not null it means the user click on a group
            filterToApply = function(item: StopItem): boolean { return item.jobId == jobId; };
        }

        _.chain(this.stopsItems)
            .filter(filterToApply)
            .map(current => current.isSelected = select)
            .value();
    }

    public allChildrenSelected(jobId?: number): boolean
    {
        let filterToApply = function(item: StopItem): boolean { return true; };

        if (!_.isNull(jobId))
        {
            //if it is not null it means the user click on a group
            filterToApply = function(item: StopItem): boolean { return item.jobId == jobId; };
        }

        return _.every(
                _.filter(this.stopsItems, filterToApply),
                current => current.isSelected);
    }

    public selectedItems(): Array<StopItem>
    {
        return _.filter(this.stopsItems, current => current.isSelected);
    }

    public selectedLineItems(): Array<number>
    {
        return _.map(this.selectedItems(), 'lineItemId');
    }

    public closeEdit(event: any): void
    {
        this.isActionMode = false;
        event.preventDefault();
    }
}