import { Component }    from '@angular/core';
import { JobService }   from './jobService';
import 'rxjs/Rx';   // Load all features

@Component({
    selector: 'ow-job'
    , templateUrl: './app/job/job.html'
    , providers: [JobService]

})
export class JobComponent
{
    public test: string;
    constructor()
    {
        this.test = 'dddddddddddddddddddd';
    }
}