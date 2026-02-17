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

## Testing workflow run