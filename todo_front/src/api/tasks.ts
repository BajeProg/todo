import type { TaskInput, TaskItem } from '../domain/todo';
import { apiRequest } from '../lib/api-client';

export const tasksApi = {
  getAll: () => apiRequest<TaskItem[]>('/task-items'),
  create: (input: TaskInput) => apiRequest<TaskItem>('/task-items', {
    method: 'POST',
    body: JSON.stringify(input),
  }),
  update: (id: string, input: TaskInput) => apiRequest<TaskItem>(`/task-items/${encodeURIComponent(id)}`, {
    method: 'PUT',
    body: JSON.stringify(input),
  }),
  remove: (id: string) => apiRequest<void>(`/task-items/${encodeURIComponent(id)}`, {
    method: 'DELETE',
  }),
};
