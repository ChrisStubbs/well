import { PipeTransform, Pipe } from '@angular/core';
import * as moment from 'moment/moment';

@Pipe({
    name: 'outstanding'
})
export class OutstandingPipe implements PipeTransform {
    transform(value: any[], args: any[]): any[] {

        let outstandingOnly: boolean = args[0]; /*args[0] ? args[0] : null;*/
        let propertyName: string = args[1];

        if (!outstandingOnly) {
            return value;
        }

        return value.filter((delivery: any) => this.filterOutstanding(delivery, propertyName));
    }

    filterOutstanding(list: any, propertyName: string) {
        if (list.hasOwnProperty(propertyName)) {
            var today = moment();
            var propertyValue = moment(list[propertyName]);
            return propertyValue.isBefore(today, 'day');
        }
        return false;
    }
}

