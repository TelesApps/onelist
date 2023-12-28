import { HttpClientModule } from '@angular/common/http';
import { Component, OnInit, inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [ MatButtonModule ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {

  authService = inject(AuthService)

  constructor() { }

  ngOnInit() {
  }

  test() {
    this.authService.getUsers().subscribe({
      next: (data) => {
        console.log(data)
      },
      error: (err) => console.log(err),
      complete: () => console.log('complete')
    })
  }

}
