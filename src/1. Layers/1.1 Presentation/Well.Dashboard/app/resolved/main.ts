import { bootstrap }    from '@angular/platform-browser-dynamic';
import { provide } from "@angular/core";
import { IGlobalSettings } from '../shared/globalSettings';
import {ResolvedDeliveryComponent} from './resolved-deliveryComponent';

export function runApplication(config: IGlobalSettings) {
    bootstrap(ResolvedDeliveryComponent, [provide('global.settings', { useValue: config })]);
}