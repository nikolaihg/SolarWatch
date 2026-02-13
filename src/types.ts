export interface CityDto {
  id: number;
  name: string;
  latitude: number;
  longitude: number;
  country: string;
  state: string;
}

export interface SolarDto {
  sunrise: string;
  sunset: string;
}


export interface UserDto {
  email: string;
  password: string;
}
