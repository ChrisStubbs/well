import { bootstrap }    from '@angular/platform-browser-dynamic';
import { provide } from "@angular/core";
import { IGlobalSettings } from '../shared/globalSettings';

import {AccountComponent} from './accountComponent';

export function runApplication(config: IGlobalSettings) {
    bootstrap(AccountComponent, [provide('global.settings', { useValue: config })]);
}