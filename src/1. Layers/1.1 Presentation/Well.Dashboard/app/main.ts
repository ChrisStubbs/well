import { bootstrap }    from 'angular2/platform/browser';
import {provide} from "angular2/core";
import { IGlobalSettings } from './shared/globalSettings';
// Our main component
import { AppComponent } from './appComponent';

export function runApplication(config: IGlobalSettings) {
    bootstrap(AppComponent, [provide('global.settings', { useValue: config })]);
}