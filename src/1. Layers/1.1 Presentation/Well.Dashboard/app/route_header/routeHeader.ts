import {IStop} from "../stops/stops";


export interface IRouteHeader {
    companyId : number;
    routeNumber: string;
    routeDate : string;
    driverName: string;
    vehicleReg: string;
    startDepotCode: string;
    startDepotId: string;
    finishDepotCode: string;
    finishDepotId: string;
    subDepotCode: string;
    subDepotId: string;
    finishsubDepotCode: string;
    finishsubDepotId: string;
    plannedRouteStartTime: string;
    plannedRouteFinishTime: string;
    initialSealNumber: string;
    plannedDistance: number;
    plannedTravelTime: string;
    plannedStops: number;
    routeStatusId: number;
    routeStatus: string;
    routeImportId: number;
    stops: Array<any>;
    id: number;
    dateCreated: string;
    dateUpdated: string;
    createdBy: string;
    updatedBy: string;
    isDeleted: string;
    version: string;
}