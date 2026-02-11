const API_URL = '/api';

export async function get<T>(endpoint: string): Promise<T> {
  const res = await fetch(`${API_URL}${endpoint}`, {
    headers: { 'Accept': 'application/json' },
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
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
    },
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
