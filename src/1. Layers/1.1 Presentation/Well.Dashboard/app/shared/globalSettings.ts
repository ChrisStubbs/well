import { Injectable, Compiler } from '@angular/core';
import { Response, RequestOptions, Headers } from '@angular/http';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { HttpErrorService } from './services/httpErrorService';
import { HttpService } from './services/httpService';
import { User } from '../user_preferences/user';
import { SessionStorageService } from 'ngx-webstorage';
import * as _ from 'lodash';

export class GlobalSettings {
    private baseUrl: string;

    public get apiUrl(): string {

        if (this.currentBranchId) {
            return `${this.baseUrl}${this.currentBranchId}/`;
        }
        return this.baseUrl;
    }

    public set apiUrl(theBaseApi: string) {
        this.baseUrl = theBaseApi;
    }

    public version: string;
    public userName: string;
    public identityName: string;
    public permissions: string[];
    public user: User;
    public crmBaseUrl: string;
    public currentBranchId?: number;
}

@Injectable()
export class GlobalSettingsService {
    public globalSettings: GlobalSettings;

    public get currentBranchId(): number | null {
        return this.globalSettings.currentBranchId;
    }

    public set currentBranchId(branchId: number | null) {
        this.globalSettings.currentBranchId = branchId;
    }

    private static cachePermissionKey = 'GlobalSettingsPermissions';

    public jsonOptions: RequestOptions = new RequestOptions({
        headers: new Headers({ 'Content-Type': 'application/json' })
    });

    constructor(
        private http: HttpService,
        private httpErrorService: HttpErrorService,
        private compiler: Compiler,
        private storageService: SessionStorageService) {
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
        this.globalSettings.crmBaseUrl = settings.crmBaseUrl;

        this.storageService.store(GlobalSettingsService.cachePermissionKey, this.globalSettings.permissions);

        return this.globalSettings;
    }

    public static getCachedPermissions(storageService: SessionStorageService): Array<string> {
        return <Array<string>>storageService.retrieve(GlobalSettingsService.cachePermissionKey);
    }

    public getBranches(): Observable<string> {
        return this.http.get(this.globalSettings.apiUrl + 'user-branches')
            .map((response: Response) => <string>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    } 

    public setCurrentBranchFromUrl(activatedRoute: ActivatedRoute) {
        if (!_.isNil(activatedRoute.snapshot.paramMap.get('branchId'))) {
            this.currentBranchId = +activatedRoute.snapshot.paramMap.get('branchId');
        } 
    }
}
