import {Injectable, Compiler } from '@angular/core';
import {Response, RequestOptions, Headers} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {HttpErrorService} from '../shared/httpErrorService';
import { HttpService } from './httpService';
import { User } from '../user_preferences/user';

export class GlobalSettings {
    public apiUrl: string;
    public version: string;
    public userName: string;
    public identityName: string;
    public permissions: string[];
    public user: User;
  
}

@Injectable() 
export class GlobalSettingsService {
    public globalSettings: GlobalSettings;

    public jsonOptions: RequestOptions = new RequestOptions({
        headers: new Headers({ 'Content-Type': 'application/json' })
    });

    constructor(
        private http: HttpService,
        private httpErrorService: HttpErrorService,
        private compiler: Compiler) { 

        const configuredApiUrl = '#{OrderWellApi}'; //This variable can be replaced by Octopus during deployment :)
        this.globalSettings = new GlobalSettings();
        this.globalSettings.apiUrl = (configuredApiUrl[0] !== '#') ? configuredApiUrl : 'http://localhost/well/api/';
        this.globalSettings.version = '';
        this.globalSettings.userName = '';
        this.globalSettings.identityName = '';
    }

    public initApp(): Promise<GlobalSettings> {
        this.compiler.clearCache();  //Ensure templates are not cached
        return this.getSettings();
    }

    public getSettings(): Promise<GlobalSettings> {
        return this.http.get(this.globalSettings.apiUrl + 'GlobalSettings')
            .map((response: Response) => {
                this.mapSettings(<GlobalSettings>response.json());
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
        this.globalSettings.user = settings.user;
        return this.globalSettings;
    }

    public getBranches(): Observable<string> {
        return this.http.get(this.globalSettings.apiUrl + 'user-branches')
            .map((response: Response) => <string>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }
}
