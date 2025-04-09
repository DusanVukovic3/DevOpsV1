import { MovieService } from './../service/movies.service';
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Movie } from '../model/movie.model';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatDialogRef } from '@angular/material/dialog';
import { MoviesComponent } from '../movies/movies.component';

@Component({
  selector: 'app-create-movie',
  standalone: true,
  imports: [CommonModule, MatDatepickerModule, MatNativeDateModule, MatFormFieldModule, MatInputModule, FormsModule, MatButtonModule, MatSelectModule],
  providers: [],
  templateUrl: './create-movie.component.html',
  styleUrl: './create-movie.component.css'
})
export class CreateMovieComponent implements OnInit {

  public movieObject: Movie = new Movie();
  genres: string[] = [];
  constructor(private movieService: MovieService, public dialogRef: MatDialogRef<CreateMovieComponent>) { }

  ngOnInit(): void {
    this.getGenres();
  }

  // Da li moze ove komentar?
  saveMovie() {
    this.movieService.createMovie(this.movieObject).subscribe(res => {
      alert(`Sucessfully added movie ${this.movieObject.name}`);
      this.dialogRef.close();

    })
  }

  getGenres() {
    this.movieService.getAllGenres().subscribe(res => {
      this.genres = res;
    })
  }
}
