import { Component, OnInit } from '@angular/core';
import { TextoService } from '../../services/texto.service';
import { CdkDragEnd, DragDropModule } from '@angular/cdk/drag-drop';
import { CursorService } from '../../services/cursor.service';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-video',
  standalone: true,
  imports: [DragDropModule, CommonModule],  
  templateUrl: './video.component.html',
  styleUrl: './video.component.css'
})
export class VideoComponent implements OnInit{
  bodyCursor: string = 'auto';
  pointerCursor: string = 'pointer';
  transform = '';
  cuadroVisible = true;
  constructor(private textoService: TextoService, private cursorService: CursorService) {}

  ngOnInit(): void {
    this.cursorService.bodyCursor$.subscribe(cursor => {
      this.bodyCursor = cursor;
    });
    this.cursorService.pointerCursor$.subscribe(cursor => {
      this.pointerCursor = cursor;
    });
    this.cuadroVisible = true;
  }

  obtenerTamanoTexto(): number {
    return this.textoService.getTamanoTexto();
  }
  
  onDragEnd(event: CdkDragEnd) {
    const point = event.source.getFreeDragPosition();
    this.transform = `translate(${point.x}px, ${point.y}px)`;
  }

  cerrarCuadro() {
    this.cuadroVisible = false;
    console.log("adios",this.cuadroVisible);
  }
}
