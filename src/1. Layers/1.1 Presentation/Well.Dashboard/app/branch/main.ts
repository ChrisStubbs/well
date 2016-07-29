import { bootstrap }    from '@angular/platform-browser-dynamic';
import { provide } from "@angular/core";
import { IGlobalSettings } from '../shared/globalSettings';
import {GlobalSettingsService} from '../shared/globalSettings';
import {BranchSelectionComponent} from './branchSelectionComponent';

export function runApplication(config: IGlobalSettings) {
    bootstrap(BranchSelectionComponent, [GlobalSettingsService, provide('global.settings', { useValue: config })]);
}