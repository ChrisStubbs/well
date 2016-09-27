import {Component, OnInit, ViewChild} from '@angular/core';
import {CleanPreferenceService} from './cleanPreferenceService';
import {CleanPreference} from './cleanPreference';
import {CleanPreferenceAddModalComponent} from './cleanPreferenceAddModalComponent';
import {CleanPreferenceRemoveModalComponent} from './cleanPreferenceRemoveModalComponent';
import {CleanPreferenceEditModalComponent} from './cleanPreferenceEditModalComponent';
import * as lodash from 'lodash';

@Component({
        selector: 'ow-clean-preferences',
        templateUrl: './app/clean_preferences/clean-preferences-view.html'
    }
)
export class CleanPreferenceComponent implements OnInit{
    cleanPreferences: CleanPreference[];

    constructor(private cleanPreferenceService: CleanPreferenceService) {}

    ngOnInit(): void {
        this.loadCleanPreferences();
    }

    @ViewChild(CleanPreferenceAddModalComponent) addModal: CleanPreferenceAddModalComponent;
    @ViewChild(CleanPreferenceRemoveModalComponent) removeModal: CleanPreferenceRemoveModalComponent;
    @ViewChild(CleanPreferenceEditModalComponent) editModal: CleanPreferenceEditModalComponent;

    loadCleanPreferences(): void {
        this.cleanPreferenceService.getCleanPreference().subscribe(x => this.cleanPreferences = x);
    }

    selectCleanPreference(clean: CleanPreference) {
        this.editModal.show(clean);
    }

    add() {
        this.addModal.show();
    }

    remove(clean: CleanPreference): void {
        this.removeModal.show(clean);
    }

    onRemoved(cleanPreference: CleanPreference) {
        lodash.remove(this.cleanPreferences, cleanPreference);
    }
}