import { Component, OnInit } from '@angular/core';
import { Member } from '../_models/member';
import { MembersService } from '../_services/members.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  members: Member[] | undefined;
  predicate: string = 'liked'

  constructor(private memService: MembersService) { }

  ngOnInit(): void {
    this.loadLikes();
  }

  loadLikes()
  {
    this.memService.getLikes(this.predicate).subscribe({
      next: resp => {
        this.members = resp;
      }
    })
  }
}
