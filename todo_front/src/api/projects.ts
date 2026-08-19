import type { Project, ProjectInput } from '../domain/todo';
import { apiRequest } from '../lib/api-client';

export const projectsApi = {
  getAll: () => apiRequest<Project[]>('/projects'),
  create: (input: ProjectInput) => apiRequest<Project>('/projects', {
    method: 'POST',
    body: JSON.stringify(input),
  }),
  update: (id: string, input: ProjectInput) => apiRequest<Project>(`/projects/${encodeURIComponent(id)}`, {
    method: 'PUT',
    body: JSON.stringify(input),
  }),
  remove: (id: string) => apiRequest<void>(`/projects/${encodeURIComponent(id)}`, {
    method: 'DELETE',
  }),
};
