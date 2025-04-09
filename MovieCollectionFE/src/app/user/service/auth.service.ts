import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpSettings } from '../../settings';
import { UserRegisterDTO } from '../model/user.registerDTO.model';
import { UserLoginDTO } from '../model/user.loginDTO.model';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { TokenClaims } from '../../movie/model/token.claims.model';
import { User } from '../model/user.model';

interface JwtPayload {
  [key: string]: string | number;
  exp: number;  //token expiration time
}

@Injectable({
  providedIn: 'root'
})

export class AuthService {

  readonly API = 'api/Authentication';

  constructor(private http: HttpClient, private router: Router) { }

  register(user: UserRegisterDTO): Observable<any> {
    return this.http.post<any>(HttpSettings.api_host + this.API + '/register', user, { headers: HttpSettings.standard_header });
  }

  login(user: UserLoginDTO): Observable<string> {
    return this.http.post(HttpSettings.api_host + this.API + '/login', user,
      { responseType: 'text' })
  }

  getMe(): Observable<string> {
    return this.http.get(HttpSettings.api_host + this.API, { responseType: 'text' });
  }

  logout(): void {
    localStorage.removeItem('authToken');
    this.router.navigate(['homepage']);
  }

  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(HttpSettings.api_host + 'api/User/getAllDto', { headers: HttpSettings.standard_header });
  }

  getAllExceptLogged(username: string): Observable<User[]> {
    return this.http.get<User[]>(HttpSettings.api_host + 'api/User/getAllExceptLogged/' + username, { headers: HttpSettings.standard_header });
  }

  decodeToken(): TokenClaims {
    const token = localStorage.getItem('authToken');
    let decoded: JwtPayload | null = null;

    try {
      if (token) {
        decoded = jwtDecode(token);
      }
    } catch (error) {
      console.error('Error decoding token:', error);
    }

    const roleClaim = decoded ? String(decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']) : '';
    const usernameClaim = decoded ? String(decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name']) : '';

    return new TokenClaims(roleClaim, usernameClaim);
  }



}
