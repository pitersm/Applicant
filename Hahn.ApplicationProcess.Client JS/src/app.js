import {PLATFORM} from 'aurelia-pal';

export class App {
  configureRouter(config, router){
    config.title = 'Applicant';
    config.options.pushState = true;
    config.options.root = '/';
    config.map([
      {route: ['', 'home'], moduleId: PLATFORM.moduleName('./components/home/home'), name: 'home', title: 'Home'},
      { route: 'applicantList',  moduleId: PLATFORM.moduleName('./components/applicant-list/applicant-list'), name:'list' },
      { route: 'applicant/new',  moduleId: PLATFORM.moduleName('./components/applicant/applicant'), name:'new' },
      { route: 'applicant/:id',  moduleId: PLATFORM.moduleName('./components/applicant/applicant'), name:'detail' }
    ]);

    this.router = router;
  }
}

