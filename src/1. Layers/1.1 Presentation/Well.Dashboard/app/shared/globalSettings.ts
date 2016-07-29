import {Injectable, Inject} from "@angular/core";

export interface IGlobalSettings {
    apiUrl: string;
    deliveryId: number;
    username: string;
    domain: string;
}

@Injectable()
export class GlobalSettingsService {
    constructor() {
        var configuredApiUrl = "#{OrderWellApi}"; //This variable can be replaced by Octopus during deployment :)
        this.globalSettings =
        {
            apiUrl: (configuredApiUrl[0] !== "#") ? configuredApiUrl : "http://localhost/well/api/",
            deliveryId: 1   //TODO - Remove this from global settings once Angular routing is in place
        };
    }
    globalSettings: IGlobalSettings;
}