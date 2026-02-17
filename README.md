# Solar watch project
- monorepo
- backend/ # .NET REST API
- frontend/ # react vite frontend

## Secrets
1. `dotnet user-secrets init`
2. `dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=solarwatch;Username=USERNAME;Password=PASSWORD"`
3. `dotnet user-secrets set "OpenWeather:ApiKey" "YOURAPIKEY"`
4. `dotnet user-secrets set "Jwt:SigningKey" "CHANGE_ME_TO_A_LONG_RANDOM_SECRET_32CHARS_MIN`


## Docker 
- backend
    - NET dockerfile
- frontend 
    - node dockerfile

### Backend
`docker build -f backend/Dockerfile -t solarwatch-api .`
`docker run --env-file .env.docker -p 8080:8080 solarwatch-api`

### Frontend