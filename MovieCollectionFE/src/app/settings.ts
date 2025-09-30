import { HttpHeaders } from "@angular/common/http";

export class HttpSettings {
  static readonly api_host: string =
    location.hostname === 'localhost'
      // Windows dev (Visual Studio default)
      ? (location.port === '4200'
        ? 'http://localhost:5000/'   // Angular dev server → Linux BE
        : 'https://localhost:7122/') // Angular dev server → Windows BE
      : 'https://api.krecimstanove.com/'; // VPS / Production

  static readonly standard_header: HttpHeaders =
    new HttpHeaders({ 'Content-Type': 'application/json' });
}
