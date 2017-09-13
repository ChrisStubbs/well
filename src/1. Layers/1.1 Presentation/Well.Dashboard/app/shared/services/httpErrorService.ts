import {Injectable}         from '@angular/core';
import {Response}           from '@angular/http';
import {Observable}         from 'rxjs/Observable';
import {ToasterService}     from 'angular2-toaster/angular2-toaster';
import {LogService}         from './logService';

@Injectable()
export class HttpErrorService
{
    constructor(
        private toasterService: ToasterService,
        private logService: LogService) { }

    public handleError(error: Response): Observable<any>
    {
        if (error.status && error.status === 404)
        {
            //Not an error, just no data. No popup required.
            return Observable.of(undefined);
        }
        else
        {
            try
            {
                const message: string = error.json()
                    ? JSON.stringify(error.json())
                    : (error.status ? error.status.toString() : 'Server error');
                this.logService.log('HTTP Error: ' + message);
                this.toasterService.pop('error', message, '');
            }
            catch (ex)
            {
                return Observable.throw(error);
            }
        }

        return Observable.throw(error);
    }
}