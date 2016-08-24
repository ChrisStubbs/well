import {Pipe, PipeTransform} from '@angular/core';

@Pipe({ name: 'orderBy' })
export class OrderBy implements PipeTransform {

    static orderByComparator(a: any, b: any): number {

        if ((isNaN(parseFloat(a)) || !isFinite(a)) || (isNaN(parseFloat(b)) || !isFinite(b))) {
            //Isn't a number so lowercase the string to properly compare
            if (a.toLowerCase() < b.toLowerCase()) return -1;
            if (a.toLowerCase() > b.toLowerCase()) return 1;
        }
        else {
            //Parse strings as numbers to compare properly
            if (parseFloat(a) < parseFloat(b)) return -1;
            if (parseFloat(a) > parseFloat(b)) return 1;
        }

        return 0; //equal each other
    }

    transform(input: any, [config = '+']): any {

        if (!Array.isArray(config) || (Array.isArray(config) && config.length === 1)) {

            var propertyToCheck: string = !Array.isArray(config) ? config : config[0];
            var desc = propertyToCheck.substr(0, 1) === '-';


            if (!propertyToCheck || propertyToCheck === '-' || propertyToCheck === '+') {
                return !desc ? input.sort() : input.sort().reverse();
            } else {
                var property: string = propertyToCheck.substr(0, 1) === '+' || propertyToCheck.substr(0, 1) === '+'
                    ? propertyToCheck.substr(1)
                    : propertyToCheck;

                return input.sort(function(a: any, b: any) {
                    return !desc ? OrderBy.orderByComparator(a[property], b[property])
                            : -OrderBy.orderByComparator(a[property], b[property]);
                });
            }


        } else {
            return input.sort(function(a: any, b: any) {
                for (var i: number = 0; i < config.length; i ++) {
                    var desc = config[i].substr(0, 1) === '-';
                    var property = config[i].substr(0, 1) === '+' || config[i].substr(0, 1) === '-'
                        ? config[i].substr(1)
                        : config[i];

                    var comparison = !desc ? OrderBy.orderByComparator(a[property], b[property]) : -OrderBy.orderByComparator(a[property], b[property]);

                    if (comparison !== 0) return comparison;
                }
                return 0;
            });
        }
    }

}