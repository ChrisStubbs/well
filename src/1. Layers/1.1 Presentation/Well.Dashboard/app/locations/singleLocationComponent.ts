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
import {IBulkEditResult}                                    from '../shared/action/bulkEditItem';
import {ManualCompletionModal}                              from '../shared/manualCompletion/manualCompletionModal';
import {SubmitActionModal}                                  from '../shared/action/submitActionModal';
import {ManualCompletionType}                               from '../shared/manualCompletion/manualCompletionRequest';
import {IJobIdResolutionStatus}                             from '../shared/models/jobIdResolutionStatus';
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
    public drivers: Array<string>;
    public assignees: Array<string>;
    public wellStatus: Array<ILookupValue>;

    @ViewChild(BulkEditActionModal) private bulkEditActionModal: BulkEditActionModal;
    @ViewChild(ManualCompletionModal) private manualCompletionModal: ManualCompletionModal;
    @ViewChild(SubmitActionModal) private submitActionModal: SubmitActionModal;

    private gridSource: Array<SingleLocationGroup> = [];
    private filters = new SingleLocationFilter();
    private source: SingleLocationHeader = new SingleLocationHeader();
    private isReadOnlyUser: boolean = false;
    private actionOptions: string[] = ['Manually Complete', 'Manually Bypass',
                                       'Edit Exceptions', 'Submit Exceptions'];

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

        this.refreshLocationFromApi();

        this.isReadOnlyUser = this.securityService
            .hasPermission(this.globalSettingsService.globalSettings.permissions, this.securityService.readOnly);
    }

    private refreshLocationFromApi(): void 
    {
        this.route.queryParams
            .flatMap(data =>
            {
                return Observable.forkJoin(
                    this.lookupService.get(LookupsEnum.ResolutionStatus),
                    this.lookupService.get(LookupsEnum.JobType),
                    this.locationsService.getSingleLocation((<any>data).locationId),
                    this.lookupService.get(LookupsEnum.WellStatus)
                );
            })
            .takeWhile(() => this.isAlive)
            .subscribe(res =>
            {
                this.resolutionStatuses = res[0];
                this.jobTypes = res[1];
                this.source = res[2];
                this.wellStatus = res[3];
                this.drivers = [];
                this.assignees = [];

                _.forEach(this.source.details, (current: SingleLocation) =>
                {
                    current.assignee = current.assignee || 'Unallocated';

                    this.drivers.push(current.driver || '');
                    this.assignees.push(current.assignee);
                });

                this.drivers = _.chain(this.drivers).uniq().filter(current => !_.isEmpty(current)).orderBy().value();
                this.assignees = _.chain(this.assignees).uniq().orderBy().value();

                this.buildGridSource();
            });
    }

    private buildGridSource(): void
    {
        const expanded = _.chain(this.gridSource)
            .filter((current: SingleLocationGroup) => current.isExpanded)
            .map('invoice')
            .value();
        this.gridSource = [];

        _.chain(this.fillGridSource())
            .groupBy((current: SingleLocation) => current.activityId)
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
                item.isInvoice = current[0].isInvoice;
                item.accountNumber = current[0].accountNumber;
                item.activityId = current[0].activityId;

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

    public bulkEditSave(result: IBulkEditResult): void
    {
        _.forEach(result.statuses, x =>
        {
            _.chain(this.source.details)
                .filter((current: SingleLocation) => current.jobId == x.jobId)
                .forEach((current: SingleLocation) => {
                    current.resolution = x.status.description;
                    current.resolutionId = x.status.description;
                })
                .value();
        });

        this.buildGridSource();
    }

    public manualCompletionSubmitted(results: IJobIdResolutionStatus[]): void
    {
        this.refreshLocationFromApi();
    }

    private submitAction(action: string): void
    {
        switch (action)
        {
        case 'Manually Complete':
            this.manualCompletionModal.show(ManualCompletionType.CompleteAsClean);
            break;
        case 'Manually Bypass':
            this.manualCompletionModal.show(ManualCompletionType.CompleteAsBypassed);
            break;
        case 'Edit Exceptions':
            this.bulkEditActionModal.show();
            break;
        case 'Submit Exceptions':
            this.submitActionModal.show();
            break;
        default:
            return;
        }
    }
}