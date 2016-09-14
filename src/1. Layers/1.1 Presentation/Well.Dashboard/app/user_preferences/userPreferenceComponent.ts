import {Router} from '@angular/router';
import { Component, OnInit, ViewChild}  from '@angular/core';
import { Response } from '@angular/http';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/Rx';   // Load all features
import {User} from './user';
import {UserPreferenceService} from './userPreferenceService';
import {UserPreferenceModal} from './userPreferenceModalComponent';
import {SecurityService} from '../shared/security/securityService';
import {UnauthorisedComponent} from '../unauthorised/unauthorisedComponent';

@Component({
    selector: 'ow-user-preferences',
    templateUrl: './app/user_preferences/user-preferences.html',
    providers: [UserPreferenceService]
}
)
export class UserPreferenceComponent {
    userText: string;
    users: Array<User> = [];
    rowCount = 10;

    constructor(private globalSettingsService: GlobalSettingsService,
        private userPreferenceService: UserPreferenceService,
        private router: Router,
        private securityService: SecurityService) {
        this.securityService.validateUser(this.globalSettingsService.globalSettings.permissions, this.securityService.userBranchPreferences);
    }

    @ViewChild(UserPreferenceModal) modal : UserPreferenceModal;

    find(): void {
        this.userPreferenceService.getUsers(this.userText)
            .subscribe(users => this.users = users);
    }

    userSelected(user): void {
        this.modal.show(user);
    }
}