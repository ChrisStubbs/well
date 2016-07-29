import {Injectable, Inject} from "@angular/core";

export interface IGlobalSettings {
    apiUrl: string;
    deliveryId: number;
    username: string;
    domain: string;
}

@Injectable()
export class GlobalSettingsService {
    constructor(
        @Inject("global.settings") private settings: IGlobalSettings
    ) {
        this.globalSettings = settings;
    }
    globalSettings: IGlobalSettings;
}