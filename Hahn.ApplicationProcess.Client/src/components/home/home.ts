import { ApplicantService } from 'resources/services/applicant-service';
import { autoinject } from 'aurelia-framework';

@autoinject
export class Home {
    applicantCount: number;
    service: ApplicantService;
    constructor(service: ApplicantService) {
        this.service = service;
        this.applicantCount = 0;
    }

    activate() {
        this.service.listApplicant().then((response) => {
            this.applicantCount = JSON.parse(response.response).length;
            this.service.toggleLoading();
        }) 
    }
}

