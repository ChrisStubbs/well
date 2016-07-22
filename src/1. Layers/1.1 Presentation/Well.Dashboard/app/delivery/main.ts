import { bootstrap }    from '@angular/platform-browser-dynamic';
import { provide } from "@angular/core";
import { IGlobalSettings } from '../shared/globalSettings';

import {DeliveryComponent} from './deliveryComponent';

export function runApplication(config: IGlobalSettings) {
    bootstrap(DeliveryComponent, [provide('global.settings', { useValue: config })]);
}