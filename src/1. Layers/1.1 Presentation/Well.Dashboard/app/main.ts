import { bootstrap }    from 'angular2/platform/browser';
import {provide} from "angular2/core";
import { GlobalSettings } from './shared/globalSettings';
// Our main component
import { AppComponent } from './appComponent';

export function runApplication(config: any) {

    let globalSettings = new GlobalSettings();
    globalSettings.apiUrl = config.apiUrl;

    bootstrap(AppComponent, [provide('global.settings', { useValue: globalSettings })]);
}