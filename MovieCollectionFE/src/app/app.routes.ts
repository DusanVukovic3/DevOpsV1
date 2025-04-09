import { Routes } from '@angular/router';
import { MoviesComponent } from './movie/movies/movies.component';
import { UsersComponent } from './user/homepage/users.component';

export const routes: Routes = [

    { path: "", redirectTo: '/homepage', pathMatch: 'full' },
    { path: "homepage", component: UsersComponent },
    { path: "movies", component: MoviesComponent }

];
