import { PipeTransform, Pipe } from '@angular/core';

import {Delivery} from "./delivery";

@Pipe({
    name: 'exceptionsFilter'
})
export class ExceptionsFilterPipe implements PipeTransform {
    transform(deliveries: Delivery[], showAll: boolean): Delivery[] {
        if (deliveries == null) return null;
        if (showAll) return deliveries;
        return deliveries.filter(delivery =>delivery.isException);

    }
}

