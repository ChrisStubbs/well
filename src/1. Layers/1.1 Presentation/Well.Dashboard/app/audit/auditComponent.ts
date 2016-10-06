import { Component, OnInit }  from '@angular/core';
import { Audit } from './audit';
import {AuditService} from './auditService';
import {DropDownItem} from '../shared/dropDownItem';
import {FilterOption} from '../shared/filterOption';

@Component({
    templateUrl: './app/audit/audit-list.html'
})
export class AuditComponent implements OnInit {
    isLoading: boolean = true;
    audits: Audit[] = [];

    pageSize: number = 10;

    filterOptions: DropDownItem[] = [
        new DropDownItem("Invoice No", "invoiceNumber"),
        new DropDownItem("Account", "account"),
        new DropDownItem("Delivery Date", "deliveryDate", false, "date")
    ];

    filterOption: FilterOption;

    onFilterClicked(filterOption: FilterOption) {
        this.filterOption = filterOption;
    }

    constructor(private auditService: AuditService) {}

    ngOnInit(): void {
        this.auditService.getAudits()
            .subscribe(a => {
                this.audits = a;
                this.isLoading = false;
            });
    }
}

