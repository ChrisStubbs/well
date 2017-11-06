import { Assignee }             from './../shared/models/assignee';
import { IFilter }              from '../shared/gridHelpers/IFilter';
import * as _                   from 'lodash';
import { AppSearchParameters }  from '../shared/appSearch/appSearchParameters';
import { DatePipe }             from '@angular/common';
import { GridHelpersFunctions } from '../shared/gridHelpers/gridHelpersFunctions';
import { Route }                from './route';

export class RouteFilter implements IFilter
{
    public branchId?: number;
    public routeNumber: string;
    public dateFormatted: string;
    public routeStatusId?: number;
    public exceptionCount: boolean;
    public driverName: string;
    public assignee: string;
    public jobIssueType?: number;
    public pageNumber: number;

    constructor()
    {
        this.branchId = undefined;
        this.routeNumber = '';
        this.dateFormatted = '';
        this.routeStatusId = undefined;
        this.exceptionCount = undefined;
        this.driverName = '';
        this.assignee = '';
        this.jobIssueType = undefined;
        this.pageNumber = 1;
    }

    public getFilterType(filterName: string): (value: any, value2: any, sourceRow: any) => boolean
    {
        switch (filterName)
        {
            case 'pageNumber':
            case 'branchId':
                //this filter is handle in the component
                return (value: number, value2: number) => {
                        return true;
                    };

            case 'routeNumber':
            case 'dateFormatted':
            case 'driverName':
                return GridHelpersFunctions.isEqualFilter;
                
            case 'assignee':
                return (value: string, value2: string, sourceRow: Route) =>
                {
                    if (_.isNil(sourceRow.assignees))
                    {
                        return value2 == 'Unallocated';
                    }

                    return sourceRow.assignees.some((current: Assignee) => current.name == value2);
                };

            case 'jobIssueType':
                return (value: number, value2: number) =>
                {
                    return +value2 == 0
                        || (+value2 == 7 && value != 0)
                        || GridHelpersFunctions.enumBitwiseAndCompare(+value2, +value);
                };

            case 'routeStatusId':
                return (value: number, value2: number) =>
                {
                    const v = +value;
                    const v2 = +value2;

                    const f = GridHelpersFunctions.isEqualFilter;

                    return  f(v, v2);
                };

            case 'exceptionCount':
                return (value: number, value2: number, sourceRow: Route) => {

                    if (+value2 == 1)
                    {
                        return sourceRow.exceptionCount > 0;
                    }

                    return sourceRow.cleanCount > 0;
                };
        }

        return undefined;
    }

    public static toRouteFilter(params: any): RouteFilter
    {
        const routeFilter = new RouteFilter();
        const datePipe: DatePipe = new DatePipe('en-Gb');

        routeFilter.branchId = params.branchId;
        routeFilter.routeNumber = _.isNil(params.routeNumber) ? params.route : params.routeNumber ;
        if (_.isNil(params.dateFormatted))
        {
            routeFilter.dateFormatted = _.isNil(params.date) 
                ? undefined 
                : datePipe.transform(params.date, 'yyyy-MM-dd');
        }
        else
        {
            routeFilter.dateFormatted = datePipe.transform(params.dateFormatted, 'yyyy-MM-dd');
        }

        routeFilter.routeStatusId = _.isNil(params.routeStatusId) ? params.status : params.routeStatusId;
        routeFilter.driverName = _.isNil(params.driverName) ? params.driver : params.driverName;
        routeFilter.exceptionCount =  params.exceptionCount;
        routeFilter.assignee = params.assignee;
        routeFilter.jobIssueType = params.jobIssueType;
        const intValue = +params.pageNumber;
        routeFilter.pageNumber = _.isNaN(intValue) ? 1 : intValue;
        return routeFilter;
    }

}
