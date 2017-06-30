import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IObservableAlive } from '../shared/IObservableAlive';
import { ApprovalsService } from './approvalsService';
import { Approval, ApprovalFilter } from './approval';
import * as _ from 'lodash';
import { GlobalSettingsService } from '../shared/globalSettings';
import { BranchService } from '../shared/branch/branchService';
import { GridHelpersFunctions } from '../shared/gridHelpers/gridHelpers';
import { AssignModalResult, AssignModel } from '../shared/components/assignModel';
import { Branch } from '../shared/branch/branch';
import { SecurityService } from '../shared/security/securityService';

@Component({
    selector: 'ow-approval',
    templateUrl: './app/approvals/approvalsComponent.html',
    providers: [ApprovalsService],
    styles: ['.colAccount {width: 10% } ' +
        '.colInvoice {width: 10% } ' +
        '.colQty { width: 6% }' +
        '.colValue { width: 7% }' +
        '.colUser { width: 11% }' +
        '.colCheckbox { width: 3% } ']
})
export class ApprovalsComponent implements IObservableAlive
{
    public isAlive: boolean = true;
    public source: Array<Approval>;

    private gridSource: Array<Approval> = [];
    private assignees: Array<string> = [];
    private assigneesTo: Array<string> = [];
    private filters: ApprovalFilter = new ApprovalFilter();
    private branches: Array<[string, string]>;
    private thresholdFilter: boolean = false;
    private isReadOnlyUser: boolean = false;
    private inputFilterTimer: any;

    constructor(
        private approvalsService: ApprovalsService,
        private route: ActivatedRoute,
        private securityService: SecurityService,
        private globalSettingsService: GlobalSettingsService,
        private branchService: BranchService) { }

    public ngOnInit(): void
    {
        this.route.params
            .flatMap(data =>
            {
                return this.approvalsService.get();
            }).takeWhile(() => this.isAlive)
            .subscribe((data: Approval[]) =>
            {
                this.source = data;
                this.fillGridSource();
            });

        this.branchService.getBranchesValueList(this.globalSettingsService.globalSettings.userName)
            .takeWhile(() => this.isAlive)
            .subscribe((branches: Array<[string, string]>) => this.branches = branches);

        this.isReadOnlyUser = this.securityService
            .hasPermission(this.globalSettingsService.globalSettings.permissions, this.securityService.readOnly);
    }

    public fillGridSource(): void
    {
        const filteredValues =
            GridHelpersFunctions.applyGridFilter<Approval, ApprovalFilter>(this.source, this.filters);

        if (this.assignees.length === 0)
        {
            this.assignees = [];
            this.assigneesTo = [];

            _.forEach(filteredValues, (current: Approval) =>
            {
                this.assignees.push(current.submittedBy || 'Unallocated');
                this.assigneesTo.push(current.assignedTo || 'Unallocated');
            });

            this.assigneesTo = _.sortBy(_.uniq(this.assigneesTo));
        }

        this.gridSource = filteredValues;
    }

    public ngOnDestroy(): void 
    {
        this.isAlive = false;
    }

    private clearFilter(): void
    {
        this.filters = new ApprovalFilter();
        this.thresholdFilter = false;
        this.filters.creditValue = this.filters.getCreditUpperLimit();
        this.fillGridSource();
    }

    public filterFreeText(): void
    {
        GridHelpersFunctions.filterFreeText(this.inputFilterTimer)
            .then(() => this.fillGridSource())
            .catch(() => this.inputFilterTimer = undefined);
    }

    private disableSubmitActions(): boolean 
    {
        return this.selectedItems().length == 0;
    }

    public selectedItems(): Array<Approval>
    {
        return _.filter(this.gridSource, (current: Approval) =>
        {
            return current.isSelected &&
                (current.assignedTo || '') == this.globalSettingsService.globalSettings.userName;
        });
    }

    private currentUserThreshold(): number
    {
        return this.globalSettingsService.globalSettings.user.threshold;
    }

    public setCreditFilterValue(event): void
    {
        if (!event.target.checked)
        {
            this.filters.creditValue = this.filters.getCreditUpperLimit();
        }
        else
        {
            this.filters.creditValue = this.globalSettingsService.globalSettings.user.threshold;
        }

        this.fillGridSource();
    }

    public getSelectedJobIds(): Array<number>
    {
        return _.map(this.selectedItems(), 'jobId');
    }

    public allChildrenSelected(): boolean
    {
        return _.every(this.gridSource, (current: Approval) => current.isSelected);
    }

    public selectAll(select: boolean): void
    {
        _.map(this.gridSource, (current: Approval) => current.isSelected = select);
    }

    private jobsSubmitted(): void
    {
        this.approvalsService.get()
            .takeWhile(() => this.isAlive)
            .subscribe((data: Approval[]) =>
            {
                this.source = data;
                this.fillGridSource();
            });
    }

    public onAssigned(event: AssignModalResult)
    {
        const userName = _.isNil(event.newUser) ? undefined : event.newUser.name;

        _.find(this.source, (current: Approval) =>
        {
            const item = <Approval>event.source;

            return item.jobId == current.jobId;
        }).assignedTo = userName;

        this.assignees = [];
        this.assigneesTo = [];
        this.fillGridSource();
    }

    public getAssignModel(line: Approval): AssignModel
    {
        const branch = { id: line.branchId } as Branch;
        const jobIds = [line.jobId];
        return new AssignModel(line.assignedTo, branch, jobIds, this.isReadOnlyUser, line);
    }
}