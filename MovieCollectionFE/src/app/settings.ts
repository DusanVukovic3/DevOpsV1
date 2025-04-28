import { HttpHeaders } from "@angular/common/http";

export class HttpSettings {
    static readonly api_host: string = 

    // Kada smo u developmentu, onda idemo na visual studio localHost, inace idemo na VPS tj. api.krecimstanove.com
    location.hostname === 'localhost'
      ? 'https://localhost:7122/'
      : 'https://api.krecimstanove.com/';

    static readonly standard_header: HttpHeaders = 
      new HttpHeaders({ 'Content-Type': 'application/json' });
}
