import type { UserDto } from "../types";
import { post } from "./http";

export const register = async (userData: UserDto) => {
    return post<{ email: string }>('/auth/register', userData);
};

export const login = async (credentials: UserDto) => {
    return post<{ token: string; email: string }>('/auth/login', credentials);
};

export const logout = async () => {
    localStorage.clear();
    console.log("User logged out!");
};