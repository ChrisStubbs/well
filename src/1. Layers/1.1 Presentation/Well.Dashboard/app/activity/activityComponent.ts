import { NumberFormatStyle } from '@angular/common/src/pipes/intl';
import { Component, ViewChild }                             from '@angular/core';
import { ActivatedRoute }                                   from '@angular/router';
import { IObservableAlive }                                 from '../shared/IObservableAlive';
import { LookupService }                                    from '../shared/services/lookupService';
import { LookupsEnum }                                      from '../shared/services/lookupsEnum';
import
{
    ActivitySource,
    ActivitySourceGroup,
    ActivitySourceDetail,
    ActivityFilter
}                                                           from './activitySource';
import { Observable }                                       from 'rxjs';
import { ActivityService }                                  from './activityService';
import { ILookupValue }                                     from '../shared/services/ILookupValue';
import * as _ from 'lodash';
import { GridHelpersFunctions }                             from '../shared/gridHelpers/gridHelpersFunctions';
import { AssignModel, AssignModalResult }                   from '../shared/components/assignModel';
import { Branch }                                           from '../shared/branch/branch';
import { SecurityService }                                  from '../shared/services/securityService';
import { GlobalSettingsService }                            from '../shared/globalSettings';
import { EditExceptionsService }                            from '../exceptions/editExceptionsService';
import
{
    EditLineItemException, EditLineItemExceptionDetail
}                                                           from '../exceptions/editLineItemException';
import { ActionEditComponent }                              from '../shared/action/actionEditComponent';
import { ResolutionStatusEnum }                             from '../shared/services/resolutionStatusEnum';
import { ISubmitActionResult, ISubmitActionResultDetails }  from '../shared/action/submitActionModel';
import { BulkEditActionModal }                              from '../shared/action/bulkEditActionModal';
import { IBulkEditResult }                                  from '../shared/action/bulkEditItem';
import { ManualCompletionModal }                            from '../shared/manualCompletion/manualCompletionModal';
import { SubmitActionModal }                                from '../shared/action/submitActionModal';
import { ManualCompletionType }                             from '../shared/manualCompletion/manualCompletionRequest';
import { IJobIdResolutionStatus }                           from '../shared/models/jobIdResolutionStatus';
import 'rxjs/add/operator/takeWhile';
import 'rxjs/add/observable/forkJoin';
import { UpliftActionHelpers} from '../job/job';

@Component({
    selector: 'ow-activity',
    templateUrl: './app/activity/activityComponent.html',
    providers: [LookupService, SecurityService, ActivityService, EditExceptionsService],
    styles: ['.group1{ width: 3%; line-height: 30px; } ' +
        '.group2{ width: 12%; line-height: 26px; } ' +
        '.groupType{ width: 20%; line-height: 26px; } ' +
        '.group3{ width: 24% } ' +
        '.group4{ width: 6%; text-align: right; line-height: 26px; } ' +
        '.group5{ width: 6%; text-align: right; line-height: 26px; } ' +
        '.group6{ width: 6%; text-align: right; line-height: 26px; } ' +
        '.group7{ width: 6%; text-align: right; line-height: 26px; } ' +
        '.group8{ width: 16%; line-height: 26px; } ' +
        '.group9{ width: 38px; text-align: right; padding-right: 2px; line-height: 26px; } ' +
        '.colExpandAll { width: 3% } ' +
        '.colProduct { width: 6% } ' +
        '.colType { width: 8% } ' +
        '.colTobacco { width: 12% } ' +
        '.colDescription { width: 20% } ' +
        '.colNumbers { width: 6%; } ' +
        '.colHigh { width: 10% } ' +
        '.colCheckbox { width: 3% } ' +
        '.exceptionsFilter { width: calc( 6 * 2)}']
})
export class ActivityComponent implements IObservableAlive
{
    public isAlive: boolean = true;
    public source: ActivitySource = new ActivitySource();
    public resolutionStatuses: ILookupValue[];

    @ViewChild(ActionEditComponent) private actionEditComponent: ActionEditComponent;
    @ViewChild(BulkEditActionModal) private bulkEditActionModal: BulkEditActionModal;
    @ViewChild(ManualCompletionModal) private manualCompletionModal: ManualCompletionModal;
    @ViewChild(SubmitActionModal) private submitActionModal: SubmitActionModal;

    private gridSource: Array<ActivitySourceGroup>;
    private filters = new ActivityFilter();
    private inputFilterTimer: any;
    private jobTypes: Array<ILookupValue> = [];
    private tobaccoBags: Array<[string, string]>;
    private canEditExceptions: boolean;
    private canDoManualActions: boolean;

    constructor(
        private lookupService: LookupService,
        private activityService: ActivityService,
        private route: ActivatedRoute,
        private globalSettingsService: GlobalSettingsService,
        private securityService: SecurityService,
        private editExceptionsService: EditExceptionsService)
    {
        this.gridSource = [];
    }

