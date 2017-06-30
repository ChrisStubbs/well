import { IFilter }              from '../shared/gridHelpers/IFilter';
import * as _                   from 'lodash';
import { AppSearchParameters }  from '../shared/appSearch/appSearchParameters';

export class RouteFilter implements IFilter
{
    public branchId?: number;
    public routeNumber: string = '';
    public routeDate?: Date = undefined;
    public stopCount?: number = undefined;
    public routeStatusId?: number = undefined;
    public hasExceptions: boolean = undefined;
    public hasClean: boolean = undefined;
    public driverName: string = '';
    public assignee: string = '';

    public getFilterType(filterName: string): (value: any, value2: any) => boolean
    {
        switch (filterName)
        {
            case 'exceptions':
            case 'clean':
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

        routeFilter.branchId = params.branchId;
        routeFilter.routeNumber = params.route;
        routeFilter.routeDate = params.date;
        routeFilter.routeStatusId = params.status;
        routeFilter.driverName = params.driver;

        return routeFilter;
    }

}
