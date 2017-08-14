export interface IFilter
{
    getFilterType(filterName: string): (value: any, value2: any, sourceRow: any) => boolean;
}
