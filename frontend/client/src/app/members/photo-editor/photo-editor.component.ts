import { Component, Input, OnInit } from '@angular/core';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { Member } from 'src/app/_models/member';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})

export class PhotoEditorComponent implements OnInit {
  @Input() member : Member | undefined;
  iconTrash = faTrash;

  constructor() { }

  ngOnInit(): void {
  }

}
