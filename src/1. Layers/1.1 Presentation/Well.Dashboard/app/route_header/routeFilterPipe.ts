import { PipeTransform, Pipe } from '@angular/core';
import { IRoute } from './route';

@Pipe({
    name: 'routeFilter'
})
export class RouteFilterPipe implements PipeTransform {
    transform(value: IRoute[], args: string[]): IRoute[] {
        let filter: string = args[0] ? args[0].toLocaleLowerCase() : null;
        return filter
            ? value.filter((route: IRoute) =>
                route.driverName.toLocaleLowerCase().indexOf(filter) !== -1)
            : value;
    }
}

