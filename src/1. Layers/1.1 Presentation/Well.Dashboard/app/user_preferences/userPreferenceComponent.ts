import { Component, ViewChild, Input}  from '@angular/core';
import 'rxjs/Rx';   // Load all features
import {User} from './user';
import {UserPreferenceService} from './userPreferenceService';
import {UserPreferenceModal} from './userPreferenceModalComponent';
import {SecurityService} from '../shared/services/securityService';
import {IObservableAlive} from '../shared/IObservableAlive';

@Component({
    selector: 'ow-user-preferences',
    templateUrl: './app/user_preferences/user-preferences.html'
})

export class UserPreferenceComponent implements IObservableAlive
{
    public isAlive: boolean = true;

    public ngOnDestroy(): void
    {
        this.isAlive = false;
    }

    public ngOnInit(): void
    {
        this.isAlive = true;
    }

    public userText: string;
    public users: Array<User> = [];
    public rowCount = 10;
    @Input() public header: string = 'User Preferences';
    @Input() public isThreshold: boolean;

    constructor(
        private userPreferenceService: UserPreferenceService,
        private securityService: SecurityService)
    {
        this.securityService.validateAccess(SecurityService.adminPages);
    }

    @ViewChild(UserPreferenceModal) public modal: UserPreferenceModal;

    public find(): void
    {
        this.userPreferenceService.getUsers(this.userText)
            .takeWhile(() => this.isAlive)
            .subscribe(users => this.users = users);
    }

    public userSelected(user): void {
        this.modal.show(user, this.isThreshold);
    }
}