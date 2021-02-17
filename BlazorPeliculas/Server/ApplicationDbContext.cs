using BlazorPeliculas.Shared.Entidades;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPeliculas.Server
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
           IOptions<OperationalStoreOptions> operationalStoreOptions)
          : base(options, operationalStoreOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GeneroPelicula>().HasKey(x => new { x.GeneroId, x.PeliculaId });
            modelBuilder.Entity<PeliculaActor>().HasKey(x => new { x.PeliculaId, x.PersonaId });

            //var personas = new List<Persona>();
            //for (int i = 5; i < 100; i++)
            //{
            //    personas.Add(new Persona()
            //    {
            //        Id = i,
            //        Nombre = $"Persona {i}",
            //        FechaNacimiento = DateTime.Today
            //    });
            //}

            //modelBuilder.Entity<Persona>().HasData(personas);
            // Para agregar datos de personas descomentamos lineas anteriores en lanzamos comandos siguientes
            // PM> Add-migration personasprueba
            // PM> Update-Database
            // para quitar los datos agregados anteriormente comentamos lineas anteriores y lanzamos comandos siguientes
            // PM> Add-migration removerpersonas
            // PM> Update-Database

            var roleAdminId = "89086180-b978-4f90-9dbd-a7040bc93f41";
            var roleAdmin = new IdentityRole()         
                { Id = roleAdminId, Name = "admin", NormalizedName = "admin" };

            modelBuilder.Entity<IdentityRole>().HasData(roleAdmin);
            // PM> Add-Migration RolAdmin
            // PM> Update-Database

            // Actualizar cambios con IdentityServer 4
            // PM> Add - Migration NuevoSistemaAutenticacion
            // Update-Database

            // Obtenemos de la migracion el script para crear un usuario con rol administrador pero ejecutarlo directamente en bbdd y no a través de la migración
            // 1.- PM > Add-Migration AdminUser
            // 2.- Una vez que tenemos la migración podemos obtener los script comparando dos migraciones
            //     Script-Migration -from RolAdmin -to adminUser
            // 3.- Eliminar la migracion
            //     PM> Remove-Migration

            
            // var hasher = new PasswordHasher<IdentityUser>();
           // var usuarioAdminId = "6D656F48-7A76-41E1-82CF-5BCF9C2C88CD";

            //var usuarioAdmin = new IdentityUser()
            //{
              //  Id = usuarioAdminId,
              //  Email = "rafael@hotmail.com",
              //  UserName = "rafael@hotmail.com",
              //  NormalizedEmail = "rafael@hotmail.com",
              //  NormalizedUserName = "rafael@hotmail.com",
              //  EmailConfirmed = true,
              //  PasswordHash = hasher.HashPassword(null, "615636C3-F3C1-4691-8D34-210F06C693A8")
            //};
            //modelBuilder.Entity<IdentityUser>().HasData(usuarioAdmin);
            //modelBuilder.Entity<IdentityUserRole<string>>()
            //    .HasData(new IdentityUserRole<string> { RoleId = roleAdminId, UserId = usuarioAdminId });
            
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<GeneroPelicula> GenerosPeliculas { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<PeliculaActor> PeliculasActores { get; set; }
        public DbSet<VotoPelicula> VotosPeliculas { get; set; }
    }
}
