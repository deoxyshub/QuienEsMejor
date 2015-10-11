using System;
using System.Collections.Generic;
using WhoIsBetter.Entity;

namespace WhoIsBetter.DataAccess
{
    public class UsuarioDA : DataAccessBase
    {
        public UsuarioDA() : base("DB") { }

        public int CrearUsuario(Usuario usuario)
        {
            return this.ExecuteScalar<int>("usp_CrearUsuario",
                new DataParameter("@pCorreo", usuario.Correo),
                new DataParameter("@pNombre", usuario.Nombre),
                new DataParameter("@pApellidoPaterno", usuario.ApellidoPaterno),
                new DataParameter("@pApellidoMaterno", usuario.ApellidoMaterno),
                new DataParameter("@pClave", usuario.Clave),
                new DataParameter("@pIDRol", usuario.IDRol)
            );
        }

        public Usuario ObtenerUsuario(int id)
        {
            return this.ExecuteEntity<Usuario>("usp_ObtenerUsuario",
                dr => new Usuario
                {
                    ID = dr.GetValue<int>("ID"),
                    Correo = dr.GetString("Correo"),
                    Nombre = dr.GetString("Nombre"),
                    ApellidoPaterno = dr.GetString("ApellidoPaterno"),
                    ApellidoMaterno = dr.GetString("ApellidoMaterno"),
                    Clave = dr.GetString("Clave"),
                    Sexo = dr.GetNullableValue<bool>("Sexo"),
                    NumeroCelular = dr.GetNullableValue<int>("NumeroCelular"),
                    NumeroTelefono = dr.GetNullableValue<int>("NumeroTelefono"),
                    FechaNacimiento = dr.GetNullableValue<DateTime>("FechaNacimiento"),
                    IDRol = dr.GetValue<int>("IDRol")
                },
                new DataParameter("@pID", id)
            );
        }

        public Usuario ObtenerUsuario(string correo)
        {
            return this.ExecuteEntity<Usuario>("usp_ObtenerUsuario",
                dr => new Usuario
                {
                    ID = dr.GetValue<int>("ID"),
                    Correo = dr.GetString("Correo"),
                    Nombre = dr.GetString("Nombre"),
                    ApellidoPaterno = dr.GetString("ApellidoPaterno"),
                    ApellidoMaterno = dr.GetString("ApellidoMaterno"),
                    Clave = dr.GetString("Clave"),
                    Sexo = dr.GetNullableValue<bool>("Sexo"),
                    NumeroCelular = dr.GetNullableValue<int>("NumeroCelular"),
                    NumeroTelefono = dr.GetNullableValue<int>("NumeroTelefono"),
                    FechaNacimiento = dr.GetNullableValue<DateTime>("FechaNacimiento"),
                    IDRol = dr.GetValue<int>("IDRol")
                },
                new DataParameter("@pCorreo", correo)
            );
        }

        public void ActualizarUsuario(Usuario usuario)
        {
            this.ExecuteNonQuery("usp_ActualizarUsuario",
                new DataParameter("@pID", usuario.ID),
                new DataParameter("@pNombre", usuario.Nombre),
                new DataParameter("@pApellidoPaterno", usuario.ApellidoPaterno),
                new DataParameter("@pApellidoMaterno", usuario.ApellidoMaterno),
                new DataParameter("@pClave", usuario.Clave),
                new DataParameter("@pSexo", usuario.Sexo),
                new DataParameter("@pNumeroCelular", usuario.NumeroCelular),
                new DataParameter("@pNumeroTelefono", usuario.NumeroTelefono),
                new DataParameter("@pFechaNacimiento", usuario.FechaNacimiento)
            );
        }

        public void EliminarUsuario(int id)
        {
            this.ExecuteNonQuery("usp_EliminarUsuario",
                new DataParameter("@pID", id)
            );
        }
        
        public ICollection<Usuario> ListarUsuarios()
        {
            return this.ExecuteEntityCollection<Usuario>("usp_ListarUsuarios",
                dr => new Usuario
                {
                    ID = dr.GetValue<int>("ID"),
                    Correo = dr.GetString("Correo"),
                    Nombre = dr.GetString("Nombre"),
                    ApellidoPaterno = dr.GetString("ApellidoPaterno"),
                    ApellidoMaterno = dr.GetString("ApellidoMaterno"),
                    Clave = dr.GetString("Clave"),
                    IDRol = dr.GetValue<int>("IDRol")
                }
            );
        }
    }
}