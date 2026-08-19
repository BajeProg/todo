import { type FormEvent, useState } from 'react';
import type { Project, TaskInput, TaskItem } from '../domain/todo';
import { toApiDeadline, toDateInput } from '../lib/date';

interface TaskFormProps {
  task?: TaskItem;
  projects: Project[];
  initialProjectId?: string;
  isSaving: boolean;
  error?: string;
  onSubmit: (input: TaskInput) => void;
  onCancel: () => void;
}

export function TaskForm({ task, projects, initialProjectId, isSaving, error, onSubmit, onCancel }: TaskFormProps) {
  const [name, setName] = useState(task?.name ?? '');
  const [description, setDescription] = useState(task?.description ?? '');
  const [projectId, setProjectId] = useState(task?.project.id ?? initialProjectId ?? projects[0]?.id ?? '');
  const [storyPoints, setStoryPoints] = useState(task?.storyPoints?.toString() ?? '');
  const [deadline, setDeadline] = useState(toDateInput(task?.deadline ?? null));
  const normalizedName = name.trim();

  const handleSubmit = (event: FormEvent) => {
    event.preventDefault();
    if (!normalizedName || !projectId) return;
    onSubmit({
      name: normalizedName,
      description: description.trim() || null,
      projectId,
      storyPoints: storyPoints === '' ? null : Number(storyPoints),
      deadline: toApiDeadline(deadline),
    });
  };

  return (
    <form className="form" onSubmit={handleSubmit}>
      <label className="field">
        <span>Задача</span>
        <input autoFocus maxLength={160} value={name} onChange={(event) => setName(event.target.value)} placeholder="Что нужно сделать?" required />
      </label>
      <label className="field">
        <span>Описание <small>необязательно</small></span>
        <textarea maxLength={1000} rows={3} value={description} onChange={(event) => setDescription(event.target.value)} placeholder="Добавьте детали и контекст" />
      </label>
      <div className="form__grid">
        <label className="field">
          <span>Проект</span>
          <select value={projectId} onChange={(event) => setProjectId(event.target.value)} required>
            {projects.map((project) => <option key={project.id} value={project.id}>{project.name || 'Без названия'}</option>)}
          </select>
        </label>
        <label className="field">
          <span>Оценка</span>
          <input type="number" min="0" max="1000" step="1" inputMode="numeric" value={storyPoints} onChange={(event) => setStoryPoints(event.target.value)} placeholder="Story points" />
        </label>
      </div>
      <label className="field">
        <span>Дедлайн <small>необязательно</small></span>
        <input type="date" value={deadline} onChange={(event) => setDeadline(event.target.value)} />
      </label>
      {error && <p className="form-error" role="alert">{error}</p>}
      <div className="form__actions">
        <button className="button button--ghost" type="button" onClick={onCancel}>Отмена</button>
        <button className="button button--primary" type="submit" disabled={!normalizedName || !projectId || isSaving}>
          {isSaving ? 'Сохраняем…' : task ? 'Сохранить' : 'Добавить задачу'}
        </button>
      </div>
    </form>
  );
}
