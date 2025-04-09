export class SearchDTO {

    public nameSearch: string = '';
    public authorSearch: string = '';
    public yearSearch: number = 0;
    public genreSearch: number = 0;

    public constructor(obj?: any) {
        if (obj) {

        }
    }
}