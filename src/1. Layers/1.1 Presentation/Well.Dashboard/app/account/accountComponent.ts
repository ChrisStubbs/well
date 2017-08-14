import { Component, OnInit}  from '@angular/core';
import 'rxjs/Rx';   // Load all features

import { IAccount } from './account';
import { AccountService } from './accountService';

@Component({
    selector: 'ow-contact-details',
    template: `<button type="button" class="btn btn-default"(click) = "openModal()">Contact</button>`,
})

export class AccountComponent implements OnInit {
    public errorMessage: string;
    public account: IAccount;
    public accountId: number;

    constructor(private accountService: AccountService) {}

    public ngOnInit(): void {
        this.accountService.getAccountByAccountId(this.accountId)
            .subscribe(account => this.account = account,
                error => this.errorMessage = <any>error);
    }
}