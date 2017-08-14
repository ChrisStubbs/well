import {Router} from '@angular/router';
import {Component} from '@angular/core';
import {User} from './user';

@Component({
    selector: 'ow-user-preference-modal',
    templateUrl: './app/user_preferences/user-preference-modal.html'
})
export class UserPreferenceModal {
    public isVisible = false;
    public user: User;
    public isThreshold: boolean;

    constructor(private router: Router) {}

    public show(user, isThreshold) {
        this.user = user;
        this.isVisible = true;
        this.isThreshold = isThreshold;
    }

    public hide() {
        this.isVisible = false;
    }

    public setBranches(user) {
        this.router.navigate(['/branch', user.name, user.domain]);
    }

    public setThresholdLevels(user) {
        this.router.navigate(['/user-threshold-level', user.name]);
    }
}