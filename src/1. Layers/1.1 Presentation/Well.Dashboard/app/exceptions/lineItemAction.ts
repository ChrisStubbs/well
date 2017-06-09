﻿export class LineItemAction
{
    public id: number = 0;
    public lineItemId: number = 0;
    public deliveryAction: number = 0;
    public exceptionType: number = 0;
    public quantity: number;
    public source: number = 0;
    public reason: number = 0;
    public originator: number = 1; // default to Customer
   
}