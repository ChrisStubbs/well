import {Pipe, PipeTransform} from '@angular/core';
import * as moment from 'moment';

@Pipe({ name: 'customDate' })
export class CustomDatePipe implements PipeTransform {

    public transform(value: Date, format: string = ""): string {

        if (!value) return "";
        return moment(value).format(format);
    }
}