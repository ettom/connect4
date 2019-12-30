.PHONY : build run webapp test dbclean clean

build:
	dotnet build
run:
	dotnet run --project Game
webapp:
	dotnet run --project WebApp
test:
	dotnet test
dbclean:
	rm -f data.db
	dotnet ef migrations remove --project DAL --startup-project Game --force -v
	dotnet ef migrations add InitialDbCreation --project DAL --startup-project Game -v
	dotnet ef database update --project DAL --startup-project Game --context AppDbContext -v
clean:
	dotnet clean
