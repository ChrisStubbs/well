import { bootstrap }    from '@angular/platform-browser-dynamic';
import { provide } from "@angular/core";
import { IGlobalSettings } from './shared/globalSettings';
import { MODAL_BROWSER_PROVIDERS } from 'angular2-modal/platform-browser/index';
// Our main component
import { AppComponent } from './appComponent';

export function runApplication(config: IGlobalSettings) {
    bootstrap(AppComponent, [provide('global.settings', { useValue: config }), MODAL_BROWSER_PROVIDERS]);
}