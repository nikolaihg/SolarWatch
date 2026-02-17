import { getToken } from "./auth";

const API_URL = '/api';

function getAuthHeaders(): HeadersInit {
  const token = getToken();

  return {
    "Content-Type": "application/json",
    Accept: "application/json",
    ...(token && { Authorization: `Bearer ${token}` }),
  };
}

export async function get<T>(endpoint: string): Promise<T> {
  const res = await fetch(`${API_URL}${endpoint}`, {
    headers: getAuthHeaders(),
  });

  if (!res.ok) {
    throw new Error(res.statusText);
  }

  const data = await res.json();
  if (data.error) {
    throw new Error(data.error);
  }

  return data;
}

export async function post<T>(endpoint: string, body: unknown): Promise<T> {
  const res = await fetch(`${API_URL}${endpoint}`, {
    method: 'POST',
    headers: getAuthHeaders(),
    body: JSON.stringify(body),
  });

  if (!res.ok) {
    throw new Error(res.statusText);
  }

  const data = await res.json();
  if (data.error) {
    throw new Error(data.error);
  }

  return data;
}
