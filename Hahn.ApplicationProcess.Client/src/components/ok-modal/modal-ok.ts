import {inject} from 'aurelia-framework';
import {DialogController} from 'aurelia-dialog';

@inject(DialogController)

export class PromptOK {
   controller: DialogController;
   answer: string;
   message: string;

   constructor(controller: DialogController) {
      this.controller = controller;
      this.answer = null;

      controller.settings.centerHorizontalOnly = true;
   }
   activate(message) {
      this.message = message;
   }
}
