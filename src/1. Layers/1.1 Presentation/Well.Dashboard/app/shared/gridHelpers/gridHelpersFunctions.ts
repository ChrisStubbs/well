import { IFilter } from './IFilter';
import * as _ from 'lodash';

interface IFilterValuePair {
    value1(): string;
    value2(): string;
}

class StringFilterValuePair implements IFilterValuePair {
    private val: string;
    private val2: string;

    constructor(value: string, value2: string) {
        this.val = value;
        this.val2 = value2;
    }

    public value1(): string {
        return (this.val) ? this.val.toString().toLowerCase() : '';
    }
    
    public value2(): string {
        return (this.val2) ? this.val2.toString().toLowerCase() : '';
    }  
}

export class GridHelpersFunctions
{
    public static startsWithFilter: (value: string, value2: string) => boolean = (value: string, value2: string) =>
    {
        const filterValues = new StringFilterValuePair(value, value2);
        return _.startsWith(filterValues.value2(), filterValues.value1());
    };

    public static containsFilter: (value: string, value2: string) => boolean = (value: string, value2: string) =>
    {       
        const filterValues = new StringFilterValuePair(value, value2);
        return filterValues.value1().indexOf(filterValues.value2()) != -1;
    };

    public static isEqualFilter: (value: any, value2: any) => boolean = (value: any, value2: any) =>
    {
        return _.isEqual(value, value2);
    };

    public static boolFilter: (value: boolean, value2: any) => boolean = (value: boolean, value2: any) =>
    {
        return _.isEqual(value, value2.toString() == 'true');
    };

    public static resolutionFilter(value: number, value2: number): boolean {
        return (value & value2) == value;
    }

    public static filterFreeText(inputFilterTimer: any): Promise<any>
    {
        return new Promise((resolve, reject) =>
        {

            if (inputFilterTimer)
            {
                clearTimeout(inputFilterTimer);
                reject();
            }
            else
            {
                inputFilterTimer = setTimeout(() =>
                {
                    resolve();
                    inputFilterTimer = undefined;
                }, 300);
            }
        });
    }

    public static applyGridFilter<list, filter extends IFilter>(values: Array<list>, filterObject: filter): Array<list>
    {
        const columnsToFilter = [];

        //get the values only from properties that are not:
        //null,
        //undefined
        //empty string
        _.map(_.keys(filterObject), (current: string) =>
        {
            const value = filterObject[current];

            if (!(_.isNil(value) || value == ''))
            {
                columnsToFilter.push(current);
            }
        });

        return _.filter(values, current =>
        {
            let result: boolean = true;

            _.map(columnsToFilter, (c: string) =>
            {
                if (result)
                {
                    //if we detect a false no point on keep testing the other columns
                    const value = current[c];
                    const value2 = filterObject[c];
                    const func = filterObject.getFilterType(c);

                    result = func(value, value2);
                }
            });

            return result;
        });
    }
}