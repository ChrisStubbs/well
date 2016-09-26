import {Branch} from '../shared/branch/branch';

export class CreditThreshold {
    id: number;
    branchName: string;
    thresholdLevel: string;
    threshold: number;
    branches: Branch[]
}