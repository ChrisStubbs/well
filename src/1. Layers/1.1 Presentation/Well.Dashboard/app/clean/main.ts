import { bootstrap }    from '@angular/platform-browser-dynamic';
import { provide } from "@angular/core";
import { IGlobalSettings } from '../shared/globalSettings';

import {CleanDeliveryComponent} from './cleanDeliveryComponent';

export function runApplication(config: IGlobalSettings) {
    bootstrap(CleanDeliveryComponent, [provide('global.settings', { useValue: config })]);
}