import { Component, OnDestroy, OnInit, signal } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-confirm-email-page',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './confirm-email-page.html',
  styleUrls: ['./confirm-email-page.css']
})
export class ConfirmEmailPage implements OnInit, OnDestroy {

  success = signal('');
message=signal('')

  private timerId: any;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router
  ) {

  }

  ngOnInit() {
    this.success.set(this.route.snapshot.queryParamMap.get('success') ?? '');
    if( this.success() === 'true')
      this.message.set("Confirmation Success")

 this.timerId = setTimeout(() => {
  this.router.navigate(['/login']);
}, 2000);
  }



  ngOnDestroy() {
    clearInterval(this.timerId);
  }
}
