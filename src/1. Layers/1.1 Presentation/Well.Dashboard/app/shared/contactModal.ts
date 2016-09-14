import {Component} from '@angular/core';
import {IAccount} from "../account/account";

@Component({
    selector: 'contact-modal',
    templateUrl: 'app/shared/contact-modal.html'
})
export class ContactModal  {
    private IsVisible: boolean = false;
    private account: IAccount;
    
    show(account: IAccount) {
        this.account = account;
        this.IsVisible = true;
    }

    hide() {
        this.IsVisible = false;
    }
}