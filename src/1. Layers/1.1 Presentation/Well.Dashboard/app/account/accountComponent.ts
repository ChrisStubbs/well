import { Component, OnInit}  from '@angular/core';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/Rx';   // Load all features

import { IAccount } from './account';
import { AccountService } from './accountService';

@Component({
    selector: 'ow-contact-details',
    template: `<button type="button" class="btn btn-default"(click) = "openModal()">Contact</button>`,
})

export class AccountComponent implements OnInit {
    errorMessage: string;
    account: IAccount;
    accountId: number;

    constructor(private accountService: AccountService) {}

    ngOnInit(): void {
        this.accountService.getAccountByAccountId(this.accountId)
            .subscribe(account => this.account = account,
                error => this.errorMessage = <any>error);
    }
}