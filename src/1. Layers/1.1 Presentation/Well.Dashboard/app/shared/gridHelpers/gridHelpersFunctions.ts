import { IFilter }  from './IFilter'
import * as _       from 'lodash'

export class GridHelpersFunctions
{
    public static startsWithFilter: (value: string, value2: string) => boolean = (value: string, value2: string) =>
    {
        return _.startsWith(value.toLowerCase(), value2.toLowerCase());
    };

    public static isEqualFilter: (value: any, value2: any) => boolean = (value: any, value2: any) =>
    {
        return _.isEqual(value.toLowerCase(), value2.toLowerCase());
    };

    public static boolFilter: (value: boolean, value2: any) => boolean = (value: boolean, value2: any) =>
    {
        return _.isEqual(value, value2.toString() == 'true');
    };

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

            if (!(_.isNil(value) || value === ''))
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