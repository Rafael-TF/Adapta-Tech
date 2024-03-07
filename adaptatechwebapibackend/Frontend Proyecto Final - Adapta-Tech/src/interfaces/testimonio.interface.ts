export interface ITestimonio {
  idTestimonio: number;
  nombre: string;
  avatar: string;
  titulo: string;
  texto: string;
}

export interface ITestimoniosGet {
  idTestimonio: number;
  idPerfilUsuario: Number;
  avatar: File | null;
  nombrePerfilUsuario: string;
  tituloTestimonio: string;
  textoTestimonio: string;
}

export interface ITestimonioPost {
  TituloTestimonio: string;
  TextoTestimonio: string;
  IdPerfilUsuario: number;
}

export interface ITestimonioPut {
  idTestimonio: number;
  tituloTestimonio: string;
  textoTestimonio: string;
}
