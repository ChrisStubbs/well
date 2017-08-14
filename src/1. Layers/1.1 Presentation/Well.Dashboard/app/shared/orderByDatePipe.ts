import {Pipe, PipeTransform} from '@angular/core';

@Pipe({ name: 'orderByDate' })
export class OrderByDatePipe implements PipeTransform {

    public transform(array: Array<string>, property: string): Array<string> {
        if (typeof property === 'undefined') {
            return array;
        }

        const direction = property[0];
        const column = (direction === '-' || direction === '+') ? property.slice(1) : property;

        array.sort((a: any, b: any) => {

            const left = Number(new Date(a[column]));
            const right = Number(new Date(b[column]));

            return (direction === '-') ? right - left : left - right;
        });

        return array;
    }
}