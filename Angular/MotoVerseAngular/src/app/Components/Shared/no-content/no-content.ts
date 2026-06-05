import { Component, Input, input } from '@angular/core';

@Component({
  selector: 'app-no-content',
  imports: [],
  templateUrl: './no-content.html',
  styleUrl: './no-content.css',
})
export class NoContent {

  @Input() title: string = 'No Content Available';

  @Input() message: string = 'There is no content to display at the moment. Please check back later.';


}
