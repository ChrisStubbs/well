export interface IFilter
{
    getFilterType(filterName: string): (value: any, value2: any) => boolean;
}
