import { bootstrap }    from '@angular/platform-browser-dynamic';
import { provide } from "@angular/core";
import { IGlobalSettings } from '../shared/globalSettings';

import {NotificationsComponent} from './notificationsComponent';

export function runApplication(config: IGlobalSettings) {
    bootstrap(NotificationsComponent, [provide('global.settings', { useValue: config })]);
}