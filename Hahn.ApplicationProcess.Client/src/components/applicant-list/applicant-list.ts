import { Router } from 'aurelia-router';
import { ApplicantDto } from './../../resources/model/applicantDto';
import { ApplicantService } from 'resources/services/applicant-service';
import { autoinject } from 'aurelia-framework';
import { throws } from 'assert';

@autoinject
export class ApplicantList {
    service: ApplicantService;
    applicants: ApplicantDto[] = [];
    selectedApplicantId: number;
    router: Router;
    constructor(service: ApplicantService, router: Router) {
        this.service = service;
        this.router = router;
    }

    activate() {
        this.service.listApplicant().then((response) => {
            this.applicants = JSON.parse(response.response);
            this.service.toggleLoading();
        }) 
    }

    selectApplicant(applicant: any) {
        this.router.navigate(`applicant/${applicant.id}`);
    }
}
