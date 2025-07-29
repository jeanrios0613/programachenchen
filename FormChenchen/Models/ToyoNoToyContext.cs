using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FormChenchen.Models;

public partial class ToyoNoToyContext : DbContext
{
    public ToyoNoToyContext()
    {
    }

    public ToyoNoToyContext(DbContextOptions<ToyoNoToyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<DocumentReference> DocumentReferences { get; set; }

    public virtual DbSet<Enterprise> Enterprises { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<RequestDetail> RequestDetails { get; set; }

    public virtual DbSet<RequestDetailsCopium> RequestDetailsCopia { get; set; }

    public virtual DbSet<RequestInfo> RequestInfos { get; set; }

    public virtual DbSet<RequestsCopium> RequestsCopia { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<VwUserRolesInfo> VwUserRolesInfos { get; set; }

 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(500);
            entity.Property(e => e.Message).HasMaxLength(512);
            entity.Property(e => e.ProcessInstanceId).HasMaxLength(500);
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasIndex(e => e.RequestId, "IX_Contacts_Include");

            entity.HasIndex(e => e.RequestId, "IX_Contacts_RequestId").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IdentificationNumber).HasMaxLength(20);
            entity.Property(e => e.IdentificationType).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(25);

            entity.HasOne(d => d.Request).WithOne(p => p.Contact).HasForeignKey<Contact>(d => d.RequestId);
        });