    public ngOnInit(): void
    {
        this.refreshActivityFromApi();
        this.canDoManualActions = this.securityService.userHasPermission(SecurityService.manuallyCompleteBypass);
        this.canEditExceptions = this.securityService.userHasPermission(SecurityService.editExceptions);
    }

    private refreshActivityFromApi(): void
    {
        this.route.params
            .flatMap(data =>
            {
                return Observable.forkJoin(
                    this.lookupService.get(LookupsEnum.ResolutionStatus),
                    this.activityService.get(<number>(<any>data).id)
                );
            })
            .takeWhile(() => this.isAlive)
            .subscribe(res =>
            {
                this.resolutionStatuses = res[0];
                this.source = res[1];
                this.buildGridSource();

                this.tobaccoBags = _.chain(this.source.details)
                    .map((value: ActivitySourceDetail) => [value.barCodeFilter, value.tobacco])
                    .uniqWith((one: [string, string], another: [string, string]) =>
                        one[0] == another[0] && one[1] == another[1])
                    .filter(value => value[1] != '')
                    .value();

                _.chain(this.source.details)
                    .map((current: ActivitySourceDetail) => current.type)
                    .uniq()
                    .map((current: string) =>
                    {
                        this.jobTypes.push(
                            <any>{
                                key: current,
                                value: current
                            });

                        return current;
                    })
                    .value();
            });
    }

    public ngOnDestroy(): void
    {
        this.isAlive = false;
    }

    private calculateTotals(data: Array<ActivitySourceDetail>): any
    {
        let totalDamaged: number = 0;
        let totalShorts: number = 0;
        let totalExpected: number = 0;
        let totalActual: number = 0;

        _.forEach(data, (current: ActivitySourceDetail) =>
        {
            totalDamaged += current.damaged;
            totalShorts += current.shorts;
            totalExpected += current.expected;
            totalActual += current.actual;
        });

        return {
            totalDamaged: totalDamaged,
            totalShorts: totalShorts,
            totalExpected: totalExpected,
            totalActual: totalActual
        };
    }

    private buildGridSource(): void
    {
        const expanded = _.chain(this.gridSource)
            .filter((current: ActivitySourceGroup) => current.isExpanded)
            .map('jobId')
            .value();

        this.gridSource = _.chain(this.fillGridSource())
            .groupBy((current: ActivitySourceDetail) => current.jobId)
            .map((current: Array<ActivitySourceDetail>) =>
            {
                const item = new ActivitySourceGroup();
                const singleItem = _.head(current);
                const summary = this.calculateTotals(current);

                item.jobId = singleItem.jobId;
                item.type = singleItem.type;
                item.stopId = singleItem.stopId;
                item.stop = singleItem.stop;
                item.stopDate = singleItem.stopDate;
                item.resolution = singleItem.resolution;
                item.resolutionId = singleItem.resolutionId;
                item.totalExpected = summary.totalExpected;
                item.totalDamaged = summary.totalDamaged;
                item.totalShorts = summary.totalShorts;
                item.totalActual = summary.totalActual;
                item.isExpanded = _.includes(expanded, item.jobId);
                item.details = current;
                item.completedOnPaper = singleItem.completedOnPaper;

                return item;
            })
            .value();
    }

    private fillGridSource(): Array<ActivitySourceDetail>
    {
        return GridHelpersFunctions.applyGridFilter<ActivitySourceDetail, ActivityFilter>
            (this.source.details, this.filters);
    }

    public getAssignModel(): AssignModel
    {
        const branch = { id: this.source.branchId } as Branch;
        const jobIds = _.uniq(_.map(this.source.details, 'jobId'));
        return new AssignModel(this.source.assignee, branch, jobIds, undefined, false);
    }

    public onAssigned(event: AssignModalResult)
    {
        this.source.assignee = _.isNil(event.newUser) ? undefined : event.newUser.name;
    }

    public clearFilter()
    {
        this.filters = new ActivityFilter();
        this.buildGridSource();
    }

    public areAllExpanded(): boolean
    {
        return _.every(this.gridSource, 'isExpanded');
    }

    public expand(event: any, item: ActivitySourceGroup, expand?: boolean): void
    {
        item.isExpanded = expand || !item.isExpanded;
        event.preventDefault();
    }

    public voidLink(e: any): void
    {
        e.preventDefault();
    }

    public expandGroup(event: any): void
    {
        const action: boolean = !this.areAllExpanded();

        _.forEach(this.gridSource, (current: ActivitySourceGroup) =>
        {
            this.expand(event, current, action);
        });

        event.preventDefault();
    }

    public allChildrenSelected(jobId?: number): boolean
    {
        let filterToApply = function (item: ActivitySourceGroup): boolean { return true; };

        if (!_.isNull(jobId))
        {
            //if it is not null it means the user click on a group
            filterToApply = function (item: ActivitySourceGroup): boolean { return item.jobId == jobId; };
        }

        const items = _.chain(this.gridSource).map('details').flatten().filter(filterToApply).value();

        return _.every(items, (current: ActivitySourceDetail) => current.isSelected);
    }

