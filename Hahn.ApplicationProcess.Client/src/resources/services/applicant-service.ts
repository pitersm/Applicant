import { HttpClient, HttpResponseMessage } from 'aurelia-http-client';
import { autoinject } from 'aurelia-dependency-injection';
import { config } from './../../config'
import { ApplicantDto } from 'resources/model/applicantDto';

@autoinject
export class ApplicantService {
    httpClient: HttpClient;
    baseUrl: string;
    loading = false;

    constructor(httpClient: HttpClient) {
        this.httpClient = httpClient;
        this.baseUrl = config.api_url;
        this.httpClient.configure(config => {
            config
                .withBaseUrl(this.baseUrl)
            // I really really tried to handle the loader through an httpInterceptor but for some reason it did not work =/
                // .withInterceptor({
                //     request(request) {
                //       this.loading = true;
                //       return request;
                //     },
                //     response(response) {
                //       this.loading = false;
                //       return response;
                //     }
                //   });
        });
    }

    toggleLoading() {
        this.loading = !this.loading;
    }

    listApplicant() {
        this.toggleLoading();
        return this.httpClient.get('Applicant');
    }

    getApplicant(id: number) {
        this.toggleLoading();
        return this.httpClient.get(`Applicant/${id}`);
    }

    deleteApplicant(id: number) {
        this.toggleLoading();
        return this.httpClient.delete(`Applicant/${id}`);
    }

    updateApplicant(applicant: ApplicantDto) {
        this.toggleLoading();
        return this.httpClient.put(`Applicant`, applicant);
    }

    createNewApplicant(applicant: ApplicantDto): Promise<HttpResponseMessage> {
        this.toggleLoading();
        return this.httpClient.post(`Applicant`, applicant)
    }
}