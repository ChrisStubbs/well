import { PipeTransform, Pipe } from '@angular/core';
import * as moment from 'moment/moment';

@Pipe({
    name: 'outstanding'
})
export class OutstandingPipe implements PipeTransform {
    public transform(value: any[], args: any[]): any[] {

        const outstandingOnly: boolean = args[0]; 
        const propertyName: string = args[1];

        if (!outstandingOnly) {
            return value;
        }

        return value.filter((delivery: any) => this.filterOutstanding(delivery, propertyName));
    }

    public filterOutstanding(list: any, propertyName: string) {
        if (list.hasOwnProperty(propertyName)) {
            const propertyValue = moment(list[propertyName]);
            return propertyValue.isBefore(moment(), 'day');
        }

        return false;
    }
}