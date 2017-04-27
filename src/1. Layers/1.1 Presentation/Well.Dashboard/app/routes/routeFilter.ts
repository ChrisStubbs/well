import { AppSearchParameters } from '../shared/appSearch/appSearch';
import { FilterMetadata } from 'primeng/components/common/api';

export class RouteFilter
{
    public branch: FilterMetadata;
    public route: FilterMetadata;
    public routeDate: FilterMetadata;
    public stopCount: FilterMetadata;
    public routeStatus: FilterMetadata;
    public driverName: FilterMetadata;

    public static toRouteFilter(params: AppSearchParameters): RouteFilter
    {
        const routeFilter = new RouteFilter();

        routeFilter.branch = { matchMode: 'contains', value: params.branchId || '' };
        routeFilter.route = { matchMode: 'contains', value: params.route || ''};
        routeFilter.routeDate = { matchMode: 'contains', value: params.date || '' };
        routeFilter.routeStatus = { matchMode: 'contains', value: params.status || '' };
        routeFilter.driverName = { matchMode: 'contains', value: params.driver || ''};
       
        return routeFilter;
    }
}