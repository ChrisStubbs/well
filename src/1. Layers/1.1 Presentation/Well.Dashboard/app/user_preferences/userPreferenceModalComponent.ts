import {Router} from '@angular/router';
import {Component} from '@angular/core';
import {User} from './user';

@Component({
    selector: 'ow-user-preference-modal',
    templateUrl: './app/user_preferences/user-preference-modal.html'
})
export class UserPreferenceModal {
    isVisible = false;
    user: User;

    constructor(private router: Router) {}

    show(user) {
        this.user = user;
        this.isVisible = true;
    }

    hide() {
        this.isVisible = false;
    }

    setBranches(user) {
        this.router.navigate(['/branch', user.name, user.domain]);
    }
}