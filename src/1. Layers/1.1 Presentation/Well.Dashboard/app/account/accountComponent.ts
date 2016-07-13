import { Component, OnInit } from 'angular2/core';

import { IAccount } from './account';
import { AccountService } from './accountService';


@Component({

    templateUrl: './app/account/accountModal.html',
    providers: [AccountService]

})


export class AccountComponent implements OnInit {
    errorMessage: string;
    account: IAccount;

    constructor(private accountService: AccountService) { }

    ngOnInit(): void {
        this.accountService.getAccountByStopId()
            .subscribe(account => this.account = account,
                error => this.errorMessage = <any>error);
    }


}