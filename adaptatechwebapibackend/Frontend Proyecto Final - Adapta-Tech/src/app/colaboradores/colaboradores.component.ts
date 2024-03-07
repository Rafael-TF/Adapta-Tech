import { Component, OnInit } from '@angular/core';
import { CursorService } from '../../services/cursor.service';

@Component({
  selector: 'app-colaboradores',
  standalone: true,
  imports: [],
  templateUrl: './colaboradores.component.html',
  styleUrl: './colaboradores.component.css'
})
export class ColaboradoresComponent implements OnInit{
  bodyCursor: string = 'auto';
pointerCursor: string = 'pointer';
  constructor(private cursorService: CursorService) { }

  ngOnInit(): void {
    this.cursorService.bodyCursor$.subscribe(cursor => {
      this.bodyCursor = cursor;
    });
    this.cursorService.pointerCursor$.subscribe(cursor => {
      this.pointerCursor = cursor;
    });
  }

}
