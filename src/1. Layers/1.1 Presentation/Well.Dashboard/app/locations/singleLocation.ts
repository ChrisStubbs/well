import { DatePipe }             from '@angular/common';
import { IFilter }              from '../shared/gridHelpers/IFilter';
import { GridHelpersFunctions } from '../shared/gridHelpers/gridHelpersFunctions';
import * as _                   from 'lodash';

export class SingleLocationHeader
{
    public branchName: string;
    public branchId: number;
    public accountNumber: string;
    public accountName: string;
    public pod: boolean;
    public cod: boolean;
    public accountAddress: string;
    public details: Array<SingleLocation>;
}

export class SingleLocationGroup
{
    constructor()
    {
        this.isExpanded = false;
    }

    public invoice: string;
    public totalException: number;
    public totalClean: number;
    public totalCredit: number;
    public details: Array<SingleLocation>;
    public isExpanded: boolean;
    public isInvoice: boolean;
}

export class SingleLocation
{
    //i should refactor this. make a static function to do this formatting
    private datePipe: DatePipe;

    constructor()
    {
        this.datePipe = new DatePipe('en-Gb');
        this.isSelected = false;
    }

    public isSelected: boolean;
    public jobId: number;
    public invoice: string;
    public driver: string;
    public date: Date;
    public get dateFormatted(): string
    {
        return this.datePipe.transform(this.date, 'yyyy-MM-dd');
    }
    public jobType: string;
    public jobTypeId: string;
    public jobStatus: string;
    public jobStatusId: string;
    public cod: boolean;
    public pod: boolean;
    public exceptions: number;
    public clean: number;
    public tba: number;
    public credit: number;
    public assignee: string;
    public resolution: string;
    public resolutionId: number;
    public isInvoice: boolean;
}

export class SingleLocationFilter implements IFilter
{
    constructor()
    {
        this.driver = '';
        this.dateFormatted = '';
        this.jobTypeId = undefined;
        this.jobStatus = undefined;
        this.exceptions = undefined;
        this.clean = undefined;
        this.assignee = '';
        this.resolutionId = undefined;
    }

    public driver: string;
    public dateFormatted: string;
    public jobTypeId: string;
    public jobStatus: string;
    public exceptions: boolean;
    public clean: boolean;
    public assignee: string;
    public resolutionId: number;

    public getFilterType(filterName: string): (value: any, value2: any) => boolean
    {
        switch (filterName) {
            case 'driver':
            case 'dateFormatted':
            case 'jobStatus':
            case 'assignee':
                return GridHelpersFunctions.isEqualFilter;

            case 'jobTypeId':
                return (value: number, value2: string) =>
                {
                    return GridHelpersFunctions.isEqualFilter(value, +value2);
                };

            case 'exceptions':
            case 'clean':
                return (value: number, value2?: boolean) =>
                {
                    if (_.isNull(value2))
                    {
                        return true;
                    }
                    if (value2.toString() == 'true')
                    {
                        return value > 0;
                    }

                    return value == 0;
                };

            case 'resolutionId':
                return GridHelpersFunctions.enumBitwiseAndCompare;
        }

        return undefined;
    }
}

export class Locations
{
    public branch: string;
    public branchId: number;
    public primaryAccountNumber: string;
    public accountNumber: string;
    public accountName: string;
    public address: string;
    public totalInvoices: number;
    public exceptions: number;
    public get cleans(): number
    {
        return this.totalInvoices - this.exceptions;
    }
}

export class LocationFilter implements  IFilter
{
    constructor()
    {
        this.primaryAccountNumber = '';
        this.accountNumber = '';
        this.accountName = '';
        this.address = '';
        this.exceptions = undefined;
        this.cleans = undefined;
    }

    public primaryAccountNumber: string;
    public accountNumber: string;
    public accountName: string;
    public address: string;
    public exceptions: boolean;
    public cleans: boolean;

    public getFilterType(filterName: string): (value: any, value2: any, sourceRow: any) => boolean
    {
        switch (filterName)
        {
            case 'accountNumber':
            case 'primaryAccountNumber':
                return GridHelpersFunctions.startsWithFilter;

            case 'accountName':
            case 'address':
                return GridHelpersFunctions.containsFilter;

            case 'exceptions':
            case 'cleans':
                return (value: number, value2?: boolean) => {
                    if (_.isNull(value2)) {
                        return true;
                    }
                    if (value2.toString() == 'true') {
                        return value > 0;
                    }

                    return value == 0;
                };
        }

        throw new Error('Method not implemented.');
    }
}