import type { UserDto } from "../types";
import { post } from "./http";

const TOKEN_KEY = "token";

export const register = async (userData: UserDto) => {
    const result = await post<{token: string; email: string }>('/auth/register', userData);
    localStorage.setItem(TOKEN_KEY, result.token);
    return result;
};

export const login = async (credentials: UserDto) => {
    const result = await post<{ token: string; email: string }>('/auth/login', credentials);
    localStorage.setItem(TOKEN_KEY, result.token);
    return result;
};

export const logout = async () => {
    localStorage.removeItem(TOKEN_KEY);
    console.log("User logged out!");
};

export const getToken = () => localStorage.getItem(TOKEN_KEY);

export const isAuthenticated = () => !!getToken();