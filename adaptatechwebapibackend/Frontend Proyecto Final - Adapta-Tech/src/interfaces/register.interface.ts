export interface IRegister {
  email: string;
  password: string;
  avatar: File | undefined;
  perfilUsuario: {
    nombre: string;
    apellidos: string;
    telefono: string;
    fechaNacimiento: Date | null;
    alias: string;
  };
}