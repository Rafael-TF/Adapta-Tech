import { Routes } from '@angular/router';
import { LoginComponent } from './menu/login/login.component';
import { MenuComponent } from './menu/menu/menu.component';
import { NotfoundComponent } from './menu/notfound/notfound.component';
import { AyudasComponent } from './Recursos/ayudas/ayudas.component';
import { InicioComponent } from './inicio/inicio.component';
import { FormacionComponent } from './Recursos/formacion/formacion.component';
import { InclusionComponent } from './Recursos/inclusion/inclusion.component';
import { OcioComponent } from './Recursos/ocio/ocio.component';
import { VisualComponent } from './Accesibilidad/visual/visual.component';
import { FisicaComponent } from './Accesibilidad/Fisica/fisica.component';
import { AuditivaComponent } from './Accesibilidad/auditiva/auditiva.component';
import { CognitivaComponent } from './Accesibilidad/cognitiva/cognitiva.component';
import { ColaboradoresComponent } from './colaboradores/colaboradores.component';
import { ContactoComponent } from './contacto/contacto.component'
import { RegistroComponent } from './menu/registro/registro.component';
import { MedicacionComponent } from './Herramientas/Medicacion/medicacion.component';
import { CalculadoraComponent } from './Herramientas/Calculadora/calculadora.component';
import { EquipoComponent } from './equipo/equipo.component';
import { TestimonioComponent } from './comunidad/testimonio/testimonio.component';
import { DescargasComponent } from './descargas/descargas.component';
import { ListaTareasComponent } from './Herramientas/lista-tareas/lista-tareas.component';
import { LogueadoComponent } from './menu/login/logueado/logueado.component';
import { ForoComponent } from './comunidad/foro/foro.component';
import { VideoComponent } from './Video/video.component';
import { NoticiasComponent } from './comunidad/noticias/noticias.component';
import { EnlacesComponent } from './comunidad/enlaces/enlaces.component';

export const routes: Routes = [
    { path: '', redirectTo: 'inicio', pathMatch: 'full' },
    { path: 'inicio', component: InicioComponent },
    { path: 'menu', component: MenuComponent },
    { path: 'login', component: LoginComponent },
    { path: 'registro', component: RegistroComponent },
    { path: 'video', component: VideoComponent },

    { path: 'ayudas', component: AyudasComponent},
    { path: 'formacion', component: FormacionComponent},
    { path: 'inclusion', component: InclusionComponent},
    { path: 'ocio', component: OcioComponent},
    { path: 'visual', component: VisualComponent},
    { path: 'fisica', component: FisicaComponent},
    { path: 'auditiva', component: AuditivaComponent},
    { path: 'cognitiva', component: CognitivaComponent},
    { path: 'colaboradores', component: ColaboradoresComponent},
    { path: 'equipo', component: EquipoComponent},
    { path: 'testimonios', component: TestimonioComponent},
    { path: 'noticias', component: NoticiasComponent},
    { path: 'enlaces', component: EnlacesComponent},
    { path: 'contacto', component: ContactoComponent },
    { path: 'calcu', component: CalculadoraComponent },
    { path: 'medic', component: MedicacionComponent },
    { path: 'lista', component: ListaTareasComponent },
    { path: 'descargas', component: DescargasComponent},
    { path: 'logueado', component: LogueadoComponent},
    { path: 'foro', component: ForoComponent},
    { path: '**', component: NotfoundComponent }
  ];
