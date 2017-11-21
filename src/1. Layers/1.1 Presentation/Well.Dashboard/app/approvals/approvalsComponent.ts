import { 
    ISubmitActionModel, 
    ISubmitActionResult 
}                                           from './../shared/action/submitActionModel';
import { VOID_VALUE }                       from '@angular/animations/browser/src/render/transition_animation_engine';
import { NumberFormatStyle }                from '@angular/common/src/pipes/intl';
import { Component, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute }                   from '@angular/router';
import { IObservableAlive }                 from '../shared/IObservableAlive';
import { ApprovalsService }                 from './approvalsService';
import { Approval, ApprovalFilter }         from './approval';
import * as _                               from 'lodash';
import { GlobalSettingsService }            from '../shared/globalSettings';
import { BranchService }                    from '../shared/branch/branchService';
import { GridHelpersFunctions }             from '../shared/gridHelpers/gridHelpers';
import { AssignModalResult, AssignModel }   from '../shared/components/assignModel';
import { Branch }                           from '../shared/branch/branch';
import { SecurityService }                  from '../shared/services/securityService';
import { SubmitActionModal }                from '../shared/action/submitActionModal';
import { ActionService }                    from '../shared/action/action';
import { ToasterService }                   from 'angular2-toaster';

class Sort {
    constructor() {
        this.mField = '';
        this.mdirection = 'asc';
    }

    private mField: string;
    public get field(): string {
        return this.mField;
    }

    public set field(value: string) {
        if (this.mField != value || (this.mField == value && this.mdirection == 'desc')) {
            this.mdirection = 'asc';
        }
        else {
            this.mdirection = 'desc';
        }

        this.mField = value;
    }

    private mdirection: string;
    public get direction(): string {
        return this.mdirection;
    }
}

@Component({
    selector: 'ow-approval',
    templateUrl: './app/approvals/approvalsComponent.html',
    providers: [ApprovalsService, ActionService],
    styles: ['.colAccount {width: 10% } ' +
        '.colInvoice {width: 10% } ' +
        '.colQty { width: 6% }' +
        '.colValue { width: 7% }' +
        '.colUser { width: 11% }' +
        '.colCheckbox { width: 3% } ' +
        'th { cursor: pointer }']
})
export class ApprovalsComponent implements IObservableAlive
{
    public isAlive: boolean = true;
    public source: Array<Approval>;
    public sortField: Sort;

    @ViewChild(SubmitActionModal) private submitActionModal: SubmitActionModal;
    @ViewChild('closeModal') private closeBtn: ElementRef;

    private gridSource: Array<Approval> = [];
    private assignees: Array<string> = [];
    private assigneesTo: Array<string> = [];
    private filters: ApprovalFilter = new ApprovalFilter();
    private branches: Array<[string, string]>;
    private thresholdFilter: boolean = false;
    private isReadOnlyUser: boolean = false;
    private inputFilterTimer: any;
    private totalQuantity: number = 0;

    constructor(
        private approvalsService: ApprovalsService,
        private route: ActivatedRoute,
        private securityService: SecurityService,
        private globalSettingsService: GlobalSettingsService,
        private actionService: ActionService,
        private branchService: BranchService,
        private toasterService: ToasterService) { }

    public ngOnInit(): void
    {
        this.globalSettingsService.setCurrentBranchFromUrl(this.route);
        this.sortField = new Sort();
        this.sortField.field = 'branchName';
        
        this.branchService.getBranchesValueList(this.globalSettingsService.globalSettings.userName)
            .takeWhile(() => this.isAlive)
            .subscribe((branches: Array<[string, string]>) => {
                this.branches = branches;
                if (!this.globalSettingsService.currentBranchId) {
                    this.globalSettingsService.currentBranchId = +this.branches[0][0];
                }
                this.refreshDataFromAPI();
            });

        this.isReadOnlyUser = !this.securityService.userHasPermission(SecurityService.submitCreditApprovals);
    }

