import { autoinject } from 'aurelia-framework';
import { ApplicantService } from 'resources/services/applicant-service';
import { PLATFORM } from 'aurelia-pal';
import { RouterConfiguration, Router } from 'aurelia-router';

@autoinject
export class App {
  router: Router;

  constructor(public service: ApplicantService) {}

  configureRouter(config: RouterConfiguration, router: Router) {
    this.router = router;
    config.title = 'Applicant';
    config.options.pushState = true;
    config.options.root = '/';
    config.map([
      { route: ['', 'home'], moduleId: PLATFORM.moduleName('./components/home/home'), name: 'home', title: 'Home' },
      { route: 'applicantList', moduleId: PLATFORM.moduleName('./components/applicant-list/applicant-list'), name: 'list' },
      { route: 'applicant/new', moduleId: PLATFORM.moduleName('./components/applicant/applicant'), name: 'new' },
      { route: 'applicant/:id', moduleId: PLATFORM.moduleName('./components/applicant/applicant'), name: 'detail', title: "Edit Applicant" }
    ]);
  }
}
