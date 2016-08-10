import {Injectable, Inject} from "@angular/core";
import {Http, Response} from '@angular/http'
import {Observable} from 'rxjs/Observable';
import {HttpErrorService} from '../shared/httpErrorService';
import {ToasterService} from 'angular2-toaster/angular2-toaster';

export interface IGlobalSettings {
    apiUrl: string;
}

@Injectable()
export class GlobalSettingsService {
    globalSettings: IGlobalSettings;

    constructor(
        private _http: Http,
        private httpErrorService: HttpErrorService,
        private toasterService: ToasterService) {
        var configuredApiUrl = "#{OrderWellApi}"; //This variable can be replaced by Octopus during deployment :)
        this.globalSettings =
        {
            apiUrl: (configuredApiUrl[0] !== "#") ? configuredApiUrl : "http://localhost/well/api/"
        };
    }

    public getVersion(): Observable<string> {
        return this._http.get(this.globalSettings.apiUrl + 'version')
            .map((response: Response) => <string>response.json())
            .catch(e => this.httpErrorService.handleError(e, this.toasterService));
    }

    public getBranches(): Observable<string> {
        return this._http.get(this.globalSettings.apiUrl + 'user-branches')
            .map((response: Response) => <string>response.json())
            .catch(e => this.httpErrorService.handleError(e, this.toasterService));
    }
}