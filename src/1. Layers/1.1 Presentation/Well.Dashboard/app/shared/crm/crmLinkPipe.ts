import { Pipe, PipeTransform } from '@angular/core';
import {GlobalSettingsService} from '../globalSettings';

@Pipe({name: 'crmLink'})
export class CrmLinkPipe implements PipeTransform {

    constructor(private globalSettingsService: GlobalSettingsService) {
        
    }

    public transform(value: IAccountReference) {
        const urlBase = this.globalSettingsService.globalSettings.crmBaseUrl;
        return encodeURI(urlBase +
            'CallLog/History?branchId=' +
            value.branchId +
            '&accountNumber=' +
            value.accountNumber);
    }
}

export class AccountReference implements IAccountReference {
    public accountNumber: string;
    public branchId: number;

    constructor(accountNumber: string, branchId: number) {
        this.accountNumber = accountNumber;
        this.branchId = branchId;
    }
}

export interface IAccountReference {
    accountNumber: string;
    branchId: number;
}