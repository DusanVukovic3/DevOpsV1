import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { Movie } from '../model/movie.model';
import { MovieService } from '../service/movies.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

@Component({
  selector: 'app-update-movie',
  standalone: true,
  imports: [CommonModule, MatDatepickerModule, MatNativeDateModule, MatFormFieldModule, MatInputModule, FormsModule, MatButtonModule, MatSelectModule],
  providers: [],
  templateUrl: './update-movie.component.html',
  styleUrl: './update-movie.component.css'
})
export class UpdateMovieComponent {

  public movieObject: Movie = new Movie();
  genres: number[] = [];

  initialDescription: string = '';
  initialAuthor: string = '';
  initialGenre: string = '';

  constructor(private movieService: MovieService, public dialogRef: MatDialogRef<UpdateMovieComponent>, @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
    this.getGenres();
    this.getMovie();
  }


  saveMovie() {
    this.movieService.updateMovie(this.movieObject.movieId, this.movieObject).subscribe(res => {
      alert(`Informations have been successfully updated for movie: ${this.movieObject.name}`);
      this.dialogRef.close();

    })
  }

  getGenres() {
    this.movieService.getAllGenres().subscribe(res => {
      this.genres = res;
      console.log(this.genres);
    })
  }

  getMovie() {
    this.movieService.getMovieById(this.data.movieId).subscribe(res => {
      this.movieObject = res;
      this.initialDescription = this.movieObject.description;
      this.initialAuthor = this.movieObject.author;
      this.initialGenre = this.movieObject.genre;

      console.log(this.initialGenre);
    })
  }

  hasChanges(): boolean {
    return (
      this.movieObject.description !== this.initialDescription ||
      this.movieObject.author !== this.initialAuthor ||
      this.movieObject.genre !== this.initialGenre
    );
  }


}
