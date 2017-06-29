import { IFilter }                  from '../shared/gridHelpers/IFilter';
import { GridHelpersFunctions }     from '../shared/gridHelpers/gridHelpersFunctions';
import { DatePipe }                 from '@angular/common';

export class Approval
{
    private datePipe: DatePipe;

    constructor()
    {
        this.datePipe = new DatePipe('en-Gb');
        this.isSelected = false;
    }

    public jobId: number;
    public branchId: number;
    public branchName: string;
    public account: string;
    public accountId: number;
    public deliveryDate: Date;
    public get deliveryDateFormatted(): string
    {
        return this.datePipe.transform(this.deliveryDate, 'yyyy-MM-dd');
    };
    public invoiceNumber: string;
    public submittedBy: string;
    public dateSubmitted: Date;
    public get dateSubmittedFormatted(): string
    {
        return this.datePipe.transform(this.dateSubmitted, 'yyyy-MM-dd');
    };
    public creditQuantity: number;
    public creditValue: number;
    public assignedTo: string;
    public isSelected: boolean;
}

export class ApprovalFilter implements IFilter
{
    constructor()
    {
        this.branchName = '';
        this.account = '';
        this.deliveryDateFormatted = '';
        this.invoiceNumber = '';
        this.submittedBy = '';
        this.dateSubmittedFormatted = '';
        this.assignedTo = '';
        this.creditValue = this.getCreditUpperLimit();
    }

    public getCreditUpperLimit(): number
    {
        return 999999999;
    }

    private branchName: string;
    public account: string;
    public deliveryDateFormatted: string;
    public invoiceNumber: string;
    public submittedBy: string;
    public dateSubmittedFormatted: '';
    public assignedTo: string;
    public creditValue: number;

    public getFilterType(filterName: string): (value: any, value2: any) => boolean
    {
        switch (filterName) {
            case 'branchName':
            case 'deliveryDateFormatted':
            case 'dateSubmittedFormatted':
            case 'submittedBy':
            case 'assignedTo':
                return GridHelpersFunctions.isEqualFilter;

            case 'account':
            case 'invoiceNumber':
                return GridHelpersFunctions.containsFilter;

            case 'creditValue':
                return GridHelpersFunctions.lowerOrEqualThanFilter;
        }

        return undefined;
    }
}