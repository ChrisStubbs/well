import {Component} from '@angular/core';
import {IAccount} from '../account/account';

@Component({
    selector: 'contact-modal',
    templateUrl: 'app/shared/contact-modal.html'
})
export class ContactModal  {
    private IsVisible: boolean = false;
    private account: IAccount;

    constructor()
    {
        this.account = new IAccount();
    }

    public show(account: IAccount) {
        this.account = account;
        this.IsVisible = true;
    }
}