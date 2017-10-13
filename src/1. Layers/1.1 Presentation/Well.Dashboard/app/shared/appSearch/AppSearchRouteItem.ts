import { IAppSearchItem } from './IAppSearchItem';
export class AppSearchRouteItem implements IAppSearchItem {
    public itemType: number;
    public id: number;
    public routeNumber: string;
    public date: string;
    public driverName: string;
}   