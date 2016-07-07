import {Injectable} from 'angular2/core';
import {Http, Response} from 'angular2/http'
import {Observable} from 'rxjs/Observable';
import {IWidgetStats} from './widgetstats'

@Injectable()
export class WidgetStatsService {
    private _exceptionsUrl = '/Well/Api/';

    constructor(private _http: Http) { }

    autoUpdateDisabled(): Observable<boolean> {
        return this._http.get(this._exceptionsUrl + 'EnableSignular')
            .map((response: Response) => <boolean>response.json())
            .do(data => console.log("All: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    getWidgetStats(): Observable<IWidgetStats> {

        return this._http.get(this._exceptionsUrl + 'getwidgetstats')
            .map((response: Response) => <IWidgetStats>response.json())
            .do(data => console.log("All: " + JSON.stringify(data)))
            .catch(this.handleError);
    }


    private handleError(error: Response) {
        console.log(error);
        return Observable.throw(error.json().error || 'Server error');
    }



}