import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Subject } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new Subject();

  model: any = {};

  constructor(private accountService: AccountService, private toastr: ToastrService) { }

  ngOnInit(): void {
    
  }

  register()
  {
    this.accountService.register(this.model).subscribe(
      {
        next: resp => 
        {
          console.log(resp);
          this.cancel();
        },
        error: err =>
        {
          console.log(err);
          this.toastr.error(err.error);
        }
      }
    );
  }

  cancel()
  {
    this.cancelRegister.next(false);
  }

}
