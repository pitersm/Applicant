import { inject } from 'aurelia-framework'
import { ValidationRules, ValidationControllerFactory, validateTrigger } from 'aurelia-validation'
import { DialogService } from 'aurelia-dialog';
import { Prompt } from '../modal/modal';
import { PromptOK } from '../ok-modal/modal-ok';
import { HttpClient } from 'aurelia-http-client';
import { config } from '../config';
import { Router } from 'aurelia-router';

@inject(ValidationControllerFactory, DialogService, Router)
export class Applicant {
    constructor(ValidationControllerFactory, dialogService, router) {
        this.controller = ValidationControllerFactory.createForCurrentScope();
        this.controller.validateTrigger = validateTrigger.change;
        this.propertiesTouched = [];
        this.dialogService = dialogService;
        this.router = router;
        this.httpClient = new HttpClient();
    }

    activate(params) {
        this.id = params.id;
        this.applicant = { };

        if (this.id) {
            this.getApplicant(this.id);
        } 

        this.applicantValidation();
    }

    getApplicant(id) {
        this.httpClient.get(`${config.api_url}Applicant/${id}`).then(data => {
            this.applicant = JSON.parse(data.response);
            this.controller.validate();
        }, response => {
            this.dialogService.open({ viewModel: PromptOK, model: response.response }).whenClosed(() => {
                this.router.navigate('home');
            });
        });
    }

    get applicantHasValue() {
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

    created(owningView, myView) {
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
        this.httpClient.put(`${config.api_url}Applicant`, this.applicant).then(() => {
            this.dialogService.open({ viewModel: PromptOK, model: "The applicant has been succesfully updated!"});
        }, response => {
            this.dialogService.open({ viewModel: PromptOK, model: response.response });
        });
    }

    createNewApplicant() {
        this.httpClient.post(`${config.api_url}Applicant`, this.applicant).then(response => {
            this.dialogService.open({ viewModel: PromptOK, model: "The applicant has been succesfully created!"}).whenClosed(() => {
                this.router.navigate(`applicant/${response.response}`, { replace: true, trigger: true });
            });
        }, response => {
            this.dialogService.open({ viewModel: PromptOK, model: response.response });
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