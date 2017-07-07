import { Component, ViewChild }                             from '@angular/core';
import { LookupService }                                    from '../shared/services/lookupService';
import { SecurityService }                                  from '../shared/security/securityService';
import { EditExceptionsService }                            from '../exceptions/editExceptionsService';
import { IObservableAlive }                                 from '../shared/IObservableAlive';
import { LocationsService }                                 from './locationsService';
import
{
    SingleLocation,
    SingleLocationHeader,
    SingleLocationFilter,
    SingleLocationGroup
}                                                           from './singleLocation';
import { ActivatedRoute }                                   from '@angular/router';
import { Observable }                                       from 'rxjs/Observable';
import { LookupsEnum }                                      from '../shared/services/lookupsEnum';
import { GlobalSettingsService }                            from '../shared/globalSettings';
import { ILookupValue }                                     from '../shared/services/ILookupValue';
import * as _                                               from 'lodash';
import { GridHelpersFunctions }                             from '../shared/gridHelpers/gridHelpersFunctions';
import { BulkEditActionModal }                              from '../shared/action/action';
import { ResolutionStatusEnum }                             from '../shared/services/resolutionStatusEnum';
import { AssignModalResult, AssignModel }                   from '../shared/components/assignModel';
import { Branch }                                           from '../shared/branch/branch';
import { ISubmitActionResult, ISubmitActionResultDetails }  from '../shared/action/submitActionModel';
import 'rxjs/add/operator/takeWhile';
import 'rxjs/add/observable/forkJoin';

@Component({
    selector: 'ow-singleLocation',
    templateUrl: './app/locations/singleLocationComponent.html',
    providers: [LookupService, SecurityService, LocationsService, EditExceptionsService],
})
export class SingleLocationComponent implements IObservableAlive
{
    public isAlive: boolean = true;
    public resolutionStatuses: ILookupValue[];
    public jobTypes: ILookupValue[];
    public jobStatus: ILookupValue[];
    public drivers: Array<string>;
    public assignees: Array<string>;

    @ViewChild(BulkEditActionModal) private bulkEditActionModal: BulkEditActionModal;

    private gridSource: Array<SingleLocationGroup>;
    private filters = new SingleLocationFilter();
    private source: SingleLocationHeader = new SingleLocationHeader();
    private isReadOnlyUser: boolean = false;

    constructor(
        private lookupService: LookupService,
        private route: ActivatedRoute,
        private securityService: SecurityService,
        private globalSettingsService: GlobalSettingsService,
        private locationsService: LocationsService) {}

    public ngOnDestroy(): void {
        this.isAlive = false;
    }

    public ngOnInit(): void {

        this.route.queryParams
            .flatMap(data =>
            {
                return Observable.forkJoin(
                    this.lookupService.get(LookupsEnum.ResolutionStatus),
                    this.lookupService.get(LookupsEnum.JobType),
                    this.lookupService.get(LookupsEnum.JobStatus),
                    this.locationsService.getSingleRoute(data.id, data.accountNumber, <number>data.branchId)
                );
            })
            .takeWhile(() => this.isAlive)
            .subscribe(res =>
            {
                this.resolutionStatuses = res[0];
                this.jobTypes = res[1];
                this.jobStatus = res[2];
                this.source = res[3];
                this.drivers = [];
                this.assignees = [];

                _.forEach(this.source.details, (current: SingleLocation) =>
                {
                    this.drivers.push(current.driver);
                    this.assignees.push(current.assignee || 'Unallocated');
                });

                this.drivers = _.chain(this.drivers).uniq().orderBy().value();
                this.assignees = _.chain(this.assignees).uniq().orderBy().value();

                this.buildGridSource();
            });

        this.isReadOnlyUser = this.securityService
            .hasPermission(this.globalSettingsService.globalSettings.permissions, this.securityService.readOnly);
    }

