import { Component, ViewChild } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { RoutesService } from './routesService';
import { SingleRoute, SingleRouteSource, SingleRouteFilter } from './singleRoute';
import { ActivatedRoute } from '@angular/router';
import * as _ from 'lodash';
import { AssignModel, AssignModalResult } from '../shared/components/components';
import { Branch } from '../shared/branch/branch';
import { SecurityService } from '../shared/security/securityService';
import { GlobalSettingsService } from '../shared/globalSettings';
import { IObservableAlive } from '../shared/IObservableAlive';
import { SingleRouteItem } from './singleRoute';
import { LookupService, LookupsEnum, ILookupValue, ResolutionStatusEnum } from '../shared/services/services';
import { Observable } from 'rxjs';
import { GridHelpersFunctions } from '../shared/gridHelpers/gridHelpersFunctions';
import 'rxjs/add/operator/mergeMap';
import { ISubmitActionResult } from '../shared/action/submitActionModel';
import { JobService, GrnHelpers } from '../job/job';
import { ISubmitActionResultDetails } from '../shared/action/submitActionModel';
import { BulkEditActionModal } from '../shared/action/bulkEditActionModal';
import { IBulkEditResult } from '../shared/action/bulkEditItem';

@Component({
    selector: 'ow-route',
    templateUrl: './app/routes/singleRouteComponent.html',
    providers: [RoutesService, LookupService, CurrencyPipe, JobService]
})
export class SingleRouteComponent implements IObservableAlive
{
    public branchId: number;
    public driver: string;
    public routeDate: Date;
    public routeNumber: string;
    public isAlive: boolean = true;
    public jobTypes: Array<ILookupValue>;
    public wellStatus: Array<ILookupValue>;
    public assignees: Array<string>;
    public resolutionStatuses: Array<ILookupValue>;

    private routeId: number;
    private isReadOnlyUser: boolean = false;
    private source = Array<SingleRouteSource>();
    private gridSource = Array<SingleRouteSource>();
    private filters = new SingleRouteFilter();
    private inputFilterTimer: any;
    @ViewChild(BulkEditActionModal) private bulkEditActionModal: BulkEditActionModal;

    constructor(
        private lookupService: LookupService,
        private routeService: RoutesService,
        private route: ActivatedRoute,
        private securityService: SecurityService,
        private globalSettingsService: GlobalSettingsService,
        private jobService: JobService) { }

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
                this.branchId = data.branchId;
                this.routeNumber = data.routeNumber;
                this.driver = data.driver;
                this.routeDate = data.routeDate;

