import { CalendarDays, MoreHorizontal, Pencil, Trash2 } from 'lucide-react';
import { useState } from 'react';
import type { TaskItem } from '../domain/todo';
import { formatDate, isOverdue } from '../lib/date';

interface TaskCardProps {
  task: TaskItem;
  onEdit: (task: TaskItem) => void;
  onDelete: (task: TaskItem) => void;
}

export function TaskCard({ task, onEdit, onDelete }: TaskCardProps) {
  const [menuOpen, setMenuOpen] = useState(false);
  const overdue = task.deadline ? isOverdue(task.deadline) : false;

  return (
    <article className="task-card">
      <div className="task-card__topline">
        <span className="project-tag">{task.project.name || 'Без названия'}</span>
        <div className="menu-wrap">
          <button className="icon-button icon-button--small" type="button" aria-label={`Действия с задачей ${task.name ?? ''}`} aria-expanded={menuOpen} onClick={() => setMenuOpen((value) => !value)}>
            <MoreHorizontal size={19} />
          </button>
          {menuOpen && (
            <div className="menu">
              <button type="button" onClick={() => { setMenuOpen(false); onEdit(task); }}><Pencil size={15} /> Редактировать</button>
              <button className="menu__danger" type="button" onClick={() => { setMenuOpen(false); onDelete(task); }}><Trash2 size={15} /> Удалить</button>
            </div>
          )}
        </div>
      </div>
      <div className="task-card__content">
        <h3>{task.name || 'Без названия'}</h3>
        {task.description && <p>{task.description}</p>}
      </div>
      <footer className="task-card__meta">
        {task.deadline ? (
          <span className={overdue ? 'deadline deadline--overdue' : 'deadline'}>
            <CalendarDays size={15} /> {overdue ? 'Просрочено · ' : ''}{formatDate(task.deadline)}
          </span>
        ) : <span className="muted">Без дедлайна</span>}
        {task.storyPoints !== null && <span className="points" title="Story points">{task.storyPoints} SP</span>}
      </footer>
    </article>
  );
}
