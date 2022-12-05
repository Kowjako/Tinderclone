import { Component, OnInit } from '@angular/core';
import { Member } from '../_models/member';
import { Pagination } from '../_models/pagination';
import { MembersService } from '../_services/members.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  members: Member[] | undefined;
  predicate: string = 'liked';
  pageNumber = 1;
  pageSize = 5;
  pagination: Pagination | undefined;

  constructor(private memService: MembersService) { }

  ngOnInit(): void {
    this.loadLikes();
  }

  loadLikes()
  {
    this.memService.getLikes(this.predicate, this.pageNumber, this.pageSize).subscribe({
      next: resp => {
        this.members = resp.result;
        this.pagination = resp.pagination;
      }
    })
  }

  pageChanged(event: any)
  {
    if(this.pageNumber !== event.page)
    {
      this.pageNumber = event.page;
      this.loadLikes();
    }
  }
}
