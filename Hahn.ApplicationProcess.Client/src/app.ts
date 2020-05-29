import { inject } from 'aurelia-framework';
import { ApplicantService } from 'resources/services/applicant-service';
import { PLATFORM } from 'aurelia-pal';
import { RouterConfiguration, Router } from 'aurelia-router';
import { I18N } from 'aurelia-i18n';

@inject(ApplicantService, I18N)
export class App {
  router: Router;
  i18n;
  // static inject = [I18N];

  constructor(public service: ApplicantService, I18N) {
    this.i18n = I18N;
      this.i18n
      .setLocale('de').then(() => {});
  }

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
