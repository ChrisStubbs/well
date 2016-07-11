import {Injectable, Inject} from "angular2/core";

export class GlobalSettings {
    apiUrl: string;
}

@Injectable()
export class GlobalSettingsService {
    constructor(
        @Inject("global.settings") private settings: GlobalSettings
    ) {
        this.globalSettings = settings;
    }
    globalSettings: GlobalSettings;
}