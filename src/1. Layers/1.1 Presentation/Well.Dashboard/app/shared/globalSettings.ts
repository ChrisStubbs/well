import {Injectable, Inject} from "angular2/core";

export interface IGlobalSettings {
    apiUrl: string;
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