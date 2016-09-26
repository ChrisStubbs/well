import {Branch} from '../shared/branch/branch';

export class CreditThreshold {
    id: number;
    branchName: string;
    role: string;
    threshold: number;
    branches: Branch[]
}