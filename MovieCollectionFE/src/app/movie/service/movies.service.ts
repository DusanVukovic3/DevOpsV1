import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, map } from "rxjs";
import { Movie } from "../model/movie.model";
import { HttpSettings } from "../../settings";
import { SearchDTO } from "../model/search.movies.model";
import { AddMovieUserDTO } from "../../user/model/add.movie.user.DTO";

@Injectable({
    providedIn: 'root'
})
export class MovieService {

    readonly API = 'api/Movie';

    constructor(private http: HttpClient) { }

    getAll(): Observable<Movie[]> {
        return this.http.get<Movie[]>(HttpSettings.api_host + this.API, { headers: HttpSettings.standard_header })
            .pipe(
                map(movies => movies.map(movie => ({ ...movie, genre: this.mapGenreToString(+movie.genre) })))
            );
    }

    getAllByUser(username: string): Observable<Movie[]> {
        return this.http.get<Movie[]>(HttpSettings.api_host + this.API + '/byUsername/' + username, { headers: HttpSettings.standard_header })
            .pipe(
                map(movies => movies.map(movie => ({ ...movie, genre: this.mapGenreToString(+movie.genre) })))
            );
    }

    getAllByUserInverse(username: string): Observable<Movie[]> {
        return this.http.get<Movie[]>(HttpSettings.api_host + this.API + '/inverseByUsername/' + username, { headers: HttpSettings.standard_header })
            .pipe(
                map(movies => movies.map(movie => ({ ...movie, genre: this.mapGenreToString(+movie.genre) })))
            );
    }

    getMovieById(movieId: string): Observable<Movie> {
        return this.http.get<Movie>(HttpSettings.api_host + this.API + '/' + movieId, { headers: HttpSettings.standard_header })
            .pipe(
                map(movie => ({ ...movie, genre: this.mapGenreToString(+movie.genre) }))
            );
    }

    getAllGenres(): Observable<any[]> {
        return this.http.get<any[]>(HttpSettings.api_host + this.API + '/allGenres', { headers: HttpSettings.standard_header });
    }

    createMovie(movie: Movie): Observable<Movie> {
        console.log(movie);
        return this.http.post<any>(HttpSettings.api_host + this.API, movie, { headers: HttpSettings.standard_header });
    }

    updateMovie(movieId: string, movie: Movie): Observable<Movie> {
        return this.http.put<Movie>(HttpSettings.api_host + this.API + '/' + movieId, movie, { headers: HttpSettings.standard_header });
    }

    deleteMovie(movieId: string): Observable<any> {
        return this.http.delete(HttpSettings.api_host + this.API + '/' + movieId, { headers: HttpSettings.standard_header });
    }

    searchAllMovies(searchDTO: SearchDTO): Observable<Movie[]> {
        return this.http.post<Movie[]>(HttpSettings.api_host + this.API + '/search', searchDTO, { headers: HttpSettings.standard_header })
            .pipe(
                map(movies => movies.map(movie => ({ ...movie, genre: this.mapGenreToString(+movie.genre) })))
            );
        ;
    }

    searchMoviesByUser(searchDTO: SearchDTO, username: string): Observable<Movie[]> {
        return this.http.post<Movie[]>(HttpSettings.api_host + this.API + '/searchByUser/' + username, searchDTO, { headers: HttpSettings.standard_header })
            .pipe(
                map(movies => movies.map(movie => ({ ...movie, genre: this.mapGenreToString(+movie.genre) })))
            );
        ;
    }

    addMovieToUser(request: AddMovieUserDTO): Observable<string> {
        return this.http.put(HttpSettings.api_host + 'api/User' + '/addMovieToUser/', request, { responseType: 'text' });
    }

    removeMovieFromUser(request: AddMovieUserDTO): Observable<string> {
        return this.http.put(HttpSettings.api_host + 'api/User' + '/removeMovieFromUser/', request, { responseType: 'text' });
    }


    private mapGenreToString(genreId: number): string {
        const genreMap: Record<number, string> = {
            0: 'NoSelection',
            1: 'Action',
            2: 'Comedy',
            3: 'Horror',
            4: 'Cartoon',
            5: 'Adventure',
            6: 'Romance',
            7: 'Science_Fiction',
        };
        return genreMap[genreId] || '';
    }


}
