# AUDIT – oop-advanced-audit

Granskning av startprojektet (.NET 10 API + React) som "ny kollega", 2026-11-11.

## Steg 1: Starta från noll
- README anger versioner (.NET 10, Node 22), portar och kommandon – projektet gick att starta
- `dotnet format` och `dotnet test` fungerar inte från roten, man måste veta att man ska stå i `backend/`
- README nämner inte hur man ser API:t: det finns ingen Swagger UI, bara `/openapi/v1.json`

## Snabbkoll
| Kontroll | Resultat | Kommentar |
|---|---|---|
| dotnet build | ✅ | |
| dotnet test | ✅ 7/7 | Mycket EF Core-loggar i utskriften |
| dotnet format --verify-no-changes (i backend/) | ✅ | |
| dotnet format / test från roten | ❌ | Ingen solution i roten |
| npm run build | ✅ | |
| npm test | ✅ 2/2 | Bara TodoList testas |
| prettier --check (i frontend/) | ✅ | |
| npx prettier --check . från roten | ❌ | README.md följer inte Prettier |
| oxlint | ✅ | 0 varningar |
| bin/obj/node_modules i git | ✅ | |
| Secrets i historiken | ✅ | Sökte på Password, ConnectionStrings, ApiKey, Secret |
| git log | – | 1 commit ("Initial commit" från template) |

## Checklista (0 = saknas, 1 = finns lite, 2 = sitter bra)
| Område | Punkt | Poäng | Kommentar |
|---|---|---|---|
| Struktur | Tunna controllers, logik i services | 2 | Controllern översätter bara OperationResult till HTTP |
| Struktur | DTOs används | 2 | CreateTodoRequest/TodoResponse, entiteten läcker inte |
| Struktur | Frontend går att hitta i | 2 | features/todos med api/, components/, types |
| Kvalitet | Tester, ett kommando | 1 | Finns, men frontend testar bara TodoList, inte TodosPage |
| Kvalitet | Servern validerar själv | 2 | Tom titel och maxlängd kontrolleras i TodoService |
| Kvalitet | Vettiga felmeddelanden | 1 | API:t svarar med ProblemDetails, men frontend visar inget när POST misslyckas. Felen är inte fältvisa |
| Kvalitet | Loading / error / empty i React | 1 | Alla tre finns, men inget "försök igen" och knappen spärras inte under sparning |
| Repo | README räcker för att starta | 2 | |
| Repo | bin/obj/node_modules utanför git | 2 | |
| Repo | Inga secrets i kod/historik | 2 | SQLite-sökväg är ingen hemlighet |
| Repo | Commit-historik | – | Går inte att bedöma, bara template-commit |
| Repo | Samma formatering överallt | 1 | Koden ok, men README.md och roten saknar Prettier |
| **Totalt** | | **18 / 22** | Commit-historik räknas inte |

## Övriga fynd
- `EnsureCreated()` i stället för EF Core migrations (kommentaren i Program.cs säger det själv)
- Frontend validerar bara tom titel, inte maxlängd
- `PUT /api/todos/{id}/done` finns men används inte i UI:t, och DELETE saknas

## De 5 viktigaste förbättringarna
1. **CI saknas** – GitHub Actions ska bygga, testa och kolla formatering på varje PR
2. **Fel vid skapande syns inte** – `createTodo` saknar try/catch, användaren får ingen feedback vid 400 eller nätverksfel
3. **Error-läget saknar "försök igen"** och knappen spärras inte under sparning (dubbla todos)
4. **Frontend-tester för TodosPage** – loading, error och misslyckad POST testas inte
5. **Migrations i stället för EnsureCreated** – schemaändringar når inte en befintlig databas

## AI-granskning
- Höll med om: att felhanteringen vid skapande saknas, jag testade och såg felet i konsolen
- Avfärdade: att DELETE saknas är ett kvalitetsproblem, det är en saknad funktion, inte dålig kvalitet