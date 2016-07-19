import { bootstrap }    from '@angular/platform-browser-dynamic';
import { provide } from "@angular/core";
import { IGlobalSettings } from '../shared/globalSettings';

import {RouteHeaderComponent} from './routeHeaderComponent';

export function runApplication(config: IGlobalSettings) {
    bootstrap(RouteHeaderComponent, [provide('global.settings', { useValue: config })]);
}