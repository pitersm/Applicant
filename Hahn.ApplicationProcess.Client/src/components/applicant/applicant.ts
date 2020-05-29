import { ValidationRules, ValidationControllerFactory, validateTrigger, ValidationController } from 'aurelia-validation'
import { DialogService } from 'aurelia-dialog';
import { Prompt } from '../modal/modal';
import { PromptOK } from '../ok-modal/modal-ok';
import { Router } from 'aurelia-router';
import { autoinject } from 'aurelia-dependency-injection';
import { ApplicantDto } from 'resources/model/applicantDto';
import { ApplicantService } from 'resources/services/applicant-service';

@autoinject
export class Applicant {
    controller: ValidationController;
    dialogService: DialogService;
    router: Router;
    id: number;
    applicant: ApplicantDto;
    routeConfig;
    service: ApplicantService;

    constructor(ValidationControllerFactory: ValidationControllerFactory, dialogService: DialogService, router: Router, service: ApplicantService) {
        this.controller = ValidationControllerFactory.createForCurrentScope();
        this.controller.validateTrigger = validateTrigger.change;
        this.dialogService = dialogService;
        this.router = router;
        this.service = service;
    }

    activate(params: { id: number; }, routeConfig) {
        this.id = params.id;
        this.applicant = new ApplicantDto();
        this.routeConfig = routeConfig;

        if (this.id) {
            this.getApplicant(this.id);
        } else {
            this.routeConfig.navModel.setTitle('Create New Applicant')
        }

        this.applicantValidation();
    }

    getApplicant(id: number) {
        this.service.getApplicant(id).then(data => {
            this.applicant = JSON.parse(data.response);
            this.controller.validate();
            this.routeConfig.navModel.setTitle(`Edit ${this.applicant.name} ${this.applicant.familyName}`);
            this.service.toggleLoading();
        }, response => {
            this.service.toggleLoading();
            this.dialogService.open({ viewModel: PromptOK, model: response.response }).whenClosed(() => {
                this.router.navigate('home');
            });
        });
    }

    get applicantHasValue(): boolean {
        if (!this.applicant) {
            return false;
        }

        let hasValue = false;
        const properties = Object.keys(this.applicant);
        for (let i = 1; i < properties.length; i++) {
            if (this.applicant[properties[i]]) {
                hasValue = true;
            }
        }
        return hasValue;
    }

    created() {
        if (!this.id) {
            this.controller.validate();
        }
    }

    applicantValidation() {
        return ValidationRules
            .ensure('name').displayName("The applicant's first name").required().minLength(5)
            .ensure('familyName').displayName("The applicant's first name").required().minLength(5)
            .ensure('address').displayName("The applicant's address").required().minLength(10)
            .ensure('countryOfOrigin').displayName("The applicant's country").required()
            .ensure('eMailAddress').displayName("The applicant's e-mail").required().email()
            .ensure('age').displayName("The applicant's age").required().range(20, 60)
            .on(this.applicant);
    }

    onSave() {
        if (this.id) {
            this.updateApplicant();
        } else {
            this.createNewApplicant();
        }
    }

    updateApplicant() {
        this.service.updateApplicant(this.applicant).then(() => {
            this.dialogService.open({ viewModel: PromptOK, model: "The applicant has been succesfully updated!" });
            this.service.toggleLoading();
        }, response => {
            this.dialogService.open({ viewModel: PromptOK, model: response.response });
            this.service.toggleLoading();
        });
    }

    createNewApplicant() {
        this.service.createNewApplicant(this.applicant).then(response => {
            this.dialogService.open({ viewModel: PromptOK, model: "The applicant has been succesfully created!" }).whenClosed(() => {
                this.router.navigate(`applicantList`);
                this.service.toggleLoading();
            });
        }, response => {
            this.dialogService.open({ viewModel: PromptOK, model: response.response });
            this.service.toggleLoading();
        });
    }

    onDelete() {
        this.dialogService.open({ viewModel: Prompt, model: 'Are you sure you want to delete this applicant?' }).whenClosed(response => {
            if (!response.wasCancelled) {
                this.service.deleteApplicant(this.applicant.id).then(response => {
                    this.dialogService.open({ viewModel: PromptOK, model: "The applicant has been succesfully deleted!" }).whenClosed(() => {
                        this.router.navigate(`applicantList`);
                        this.service.toggleLoading();
                    });
                }, response => {
                    this.dialogService.open({ viewModel: PromptOK, model: response.response });
                    this.service.toggleLoading();
                });
            }
        });
    }

    onReset() {
        this.dialogService.open({ viewModel: Prompt, model: 'Are you sure you want to reset the form?' }).whenClosed(response => {
            if (!response.wasCancelled) {
                this.applicant.name = '';
                this.applicant.familyName = '';
                this.applicant.address = '';
                this.applicant.countryOfOrigin = '';
                this.applicant.eMailAddress = '';
                this.applicant.age = '';
                this.applicant.hired = false;
                this.controller.validate();
            }
        });
    }
}