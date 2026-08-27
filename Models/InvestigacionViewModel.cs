using System;
using System.Collections.Generic;

public class InvestigacionViewModel
{
    public List<EmpleadoConJerarquia> Empleados { get; set; }
    public List<LogsAuditoria> Logs { get; set; }

    public InvestigacionViewModel()
    {
        Empleados = new List<EmpleadoConJerarquia>();
        Logs = new List<LogsAuditoria>();
    }
}

public class EmpleadoConJerarquia
{
    public int Id { get; set; }
    public string Legajo { get; set; }
    public string Nombre { get; set; }
    public string NombreRol { get; set; }
    public int NivelAcceso { get; set; }

    public EmpleadoConJerarquia()
    {
    }

    public EmpleadoConJerarquia(int id, string legajo, string nombre, string nombreRol, int nivelAcceso)
    {
        Id = id;
        Legajo = legajo;
        Nombre = nombre;
        NombreRol = nombreRol;
        NivelAcceso = nivelAcceso;
    }
}
