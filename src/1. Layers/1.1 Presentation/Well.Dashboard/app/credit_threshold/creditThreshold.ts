import {Branch} from '../shared/branch/branch';

export class CreditThreshold {
    public id: number;
    public branchName: string;
    public thresholdLevel: string;
    public value: number;
    public branches: Branch[];
}  