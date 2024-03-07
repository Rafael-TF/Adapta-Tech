export interface ICitaMedica {
    idCita?: number;
    medico: string;
    fechaHora: Date;
    centroMedico: string;
    diaSemana: string;
    idPerfilUsuario: number;
  }

  export interface IAgregarCitaMedica {
    medico: string;
    fechaHora: Date;
    centroMedico: string;
    idPerfilUsuario: number;
}

export interface IModificarCitaMedica {
  idCita: number;
  medico: string;
  fechaHora: Date | null; // Permitir valores nulos
  centroMedico: string;
}

