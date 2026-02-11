export interface CityDto {
  id: number;
  name: string;
  latitude: number;
  longitude: number;
  country: string;
  state: string;
}

export interface SolarDto {
  id: number;
  sunrise: string;
  sunset: string;
  date: string;
  cityId: number;
}

export interface UserDto {
  email: string;
  password: string;
}
