import { Component, ViewChild, Input}   from '@angular/core';
import {Router}                         from '@angular/router';
import {User}                           from './user';
import {UserPreferenceService}          from './userPreferenceService';
import {UserPreferenceModal}            from './userPreferenceModalComponent';
import {SecurityService}                from '../shared/services/securityService';
import {IObservableAlive}               from '../shared/IObservableAlive';
import 'rxjs/Rx';   // Load all features

@Component({
    selector: 'ow-user-preferences',
    templateUrl: './app/user_preferences/user-preferences.html'
})

export class UserPreferenceComponent implements IObservableAlive
{
    public isAlive: boolean = true;
    public userText: string;
    public users: Array<User> = [];
    public rowCount = 10;

    @Input() public header: string = 'User Preferences';
    @Input() public isThreshold: boolean;

    constructor(
        private userPreferenceService: UserPreferenceService,
        private securityService: SecurityService, 
        private router: Router)
    {
        this.securityService.validateAccess(SecurityService.adminPages);
    }

    public ngOnDestroy(): void
    {
        this.isAlive = false;
    }

    public ngOnInit(): void
    {
        this.isAlive = true;
    }

    public find(): void
    {
        this.userPreferenceService.getUsers(this.userText)
            .takeWhile(() => this.isAlive)
            .subscribe(users => this.users = users);
    }

    public userSelected(user): void 
    {
        this.router.navigate(['/user-threshold-level', user.name]);
    }
}