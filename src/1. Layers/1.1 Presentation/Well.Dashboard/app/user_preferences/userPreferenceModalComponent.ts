import {Component} from '@angular/core';
import {User} from './user';

@Component({
    selector: 'ow-user-preference-modal',
    templateUrl: './app/user_preferences/user-preference-modal.html'
})
export class UserPreferenceModal {
    isVisible = false;
    user: User;

    show(user) {
        this.user = user;
        this.isVisible = true;
    }

    hide() {
        this.isVisible = false;
    }

    setBranches() {
        window.location.href = encodeURI('./user-preferences/branches/' +  this.user.friendlyName + '/' + this.user.domain);
    }
}