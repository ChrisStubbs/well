import { ISort }                from './IOptionFilter'
import * as _                   from 'lodash';

export class OrderByExecutor
{
    public Order(value: any[], args: ISort): any[]
    {
        return _.orderBy(value, [args.sortField], [args.sortDirection]);
    }
}