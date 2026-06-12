import * as signalR from '@microsoft/signalr';
import { Injectable } from '@angular/core';
import { ProductDTO } from '../../models/Product/ProductDTO';

@Injectable({
  providedIn: 'root',
})
export class ProductSignalrService {

private hubConnection!: signalR.HubConnection;

 startConnection(): Promise<void> {
    this.hubConnection = new signalR.HubConnectionBuilder()

      .withUrl('https://localhost:7081/productHub', {
        accessTokenFactory: () => localStorage.getItem('token') ?? ''
      })
      .withAutomaticReconnect()
      .build();

    return this.hubConnection.start();
  }

 onNewProduct(callback: (product: ProductDTO) => void) {
    this.hubConnection.on('NewProduct', callback);
  }



  // addMessageListener() {
  //   this.hubConnection.on(
  //     'ReceiveMessage',
  //     (user: string, message: string) => {
  //       console.log(`${user}: ${message}`);
  //     }
  //   );
  // }

  // sendMessage(user: string, message: string) {
  //   this.hubConnection.invoke(
  //     'SendMessage',
  //     user,
  //     message
  //   );
  // }


}
