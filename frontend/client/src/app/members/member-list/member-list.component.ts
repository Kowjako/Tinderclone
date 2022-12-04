import { Component, OnInit } from '@angular/core';
import { Observable, take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  //members$: Observable<Member[]> | undefined;
  members: Member[] = [];
  pagination: Pagination | undefined;
  userParams: UserParams;
  user: User;
  genderList = [{value: 'male', display: 'Males'}, {value: 'female', display: 'Females'}]

  constructor(private memberService: MembersService, private accService: AccountService) { 
    this.accService.currentUser$.pipe(take(1)).subscribe({
      next: resp => {
        if(resp)
        {
          this.userParams = new UserParams(resp);
          this.user = resp;
        }
      }
    })
  }

  ngOnInit(): void {
    //this.members$ = this.memberService.getMembers();
    this.loadMembers();
  }

  resetFilters()
  {
    if(this.user)
    {
      this.userParams = new UserParams(this.user);
      this.loadMembers();
    }
  }

  loadMembers()
  {
    this.memberService.getMembers(this.userParams).subscribe({
      next: resp => {
        if(resp.result && resp.pagination)
        {
          this.members = resp.result;
          this.pagination = resp.pagination;
        }
      }
    })
  }

  pageChanged(event: any)
  {
    if(this.userParams?.pageNumber !== event.page)
    {
      this.userParams.pageNumber = event.page;
      this.loadMembers();
    } 
  }
}
