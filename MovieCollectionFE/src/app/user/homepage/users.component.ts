import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../service/auth.service';
import { UserLoginDTO } from '../model/user.loginDTO.model';
import { FormsModule } from '@angular/forms';
import { UserRegisterDTO } from '../model/user.registerDTO.model';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { Router } from '@angular/router';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule, FormsModule, MatInputModule, MatFormFieldModule, MatButtonModule],
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent {

  userLogIn: UserLoginDTO = new UserLoginDTO();
  userRegister: UserRegisterDTO = new UserRegisterDTO();

  constructor(private authService: AuthService, private router: Router) { }

  register(user: UserRegisterDTO) {
    this.authService.register(user).subscribe(res => {
    })
  }

  login(user: UserLoginDTO) {
    this.authService.login(user).subscribe((token: string) => {
      localStorage.setItem('authToken', token);
      alert(`Welcome ${user.username}`);
      this.router.navigate(['/movies']);
    });
  }

  getMe() {
    this.authService.getMe().subscribe((name: string) => {
      console.log(name);
    });
  }

}
