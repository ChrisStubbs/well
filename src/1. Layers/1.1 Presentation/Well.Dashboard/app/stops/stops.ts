import {IAccount} from "../account/account";

export interface IStop {
    id: number;
    plannedStopNumber: string;
    plannedArrivalTime: string;
    plannedDepartTime: string;
    routeHeaderId: number;
    dropId: string;
    locationId: string;
    deliveryDate: string;
    specialInstructions: string;
    startWindow: string;
    endWindow: string;
    textField1: string;
    textField2: string;
    textField3: string;
    textField4: string;
    accounts : IAccount[];
}