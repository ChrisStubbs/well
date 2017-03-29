import {Component, ViewChild, EventEmitter, Output} from '@angular/core';
import {Response} from '@angular/http';
import {CleanPreference} from './cleanPreference';
import {ToasterService} from 'angular2-toaster/angular2-toaster';
import {BranchCheckboxComponent} from '../shared/branch/branchCheckboxComponent';
import {CleanPreferenceService} from './cleanPreferenceService';
import {HttpResponse} from '../shared/httpResponse';

@Component({
    selector: 'ow-clean-preference-add-modal',
    templateUrl: './app/clean_preferences/clean-preferences-add-modal.html'
})
export class CleanPreferenceAddModalComponent {
    public isVisible: boolean = false;
    public cleanPreference: CleanPreference = new CleanPreference();
    public httpResponse: HttpResponse = new HttpResponse();
    public errors: string[];
    @Output() public onCleanPreferenceSave = new EventEmitter<CleanPreference>();

    constructor(private cleanPreferenceService: CleanPreferenceService, private toasterService: ToasterService) { }

    @ViewChild(BranchCheckboxComponent) public branch: BranchCheckboxComponent;

    public show() {
        this.clear();
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

    public save() {
        this.cleanPreference.branches = this.branch.selectedBranches;

        this.cleanPreferenceService.saveCleanPreference(this.cleanPreference, false)
            .subscribe((res: Response) => {
                this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success) {
                    this.toasterService.pop('success', 'Clean preference has been saved', '');
                    this.isVisible = false;
                    this.clear();
                    this.onCleanPreferenceSave.emit(this.cleanPreference);
                }
                if (this.httpResponse.failure) {
                    this.toasterService.pop(
                        'error',
                        'Clean preference could not be saved at this time',
                        'Please try again later!');
                    this.isVisible = false;
                }
                if (this.httpResponse.notAcceptable) {
                    this.errors = this.httpResponse.errors;
                }
            });
    }
}