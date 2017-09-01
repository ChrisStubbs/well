import {Injectable} from '@angular/core';

@Injectable()
export class LogService {

    public log(message: any, ...args: any[]): void {
        //(console && console.log) && console.log(message, ...args);
    }
}