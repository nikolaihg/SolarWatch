import type { UserDto } from "../types";
import { post } from "./http";

export const register = (userData: UserDto) => {
  return post<{ email: string }>('/auth/register', userData);
};

export const login = (credentials: UserDto) => {
  return post<{ token: string; email: string }>('/auth/login', credentials);
};