import { IFilter }              from '../shared/gridHelpers/IFilter';
import * as _                   from 'lodash';
import { AppSearchParameters }  from '../shared/appSearch/appSearchParameters';
import { DatePipe }             from '@angular/common';
import { GridHelpersFunctions } from '../shared/gridHelpers/gridHelpersFunctions';

export class RouteFilter implements IFilter
{
    public branchId?: number;
    public routeNumber: string;
    public dateFormatted: string;
    public routeStatusId?: number;
    public exceptionCount: boolean;
    public cleanCount: boolean;
    public driverName: string;
    public assignee: string;

    constructor()
    {
        this.branchId = undefined;
        this.routeNumber = '';
        this.dateFormatted = '';
        this.routeStatusId = undefined;
        this.exceptionCount = undefined;
        this.cleanCount = undefined;
        this.driverName = '';
        this.assignee = '';
    }

    public getFilterType(filterName: string): (value: any, value2: any) => boolean
    {
        switch (filterName)
        {
            case 'branchId':
                //this filter is handle in the component
                return (value: number, value2: number) => {
                        return true;
                    };

            case 'routeNumber':
            case 'dateFormatted':
            case 'driverName':
            case 'assignee':
                return GridHelpersFunctions.isEqualFilter;

            case 'routeStatusId':
                return (value: number, value2: number) =>
                {
                    const v = +value;
                    const v2 = +value2;

                    const f = GridHelpersFunctions.isEqualFilter;

                    return  f(v, v2);
                };

            case 'exceptionCount':
            case 'cleanCount':
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

        return undefined;
    }

    public static toRouteFilter(params: AppSearchParameters): RouteFilter
    {
        const routeFilter = new RouteFilter();
        const datePipe: DatePipe = new DatePipe('en-Gb');

        routeFilter.branchId = params.branchId;
        routeFilter.routeNumber = params.route;
        routeFilter.dateFormatted = _.isNil(params.date) ? undefined : datePipe.transform(params.date, 'yyyy-MM-dd');
        routeFilter.routeStatusId = params.status;
        routeFilter.driverName = params.driver;

        return routeFilter;
    }

}