        modelBuilder.Entity<DocumentReference>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(256);
            entity.Property(e => e.DocumentTitle).HasMaxLength(256);
            entity.Property(e => e.ProcessInstanceId).HasMaxLength(500);
            entity.Property(e => e.StageName).HasMaxLength(256);
        });

        modelBuilder.Entity<Enterprise>(entity =>
        {
            entity.HasIndex(e => e.BusinessDescription, "IX_Enterprises_Include");

            entity.HasIndex(e => e.RequestId, "IX_Enterprises_RequestId").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.BusinessDescription)
                .HasMaxLength(600)
                .IsUnicode(false);
            entity.Property(e => e.BusinessName).HasMaxLength(100);
            entity.Property(e => e.BusinessTime)
                .HasMaxLength(16)
                .HasDefaultValue("");
            entity.Property(e => e.Corregimiento)
                .HasMaxLength(50)
                .HasDefaultValue("");
            entity.Property(e => e.District)
                .HasMaxLength(50)
                .HasDefaultValue("");
            entity.Property(e => e.EconomicActivity).HasMaxLength(100);
            entity.Property(e => e.Instagram).HasMaxLength(50);
            entity.Property(e => e.MonthlySales).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Province)
                .HasMaxLength(50)
                .HasDefaultValue("");
            entity.Property(e => e.ProyectedSales).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Ruc)
                .HasMaxLength(28)
                .IsUnicode(false);
            entity.Property(e => e.WebSite).HasMaxLength(100);

            entity.HasOne(d => d.Request).WithOne(p => p.Enterprise).HasForeignKey<Enterprise>(d => d.RequestId);
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasIndex(e => e.Id, "IX_Requests_Id");

            entity.HasIndex(e => e.Id, "IX_Requests_Include");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Suggestion)
                .HasMaxLength(50)
                .HasDefaultValue("");
        });

        modelBuilder.Entity<RequestDetail>(entity =>
        {
            entity.HasIndex(e => e.RequestId, "IX_RequestDetails_Include");

            entity.HasIndex(e => e.RequestId, "IX_RequestDetails_RequestId").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ContactReason).HasMaxLength(500);
            entity.Property(e => e.ManagementExecuted)
                .HasMaxLength(500)
                .HasColumnName("managementExecuted");
            entity.Property(e => e.QuantityToInvert).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ReasonForMoney).HasMaxLength(500);
            entity.Property(e => e.TipoAtencion).HasMaxLength(50);
            entity.Property(e => e.VerifyClient).HasMaxLength(50);

            entity.HasOne(d => d.Request).WithOne(p => p.RequestDetail).HasForeignKey<RequestDetail>(d => d.RequestId);
        });

        modelBuilder.Entity<RequestDetailsCopium>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("RequestDetails_copia");

            entity.Property(e => e.QuantityToInvert).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ReasonForMoney).HasMaxLength(500);
        });

        modelBuilder.Entity<RequestInfo>(entity =>
        {
            entity.HasKey(e => e.CodId).HasName("PK_Request_Info_cod_ID");

            entity.ToTable("Request_Info");

            entity.Property(e => e.CodId)
                .ValueGeneratedNever()
                .HasColumnName("cod_ID");
            entity.Property(e => e.ActividadEconomica)
                .HasMaxLength(4000)
                .HasColumnName("Actividad_economica");
            entity.Property(e => e.Apellido).HasMaxLength(4000);
            entity.Property(e => e.CodigoDeSolicitud)
                .HasMaxLength(4000)
                .HasColumnName("Codigo_de_solicitud");
            entity.Property(e => e.Corregimiento)
                .HasMaxLength(4000)
                .HasColumnName("corregimiento");
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(4000)
                .HasColumnName("Correo_Electronico");
            entity.Property(e => e.CuantoChenchenNecesitas)
                .HasMaxLength(4000)
                .HasColumnName("Cuanto_Chenchen_necesitas");
            entity.Property(e => e.DescripcionNegocio)
                .HasMaxLength(4000)
                .HasColumnName("Descripcion_negocio");
            entity.Property(e => e.Distrito).HasMaxLength(4000);
            entity.Property(e => e.EnQueLoInvertiras)
                .HasMaxLength(4000)
                .HasColumnName("En_que_lo_invertiras");
            entity.Property(e => e.Etapa).HasMaxLength(32);
            entity.Property(e => e.EtapaDelNegocio)
                .HasMaxLength(17)
                .HasColumnName("Etapa_del_Negocio");
            entity.Property(e => e.FechaActualizacion).HasColumnName("Fecha_Actualizacion");
            entity.Property(e => e.FechaDeCreacion).HasColumnName("Fecha_de_Creacion");
            entity.Property(e => e.FechaInicioOperaciones)
                .HasMaxLength(4000)
                .HasColumnName("Fecha_Inicio_Operaciones");
            entity.Property(e => e.GestionRealizada)
                .HasMaxLength(4000)
                .HasColumnName("Gestion_Realizada");
            entity.Property(e => e.Gestor).HasMaxLength(32);
            entity.Property(e => e.IdChen).HasColumnName("id_chen");
            entity.Property(e => e.Instagram).HasMaxLength(4000);
            entity.Property(e => e.Nombre).HasMaxLength(4000);
            entity.Property(e => e.NombreNegocio)
                .HasMaxLength(4000)
                .HasColumnName("Nombre_Negocio");
            entity.Property(e => e.NumeroIdentificacion)
                .HasMaxLength(4000)
                .HasColumnName("Numero_identificacion");
            entity.Property(e => e.PorqueNoContacto)
                .HasMaxLength(4000)
                .HasColumnName("Porque_no_contacto");
            entity.Property(e => e.Provincia).HasMaxLength(4000);
            entity.Property(e => e.ProyeccionVentasMensuales)
                .HasMaxLength(4000)
                .HasColumnName("Proyeccion_ventas_mensuales");
            entity.Property(e => e.Ruc)
                .HasMaxLength(4000)
                .HasColumnName("RUC");
            entity.Property(e => e.Telefono).HasMaxLength(4000);
            entity.Property(e => e.TipoAtencion)
                .HasMaxLength(4000)
                .HasColumnName("Tipo_atencion");
            entity.Property(e => e.TipoIdentificacion)
                .HasMaxLength(4000)
                .HasColumnName("Tipo_identificacion");
            entity.Property(e => e.UsuarioAsignado)
                .HasMaxLength(256)
                .HasColumnName("Usuario_Asignado");
            entity.Property(e => e.VentasMensuales)
                .HasMaxLength(4000)
                .HasColumnName("Ventas_mensuales");
            entity.Property(e => e.VerificacionCliente)
                .HasMaxLength(4000)
                .HasColumnName("Verificacion_Cliente");
            entity.Property(e => e.WebSite)
                .HasMaxLength(4000)
                .HasColumnName("Web_Site");
            entity.Property(e => e.TipoRequest)
                .HasMaxLength(1);
        });

        modelBuilder.Entity<RequestsCopium>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Requests_copia");

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Suggestion).HasMaxLength(50);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.ActiveDirectoryGroup).HasMaxLength(64);
            entity.Property(e => e.Description).HasMaxLength(64);
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.RolName).HasMaxLength(256);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.Id).HasMaxLength(450);
            entity.Property(e => e.Lastname).HasMaxLength(30);
            entity.Property(e => e.Names).HasMaxLength(30);
            entity.Property(e => e.Updatepass).HasColumnName("UPDATEPASS");
            entity.Property(e => e.UserName).HasMaxLength(256);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });

            entity.Property(e => e.UserId).HasMaxLength(200);
            entity.Property(e => e.RoleId).HasMaxLength(200);
        });

        modelBuilder.Entity<VwUserRolesInfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_UserRolesInfo");

            entity.Property(e => e.Description).HasMaxLength(64);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.Nombre).HasMaxLength(61);
            entity.Property(e => e.RolName).HasMaxLength(256);
            entity.Property(e => e.Username).HasMaxLength(256);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
