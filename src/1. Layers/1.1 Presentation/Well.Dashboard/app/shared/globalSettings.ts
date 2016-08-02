import {Injectable, Inject} from "@angular/core";
import {Http, Response} from '@angular/http'
import {Observable} from 'rxjs/Observable';

export interface IGlobalSettings {
    apiUrl: string;
}

@Injectable()
export class GlobalSettingsService {
    globalSettings: IGlobalSettings;

    constructor(private _http: Http) {
        var configuredApiUrl = "#{OrderWellApi}"; //This variable can be replaced by Octopus during deployment :)
        this.globalSettings =
        {
            apiUrl: (configuredApiUrl[0] !== "#") ? configuredApiUrl : "http://localhost/well/api/"
        };
    }

    public getVersion(): Observable<string> {
        return this._http.get(this.globalSettings.apiUrl + 'version')
            .map((response: Response) => <string>response.json())
            //.do(data => console.log("Version: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    public getBranches(): Observable<string> {
        return this._http.get(this.globalSettings.apiUrl + 'user-branches')
            .map((response: Response) => <string>response.json())
            //.do(data => console.log("Branches: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    private handleError(error: Response) {
        console.log(error);
        return Observable.throw(error.json().error || 'Server error');
    }


}