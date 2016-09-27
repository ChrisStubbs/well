import {Component, OnInit} from '@angular/core';
import {CleanPreferenceService} from './cleanPreferenceService';
import {CleanPreference} from './cleanPreference';

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

    loadCleanPreferences(): void {
        this.cleanPreferenceService.getCleanPreference().subscribe(x => { this.cleanPreferences = x; console.log(this.cleanPreferences); });
    }
}