    public refreshDataFromAPI(): void
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
    }

    public fillGridSource(): void 
    {

        const filteredValues =
            GridHelpersFunctions.applyGridFilter<Approval, ApprovalFilter>(this.source, this.filters);

        if (this.assignees.length === 0) {
            this.assignees = [];
            this.assigneesTo = [];

            _.forEach(filteredValues, (current: Approval) => {
                this.assignees.push(current.submittedBy || 'Unallocated');
                this.assigneesTo.push(current.assignedTo || 'Unallocated');
            });

            this.assignees = _.sortBy(_.uniq(this.assignees));
            this.assigneesTo = _.sortBy(_.uniq(this.assigneesTo));
        }

        this.gridSource = filteredValues;
        this.sortData(undefined, undefined);
    }

    private sortData(field: string, event: any): void
    {
        if (!_.isNil(event)
            && (event.target.tagName == 'SELECT'
                || event.target.tagName == 'INPUT')) {
            //user clicked on a filter element
            return;
        }

        if (field) {
            this.sortField.field = field;
        }

        this.gridSource = _.orderBy(this.gridSource, this.sortField.field, this.sortField.direction);
    }

    private isSortedBy(field: string): boolean
    {
        return this.sortField.field == field;
    }

    private getSortStyles(field: string): any
    {
        const asc: boolean = this.sortField.direction == 'asc';

        return {
            ['glyphicon-sort-by-attributes']: asc,
            ['glyphicon-sort-by-attributes-alt']: !asc,
            invisible: this.sortField.field != field
        };
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

    private disableSubmitActions(): boolean {
        return this.selectedItems().length == 0;
    }

    private getTotalQuantitySelected(): number 
    {
        return _.sumBy(this.selectedItems(), (current: Approval) => current.creditQuantity);
    }

    private getTotalCreditedSelected(): number 
    {
        return _.sumBy(this.selectedItems(), (current: Approval) => current.creditValue);
    }

    private submitActions(): void {
        this.submitActionModal.show();
    }

    public selectedItems(): Array<Approval> {
        return _.filter(this.gridSource, (current: Approval) => {
            return current.isSelected &&
                (current.assignedTo || '') == this.globalSettingsService.globalSettings.userName;
        });
    }

    private currentUserThreshold(): number {
        return this.globalSettingsService.globalSettings.user.threshold;
    }

    public setCreditFilterValue(event): void {
        if (!event.target.checked) {
            this.filters.creditValue = this.filters.getCreditUpperLimit();
        }
        else {
            this.filters.creditValue = this.globalSettingsService.globalSettings.user.threshold;
        }

        this.fillGridSource();
    }

    public getSelectedJobIds(): Array<number> {
        return _.map(this.selectedItems(), 'jobId');
    }

    public allChildrenSelected(): boolean {
        return _.every(this.gridSource, (current: Approval) => current.isSelected);
    }

    public selectAll(select: boolean): void {
        _.map(this.gridSource, (current: Approval) => current.isSelected = select);
    }

    private jobsSubmitted(): void {
        this.approvalsService.get()
            .takeWhile(() => this.isAlive)
            .subscribe((data: Approval[]) => {
                this.source = data;
                this.fillGridSource();
            });
    }

    public onAssigned(event: AssignModalResult) {
        const userName = _.isNil(event.newUser) ? 'Unallocated' : event.newUser.name; 

        _.find(this.source, (current: Approval) => {
            const item = <Approval>event.source;

            return item.jobId == current.jobId;
        }).assignedTo = userName;

        this.assignees = [];
        this.assigneesTo = [];
        this.filters.assignedTo = '';
        this.fillGridSource();
    }

    public getAssignModel(line: Approval): AssignModel {
        const branch = { id: line.branchId } as Branch;
        const jobIds = [line.jobId];
        return new AssignModel(line.assignedTo, branch, jobIds, line, true);
    }

    private rejectExceptions(): void
    {
        const submitAction: ISubmitActionModel = {
            submit: false,
            jobIds: this.getSelectedJobIds()
        };

        this.actionService.post(submitAction)
            .takeWhile(() => this.isAlive)
            .subscribe(((res: ISubmitActionResult) => 
            {
                this.closeBtn.nativeElement.click();
                if (res.isValid)
                {
                    this.toasterService.pop('success', res.message, '');
                    
                    this.refreshDataFromAPI();
                }
                else
                { 
                    this.toasterService.pop('error', res.message, '');
                }   
            }));
    }

    public changeBranch(event): void {
        if (event) {
            this.globalSettingsService.currentBranchId = +event.target.value;
            this.refreshDataFromAPI();
        }
    }
}