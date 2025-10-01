//import { Injectable } from '@angular/core';
//import * as signalR from '@microsoft/signalr';
//import { BehaviorSubject } from 'rxjs';
//import { environment } from '../../environments/environment';
//import { Product } from '../models/product';

//@Injectable({
//  providedIn: 'root'
//})
//export class ProductSignalRService {
//  private hubConnection: signalR.HubConnection;
//  private productCreated = new BehaviorSubject<Product>(null);
//  private productUpdated = new BehaviorSubject<Product>(null);
//  private productDeleted = new BehaviorSubject<string>(null);

//  productCreated$ = this.productCreated.asObservable();
//  productUpdated$ = this.productUpdated.asObservable();
//  productDeleted$ = this.productDeleted.asObservable();

//  constructor() {
//    this.createConnection();
//    this.registerOnServerEvents();
//    this.startConnection();
//  }

//  private createConnection() {
//    this.hubConnection = new signalR.HubConnectionBuilder()
//      .withUrl(`${environment.apiUrl}/ProductHub`)
//      .withAutomaticReconnect()
//      .build();
//  }

//  private startConnection() {
//    this.hubConnection
//      .start()
//      .then(() => console.log('Connection started'))
//      .catch(err => console.log('Error while starting connection: ' + err));
//  }

//  private registerOnServerEvents() {
//    this.hubConnection.on('ProductCreated', (data: Product) => {
//      this.productCreated.next(data);
//    });

//    this.hubConnection.on('ProductUpdated', (data: Product) => {
//      this.productUpdated.next(data);
//    });

//    this.hubConnection.on('ProductDeleted', (id: string) => {
//      this.productDeleted.next(id);
//    });
//  }
//}
