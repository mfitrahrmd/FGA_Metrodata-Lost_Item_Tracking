migrate-add:
	dotnet dotnet-ef migrations add $(name) --project Persistence --startup-project RESTAPI --output-dir Migrations/
migrate-up:
	dotnet dotnet-ef database update --project Persistence --startup-project RESTAPI