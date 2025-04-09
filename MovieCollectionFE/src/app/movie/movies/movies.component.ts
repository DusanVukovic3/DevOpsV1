import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableDataSource, MatTableModule } from '@angular/material/table'
import { Movie } from '../model/movie.model';
import { MovieService } from '../service/movies.service';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { UpdateMovieComponent } from '../update-movie/update-movie.component';
import { MatDialog } from '@angular/material/dialog';
import { CreateMovieComponent } from '../create-movie/create-movie.component';
import { DeleteMovieComponent } from '../delete-movie/delete-movie.component';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { SearchDTO } from '../model/search.movies.model';
import { AuthService } from '../../user/service/auth.service';
import { MatToolbarModule } from '@angular/material/toolbar';
import { jwtDecode } from "jwt-decode";
import { TokenClaims } from '../model/token.claims.model';
import { AddMoviesToUserComponent } from '../add-movies-to-user/add-movies-to-user.component';
import { AddMovieUserDTO } from '../../user/model/add.movie.user.DTO';
import { MatTooltipModule } from '@angular/material/tooltip';
import { UsersSeeCollectionComponent } from '../../user/users-see-collection/users-see-collection.component';


@Component({
  selector: 'app-movies',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, MatSortModule, MatIconModule, MatInputModule, FormsModule, MatSelectModule, MatFormFieldModule, MatToolbarModule, MatTooltipModule],
  providers: [],
  templateUrl: './movies.component.html',
  styleUrl: './movies.component.css',
})
export class MoviesComponent implements OnInit {


  displayedColumns: String[] = ["name", "description", "author", "genre", "releaseDate", "actions"];
  movies: Movie[] = [];
  genres: string[] = [];
  genreMapping: { [key: string]: number } = {
    'NoSelection': 0,
    'Action': 1,
    'Comedy': 2,
    'Horror': 3,
    'Cartoon': 4,
    'Adventure': 5,
    'Romance': 6,
    'Science_Fiction': 7,
  };
  searchDTO: SearchDTO = new SearchDTO();

  dataSource = new MatTableDataSource<Movie>();

  @ViewChild(MatSort) sort!: MatSort;

  userData: TokenClaims = new TokenClaims('', '');

  constructor(private movieService: MovieService, public dialog: MatDialog, private authService: AuthService) { }

  ngOnInit(): void {
    this.userData = this.authService.decodeToken();
    this.getAll();
  }

  getAll(): void {
    const serviceCall = this.userData.role === 'Admin' ? this.movieService.getAll() : this.movieService.getAllByUser(this.userData.username);

    serviceCall.subscribe(res => {
      this.getGenres();
      this.movies = res;
      this.dataSource.data = this.movies;
      this.dataSource.sort = this.sort;
    });
  }


  getGenres() {
    this.movieService.getAllGenres().subscribe(res => {
      this.genres = res;
    })
  }

  addMovie(): void {
    this.dialog.open(CreateMovieComponent, {
      width: '50%',
      height: '50%'
    })
      .afterClosed().subscribe(res => {
        this.getAll();
      });
  }

  editMovie(movie: Movie): void {
    this.dialog.open(UpdateMovieComponent, {
      width: '50%',
      height: '50%',
      data: movie
    })
      .afterClosed().subscribe(res => {
        this.getAll();
      });
  }

  deleteMovie(movie: Movie): void {
    const dialogRef = this.dialog.open(DeleteMovieComponent, {
      width: '20%',
      height: '20%',
      data: {
        message: 'Are you sure you want to delete this movie?',
        confirmText: 'Delete',
        cancelText: 'Cancel'
      }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.movieService.deleteMovie(movie.movieId).subscribe(res => {
          alert('Movie successfully deleted!');
          this.getAll();
        });
      }
    });
  }

  searchMovies() {
    this.movieService.searchAllMovies(this.searchDTO).subscribe(res => {
      this.movies = res;
      this.dataSource.data = this.movies;
      this.dataSource.sort = this.sort;
    })
  }

  searchMoviesByUser() {
    this.movieService.searchMoviesByUser(this.searchDTO, this.userData.username).subscribe(res => {
      this.movies = res;
      this.dataSource.data = this.movies;
      this.dataSource.sort = this.sort;
    })
  }

  logOut() {
    this.authService.logout();
  }

  addMovieToCollection(): void {
    this.dialog.open(AddMoviesToUserComponent, {
      width: '70%',
      height: '70%',
      data: this.userData
    })
      .afterClosed().subscribe(res => {
        this.getAll();
      });
  }

  removeMovie(movie: Movie): void {
    var request = new AddMovieUserDTO();
    request.movieId = movie.movieId;
    request.username = this.userData.username;

    this.movieService.removeMovieFromUser(request).subscribe(res => {
      alert(`Movie has successfully been removed from the collection`);
      this.getAll();
    })
  }

  showOtherUsers() {
    this.dialog.open(UsersSeeCollectionComponent, {
      width: '40%',
      height: '50%',
      data: this.userData
    })
      .afterClosed().subscribe(res => {
        this.getAll();
      });
  }



}
