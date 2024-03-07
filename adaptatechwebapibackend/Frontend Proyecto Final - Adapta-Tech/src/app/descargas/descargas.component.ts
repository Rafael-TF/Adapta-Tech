import { Component, OnInit } from '@angular/core';
import { TextoService } from '../../services/texto.service';
import { CursorService } from '../../services/cursor.service';

@Component({
  selector: 'app-descargas',
  standalone: true,
  imports: [],
  templateUrl: './descargas.component.html',
  styleUrl: './descargas.component.css'
})
export class DescargasComponent implements OnInit {
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
