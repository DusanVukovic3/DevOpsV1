import { Movie } from "../../movie/model/movie.model";

export class User {
    userId: string = '';
    email: string = '';
    username: string = '';
    movies: Movie[] = [];

    public constructor(obj?: any) {
        if (obj) { }
    }
}