import type { SolarDto } from "../types";
import { get } from "./http";

export const getSolarWatch = (city: string, date?: string) => {
  const params = new URLSearchParams({ city });
  if (date) {
    params.append('date', date);
  }
  return get<SolarDto>(`/solarwatch?${params.toString()}`);
};


