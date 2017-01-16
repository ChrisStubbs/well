import {Branch} from '../shared/branch/branch';

export class CreditThreshold {
    public id: number;
    public branchName: string;
    public thresholdLevel: string;
    public threshold: number;
    public branches: Branch[];
}