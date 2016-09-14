import { Component, OnInit }  from '@angular/core';
import { Audit } from './audit';
import {DropDownItem} from '../shared/dropDownItem';
import {FilterOption} from '../shared/filterOption';

@Component({
    templateUrl: './app/audit/audit-list.html'
})

export class AuditComponent implements OnInit {

    private audits: Audit[] = [
        {
            entry: "Credit",
            type: "Action",
            invoiceNumber: "92874.033",
            accountCode: "2874.033",
            accountName: "CSG Blah",
            deliveryDate: new Date(),
            auditDate: new Date(),
            auditBy: "Michael Hook"
        },
        {
            entry: "Credit and Re-Order",
            type: "Action",
            invoiceNumber: "92874.033",
            accountCode: "2874.033",
            accountName: "CSG Blah",
            deliveryDate: new Date(),
            auditDate: new Date(),
            auditBy: "Michael Hook"
        },
        {
            entry: "Short Qty changed from 0 to 2, Damages changed from 1 - CAR01, 2 - CAR02 to 2 - CAR01 for Product 50035 - Ind Potato Gratin 400g",
            type: "Delivery line update",
            invoiceNumber: "12345.033",
            accountCode: "6666.033",
            accountName: "ABC Blah",
            deliveryDate: new Date(),
            auditDate: new Date(),
            auditBy: "Michael Hook"
        }
    ];

    pageSize: number = 10;

    filterOptions: DropDownItem[] = [
        new DropDownItem("Invoice No", "invoiceNumber"),
        new DropDownItem("Account Code", "accountCode"),
        new DropDownItem("Account Name", "accountName"),
        new DropDownItem("Delivery Date", "deliveryDate", false, "date")
    ];

    filterOption: FilterOption;

    onFilterClicked(filterOption: FilterOption) {
        this.filterOption = filterOption;
    }

    constructor() {}

    ngOnInit(): void {

    }
}