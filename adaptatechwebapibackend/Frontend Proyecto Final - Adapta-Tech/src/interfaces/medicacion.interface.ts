export interface IMedicacion {
    idMedicamento: number;
    medicacion: string;
    posologia: string;
    funcion: string;
    diaSemana?: string;
    idPerfilUsuario?: number;
}

export interface IAnnadirMedicacion {
    medicacion: string;
    posologia: string;
    funcion: string;
    diaSemana?: string;
    idPerfilUsuario?: number;
}

export interface IModificarMedicamento {
    idMedicamento: number;
    medicacion: string;
    posologia: string;
    funcion: string;
    diaSemana?: string;
}

export interface ICitaMedica {
    idCita: number;
    medico: string;
    fechaHora: Date | null;
    centroMedico: string;
    diaSemana: string;
    idPerfilUsuario: number;
}