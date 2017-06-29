import { PipeTransform, Pipe }  from '@angular/core';
import { ISort }                from './IOptionFilter';
import { OrderByExecutor }      from './OrderByExecutor';

@Pipe({ name: 'orderBy' })
export class OrderByPipe implements PipeTransform {

    private orderBy: OrderByExecutor;
    constructor()
    {
        this.orderBy = new OrderByExecutor();
    }

    public transform(value: any[], args: ISort): any[]
    {
        let result: any[];

        result = this.orderBy.Order(value, args);
        return result;
    }
}