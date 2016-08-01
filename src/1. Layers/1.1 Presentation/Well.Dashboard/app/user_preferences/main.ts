import { bootstrap }    from '@angular/platform-browser-dynamic';
import { provide } from "@angular/core";
import { IGlobalSettings } from '../shared/globalSettings';

import {UserPreferenceComponent} from './userPreferenceComponent';

export function runApplication(config: IGlobalSettings) {
    bootstrap(UserPreferenceComponent, [provide('global.settings', { useValue: config })]);
}