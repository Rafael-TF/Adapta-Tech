export interface IPerfil {
  idPerfil: number;
  usuarioId: number;
  nombre: string;
  apellidos: string;
  telefono: string;
  fechaNacimiento: Date | null;
  avatar?: File | undefined;
  alias: string;
}

export interface IPerfilUsuarioPut {
  idPerfil: number;
  idUsuario: number;
  nombre: string;
  apellidos: string;
  telefono: string;
  fechaNacimiento: Date;
  avatar?: File | undefined;
  alias: string | null;
}