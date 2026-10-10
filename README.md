# junior-developer-assessment

Part D of the Junior Developer Assessment: the **Sales Delivery Terms** master screen, built through four layers.

## Why this repository contains a rebuilt base

The AxiomSys solution was not provided to the candidate, and the assessor asked for a new repository.
The `main` branch is therefore a **minimal rebuild of the AxiomSys patterns shown in the candidate pack**
(snippets C1–C3 and the request diagram). It is not the company's code. Part D is delivered on
`feature/sales-delivery-terms` as a pull request against `main`, so the PR diff contains only Part D.

| Project | Role | Rebuilt from the pack |
|---|---|---|
| `AxiomSysDL` | Data layer: EF Core (Oracle), `SupplyChainContext`, `IRepository<T>`, `IUnitOfWork<TContext>` | `GetRepository<T>().GetAll()`, `Add`, `Commit()` |
| `AxiomSysBL` | Business layer: services, `ResponseModel`, `ISharedService.HandleException` | `status / data / messageAr / messageEn` |
| `AxiomSysSL` | API: JWT auth, `[AuthorizeCompany]`, `ICurrentUserService` ("Id" claim), Swagger, `SalesModuleServiceCollectionExtensions` | C2 attributes and route style |
| `AxiomSysPortal` | Razor Pages UI: `/{culture}/...` routes (en/ar, RTL), cookie sign-in holding the JWT, `Helper`, `SharedLocalizer`, toasts | C1 `Helper.PostRequest`, `SetResponseMessage`, TempData |

The Portal never touches the database; it only calls the API.

### Assumptions (not confirmed against the real system)

- `[AuthorizeCompany]` here only checks that the token carries a company code (403 otherwise).
- Localizer keys `3` Code, `18` Delete, `32` Add and `5583` Edit come from the brief. **`4` = English name and `5` = Arabic name is a guess**; the brief only says "5 and 4 for the names".
- Sign-in is a **development-only** endpoint (`POST /api/Auth/DevLogin`) that issues a JWT for a user id and company code without a password. It returns 404 outside Development.
- Targets .NET 10.

## Run locally

Prerequisites: .NET 10 SDK, and an Oracle database for the screens that read or write data.

Secrets are kept in .NET user-secrets (outside the repository), never in `appsettings.json`:

```
cd AxiomSysSL
dotnet user-secrets init
dotnet user-secrets set "Jwt:Key" "<a random string of at least 32 characters>"
dotnet user-secrets set "ConnectionStrings:SupplyChain" "User Id=<user>;Password=<password>;Data Source=localhost:1521/FREEPDB1"
```

Start both apps (two terminals):

```
dotnet run --project AxiomSysSL --launch-profile http      # http://localhost:5197/swagger
dotnet run --project AxiomSysPortal --launch-profile http  # http://localhost:5152/en
```
