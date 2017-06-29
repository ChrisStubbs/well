import {Component, ViewChild, Input} from '@angular/core';
import {Delivery}                   from './model/delivery';
import {DeliveryLine}               from './model/deliveryLine';
import {Damage}                     from './model/damage';
import {JobDetailReason}            from './model/jobDetailReason';
import {JobDetailSource}            from './model/jobDetailSource';
import {ConfirmModal}               from '../shared/confirmModal';
import {DeliveryService}            from './deliveryService';
import {Router}                     from '@angular/router';
import { ToasterService }           from 'angular2-toaster/angular2-toaster';
import { Action }                   from './model/action';
import * as lodash                  from 'lodash';
import { GlobalSettingsService }    from '../shared/globalSettings';
import { SecurityService }          from '../shared/security/securityService';

@Component({
    templateUrl: './app/delivery/delivery-issues.html',
    selector: 'ow-delivery-issues'
})
export class DeliveryIssuesComponent {
    @Input() public delivery: Delivery;
    @Input() public deliveryLine: DeliveryLine;

    public reasons: JobDetailReason[] = new Array<JobDetailReason>();
    public sources: JobDetailSource[] = new Array<JobDetailSource>();
    public actions: Action[] = new Array<Action>();
    public confirmMessage: string;
    public confirmModalIsVisible: boolean = false;
    @ViewChild(ConfirmModal) private confirmModal: ConfirmModal;

    constructor(
        private deliveryService: DeliveryService,
        private toasterService: ToasterService,
        private globalSettingsService: GlobalSettingsService,
        private securityService: SecurityService,
        private router: Router) {
    }

    public ngOnInit(): void {
        this.deliveryService.getDamageReasons()
            .subscribe(r => { this.reasons = r; });

        this.deliveryService.getSources()
            .subscribe(s => this.sources = s);

        this.deliveryService.getActions()
            .subscribe(actions => {
                this.actions = actions;
                if (this.delivery.isProofOfDelivery) {
                    lodash.remove(this.actions, a => { return a.id === 1 || a.id === 2; });
                }
            });
    }

    public addDamage() {
        const index = this.deliveryLine.damages.length;
        this.deliveryLine.damages.push(new Damage(index, 0, 0, 0, 0));
    }

    public removeDamage(index) {
        lodash.remove(this.deliveryLine.damages, { index: index });
    }

    public update() {
        if (this.delivery.isCleanOnInit() && this.delivery.isClean() === false) {
            //Changing a Clean to an Exception
            this.confirmModal.isVisible = true;
            this.confirmModal.heading = 'Make delivery dirty?';
            this.confirmModal.messageHtml =
                '<p>You have added shorts and/or damages, this will make the delivery dirty. ' +
                'Are you sure you want to save your changes?</p>';
            return;
        }
        if (this.delivery.isCleanOnInit() === false && this.delivery.isClean()) {
            ///Changing an Exception to a clean
            this.confirmModal.isVisible = true;
            this.confirmModal.heading = 'Resolve delivery?';
            this.confirmModal.messageHtml =
                '<p>You have removed all shorts and/or damages, this will resolve the delivery. ' +
                'Are you sure you want to save your changes?</p>';
            return;
        }

        this.updateConfirmed();
    }
     
    public updateConfirmed() {
        this.deliveryService.updateDeliveryLine(this.deliveryLine)
            .subscribe(() => {
                this.toasterService.pop('success', 'Delivery line issues updated', '');
                this.router.navigate(['/delivery', this.delivery.id]);
            });
    }

    public cancel() {
        this.router.navigate(['/delivery', this.delivery.id]);
    }

    public IsDisabled(): boolean {

        const hasPermission = this.securityService.hasPermission(
             this.globalSettingsService.globalSettings.permissions, 
             this.securityService.actionDeliveries);

        return !(this.delivery.canAction && hasPermission);
    }
} 