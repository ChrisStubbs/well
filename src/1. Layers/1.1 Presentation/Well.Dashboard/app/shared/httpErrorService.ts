import {Injectable} from '@angular/core';
import {Response} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {ToasterService} from 'angular2-toaster/angular2-toaster';

@Injectable()
export class HttpErrorService {

    //Passing the toasterService through the constructor didn't work so passed as parameter instead, might work in future Angular2 version!
    handleError(error: Response, toasterService: ToasterService) {
        console.log(error);
        if (error.status && error.status === 404) {
            //No popup required.
        } else {
            var message = error.json() ? error.json().message : (error.status ? error.status : 'Server error');
            toasterService.pop('error', message, '');
        }
        return Observable.throw(error);
    }
}