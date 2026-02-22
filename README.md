# Enterprise Media Vault

ASP.NET Core Minimal API + MongoDB + GridFS + CQRS (MediatR) para repositorio documental empresarial.

## Inicio rapido

```bash
docker compose up --build
```

API: `http://localhost:5000`
Swagger: `http://localhost:5000/swagger`
Dashboard: `http://localhost:5000/login.html`

Usuario semilla:
- email: `admin@vault.local`
- password: `Admin12345!`

## Proyectos
- `src/EnterpriseMediaVault.Domain`: entidades, eventos de dominio, enums
- `src/EnterpriseMediaVault.Application`: CQRS, DTOs, validaciones, abstracciones
- `src/EnterpriseMediaVault.Infrastructure`: MongoDB, GridFS, JWT, repositorios, seed, indices
- `src/EnterpriseMediaVault.API`: endpoints Carter, middleware, SignalR, dashboard estatico
- `tests/EnterpriseMediaVault.UnitTests`: base para pruebas unitarias xUnit
- `tests/EnterpriseMediaVault.IntegrationTests`: base para pruebas de integracion

## Funcionalidades implementadas
- Autenticacion JWT + refresh token
- Roles: Admin, Manager, Employee, Auditor
- Politicas de autorizacion
- Rate limiting global
- Gestion de carpetas jerarquicas (crear, eliminar soft, arbol)
- Upload streaming multipart para archivos grandes
- GridFS y estrategia de almacenamiento (local/gridfs + stubs S3/Azure)
- Versionado automatico y rollback
- Hash SHA256 para integridad
- Full text search basico + filtros + paginacion
- Dashboard ejecutivo (KPIs + auditoria)
- Logging estructurado con Serilog
- Middleware global de excepciones
- Seed inicial y creacion de indices MongoDB
- SignalR para notificacion de nuevo archivo

## Scripts de indices
- `scripts/create-indexes.mongodb.js`

## Seguridad
- Validacion de MIME permitido
- Soft delete
- Auditoria completa por accion

## Nota tecnica
Este entorno de trabajo no tiene `dotnet` instalado, por lo que no se ejecuto compilacion local aqui. El codigo queda estructurado para compilar en un entorno con SDK .NET 8.

---

## Desarrollado por Isaac Esteban Haro Torres

**Ingeniero en Sistemas · Full Stack · Automatizacion · Data**

- Email: zackharo1@gmail.com
- WhatsApp: 098805517
- GitHub: https://github.com/ieharo1
- Portafolio: https://ieharo1.github.io/portafolio-isaac.haro/

---

## Licencia

© 2026 Isaac Esteban Haro Torres - Todos los derechos reservados.
