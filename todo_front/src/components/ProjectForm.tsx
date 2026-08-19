import { type FormEvent, useState } from 'react';
import type { Project, ProjectInput } from '../domain/todo';

interface ProjectFormProps {
  project?: Project;
  isSaving: boolean;
  error?: string;
  onSubmit: (input: ProjectInput) => void;
  onCancel: () => void;
}

export function ProjectForm({ project, isSaving, error, onSubmit, onCancel }: ProjectFormProps) {
  const [name, setName] = useState(project?.name ?? '');
  const [description, setDescription] = useState(project?.description ?? '');
  const normalizedName = name.trim();

  const handleSubmit = (event: FormEvent) => {
    event.preventDefault();
    if (!normalizedName) return;
    onSubmit({ name: normalizedName, description: description.trim() || null });
  };

  return (
    <form className="form" onSubmit={handleSubmit}>
      <label className="field">
        <span>Название</span>
        <input autoFocus maxLength={120} value={name} onChange={(event) => setName(event.target.value)} placeholder="Например, Запуск продукта" required />
      </label>
      <label className="field">
        <span>Описание <small>необязательно</small></span>
        <textarea maxLength={500} rows={4} value={description} onChange={(event) => setDescription(event.target.value)} placeholder="Коротко о цели проекта" />
      </label>
      {error && <p className="form-error" role="alert">{error}</p>}
      <div className="form__actions">
        <button className="button button--ghost" type="button" onClick={onCancel}>Отмена</button>
        <button className="button button--primary" type="submit" disabled={!normalizedName || isSaving}>
          {isSaving ? 'Сохраняем…' : project ? 'Сохранить' : 'Создать проект'}
        </button>
      </div>
    </form>
  );
}
