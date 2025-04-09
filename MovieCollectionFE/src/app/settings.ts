import { HttpHeaders } from "@angular/common/http";

export class HttpSettings {
    static readonly api_host: string = 'https://localhost:7122/';
    static readonly standard_header: HttpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });
}