    public filterFreeText(): void
    {
        GridHelpersFunctions.filterFreeText(this.inputFilterTimer)
            .then(() => this.buildGridSource())
            .catch(() => this.inputFilterTimer = undefined);
    }

    public selectJobs(select: boolean, jobId?: number): void
    {
        let filterToApply = function (item: ActivitySourceDetail): boolean { return true; };

        if (!_.isNull(jobId))
        {
            //if it is not null it means the user click on a group
            filterToApply = function (item: ActivitySourceDetail): boolean { return item.jobId == jobId; };
        }

        _.chain(this.gridSource)
            .map('details')
            .flatten()
            .filter(filterToApply)
            .map((current: ActivitySourceDetail) => current.isSelected = select)
            .value();
    }

    public editLineItemActions(item, $event): void
    {
        this.editExceptionsService.get([item.lineItemId])
            .takeWhile(() => this.isAlive)
            .subscribe((res: Array<EditLineItemException>) =>
            {
                this.actionEditComponent.show(res[0]);
            });
    }

    public lineItemSaved(data: EditLineItemException): void
    {
        //find the invoice edited (via lineitem edit)
        const job = _.find(this.gridSource, current => current.jobId == data.jobId);
        let damages = 0;
        let shorts = 0;
        //find the line that was edited
        const lineItem = _.find(job.details, (current: ActivitySourceDetail) =>
            current.product == data.productNumber);

        //sum the shorts and damages sent from the server
        _.forEach(data.exceptions, (current: EditLineItemExceptionDetail) =>
        {
            if (current.exception == 'Short')
            {
                shorts += current.quantity;
            }
            else if (current.exception == 'Damage')
            {
                damages += current.quantity;
            }

        });

        //remove the shorts and damages from the current invoice based on the selected lineitem
        job.totalDamaged -= lineItem.damaged;
        job.totalShorts -= lineItem.shorts;

        //now lets add the values sent from server
        job.totalDamaged += damages;
        job.totalShorts += shorts;
        lineItem.shorts = shorts;
        lineItem.damaged = damages;
        lineItem.hasUnresolvedActions = data.hasUnresolvedActions;
        this.setResolutionStatus(job, data.resolutionId, data.resolutionStatus, data.id, data.exceptions.length > 0);
    }

    public selectedItems(): Array<ActivitySourceDetail>
    {
        return _.chain(this.gridSource)
            .map('details')
            .flatten()
            .filter((current: ActivitySourceDetail) => current.isSelected)
            .value();
    }

    public disableSubmitActions(): boolean
    {
        const items = this.selectedItems();
        if (items.length == 0)
        {
            return true;
        }

        return _.some(items,
            x => x.resolutionId !== ResolutionStatusEnum.PendingSubmission) ||
            (this.source.assignee !== this.globalSettingsService.globalSettings.userName);
    }

    private getSelectedJobIds(): number[]
    {
        return _.chain(this.selectedItems())
            .map('jobId')
            .uniq()
            .value();
    }

    private jobsSubmitted(data: ISubmitActionResult): void
    {
        _.forEach(data.details, (x: ISubmitActionResultDetails) =>
        {
            const job = _.find(this.gridSource, current => current.jobId === x.jobId);
            job.hasUnresolvedActions = false;
            this.setResolutionStatus(job, x.resolutionStatusId, x.resolutionStatusDescription);
        });
    }

    private bulkEditSave(result: IBulkEditResult): void
    {
        _.forEach(result.statuses,
            x =>
            {
                const job = _.find(this.gridSource, current => current.jobId === x.jobId);

                this.setResolutionStatus(job, x.status.value, x.status.description);
                _.forEach(job.details,
                    (current) =>
                    {
                        current.hasUnresolvedActions = false;
                    });
            });
    }

    public setResolutionStatus(
        job: ActivitySourceGroup, 
        resolutionId: number,
        resolutionDescription: string, 
        lineItemId?: number,
        hasLineItemActions?: boolean): void
    {
        job.resolution = resolutionDescription;
        job.resolutionId = resolutionId;

        _.forEach(job.details,
            (current) =>
            {
                current.resolutionDescription = resolutionDescription;
                current.resolutionId = resolutionId;
                if (!_.isUndefined(lineItemId) && lineItemId == current.lineItemId)
                {
                    if (!_.isUndefined(hasLineItemActions))
                    {
                        current.hasLineItemActions = hasLineItemActions;
                    }
                }
            });
    }

    private disableBulkEdit(): boolean
    {
        return (this.selectedItems().length === 0
            || this.source.assignee !== this.globalSettingsService.globalSettings.userName);
    }

    public selectedLineItems(): Array<number>
    {
        return _.map(this.selectedItems(), 'lineItemId');
    }

    public manualCompletionSubmitted(results: IJobIdResolutionStatus[]): void
    {
        this.refreshActivityFromApi();
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

    private getUpliftActionCode(item: ActivitySourceDetail): string {
        return UpliftActionHelpers.getUpliftActionCode(item.jobTypeId, item.upliftAction);
    }
}