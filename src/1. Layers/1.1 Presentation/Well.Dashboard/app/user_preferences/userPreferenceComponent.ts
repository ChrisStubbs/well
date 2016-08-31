import {Router} from '@angular/router';
import { Component, OnInit, ViewChild}  from '@angular/core';
import { HTTP_PROVIDERS, Response } from '@angular/http';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/Rx';   // Load all features
import {User} from './user';
import {UserPreferenceService} from './userPreferenceService';
import {PaginationService } from 'ng2-pagination';
import {UserPreferenceModal} from './userPreferenceModalComponent';

@Component({
    selector: 'ow-user-preferences',
    templateUrl: './app/user_preferences/user-preferences.html',
    providers: [HTTP_PROVIDERS, UserPreferenceService, GlobalSettingsService, PaginationService],
    directives: [UserPreferenceModal]
}
)
export class UserPreferenceComponent {
    userText: string;
    users: Array<User> = [];
    rowCount = 10;

    constructor(private userPreferenceService: UserPreferenceService, private router: Router) {
    }

    @ViewChild(UserPreferenceModal) modal = new UserPreferenceModal(this.router);

    find(): void {
        this.userPreferenceService.getUsers(this.userText)
            .subscribe(users => this.users = users);
    }

    userSelected(user): void {
        this.modal.show(user);
    }
}