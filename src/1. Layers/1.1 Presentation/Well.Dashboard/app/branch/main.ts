import { bootstrap }    from '@angular/platform-browser-dynamic';
import { provide } from "@angular/core";
import { IGlobalSettings } from '../shared/globalSettings';

import {BranchSelectionComponent} from './branchSelectionComponent';

export function runApplication(config: IGlobalSettings) {
    bootstrap(BranchSelectionComponent, [provide('global.settings', { useValue: config })]);
}