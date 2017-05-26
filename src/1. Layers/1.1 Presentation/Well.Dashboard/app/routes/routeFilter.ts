import { AppSearchParameters } from '../shared/appSearch/appSearch';
import { FilterMetadata } from 'primeng/components/common/api';
import * as _ from 'lodash';

export class RouteFilter
{
    public branchId: FilterMetadata = { matchMode: 'equals', value: '' };   
    public routeNumber: FilterMetadata = { matchMode: 'contains', value: '' };
    public routeDate: FilterMetadata = { matchMode: 'contains', value: '' };
    public stopCount: FilterMetadata = { matchMode: 'equals', value: '' };
    public routeStatusId: FilterMetadata = { matchMode: 'equals', value: '' };
    public exception: FilterMetadata = { matchMode: 'equals', value: '' };
    public driverName: FilterMetadata = { matchMode: 'contains', value: '' };
    public assignee: FilterMetadata = { matchMode: 'contains', value: '' };
    public id: FilterMetadata = { matchMode: 'in', value: undefined };
    
    public static toRouteFilter(params: AppSearchParameters): RouteFilter
    {
        const routeFilter = new RouteFilter();

        routeFilter.branchId.value = this.convertUndefined(params.branchId);
        routeFilter.routeNumber.value = this.convertUndefined(params.route);
        routeFilter.routeDate.value = this.convertUndefined(params.date);
        routeFilter.routeStatusId.value = this.convertUndefined(params.status);
        routeFilter.driverName.value = this.convertUndefined(params.driver);
        if (params.routeIds) {
            routeFilter.id.value = _.map(params.routeIds, _.ary(parseInt, 1));
        }

        return routeFilter;
    }

    private static convertUndefined(value: any): any
    {
        return (value === 'undefined') ? '' : value || '';
    }
}