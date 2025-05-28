using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class createbd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bibliothecaires",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    nom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    prenom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bibliothecaires", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_Bibliothecaires_UserId",
                        column: x => x.UserId,
                        principalTable: "Bibliothecaires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_Bibliothecaires_UserId",
                        column: x => x.UserId,
                        principalTable: "Bibliothecaires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Bibliothecaires_UserId",
                        column: x => x.UserId,
                        principalTable: "Bibliothecaires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_Bibliothecaires_UserId",
                        column: x => x.UserId,
                        principalTable: "Bibliothecaires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Livres",
                columns: table => new
                {
                    id_livre = table.Column<string>(type: "text", nullable: false),
                    id_biblio = table.Column<string>(type: "text", nullable: true),
                    date_edition = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    titre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    auteur = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isbn = table.Column<string>(type: "character varying(18)", maxLength: 18, nullable: true),
                    editeur = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Langue = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: true),
                    couverture = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livres", x => x.id_livre);
                    table.ForeignKey(
                        name: "FK_Livres_Bibliothecaires_id_biblio",
                        column: x => x.id_biblio,
                        principalTable: "Bibliothecaires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Membre",
                columns: table => new
                {
                    id_membre = table.Column<string>(type: "text", nullable: false),
                    id_biblio = table.Column<string>(type: "text", nullable: true),
                    TypeMembre = table.Column<string>(type: "text", nullable: false),
                    nom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    prenom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    telephone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    cin_ou_passeport = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    date_inscription = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Statut = table.Column<string>(type: "text", nullable: false, defaultValue: "actif")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Membre", x => x.id_membre);
                    table.ForeignKey(
                        name: "FK_Membre_Bibliothecaires_id_biblio",
                        column: x => x.id_biblio,
                        principalTable: "Bibliothecaires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Nouveaute",
                columns: table => new
                {
                    id_nouv = table.Column<string>(type: "text", nullable: false),
                    id_biblio = table.Column<string>(type: "text", nullable: true),
                    titre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    fichier = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    date_publication = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    couverture = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, defaultValue: "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Favf.asso.fr%2Famboise%2Fwp-content%2Fuploads%2Fsites%2F171%2F2021%2F03%2FLogo-Nouveau.jpg&f=1&nofb=1&ipt=fdbaaa07e45eb9aa0e1f8802a963c3259485319662623816e07adf250d84f1f9")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nouveaute", x => x.id_nouv);
                    table.ForeignKey(
                        name: "FK_Nouveaute_Bibliothecaires_id_biblio",
                        column: x => x.id_biblio,
                        principalTable: "Bibliothecaires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Parametres",
                columns: table => new
                {
                    id_param = table.Column<string>(type: "text", nullable: false),
                    IdBiblio = table.Column<string>(type: "text", nullable: true),
                    Delais_Emprunt_Etudiant = table.Column<int>(type: "integer", nullable: false),
                    Delais_Emprunt_Enseignant = table.Column<int>(type: "integer", nullable: false),
                    Delais_Emprunt_Autre = table.Column<int>(type: "integer", nullable: false),
                    Modele_Email_Retard = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    date_modification = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parametres", x => x.id_param);
                    table.ForeignKey(
                        name: "FK_Parametres_Bibliothecaires_IdBiblio",
                        column: x => x.IdBiblio,
                        principalTable: "Bibliothecaires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Inventaire",
                columns: table => new
                {
                    id_inv = table.Column<string>(type: "text", nullable: false),
                    id_liv = table.Column<string>(type: "text", nullable: false),
                    cote_liv = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    etat = table.Column<string>(type: "text", nullable: false, defaultValue: "moyen"),
                    statut = table.Column<string>(type: "text", nullable: false, defaultValue: "disponible"),
                    inventaire = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventaire", x => x.id_inv);
                    table.ForeignKey(
                        name: "FK_Inventaire_Livres_id_liv",
                        column: x => x.id_liv,
                        principalTable: "Livres",
                        principalColumn: "id_livre",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Statistiques",
                columns: table => new
                {
                    id_stat = table.Column<string>(type: "text", nullable: false),
                    id_param = table.Column<string>(type: "text", nullable: true),
                    Nombre_Sanction_Emises = table.Column<int>(type: "integer", nullable: false),
                    Somme_Amende_Collectées = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Taux_Emprunt_En_Perte = table.Column<double>(type: "double precision", nullable: false),
                    Emprunt_Par_Membre = table.Column<double>(type: "double precision", nullable: false),
                    Taux_Emprunt_En_Retard = table.Column<double>(type: "double precision", nullable: false),
                    Période_en_jour = table.Column<int>(type: "integer", nullable: false),
                    date_stat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statistiques", x => x.id_stat);
                    table.ForeignKey(
                        name: "FK_Statistiques_Parametres_id_param",
                        column: x => x.id_param,
                        principalTable: "Parametres",
                        principalColumn: "id_param",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Emprunts",
                columns: table => new
                {
                    id_emp = table.Column<string>(type: "text", nullable: false),
                    id_membre = table.Column<string>(type: "text", nullable: false),
                    id_biblio = table.Column<string>(type: "text", nullable: true),
                    Id_inv = table.Column<string>(type: "text", nullable: false),
                    date_emp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2025, 5, 26, 12, 47, 30, 213, DateTimeKind.Utc).AddTicks(8473)),
                    date_retour_prevu = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    date_effectif = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Statut_emp = table.Column<string>(type: "text", nullable: false, defaultValue: "en_cours"),
                    note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emprunts", x => x.id_emp);
                    table.ForeignKey(
                        name: "FK_Emprunts_Bibliothecaires_id_biblio",
                        column: x => x.id_biblio,
                        principalTable: "Bibliothecaires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Emprunts_Inventaire_Id_inv",
                        column: x => x.Id_inv,
                        principalTable: "Inventaire",
                        principalColumn: "id_inv",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Emprunts_Membre_id_emp",
                        column: x => x.id_emp,
                        principalTable: "Membre",
                        principalColumn: "id_membre",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Sanction",
                columns: table => new
                {
                    id_sanc = table.Column<string>(type: "text", nullable: false),
                    id_membre = table.Column<string>(type: "text", nullable: false),
                    id_biblio = table.Column<string>(type: "text", nullable: true),
                    id_emp = table.Column<string>(type: "text", nullable: true),
                    raison = table.Column<string>(type: "text", nullable: false),
                    date_sanction = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    date_fin_sanction = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    montant = table.Column<decimal>(type: "numeric(100,3)", nullable: true),
                    payement = table.Column<bool>(type: "boolean", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sanction", x => x.id_sanc);
                    table.ForeignKey(
                        name: "FK_Sanction_Bibliothecaires_id_biblio",
                        column: x => x.id_biblio,
                        principalTable: "Bibliothecaires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Sanction_Emprunts_id_emp",
                        column: x => x.id_emp,
                        principalTable: "Emprunts",
                        principalColumn: "id_emp",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Sanction_Membre_id_membre",
                        column: x => x.id_membre,
                        principalTable: "Membre",
                        principalColumn: "id_membre",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Bibliothecaires",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Bibliothecaires",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Emprunts_id_biblio",
                table: "Emprunts",
                column: "id_biblio");

            migrationBuilder.CreateIndex(
                name: "IX_Emprunts_Id_inv",
                table: "Emprunts",
                column: "Id_inv");

            migrationBuilder.CreateIndex(
                name: "IX_Inventaire_id_liv",
                table: "Inventaire",
                column: "id_liv");

            migrationBuilder.CreateIndex(
                name: "IX_Livres_id_biblio",
                table: "Livres",
                column: "id_biblio");

            migrationBuilder.CreateIndex(
                name: "IX_Livres_isbn",
                table: "Livres",
                column: "isbn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Membre_cin_ou_passeport",
                table: "Membre",
                column: "cin_ou_passeport",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Membre_email",
                table: "Membre",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Membre_id_biblio",
                table: "Membre",
                column: "id_biblio");

            migrationBuilder.CreateIndex(
                name: "IX_Nouveaute_id_biblio",
                table: "Nouveaute",
                column: "id_biblio");

            migrationBuilder.CreateIndex(
                name: "IX_Parametres_IdBiblio",
                table: "Parametres",
                column: "IdBiblio");

            migrationBuilder.CreateIndex(
                name: "IX_Sanction_id_biblio",
                table: "Sanction",
                column: "id_biblio");

            migrationBuilder.CreateIndex(
                name: "IX_Sanction_id_emp",
                table: "Sanction",
                column: "id_emp");

            migrationBuilder.CreateIndex(
                name: "IX_Sanction_id_membre",
                table: "Sanction",
                column: "id_membre");

            migrationBuilder.CreateIndex(
                name: "IX_Statistiques_id_param",
                table: "Statistiques",
                column: "id_param",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Nouveaute");

            migrationBuilder.DropTable(
                name: "Sanction");

            migrationBuilder.DropTable(
                name: "Statistiques");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Emprunts");

            migrationBuilder.DropTable(
                name: "Parametres");

            migrationBuilder.DropTable(
                name: "Inventaire");

            migrationBuilder.DropTable(
                name: "Membre");

            migrationBuilder.DropTable(
                name: "Livres");

            migrationBuilder.DropTable(
                name: "Bibliothecaires");
        }
    }
}
