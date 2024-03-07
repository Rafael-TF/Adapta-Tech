import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TextoService } from '../../../services/texto.service';
import { CursorService } from '../../../services/cursor.service';

@Component({
  selector: 'app-ayudas',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './ayudas.component.html',
  styleUrl: './ayudas.component.css'
})
export class AyudasComponent implements OnInit {
  bodyCursor: string = 'auto';
  pointerCursor: string = 'pointer';
  constructor(private textoService: TextoService, private cursorService: CursorService) { }

  ngOnInit(): void {
    this.cursorService.bodyCursor$.subscribe(cursor => {
      this.bodyCursor = cursor;
    });
    this.cursorService.pointerCursor$.subscribe(cursor => {
      this.pointerCursor = cursor;
    });
  }

  obtenerTamanoTexto(): number {
    return this.textoService.getTamanoTexto();
  }
}
