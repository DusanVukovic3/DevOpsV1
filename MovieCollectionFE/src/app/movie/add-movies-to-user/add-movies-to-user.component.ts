import { Component, ViewChild, OnInit, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { Movie } from '../model/movie.model';
import { MovieService } from '../service/movies.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AddMovieUserDTO } from '../../user/model/add.movie.user.DTO';

@Component({
  selector: 'app-add-movies-to-user',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, MatSortModule],
  templateUrl: './add-movies-to-user.component.html',
  styleUrl: './add-movies-to-user.component.css'
})
export class AddMoviesToUserComponent implements OnInit {

  displayedColumns: String[] = ["name", "description", "author", "genre", "releaseDate", "actions"];
  movies: Movie[] = [];
  dataSource = new MatTableDataSource<Movie>();

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

  @ViewChild(MatSort) sort!: MatSort;

  constructor(private movieService: MovieService, @Inject(MAT_DIALOG_DATA) public data: any, public dialogRef: MatDialogRef<AddMoviesToUserComponent>) { }

  ngOnInit(): void {
    this.getAllThatAreNotAdded();
  }

  getAllThatAreNotAdded(): void {
    this.movieService.getAllByUserInverse(this.data.username).subscribe(res => {
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

  addMovieToTheUser(movie: Movie): void {
    var request = new AddMovieUserDTO();
    request.movieId = movie.movieId;
    request.username = this.data.username;

    this.movieService.addMovieToUser(request).subscribe(res => {
      alert(`Movie has been successfully added to the collection`);
      this.dialogRef.close();
    })
  }
}
