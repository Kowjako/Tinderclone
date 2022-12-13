import { Component, OnInit } from '@angular/core';
import { Photo } from 'src/app/_models/photo';
import { AdminService } from 'src/app/_services/admin.service';

@Component({
  selector: 'app-photo-managment',
  templateUrl: './photo-managment.component.html',
  styleUrls: ['./photo-managment.component.css']
})
export class PhotoManagmentComponent implements OnInit {
  photos: Photo[];

  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    this.getPhotosForApproval();
  }

  getPhotosForApproval()
  {
    this.adminService.getPhotosForApproval().subscribe({
      next: photos => this.photos = photos
    });
  }

  approvePhoto(photoId: number)
  {
    this.adminService.approvePhoto(photoId).subscribe({
      next: _ => this.photos.splice(this.photos.findIndex(p => p.id === photoId), 1)
    });
  }

  rejectPhoto(photoId: number)
  {
    this.adminService.rejectPhoto(photoId).subscribe({
      next: _ => this.photos.splice(this.photos.findIndex(p => p.id === photoId), 1)
    });
  }
}
