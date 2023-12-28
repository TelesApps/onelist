import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  public getUsers() {
    return this.http.get('https://localhost:5001/api/users/');
  }

  public getUser(id: number) {
    return this.http.get('https://localhost:5001/api/users/' + id);
  }


}