    private buildGridSource(): void
    {
        const expanded = _.chain(this.gridSource)
            .filter((current: SingleLocationGroup) => current.isExpanded)
            .map('invoice')
            .value();
        this.gridSource = [];

        _.chain(this.fillGridSource())
            .groupBy((current: SingleLocation) => current.invoice)
            .forEach(current =>
            {
                const accumulator = new SingleLocationGroup();

                accumulator.totalException = 0;
                accumulator.totalClean = 0;
                accumulator.totalCredit = 0;

                const item = _.reduce(current, (all: SingleLocationGroup, current: SingleLocation) =>
                {
                    all.totalException += current.exceptions;
                    all.totalClean += current.clean;
                    all.totalCredit += current.credit;

                    return all;
                }, accumulator);

                item.invoice = current[0].invoice;
                item.isExpanded = _.includes(expanded, current[0].invoice);
                item.details = current;

                this.gridSource.push(item);
            })
            .value();
    }

    private fillGridSource(): Array<SingleLocation>
    {
        return GridHelpersFunctions.applyGridFilter<SingleLocation, SingleLocationFilter>
        (this.source.details, this.filters);
    }

    public clearFilter()
    {
        this.filters = new SingleLocationFilter();
        this.buildGridSource();
    }

    private bulkEdit(): void
    {
        this.bulkEditActionModal.show();
    }

    public selectedItems(): Array<SingleLocation>
    {
        return _.chain(this.gridSource)
            .map('details')
            .flatten()
            .filter((current: SingleLocation) => current.isSelected)
            .value();
    }

    private disableBulkEdit(): boolean
    {
        return this.selectedItems().length === 0;
    }

    public disableSubmitActions(): boolean
    {
        const items = this.selectedItems();
        if (items.length === 0)
        {
            return true;
        }

        return _.some(items, x => x.resolutionId !== ResolutionStatusEnum.PendingSubmission);
    }

    private getSelectedJobIds(): number[]
    {
        return _.chain(this.selectedItems())
            .map('jobId')
            .uniq()
            .value();
    }

    public expand(event: any, item: SingleLocationGroup, expand?: boolean): void
    {
        item.isExpanded = expand || !item.isExpanded;
        event.preventDefault();
    }

    public expandAll(event: any): void
    {
        const action: boolean = !this.areAllExpanded();

        _.forEach(this.gridSource, (current: SingleLocationGroup) =>
        {
            this.expand(event, current, action);
        });

        event.preventDefault();
    }

    public areAllExpanded(): boolean
    {
        return _.every(this.gridSource, 'isExpanded');
    }

    public allChildrenSelected(invoice?: string): boolean
    {
        let filterToApply = function (item: SingleLocationGroup): boolean { return true; };

        if (invoice)
        {
            //if it is not null it means the user click on a group
            filterToApply = function (item: SingleLocationGroup): boolean { return item.invoice == invoice; };
        }

        return _.every(
            _.filter(this.gridSource, filterToApply),
            (current: SingleLocationGroup) => _.every(current.details, (item: SingleLocation) => item.isSelected));
    }

    public getAssignModel(location: SingleLocation): AssignModel
    {
        const branch = { id: this.source.branchId } as Branch;
        const jobIds = [location.jobId];

        return new AssignModel(location.assignee, branch, jobIds, this.isReadOnlyUser, location);
    }

    public onAssigned(event: AssignModalResult): void
    {
        const userName = _.isNil(event.newUser) ? undefined : event.newUser.name;
        const location = _.find(this.source.details,
            (value: SingleLocation) => value.invoice == event.source.invoice) as SingleLocation;

        location.assignee = userName;

        this.buildGridSource();
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

        this.buildGridSource();
    }

    public selectLocations(select: boolean, invoice?: string): void
    {
        let filterToApply = function (item: SingleLocation): boolean { return true; };

        if (!_.isNull(invoice))
        {
            //if it is not null it means the user click on a group
            filterToApply = function (item: SingleLocation): boolean { return item.invoice == invoice; };
        }

        _.chain(this.gridSource)
            .map('details')
            .flatten()
            .filter(filterToApply)
            .map((current: SingleLocation) => current.isSelected = select)
            .value();
    }
}