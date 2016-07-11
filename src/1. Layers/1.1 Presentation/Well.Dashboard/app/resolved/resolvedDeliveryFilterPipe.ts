import { PipeTransform, Pipe } from 'angular2/core';
import { IResolvedDelivery } from './resolvedDelivery';

@Pipe({
    name: 'resolvedDeliveryFilter'
})
export class ResolvedDeliveryFilterPipe implements PipeTransform {
    transform(value: IResolvedDelivery[], args: string[]): IResolvedDelivery[] {
        let filter: string = args[0] ? args[0].toLocaleLowerCase() : null;
        return filter
            ? value.filter((delivery: IResolvedDelivery) =>
                delivery.accountName.toLocaleLowerCase().indexOf(filter) !== -1)
            : value;
    }
}

