import { Component, OnInit, ViewChild } from '@angular/core';
import {PendingCredit} from './pendingCredit';
import { PendingCreditService } from './pendingCreditService';
import { AccountService } from "../account/accountService";
import { ContactModal } from '../shared/contactModal';
import { PendingCreditDetailModal } from './pendingCreditDetailModal';
import {PendingCreditConfirmationModal} from './pendingCreditConfirmationModal';

@Component({
    selector: 'ow-pending-credit',
    templateUrl: './app/pending_credit/pending-credit.html'
})
export class PendingCreditComponent implements OnInit{
    rowCount: number = 10;
    pendingCredits: PendingCredit[];
    @ViewChild(ContactModal)
    private contactModal: ContactModal;

    @ViewChild(PendingCreditDetailModal)
    private detailModal: PendingCreditDetailModal;

    @ViewChild(PendingCreditConfirmationModal)
    private confirmModal: PendingCreditConfirmationModal;

    constructor(private pendingCreditService: PendingCreditService,
                private accountService: AccountService) { }

    ngOnInit(): void {
        this.pendingCreditService.getPendingCredits().subscribe(x => this.pendingCredits = x);
    }

    openContactModal(accountId): void {
        this.accountService.getAccountByAccountId(accountId)
            .subscribe(account => {
                this.contactModal.show(account);
            });
    }

    setAction(pendingCredit: PendingCredit, action: string) {
        pendingCredit.action = action;
        this.confirmModal.show();
    }

    viewDetail(pendingCredit: PendingCredit) {
        this.detailModal.show(pendingCredit);
    }
}