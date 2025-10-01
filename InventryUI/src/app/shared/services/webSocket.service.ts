import { Injectable } from '@angular/core';
import { io } from 'socket.io-client';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class WebSocketService {
  private socket = io('http://localhost:3000');

  constructor() {}

  listenForUpdates(): Observable<any> {
    return new Observable((observer) => {
      this.socket.on('dataChanged', (data: any) => {
        observer.next(data);
      });

      return () => {
        this.socket.disconnect();
      };
    });
  }
}
