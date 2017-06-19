import {OnDestroy, OnInit} from '@angular/core';

export interface IObservableAlive extends OnDestroy, OnInit
{
    isAlive: boolean;
}
