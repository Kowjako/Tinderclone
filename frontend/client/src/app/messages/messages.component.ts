import { Component, OnInit } from '@angular/core';
import { faEnvelope, faEnvelopeOpen, faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import { Message } from '../_models/message';
import { Pagination } from '../_models/pagination';
import { MessagesService } from '../_services/messages.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})

export class MessagesComponent implements OnInit {
  messages?: Message[];
  pagination?: Pagination;
  container: string = 'Unread';
  pageNumber = 1;
  pageSize = 5;

  iconEnvelope = faEnvelope;
  iconEnvelopeOpen = faEnvelopeOpen;
  iconPaperPlane = faPaperPlane;

  loading: boolean = false;

  constructor(private msgService: MessagesService) { }

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages()
  {
    this.loading = true;
    console.log('load msg');
    this.msgService.getMessages(this.pageNumber, this.pageSize, this.container).subscribe({
      next: resp => {
        this.messages = resp.result;
        console.log(this.messages[0]);
        this.pagination = resp.pagination;
        this.loading = false;
      } 
    })
  }

  pageChanged(event: any)
  {
    if(this.pageNumber !== event.page)
    {
      this.pageNumber = event.page;
      this.loadMessages();
    }
  }
}
