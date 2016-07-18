import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { IAccount } from './account';
import { AccountService } from './accountService';

@Component({
    selector:'contact-details',
    //template: `<button type="button" class="btn btn-default"(click) = "openModal(delivery.accountId)">lee</button>`,
    template: `<button type="button" class="btn btn-default"(click) = "openModal()">Contact</button>`,
    providers: [AccountService]
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

    openModal(accountId: number): void {
        console.log(accountId);
    }


}