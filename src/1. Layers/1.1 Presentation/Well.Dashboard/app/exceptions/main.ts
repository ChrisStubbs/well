import { bootstrap }    from '@angular/platform-browser-dynamic';
import { provide } from "@angular/core";
import { IGlobalSettings } from '../shared/globalSettings';

import {ExceptionsComponent} from './exceptionsComponent';

export function runApplication(config: IGlobalSettings) {
    bootstrap(ExceptionsComponent, [provide('global.settings', { useValue: config })]);
}