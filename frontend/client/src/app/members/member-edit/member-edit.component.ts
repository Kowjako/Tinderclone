import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm | undefined;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any)
  {
    if(this.editForm?.dirty)
    {
      $event.returnValue = true;
    }
  }
  member: Member | undefined;
  user: User | undefined;

  constructor(private accService: AccountService, 
              private memberService: MembersService, private tt: ToastrService) { 
    this.accService.currentUser$.pipe(take(1)).subscribe({
      next: data => this.user = data,
      error: err => console.log(err)
    });
  }

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember(){
    if(!this.user) return;
    this.memberService.getMember(this.user.username).subscribe({
      next: resp => this.member = resp,
      error: err => console.log(err)
    });
  }

  updateMember()
  {
    this.memberService.updateMember(this.editForm?.value).subscribe({
      next: _ => {
        this.tt.success('Profile updated successfully!');
        this.editForm?.reset(this.member);
      }
    });
  }
}