                this.source = this.buildGridSource(data.items);
                this.fillGridSource();
            });

        Observable.forkJoin(
            this.lookupService.get(LookupsEnum.JobType),
            this.lookupService.get(LookupsEnum.WellStatus),
            this.lookupService.get(LookupsEnum.ResolutionStatus)
        )
            .takeWhile(() => this.isAlive)
            .subscribe(res =>
            {
                this.jobTypes = res[0];
                this.wellStatus = res[1];
                this.resolutionStatuses = res[2];
            });

        this.isReadOnlyUser = this.securityService
            .hasPermission(this.globalSettingsService.globalSettings.permissions, this.securityService.readOnly);
    }

    public ngOnDestroy(): void
    {
        this.isAlive = false;
    }

    public getAssignModel(route: SingleRouteSource, level: string): AssignModel
    {
        const branch = { id: this.branchId } as Branch;
        const jobs = _.map(route.items, 'jobId');

        return new AssignModel(
            route.stopAssignee,
            branch,
            jobs,
            this.isReadOnlyUser,
            route);
    }

    public onAssigned(event: AssignModalResult): void
    {
        const userName = _.isNil(event.newUser) ? undefined : event.newUser.name;
        const route = _.find(this.source,
            (value: SingleRouteSource) => value.stopId == event.source.stopId) as SingleRouteSource;

        route.stopAssignee = userName;

        _.forEach(route.items, (value: SingleRouteItem) =>
        {
            value.assignee = userName;
            value.stopAssignee = userName;
        });

        this.fillGridSource();
    }

    public clearFilter(): void
    {
        this.jobService.setGrnForJob(1, 'test').subscribe();
        this.filters = new SingleRouteFilter();
        this.fillGridSource();
    }

    public selectStops(select: boolean, stop?: SingleRouteSource): void
    {
        let collection: Array<SingleRouteItem>;

        if (!_.isNil(stop))
        {
            //if it is not null it means the user click on a group - get all displayed items for stop
            collection = _.reduce(this.gridSource, (total: SingleRouteItem[], current: SingleRouteSource) =>
            {
                return total.concat(_.filter(current.items, (item: SingleRouteItem) => item.stopId == stop.stopId));
            }, []);
        }
        else
        {
            collection = _.reduce(this.gridSource, (total: SingleRouteItem[], current: SingleRouteSource) =>
            {
                return total.concat(current.items);
            }, []);
        }

        _.map(collection, current => current.isSelected = select);
    }

    public selectedItems(): Array<SingleRouteItem>
    {
        return _.chain(this.gridSource)
            .reduce((total: SingleRouteItem[], current: SingleRouteSource) =>
            {
                return total.concat(current.items);
            }, [])
            .filter((current: SingleRouteItem) => current.isSelected)
            .value();
    }

    public allChildrenSelected(stop?: SingleRouteSource): boolean
    {
        let collection: Array<SingleRouteItem>;

        if (!_.isNil(stop))
        {
            collection = stop.items;
        }
        else
        {
            collection = _.reduce(this.gridSource, (total: SingleRouteItem[], current: SingleRouteSource) =>
            {
                return total.concat(current.items);
            }, []);
        }

        return (collection.length > 0) ?
            _.every(collection, ((current: SingleRouteItem) => current.isSelected))
            : false;
    }

    public getSelectedJobIds(): number[]
    {
        return _.uniq(_.map(this.selectedItems(), 'jobId'));
    }

    private calculateTotals(data: Array<SingleRouteItem>): any
    {
        let totalExceptions: number = 0;
        let totalClean: number = 0;
        let totalTBA: number = 0;

        _.forEach(data, (current: SingleRouteItem) =>
        {
            totalExceptions += current.exceptions;
            totalClean += current.clean;
            totalTBA += current.tba;
        });

        return {
            totalExceptions: totalExceptions,
            totalClean: totalClean,
            totalTBA: totalTBA
        };
    }

    private buildGridSource(data: Array<SingleRouteItem>): Array<SingleRouteSource>
    {
        const result = Array<SingleRouteSource>();
        this.assignees = [];

        _.chain(data)
            .groupBy(current => current.stopId)
            .map((current: Array<SingleRouteItem>) =>
            {
                const item = new SingleRouteSource();
                const summary = this.calculateTotals(current);
                const singleItem = _.head(current);

                item.stopId = singleItem.stopId;
                item.stop = singleItem.stop;
                item.stopStatus = singleItem.stopStatus;
                item.totalExceptions = summary.totalExceptions;
                item.totalClean = summary.totalClean;

                item.totalTBA = singleItem.tba;
                item.stopAssignee = singleItem.stopAssignee;
                item.items = current;

                result.push(item);
            })
            .value();

        return result;
    }

    public areAllExpanded(): boolean
    {
        let value: boolean = true;

        _.forEach(this.source, current => value = value && current.isExpanded);

        return value;
    }

    public expandGroup(event: any): void
    {
        const action: boolean = !this.areAllExpanded();

        _.forEach(this.gridSource, (current: SingleRouteSource) =>
        {
            this.expand(event, current, action);
        });
    }

    private fillGridSource()
    {
        const values = Array<SingleRouteSource>();
        const assignees = [];
        _.map(this.source, (current: SingleRouteSource) =>
        {
            const filteredValues =
                GridHelpersFunctions.applyGridFilter<SingleRouteItem, SingleRouteFilter>(current.items, this.filters);

            assignees.push(current.stopAssignee || 'Unallocated');

            if (!_.isEmpty(filteredValues))
            {
                const newItem: SingleRouteSource = _.clone(current);

                newItem.items = _.clone(filteredValues);
                values.push(newItem);
            }
        });

        this.assignees = _.uniq(assignees);
        this.gridSource = values;
    }

    public filterFreeText(): void
    {
        GridHelpersFunctions.filterFreeText(this.inputFilterTimer)
            .then(() => this.fillGridSource())
            .catch(() => this.inputFilterTimer = undefined);
    }

    public expand(event: any, item: SingleRouteSource, expand?: boolean)
    {
        item.isExpanded = expand || !item.isExpanded;
        _.find(this.source, (current: SingleRouteSource) =>
            current.stopId == item.stopId).isExpanded = item.isExpanded;

        event.preventDefault();
    }

    public disableSubmitActions(): boolean
    {

        if (this.selectedItems().length === 0)
        {
            return true;
        }
        return _.some(this.selectedItems(), (x: SingleRouteItem) =>
            x.resolutionId !== ResolutionStatusEnum.PendingSubmission ||
            (x.stopAssignee || '') != this.globalSettingsService.globalSettings.userName);
    }

    private jobsSubmitted(data: ISubmitActionResult): void
    {
        _.forEach(data.details,
            (x: ISubmitActionResultDetails) =>
            {
                const job = _.find(this.selectedItems(), current => current.jobId === x.jobId);
                job.resolution = x.resolutionStatusDescription;
                job.resolutionId = x.resolutionStatusId;
            });
    }

    private bulkEditSave(result: IBulkEditResult): void
    {
        _.forEach(result.statuses,
            x =>
            {
                const job = _.find(this.selectedItems(), current => current.jobId === x.jobId);
                job.resolution = x.status.description;
                job.resolutionId = x.status.value;
            });
    }

    private isGrnRequired = (item: SingleRouteItem): boolean =>
    {
        return GrnHelpers.isGrnRequired(item);
    }

    private bulkEdit(): void 
    {
        this.bulkEditActionModal.show();
    }
}