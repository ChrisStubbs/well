import { bootstrap }    from '@angular/platform-browser-dynamic';
import { provide } from "@angular/core";
import { IGlobalSettings } from '../shared/globalSettings';

import {WidgetStatsComponent} from './widgetStatsComponent';

export function runApplication(config: IGlobalSettings) {
    bootstrap(WidgetStatsComponent, [provide('global.settings', { useValue: config })]);
}