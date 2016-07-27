import {Injectable} from '@angular/core';
import {Http, Response, Headers, RequestOptions} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {IUser} from './user';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/add/operator/map';

@Injectable()
export class Userservice {

    constructor(private http: Http, private globalSettingsService: GlobalSettingsService) { }

    getUsers(name: string): Observable<IUser[]> {

        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'users/' + name)
            .map((response: Response) => <IUser[]>response.json())
            .catch(this.handleError);

    }

    private handleError(error: Response) {
        console.log(error);
        return Observable.throw(error.json().error || 'Server error');
    }

}