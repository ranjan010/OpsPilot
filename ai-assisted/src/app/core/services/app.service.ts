import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class AppService {
  getMessage(): string {
    return 'Angular 19 architecture boilerplate is ready.';
  }
}
