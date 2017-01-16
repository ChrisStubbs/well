import {Component, ViewChild, EventEmitter, Output} from '@angular/core';
import {Response} from '@angular/http';
import {CleanPreference} from './cleanPreference';
import {ToasterService} from 'angular2-toaster/angular2-toaster';
import {BranchCheckboxComponent} from '../shared/branch/branchCheckboxComponent';
import {CleanPreferenceService} from './cleanPreferenceService';
import {HttpResponse} from '../shared/httpResponse';

@Component({
    selector: 'ow-clean-preference-edit-modal',
    templateUrl: './app/clean_preferences/clean-preference-edit-modal.html'
})
export class CleanPreferenceEditModalComponent {
    public isVisible: boolean = false;
    public cleanPreference: CleanPreference;
    public httpResponse: HttpResponse = new HttpResponse();
    public errors: string[];
    @Output() public onCleanPreferenceUpdate = new EventEmitter<CleanPreference>();

    constructor(private cleanPreferenceService: CleanPreferenceService, private toasterService: ToasterService) { }

    @ViewChild(BranchCheckboxComponent) public branch: BranchCheckboxComponent;
    
    public show(cleanPreference: CleanPreference) {
        this.clear();
        this.cleanPreference = cleanPreference;
        this.isVisible = true;
    }

    public hide() {
        this.isVisible = false;
        this.clear();
    }

    public clear() {
        this.cleanPreference = new CleanPreference();
        this.errors = [];
    }

    public update() {
        this.cleanPreferenceService.saveCleanPreference(this.cleanPreference, true)
            .subscribe((res: Response) => {
                this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success) {
                    this.toasterService.pop('success', 'Clean preference has been updated!', '');
                    this.isVisible = false;
                    this.onCleanPreferenceUpdate.emit(this.cleanPreference);
                }
                if (this.httpResponse.failure) {
                    this.toasterService.pop(
                        'error',
                        'Clean preference could not be updated at this time!',
                        'Please try again later!');
                    this.isVisible = false;
                }
                if (this.httpResponse.notAcceptable) {
                    this.errors = this.httpResponse.errors;
                }
            });
    }
}