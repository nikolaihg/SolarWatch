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

# solar watch

## Endpoint Design
- base url: `api/solarwatch`
- GET `api/solarwatch?cites='{name:string}&date={YYYY-MM-DD}`
- Example: `api/solarwatch?cities=bergen&2026-01-20*
- Returns:
```json
{
  "sunrise":"7:27:02 AM",
  "sunset":"5:05:55 PM"
}
```

## Running with Docker

Build the image:
```bash
docker build -f backend/Dockerfile -t solarwatch-backend .
```

Run the container (replace secrets with actual values):
```bash
docker run -p 8080:8080 \
  -e "Jwt:SigningKey=YOUR_SECRET_KEY" \
  -e "OpenWeather:ApiKey=YOUR_OPENWEATHER_API_KEY" \
  solarwatch-backend
```

### Docker compose 
1. 
```bash
cp .env.docker.example. env.docker
```
2. Change to your secrets

3. 
```bash
docker-compose --env-file .env.docker up --build
```

### Frontend

