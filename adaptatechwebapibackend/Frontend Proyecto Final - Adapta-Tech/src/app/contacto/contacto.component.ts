import { Component } from '@angular/core';
import { routes } from '../app.routes';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-contacto',
  standalone: true,
  imports: [RouterModule],
  templateUrl:'./contacto.component.html',
  styleUrl: './contacto.component.css'
})
export class ContactoComponent {

}
