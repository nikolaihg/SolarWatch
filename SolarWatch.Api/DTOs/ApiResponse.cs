namespace SolarWatch.Api.DTOs;

public record ApiResponse<T>(
    T Results,
    string Status
    );