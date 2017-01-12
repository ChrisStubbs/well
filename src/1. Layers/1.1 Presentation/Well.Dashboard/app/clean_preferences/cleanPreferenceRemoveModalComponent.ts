﻿import {Component, EventEmitter, Output} from '@angular/core';
import {Response} from '@angular/http';
import {ToasterService} from 'angular2-toaster/angular2-toaster';
import {HttpResponse} from '../shared/httpResponse';
import {CleanPreferenceService} from './cleanPreferenceService';
import {CleanPreference} from './cleanPreference';

@Component({
    selector: 'ow-clean-preference-remove-modal',
    templateUrl: './app/clean_preferences/clean-preference-remove-modal.html'
})
export class CleanPreferenceRemoveModalComponent {
    public isVisible: boolean = false;
    public cleanPreference: CleanPreference;
    public httpResponse: HttpResponse = new HttpResponse();
    @Output() public onCleanPreferenceRemoved = new EventEmitter<CleanPreference>();

    constructor(private cleanPreferenceService: CleanPreferenceService, private toasterService: ToasterService) { }

    public show(cleanPreference: CleanPreference) {
        this.cleanPreference = cleanPreference;
        this.isVisible = true;
    }

    public hide() {
        this.isVisible = false;
    }

    public yes() {
        this.cleanPreferenceService.removeCleanPreference(this.cleanPreference.id)
            .subscribe((res: Response) => {
                this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success) {
                    this.toasterService.pop('success', 'Clean preference has been removed!', '');
                }
                if (this.httpResponse.failure) {
                    this.toasterService.pop(
                        'error',
                        'Clean preference could not be deleted at this time!',
                        'Please try again later!');
                }
                this.isVisible = false;

                this.onCleanPreferenceRemoved.emit(this.cleanPreference);
            });
    }
}