import {Pipe, PipeTransform} from '@angular/core';

@Pipe({ name: 'orderByDate' })
export class OrderByDatePipe implements PipeTransform {

    transform(array: Array<string>, property: string): Array<string> {
        if (typeof property === "undefined") {
            return array;
        }

        let direction = property[0];
        let column = (direction === "-" || direction === "+") ? property.slice(1) : property;

        array.sort((a: any, b: any) => {

            let left = Number(new Date(a[column]));
            let right = Number(new Date(b[column]));

            return (direction === "-") ? right - left : left - right;
        });

        return array;
    }

}