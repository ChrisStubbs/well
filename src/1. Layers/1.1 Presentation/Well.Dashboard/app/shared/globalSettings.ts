import {Injectable, Inject} from "@angular/core";
import {Http, Response} from '@angular/http'
import {Observable} from 'rxjs/Observable';
import {HttpErrorService} from '../shared/httpErrorService';

export class GlobalSettings {
    apiUrl: string;
    version: string;
    userName: string;
    identityName: string;
    permissions: string[];
}

@Injectable()
export class GlobalSettingsService {
    globalSettings: GlobalSettings;

    constructor(private _http: Http, private httpErrorService: HttpErrorService) {
        var configuredApiUrl = "#{OrderWellApi}"; //This variable can be replaced by Octopus during deployment :)
        this.globalSettings = new GlobalSettings();
        this.globalSettings.apiUrl = (configuredApiUrl[0] !== "#") ? configuredApiUrl : "http://localhost/well/api/";
        this.globalSettings.version = "";
        this.globalSettings.userName = "";
        this.globalSettings.identityName = "";
    }

    public getSettings(): Promise<GlobalSettings> {
        return this._http.get(this.globalSettings.apiUrl + 'global-settings')
            .map((response: Response) => {
                this.mapSettings(<GlobalSettings>response.json());
                //console.log("Settings: " + JSON.stringify(this.globalSettings));
                return this.globalSettings;
            })
            .catch(e => this.httpErrorService.handleError(e))
            .toPromise();
    }

    private mapSettings(settings: GlobalSettings): GlobalSettings {
        this.globalSettings.version = settings.version;
        this.globalSettings.userName = settings.userName;
        this.globalSettings.identityName = settings.identityName;
        this.globalSettings.permissions = settings.permissions;
        return this.globalSettings;
    }

    public getBranches(): Observable<string> {
        return this._http.get(this.globalSettings.apiUrl + 'user-branches')
            .map((response: Response) => <string>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }
}