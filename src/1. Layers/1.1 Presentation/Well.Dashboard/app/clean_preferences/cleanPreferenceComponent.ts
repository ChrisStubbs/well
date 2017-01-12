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
export class CleanPreferenceComponent implements OnInit {
    public cleanPreferences: CleanPreference[];

    constructor(private cleanPreferenceService: CleanPreferenceService) {}

    public ngOnInit(): void {
        this.loadCleanPreferences();
    }

    @ViewChild(CleanPreferenceAddModalComponent) public addModal: CleanPreferenceAddModalComponent;
    @ViewChild(CleanPreferenceRemoveModalComponent) public removeModal: CleanPreferenceRemoveModalComponent;
    @ViewChild(CleanPreferenceEditModalComponent) public editModal: CleanPreferenceEditModalComponent;

    public loadCleanPreferences(): void {
        this.cleanPreferenceService.getCleanPreference().subscribe(x => this.cleanPreferences = x);
    }

    public selectCleanPreference(clean: CleanPreference) {
        this.editModal.show(clean);
    }

    public add() {
        this.addModal.show();
    }

    public remove(clean: CleanPreference): void {
        this.removeModal.show(clean);
    }

    public onRemoved(cleanPreference: CleanPreference) {
        lodash.remove(this.cleanPreferences, cleanPreference);
    }
}