import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Message } from 'src/app/_models/message';
import { MessagesService } from 'src/app/_services/messages.service';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
  @ViewChild('msgForm') msgForm: NgForm;
  @Input() userName?: string;
  messageContent: string = '';

  constructor(public msgService: MessagesService) { }

  ngOnInit(): void {
  }

  sendMessage() {
    if (!this.userName) return;
    this.msgService.sendMessage(this.userName, this.messageContent).then(() => {
      this.msgForm.reset();
    })
  }
}
