import {Injectable, Inject} from "@angular/core";
import {Http, Response} from '@angular/http'
import {Observable} from 'rxjs/Observable';
import {HttpErrorService} from '../shared/httpErrorService';

export interface IGlobalSettings {
    apiUrl: string;
    version: string;
    userName: string;
    identityName: string;
}

@Injectable()
export class GlobalSettingsService {
    globalSettings: IGlobalSettings;

    constructor(
        private _http: Http,
        private httpErrorService: HttpErrorService) {
        var configuredApiUrl = "#{OrderWellApi}"; //This variable can be replaced by Octopus during deployment :)
        this.globalSettings =
        {
            apiUrl: (configuredApiUrl[0] !== "#") ? configuredApiUrl : "http://localhost/well/api/",
            version: "",
            userName: "",
            identityName: ""
        };
    }

    public getSettings(): Observable<IGlobalSettings> {
        return this._http.get(this.globalSettings.apiUrl + 'global-settings')
            .map((response: Response) => this.mapSettings(<IGlobalSettings>response.json()))
            .catch(e => this.httpErrorService.handleError(e));
    }

    private mapSettings(settings: IGlobalSettings): IGlobalSettings {
        this.globalSettings.version = settings.version;
        this.globalSettings.userName = settings.userName;
        this.globalSettings.identityName = settings.identityName;
        return this.globalSettings;
    }

    public getBranches(): Observable<string> {
        return this._http.get(this.globalSettings.apiUrl + 'user-branches')
            .map((response: Response) => <string>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }
}